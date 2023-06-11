namespace Master.Infrastructure.EFCore.Command.Interceptor;

using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Utilities.Extentions;
using Core.Contract.Application.Event;
using Utilities.Services.Abstraction.Identity;

public class CommandDbContextInterceptor : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        var output = default(InterceptionResult<int>);
        var context = eventData.Context;

        context.ChangeTracker.DetectChanges();
        BeforeSaving(context);
        context.ChangeTracker.AutoDetectChangesEnabled = false;
        output = base.SavingChanges(eventData, result);
        context.ChangeTracker.AutoDetectChangesEnabled = true;
        return output;
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        var output = default(ValueTask<InterceptionResult<int>>);
        var context = eventData.Context;

        context.ChangeTracker.DetectChanges();
        BeforeSaving(context);
        context.ChangeTracker.AutoDetectChangesEnabled = false;
        output = base.SavingChangesAsync(eventData, result, cancellationToken);
        context.ChangeTracker.AutoDetectChangesEnabled = true;
        return output;
    }

    protected virtual void BeforeSaving(DbContext context)
    {
        SetShadowProperties(context);
        DispatchEvents(context);
    }

    private void SetShadowProperties(DbContext context)
    {
        var service = context.GetService<IUserService>();
        context.ChangeTracker.SetAuditableEntityShadowPropertyValues(service);
    }

    private void DispatchEvents(DbContext context)
    {
        var dispatcher = context.GetService<IEventDispatcher>();
        var aggregates = context.ChangeTracker.AggregateWithEvents();

        foreach (var item in aggregates)
        {
            var events = item.Events;
            foreach (dynamic _ in events) dispatcher.DispatchAsync(_);
        }
    }
}
