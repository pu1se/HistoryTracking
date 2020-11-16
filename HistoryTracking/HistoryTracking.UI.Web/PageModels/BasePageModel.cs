using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HistoryTracking.UI.Web.PageModels
{
    public class BasePageModel
    {
        public string ErrorMessage { get; set; }
        public string LogMessage { get; set; }
        public bool IsActionSuccess { get; set; }
        public bool IsReady { get; set; }
    }
}
