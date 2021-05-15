using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HistoryTracking.DAL.Sql
{
    [TestClass]
    public class Tests
    {
        [TestMethod]
        public async Task Tmp()
        {
            using (var Storage = new DataContext())

            try
            {
                var users = await Storage.Products.ToListAsync();
                var user = users.Last();

                var oldName = user.Name;
                var newName = "new name " + Guid.NewGuid();
                user.Name = newName;
                Storage.Products.AddOrUpdate(user);
                await Storage.SaveChangesAsync();

                users = await Storage.Products.ToListAsync();
                user = users.Last();
                Assert.IsTrue(user.Name == newName);
                user.Name = oldName;
                Storage.Products.AddOrUpdate(user);
                await Storage.SaveChangesAsync();
                users = await Storage.Products.ToListAsync();
                user = users.Last();
                Assert.IsTrue(user.Name == oldName);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }
    }
}
