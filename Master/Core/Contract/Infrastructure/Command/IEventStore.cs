namespace Master.Core.Contract.Infrastructure.Command;

using Domain.Aggregate.Event;

public interface IEventStore
{
    void Save<TEvent>(string aggregateName, string aggregateId, IEnumerable<TEvent> events) where TEvent : IEvent;
    Task SaveAsync<TEvent>(string aggregateName, string aggregateId, IEnumerable<TEvent> events) where TEvent : IEvent;
}
