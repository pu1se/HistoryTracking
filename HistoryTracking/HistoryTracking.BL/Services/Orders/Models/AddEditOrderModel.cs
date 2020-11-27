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
    public class AddEditOrderModel
    {
        public Guid Id { get; set; }
        public OrderStatusType OrderStatus { get; set; }
        public Guid CustomerId { get; set; }
        public List<Guid> SubscriptionIdList { get; set; } = new List<Guid>();
    }
}
