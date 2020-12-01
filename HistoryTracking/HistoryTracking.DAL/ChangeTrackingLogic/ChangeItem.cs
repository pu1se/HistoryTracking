using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HistoryTracking.DAL.ChangeTrackingLogic
{
    public class ChangeItem
    {
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public bool WasAddedForFirstTime => OldValue == string.Empty;
    }
}
