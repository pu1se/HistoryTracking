using System;
using HistoryTracking.DAL;

namespace HistoryTracking.UI.Web
{
    public static class UserManager
    {
        public static string CurrentUser { get; set; } = TestData.SystemUserId.ToString();
    }
}
