using LibraryManagementSystem.Domain.Common;
using LibraryManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Infrastructure.Persistence
{
    public class LibraryDbContext : DbContext
    {
        public DbSet<User> Users => Set<User>();

        public LibraryDbContext(DbContextOptions<LibraryDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(LibraryDbContext).Assembly);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            SetEntriesDates();
            return await base.SaveChangesAsync(cancellationToken);
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            SetEntriesDates();
            return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public override int SaveChanges()
        {
            SetEntriesDates();
            return base.SaveChanges();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            SetEntriesDates();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }


        private void SetEntriesDates()
        {
            var entries = ChangeTracker.Entries<EntityBase>().Where(e => (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                    entry.Entity.CreatedAtUtc = DateTime.UtcNow;
                else
                {
                    entry.Property(e => e.CreatedAtUtc).IsModified = false;
                    entry.Entity.LastUpdatedAtUtc = DateTime.UtcNow;
                }
            }
        }
    }
}
