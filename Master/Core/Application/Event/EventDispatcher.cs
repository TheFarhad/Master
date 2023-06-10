namespace Master.Core.Application.Event;

using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Utilities.Extentions;
using Domain.Aggregate.Event;
using Contract.Application.Event;

public class EventDispatcher : IEventDispatcher
{
    private readonly IServiceProvider _service;
    private readonly ILogger<EventDispatcher> _logger;
    private readonly Stopwatch _timer;

    public EventDispatcher(IServiceProvider service, ILogger<EventDispatcher> logger)
    {
        _service = service;
        _logger = logger;
        _timer = new Stopwatch();
    }

    public async Task DispatchAsync<TEvent>(TEvent source) where TEvent : IEvent
    {
        _timer.Start();
        var counter = 0;
        var type = source.Type();
        try
        {
            _logger.LogDebug("Routing event of type {EventType} With value {Event}  Start at {StartDateTime}", type, source, DateTime.Now);

            var tasks = new List<Task>();
            var handlers = _service.GetServices<IEventHandler<TEvent>>();
            foreach (var item in handlers)
            {
                counter++;
                tasks.Add(item.HandleAsync(source));
            }
            await Task.WhenAll(tasks);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "There is not suitable handler for {EventType} Routing failed at {StartDateTime}.", type, DateTime.Now);
            throw;
        }
        finally
        {
            _timer.Stop();
            _logger.LogDebug("Total number of handler for {EventType} is {Count}, EventHandlers tooks {Millisecconds} Millisecconds", _timer, counter, _timer.ElapsedMilliseconds);
        }
    }
}
