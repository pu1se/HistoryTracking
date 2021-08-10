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
        private EntityChangesService EntityChangesService { get; }

        public EntityChangesController(EntityChangesService entityChangesService)
        {
            EntityChangesService = entityChangesService;
        }


        [Route("tracking-entity-names")]
        [HttpGet]
        public IHttpActionResult GetTrackingEntityNames()
        {
            var result = EntityChangesService.GetTrackingTableNames();
            return Ok(result);
        }

        [Route("")]
        [HttpPost]
        public async Task<IHttpActionResult> GetEntityChanges([FromBody]GetChangesListModel model)
        {
            model = model ?? new GetChangesListModel();
            model.FilterByUserRole = UserManager.GetCurrentUserType();
            var result = await EntityChangesService.GetChangesAsync(model);
            return Ok(result);
        }
    }
}