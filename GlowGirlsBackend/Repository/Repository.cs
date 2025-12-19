using Ardalis.Specification.EntityFrameworkCore;
using GlowGirlsBackend.Interfaces;
using GlowGirlsData.Database;
using GlowGirlsData.Models;
using Microsoft.EntityFrameworkCore;

namespace GlowGirlsBackend.Repository;

public class Repository<T>(GlowGirlsDbContext dbContext)
    : RepositoryBase<T>(dbContext),
        IRepository<T>
    where T : class
{
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        SetAuditingFields();
        return await DbContext.SaveChangesAsync(cancellationToken);
    }

    private void SetAuditingFields()
    {
        var entries = DbContext.ChangeTracker.Entries<IAuditableEntity>();

        foreach (var entry in entries)
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                    break;
                case EntityState.Modified:
                {
                    // Only update UpdatedAt if there was an actual change.
                    var hasChanges = entry.Properties.Any(p => p.IsModified);

                    if (hasChanges) entry.Entity.UpdatedAt = DateTime.UtcNow;

                    // Prevent updating the CreatedAt column
                    entry.Property(nameof(IAuditableEntity.CreatedAt)).IsModified = false;
                    break;
                }
            }
    }
}