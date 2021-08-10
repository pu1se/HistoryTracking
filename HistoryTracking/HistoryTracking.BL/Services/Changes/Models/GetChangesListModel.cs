using System;
using System.Collections.Generic;
using HistoryTracking.DAL.Enums;

namespace HistoryTracking.BL.Services.Changes.Models
{
    public class GetChangesListModel
    {
        public List<string> EntityNames { get; set; } = new List<string>();
        public List<Guid> UserIds { get; set; } = new List<Guid>();
        public int? TakeHistoryForLastNumberOfDays { get; set; }
        public Guid? EntityId { get; set; }
        public UserType? FilterByUserRole { get; set; }
    }
}
