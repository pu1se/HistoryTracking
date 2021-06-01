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
using HistoryTracking.DAL.TrackEntityChangesLogic.PropertiesTrackingConfigurations;
using Newtonsoft.Json;

namespace HistoryTracking.BL.Services.Changes
{
    public class EntityChangeService : BaseService
    {
        public EntityChangeService(DataContext storage) : base(storage)
        {
        }

        public async Task<List<EntityNameModel>> GetTrackingTableNames()
        {
            var trackingEntityNames = TrackingEntitiesConfiguration.GetConfigList().Select(x => x.EntityName);

            return trackingEntityNames.Select(x => new EntityNameModel
            {
                EntityName = x,
                EntityNameForDisplaying = x.SplitByCaps()
            }).ToList();
        }

        public async Task<List<ChangeModel>> GetChanges(GetChangesListModel query = null)
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
                    PropertyChangesAsJson = e.PropertiesChangesWay1,
                    EntityName = e.EntityTable,
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

                changeModel.PropertyChanges = JsonConvert.DeserializeObject<List<PropertyChangeDescription>>(changeModel.PropertyChangesAsJson) 
                                    ?? new List<PropertyChangeDescription>(); 
                changeModel.EntityNameForDisplaying = changeModel.EntityName.SplitByCaps();

                
                foreach (var property in changeModel.PropertyChanges)
                {
                    var propertyConfig = config.PropertyList.FirstOrDefault(x => x.Name == property.PropertyName);
                    if (propertyConfig == null)
                    {
                        continue;
                    }

                    property.PropertyNameForDisplaying = property.PropertyName.SplitByCaps();
                    property.IsVisibleForUserRoles = propertyConfig.IsVisibleForUserRoles;
                    property.OldValueForDisplaying = propertyConfig.DisplayingPropertyFunction(property.OldValue);
                    property.NewValueForDisplaying = propertyConfig.DisplayingPropertyFunction(property.NewValue);
                }
            });

            //todo: fill up ChangeModel with allowed user roles and filter by it.
            entityChanges = FilterChangesByCurrentUserRole(entityChanges, UserManager.GetCurrentUserType());

            return entityChanges;
        }

        private List<ChangeModel> FilterChangesByCurrentUserRole(List<ChangeModel> entityChanges, UserType currentUserRole)
        {
            var result = new List<ChangeModel>();
            foreach (var entityChange in entityChanges)
            {
                var configForEntity = TrackingEntitiesConfiguration.GetConfigFor(entityChange.EntityName);
                if (configForEntity == null)
                {
                    continue;
                }

                var visibleProperties = new List<PropertyChangeDescription>();
                foreach (var property in entityChange.PropertyChanges)
                {
                    var configForProperty = configForEntity.PropertyList.FirstOrDefault(x => x.Name == property.PropertyName);
                    if (configForProperty == null)
                    {
                        continue;
                    }

                    if (configForProperty.IsVisibleForUserRoles.Contains(currentUserRole))
                    {
                        visibleProperties.Add(property);
                    }
                }

                if (visibleProperties.Any())
                {
                    entityChange.PropertyChanges = visibleProperties;
                    result.Add(entityChange);
                }
            }

            return result;
        }
    }
}
