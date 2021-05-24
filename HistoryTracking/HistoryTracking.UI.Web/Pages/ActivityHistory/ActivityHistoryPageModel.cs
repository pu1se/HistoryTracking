using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HistoryTracking.BL.Services.Changes.Models;
using HistoryTracking.BL.Services.User;
using HistoryTracking.UI.Web.Shared;

namespace HistoryTracking.UI.Web.Pages.ActivityHistory
{
    public class ActivityHistoryPageModel : BasePageModel
    {
        public List<GetChangeModel> ChangeList { get; set; } = new List<GetChangeModel>();
        public IEnumerable<DropdownItem> UserList { get; set; } = new List<DropdownItem>();
        public List<GetEntityNameModel> TrackingEntityNames { get; set; } = new List<GetEntityNameModel>();
        public string FilterEntityName { get; set; }
        public GetChangeModel SelectedEntityChange { get; set; }
    }
}
