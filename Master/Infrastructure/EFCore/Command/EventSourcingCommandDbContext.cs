namespace Master.Infrastructure.EFCore.Command;

using Microsoft.EntityFrameworkCore;
using Interceptor;
using Core.Contract.Infrastructure.Command;

public class EventSourcingCommandDbContext : CommandDbContext
{
    public DbSet<OutboxEvent> OutboxEvents => Set<OutboxEvent>();

    protected EventSourcingCommandDbContext() { }
    public EventSourcingCommandDbContext(DbContextOptions options) : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.AddInterceptors(new EventSourcingCommandDbContextInterceptor());
    }

    //protected override void BeforeSave()
    //{
    //    base.BeforeSave();
    //    SetOutboxEvent();
    //}

    //private void SetOutboxEvent()
    //{
    //    var aggregates = ChangeTracker.AggregateWithEvents();
    //    var userService = this.GetService<IUserService>();
    //    var serializeService = this.GetService<ISerialize>();
    //    var userId = userService.Id();
    //    var time = DateTime.UtcNow;

    //    if (aggregates?.Any() == true)
    //    {
    //        foreach (var _ in aggregates)
    //        {
    //            var events = _.Events;
    //            foreach (var @event in events)
    //            {
    //                OutboxEvents.Add(new OutboxEvent
    //                {
    //                    AggregateId = _.Code.ToString(),
    //                    EventId = Guid.NewGuid(),
    //                    UserId = userId,
    //                    AggregateName = _.Type().Name,
    //                    AggregateTypeName = _.Type().FullName,
    //                    EventName = @event.Type().Name,
    //                    EventTypeName = @event.Type().FullName,
    //                    AccuredOn = time,
    //                    IsProccessd = false,
    //                    Payload = serializeService.Serialize(@event)
    //                });
    //            }
    //            _.ClearEvents();
    //        }
    //    }
    //}
}