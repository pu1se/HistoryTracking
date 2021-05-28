using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HistoryTracking.DAL.TrackChangesLogic.PropertiesTrackingConfigurations
{
    public class TrackEntityInfo
    {
        public string EntityName { get; }

        public List<TrackPropertyInfo> PropertyList { get; } = new List<TrackPropertyInfo>();

        public TrackEntityInfo(string entityName)
        {
            EntityName = entityName;
        }
    }
}
