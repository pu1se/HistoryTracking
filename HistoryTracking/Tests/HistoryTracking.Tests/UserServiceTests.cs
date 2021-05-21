using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HistoryTracking.BL.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HistoryTracking.Tests
{
    [TestClass]
    public class UserServiceTests : BaseServiceTests<UserService>
    {
        [TestMethod]
        public async Task CheckGetUserList()
        {
            try
            {
                var users = await Service.GetList();

                Assert.IsTrue(users != null);
                Assert.IsTrue(users.Any());
                Assert.IsTrue(users.First().Name.IsNullOrEmpty() == false);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
