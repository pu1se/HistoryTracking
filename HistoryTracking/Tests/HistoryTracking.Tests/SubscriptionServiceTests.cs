using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HistoryTracking.BL.Services.SubscriptionProducts;
using HistoryTracking.DAL;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HistoryTracking.Tests
{
    [TestClass]
    public class SubscriptionServiceTests : BaseServiceTests<SubscriptionService>
    {
        [TestMethod]
        public async Task EditSubscription()
        {
            try
            {
                var subscription = await Storage.SubscriptionProducts.FirstAsync();

                var oldName = subscription.Name;
                var oldResellerMarkup = subscription.ResellerMarkupAsPercent;
                var oldDistiMarkup = subscription.DistributorMarkupAsPercent;
                var newName = "new name " + Guid.NewGuid();
                subscription.Name = newName;
                var random = new Random();
                subscription.DistributorMarkupAsPercent += random.Next(1, 30);
                subscription.ResellerMarkupAsPercent += random.Next(1, 30);
                Storage.SubscriptionProducts.AddOrUpdate(subscription);
                await Storage.SaveChangesAsync();
                CleanStorageCache();

                subscription = await Storage.SubscriptionProducts.FirstAsync();
                Assert.IsTrue(subscription.Name == newName);
                Assert.IsTrue(subscription.DistributorMarkupAsPercent > oldDistiMarkup);
                Assert.IsTrue(subscription.ResellerMarkupAsPercent > oldResellerMarkup);
                subscription.Name = oldName;
                subscription.DistributorMarkupAsPercent = oldDistiMarkup;
                subscription.ResellerMarkupAsPercent = oldResellerMarkup;
                Storage.SubscriptionProducts.AddOrUpdate(subscription);
                await Storage.SaveChangesAsync();
                subscription = await Storage.SubscriptionProducts.FirstAsync();
                Assert.IsTrue(subscription.Name == oldName);
                Assert.IsTrue(subscription.DistributorMarkupAsPercent == oldDistiMarkup);
                Assert.IsTrue(subscription.ResellerMarkupAsPercent == oldResellerMarkup);
                Assert.IsTrue(subscription.UpdatedDateUtc >= DateTime.UtcNow.Date);
                Assert.IsTrue(subscription.UpdatedByUserId == TestData.SystemUserId);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }
    }
}
