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
using HistoryTracking.DAL.Enums;

namespace HistoryTracking.Api.Controllers
{
    [RoutePrefix("orders")]
    public class OrdersController : BaseController
    {
        private OrderService OrderService { get; }

        public OrdersController(OrderService service)
        {
            OrderService = service;
        }

        [HttpGet]
        [Route("")]
        public async Task<List<GetOrderModel>> GetUserList()
        {
            return await OrderService.GetList();
        }
        
        [HttpGet]
        [Route("{orderId:guid}")]
        public async Task<GetOrderModel> GetOrder(Guid orderId)
        {
            return await OrderService.GetItem(orderId);
        }

        [HttpGet]
        [Route("order-status-types")]
        public List<OrderStatusType> GetCurrencyTypes()
        {
            return OrderService.GetOrderStatusTypes();
        }

        [HttpPut]
        [Route("")]
        public async Task<IHttpActionResult> AddEdiOrder([FromBody] AddEditOrderModel model)
        {
            await OrderService.AddEditItem(model);
            return Ok();
        }
    }
}