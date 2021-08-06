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

                if (propertyConfig.IsComplex)
                {
                    var complexChanges = HandelComplexProperties(oldEntity, newEntity, propertyOfEntity, propertyConfig);
                    changeList.AddRange(complexChanges);
                }
                else
                {
                    var change = HandelSimpleProperties(oldEntity, newEntity, propertyOfEntity, propertyConfig);
                    if (change != null)
                    {
                        changeList.Add(change);
                    }
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
            if (propertyConfig.IsComplex)
            {
                propertyConfig = propertyConfig.SubProperties.First(x => x.Name == propertyName);
            }
            
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

        private static List<PropertyChange> HandelComplexProperties<T>(
            T oldParentEntityList,
            T newParentEntityList,
            PropertyInfo propertyOfParentEntityList,
            TrackedPropertyConfig propertyConfig) where T : class
        {
            var changeList = new List<PropertyChange>();
            IEnumerable<BaseEntity> oldChildEntityList = new List<BaseEntity>();
            if (oldParentEntityList != null)
            {
                oldChildEntityList = (IEnumerable<BaseEntity>) propertyOfParentEntityList.GetValue(oldParentEntityList);
                oldChildEntityList = oldChildEntityList ?? new List<BaseEntity>();
            }

            IEnumerable<BaseEntity> newChildEntityList = new List<BaseEntity>();
            if (newParentEntityList != null)
            {
                newChildEntityList = (IEnumerable<BaseEntity>) propertyOfParentEntityList.GetValue(newParentEntityList);
                newChildEntityList = newChildEntityList ?? new List<BaseEntity>();
            }


            foreach (var newChildEntity in newChildEntityList)
            {
                var oldChildEntity = oldChildEntityList.FirstOrDefault(x => x.Id == newChildEntity.Id);

                var allChildProperties = newChildEntity.GetType().GetProperties();
                foreach (var childProperty in allChildProperties)
                {
                    var childConfig = propertyConfig.SubProperties.FirstOrDefault(x => x.Name == childProperty.Name);
                    if (childConfig == null)
                    {
                        continue;
                    }

                    var change = HandelSimpleProperties(oldChildEntity, newChildEntity, childProperty, propertyConfig);
                    if (change != null)
                    {
                        changeList.Add(change);
                    }
                }
            }

            return changeList;
        }
    }
}
