using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HistoryTracking.DAL.TrackEntityChangesLogic.PropertiesTrackingConfigurations;

namespace HistoryTracking.DAL.TrackEntityChangesLogic.Base.Configuration
{
    public interface ITrackedEntityConfig
    {
        Type EntityType { get; }
        List<DisplayPropertyConfig> PropertyList { get; }
    }
}
