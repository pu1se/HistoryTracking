using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HistoryTracking.DAL.Sql.Migrations;

namespace HistoryTracking.DAL.Sql
{
    public class DataContext : DbContext
    {
        public static readonly Guid SystemUserId = new Guid("11000000-0000-0000-0000-AAAAAAAAAAAA");
        public DbSet<Product> Products { get; set; }

        public DataContext() : base("DefaultConnection")
        {
            Configuration.LazyLoadingEnabled = false;
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<DataContext, Configuration>());

            var objectContext = ((IObjectContextAdapter) this).ObjectContext;
            objectContext.SavingChanges += (sender, args) =>
            {
                var now = DateTime.UtcNow;

                foreach (var entry in this.ChangeTracker.Entries<IBaseEntity>())
                {
                    var entity = entry.Entity;

                    if (entry.State == EntityState.Added)
                    {
                        if (entity.Id == Guid.Empty)
                        {
                            entity.Id = Guid.NewGuid();
                        }

                        entity.CreatedDateUtc = now;
                        entity.CreatedByUserId = SystemUserId;
                    }

                    if (entry.State == EntityState.Added || entry.State == EntityState.Modified)
                    {
                        entity.UpdatedDateUtc = now;                            
                        entity.UpdatedByUserId = Guid.Empty;
                    }                    
                }
            };
        }


    }
}
