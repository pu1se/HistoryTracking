using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;

namespace HistoryTracking.Api.Controllers
{
    [RoutePrefix("changes")]
    public class ChangesController : ApiController
    {
        [Route("tracking-entity-table-names")]
        [HttpGet]
        public IHttpActionResult GetTrackingEntityTableNames()
        {
            return Ok();
        }

        [HttpGet]
        public IHttpActionResult GetChanges()
        {
            return Ok();
        }
    }
}