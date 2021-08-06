using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HistoryTracking.BL.Services;
using HistoryTracking.DAL;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HistoryTracking.Tests
{
    [TestClass]
    public class UserServiceTests : BaseServiceTests<UserService>
    {
        [TestMethod]
        public async Task CheckGetUserList()
        {
            var users = await Service.GetList();

            Assert.IsTrue(users != null);
            Assert.IsTrue(users.Any());
            Assert.IsTrue(users.First().Name.IsNullOrEmpty() == false);
        }

        [TestMethod]
        public async Task EditUser()
        {
            var user = await Storage.Users.Include(x=> x.Contacts).Include(x=>x.Addresses).FirstAsync();

            var oldName = user.Name;
            var newName = "new name " + Guid.NewGuid();
            user.Name = newName;
            var oldAddress = user.Addresses.First().HouseAddress;
            user.Addresses.First().HouseAddress += " some more info";
            Storage.Users.AddOrUpdate(user);
            await Storage.SaveChangesAsync();
            CleanStorageCache();

            user = await Storage.Users.Include(x=> x.Contacts).Include(x=>x.Addresses).FirstAsync();
            Assert.IsTrue(user.Name == newName);
            user.Name = oldName;
            user.Addresses.First().HouseAddress = oldAddress;
            Storage.Users.AddOrUpdate(user);
            await Storage.SaveChangesAsync();
            user = await Storage.Users.Include(x=> x.Contacts).Include(x=>x.Addresses).FirstAsync();
            Assert.IsTrue(user.Name == oldName);
            Assert.IsTrue(user.UpdatedDateUtc >= DateTime.UtcNow.Date);
            Assert.IsTrue(user.UpdatedByUserId == TestData.SystemUserId);
        }
    }
}
