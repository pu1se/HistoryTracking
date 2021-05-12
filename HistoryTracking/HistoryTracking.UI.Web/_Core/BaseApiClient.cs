using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace HistoryTracking.UI.Web.ApiRequests
{
    public abstract class BaseApiClient
    {
        protected ApiRequestExecuter Api { get; }
        private UiSettings Settings { get; }

        protected BaseApiClient(UiSettings settings)
        {
            Settings = settings;
            Api = new ApiRequestExecuter(Settings.ApiUrl);
        }

        static BaseApiClient()
        {
            ServicePointManager.ServerCertificateValidationCallback +=
                (sender, certificate, chain, sslPolicyErrors) => true;
        }
    }
}
