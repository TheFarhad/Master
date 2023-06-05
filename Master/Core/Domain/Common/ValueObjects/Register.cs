namespace Master.Core.Domain.Common.ValueObjects;

using System;
using System.Collections.Generic;
using Master.Utilities.Extentions;
using Aggregate.Exception;
using Aggregate.ValueObject;
using Master.Core.Domain.Aggregate.ValueObject;

public class Register : ValueObject<Register>
{
    public DateTime Value { get; private set; }

    private Register() { }
    public Register(DateTime value) =>
        Init(value);

    private void Init(DateTime value)
    {
        if (value.IsNull())
            throw new InvalidValueObjectStateException("");
        Value = value;
    }

    public Register AddDays(int value) =>
        new(Value.AddDays(value));

    public Register AddMonths(int value) =>
    new(Value.AddMonths(value));

    public Register AddYears(int value) =>
    new(Value.AddYears(value));

    public static Register New(DateTime value) =>
        new(value);

    public static bool operator <(Register left, Register right) =>
        left.Value < right.Value;

    public static bool operator <=(Register left, Register right) =>
        left.Value <= right.Value;

    public static bool operator >(Register left, Register right) =>
       left.Value > right.Value;

    public static bool operator >=(Register left, Register right) =>
     left.Value >= right.Value;

    public static explicit operator DateTime(Register description) =>
        description.Value;

    public static implicit operator Register(DateTime value) =>
        new(value);

    public override string ToString() =>
        Value.ToString();

    protected override IEnumerable<object> GetEqualityAttributes()
    {
        yield return Value;
    }
}
