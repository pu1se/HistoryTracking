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
            ChangeTracker.DetectChanges();
            var changes = this.ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);
            var now = DateTime.UtcNow;

            foreach (var dbEntry in changes)
            {
                var entity = dbEntry.Entity as BaseEntity;
                if (entity is null)
                {
                    continue;
                }

                if (entity is ITrackEntityChanges historyTrackingEntity)
                {
                    var trackEntityChange = TrackChanges.GetTrackEntityChange(dbEntry);
                    if (trackEntityChange != null)
                    {
                        TrackEntityChanges.Add(trackEntityChange);
                    }
                }

                if (dbEntry.State == EntityState.Added)
                {
                    if (entity.Id == Guid.Empty)
                    {
                        entity.Id = Guid.NewGuid();
                    }
                    entity.CreatedDateUtc = now;
                    entity.CreatedByUserId = UserManager.GetCurrentUser();
                }

                if (dbEntry.State == EntityState.Modified)
                {
                    dbEntry.Property(nameof(BaseEntity.CreatedDateUtc)).IsModified = false;
                    dbEntry.Property(nameof(BaseEntity.CreatedByUserId)).IsModified = false;
                }

                if (dbEntry.State == EntityState.Modified)
                {
                    entity.UpdatedDateUtc = now;
                    entity.UpdatedByUserId = UserManager.GetCurrentUser();
                    dbEntry.Property(nameof(BaseEntity.UpdatedDateUtc)).IsModified = true;
                    dbEntry.Property(nameof(BaseEntity.UpdatedByUserId)).IsModified = true;
                }
            }
        }
    }
}
