using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HistoryTracking.DAL.TrackEntityChangesLogic.Base.Configuration;

namespace HistoryTracking.DAL.TrackEntityChangesLogic.PropertiesTrackingConfigurations
{
    // todo: maybe rename to DisplayChangesInEntity
    public class TrackedEntityConfig : ITrackedEntityConfig
    {
        public Type EntityType { get; }

        public string EntityName { get; }

        public List<TrackedPropertyConfig> PropertyList { get; } = new List<TrackedPropertyConfig>();

        public List<DisplayRelatedEntityPropertiesConfig> RelatedEntities { get; } = new List<DisplayRelatedEntityPropertiesConfig>();
        public string SaveRelatedEntityIdPropertyName { get; set; }

        public TrackedEntityConfig(string entityName, Type entityType)
        {
            EntityName = entityName;
            EntityType = entityType;
        }
    }
}
