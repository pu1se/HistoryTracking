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
    [TrackEntityChanges]
    public class UserEntity : BaseEntity, IHistoryTracking
    {
        [DisplayPropertyChanges]
        public string Name { get; set; }

        [DisplayPropertyChanges]
        public string Email { get; set; }

        [DisplayPropertyChanges]
        public UserType UserType { get; set; }

        public virtual ICollection<SubscriptionProductEntity> SubscriptionProducts { get; set; }

        public virtual ICollection<OrderEntity> Orders { get; set; }
    }
}
