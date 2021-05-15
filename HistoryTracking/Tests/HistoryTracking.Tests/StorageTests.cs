using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.DependencyResolution;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;
using HistoryTracking.DAL;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HistoryTracking.Tests
{
    [TestClass]
    public class StorageTests : BaseTest
    {
        [TestMethod]
        public async Task CheckDataContextIsWorking()
        {
            var list = await Storage.Users.ToListAsync();
            Assert.IsTrue(list != null);
            Assert.IsTrue(list.Any());
        }

        [TestMethod]
        public async Task EditUser()
        {
            try
            {
                var users = await Storage.Users.ToListAsync();
                var user = users.First();

                var oldName = user.Name;
                var newName = "new name " + Guid.NewGuid();
                user.Name = newName;
                Storage.Users.AddOrUpdate(user);
                await Storage.SaveChangesAsync();
                CleanStorageCache();

                users = await Storage.Users.ToListAsync();
                user = users.First();
                Assert.IsTrue(user.Name == newName);
                user.Name = oldName;
                Storage.Users.AddOrUpdate(user);
                await Storage.SaveChangesAsync();
                users = await Storage.Users.ToListAsync();
                user = users.First();
                Assert.IsTrue(user.Name == oldName);
                Assert.IsTrue(user.UpdatedDateUtc >= DateTime.UtcNow.Date);
                Assert.IsTrue(user.UpdatedByUserId == TestData.SystemUserId);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }
    }
}
