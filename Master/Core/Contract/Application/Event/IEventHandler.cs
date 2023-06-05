namespace Master.Core.Contract.Application.Event;

using Domain.Aggregate.Event;

public interface IEventHandler<TEvent> where TEvent : IEvent
{
    Task HandleAsync(TEvent Source);
}
