namespace Master.Core.Application.Event;

using Domain.Aggregate.Event;
using Contract.Application.Event;

public abstract class EventDispatcherDecorator : IEventDispatcher
{
    protected IEventDispatcher Dispatcher;
    public abstract int Order { get; }

    public void Set(IEventDispatcher dispatcher) => Dispatcher = dispatcher;

    public abstract Task DispatchAsync<TEvent>(TEvent source) where TEvent : IEvent;
}