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
                    // todo: rename to DisplayProperty
                    .TrackProperty(x => x.Name, allUserRoles)
                    .TrackProperty(x => x.Email, allUserRoles)
                    .TrackProperty(x => x.UserType, allUserRoles, type => type?.ToString().SplitByCaps())
                    .DisplayRelatedEntity(x => x.Addresses)
                        .TrackRelatedProperty(x => x.HouseAddress, allUserRoles)
                        .EndOfComplexProperty()
                    .BuildConfiguration(),

                TrackEntityChangesFor<UserAddressEntity>(showOnUiAsCategory: false)
                    .SaveRelatedEntityId(x => x.UserId)
                    .BuildConfiguration(),

                TrackEntityChangesFor<SubscriptionProductEntity>(showOnUiAsCategory: true)
                    .TrackProperty(x => x.Title, allUserRoles)
                    .TrackProperty(x => x.Price, allUserRoles)
                    .TrackProperty(x => x.Currency, allUserRoles)
                    .TrackProperty(x => x.DistributorMarkupAsPercent, new [] {UserType.SystemUser, UserType.Distributor})
                    .TrackProperty(x => x.ResellerMarkupAsPercent, new [] {UserType.SystemUser, UserType.Distributor, UserType.Reseller})
                    .AlsoDisplayChangesInParentEntityWithId(x => x.ParentId)
                    .BuildConfiguration(),

                TrackEntityChangesFor<OrderEntity>(showOnUiAsCategory: true)
                    .TrackProperty(x => x.Comments, allUserRoles)
                    .TrackProperty(x => x.OrderStatus, allUserRoles, type => type?.ToString().SplitByCaps())
                    .TrackProperty(x => x.PaymentStatus, allUserRoles, type => type?.ToString().SplitByCaps())
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
