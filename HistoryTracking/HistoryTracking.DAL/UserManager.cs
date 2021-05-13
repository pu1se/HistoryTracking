using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace HistoryTracking.DAL
{
    public static class UserManager
    {
        public static Guid GetCurrentUser()
        {
            var userIdAsString = HttpContext.Current.Request.Cookies["CurrentUser"];
            var userId = userIdAsString != null ? new Guid(userIdAsString.Value) : TestData.SystemUserId;
            return userId;
        }
    }
}
