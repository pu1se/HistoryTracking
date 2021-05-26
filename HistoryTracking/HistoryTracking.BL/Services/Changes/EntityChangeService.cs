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

        public async Task<List<EntityNameModel>> GetTrackingTableNames()
        {
            var trackingTableNames = await Storage.TrackEntityChanges.GroupBy(e => e.EntityTable).ToListAsync();

            return trackingTableNames.Select(x => new EntityNameModel
            {
                EntityName = x.Key,
                EntityNameForDisplaying = x.Key.SplitByCaps()
            }).ToList();
        }

        public async Task<List<ChangeModel>> GetChanges(GetChangesListModel query = null)
        {
            query = query ?? new GetChangesListModel();
            var getEntityChangesDbQuery = Storage.TrackEntityChanges.AsQueryable();

            if (query.EntityNames.Any())
            {
                getEntityChangesDbQuery = getEntityChangesDbQuery.Where(e => query.EntityNames.Contains(e.EntityTable));
            }
            if (query.UserIds.Any())
            {
                getEntityChangesDbQuery = getEntityChangesDbQuery.Where(e => query.UserIds.Contains(e.ChangedByUserId));
            }
            if (query.TakeHistoryForLastNumberOfDays.HasValue)
            {
                var fromDate = DateTime.UtcNow.AddDays(-query.TakeHistoryForLastNumberOfDays.Value);
                getEntityChangesDbQuery = getEntityChangesDbQuery.Where(e => fromDate <= e.ChangeDateUtc);
            }

            if (query.EntityId.HasValue)
            {
                getEntityChangesDbQuery = getEntityChangesDbQuery.Where(e => e.EntityId == query.EntityId.Value);
            }

            var changes = await getEntityChangesDbQuery

                .Select(e => new ChangeModel
                {
                    Id = e.Id,
                    ChangeDate = e.ChangeDateUtc,
                    ChangeType = e.ChangeType,
                    PropertyChangesAsJson = e.PropertiesChangesWay1,
                    EntityName = e.EntityTable,
                    ChangedByUser = new UserModel
                    {
                        Name = e.ChangedByUser.Name,
                        Email = e.ChangedByUser.Email,
                        UserType = e.ChangedByUser.UserType
                    }
                })
                .OrderByDescending(x => x.ChangeDate)
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
