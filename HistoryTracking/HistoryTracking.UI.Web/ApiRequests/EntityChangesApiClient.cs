using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HistoryTracking.BL.Services.Changes.Models;

namespace HistoryTracking.UI.Web.ApiRequests
{
    public class EntityChangesApiClient : BaseApiClient
    {
        public EntityChangesApiClient(UiSettings settings) : base(settings)
        {
        }

        public Task<ApiCallDataResult<List<GetEntityNameModel>>> GetTrackingEntityNames()
        {
            return Api.GetAsync<List<GetEntityNameModel>>("entity-changes/tracking-entity-names");
        }

        public Task<ApiCallDataResult<List<GetChangeModel>>> GetEntityChanges()
        {
            return Api.GetAsync<List<GetChangeModel>>("entity-changes");
        }
    }
}
