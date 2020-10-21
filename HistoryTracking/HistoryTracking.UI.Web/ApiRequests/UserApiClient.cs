using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HistoryTracking.BL.Services.User;

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

        /*public Task<ApiCallResult> AddEditConfigAsync(AddEditConfigModel command)
        {
            return Api.PutAsync(
                $"exchange/organizations/{command.OrganizationId}/configs",
                command
            );
        }

        public Task<ApiCallResult> DeleteConfigAsync(AddEditConfigModel command)
        {
            return Api.DeleteAsync(
                $"exchange/organizations/{command.OrganizationId}/configs",
                command
            );
        }

        public Task<ApiCallDataResult<List<RateResponse>>> GetRatesAsync(Guid organizationId)
        {
            return Api.GetAsync<List<RateResponse>>(
                $"exchange/organizations/{organizationId}/rates"
            );
        }

        public void FillConfigWithRate(ConfigViewModel config, List<RateResponse> rateList)
        {
            var rate = rateList.FirstOrDefault(
                x => 
                    x.FromCurrency == config.FromCurrency &&
                    x.ToCurrency == config.ToCurrency
            );

            if (rate == null)
            {
                return;
            }

            config.ExchangeRate = rate.ExchangeRate;
        }*/
    }
}
