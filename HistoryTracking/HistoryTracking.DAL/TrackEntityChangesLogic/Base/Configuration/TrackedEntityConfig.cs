using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HistoryTracking.DAL.TrackEntityChangesLogic.Base.Configuration;

namespace HistoryTracking.DAL.TrackEntityChangesLogic.PropertiesTrackingConfigurations
{
    public class TrackedEntityConfig : ITrackedEntityConfig
    {
        public bool ShowOnUiAsCategory { get; }

        public Type EntityType { get; }

        public string EntityName { get; }

        public List<TrackedPropertyConfig> PropertyList { get; } = new List<TrackedPropertyConfig>();

        public List<RelatedEntityPropertiesConfig> RelatedEntities { get; } = new List<RelatedEntityPropertiesConfig>();
        public string SaveRelatedEntityIdPropertyName { get; set; }

        public TrackedEntityConfig(string entityName, Type entityType, bool showOnUiAsCategory)
        {
            EntityName = entityName;
            EntityType = entityType;
            ShowOnUiAsCategory = showOnUiAsCategory;
        }
    }
}
