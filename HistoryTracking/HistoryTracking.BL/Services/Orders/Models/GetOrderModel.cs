using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HistoryTracking.BL.Services.SubscriptionProducts.Models;
using HistoryTracking.BL.Services.User;
using HistoryTracking.DAL.Enums;

namespace HistoryTracking.BL.Services.Order.Models
{
    public class GetOrderModel
    {
        public Guid Id { get; set; }
        public OrderStatusType OrderStatus { get; set; }
        public GetUserModel CustomerUser { get; set; }
        public DateTime OrderDate { get; set; }
        public List<GetSubscriptionModel> SubscriptionList { get; set; } = new List<GetSubscriptionModel>();
    }
}
