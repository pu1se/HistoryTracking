using System;
using HistoryTracking.DAL;
using HistoryTracking.DAL.Enums;

namespace HistoryTracking.UI.Web
{
    public static class UserManager
    {
        public static string CurrentUserId { get; set; } = TestData.SystemUserId.ToString();
        public static UserType CurrentUserType { get; set; } = UserType.SystemUser;
    }
}
