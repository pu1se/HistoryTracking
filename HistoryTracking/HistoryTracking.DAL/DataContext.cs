using System;
using System.Collections.Generic;
using System.Data.Entity;
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
        public DbSet<OfferEntity> Offers { get; set; }
        public DbSet<OrderEntity> Orders { get; set; }
         

        public DataContext() : base(DefaultConnectionStringName)
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderEntity>()
                .HasMany(e => e.Offers)
                .WithMany(e => e.Orders)
                .Map(config =>
                {
                    config.MapLeftKey("OfferId");
                    config.MapRightKey("OrderId");
                    config.ToTable("Offers_Orders");
                });

            base.OnModelCreating(modelBuilder);
        }
    }
}
