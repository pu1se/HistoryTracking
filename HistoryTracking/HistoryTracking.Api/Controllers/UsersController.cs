using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using HistoryTracking.BL.Services;
using HistoryTracking.BL.Services.User;
using HistoryTracking.DAL.Enums;

namespace HistoryTracking.Api.Controllers
{
    [RoutePrefix("users")]
    public class UsersController : BaseController
    {
        private UserService UserService { get; }

        public UsersController(UserService userService)
        {
            UserService = userService;
        }

        [HttpGet]
        [Route("")]
        public async Task<List<GetUserModel>> GetUserList()
        {
            return await UserService.GetList();
        }

        [HttpGet]
        [Route("{userId:guid}")]
        public async Task<GetUserModel> GetList(Guid userId)
        {
            return await UserService.GetItem(userId);
        }

        [HttpGet]
        [Route("user-types")]
        public List<UserType> GetUserTypes()
        {
            return UserService.GetUserTypes();
        }

        [HttpPut]
        [Route("")]
        public IHttpActionResult AddEditUser([FromBody] AddEditUserModel model)
        {
            UserService.AddEditUser(model);
            return Ok();
        }
    }
}