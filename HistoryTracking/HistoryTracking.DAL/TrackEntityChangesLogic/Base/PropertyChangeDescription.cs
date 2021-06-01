using System.Collections.Generic;
using HistoryTracking.DAL.Enums;

namespace HistoryTracking.DAL
{
    public class PropertyChangeDescription
    {
        public string PropertyName { get; set; }
        public string PropertyNameForDisplaying { get; set; }

        public string OldValue { get; set; }
        public string OldValueForDisplaying { get; set; }

        public string NewValue { get; set; }
        public string NewValueForDisplaying { get; set; }

        public List<UserType> IsVisibleForUserRoles { get; set; } = new List<UserType>();
    }
}
