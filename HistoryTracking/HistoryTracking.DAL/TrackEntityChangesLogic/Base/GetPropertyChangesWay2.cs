using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HistoryTracking.DAL.TrackEntityChangesLogic.PropertiesTrackingConfigurations;

namespace HistoryTracking.DAL.TrackEntityChangesLogic
{
    public static class GetPropertyChangesWay2
    {
        public static List<PropertyChangeDescription> For(DbEntityEntry dbEntry,
            TrackingEntityInfo trackingEntityConfig)
        {
            var propertyChanges = new List<PropertyChangeDescription>();
            switch (dbEntry.State)
            {
                case EntityState.Added:
                {
                    propertyChanges = GetChangesFor(null, dbEntry.Entity, trackingEntityConfig);
                    break;
                }
                case EntityState.Modified:
                {
                    var originalEntity = GetOriginalEntity(dbEntry);
                    propertyChanges = GetChangesFor(originalEntity, dbEntry.Entity, trackingEntityConfig);
                    break;
                }
                case EntityState.Deleted:
                {
                    var originalEntity = GetOriginalEntity(dbEntry);
                    propertyChanges = GetChangesFor(originalEntity, null, trackingEntityConfig);
                    break;
                }
            }

            return propertyChanges;
        }

        //todo: calculate its executing time
        private static object GetOriginalEntity(DbEntityEntry dbEntry)
        {
            var entityType = dbEntry.Entity.GetType();
            var originalEntity = dbEntry.Entity.DeepClone();
            foreach (var propertyName in dbEntry.OriginalValues.PropertyNames)
            {
                var property = entityType.GetProperty(propertyName);
                var value = dbEntry.OriginalValues[propertyName];
                if (!(value is DbPropertyValues))
                {
                    property.SetValue(originalEntity, value);
                }
                /*else
                {
                    // nested entity
                    property.SetValue(originalEntity, GetOriginalEntity(value as DbPropertyValues, property.PropertyType));
                }*/
            }

            return originalEntity;
        }

        //todo: calculate its executing time
        private static List<PropertyChangeDescription> GetChangesFor<T>(T oldEntity, T newEntity, TrackingEntityInfo propertyConfig) where T: class
        {
            var changeList = new List<PropertyChangeDescription>();

            var propertyList = typeof(T).GetProperties();
            foreach (var property in propertyList)
            {
                if (propertyConfig.PropertyList.Select(x => x.Name).Contains(property.Name) == false)
                {
                    continue;
                }

                if (property.GetValue(oldEntity) == property.GetValue(newEntity))
                {
                    continue;
                }

                if (property.GetValue(oldEntity)?.ToString() == property.GetValue(newEntity)?.ToString())
                {
                    continue;
                }


                var change = new PropertyChangeDescription
                {
                    PropertyName = property.Name,
                    OldValue = property.GetValue(oldEntity)?.ToString(),
                    NewValue = property.GetValue(newEntity)?.ToString(),
                };
                change.OldValue = change.OldValue ?? string.Empty;
                change.NewValue = change.NewValue ?? string.Empty;
                changeList.Add(change);
            }

            return changeList;
        }
    }
}
