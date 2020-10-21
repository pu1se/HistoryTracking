using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using HistoryTracking.BL.Services;
using HistoryTracking.BL.Services.User;

namespace HistoryTracking.Api.Controllers
{
    [Route("users")]
    public class UsersController : BaseController
    {
        private UserService UserService { get; }

        public UsersController(UserService userService)
        {
            UserService = userService;
        }

        [HttpGet]
        public async Task<List<GetUserModel>> GetUserList()
        {
            return await UserService.GetList();
        }
    }
}