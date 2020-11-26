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
        private SubscriptionService SubscriptionProductService { get; }

        public SubscriptionProductsController(SubscriptionService service)
        {
            SubscriptionProductService = service;
        }

        [HttpGet]
        [Route("")]
        public async Task<List<GetSubscriptionModel>> GetUserList()
        {
            return await SubscriptionProductService.GetList();
        }

        [HttpGet]
        [Route("{subscriptionId:guid}")]
        public async Task<GetSubscriptionModel> GetSubscription(Guid subscriptionId)
        {
            return await SubscriptionProductService.GetItem(subscriptionId);
        }

        /*[HttpGet]
        [Route("user-types")]
        public List<UserType> GetUserTypes()
        {
            return UserService.GetUserTypes();
        }*/

        [HttpPut]
        [Route("")]
        public async Task<IHttpActionResult> AddEdiSubscription([FromBody] AddEditSubscriptionModel model)
        {
            await SubscriptionProductService.AddEditSubscription(model);
            return Ok();
        }
    }
}