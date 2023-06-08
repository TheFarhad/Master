namespace Master.Core.Application.Event;

using Microsoft.Extensions.Logging;
using Utilities.Extentions;
using Domain.Aggregate.Exception;

public class EventDispatcherDomainExceptionDecorator : EventDispatcherDecorator
{
    // IEventDispatcher => EventDispatcher
    protected override int Order => 2;
    private readonly ILogger<EventDispatcherDomainExceptionDecorator> _logger;

    public EventDispatcherDomainExceptionDecorator(ILogger<EventDispatcherDomainExceptionDecorator> logger) =>
        _logger = logger;

    public override async Task DispatchAsync<TEvent>(TEvent source)
    {
        var type = source.Type();
        try
        {
            await Dispatcher.DispatchAsync(source);
        }
        catch (DomainStateException e)
        {
            _logger.LogError(e, "Processing of {EventType} With value {Event} failed at {StartDateTime} because there are domain exceptions.", type, source, DateTime.Now);
        }
        catch (AggregateException e)
        {
            if (e.InnerException is DomainStateException ie)
            {
                _logger.LogError(ie, "Processing of {EventType} With value {Event} failed at {StartDateTime} because there are domain exceptions.", type, source, DateTime.Now);
            }
            throw e;
        }
    }
}
