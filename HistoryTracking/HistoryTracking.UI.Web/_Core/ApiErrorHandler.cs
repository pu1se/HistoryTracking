using System;
using RestSharp;

namespace HistoryTracking.UI.Web.ApiRequests
{
    public static class ApiErrorHandler
    {
        public static ApiCallResult HandleResponse(IRestResponse response)
        {
            var statusCode = (int)response.StatusCode;

            if (statusCode == 200)
            {
                return new ApiCallResult();
            }

            if (statusCode == 404)
            {
                return new ApiCallResult("Not Found");
            }

            if (statusCode == 400)
            {
                return new ApiCallResult($"Validation error: {response.Content.Replace(Environment.NewLine, "<br/>")}");
            }
            

            if (statusCode >= 500)
            {
                return new ApiCallResult($"{response.Content.Replace(Environment.NewLine, "<br/>")}");
            }

            return new ApiCallResult($"Error Uri: {response.Request.Resource}. Response: {response.Content.Replace(Environment.NewLine, "<br/>")}");
        }
    }
}
