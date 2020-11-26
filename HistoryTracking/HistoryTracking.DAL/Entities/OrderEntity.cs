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
    public class OrderEntity : BaseEntity
    {
        public string Comments { get; set; }

        public OrderStatusType OrderStatus { get; set; }

        public PaymentStatusType PaymentStatus { get; set; }

        public virtual ICollection<SubscriptionProductEntity> SubscriptionProducts { get; set; } = new List<SubscriptionProductEntity>();

        public Guid CustomerUserId { get; set; }
        [ForeignKey("CustomerUserId")]
        public UserEntity CustomerUser { get; set; }
    }
}
