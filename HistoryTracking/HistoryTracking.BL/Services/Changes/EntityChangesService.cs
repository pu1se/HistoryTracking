﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HistoryTracking.BL.Services.Changes.Models;
using HistoryTracking.BL.Services.User;
using HistoryTracking.DAL;
using HistoryTracking.DAL.Entities;
using HistoryTracking.DAL.Enums;
using HistoryTracking.DAL.TrackEntityChangesLogic;
using HistoryTracking.DAL.TrackEntityChangesLogic.PropertiesTrackingConfigurations;
using Newtonsoft.Json;

namespace HistoryTracking.BL.Services.Changes
{
    public class EntityChangesService : BaseService
    {
        public EntityChangesService(DataContext storage) : base(storage)
        {
        }

        public List<TrackedEntityNameModel> GetTrackingTableNames()
        {
            return ConfigurationOfTrackedEntities.GetConfigList().Where(x => x.ShowOnUiAsCategory).Select(x => new TrackedEntityNameModel
            {
                EntityName = x.EntityName,
                EntityNameForDisplaying = x.EntityName.SplitByCaps()
            }).ToList();
        }

        public async Task<List<ChangeModel>> GetChangesAsync(GetChangesListModel query)
        {
            query = query ?? new GetChangesListModel();

            var getEntityChangesDbQuery = Storage.TrackEntityChanges.AsQueryable();

            // 1. Filter changes.
            if (query.EntityNames.Any())
            {
                getEntityChangesDbQuery = getEntityChangesDbQuery.Where(e => query.EntityNames.Contains(e.EntityTable));
            }
            if (query.UserIds.Any())
            {
                getEntityChangesDbQuery = getEntityChangesDbQuery.Where(e => query.UserIds.Contains(e.ChangedByUserId));
            }
            if (query.TakeHistoryForLastNumberOfDays.HasValue)
            {
                var fromDate = DateTime.UtcNow.AddDays(-query.TakeHistoryForLastNumberOfDays.Value);
                getEntityChangesDbQuery = getEntityChangesDbQuery.Where(e => fromDate <= e.ChangeDateUtc);
            }

            if (query.EntityId.HasValue)
            {
                getEntityChangesDbQuery = getEntityChangesDbQuery
                    .Where(
                        e => 
                        e.EntityId == query.EntityId.Value || 
                        e.ParentId == query.EntityId.Value);
            }

            // 2. Get changes from DB.
            var entityChanges = await getEntityChangesDbQuery
                .MapToChangeModel()
                .OrderByDescending(x => x.ChangeDate)
                .ToListAsync();

            // 3. Get all possible changes for correct BeforeChange and AfterChange snapshot filling.
            // Fill up BeforeChange and AfterChange fields.
            var allPossibleChangesWithEntities = entityChanges.ToList();
            var allEntityIds = allPossibleChangesWithEntities
                .Select(x => x.EntityId).Distinct()
                .ToList();
            if (query.EntityId == null)
            {
                allPossibleChangesWithEntities = await Storage.TrackEntityChanges
                    .Where(e => allEntityIds.Contains(e.EntityId))
                    .MapToChangeModel()
                    .OrderBy(x => x.EntityId)
                    .ThenByDescending(x => x.ChangeDate)
                    .ToListAsync();
            }
            entityChanges = FillUpBeforeChange(entityChanges, allPossibleChangesWithEntities);

            // 4. Get all related changes
            var allRelatedEntityChanges = await Storage.TrackEntityChanges
                .Where(e => e.RelatedEntityId != null && allEntityIds.Contains(e.RelatedEntityId.Value))
                .MapToChangeModel()
                .OrderBy(x => x.EntityId)
                .ThenByDescending(x => x.ChangeDate)
                .ToListAsync();

            // 5. Filter related changes
            var filteredRelatedEntityChanges = allRelatedEntityChanges.ToList();
            if (query.UserIds.Any())
            {
                filteredRelatedEntityChanges = filteredRelatedEntityChanges.Where(e => query.UserIds.Contains(e.ChangedByUser.Id)).ToList();
            }
            if (query.TakeHistoryForLastNumberOfDays.HasValue)
            {
                var fromDate = DateTime.UtcNow.AddDays(-query.TakeHistoryForLastNumberOfDays.Value);
                filteredRelatedEntityChanges = filteredRelatedEntityChanges.Where(e => fromDate <= e.ChangeDate).ToList();
            }

            // 6. Fill up BeforeChange and AfterChange fields for Related Changes.
            filteredRelatedEntityChanges = FillUpBeforeChange(filteredRelatedEntityChanges, allRelatedEntityChanges);

            // 7. Create PropertyChange list.
            entityChanges.ForEach(change =>
            {
                var config = ConfigurationOfTrackedEntities.GetConfigFor(change.EntityName);
                if (config == null)
                {
                    return;
                }

                FillUpPropertyChanges(change, config);
                MergeRelatedPropertyChangesIntoChangeWhichWereDoneAtTheSameTime(change, config, filteredRelatedEntityChanges);
                FilterByUserRole(query, change);
            });

            // 8. Add related entities changes which were done at the separated time.
            var relatedEntitesChangesWhichWereDoneAtTheSeparatedTime = GetRelatedEntitiesChangesWhichWereDoneAtTheSeparatedTime(entityChanges, filteredRelatedEntityChanges);
            entityChanges.AddRange(relatedEntitesChangesWhichWereDoneAtTheSeparatedTime);

            return entityChanges.Where(x => x.PropertyChanges.Any()).OrderByDescending(x => x.ChangeDate).ToList();
        }

        List<ChangeModel> GetRelatedEntitiesChangesWhichWereDoneAtTheSeparatedTime(List<ChangeModel> entityChanges, List<ChangeModel> relatedEntityChanges)
        {
            var relatedChangesWhichWereDoneAtTheSeparatedTime = new List<ChangeModel>();
            var entityChangeDates = entityChanges.Select(x => x.ChangeDate).ToList();
            var relatedEntitiesThatCanBeAdded = relatedEntityChanges.Where(x => !entityChangeDates.Contains(x.ChangeDate) && x.RelatedEntityId.HasValue).ToList();
            var entityGroups = entityChanges.GroupBy(x => x.EntityName);

            foreach (var entityGroup in entityGroups)
            {
                var config = ConfigurationOfTrackedEntities.GetConfigFor(entityGroup.Key);
                if (config == null)
                {
                    continue;
                }

                foreach (var relatedChange in relatedEntitiesThatCanBeAdded)
                {
                    var relatedEntityConfig = config.RelatedEntities.FirstOrDefault(x => x.EntityName == relatedChange.EntityName);

                    if (relatedEntityConfig == null || relatedChange.RelatedEntityId == null)
                    {
                        continue;
                    }

                    var entityBeforeChange = JsonConvert.DeserializeObject(relatedChange.EntityBeforeChangeAsJson ?? string.Empty, relatedEntityConfig.EntityType);
                    var entityAfterChange = JsonConvert.DeserializeObject(relatedChange.EntityAfterChangeAsJson ?? string.Empty, relatedEntityConfig.EntityType);

                    var propertyChanges = CompareAndGetChanges.For(entityBeforeChange, entityAfterChange, relatedEntityConfig);
                    relatedChangesWhichWereDoneAtTheSeparatedTime.Add(new ChangeModel
                    {
                        Id = relatedChange.Id,
                        ChangeDate = relatedChange.ChangeDate,
                        ChangeType = relatedChange.ChangeType,
                        EntityAfterChangeAsJson = relatedChange.EntityAfterChangeAsJson,
                        EntityName = config.EntityName,
                        EntityNameForDisplaying = config.EntityName.SplitByCaps(),
                        EntityId = relatedChange.RelatedEntityId.Value,
                        ParentEntityId = null,
                        RelatedEntityId = relatedChange.EntityId,
                        ChangedByUser = new UserModel
                        {
                            Name = relatedChange.ChangedByUser.Name,
                            Email = relatedChange.ChangedByUser.Email,
                            UserType = relatedChange.ChangedByUser.UserType
                        },
                        PropertyChanges = propertyChanges
                    });
                }
            }

            return relatedChangesWhichWereDoneAtTheSeparatedTime;
        }

        private static List<ChangeModel> FillUpBeforeChange(List<ChangeModel> entityChanges, List<ChangeModel> allPossibleChangesWithEntities)
        {
            var entityChangesDictionary = entityChanges.ToDictionary(x => x.Id, x => x);
            for (int i = 0; i < allPossibleChangesWithEntities.Count; i++)
            {
                if (i + 1 == allPossibleChangesWithEntities.Count)
                {
                    continue;
                }

                var currentChange = allPossibleChangesWithEntities[i];
                var beforeChange = allPossibleChangesWithEntities[i + 1];

                if (currentChange.EntityId != beforeChange.EntityId)
                {
                    continue;
                }

                currentChange.EntityBeforeChangeAsJson = beforeChange.EntityAfterChangeAsJson;

                if (entityChangesDictionary.ContainsKey(currentChange.Id))
                {
                    entityChangesDictionary[currentChange.Id].EntityBeforeChangeAsJson = currentChange.EntityBeforeChangeAsJson;
                }
            }
            entityChanges = entityChangesDictionary.Select(x => x.Value).ToList();

            foreach (var change in entityChanges)
            {
                switch (change.ChangeType)
                {
                    case "Added":
                        if (change.EntityBeforeChangeAsJson.IsNullOrEmpty()
                            && !change.EntityAfterChangeAsJson.IsNullOrEmpty())
                        {

                        }
                        else
                        {
                            //todo: log this as error
                            Console.WriteLine("error");
                        }
                        break;

                    case "Modified":
                        if (!change.EntityBeforeChangeAsJson.IsNullOrEmpty()
                            && !change.EntityAfterChangeAsJson.IsNullOrEmpty())
                        {
                        }
                        else
                        {
                            // this is a possible case when feature is turning on, in a year after the feature is on it can be error
                            change.EntityBeforeChangeAsJson = change.EntityAfterChangeAsJson;
                        }
                        break;

                    case "Deleted":
                        if (!change.EntityBeforeChangeAsJson.IsNullOrEmpty()
                            && change.EntityAfterChangeAsJson.IsNullOrEmpty())
                        {

                        }
                        else
                        {
                            //todo: log this as error
                            Console.WriteLine("error");
                        }
                        break;
                }
            }

            return entityChanges;
        }

        private static void FillUpPropertyChanges(ChangeModel change, TrackedEntityConfig config)
        {
            var entityBeforeChange = JsonConvert.DeserializeObject(change.EntityBeforeChangeAsJson ?? string.Empty, config.EntityType);
            var entityAfterChange = JsonConvert.DeserializeObject(change.EntityAfterChangeAsJson ?? string.Empty, config.EntityType);
            change.PropertyChanges = CompareAndGetChanges.For(entityBeforeChange, entityAfterChange, config);
            change.EntityNameForDisplaying = change.EntityName.SplitByCaps();
        }

        private void MergeRelatedPropertyChangesIntoChangeWhichWereDoneAtTheSameTime(ChangeModel change, TrackedEntityConfig config, List<ChangeModel> relatedEntityChanges)
        {
            var relatedChanges = relatedEntityChanges.Where(x => x.RelatedEntityId == change.EntityId).ToList();
            if (relatedChanges.Count == 0)
            {
                return;
            }

            var relatedChangesGroups = relatedChanges.Where(x => x.ChangeDate == change.ChangeDate).GroupBy(x => x.EntityName);
            foreach (var group in relatedChangesGroups)
            {
                // there can be only one change in related entity in one ChangeDate
                var relatedChange = group.First();
                var relatedEntityConfig = config.RelatedEntities.FirstOrDefault(x => x.EntityName == group.Key);

                if (relatedEntityConfig == null)
                {
                    continue;
                }

                var entityBeforeChange = JsonConvert.DeserializeObject(relatedChange.EntityBeforeChangeAsJson ?? string.Empty, relatedEntityConfig.EntityType);
                var entityAfterChange = JsonConvert.DeserializeObject(relatedChange.EntityAfterChangeAsJson ?? string.Empty, relatedEntityConfig.EntityType);

                var propertyChanges = CompareAndGetChanges.For(entityBeforeChange, entityAfterChange, relatedEntityConfig);
                change.PropertyChanges.AddRange(propertyChanges);
            }
        }

        private static void FilterByUserRole(GetChangesListModel query, ChangeModel changeModel)
        {
            if (query.FilterByUserRole.HasValue)
            {
                changeModel.PropertyChanges = changeModel.PropertyChanges
                    .Where(x => x.IsVisibleForUserRoles.Contains(query.FilterByUserRole.Value))
                    .ToList();
            }
        }
    }

    public static class Extensions
    {
        public static IQueryable<ChangeModel> MapToChangeModel(this IQueryable<TrackedEntityChange> changeEntities)
        {
            return changeEntities.Select(e => new ChangeModel
            {
                Id = e.Id,
                ChangeDate = e.ChangeDateUtc,
                ChangeType = e.ChangeType,
                EntityAfterChangeAsJson = e.EntityAfterChangeSnapshot,
                EntityName = e.EntityTable,
                EntityId = e.EntityId,
                ParentEntityId = e.ParentId,
                RelatedEntityId = e.RelatedEntityId,
                ChangedByUser = new UserModel
                {
                    Name = e.ChangedByUser.Name,
                    Email = e.ChangedByUser.Email,
                    UserType = e.ChangedByUser.UserType
                }
            });
        }
    }
}
