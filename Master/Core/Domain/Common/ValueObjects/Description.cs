namespace Master.Core.Domain.Common.ValueObjects;

using System;
using System.Collections.Generic;
using Aggregate.Exception;
using Aggregate.ValueObject;

public class Description : ValueObject<Description>
{
    public string Value { get; private set; }
    private const int maxChar = 500;

    private Description() { }
    public Description(string value) =>
        Init(value);

    private void Init(string value)
    {
        if (value.Length > maxChar)
            throw new InvalidValueObjectStateException("Max lenght of {0} shoud not be greather than {1} charachters", nameof(Description), $"{maxChar}");
        Value = value;
    }

    public static Description New(string value) =>
        new(value);

    protected override IEnumerable<object> GetEqualityAttributes()
    {
        yield return Value;
    }

    public static explicit operator string(Description description) =>
        description.Value;

    public static implicit operator Description(string value) =>
        new(value);
}
