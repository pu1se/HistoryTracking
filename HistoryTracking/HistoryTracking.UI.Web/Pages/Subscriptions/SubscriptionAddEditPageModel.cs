using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HistoryTracking.BL.Services.SubscriptionProducts.Models;
using HistoryTracking.DAL.Enums;

namespace HistoryTracking.UI.Web.Pages.Subscriptions
{
    public class SubscriptionAddEditPageModel : BasePageModel
    {
        public bool IsAddingNewOne { get; set; }
        public AddEditSubscriptionModel SubscriptionConfig { get; set; }
        public List<CurrencyType> CurrencyList { get; set; }
    }
}
