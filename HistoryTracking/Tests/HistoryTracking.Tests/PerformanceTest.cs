using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HistoryTracking.BL.Services;
using HistoryTracking.BL.Services.Order.Models;
using HistoryTracking.BL.Services.SubscriptionProducts;
using HistoryTracking.BL.Services.SubscriptionProducts.Models;
using HistoryTracking.DAL;
using HistoryTracking.DAL.Entities;
using HistoryTracking.DAL.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HistoryTracking.Tests
{
    [TestClass]
    public class PerformanceTest : BaseTest
    {
        [TestMethod]
        public async Task AddEditLoop()
        {
            var index = 0;
            while (index++ < 100)
            {
                var id = Guid.NewGuid();
                var addSubscription = new SubscriptionProductEntity
                {
                    Id = id,
                    Name = "some name" + id,
                    CreatedByUserId = TestData.ResellerUserId,
                    UpdatedByUserId = TestData.ResellerUserId,
                    AccountId = TestData.ResellerUserId,
                    AutoRenewEnabled = true,
                    AzureTenantId = TestData.ResellerUserId,
                    Currency = CurrencyType.Euro,
                    DistributorMarkupAsPercent = 10,
                    EntitlementId = Guid.NewGuid().ToString(),
                };

                Storage.SubscriptionProducts.AddOrUpdate(addSubscription);

                if (index % 5 == 0)
                {
                    await Storage.SaveChangesAsync();
                    CleanStorageCache();
                }
                
            }
        }
    }
}
