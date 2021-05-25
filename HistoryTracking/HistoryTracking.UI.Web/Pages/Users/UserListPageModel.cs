﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HistoryTracking.BL.Services.User;

namespace HistoryTracking.UI.Web
{
    public class UserListPageModel : BasePageModel
    {
        public List<UserModel> UserList { get; set; } = new List<UserModel>();
    }
}
