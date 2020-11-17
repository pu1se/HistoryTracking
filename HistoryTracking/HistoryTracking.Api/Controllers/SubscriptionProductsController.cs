using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using HistoryTracking.BL.Services.Order.Models;
using HistoryTracking.BL.Services.SubscriptionProducts;
using HistoryTracking.BL.Services.SubscriptionProducts.Models;

namespace HistoryTracking.Api.Controllers
{
    [Route("subscriptions")]
    public class SubscriptionProductsController : BaseController
    {
        private SubscriptionProductService SubscriptionProductService { get; }

        public SubscriptionProductsController(SubscriptionProductService service)
        {
            SubscriptionProductService = service;
        }

        [HttpGet]
        public async Task<List<GetSubscriptionProductModel>> GetUserList()
        {
            return await SubscriptionProductService.GetList();
        }
    }
}