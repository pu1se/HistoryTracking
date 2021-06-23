using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using HistoryTracking.DAL.Enums;

namespace HistoryTracking.DAL.TrackEntityChangesLogic.PropertiesTrackingConfigurations
{
    public class TrackedPropertyConfig
    {
        public string Name { get; set; }
        public List<UserType> IsVisibleForUserRoles { get; set; }
        public Type PropertyType { get; set; }
        public Func<object, string> DisplayingPropertyFunction { get; set; }
        public bool IsComplex => SubProperties.Any();
        public List<TrackedPropertyConfig> SubProperties { get; set; } = new List<TrackedPropertyConfig>();
        public bool IsParentEntityId { get; set; } = false;
    }
}
