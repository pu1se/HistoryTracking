using System;
using System.Collections.Generic;
using HistoryTracking.BL.Services.User;
using HistoryTracking.DAL;

namespace HistoryTracking.BL.Services.Changes.Models
{
    public class GetChangeModel
    {
        public DateTime ChangeDate { get; set; }

        public string ChangeType { get; set; }

        public GetUserModel ChangedByUser { get; set; }

        public string PropertyChangesAsJson { get; set; }

        public List<PropertyChangeDescription> PropertyChanges { get; set; } = new List<PropertyChangeDescription>();

        public string EntityName { get; set; }
    }
}
