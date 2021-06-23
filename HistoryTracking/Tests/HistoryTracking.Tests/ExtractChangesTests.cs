using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HistoryTracking.DAL.Entities;
using HistoryTracking.DAL.Enums;
using HistoryTracking.DAL.TrackEntityChangesLogic;
using HistoryTracking.DAL.TrackEntityChangesLogic.PropertiesTrackingConfigurations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HistoryTracking.Tests
{
    [TestClass]
    public class ExtractChangesTests : BaseTest
    {
        [TestMethod]
        public void TrackNoChangesForTheSameUserEntityWhichIsNotNull()
        {
            var userEntity = new UserEntity
            {
                Id = Guid.NewGuid(),
                Email = "some@email.com",
                Name = "some name",
                UserType = UserType.Customer
            };
            var config = ConfigurationOfTrackedEntities.GetConfigFor(userEntity.GetType());

            var result = CompareAndGetChanges.For(userEntity, userEntity, config);
            Assert.IsTrue(result.Count == 0);
        }

        [TestMethod]
        public void TrackNoChangesForNull()
        {
            var userEntity = new UserEntity();
            var config = ConfigurationOfTrackedEntities.GetConfigFor(userEntity.GetType());

            var result = CompareAndGetChanges.For((UserEntity) null, null, config);
            Assert.IsTrue(result.Count == 0);
            
            result = CompareAndGetChanges.For(userEntity, userEntity, config);
            Assert.IsTrue(result.Count == 0);
        }

        [TestMethod]
        public void TrackNoChangesForNewEntity()
        {
            var userEntity = new UserEntity{Email = "tmp@tut.by"};
            var config = ConfigurationOfTrackedEntities.GetConfigFor(userEntity.GetType());

            var result = CompareAndGetChanges.For(null, userEntity, config);
            Assert.IsTrue(result.Count == 2);

            var userTypeChange = result.FirstOrDefault(x => x.PropertyName == nameof(UserEntity.UserType));
            Assert.IsTrue(userTypeChange != null);
            Assert.IsTrue(userTypeChange.OldValue == null);
            Assert.IsTrue(userTypeChange.NewValue == default(UserType).ToString());

            var emailChange = result.FirstOrDefault(x => x.PropertyName == nameof(userEntity.Email));
            Assert.IsTrue(emailChange != null);
            Assert.IsTrue(emailChange.OldValue == null);
            Assert.IsTrue(emailChange.NewValue.IsNullOrEmpty() == false);
        }

        [TestMethod]
        public void TrackNoChangesForDeletedEntity()
        {
            var userEntity = new UserEntity();
            var config = ConfigurationOfTrackedEntities.GetConfigFor(userEntity.GetType());

            var result = CompareAndGetChanges.For(userEntity, null, config);
            Assert.IsTrue(result.Count == 1);
            Assert.IsTrue(result.First().OldValue == default(UserType).ToString());
            Assert.IsTrue(result.First().NewValue == null);
        }

        [TestMethod]
        public void TrackOneChangeForTheUserEntity()
        {
            var oldUserEntity = new UserEntity
            {
                Id = Guid.NewGuid(),
                Email = "some@email.com",
                Name = "some name",
                UserType = UserType.Customer
            };
            var config = ConfigurationOfTrackedEntities.GetConfigFor(oldUserEntity.GetType());
            var newUserEntity = oldUserEntity.DeepClone();
            newUserEntity.Name = "new name";

            var result = CompareAndGetChanges.For(oldUserEntity, newUserEntity, config);
            Assert.IsTrue(result.Count == 1);
            Assert.IsTrue(result.First().OldValue == oldUserEntity.Name);
            Assert.IsTrue(result.First().NewValue == newUserEntity.Name);
        }

        [TestMethod]
        public void TrackFirstOneChangeForTheUserEntity()
        {
            var oldUserEntity = new UserEntity();
            var config = ConfigurationOfTrackedEntities.GetConfigFor(oldUserEntity.GetType());
            var newUserEntity = oldUserEntity.DeepClone();
            newUserEntity.Name = "new name";

            var result = CompareAndGetChanges.For(oldUserEntity, newUserEntity, config);
            Assert.IsTrue(result.Count == 1);
            Assert.IsTrue(result.First().OldValue ==  null);
            Assert.IsTrue(result.First().NewValue == (newUserEntity.Name ?? string.Empty));
        }

        [TestMethod]
        public void TrackManyChangesForTheUserEntity()
        {
            var oldUserEntity = new UserEntity
            {
                Id = Guid.NewGuid(),
                Email = "some@email.com",
                Name = "some name",
                UserType = UserType.Customer
            };
            var config = ConfigurationOfTrackedEntities.GetConfigFor(oldUserEntity.GetType());
            var newUserEntity = oldUserEntity.DeepClone();
            newUserEntity.Name = "new name";
            newUserEntity.Email = "new@email.com";
            newUserEntity.UserType = UserType.Reseller;

            var result = CompareAndGetChanges.For(oldUserEntity, newUserEntity, config);
            Assert.IsTrue(result.Count == 3);
            var userTypeChange = result.FirstOrDefault(x => x.PropertyName == "UserType");
            Assert.IsTrue(userTypeChange != null);

            Assert.IsTrue(userTypeChange.OldValue == oldUserEntity.UserType.ToString());
            Assert.IsTrue(userTypeChange.NewValue == newUserEntity.UserType.ToString());
        }

        [TestMethod]
        public void GetOneModifiedComplexPropertyChanges()
        {
            var oldUserEntity = new UserEntity
            {
                Addresses = new List<UserAddressEntity>
                {
                    new UserAddressEntity{City = "city 1", HouseAddress = "address 1"}
                }
            };
            var config = ConfigurationOfTrackedEntities.GetConfigFor(oldUserEntity.GetType());
            var newUserEntity = oldUserEntity.DeepClone();
            newUserEntity.Addresses.First().City = "another city 2";
            newUserEntity.Addresses.First().HouseAddress = "another address 2";

            var result = CompareAndGetChanges.For(oldUserEntity, newUserEntity, config);
            Assert.IsTrue(result.Count == 2);

            Assert.IsTrue(result.First().OldValue == oldUserEntity.Addresses.First().City);
            Assert.IsTrue(result.First().NewValue == newUserEntity.Addresses.First().City);

            Assert.IsTrue(result.Last().OldValue == oldUserEntity.Addresses.First().HouseAddress);
            Assert.IsTrue(result.Last().NewValue == newUserEntity.Addresses.First().HouseAddress);
        }

        [TestMethod]
        public void GetOneAddedComplexPropertyChanges()
        {
            var oldUserEntity = new UserEntity
            {
            };
            var config = ConfigurationOfTrackedEntities.GetConfigFor(oldUserEntity.GetType());
            var newUserEntity = oldUserEntity.DeepClone();
            newUserEntity.Addresses = new List<UserAddressEntity>
            {
                new UserAddressEntity {City = "City 2", HouseAddress = "Address 2"}
            };

            var result = CompareAndGetChanges.For(oldUserEntity, newUserEntity, config);
            Assert.IsTrue(result.Count == 2);

            Assert.IsTrue(result.First().OldValue == null);
            Assert.IsTrue(result.First().NewValue == newUserEntity.Addresses.First().City);

            Assert.IsTrue(result.Last().OldValue == null);
            Assert.IsTrue(result.Last().NewValue == newUserEntity.Addresses.First().HouseAddress);
        }

        [TestMethod]
        public void GetTwoComplexPropertyChanges()
        {
            var oldUserEntity = new UserEntity
            {
                Addresses = new List<UserAddressEntity>
                {
                    new UserAddressEntity{City = "city 1", HouseAddress = "address 1"}
                },
                Contacts = new List<UserContactEntity>
                {
                    new UserContactEntity{PhoneNumber = "one number"}
                }
            };
            var config = ConfigurationOfTrackedEntities.GetConfigFor(oldUserEntity.GetType());
            var newUserEntity = oldUserEntity.DeepClone();
            newUserEntity.Addresses.First().City = "another city 2";
            newUserEntity.Addresses.First().HouseAddress = "another address 2";
            newUserEntity.Contacts.First().PhoneNumber = "another number";

            var result = CompareAndGetChanges.For(oldUserEntity, newUserEntity, config);
            Assert.IsTrue(result.Count == 3);

            Assert.IsTrue(result[0].OldValue == oldUserEntity.Addresses.First().City);
            Assert.IsTrue(result[0].NewValue == newUserEntity.Addresses.First().City);

            Assert.IsTrue(result[1].OldValue == oldUserEntity.Addresses.First().HouseAddress);
            Assert.IsTrue(result[1].NewValue == newUserEntity.Addresses.First().HouseAddress);

            Assert.IsTrue(result[2].OldValue == oldUserEntity.Contacts.First().PhoneNumber);
            Assert.IsTrue(result[2].NewValue == newUserEntity.Contacts.First().PhoneNumber);
        }



        [TestMethod]
        public void GetOneModifiedChildPropertyChanges()
        {
            var oldSubscriptionEntity = new SubscriptionProductEntity()
            {
                ChildrenSubscriptions = new List<SubscriptionProductEntity>
                {
                    new SubscriptionProductEntity
                    {
                        Price = 10,
                        Currency = CurrencyType.Euro
                    }
                }
            };
            var config = ConfigurationOfTrackedEntities.GetConfigFor(oldSubscriptionEntity.GetType());
            var newSubscriptionEntity = oldSubscriptionEntity.DeepClone();
            newSubscriptionEntity.ChildrenSubscriptions.First().Price = 20;
            newSubscriptionEntity.ChildrenSubscriptions.First().Currency = CurrencyType.Nok;

            var result = CompareAndGetChanges.For(oldSubscriptionEntity, newSubscriptionEntity, config);
            Assert.IsTrue(result.Count > 0);
        }

        
    }
}
