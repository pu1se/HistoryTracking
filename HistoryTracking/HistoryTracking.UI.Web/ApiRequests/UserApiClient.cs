using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HistoryTracking.BL.Services.User;
using HistoryTracking.DAL.Enums;

namespace HistoryTracking.UI.Web.ApiRequests
{
    public class UserApiClient : BaseApiClient
    {
        public UserApiClient(UiSettings settings) : base(settings)
        {
        }

        public Task<ApiCallDataResult<List<GetUserModel>>> GetUserListAsync()
        {
            return Api.GetAsync<List<GetUserModel>>("users");
        }

        public Task<ApiCallResult> AddEditUserAsync(AddEditUserModel model)
        {
            return Api.PutAsync(
                $"users",
                model
            );
        }

        public Task<ApiCallDataResult<GetUserModel>> GetUserAsync(Guid userId)
        {
            return Api.GetAsync<GetUserModel>(
                $"users/{userId}"
            );
        }

        public Task<ApiCallDataResult<List<UserType>>> GetUserTypeListAsync()
        {
            return Api.GetAsync<List<UserType>>(
                $"users/user-types"
            );
        }
    }
}
