using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace HistoryTracking.UI.Web.ApiRequests
{
    public class UiSettings
    {
        public UiSettings(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        public string ApiUrl => Configuration["ApiUrl"];
    }
}
