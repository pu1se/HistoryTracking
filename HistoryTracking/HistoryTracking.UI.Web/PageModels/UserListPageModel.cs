using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HistoryTracking.BL.Services.User;

namespace HistoryTracking.UI.Web.PageModels
{
    public class UserListPageModel : BasePageModel
    {
        public List<GetUserModel> UserList { get; set; } = new List<GetUserModel>();
    }
}
