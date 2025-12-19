using GlowGirlsData.Models;
using Microsoft.EntityFrameworkCore;

namespace GlowGirlsData.Database;

public class GlowGirlsDbContext : DbContext
{
    public override int SaveChanges()
    {
        SetAuditingFields();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        SetAuditingFields();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void SetAuditingFields()
    {
        var entries = ChangeTracker.Entries<IAuditableEntity>();

        foreach (var entry in entries)
            if (entry.State == EntityState.Added)
            {
                // Set CreatedAt timestamp only for new entities
                entry.Entity.CreatedAt = DateTime.UtcNow;
                entry.Entity.UpdatedAt = DateTime.UtcNow; // Set UpdatedAt to same for new record
            }
            else if (entry.State == EntityState.Modified)
            {
                // Set UpdatedAt timestamp for modified entities
                entry.Entity.UpdatedAt = DateTime.UtcNow;

                // Ensure CreatedAt is not modified during an update
                entry.Property(nameof(IAuditableEntity.CreatedAt)).IsModified = false;
            }
    }
}