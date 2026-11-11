using AmazonClone.Domain.Common;
using AmazonClone.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Text;

namespace Amazon.Infrastructure.Persistance
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options) { }

        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) 
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) 
        {
            DateTime now = DateTime.UtcNow;

            foreach (var entry in ChangeTracker.Entries<BaseEntity>()) 
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = now;
                }
                else if (entry.State == EntityState.Modified) 
                {
                    entry.Entity.ModifiedAt = now;
                    entry.Property(e => e.CreatedAt).IsModified = false;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
