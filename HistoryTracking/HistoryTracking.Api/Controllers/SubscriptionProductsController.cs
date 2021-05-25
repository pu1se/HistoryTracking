using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using HistoryTracking.BL.Services;
using HistoryTracking.BL.Services.Order.Models;
using HistoryTracking.BL.Services.SubscriptionProducts;
using HistoryTracking.BL.Services.SubscriptionProducts.Models;
using HistoryTracking.BL.Services.User;
using HistoryTracking.DAL.Enums;

namespace HistoryTracking.Api.Controllers
{
    [RoutePrefix("subscriptions")]
    public class SubscriptionProductsController : BaseController
    {
        private SubscriptionService SubscriptionService { get; }

        public SubscriptionProductsController(SubscriptionService service)
        {
            SubscriptionService = service;
        }

        [HttpGet]
        [Route("")]
        public async Task<List<SubscriptionModel>> GetUserList()
        {
            return await SubscriptionService.GetList();
        }

        [HttpGet]
        [Route("{subscriptionId:guid}")]
        public async Task<SubscriptionModel> GetSubscription(Guid subscriptionId)
        {
            return await SubscriptionService.GetItem(subscriptionId);
        }

        [HttpGet]
        [Route("currency-types")]
        public List<CurrencyType> GetCurrencyTypes()
        {
            return SubscriptionService.GetCurrencyTypes();
        }

        [HttpPut]
        [Route("")]
        public async Task<IHttpActionResult> AddEdiSubscription([FromBody] AddEditSubscriptionModel model)
        {
            await SubscriptionService.AddEditSubscription(model);
            return Ok();
        }
    }
}