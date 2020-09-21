using System;
using System.Data.Entity;
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
    }
}
