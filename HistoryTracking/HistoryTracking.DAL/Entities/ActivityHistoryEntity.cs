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
    public class ActivityHistoryEntity
    {
        [Key]
        public Guid Id { get; set; }

        public string EntitySnapshotAsJson { get; set; }

        public string EntityName { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime UpdatedDate { get; set; }
    }
}
