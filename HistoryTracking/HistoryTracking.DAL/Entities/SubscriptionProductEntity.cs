﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public class SubscriptionProductEntity : BaseEntity
    {
        public string Title { get; set; }

        public decimal Price { get; set; }

        public decimal DistributorMarkupAsPercent { get; set; }

        public decimal ResellerMarkupAsPercent { get; set; }

        public CurrencyType Currency { get; set; }

        public virtual ICollection<OrderEntity> Orders { get; set; } = new List<OrderEntity>();

        public virtual ICollection<UserEntity> OwnerUsers { get; set; }
        
        public Guid? ParentId { get; set; }
        [ForeignKey("ParentId")]
        public SubscriptionProductEntity ParentSubscription { get; set; }

        public virtual ICollection<SubscriptionProductEntity> ChildrenSubscriptions { get; set; } = new List<SubscriptionProductEntity>();
    }
}
