using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HistoryTracking.DAL.Entities;

namespace HistoryTracking.DAL
{
    public class DataContext : DbContext
    {
        private const String DefaultConnectionStringName = "DefaultConnection";
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<SubscriptionProductEntity> SubscriptionProducts { get; set; }
        public DbSet<OrderEntity> Orders { get; set; }
         

        public DataContext() : base(DefaultConnectionStringName)
        {
            var objectContext = ((IObjectContextAdapter) this).ObjectContext;
            objectContext.SavingChanges += (sender, args) =>
            {
                var now = DateTime.UtcNow;

                foreach (var entry in this.ChangeTracker.Entries<BaseEntity>())
                {
                    var entity = entry.Entity;

                    if (entry.State == EntityState.Added)
                    {
                        entity.CreatedDate = now;
                    }

                    if (entry.State == EntityState.Modified)
                    {
                        entry.Property(nameof(BaseEntity.CreatedDate)).IsModified = false;
                    }

                    if (entry.State == EntityState.Added || entry.State == EntityState.Modified)
                    {
                        entity.UpdatedDate = now;

                        /*this.ActivityHistories.Add(new ActivityHistoryEntity
                        {
                            Id = Guid.NewGuid(),
                            EntityName = entry.
                        });*/
                    }                    
                }
            };
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
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
                .WithMany(e => e.SubscriptionOwnerUsers)
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
    }
}
