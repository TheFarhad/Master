namespace Master.Core.Domain.Aggregate.ValueObject;

using Master.Utilities.Extentions;

public abstract class ValueObject<TValueObject> : IEquatable<TValueObject> where TValueObject : ValueObject<TValueObject>
{
    public static bool operator ==(ValueObject<TValueObject> left, ValueObject<TValueObject> right) =>
        left.IsNull() && right.IsNull() ? true
            : left.IsNotNull() && right.IsNotNull() ? left.Equals(right)
        : false;

    public static bool operator !=(ValueObject<TValueObject> left, ValueObject<TValueObject> right) =>
        !(left == right);

    public bool Equals(TValueObject? other) =>
        this == other;

    protected abstract IEnumerable<object> GetEqualityAttributes();

    public override bool Equals(object? obj) =>
        obj is TValueObject other && GetEqualityAttributes().SequenceEqual(other.GetEqualityAttributes()) ? true : false;

    public override int GetHashCode() =>
        GetEqualityAttributes()
            .Select(_ => _.IsNotNull() ? _.GetHashCode() : 0)
            .Aggregate((a, b) => a ^ b);
}
