using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using HistoryTracking.Api.App_Start;

namespace HistoryTracking.Api
{
    [ExceptionHandler]
    [AllowAnonymous]
    public class BaseController : ApiController
    {
    }
}