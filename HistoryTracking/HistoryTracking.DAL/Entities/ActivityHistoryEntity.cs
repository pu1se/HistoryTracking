using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HistoryTracking.DAL.Entities
{
    [Table("ActivityHistories")]
    public class ActivityHistoryEntity : BaseEntity
    {
        [Key]
        public Guid Id { get; set; }

        public string EntityName { get; set; }

        public string EntitySnapshotBeforeChangeAsJson { get; set; }

        public string EntitySnapshotAfterChangeAsJson { get; set; }

        public int PropertiesWasChanged { get; set; }
    }
}
