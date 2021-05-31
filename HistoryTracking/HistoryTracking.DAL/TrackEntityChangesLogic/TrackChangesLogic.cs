using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using HistoryTracking.DAL.Entities;
using HistoryTracking.DAL.TrackEntityChangesLogic;
using HistoryTracking.DAL.TrackEntityChangesLogic.Base;
using HistoryTracking.DAL.TrackEntityChangesLogic.PropertiesTrackingConfigurations;

namespace HistoryTracking.DAL
{
    public static class TrackChangesLogic
    {
        public static TrackEntityChange GetTrackEntityChangeRecord(
            DataContext dataContext, 
            DbEntityEntry dbEntry,
            TrackingEntityInfo trackingEntityConfig)
        {
            var supportedEntryState = new List<EntityState> { EntityState.Modified, EntityState.Added, EntityState.Deleted };
            if (!supportedEntryState.Contains(dbEntry.State))
            {
                return null;
            }

            var changedDateUtc = DateTime.UtcNow;
            if (dbEntry.Entity is BaseEntity baseEntity)
            {
                changedDateUtc = baseEntity.UpdatedDateUtc;
            }

            var trackEntityChange = new TrackEntityChange
            {
                Id = Guid.NewGuid(),
                EntityTable = trackingEntityConfig.EntityName,
                EntityId = GetPrimaryKeyId(dataContext, dbEntry),
                ChangeType = dbEntry.State.ToString(),
                ChangeDateUtc = changedDateUtc,
                EntityAfterChangeSnapshot = dbEntry.State != EntityState.Deleted ? dbEntry.Entity.ToJson() : null,
                PropertiesChangesWay1 = GetPropertyChangesWay1.For(dbEntry, trackingEntityConfig).ToJson(),
                //PropertiesChangesWay2 = GetPropertyChangesWay2.For(dbEntry).ToJson(),
                ChangedByUserId = UserManager.GetCurrentUserId(),
            };
            return trackEntityChange;
        }

        private static Guid? GetPrimaryKeyId(DataContext dataContext, DbEntityEntry dbEntry)
        {
            try
            {
                if (dbEntry.Entity is BaseEntity baseEntity)
                {
                    return baseEntity.Id;
                }

                var objectStateEntry = ((IObjectContextAdapter)dataContext).ObjectContext.ObjectStateManager.GetObjectStateEntry(dbEntry.Entity);
                var entityId = objectStateEntry.EntityKey.EntityKeyValues?[0].Value.ToString();
                if (!entityId.IsNullOrEmpty())
                {
                    return new Guid(entityId);    
                }
                
                return null;
            }
            catch (Exception e)
            {
                Console.Write(e.ToJson());
                return null;
            }
        }
    }
}