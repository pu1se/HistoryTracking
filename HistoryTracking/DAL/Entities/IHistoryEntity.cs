using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Entities
{
    public interface IHistoryEntity
    {
        Guid Id { get; set; }
        Guid ChangedEntityId { get; set; }
        string EntityBeforChangeAsJson { get; set; }
        string EntityAfterChangeAsJson { get; set; }
        Guid ChangedByUserId { get; set; }
        DateTime ChangedDate { get; set; }
    }
}
