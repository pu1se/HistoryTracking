using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using HistoryTracking.BL.Services.Changes;

namespace HistoryTracking.Api.Controllers
{
    [RoutePrefix("changes")]
    public class ChangesController : ApiController
    {
        private ChangeService ChangeService { get; }

        public ChangesController(ChangeService changeService)
        {
            ChangeService = changeService;
        }


        [Route("tracking-entity-table-names")]
        [HttpGet]
        public IHttpActionResult GetTrackingEntityTableNames()
        {
            var result = ChangeService.GetTrackingTableNames();
            return Ok(result);
        }

        [HttpGet]
        public IHttpActionResult GetChanges()
        {
            return Ok();
        }
    }
}