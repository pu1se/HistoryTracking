﻿using System;
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
        public OrderApiClient(UiSettings settings) : base(settings)
        {
        }

        public Task<ApiCallDataResult<List<OrderModel>>> GetOrderListAsync()
        {
            return Api.GetAsync<List<OrderModel>>("orders");
        }

        public Task<ApiCallResult> AddEditOrderAsync(AddEditOrderModel model)
        {
            return Api.PutAsync(
                $"orders",
                model
            );
        }

        public Task<ApiCallDataResult<OrderModel>> GetOrderAsync(Guid orderId)
        {
            return Api.GetAsync<OrderModel>(
                $"orders/{orderId}"
            );
        }

        public Task<ApiCallDataResult<List<OrderStatusType>>> GetOrderStatusTypeListAsync()
        {
            return Api.GetAsync<List<OrderStatusType>>(
                $"orders/order-status-types"
            );
        }
    }
}
