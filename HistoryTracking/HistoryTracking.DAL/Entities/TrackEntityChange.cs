using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HistoryTracking.DAL.Enums;

namespace HistoryTracking.DAL.Entities
{
    [Table("TrackEntityChanges")]
    public class TrackEntityChange
    {
        [Key]
        public Guid Id { get; set; }

        public string EventType { get; set; }

        public DateTime EventDateUtc { get; set; }

        public string EntityTable { get; set; }

        public string EntityId { get; set; }

        public string OldValue { get; set; }

        public string NewValue { get; set; }

        public string TrackingPropertiesChanges { get; set; }

        public bool TrackingPropertiesWasChanged { get; set; }
    }
}
