namespace Master.Core.Contract.Application.Event;

using Domain.Aggregate.Event;

public interface IEventDispatcher
{
    Task DispatchAsync<TEvent>(TEvent source) where TEvent : IEvent;
}
