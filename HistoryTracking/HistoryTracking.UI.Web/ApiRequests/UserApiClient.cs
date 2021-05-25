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

        public Task<ApiCallDataResult<List<UserModel>>> GetUserListAsync()
        {
            return Api.GetAsync<List<UserModel>>("users");
        }

        public Task<ApiCallResult> AddEditUserAsync(AddEditUserModel model)
        {
            return Api.PutAsync(
                $"users",
                model
            );
        }

        public Task<ApiCallDataResult<UserModel>> GetUserAsync(Guid userId)
        {
            return Api.GetAsync<UserModel>(
                $"users/{userId}"
            );
        }

        public Task<ApiCallDataResult<List<UserType>>> GetUserTypeListAsync()
        {
            return Api.GetAsync<List<UserType>>(
                $"users/user-types"
            );
        }

        public async Task<ApiCallDataResult<List<UserModel>>> GetCustomerListAsync()
        {
            var getUserListResult = await GetUserListAsync();
            if (!getUserListResult.IsSuccess)
            {
                return getUserListResult;
            }

            var customerList = getUserListResult.Data.Where(item => item.UserType == UserType.Customer).ToList();
            return new ApiCallDataResult<List<UserModel>>(customerList);
        }
    }
}
