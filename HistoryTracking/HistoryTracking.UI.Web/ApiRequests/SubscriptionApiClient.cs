using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HistoryTracking.BL.Services.Order.Models;
using HistoryTracking.BL.Services.SubscriptionProducts.Models;

namespace HistoryTracking.UI.Web.ApiRequests
{
    public class SubscriptionApiClient : BaseApiClient
    {
        public SubscriptionApiClient(UiSettings settings) : base(settings)
        {
        }

        public Task<ApiCallDataResult<List<GetSubscriptionProductModel>>> GetSubscriptionListAsync()
        {
            return Api.GetAsync<List<GetSubscriptionProductModel>>("subscriptions");
        }
    }
}
