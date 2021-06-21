using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HistoryTracking.BL.Services.Changes;
using HistoryTracking.BL.Services.Changes.Models;
using HistoryTracking.DAL;
using HistoryTracking.DAL.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HistoryTracking.Tests
{
    [TestClass]
    public class EntityChangeServiceTests : BaseServiceTests<EntityChangesService>
    {
        [TestMethod]
        public async Task CheckGetTableNamesTest()
        {
            var tableNames = await Service.GetTrackingTableNamesAsync();

            Assert.IsTrue(tableNames.Any());
            Assert.IsTrue(tableNames.First().EntityName.IsNullOrEmpty() == false);
            Assert.IsTrue(tableNames.First().EntityNameForDisplaying.IsNullOrEmpty() == false);
        }

        [TestMethod]
        public async Task CheckGetChangesAsync()
        {
            var changes = await Service.GetChangesAsync(new GetChangesListModel());

            foreach (var change in changes)
            {
                Assert.IsTrue(change.ChangeDate > DateWhenProjectWasStarted);
                Assert.IsTrue(change.ChangeType.IsNullOrEmpty() == false);
                foreach (var propertyChange in change.PropertyChanges)
                {
                    Assert.IsTrue(propertyChange.PropertyName.IsNullOrEmpty() == false);
                }
            }
        }


        [TestMethod]
        public async Task CheckUserRoleFiltration()
        {
            var allChanges = await Service.GetChangesAsync(new GetChangesListModel{FilterByUserRole = UserType.SystemUser});
            var allPropertyChanges = allChanges.SelectMany(x => x.PropertyChanges);

            var notAllChanges = await Service.GetChangesAsync(new GetChangesListModel{FilterByUserRole = UserType.Customer});
            var notAllPropertyChanges = notAllChanges.SelectMany(x => x.PropertyChanges);

            Assert.IsTrue(allPropertyChanges.Count() > notAllPropertyChanges.Count());
        }


        [TestMethod]
        public async Task ChangeTwoEntitiesAndTrackTheChanges()
        {
            var user = await Storage.Users.Include(x=> x.Contacts).Include(x=>x.Addresses).FirstAsync();
            var oldUserName = user.Name;
            var newUserName = "new name " + Guid.NewGuid();
            user.Name = newUserName;
            Storage.Users.AddOrUpdate(user);

            var subscription = await Storage.SubscriptionProducts.FirstAsync();
            var oldSubscriptionName = subscription.Title;
            var oldResellerMarkup = subscription.ResellerMarkupAsPercent;
            var oldDistiMarkup = subscription.DistributorMarkupAsPercent;
            var newSubscriptionName = "new name " + Guid.NewGuid();
            subscription.Title = newSubscriptionName;
            var random = new Random();
            subscription.DistributorMarkupAsPercent += random.Next(1, 30);
            subscription.ResellerMarkupAsPercent += random.Next(1, 30);
            Storage.SubscriptionProducts.AddOrUpdate(subscription);

            await Storage.SaveChangesAsync();
            CleanStorageCache();


            var changes = await Service.GetChangesAsync(new GetChangesListModel
            {
                TakeHistoryForLastNumberOfDays = 1,
                UserIds = new List<Guid>{TestData.SystemUserId}
            });
            var ids = new[] {user.Id, subscription.Id}.ToList();
            changes = changes.Where(x => ids.Contains(x.EntityId)).ToList();
            Assert.IsTrue(changes.Any(change => change.PropertyChanges.Any(
                property => 
                property.PropertyName == nameof(user.Name) &&
                property.OldValue == oldUserName &&
                property.NewValue == newUserName)));
            Assert.IsTrue(changes.Any(change => change.PropertyChanges.Any(
                property => 
                property.PropertyName == nameof(subscription.Title) &&
                property.OldValue == oldSubscriptionName &&
                property.NewValue == newSubscriptionName)));
            Assert.IsTrue(changes.Any(change => change.PropertyChanges.Any(
                property => 
                property.PropertyName == nameof(subscription.DistributorMarkupAsPercent) &&
                property.OldValue == oldDistiMarkup.ToString())));
            Assert.IsTrue(changes.Any(change => change.PropertyChanges.Any(
                property => 
                property.PropertyName == nameof(subscription.ResellerMarkupAsPercent) &&
                property.OldValue == oldResellerMarkup.ToString())));


            user.Name = oldUserName;
            Storage.Users.AddOrUpdate(user);
            await Storage.SaveChangesAsync();
            user = await Storage.Users.Include(x=> x.Contacts).Include(x=>x.Addresses).FirstAsync();
            Assert.IsTrue(user.Name == oldUserName);
            Assert.IsTrue(user.UpdatedDateUtc >= DateTime.UtcNow.Date);
            Assert.IsTrue(user.UpdatedByUserId == TestData.SystemUserId);
            subscription.Title = oldSubscriptionName;
            subscription.DistributorMarkupAsPercent = oldDistiMarkup;
            subscription.ResellerMarkupAsPercent = oldResellerMarkup;
            Storage.SubscriptionProducts.AddOrUpdate(subscription);
            await Storage.SaveChangesAsync();
            subscription = await Storage.SubscriptionProducts.FirstAsync();
            Assert.IsTrue(subscription.Title == oldSubscriptionName);
            Assert.IsTrue(subscription.DistributorMarkupAsPercent == oldDistiMarkup);
            Assert.IsTrue(subscription.ResellerMarkupAsPercent == oldResellerMarkup);
            Assert.IsTrue(subscription.UpdatedDateUtc >= DateTime.UtcNow.Date);
            Assert.IsTrue(subscription.UpdatedByUserId == TestData.SystemUserId);
        }
    }
}
