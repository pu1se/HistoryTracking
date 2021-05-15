using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using HistoryTracking.DAL.Entities;
using HistoryTracking.DAL.Enums;
using HistoryTracking.DAL.Migrations;

namespace HistoryTracking.DAL
{
    public class DataContext : DbContext
    {
        private const String DefaultConnectionStringName = "DefaultConnection";
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<SubscriptionProductEntity> SubscriptionProducts { get; set; }
        public DbSet<OrderEntity> Orders { get; set; }
        public DbSet<TrackEntityChange> TrackEntityChanges { get; set; }


        public DataContext() : base(DefaultConnectionStringName)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<DataContext, Configuration>());
            Configuration.LazyLoadingEnabled = false;
        }
        
        

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Ignore(new[] { typeof(BaseEntity) });

            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            modelBuilder.Properties<DateTime>().Configure(c => c.HasColumnType("datetime2"));

            modelBuilder.Entity<OrderEntity>()
                .HasMany(e => e.SubscriptionProducts)
                .WithMany(e => e.Orders)
                .Map(config =>
                {
                    config.MapLeftKey("SubscriptionProductId");
                    config.MapRightKey("OrderId");
                    config.ToTable("SubscriptionProducts_Orders");
                });

            modelBuilder.Entity<UserEntity>()
                .HasMany(e => e.SubscriptionProducts)
                .WithMany(e => e.OwnerUsers)
                .Map(config =>
                {
                    config.MapLeftKey("SubscriptionProductId");
                    config.MapRightKey("UserId");
                    config.ToTable("SubscriptionProducts_Users");
                });

            modelBuilder.Entity<UserEntity>()
                .HasMany(e => e.Orders)
                .WithRequired(e => e.CustomerUser)
                .WillCascadeOnDelete(false);

            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            ProcessChangesBeforeSave();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync()
        {
            ProcessChangesBeforeSave();
            return base.SaveChangesAsync();
        }

        private void ProcessChangesBeforeSave()
        {
            var changes = this.ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);
            var now = DateTime.UtcNow;

            foreach (var entry in this.ChangeTracker.Entries<BaseEntity>())
            {
                var entity = entry.Entity;

                if (entry.State == EntityState.Added)
                {
                    if (entity.Id == Guid.Empty)
                    {
                        entity.Id = Guid.NewGuid();
                    }
                    entity.CreatedDateUtc = now;
                    entity.CreatedByUserId = UserManager.GetCurrentUser();
                }

                if (entry.State == EntityState.Modified)
                {
                    entry.Property(nameof(BaseEntity.CreatedDateUtc)).IsModified = false;
                    entry.Property(nameof(BaseEntity.CreatedByUserId)).IsModified = false;
                }

                if (entry.State == EntityState.Added || entry.State == EntityState.Modified)
                {
                    entity.UpdatedDateUtc = now;
                    entity.UpdatedByUserId = UserManager.GetCurrentUser();
                    entry.Property(nameof(BaseEntity.UpdatedDateUtc)).IsModified = true;
                    entry.Property(nameof(BaseEntity.UpdatedByUserId)).IsModified = true;

                    if (entity is IHistoryTracking historyTrackingEntity)
                    {
                        var trackEntityChange = GetTrackEntityChange(entry);
                        TrackEntityChanges.Add(trackEntityChange);
                    }
                }
            }
        }

        private TrackEntityChange GetTrackEntityChange(DbEntityEntry dbEntry)
        {
            var tableAttr = dbEntry.Entity.GetType().GetCustomAttributes(typeof(TableAttribute), true).SingleOrDefault() as TableAttribute;
            var entityTableName = tableAttr != null ? tableAttr.Name : dbEntry.Entity.GetType().Name;

            var keyName = dbEntry.Entity.GetType().GetProperties().FirstOrDefault(p => p.GetCustomAttributes(typeof(KeyAttribute), false).Count() > 0)?.Name;
            var entityId = keyName != null ? dbEntry.CurrentValues.GetValue<object>(keyName).ToString() : string.Empty;

            var trackEntityChange = new TrackEntityChange
            {
                Id = Guid.NewGuid(),
                EntityTable = entityTableName,
                EntityId = entityId,
                EventType = dbEntry.State.ToString(),
                EventDateUtc = DateTime.UtcNow,
                NewValue = dbEntry.Entity.ToJson()
            };

            if (dbEntry.State == EntityState.Modified)
            {
                trackEntityChange.OldValue = GetOriginalEntity(dbEntry.OriginalValues, dbEntry.Entity.GetType()).ToJson();
            }

            return trackEntityChange;
        }

        object GetOriginalEntity(DbPropertyValues originalValues, Type tEntity)
        {
            var originalEntity = Activator.CreateInstance(tEntity, true);
            foreach (var propertyName in originalValues.PropertyNames)
            {
                var property = tEntity.GetProperty(propertyName);
                var value = originalValues[propertyName];
                if (!(value is DbPropertyValues))
                {
                    property.SetValue(originalEntity, value);
                }
                /*else
                {
                    // nested entity
                    property.SetValue(originalEntity, GetOriginalEntity(value as DbPropertyValues, property.PropertyType));
                }*/
            }

            return originalEntity;
        }

        /*private List<AuditLog> GetAuditRecordsForChange(DbEntityEntry dbEntry)
        {
            List<AuditLog> result = new List<AuditLog>();

            DateTime changeTime = DateTime.UtcNow;

            // Get primary key value (If you have more than one key column, this will need to be adjusted)
            var keyNames = dbEntry.Entity.GetType().GetProperties().Where(p => p.GetCustomAttributes(typeof(KeyAttribute), false).Count() > 0).ToList();

            string keyName = keyNames[0].Name; //dbEntry.Entity.GetType().GetProperties().Single(p => p.GetCustomAttributes(typeof(KeyAttribute), false).Count() > 0).Name;

            if (dbEntry.State == EntityState.Added)
            {
                // For Inserts, just add the whole record
                // If the entity implements IDescribableEntity, use the description from Describe(), otherwise use ToString()

                foreach (string propertyName in dbEntry.CurrentValues.PropertyNames)
                {
                    result.Add(new AuditLog()
                    {
                        AuditLogId = Guid.NewGuid(),
                        UserId = userId,
                        EventDateUTC = changeTime,
                        EventType = "A",    // Added
                        TableName = tableName,
                        RecordId = dbEntry.CurrentValues.GetValue<object>(keyName).ToString(),
                        ColumnName = propertyName,
                        NewValue = dbEntry.CurrentValues.GetValue<object>(propertyName) == null ? null : dbEntry.CurrentValues.GetValue<object>(propertyName).ToString()
                    }
                            );
                }
            }
            else if (dbEntry.State == EntityState.Modified)
            {
                foreach (string propertyName in dbEntry.OriginalValues.PropertyNames)
                {
                    // For updates, we only want to capture the columns that actually changed
                    if (!object.Equals(dbEntry.OriginalValues.GetValue<object>(propertyName), dbEntry.CurrentValues.GetValue<object>(propertyName)))
                    {
                        result.Add(new AuditLog()
                        {
                            AuditLogId = Guid.NewGuid(),
                            UserId = userId,
                            EventDateUTC = changeTime,
                            EventType = "M",    // Modified
                            TableName = tableName,
                            RecordId = dbEntry.OriginalValues.GetValue<object>(keyName).ToString(),
                            ColumnName = propertyName,
                            OriginalValue = dbEntry.OriginalValues.GetValue<object>(propertyName) == null ? null : dbEntry.OriginalValues.GetValue<object>(propertyName).ToString(),
                            NewValue = dbEntry.CurrentValues.GetValue<object>(propertyName) == null ? null : dbEntry.CurrentValues.GetValue<object>(propertyName).ToString()
                        }
                            );
                    }
                }
            }
            // Otherwise, don't do anything, we don't care about Unchanged or Detached entities

            return result;
        }*/
    }
}
