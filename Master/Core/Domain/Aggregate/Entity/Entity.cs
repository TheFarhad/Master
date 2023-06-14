namespace Master.Core.Domain.Aggregate.Entity;

using ValueObject;
using Utilities.Extentions;

public abstract class Entity : IEquatable<Entity>
{
    public long Id { get; protected set; }
    public Code Code { get; protected set; } = Code.New(Guid.NewGuid());

    protected Entity() { }

    public override bool Equals(object? obj) => obj is Entity other && Id == other.Id;

    public static bool operator ==(Entity left, Entity right)
    {
        var result = false;
        if (left.IsNull() && right.IsNull()) result = true;
        else if (left.IsNotNull() && right.IsNotNull()) result = left.Equals(right);
        return result;
    }

    public static bool operator !=(Entity left, Entity right) => !(left == right);

    public bool Equals(Entity? other) => this == other;

    public override int GetHashCode() => Id.GetHashCode();
}
