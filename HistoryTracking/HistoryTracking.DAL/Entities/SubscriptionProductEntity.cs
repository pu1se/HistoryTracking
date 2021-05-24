using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HistoryTracking.DAL.Enums;
using Newtonsoft.Json;

namespace HistoryTracking.DAL.Entities
{
    [Table("SubscriptionProducts")]
    public class SubscriptionProductEntity : BaseEntity, ITrackEntityChanges
    {
        [TrackPropertyChanges]
        public string Title { get; set; }

        [TrackPropertyChanges]
        public decimal Price { get; set; }

        [TrackPropertyChanges]
        public decimal DistributorMarkupAsPercent { get; set; }

        [TrackPropertyChanges]
        public decimal ResellerMarkupAsPercent { get; set; }

        [TrackPropertyChanges]
        public CurrencyType Currency { get; set; }

        public virtual ICollection<OrderEntity> Orders { get; set; } = new List<OrderEntity>();

        public virtual ICollection<UserEntity> OwnerUsers { get; set; }
    }
}
