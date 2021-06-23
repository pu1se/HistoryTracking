using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HistoryTracking.DAL.TrackEntityChangesLogic.PropertiesTrackingConfigurations
{
    public class TrackedEntityConfig
    {
        public Type EntityType { get; }

        public string EntityName { get; }

        public List<TrackedPropertyConfig> PropertyList { get; } = new List<TrackedPropertyConfig>();

        public TrackedEntityConfig(string entityName, Type entityType)
        {
            EntityName = entityName;
            EntityType = entityType;
        }
    }
}
