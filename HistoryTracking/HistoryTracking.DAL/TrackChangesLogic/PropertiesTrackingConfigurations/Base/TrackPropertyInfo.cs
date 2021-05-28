using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HistoryTracking.DAL.Enums;

namespace HistoryTracking.DAL.TrackChangesLogic.PropertiesTrackingConfigurations
{
    public class TrackPropertyInfo
    {
        public string Name { get; set; }
        public List<UserType> IsVisibleForUserRoles { get; set; }
    }
}
