using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using HistoryTracking.BL.Services.Changes;
using HistoryTracking.BL.Services.Changes.Models;
using HistoryTracking.DAL;

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
            var result = await EntityChangeService.GetTrackingTableNamesAsync();
            return Ok(result);
        }

        [Route("")]
        [HttpPost]
        public async Task<IHttpActionResult> GetEntityChanges([FromBody]GetChangesListModel model)
        {
            model = model ?? new GetChangesListModel();
            model.FilterByUserRole = UserManager.GetCurrentUserType();
            var result = await EntityChangeService.GetChangesAsync(model);
            return Ok(result);
        }
    }
}