namespace Master.Utilities.Extentions;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Core.Domain.Aggregate.Entity;
using Services.Abstraction.Identity;

public static class ChangeTrackerExtentions
{
    public static void SetAuditableEntityShadowPropertyValues(this ChangeTracker source, IUserService userService)
    {
        var entities = source.Entries<Entity>();
        var userId = userService.Id();
        var dateTime = DateTime.UtcNow;

        var AddedEntries = entities.Where(_ => _.State == EntityState.Added);
        foreach (var item in AddedEntries)
        {
            item.Property<string>(ModelBuilderExtentions.CreatedByUserId).CurrentValue = userId;
            item.Property<DateTime>(ModelBuilderExtentions.CreatedDateTime).CurrentValue = dateTime;
        }

        var modifiedEntries = entities.Where(_ => _.State == EntityState.Modified);
        foreach (var item in modifiedEntries)
        {
            item.Property<string>(ModelBuilderExtentions.ModifiedByUserId).CurrentValue = userId;
            item.Property<DateTime>(ModelBuilderExtentions.ModifiedDateTime).CurrentValue = dateTime;
        }
    }

    public static IEnumerable<EntityEntry<AggregateRoot>> Aggregate(this ChangeTracker source, Func<EntityEntry<AggregateRoot>, bool> condition = null)
    {
        var result = source.Entries<AggregateRoot>();
        if (condition.IsNotNull()) result.Where(condition);
        return result;
    }

    public static IEnumerable<AggregateRoot> AggregateWithEvents(this ChangeTracker source) =>
       source.Aggregate(HasEvent()).Select(_ => _.Entity);

    public static IEnumerable<EntityEntry<AggregateRoot>> ModifiedAggregate(this ChangeTracker source) =>
       source.Aggregate(IsModified());

    private static Func<EntityEntry<AggregateRoot>, bool> IsModified() =>
        _ =>
        _.State == EntityState.Added || _.State == EntityState.Modified || _.State == EntityState.Deleted;

    private static Func<EntityEntry<AggregateRoot>, bool> HasEvent() =>
        _ => _.Entity.Events.Any();
}
