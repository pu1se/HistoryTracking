using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HistoryTracking.DAL.TrackEntityChangesLogic.PropertiesTrackingConfigurations;

namespace HistoryTracking.DAL.TrackEntityChangesLogic.Base.Configuration
{
    public class RelatedEntityPropertiesConfig : ITrackedEntityConfig
    {
        public string EntityName { get; }
        public Type EntityType { get; }
        public List<TrackedPropertyConfig> PropertyList { get; } = new List<TrackedPropertyConfig>();

        public RelatedEntityPropertiesConfig(Type relatedEntityType)
        {
            EntityType = relatedEntityType;
            EntityName = EntityType.Name;
        }
    }
}
