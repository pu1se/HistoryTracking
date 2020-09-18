using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HistoryTracking.DAL.Entities
{
    public class BaseEntity
    {
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;
        public Guid CreatedByUserId { get; set; } = TestData.SystemUserId;
        public Guid UpdatedByUserId { get; set; } = TestData.SystemUserId;
    }
}
