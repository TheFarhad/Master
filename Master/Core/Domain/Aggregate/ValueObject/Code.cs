namespace Master.Core.Domain.Aggregate.ValueObject;

using System;
using System.Collections.Generic;
using Exception;
using Master.Utilities.Extentions;

public class Code : ValueObject<Code>
{
    public Guid Value { get; private set; }

    private Code() { }
    public Code(string value) => Init(value);

    public static Code New(Guid value) => new() { Value = value };
    public static Code New(string value) => new(value);

    private void Init(string value)
    {
        if (value.IsNull()) throw Error("");
        else if (value.IsGuid(out Guid result)) Value = result;
        else throw Error("");
    }

    public InvalidValueObjectStateException Error(string message) =>
        new InvalidValueObjectStateException("");

    protected override IEnumerable<object> GetEqualityAttributes()
    {
        yield return Value;
    }

    public override string ToString() =>
        Value.ToString();

    public static explicit operator string(Code source) => source.ToString();
    public static explicit operator Guid(Code source) => source.Value;

    public static implicit operator Code(string source) => new(source);
    public static implicit operator Code(Guid source) => New(source);
}