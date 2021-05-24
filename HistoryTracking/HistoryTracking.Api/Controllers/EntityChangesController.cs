using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using HistoryTracking.BL.Services.Changes;

namespace HistoryTracking.Api.Controllers
{
    [RoutePrefix("entity-changes")]
    public class EntityChangesController : BaseController
    {
        private EntityChangeService EntityChangeService { get; }

        public EntityChangesController(EntityChangeService entityChangeService)
        {
            EntityChangeService = entityChangeService;
        }


        [Route("tracking-entity-names")]
        [HttpGet]
        public async Task<IHttpActionResult> GetTrackingEntityNames()
        {
            var result = await EntityChangeService.GetTrackingTableNames();
            return Ok(result);
        }

        [Route("")]
        [HttpGet]
        public async Task<IHttpActionResult> GetEntityChanges()
        {
            var result = await EntityChangeService.GetChanges();
            return Ok(result);
        }
    }
}