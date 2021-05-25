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
    public class OrderModel
    {
        public Guid Id { get; set; }
        public OrderStatusType OrderStatus { get; set; }
        public UserModel CustomerUser { get; set; }
        public DateTime OrderDate { get; set; }
        public List<SubscriptionModel> SubscriptionList { get; set; } = new List<SubscriptionModel>();
    }
}
