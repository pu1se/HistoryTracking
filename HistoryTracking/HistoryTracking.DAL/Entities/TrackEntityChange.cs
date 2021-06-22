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

        public string EntityTable { get; set; }

        public string ChangeType { get; set; }

        [Index]
        public Guid EntityId { get; set; }

        [Index]
        public Guid? ParentEntityId { get; set; }

        public string EntityAfterChangeSnapshot { get; set; }

        [Index]
        public DateTime ChangeDateUtc { get; set; }


        public Guid ChangedByUserId { get; set; }
        [ForeignKey("ChangedByUserId")]
        public UserEntity ChangedByUser { get; set; }
    }
}
