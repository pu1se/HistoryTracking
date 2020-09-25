﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HistoryTracking.DAL.Enums;

namespace HistoryTracking.DAL.Entities
{
    [Table("SubscriptionProducts")]
    public class SubscriptionProductEntity : BaseEntity
    {
        [Key]
        public Guid Id { get; set; }

        public string Title { get; set; }

        public decimal Price { get; set; }

        public decimal DistributorMarkupAsPercent { get; set; }

        public decimal ResellerMarkupAsPercent { get; set; }

        public CurrencyType Currency { get; set; }

        public virtual ICollection<OrderEntity> Orders { get; set; } = new List<OrderEntity>();

        public virtual ICollection<UserEntity> SubscriptionOwnerUsers { get; set; }
    }
}