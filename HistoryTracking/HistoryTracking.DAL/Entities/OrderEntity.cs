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
    [Table("Orders")]
    public class OrderEntity : IEntity
    {
        [Key]
        public Guid Id { get; set; }        

        public string Title { get; set; }

        public string Comments { get; set; }

        public OrderStatusType Status { get; set; }

        public virtual ICollection<OfferEntity> Offers { get; set; }


        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public Guid CreatedByUserId { get; set; }
        public Guid UpdatedByUserId { get; set; }
    }
}
