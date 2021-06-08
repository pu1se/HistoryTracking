using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HistoryTracking.BL.Services.Changes;
using HistoryTracking.BL.Services.Changes.Models;
using HistoryTracking.DAL.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HistoryTracking.Tests
{
    [TestClass]
    public class EntityChangeServiceTests : BaseServiceTests<EntityChangeService>
    {
        [TestMethod]
        public async Task CheckGetTableNamesTest()
        {
            var tableNames = await Service.GetTrackingTableNames();

            Assert.IsTrue(tableNames.Any());
            Assert.IsTrue(tableNames.First().EntityName.IsNullOrEmpty() == false);
            Assert.IsTrue(tableNames.First().EntityNameForDisplaying.IsNullOrEmpty() == false);
        }

        [TestMethod]
        public async Task CheckGetChangesAsync()
        {
            try
            {
                var changes = await Service.GetChanges(new GetChangesListModel{FilterByUserRole = UserType.SystemUser});

                foreach (var change in changes)
                {
                    Assert.IsTrue(change.ChangeDate > DateWhenProjectWasStarted);
                    Assert.IsTrue(change.ChangeType.IsNullOrEmpty() == false);
                    foreach (var propertyChange in change.PropertyChanges)
                    {
                        Assert.IsTrue(propertyChange.PropertyName.IsNullOrEmpty() == false);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [TestMethod]
        public async Task CheckUserRoleFiltration()
        {
            var allChanges = await Service.GetChanges(new GetChangesListModel{FilterByUserRole = UserType.SystemUser});
            var allPropertyChanges = allChanges.SelectMany(x => x.PropertyChanges);

            var notAllChanges = await Service.GetChanges(new GetChangesListModel{FilterByUserRole = UserType.Customer});
            var notAllPropertyChanges = notAllChanges.SelectMany(x => x.PropertyChanges);

            Assert.IsTrue(allPropertyChanges.Count() > notAllPropertyChanges.Count());
        }
    }
}
