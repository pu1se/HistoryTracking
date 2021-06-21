using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HistoryTracking.BL.Services.Changes;
using HistoryTracking.BL.Services.Changes.Models;
using HistoryTracking.BL.Services.SubscriptionProducts;
using HistoryTracking.DAL;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HistoryTracking.Tests
{
    [TestClass]
    public class SubscriptionServiceTests : BaseServiceTests<EntityChangesService>
    {
        [TestMethod]
        public async Task AddChildSubscriptionToSubscription()
        {
            var childSubscription = await Storage.SubscriptionProducts
                .FirstOrDefaultAsync(x => x.Id == TestData.SubscriptionProductId);


        }

        [TestMethod]
        public async Task EditChildSubscriptionToSubscription()
        {
            var parentSubscription = await Storage.SubscriptionProducts.Include(x => x.ChildrenSubscriptions)
                .FirstOrDefaultAsync(x => x.Id == TestData.ParentSubscriptionProductId);

            if (!parentSubscription.ChildrenSubscriptions.Any())
            {
                return;
            }


            /*var childSubscription = parentSubscription.ChildrenSubscriptions.First();
            var oldTitle = childSubscription.Title;
            childSubscription.Title = "new title" + Guid.NewGuid();
            await Storage.SaveChangesAsync();

            childSubscription.Title = oldTitle;
            await Storage.SaveChangesAsync();*/

            var changes = await Service.GetChangesAsync(new GetChangesListModel
            {
                EntityId = TestData.SubscriptionProductId
            });


        }
    }
}
