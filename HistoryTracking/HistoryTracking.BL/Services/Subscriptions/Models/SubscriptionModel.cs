﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HistoryTracking.DAL.Enums;

namespace HistoryTracking.BL.Services.SubscriptionProducts.Models
{
    public class SubscriptionModel
    {
        public Guid Id { get; set; }
        public decimal Price { get; set; }
        public CurrencyType Currency { get; set; }
        public string Title { get; set; }
        public decimal DistributorMarkupAsPercent { get; set; }
        public decimal ResellerMarkupAsPercent { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
