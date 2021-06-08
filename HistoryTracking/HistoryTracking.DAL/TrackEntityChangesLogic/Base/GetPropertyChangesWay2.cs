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
            TrackingEntityInfo config)
        {
            var propertyChanges = new List<PropertyChangeDescription>();
            switch (dbEntry.State)
            {
                case EntityState.Added:
                {
                    propertyChanges = GetChangesFor(null, dbEntry.Entity, config);
                    break;
                }
                case EntityState.Modified:
                {
                    var originalEntity = GetOriginalEntity(dbEntry);
                    propertyChanges = GetChangesFor(originalEntity, dbEntry.Entity, config);
                    break;
                }
                case EntityState.Deleted:
                {
                    var originalEntity = GetOriginalEntity(dbEntry);
                    propertyChanges = GetChangesFor(originalEntity, null, config);
                    break;
                }
            }

            return propertyChanges;
        }

        public static object GetOriginalEntity(DbEntityEntry dbEntry)
        {
            var entityType = dbEntry.Entity.GetType();
            if (TrackingEntitiesConfiguration.GetConfigFor(entityType) == null)
            {
                entityType = entityType.BaseType;
            }

            var originalEntity = Activator.CreateInstance(entityType, true);
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

        public static List<PropertyChangeDescription> GetChangesFor<T>(T oldEntity, T newEntity, TrackingEntityInfo propertyConfig) where T: class
        {
            var changeList = new List<PropertyChangeDescription>();

            var propertyList = typeof(T).GetProperties();
            foreach (var property in propertyList)
            {
                if (propertyConfig.PropertyList.Select(x => x.Name).Contains(property.Name) == false)
                {
                    continue;
                }

                object oldValue = null;
                if (oldEntity != null)
                {
                    oldValue = property.GetValue(oldEntity);
                }

                object newValue = null;
                if (newEntity != null)
                {
                    newValue = property.GetValue(newEntity);
                }

                if (oldValue?.ToString() == newValue?.ToString())
                {
                    continue;
                }


                var change = new PropertyChangeDescription
                {
                    PropertyName = property.Name,
                    OldValue = oldValue?.ToString(),
                    NewValue = newValue?.ToString(),
                };
                changeList.Add(change);
            }

            return changeList;
        }
    }
}
