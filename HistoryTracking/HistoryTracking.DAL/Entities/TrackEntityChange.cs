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

        public EntityType EntityType { get; set; }

        public string EntitySnapshotBeforeChangeAsJson { get; set; }

        public string EntitySnapshotAfterChangeAsJson { get; set; }

        public int TrackingPropertiesWasChanged { get; set; }
    }
}
