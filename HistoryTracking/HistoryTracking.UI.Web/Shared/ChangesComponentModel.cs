using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HistoryTracking.BL.Services.Changes.Models;

namespace HistoryTracking.UI.Web.Shared
{
    public class ChangesComponentModel : BasePageModel
    {
        public List<ChangeModel> ChangeList { get; set; } = new List<ChangeModel>();
        public IEnumerable<DropdownItem> UserList { get; set; } = new List<DropdownItem>();
        public ChangeModel SelectedEntityChange { get; set; }
        public GetChangesListModel Filter { get; set; } = new GetChangesListModel{TakeHistoryForLastNumberOfDays = 7};
    }
}
