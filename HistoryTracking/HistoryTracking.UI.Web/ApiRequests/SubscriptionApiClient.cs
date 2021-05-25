using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HistoryTracking.BL.Services.Order.Models;
using HistoryTracking.BL.Services.SubscriptionProducts.Models;
using HistoryTracking.DAL.Enums;

namespace HistoryTracking.UI.Web.ApiRequests
{
    public class SubscriptionApiClient : BaseApiClient
    {
        public SubscriptionApiClient(UiSettings settings) : base(settings)
        {
        }

        public Task<ApiCallDataResult<List<SubscriptionModel>>> GetSubscriptionListAsync()
        {
            return Api.GetAsync<List<SubscriptionModel>>("subscriptions");
        }

        public Task<ApiCallResult> AddEditSubscriptionAsync(AddEditSubscriptionModel model)
        {
            return Api.PutAsync(
                $"subscriptions",
                model
            );
        }

        public Task<ApiCallDataResult<SubscriptionModel>> GetSibscriptionAsync(Guid subscriptionId)
        {
            return Api.GetAsync<SubscriptionModel>(
                $"subscriptions/{subscriptionId}"
            );
        }

        public Task<ApiCallDataResult<List<CurrencyType>>> GetCurrencyListAsync()
        {
            return Api.GetAsync<List<CurrencyType>>(
                $"subscriptions/currency-types"
            );
        }
    }
}
