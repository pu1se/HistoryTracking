using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using HistoryTracking.DAL.TrackEntityChangesLogic.PropertiesTrackingConfigurations;

namespace HistoryTracking.DAL.TrackEntityChangesLogic.Base
{
    public class GetPropertyChangesWay1
    {
        public static List<PropertyChangeDescription> For(
            DbEntityEntry dbEntry,
            TrackingEntityInfo trackingEntityConfig)
        {
            var trackPropertiesWithName = trackingEntityConfig.PropertyList.Select(x => x.Name).ToList();
            var propertyChanges = new List<PropertyChangeDescription>();

            switch (dbEntry.State)
            {
                case EntityState.Added:
                {
                    propertyChanges = (
                        from propertyName in dbEntry.CurrentValues.PropertyNames 
                        where trackPropertiesWithName.Contains(propertyName) 
                            select new PropertyChangeDescription
                            {
                                PropertyName = propertyName, 
                                OldValue = null, 
                                NewValue = dbEntry.CurrentValues.GetValue<object>(propertyName) == null 
                                            ? null 
                                            : dbEntry.CurrentValues.GetValue<object>(propertyName).ToString()
                            })
                        .ToList();
                    break;
                }
                case EntityState.Modified:
                {
                    propertyChanges = (
                        from propertyName in dbEntry.CurrentValues.PropertyNames
                        where trackPropertiesWithName.Contains(propertyName)
                        where !object.Equals(dbEntry.OriginalValues.GetValue<object>(propertyName), dbEntry.CurrentValues.GetValue<object>(propertyName))
                            select new PropertyChangeDescription
                            {
                                PropertyName = propertyName,
                                OldValue = dbEntry.OriginalValues.GetValue<object>(propertyName) == null ? null : dbEntry.OriginalValues.GetValue<object>(propertyName).ToString(),
                                NewValue = dbEntry.CurrentValues.GetValue<object>(propertyName) == null ? null : dbEntry.CurrentValues.GetValue<object>(propertyName).ToString()
                            })
                        .ToList();
                    break;
                }
                case EntityState.Deleted:
                {
                    propertyChanges = (
                        from propertyName in dbEntry.OriginalValues.PropertyNames
                        where trackPropertiesWithName.Contains(propertyName)
                            select new PropertyChangeDescription
                            {
                                PropertyName = propertyName,
                                OldValue = dbEntry.OriginalValues.GetValue<object>(propertyName) == null
                                    ? null
                                    : dbEntry.OriginalValues.GetValue<object>(propertyName).ToString(),
                                NewValue = null
                            })
                        .ToList();
                    break;
                }
            }

            return propertyChanges.ToList();
        }
    }
}
