using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HistoryTracking.BL.Services.User;
using HistoryTracking.DAL.Enums;

namespace HistoryTracking.UI.Web.Pages.Users
{
    public class UserAddEditPageModel : BasePageModel
    {
        public bool IsAddingNewOne { get; set; }
        public List<UserType> UserTypeList { get; set; }
        public AddEditUserModel UserConfig { get; set; }
    }
}
