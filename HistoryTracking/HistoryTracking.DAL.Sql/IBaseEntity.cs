using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HistoryTracking.DAL.Sql
{
    public interface IBaseEntity
    {
        Guid Id { get; set; }
        DateTime CreatedDateUtc { get; set; }
        DateTime UpdatedDateUtc { get; set; }
        Guid CreatedByUserId { get; set; }
        Guid UpdatedByUserId { get; set; }
    }
}
