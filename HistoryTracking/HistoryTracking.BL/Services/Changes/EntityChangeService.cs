using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HistoryTracking.BL.Services.Changes.Models;
using HistoryTracking.BL.Services.User;
using HistoryTracking.DAL;
using Newtonsoft.Json;

namespace HistoryTracking.BL.Services.Changes
{
    public class EntityChangeService : BaseService
    {
        public EntityChangeService(DataContext storage) : base(storage)
        {
        }

        public async Task<List<GetEntityNameModel>> GetTrackingTableNames()
        {
            var trackingTableNames = await Storage.TrackEntityChanges.GroupBy(e => e.EntityTable).ToListAsync();

            return trackingTableNames.Select(x => new GetEntityNameModel
            {
                EntityName = x.Key,
                EntityNameForDisplaying = x.Key.SplitByCaps()
            }).ToList();
        }

        public async Task<List<GetChangeModel>> GetChanges()
        {
            var changes = await Storage.TrackEntityChanges
                .Select(e => new GetChangeModel
                {
                    Id = e.Id,
                    ChangeDate = e.ChangeDateUtc,
                    ChangeType = e.ChangeType,
                    PropertyChangesAsJson = e.PropertiesChanges,
                    EntityName = e.EntityTable,
                    ChangedByUser = new GetUserModel
                    {
                        Name = e.ChangedByUser.Name,
                        Email = e.ChangedByUser.Email,
                        UserType = e.ChangedByUser.UserType
                    }
                })
                .ToListAsync();
            changes.ForEach(x =>
            {
                x.PropertyChanges = JsonConvert.DeserializeObject<List<PropertyChangeDescription>>(x.PropertyChangesAsJson);
                x.EntityName = x.EntityName.SplitByCaps();
            });

            return changes;
        }
    }
}
