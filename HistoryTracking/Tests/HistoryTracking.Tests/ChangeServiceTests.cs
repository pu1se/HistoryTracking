using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HistoryTracking.BL.Services.Changes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HistoryTracking.Tests
{
    [TestClass]
    public class ChangeServiceTests : BaseServiceTests<ChangeService>
    {
        [TestMethod]
        public async Task CheckGetTableNamesTest()
        {
            var tableNames = await Service.GetTrackingTableNames();

            Assert.IsTrue(tableNames.Any());
            Assert.IsTrue(tableNames.First().IsNullOrEmpty() == false);
        }

        [TestMethod]
        public async Task CheckGetChangesAsync()
        {
            try
            {
                var changes = await Service.GetChanges();

                foreach (var change in changes)
                {
                    Assert.IsTrue(change.ChangeDate > DateWhenProjectWasStarted);
                    Assert.IsTrue(change.ChangeType.IsNullOrEmpty() == false);
                    foreach (var propertyChange in change.PropertyChanges)
                    {
                        Assert.IsTrue(propertyChange.PropertyName.IsNullOrEmpty() == false);
                        Assert.IsTrue(propertyChange.NewValue.IsNullOrEmpty() == false || propertyChange.OldValue.IsNullOrEmpty() == false);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
