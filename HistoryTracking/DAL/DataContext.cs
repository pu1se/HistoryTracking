using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class DataContext : DbContext
    {
        private DbSet<OrderEntity> Orders { get; set; }

        private static bool MigrationWasChecked { get; set; }

        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
            if (!MigrationWasChecked)
            {
                Database.Migrate();
                MigrationWasChecked = true;
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            DeletionPolicy(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }

        private static void DeletionPolicy(ModelBuilder modelBuilder)
        {
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            var now = DateTime.UtcNow;

            foreach (var changedEntity in ChangeTracker.Entries())
            {
                if (changedEntity.Entity is IEntity entity)
                {
                    switch (changedEntity.State)
                    {
                        case EntityState.Added:
                            entity.CreatedDate = now;
                            entity.UpdatedDate = now;
                            break;
                        case EntityState.Modified:
                            entity.UpdatedDate = now;
                            break;
                    }
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {            
            if (optionsBuilder.IsConfigured)
                return;

            optionsBuilder.UseLazyLoadingProxies();

            base.OnConfiguring(optionsBuilder);
        }
    }
}
