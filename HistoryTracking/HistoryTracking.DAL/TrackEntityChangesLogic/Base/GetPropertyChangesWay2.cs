using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using HistoryTracking.DAL.Entities;
using HistoryTracking.DAL.TrackEntityChangesLogic.PropertiesTrackingConfigurations;

namespace HistoryTracking.DAL.TrackEntityChangesLogic
{
    public static class GetPropertyChangesWay2
    {
        public static List<PropertyChangeDescription> For<T>(T oldEntity, T newEntity, TrackingEntityInfo propertyConfigs) where T: class
        {
            var changeList = new List<PropertyChangeDescription>();

            var allEntityProperties = propertyConfigs.EntityType.GetProperties();
            var simplePropertyNames = propertyConfigs.PropertyList.Where(x => !x.IsComplex).Select(x => x.Name).ToList();
            var complexPropertyNames = propertyConfigs.PropertyList.Where(x => x.IsComplex).Select(x => x.Name).ToList();

            foreach (var propertyOfEntity in allEntityProperties)
            {
                var propertyConfig = propertyConfigs.PropertyList.FirstOrDefault(x => x.Name == propertyOfEntity.Name);
                if (propertyConfig == null)
                {
                    continue;
                }

                if (propertyConfig.IsComplex)
                {
                    var complexChanges = HandelComplexProperties(propertyConfig, oldEntity, newEntity, propertyOfEntity);
                    changeList.AddRange(complexChanges);
                }
                else
                {
                    var change = HandelSimpleProperties(oldEntity, newEntity, propertyOfEntity);
                    if (change != null)
                    {
                        changeList.Add(change);
                    }
                }
            }

            return changeList;
        }

        private static PropertyChangeDescription HandelSimpleProperties<T>(
            T oldEntity, 
            T newEntity, 
            PropertyInfo propertyOfEntity) where T : class
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

            // todo: move displaying of value here
            return new PropertyChangeDescription
            {
                PropertyName = propertyOfEntity.Name,
                OldValue = oldValue?.ToString(),
                NewValue = newValue?.ToString(),
            };
        }

        private static List<PropertyChangeDescription> HandelComplexProperties<T>(
            TrackingPropertyInfo complexPropertyConfig, 
            T oldParentEntityList,
            T newParentEntityList,
            PropertyInfo propertyOfParentEntityList) where T : class
        {
            var changeList = new List<PropertyChangeDescription>();
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
                    var childConfig = complexPropertyConfig.SubProperties.FirstOrDefault(x => x.Name == childProperty.Name);
                    if (childConfig == null)
                    {
                        continue;
                    }

                    var change = HandelSimpleProperties(oldChildEntity, newChildEntity, childProperty);
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
