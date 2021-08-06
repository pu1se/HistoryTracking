using System;
using System.Collections.Generic;
using System.Data.Entity;
using HistoryTracking.BL.Services.User;
using HistoryTracking.DAL;

namespace HistoryTracking.BL.Services.Changes.Models
{
    public class ChangeModel
    {
        public DateTime ChangeDate { get; set; }

        public string ChangeType { get; set; }

        public UserModel ChangedByUser { get; set; }

        public List<PropertyChange> PropertyChanges { get; set; } = new List<PropertyChange>();

        // todo: rename to EntityType
        public string EntityName { get; set; }

        public string EntityNameForDisplaying { get; set; }

        public Guid Id { get; set; }

        public Guid EntityId { get; set; }
        public string EntityBeforeChangeAsJson { get; set; }
        public string EntityAfterChangeAsJson { get; set; }
        public Guid? ParentEntityId { get; set; }
        public Guid? RelatedEntityId { get; set; }
    }
}
