using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Entities
{
    public interface IEntity
    {
        Guid Id { get; set; }
        DateTime CreatedDate { get; set; }
        DateTime UpdatedDate { get; set; }
        Guid CreatedByUserId { get; set; }
        Guid UpdatedByUserId { get; set; }
    }
}
