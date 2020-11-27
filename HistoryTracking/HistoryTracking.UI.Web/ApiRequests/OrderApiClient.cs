using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HistoryTracking.BL.Services.Order.Models;
using HistoryTracking.DAL.Enums;

namespace HistoryTracking.UI.Web.ApiRequests
{
    public class OrderApiClient : BaseApiClient
    {
        private SubscriptionApiClient SubscriptionApi { get; }
        private UserApiClient UserApi { get; }

        public OrderApiClient(UiSettings settings, SubscriptionApiClient subscriptionApi, UserApiClient userApi) : base(settings)
        {
            SubscriptionApi = subscriptionApi;
            UserApi = userApi;
        }

        public Task<ApiCallDataResult<List<GetOrderModel>>> GetOrderListAsync()
        {
            return Api.GetAsync<List<GetOrderModel>>("orders");
        }

        public Task<ApiCallResult> AddEditOrderAsync(AddEditOrderModel model)
        {
            return Api.PutAsync(
                $"orders",
                model
            );
        }

        public Task<ApiCallDataResult<GetOrderModel>> GetOrderAsync(Guid subscriptionId)
        {
            return Api.GetAsync<GetOrderModel>(
                $"orders/{subscriptionId}"
            );
        }

        public Task<ApiCallDataResult<List<OrderStatusType>>> GetOrderStatusTypeListAsync()
        {
            return Api.GetAsync<List<OrderStatusType>>(
                $"orders/currency-types"
            );
        }
    }
}
