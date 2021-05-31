using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HistoryTracking.DAL.TrackEntityChangesLogic
{
    public static class GetPropertyChangesWay2
    {
        public static List<PropertyChangeDescription> For(DbEntityEntry dbEntry)
        {
            var propertyChanges = new List<PropertyChangeDescription>();
            switch (dbEntry.State)
            {
                case EntityState.Added:
                {
                    propertyChanges = GetChangesFor(null, dbEntry.Entity);
                    break;
                }
                case EntityState.Modified:
                {
                    var originalEntity = GetOriginalEntity(dbEntry.OriginalValues, dbEntry.Entity.GetType());
                    propertyChanges = GetChangesFor(originalEntity, dbEntry.Entity);
                    break;
                }
                case EntityState.Deleted:
                {
                    var originalEntity = GetOriginalEntity(dbEntry.OriginalValues, dbEntry.Entity.GetType());
                    propertyChanges = GetChangesFor(originalEntity, null);
                    break;
                }
            }

            return propertyChanges;
        }

        private static object GetOriginalEntity(DbPropertyValues originalValues, Type tEntity)
        {
            var originalEntity = Activator.CreateInstance(tEntity, true);
            foreach (var propertyName in originalValues.PropertyNames)
            {
                var property = tEntity.GetProperty(propertyName);
                var value = originalValues[propertyName];
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

        private static List<PropertyChangeDescription> GetChangesFor<T>(T oldEntity, T newEntity) where T: class
        {
            var changeList = new List<PropertyChangeDescription>();
            if (oldEntity == null || newEntity == null)
            {
                return changeList;
            }

            var propertyList = oldEntity.GetType().GetProperties();
            foreach (var property in propertyList)
            {
                /*if (!property.HasTrackChangesAttribute())
                {
                    continue;
                }*/

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

        /*public static List<ChangeItem> For<T>(T oldEntity, T newEntity) where T: class
        {
            var changeList = new List<ChangeItem>();
            if (oldEntity == null || newEntity == null)
            {
                return changeList;
            }

            var propertyList = oldEntity.GetType().GetProperties();
            foreach (var property in propertyList)
            {
                if (!property.HasTrackChangesAttribute())
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

                var change = new ChangeItem
                {
                    OldValue = property.GetValue(oldEntity)?.ToString(),
                    NewValue = property.GetValue(newEntity)?.ToString(),
                };
                change.OldValue = change.OldValue ?? string.Empty;
                change.NewValue = change.NewValue ?? string.Empty;
                changeList.Add(change);
            }

            return changeList;
        }*/
    }
}
