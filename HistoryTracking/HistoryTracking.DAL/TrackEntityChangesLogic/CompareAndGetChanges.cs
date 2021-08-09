using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using HistoryTracking.DAL.Entities;
using HistoryTracking.DAL.TrackEntityChangesLogic.Base.Configuration;
using HistoryTracking.DAL.TrackEntityChangesLogic.PropertiesTrackingConfigurations;

namespace HistoryTracking.DAL.TrackEntityChangesLogic
{
    public static class CompareAndGetChanges
    {
        public static List<PropertyChange> For<T>(T oldEntity, T newEntity, ITrackedEntityConfig propertyConfigs) where T: class
        {
            var changeList = new List<PropertyChange>();

            var allEntityProperties = propertyConfigs.EntityType.GetProperties();

            foreach (var propertyOfEntity in allEntityProperties)
            {
                var propertyConfig = propertyConfigs.PropertyList.FirstOrDefault(x => x.Name == propertyOfEntity.Name);
                if (propertyConfig == null || propertyConfig.IsParentEntityId)
                {
                    continue;
                }

                var change = HandelSimpleProperties(oldEntity, newEntity, propertyOfEntity, propertyConfig);
                if (change != null)
                {
                    changeList.Add(change);
                }
            }

            return changeList;
        }

        private static PropertyChange HandelSimpleProperties<T>(T oldEntity,
            T newEntity,
            PropertyInfo propertyOfEntity, 
            TrackedPropertyConfig propertyConfig) where T : class
        {
            object oldValue = null;
            if (oldEntity != null)
            {
                oldValue = propertyOfEntity.GetValue(oldEntity);
            }

            object newValue = null;
            if (newEntity != null)
            {
                newValue = propertyOfEntity.GetValue(newEntity);
            }

            if (oldValue?.ToString() == newValue?.ToString())
            {
                return null;
            }

            var propertyName = propertyOfEntity.Name;
            return new PropertyChange
            {
                PropertyName = propertyName,
                PropertyNameForDisplaying = propertyName.SplitByCaps(),
                IsVisibleForUserRoles = propertyConfig.IsVisibleForUserRoles,
                OldValue = oldValue?.ToString(),
                OldValueForDisplaying = propertyConfig.DisplayingPropertyFunction(oldValue),
                NewValue = newValue?.ToString(),
                NewValueForDisplaying = propertyConfig.DisplayingPropertyFunction(newValue)
            };
        }
    }
}
