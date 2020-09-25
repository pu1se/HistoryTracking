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
    [Table("Users")]
    public class UserEntity : BaseEntity
    {
        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public UserType UserType { get; set; }

        public virtual ICollection<SubscriptionProductEntity> SubscriptionProducts { get; set; }

        public virtual ICollection<OrderEntity> Orders { get; set; }
    }
}
