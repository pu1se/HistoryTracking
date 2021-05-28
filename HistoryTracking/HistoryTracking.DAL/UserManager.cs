using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using HistoryTracking.DAL.Enums;

namespace HistoryTracking.DAL
{
    public static class UserManager
    {
        public static Guid GetCurrentUserId()
        {
            var userIdAsString = HttpContext.Current?.Request.Cookies["CurrentUserId"];
            var userId = userIdAsString != null ? new Guid(userIdAsString.Value) : TestData.SystemUserId;
            return userId;
        }

        public static UserType GetCurrentUserType()
        {
            var userTypeAsString = HttpContext.Current?.Request.Cookies["CurrentUserType"]?.Value;
            var currentUserType = userTypeAsString != null ? EnumHelper.Parse<UserType>(userTypeAsString) : UserType.SystemUser;
            return currentUserType;
        }
    }
}
