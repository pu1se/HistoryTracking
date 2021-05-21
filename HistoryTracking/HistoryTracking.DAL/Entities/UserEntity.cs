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
    public class UserEntity : BaseEntity, ITrackEntityChanges
    {
        [TrackPropertyChanges]
        public string Name { get; set; }

        [TrackPropertyChanges]
        public string Email { get; set; }

        [TrackPropertyChanges]
        public UserType UserType { get; set; }

        public virtual ICollection<SubscriptionProductEntity> SubscriptionProducts { get; set; }

        public virtual ICollection<OrderEntity> Orders { get; set; }

        public virtual ICollection<TrackEntityChange> TrackEntityChanges { get; set; }
    }
}
