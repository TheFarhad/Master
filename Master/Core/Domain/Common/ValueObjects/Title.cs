namespace Master.Core.Domain.Common.ValueObjects;

using System;
using System.Collections.Generic;
using Master.Utilities.Extentions;
using Aggregate.Exception;
using Aggregate.ValueObject;

public class Title : ValueObject<Title>
{
    public string Value { get; private set; }
    private const int maxChar = 100;

    private Title() { }
    public Title(string value) =>
        Init(value);

    private void Init(string value)
    {
        if (value.IsNull())
            throw new InvalidValueObjectStateException("");
        if (value.Length > maxChar)
            throw new InvalidValueObjectStateException("توضیحات باید حداکثر }0{ باشد", $"{maxChar}");
        Value = value;
    }

    public static Title New(string value) =>
        new(value);

    protected override IEnumerable<object> GetEqualityAttributes()
    {
        yield return Value;
    }

    public static explicit operator string(Title description) =>
        description.Value;

    public static implicit operator Title(string value) =>
        new(value);

    public override string ToString() =>
        Value;
}
