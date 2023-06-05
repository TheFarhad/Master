namespace Master.Core.Domain.Aggregate.Entity;

using System.Reflection;
using Event;
using Utilities.Extentions;

public abstract class AggregateRoot : Entity
{
    private List<IEvent> _events;
    public IReadOnlyList<IEvent> Events => _events;
    public int Version { get; private set; }

    protected AggregateRoot() => _events = new();
    public AggregateRoot(IEnumerable<IEvent> source) => EndStateFromEvents(source);

    public void ClearEvents() =>
     _events.Clear();

    protected void Apply(IEvent source)
    {
        Mutate(source);
        SetEvent(source);
    }

    private void SetEvent(IEvent source) =>
       _events.Add(source);

    private void Mutate(IEvent source)
    {
        this
        .Type()
        .GetMethod("On",
        BindingFlags.Instance | BindingFlags.NonPublic, new Type[] { source.Type() })
        ?.Invoke(this, new[] { source });
        //((dynamic)this).On((dynamic)@source);
    }

    private void EndStateFromEvents(IEnumerable<IEvent> source)
    {
        if (!source.IsNull() && source.Any())
        {
            foreach (var item in source)
            {
                Mutate(item);
                Version++;
            }
            _events = source.ToList();
        }
    }
}
