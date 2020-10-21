namespace HistoryTracking.UI.Web.ApiRequests
{
    public class ApiCallDataResult<T> : ApiCallResult
    {
        public T Data { get; }

        public ApiCallDataResult(T data, string errorMessage = null) : base(errorMessage)
        {
            Data = errorMessage.IsNullOrEmpty() ? data : default(T);
        }
    }
}
