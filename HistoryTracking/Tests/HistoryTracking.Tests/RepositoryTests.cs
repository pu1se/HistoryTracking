using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using HistoryTracking.DAL;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HistoryTracking.Tests
{
    [TestClass]
    public class RepositoryTests
    {
        [TestMethod]
        public async Task CheckDataContextIsWorking()
        {
            using (var context = new DataContext())
            {
                var list = await context.Users.ToListAsync();
                Assert.IsTrue(list != null);
            }
        }
    }
}
