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
                    .TrackProperty(x => x.UserType, allUserRoles)
                    .BuildConfiguration(),

                TrackEntityChangesFor<SubscriptionProductEntity>()
                    .TrackProperty(x => x.Title, allUserRoles)
                    .TrackProperty(x => x.Price, allUserRoles)
                    .TrackProperty(x => x.Currency, allUserRoles)
                    .TrackProperty(x => x.DistributorMarkupAsPercent, new [] {UserType.SystemUser, UserType.Distributor})
                    .TrackProperty(x => x.ResellerMarkupAsPercent, new [] {UserType.SystemUser, UserType.Distributor, UserType.Reseller})
                    .BuildConfiguration(),

                TrackEntityChangesFor<OrderEntity>()
                    .TrackProperty(x => x.Comments, allUserRoles)
                    .TrackProperty(x => x.OrderStatus, allUserRoles)
                    .TrackProperty(x => x.PaymentStatus, allUserRoles)
                    .BuildConfiguration(),
            };
            return ConfigList;
        }

        public static TrackingPropertiesConfig<T> TrackEntityChangesFor<T>()
        {
            return new TrackingPropertiesConfig<T>();
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
