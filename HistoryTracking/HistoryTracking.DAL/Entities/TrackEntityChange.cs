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

        public string ChangeType { get; set; }

        [Index]
        public DateTime ChangeDateUtc { get; set; }

        // todo: find a way how to index this field
        public string EntityTable { get; set; }

        [Index]
        public Guid? EntityId { get; set; }

        public string EntityBeforeChangeSnapshot { get; set; }

        public string EntityAfterChangeSnapshot { get; set; }

        public string PropertiesChangesWay1 { get; set; }

        public TimeSpan TimeOfWay1 { get; set; }

        public string PropertiesChangesWay2 { get; set; }

        public TimeSpan TimeOfWay2 { get; set; }

        public string PropertiesChangesWay3 { get; set; }

        public TimeSpan TimeOfWay3 { get; set; }


        public Guid ChangedByUserId { get; set; }
        [ForeignKey("ChangedByUserId")]
        public UserEntity ChangedByUser { get; set; }
    }
}
