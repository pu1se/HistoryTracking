using System;
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

        public async Task<List<TrackedEntityNameModel>> GetTrackingTableNamesAsync()
        {
            var trackingEntityNames = ConfigurationOfTrackedEntities.GetConfigList().Select(x => x.EntityName);

            return trackingEntityNames.Select(x => new TrackedEntityNameModel
            {
                EntityName = x,
                EntityNameForDisplaying = x.SplitByCaps()
            }).ToList();
        }

        public async Task<List<ChangeModel>> GetChangesAsync(GetChangesListModel query)
        {
            query = query ?? new GetChangesListModel();
            var getEntityChangesDbQuery = Storage.TrackEntityChanges.AsQueryable();

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

            var entityChanges = await getEntityChangesDbQuery
                .MapToChangeModel()
                .OrderByDescending(x => x.ChangeDate)
                .ToListAsync();

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

            var relatedEntityChanges = await Storage.TrackEntityChanges
                .Where(e => e.RelatedEntityId != null && allEntityIds.Contains(e.RelatedEntityId.Value))
                .MapToChangeModel()
                .OrderBy(x => x.EntityId)
                .ThenByDescending(x => x.ChangeDate)
                .ToListAsync();

            entityChanges = FillUpBeforeChange(entityChanges, allPossibleChangesWithEntities);
            relatedEntityChanges = FillUpBeforeChange(relatedEntityChanges, relatedEntityChanges);

            entityChanges.ForEach(change =>
            {
                var config = ConfigurationOfTrackedEntities.GetConfigFor(change.EntityName);
                if (config == null)
                {
                    return;
                }

                FillUpPropertyChanges(change, config);
                FillUpRelatedPropertyChanges(change, config, relatedEntityChanges);
                FilterByUserRole(query, change);
            });

            return entityChanges;
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

        private void FillUpRelatedPropertyChanges(ChangeModel change, TrackedEntityConfig config, List<ChangeModel> relatedEntityChanges)
        {
            var relatedChanges = relatedEntityChanges.Where(x => x.RelatedEntityId == change.EntityId).ToList();
            if (relatedChanges.Count == 0)
            {
                return;
            }

            relatedChanges = MergeRelatedChangesIntoChange(change, config, relatedChanges);
            GetSaparatedRelatedChanges();
        }

        private static List<ChangeModel> MergeRelatedChangesIntoChange(ChangeModel change, TrackedEntityConfig config, List<ChangeModel> relatedChanges)
        {
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

            return relatedChanges;
        }

        void GetSeparatedRelatedChanges()
        {

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
