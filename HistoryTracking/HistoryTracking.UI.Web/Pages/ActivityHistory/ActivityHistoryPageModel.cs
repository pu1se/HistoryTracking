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
        public List<ChangeModel> ChangeList { get; set; } = new List<ChangeModel>();
        public IEnumerable<DropdownItem> UserList { get; set; } = new List<DropdownItem>();
        public IEnumerable<DropdownItem> TrackingEntityNames { get; set; } = new List<DropdownItem>();
        public ChangeModel SelectedEntityChange { get; set; }
        public GetChangesListModel Filter { get; set; } = new GetChangesListModel{TakeHistoryForLastNumberOfDays = 7};
    }
}
