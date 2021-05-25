using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HistoryTracking.BL.Services.Order.Models;

namespace HistoryTracking.UI.Web.Pages.Orders
{
    public class OrderListPageModel : BasePageModel
    {
        public List<OrderModel> OrderList { get; set; } = new List<OrderModel>();
    }
}
