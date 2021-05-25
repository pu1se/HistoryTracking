using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HistoryTracking.BL.Services.Order.Models;
using HistoryTracking.BL.Services.SubscriptionProducts.Models;
using HistoryTracking.BL.Services.User;
using HistoryTracking.DAL.Enums;

namespace HistoryTracking.UI.Web.Pages.Orders
{
    public class OrderAddEditPageModel : BasePageModel
    {
        public bool IsAddingNewOne { get; set; }
        public AddEditOrderModel OrderConfig { get; set; }
        public List<OrderStatusType> OrderStatusTypeList { get; set; }
        public List<UserModel> CustomerList { get; set; }
        public List<SubscriptionModel> SubscriptionList { get; set; }
    }
}
