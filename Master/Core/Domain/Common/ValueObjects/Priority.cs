namespace Master.Core.Domain.Common.ValueObjects;

using System.Collections.Generic;
using Aggregate.Exception;
using Aggregate.ValueObject;
using Master.Core.Domain.Aggregate.ValueObject;

public class Priority : ValueObject<Priority>
{
    public int Value { get; private set; }

    private Priority() { }
    public Priority(int value) =>
        Init(value);

    private void Init(int value)
    {
        if (value < 1)
            throw new InvalidValueObjectStateException("");
        Value = value;
    }

    public static Priority New(int value) =>
        new(value);

    protected override IEnumerable<object> GetEqualityAttributes()
    {
        yield return Value;
    }

    public Priority Increase(int value) =>
        new(Value + value);

    public Priority Decrease(int value) =>
        new(Value - value);

    public static Priority operator +(Priority source, int value) =>
        new(source.Value + value);

    public static Priority operator -(Priority source, int value) =>
        new(source.Value - value);

    public static bool operator <(Priority left, Priority right) =>
        left.Value < right.Value;

    public static bool operator <=(Priority left, Priority right) =>
       left.Value <= right.Value;

    public static bool operator >(Priority left, Priority right) =>
        left.Value > right.Value;

    public static bool operator >=(Priority left, Priority right) =>
        left.Value >= right.Value;

    public static explicit operator int(Priority source) =>
        source.Value;

    public static implicit operator Priority(int value) =>
        new(value);
}
