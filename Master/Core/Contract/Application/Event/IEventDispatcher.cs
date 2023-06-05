namespace Master.Core.Contract.Application.Event;

using Domain.Aggregate.Event;

public interface IEventDispatcher
{
    Task Dispatch<TEvent>(TEvent Source) where TEvent : IEvent;
}
