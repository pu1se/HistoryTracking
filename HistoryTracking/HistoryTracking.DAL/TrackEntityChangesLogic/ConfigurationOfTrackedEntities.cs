using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using HistoryTracking.DAL.Entities;
using HistoryTracking.DAL.Enums;

namespace HistoryTracking.DAL.TrackEntityChangesLogic.PropertiesTrackingConfigurations
{
    public static class ConfigurationOfTrackedEntities
    {
        private static List<TrackedEntityConfig> ConfigList { get; set; }

        public static List<TrackedEntityConfig> GetConfigList()
        {
            if (ConfigList != null)
            {
                return ConfigList;
            }

            var allUserRoles = EnumHelper.ToArray<UserType>();
            ConfigList = new List<TrackedEntityConfig>
            {
                TrackEntityChangesFor<UserEntity>(showOnUiAsCategory: true)
                    .ShowOnUiChangesInProperty(x => x.Name, allUserRoles)
                    .ShowOnUiChangesInProperty(x => x.Email, allUserRoles)
                    .ShowOnUiChangesInProperty(x => x.UserType, allUserRoles, type => type?.ToString().SplitByCaps())
                    .MergeDisplayingOfRelatedEntity(x => x.Addresses)
                        .ShowOnUiChangesInRelatedProperty(x => x.HouseAddress, allUserRoles)
                        .ShowOnUiChangesInRelatedProperty(x => x.City, allUserRoles)
                        .EndOfRelatedEntity()
                    .MergeDisplayingOfRelatedEntity(x => x.Contacts)
                        .ShowOnUiChangesInRelatedProperty(x => x.PhoneNumber, allUserRoles)
                        .ShowOnUiChangesInRelatedProperty(x => x.Email, allUserRoles)
                        .EndOfRelatedEntity()
                    .BuildConfiguration(),

                TrackEntityChangesFor<UserAddressEntity>(showOnUiAsCategory: false)
                    .SaveRelatedEntityId(x => x.UserId)
                    .BuildConfiguration(),

                TrackEntityChangesFor<UserContactEntity>(showOnUiAsCategory: false)
                    .SaveRelatedEntityId(x => x.UserId)
                    .BuildConfiguration(),

                TrackEntityChangesFor<SubscriptionProductEntity>(showOnUiAsCategory: true)
                    .ShowOnUiChangesInProperty(x => x.Title, allUserRoles)
                    .ShowOnUiChangesInProperty(x => x.Price, allUserRoles)
                    .ShowOnUiChangesInProperty(x => x.Currency, allUserRoles)
                    .ShowOnUiChangesInProperty(x => x.DistributorMarkupAsPercent, new [] {UserType.SystemUser, UserType.Distributor})
                    .ShowOnUiChangesInProperty(x => x.ResellerMarkupAsPercent, new [] {UserType.SystemUser, UserType.Distributor, UserType.Reseller})
                    .AlsoDisplayChangesInParentEntityWithId(x => x.ParentId)
                    .BuildConfiguration(),

                TrackEntityChangesFor<OrderEntity>(showOnUiAsCategory: true)
                    .ShowOnUiChangesInProperty(x => x.Comments, allUserRoles)
                    .ShowOnUiChangesInProperty(x => x.OrderStatus, allUserRoles, type => type?.ToString().SplitByCaps())
                    .ShowOnUiChangesInProperty(x => x.PaymentStatus, allUserRoles, type => type?.ToString().SplitByCaps())
                    .BuildConfiguration(),
            };
            return ConfigList;
        }

        public static TrackedEntityConfigBuilder<T> TrackEntityChangesFor<T>(bool showOnUiAsCategory) where T : class
        {
            return new TrackedEntityConfigBuilder<T>(showOnUiAsCategory);
        }
        
        public static TrackedEntityConfig GetConfigFor(Type searchingEntityType)
        {
            return GetConfigList().FirstOrDefault(x => x.EntityType == searchingEntityType);
        }

        public static TrackedEntityConfig GetConfigFor(Func<TrackedEntityConfig, bool> searchingEntityTypePredicate)
        {
            return GetConfigList().FirstOrDefault(searchingEntityTypePredicate);
        }

        public static TrackedEntityConfig GetConfigFor(string searchingEntityTypeName)
        {
            return GetConfigList().FirstOrDefault(x => x.EntityName == searchingEntityTypeName);
        }
    }
}
