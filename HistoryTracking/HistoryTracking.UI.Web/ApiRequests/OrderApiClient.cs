using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HistoryTracking.BL.Services.Order.Models;
using HistoryTracking.BL.Services.SubscriptionProducts.Models;
using HistoryTracking.BL.Services.User;
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

        public Task<ApiCallDataResult<GetOrderModel>> GetOrderAsync(Guid orderId)
        {
            return Api.GetAsync<GetOrderModel>(
                $"orders/{orderId}"
            );
        }

        public Task<ApiCallDataResult<List<OrderStatusType>>> GetOrderStatusTypeListAsync()
        {
            return Api.GetAsync<List<OrderStatusType>>(
                $"orders/order-status-types"
            );
        }

        public async Task<ApiCallDataResult<List<GetUserModel>>> GetCustomerListAsync()
        {
            var getUserListResult = await UserApi.GetUserListAsync();
            if (!getUserListResult.IsSuccess)
            {
                return getUserListResult;
            }

            var customerList = getUserListResult.Data.Where(item => item.UserType == UserType.Customer).ToList();
            return new ApiCallDataResult<List<GetUserModel>>(customerList);
        }

        public async Task<ApiCallDataResult<List<GetSubscriptionModel>>> GetSubscriptionList()
        {
            var getSubscriptionListResult = await SubscriptionApi.GetSubscriptionListAsync();
            if (!getSubscriptionListResult.IsSuccess)
            {
                return getSubscriptionListResult;
            }

            return new ApiCallDataResult<List<GetSubscriptionModel>>(getSubscriptionListResult.Data);
        }
    }
}
