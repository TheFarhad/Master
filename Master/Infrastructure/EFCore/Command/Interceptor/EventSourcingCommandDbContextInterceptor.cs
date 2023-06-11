namespace Master.Infrastructure.EFCore.Command.Interceptor;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Utilities.Extentions;
using Utilities.Services.Abstraction.Identity;
using Core.Contract.Infrastructure.Command;
using Utilities.Services.Abstraction.Serializing;
using System.Diagnostics;

public class EventSourcingCommandDbContextInterceptor : CommandDbContextInterceptor
{
    protected override void BeforeSaving(DbContext context)
    {
        base.BeforeSaving(context);
        SetOutboxEvent(context);
    }

    private void SetOutboxEvent(DbContext context)
    {
        var aggregates = context.ChangeTracker.AggregateWithEvents();
        var userService = context.GetService<IUserService>();
        var serializeService = context.GetService<ISerialize>();
        var userId = userService.Id();
        var time = DateTime.UtcNow;

        var traceId = String.Empty;
        var spanId = String.Empty;
        var activity = Activity.Current;
        if (activity.IsNotNull())
        {
            traceId = activity.TraceId.ToHexString();
            spanId = activity.SpanId.ToHexString();
        }

        if (aggregates?.Any() == true)
        {
            foreach (var aggregate in aggregates)
            {
                var aggregateType = aggregate.Type();
                var events = aggregate.Events;
                foreach (var @event in events)
                {
                    var eventType = aggregate.Type();
                    context.Add(new OutboxEvent
                    {
                        AggregateId = aggregate.Code.ToString(),
                        EventId = Guid.NewGuid(),
                        UserId = userId,
                        AggregateName = aggregateType.Name,
                        AggregateTypeName = aggregateType.FullName,
                        EventName = eventType.Name,
                        EventTypeName = eventType.FullName,
                        AccuredOn = time,
                        Payload = serializeService.Serialize(@event),
                        TraceId = traceId,
                        SpanId = spanId,
                        IsProccessd = false
                    });
                }
                aggregate.ClearEvents();
            }
        }
    }
}
