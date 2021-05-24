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

        // todo: add index
        public DateTime ChangeDateUtc { get; set; }

        // todo: add index
        public string EntityTable { get; set; }

        // todo: add index
        public string EntityId { get; set; }

        public string EntityBeforeChangeSnapshot { get; set; }

        public string EntityAfterChangeSnapshot { get; set; }

        public string PropertiesChanges { get; set; }

        public string AltPropertiesChanges { get; set; }


        public Guid ChangedByUserId { get; set; }
        [ForeignKey("ChangedByUserId")]
        public UserEntity ChangedByUser { get; set; }
    }
}
