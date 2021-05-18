using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HistoryTracking.UI.Web.ApiRequests;

namespace HistoryTracking.UI.Web
{
    public class ServerApiCall
    {
        public OrderApiClient Orders { get; }
        public SubscriptionApiClient Subscriptions { get; }
        public UserApiClient Users { get; }

        public ServerApiCall(UiSettings settings)
        {
            Users = new UserApiClient(settings);
            Subscriptions = new SubscriptionApiClient(settings);
            Orders = new OrderApiClient(settings, Subscriptions, Users);
        }
    }
}
