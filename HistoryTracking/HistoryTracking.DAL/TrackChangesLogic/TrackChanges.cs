using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using HistoryTracking.DAL.Entities;

namespace HistoryTracking.DAL
{
    public static class TrackChanges
    {
        // todo: ppontus: use lymbda stead of attributes and interfaces to set properties and entities that will be tracking
        public static TrackEntityChange GetTrackEntityChange(DbEntityEntry dbEntry)
        {
            var supportedEntryState = new List<EntityState> {EntityState.Modified, EntityState.Added, EntityState.Deleted};
            if (!supportedEntryState.Contains(dbEntry.State))
            {
                return null;
            }

            var tableAttr = dbEntry.Entity.GetType().GetCustomAttributes(typeof(TableAttribute), true).SingleOrDefault() as TableAttribute;
            var entityTableName = tableAttr != null ? tableAttr.Name : dbEntry.Entity.GetType().Name;

            var keyName = dbEntry.Entity.GetType().GetProperties().FirstOrDefault(p => p.GetCustomAttributes(typeof(KeyAttribute), false).Count() > 0)?.Name;
            var entityId = keyName != null ? dbEntry.CurrentValues.GetValue<object>(keyName).ToString() : string.Empty;

            var trackEntityChange = new TrackEntityChange
            {
                Id = Guid.NewGuid(),
                EntityTable = entityTableName,
                EntityId = entityId,
                ChangeType = dbEntry.State.ToString(),
                ChangeDateUtc = DateTime.UtcNow,
                EntityAfterChangeSnapshot = dbEntry.Entity.ToJson(),
                PropertiesChanges = getPropertyChanges(dbEntry).ToJson(),
                ChangedByUserId = UserManager.GetCurrentUser()
            };

            if (dbEntry.State == EntityState.Modified)
            {
                trackEntityChange.EntityBeforeChangeSnapshot = GetOriginalEntity(dbEntry.OriginalValues, dbEntry.Entity.GetType()).ToJson();
            }

            return trackEntityChange;
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

        private static List<PropertyChangeDescription> getPropertyChanges(DbEntityEntry dbEntry)
        {
            var result = new List<PropertyChangeDescription>();

            switch (dbEntry.State)
            {
                case EntityState.Added:
                {
                    foreach (string propertyName in dbEntry.CurrentValues.PropertyNames)
                    {
                        result.Add(new PropertyChangeDescription
                        {
                            PropertyName = propertyName,
                            OldValue = null,
                            NewValue = dbEntry.CurrentValues.GetValue<object>(propertyName) == null ? null : dbEntry.CurrentValues.GetValue<object>(propertyName).ToString()
                        });
                    }

                    break;
                }
                case EntityState.Modified:
                {
                    foreach (string propertyName in dbEntry.CurrentValues.PropertyNames)
                    {
                        if (!object.Equals(dbEntry.OriginalValues.GetValue<object>(propertyName), dbEntry.CurrentValues.GetValue<object>(propertyName)))
                        {
                            result.Add(new PropertyChangeDescription
                            {
                                PropertyName = propertyName,
                                OldValue = dbEntry.OriginalValues.GetValue<object>(propertyName) == null ? null : dbEntry.OriginalValues.GetValue<object>(propertyName).ToString(),
                                NewValue = dbEntry.CurrentValues.GetValue<object>(propertyName) == null ? null : dbEntry.CurrentValues.GetValue<object>(propertyName).ToString()
                            });
                        }
                    }

                    break;
                }
                case EntityState.Deleted:
                {
                    foreach (string propertyName in dbEntry.OriginalValues.PropertyNames)
                    {
                        result.Add(new PropertyChangeDescription
                        {
                            PropertyName = propertyName,
                            OldValue = dbEntry.OriginalValues.GetValue<object>(propertyName) == null ? null : dbEntry.OriginalValues.GetValue<object>(propertyName).ToString(),
                            NewValue = null
                        });
                    }

                    break;
                }
            }

            return result;
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