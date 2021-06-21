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
    public static class TrackingEntitiesConfiguration
    {
        private static List<TrackingEntityInfo> ConfigList { get; set; }

        public static List<TrackingEntityInfo> GetConfigList()
        {
            if (ConfigList != null)
            {
                return ConfigList;
            }

            var allUserRoles = EnumHelper.ToArray<UserType>();
            ConfigList = new List<TrackingEntityInfo>
            {
                TrackEntityChangesFor<UserEntity>()
                    .TrackProperty(x => x.Name, allUserRoles)
                    .TrackProperty(x => x.Email, allUserRoles)
                    .TrackProperty(x => x.UserType, allUserRoles, type => type?.ToString().SplitByCaps())
                    .TrackComplexProperty(x => x.Contacts)
                            .TrackProperty(x => x.Email, allUserRoles)
                            .TrackProperty(x => x.PhoneNumber, allUserRoles)
                            .EndOfComplexProperty()
                    .TrackComplexProperty(x => x.Addresses)
                            .TrackProperty(x => x.City, allUserRoles)
                            .TrackProperty(x => x.HouseAddress, allUserRoles)
                            .EndOfComplexProperty()
                    .BuildConfiguration(),

                TrackEntityChangesFor<SubscriptionProductEntity>()
                    .TrackProperty(x => x.Title, allUserRoles)
                    .TrackProperty(x => x.Price, allUserRoles)
                    .TrackProperty(x => x.Currency, allUserRoles)
                    .TrackProperty(x => x.DistributorMarkupAsPercent, new [] {UserType.SystemUser, UserType.Distributor})
                    .TrackProperty(x => x.ResellerMarkupAsPercent, new [] {UserType.SystemUser, UserType.Distributor, UserType.Reseller})
                    // todo: DisplayInParentEntity()
                    .TrackComplexProperty(x => x.ChildrenSubscriptions)
                            .TrackProperty(x => x.Title, allUserRoles)
                            .TrackProperty(x => x.Price, allUserRoles)
                            .TrackProperty(x => x.Currency, allUserRoles)
                            .TrackProperty(x => x.DistributorMarkupAsPercent, new [] {UserType.SystemUser, UserType.Distributor})
                            .TrackProperty(x => x.ResellerMarkupAsPercent, new [] {UserType.SystemUser, UserType.Distributor, UserType.Reseller})
                            .EndOfComplexProperty()
                    .BuildConfiguration(),

                TrackEntityChangesFor<OrderEntity>()
                    .TrackProperty(x => x.Comments, allUserRoles)
                    .TrackProperty(x => x.OrderStatus, allUserRoles, type => type?.ToString().SplitByCaps())
                    .TrackProperty(x => x.PaymentStatus, allUserRoles, type => type?.ToString().SplitByCaps())
                    .BuildConfiguration(),
            };
            return ConfigList;
        }

        public static TrackPropertiesConfig<T> TrackEntityChangesFor<T>() where T : class
        {
            return new TrackPropertiesConfig<T>();
        }
        
        public static TrackingEntityInfo GetConfigFor(Type searchingEntityType)
        {
            return GetConfigList().FirstOrDefault(x => x.EntityType == searchingEntityType);
        }

        public static TrackingEntityInfo GetConfigFor(Func<TrackingEntityInfo, bool> searchingEntityTypePredicate)
        {
            return GetConfigList().FirstOrDefault(searchingEntityTypePredicate);
        }

        public static TrackingEntityInfo GetConfigFor(string searchingEntityTypeName)
        {
            return GetConfigList().FirstOrDefault(x => x.EntityName == searchingEntityTypeName);
        }
    }
}
