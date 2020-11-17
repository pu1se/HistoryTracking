using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HistoryTracking.DAL.Enums;

namespace HistoryTracking.BL.Services.Order.Models
{
    public class GetOrderModel
    {
        public Guid Id { get; set; }
        public OrderStatusType OrderStatus { get; set; }
        public PaymentStatusType PaymentStatus { get; set; }
        public Guid CustomerUserId { get; set; }
        public string CustomerUserName { get; set; }
    }
}
