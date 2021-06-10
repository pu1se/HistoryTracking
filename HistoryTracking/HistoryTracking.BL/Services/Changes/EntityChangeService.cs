using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HistoryTracking.BL.Services.Changes.Models;
using HistoryTracking.BL.Services.User;
using HistoryTracking.DAL;
using HistoryTracking.DAL.Enums;
using HistoryTracking.DAL.TrackEntityChangesLogic;
using HistoryTracking.DAL.TrackEntityChangesLogic.PropertiesTrackingConfigurations;
using Newtonsoft.Json;

namespace HistoryTracking.BL.Services.Changes
{
    public class EntityChangeService : BaseService
    {
        public EntityChangeService(DataContext storage) : base(storage)
        {
        }

        public async Task<List<EntityNameModel>> GetTrackingTableNamesAsync()
        {
            var trackingEntityNames = TrackingEntitiesConfiguration.GetConfigList().Select(x => x.EntityName);

            return trackingEntityNames.Select(x => new EntityNameModel
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
                getEntityChangesDbQuery = getEntityChangesDbQuery.Where(e => e.EntityId == query.EntityId.Value);
            }

            var entityChanges = await getEntityChangesDbQuery

                .Select(e => new ChangeModel
                {
                    Id = e.Id,
                    ChangeDate = e.ChangeDateUtc,
                    ChangeType = e.ChangeType,
                    EntityBeforeChangeAsJson = e.EntityBeforeChangeSnapshot,
                    EntityAfterChangeAsJson = e.EntityAfterChangeSnapshot,
                    EntityName = e.EntityTable,
                    EntityId = e.EntityId,
                    ChangedByUser = new UserModel
                    {
                        Name = e.ChangedByUser.Name,
                        Email = e.ChangedByUser.Email,
                        UserType = e.ChangedByUser.UserType
                    }
                })
                .OrderByDescending(x => x.ChangeDate)
                .ToListAsync();

            entityChanges.ForEach(changeModel =>
            {
                var config = TrackingEntitiesConfiguration.GetConfigFor(changeModel.EntityName);
                if (config == null)
                {
                    return;
                }

                var entityBeforeChange = JsonConvert.DeserializeObject(changeModel.EntityBeforeChangeAsJson, config.EntityType);
                var entityAfterChange = JsonConvert.DeserializeObject(changeModel.EntityAfterChangeAsJson, config.EntityType);
                changeModel.PropertyChanges = GetPropertyChangesWay2.GetChangesFor(entityBeforeChange, entityAfterChange, config); 
                changeModel.EntityNameForDisplaying = changeModel.EntityName.SplitByCaps();

                
                foreach (var property in changeModel.PropertyChanges)
                {
                    var propertyConfig = config.PropertyList.FirstOrDefault(x => x.Name == property.PropertyName);
                    if (propertyConfig == null)
                    {
                        continue;
                    }

                    property.IsVisibleForUserRoles = propertyConfig.IsVisibleForUserRoles;
                    property.PropertyNameForDisplaying = property.PropertyName.SplitByCaps();
                    property.OldValueForDisplaying = propertyConfig.DisplayingPropertyFunction(property.OldValue);
                    property.NewValueForDisplaying = propertyConfig.DisplayingPropertyFunction(property.NewValue);
                }

                if (query.FilterByUserRole.HasValue)
                {
                    changeModel.PropertyChanges = changeModel.PropertyChanges
                        .Where(x => x.IsVisibleForUserRoles.Contains(query.FilterByUserRole.Value))
                        .ToList();
                }
            });

            return entityChanges;
        }

    }
}
