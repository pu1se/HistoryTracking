using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HistoryTracking.DAL.Entities;
using HistoryTracking.DAL.Enums;
using HistoryTracking.DAL.TrackEntityChangesLogic.PropertiesTrackingConfigurations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HistoryTracking.Tests
{
    [TestClass]
    public class CreateTrackEntitiesChangesConfigurationTests
    {
        [TestMethod]
        public void TestConfigurationForUserEntity()
        {
            var allUserRoles = EnumHelper.ToArray<UserType>();
            var trackingEntityConfig = ConfigurationOfTrackedEntities
                .TrackEntityChangesFor<UserEntity>()
                .TrackProperty(x => x.Name, allUserRoles)
                .TrackProperty(x => x.Email, allUserRoles.ExceptItem(UserType.Customer))
                .TrackProperty(x => x.UserType, allUserRoles.ExceptItem(UserType.Customer, UserType.Reseller))
                .TrackProperty(x => x.Orders, new []{UserType.SystemUser})
                .BuildConfiguration();

            Assert.IsTrue(trackingEntityConfig.EntityName.IsNullOrEmpty() == false);
            var nameProperty = trackingEntityConfig.PropertyList.FirstOrDefault(x => x.Name == nameof(UserEntity.Name));
            Assert.IsTrue(nameProperty != null);
            Assert.IsTrue(nameProperty.Name == nameof(UserEntity.Name));
            Assert.IsTrue(nameProperty.IsVisibleForUserRoles.SequenceEqual(allUserRoles));

            Assert.IsTrue(trackingEntityConfig.EntityName.IsNullOrEmpty() == false);
            var orderProperty = trackingEntityConfig.PropertyList.FirstOrDefault(x => x.Name == nameof(UserEntity.Orders));
            Assert.IsTrue(orderProperty != null);
            Assert.IsTrue(orderProperty.IsVisibleForUserRoles.Count == 1);
            Assert.IsTrue(orderProperty.IsVisibleForUserRoles.First() == UserType.SystemUser);
            Assert.IsTrue(orderProperty.Name == nameof(UserEntity.Orders));
        }
    }
}
