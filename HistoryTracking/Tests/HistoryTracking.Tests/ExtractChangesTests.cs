using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HistoryTracking.DAL.Entities;
using HistoryTracking.DAL.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HistoryTracking.Tests
{
    [TestClass]
    public class ExtractChangesTests
    {
        /*[TestMethod]
        public void TrackNoChangesForTheSameUserEntityWhichIsNotNull()
        {
            var userEntity = new UserEntity
            {
                Id = Guid.NewGuid(),
                Email = "some@email.com",
                Name = "some name",
                UserType = UserType.Customer
            };
            var result = GetChanges.For(userEntity, userEntity);
            Assert.IsTrue(result.Count == 0);
        }

        [TestMethod]
        public void TrackNoChangesForTheSameUserEntityWhichIsNull()
        {
            var result = GetChanges.For<UserEntity>(null, null);
            Assert.IsTrue(result.Count == 0);
            var userEntity = new UserEntity();
            result = GetChanges.For(userEntity, userEntity);
            Assert.IsTrue(result.Count == 0);
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
            var newUserEntity = oldUserEntity.DeepClone();
            newUserEntity.Name = "new name";

            var result = GetChanges.For(oldUserEntity, newUserEntity);
            Assert.IsTrue(result.Count > 0);
            Assert.IsTrue(result.Count == 1);
            Assert.IsTrue(result.First().OldValue == oldUserEntity.Name);
            Assert.IsTrue(result.First().NewValue == newUserEntity.Name);
            Assert.IsTrue(result.First().WasAddedForFirstTime == false);
        }

        [TestMethod]
        public void TrackFirstOneChangeForTheUserEntity()
        {
            var oldUserEntity = new UserEntity();
            var newUserEntity = oldUserEntity.DeepClone();
            newUserEntity.Name = "new name";

            var result = GetChanges.For(oldUserEntity, newUserEntity);
            Assert.IsTrue(result.Count > 0);
            Assert.IsTrue(result.Count == 1);
            Assert.IsTrue(result.First().OldValue == (oldUserEntity.Name ?? string.Empty));
            Assert.IsTrue(result.First().NewValue == (newUserEntity.Name ?? string.Empty));
            Assert.IsTrue(result.First().WasAddedForFirstTime == true);
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
            var newUserEntity = oldUserEntity.DeepClone();
            newUserEntity.Name = "new name";
            newUserEntity.Email = "new@email.com";
            newUserEntity.UserType = UserType.Reseller;

            var result = GetChanges.For(oldUserEntity, newUserEntity);
            Assert.IsTrue(result.Count > 0);
        }*/
    }
}
