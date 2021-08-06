using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HistoryTracking.DAL.TrackEntityChangesLogic.PropertiesTrackingConfigurations;

namespace HistoryTracking.DAL.TrackEntityChangesLogic.Base.Configuration
{
    public class DisplayRelatedEntityPropertiesConfig : ITrackedEntityConfig
    {
        public string EntityName { get; }
        public Type EntityType { get; }
        public List<TrackedPropertyConfig> PropertyList { get; } = new List<TrackedPropertyConfig>();

        public DisplayRelatedEntityPropertiesConfig(Type relatedEntityType)
        {
            EntityType = relatedEntityType;
            EntityName = EntityType.Name;
        }
    }
}
