using System.Net;
using System.Threading.Tasks;
using RestSharp;

namespace HistoryTracking.UI.Web.ApiRequests
{
    public class ApiRequestExecuter
    {
        public ApiRequestExecuter(string baseUrl)
        {
            this.BaseUrl = baseUrl;
        }

        private string BaseUrl { get; }

        private RestRequest GenerateRequest(Method httpMethod, string path, object data)
        {
            var request = new RestRequest(path, httpMethod);

            if (data != null)
            {
                //todo: check if http get request with variables works correct
                request.AddJsonBody(data);
            }

            return request;
        }

        private async Task<ApiCallResult> SendRequestAsync(Method httpMethod, string path, object data)
        {
            if (path.Contains(this.BaseUrl))
            {
                path = path.Replace(this.BaseUrl, string.Empty);
            }

            var restClient = new RestClient(this.BaseUrl);
            var request = GenerateRequest(httpMethod, path, data);
            request.AddCookie("CurrentUser", UserManager.CurrentUser);

            var response = await restClient.ExecuteTaskAsync(request);
            var callResult = ApiErrorHandler.HandleResponse(response);
            return callResult;
        }

        private async Task<ApiCallDataResult<T>> SendRequestAsync<T>(Method httpMethod, string path, object data)
        {
            if (path.Contains(this.BaseUrl))
            {
                path = path.Replace(this.BaseUrl, string.Empty);
            }

            var restClient = new RestClient(this.BaseUrl);
            var request = GenerateRequest(httpMethod, path, data);
            request.AddCookie("CurrentUser", UserManager.CurrentUser);

            var response = await restClient.ExecuteTaskAsync<T>(request);
            var callResult = ApiErrorHandler.HandleResponse(response);
            return new ApiCallDataResult<T>(response.Data, callResult.ErrorMessage);
        }

        public Task<ApiCallDataResult<TResult>> GetAsync<TResult>(string path, object data = null) where TResult : class, new()
        {
            return SendRequestAsync<TResult>(Method.GET, path, data);
        }

        public Task<ApiCallResult> PutAsync(string path, object data)
        {
            return SendRequestAsync(Method.PUT, path, data);
        }

        public Task<ApiCallResult> DeleteAsync(string path, object data)
        {
            return SendRequestAsync(Method.DELETE, path, data);
        }

        public Task<ApiCallResult> PostAsync(string path, object data = null)
        {
            return SendRequestAsync(Method.POST, path, data);
        }

        public Task<ApiCallDataResult<TResult>> PostAsync<TResult>(string path, object data) where TResult : class, new()
        {
            return SendRequestAsync<TResult>(Method.POST, path, data);
        }

        private class TokenModel
        {
            public string access_token { get; set; }
            public string token_type { get; set; }
        }
    }
}
