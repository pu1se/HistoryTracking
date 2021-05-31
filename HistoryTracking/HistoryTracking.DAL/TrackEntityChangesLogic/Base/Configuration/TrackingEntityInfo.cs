using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HistoryTracking.DAL.TrackEntityChangesLogic.PropertiesTrackingConfigurations
{
    public class TrackingEntityInfo
    {
        public Type EntityType { get; }

        public string EntityName { get; }

        public List<TrackingPropertyInfo> PropertyList { get; } = new List<TrackingPropertyInfo>();

        public TrackingEntityInfo(string entityName, Type entityType)
        {
            EntityName = entityName;
            EntityType = entityType;
        }
    }
}
