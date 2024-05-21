

namespace ordering.infrastracture.Data.Interceptors
{
    public  class AuditableEntityInterceptors:SaveChangesInterceptor
    {
        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            UpdateEntities(eventData.Context);
            return base.SavingChanges(eventData, result);
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            UpdateEntities(eventData.Context);
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        public void UpdateEntities(DbContext context)
        {
            if (context == null) return;
            foreach(var entry in context.ChangeTracker.Entries<IEntity>())
            {
                 if(entry.State== EntityState.Added)
                {
                    entry.Entity.CreatedBy = "unknown";
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                }
                if (entry.State == EntityState.Added || entry.State == EntityState.Modified || entry.HasChangedOwnerEntity())
                {
                    entry.Entity.LastModifiedBy = "unknown";
                    entry.Entity.LastModified = DateTime.UtcNow;
                }
            }
        }

    }

    public static class Extensions
    {
        public static bool HasChangedOwnerEntity(this EntityEntry entry)
        {
            return entry.References.Any(r => r.TargetEntry != null && r.TargetEntry.Metadata.IsOwned() && (r.TargetEntry.State == EntityState.Added || 
            r.TargetEntry.State == EntityState.Modified 
          ));
        }
       
    }
}
