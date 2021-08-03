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

        public Task<ApiCallDataResult<List<TrackedEntityNameModel>>> GetTrackingEntityNames()
        {
            return Api.GetAsync<List<TrackedEntityNameModel>>("entity-changes/tracking-entity-names");
        }

        public Task<ApiCallDataResult<List<ChangeModel>>> GetEntityChanges(GetChangesListModel model = null)
        {
            return Api.PostAsync<List<ChangeModel>>("entity-changes", model);
        }
    }
}
