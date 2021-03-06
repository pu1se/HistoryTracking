﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HistoryTracking.BL.Services.SubscriptionProducts.Models;

namespace HistoryTracking.UI.Web.Pages.Subscriptions
{
    public class SubscriptionListPageModel : BasePageModel
    {
        public List<SubscriptionModel> SubscriptionList { get; set; } = new List<SubscriptionModel>();
    }
}
