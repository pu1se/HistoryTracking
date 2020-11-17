using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using HistoryTracking.BL.Services;
using HistoryTracking.BL.Services.Order.Models;
using HistoryTracking.BL.Services.User;

namespace HistoryTracking.Api.Controllers
{
    [Route("orders")]
    public class OrdersController : BaseController
    {
        private OrderService OrderService { get; }

        public OrdersController(OrderService service)
        {
            OrderService = service;
        }

        [HttpGet]
        public async Task<List<GetOrderModel>> GetUserList()
        {
            return await OrderService.GetList();
        }
    }
}