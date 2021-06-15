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

        // todo: use EntityState instead of string
        public string ChangeType { get; set; }

        [Index]
        public DateTime ChangeDateUtc { get; set; }

        public string EntityTable { get; set; }

        // todo: make it not nullable
        [Index]
        public Guid? EntityId { get; set; }

        // todo: delete this one
        public string EntityBeforeChangeSnapshot { get; set; }

        public string EntityAfterChangeSnapshot { get; set; }

        public string PropertiesChangesWay1 { get; set; }

        public double TimeOfWay1 { get; set; }

        public string PropertiesChangesWay2 { get; set; }

        public double TimeOfWay2 { get; set; }

        public double TimeOfGetOldEntity { get; set; }


        public Guid ChangedByUserId { get; set; }
        [ForeignKey("ChangedByUserId")]
        public UserEntity ChangedByUser { get; set; }
    }
}
