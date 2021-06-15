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
            var config = TrackingEntitiesConfiguration.GetConfigFor(userEntity.GetType());

            var result = GetPropertyChangesWay2.For(userEntity, userEntity, config);
            Assert.IsTrue(result.Count == 0);
        }

        [TestMethod]
        public void TrackNoChangesForNull()
        {
            var userEntity = new UserEntity();
            var config = TrackingEntitiesConfiguration.GetConfigFor(userEntity.GetType());

            var result = GetPropertyChangesWay2.For((UserEntity) null, null, config);
            Assert.IsTrue(result.Count == 0);
            
            result = GetPropertyChangesWay2.For(userEntity, userEntity, config);
            Assert.IsTrue(result.Count == 0);
        }

        [TestMethod]
        public void TrackNoChangesForNewEntity()
        {
            var userEntity = new UserEntity{Email = "tmp@tut.by"};
            var config = TrackingEntitiesConfiguration.GetConfigFor(userEntity.GetType());

            var result = GetPropertyChangesWay2.For(null, userEntity, config);
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
            var config = TrackingEntitiesConfiguration.GetConfigFor(userEntity.GetType());

            var result = GetPropertyChangesWay2.For(userEntity, null, config);
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
            var config = TrackingEntitiesConfiguration.GetConfigFor(oldUserEntity.GetType());
            var newUserEntity = oldUserEntity.DeepClone();
            newUserEntity.Name = "new name";

            var result = GetPropertyChangesWay2.For(oldUserEntity, newUserEntity, config);
            Assert.IsTrue(result.Count == 1);
            Assert.IsTrue(result.First().OldValue == oldUserEntity.Name);
            Assert.IsTrue(result.First().NewValue == newUserEntity.Name);
        }

        [TestMethod]
        public void TrackFirstOneChangeForTheUserEntity()
        {
            var oldUserEntity = new UserEntity();
            var config = TrackingEntitiesConfiguration.GetConfigFor(oldUserEntity.GetType());
            var newUserEntity = oldUserEntity.DeepClone();
            newUserEntity.Name = "new name";

            var result = GetPropertyChangesWay2.For(oldUserEntity, newUserEntity, config);
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
            var config = TrackingEntitiesConfiguration.GetConfigFor(oldUserEntity.GetType());
            var newUserEntity = oldUserEntity.DeepClone();
            newUserEntity.Name = "new name";
            newUserEntity.Email = "new@email.com";
            newUserEntity.UserType = UserType.Reseller;

            var result = GetPropertyChangesWay2.For(oldUserEntity, newUserEntity, config);
            Assert.IsTrue(result.Count == 3);
            var userTypeChange = result.FirstOrDefault(x => x.PropertyName == "UserType");
            Assert.IsTrue(userTypeChange != null);

            Assert.IsTrue(userTypeChange.OldValue == oldUserEntity.UserType.ToString());
            Assert.IsTrue(userTypeChange.NewValue == newUserEntity.UserType.ToString());
        }

        [TestMethod]
        public void GetComplexChanges()
        {

        }
    }
}
