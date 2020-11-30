using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Filters;
using HistoryTracking.BL;

namespace HistoryTracking.Api.App_Start
{
    public class ExceptionHandlerAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            if (context.Exception == null)
            {
                return;
            }
            
            switch (context.Exception)
            {

                case ValidationException validationException:

                    context.Response = new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        Content = validationException.Message.ToJsonStringContent()
                    };

                    break;


                case Exception genericException:

                    var error500 = new
                    {
                        ErrorMessage = "Server error",
                        Exception = genericException
                    };
                    context.Response = new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.InternalServerError,
                        Content = error500.ToJsonStringContent()
                    };
                    break;
            }
        }
    }
}