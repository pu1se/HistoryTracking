using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HistoryTracking.BL.Services;
using HistoryTracking.BL.Services.Order.Models;
using HistoryTracking.DAL;
using HistoryTracking.DAL.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HistoryTracking.Tests
{
    [TestClass]
    public class PerformanceTest : BaseServiceTests<OrderService>
    {
        /*[TestMethod]
        public async Task AddEditLoop()
        {
            await Service.AddEditItem(new AddEditOrderModel
            {
                CustomerId = TestData.CustomerUserId,
                OrderDate = DateTime.UtcNow,
                OrderStatus = OrderStatusType.WaitingForApproval,
                SubscriptionIdList = new List<Guid> { TestData.SubscriptionProductId }
            });
        }*/
    }
}
