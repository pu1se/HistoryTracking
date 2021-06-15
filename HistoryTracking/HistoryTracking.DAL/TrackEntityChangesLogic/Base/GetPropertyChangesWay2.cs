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
        public static List<PropertyChangeDescription> For<T>(T oldEntity, T newEntity, TrackingEntityInfo propertyConfig) where T: class
        {
            var changeList = new List<PropertyChangeDescription>();

            var propertyList = propertyConfig.EntityType.GetProperties();
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
