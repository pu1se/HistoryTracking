using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HistoryTracking.DAL.Entities;
using HistoryTracking.DAL.Enums;

namespace HistoryTracking.DAL.TrackChangesLogic.PropertiesTrackingConfigurations
{
    public static class TrackEntitiesChangesConfig
    {
        private static List<TrackEntityInfo> Config { get; set; }

        public static List<TrackEntityInfo> GetConfigsInfo()
        {
            if (Config == null)
            {
                var allUserRoles = EnumHelper.ToArray<UserType>();
                Config = new List<TrackEntityInfo>
                {
                    TrackEntityChangesFor<UserEntity>()
                        .TrackProperty(x => x.Name, allUserRoles)
                        .TrackProperty(x => x.Email, allUserRoles)
                        .TrackProperty(x => x.UserType, allUserRoles)
                        .TrackProperty(x => x.Orders, allUserRoles)
                        .BuildConfiguration(),

                    TrackEntityChangesFor<SubscriptionProductEntity>()
                        .TrackProperty(x => x.Title, allUserRoles)
                        .TrackProperty(x => x.Price, allUserRoles)
                        .TrackProperty(x => x.Currency, allUserRoles)
                        .TrackProperty(x => x.DistributorMarkupAsPercent, allUserRoles)
                        .TrackProperty(x => x.ResellerMarkupAsPercent, allUserRoles)
                        .BuildConfiguration(),

                    TrackEntityChangesFor<OrderEntity>()
                        .TrackProperty(x => x.Comments, allUserRoles)
                        .TrackProperty(x => x.OrderStatus, allUserRoles)
                        .TrackProperty(x => x.PaymentStatus, allUserRoles)
                        .BuildConfiguration(),
                };
            }
            

            return Config;
        }

        public static PropertyChangeConfiguration<T> TrackEntityChangesFor<T>()
        {
            return new PropertyChangeConfiguration<T>();
        }
    }
}
