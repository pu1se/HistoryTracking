using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HistoryTracking.BL.Services.User;
using HistoryTracking.UI.Web.Shared;

namespace HistoryTracking.UI.Web.Pages.ActivityHistory
{
    public class ActivityHistoryPageModel : BasePageModel
    {
        public List<string> ChangeList { get; set; } = new List<string>();
        public IEnumerable<DropdownItem> UserList { get; set; }
    }
}
