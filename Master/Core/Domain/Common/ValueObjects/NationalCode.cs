namespace Master.Core.Domain.Common.ValueObjects;

using System.Collections.Generic;
using Aggregate.Exception;
using Aggregate.ValueObject;
using Master.Core.Domain.Aggregate.ValueObject;

public class NationalCode : ValueObject<NationalCode>
{
    public string Value { get; private set; }

    private NationalCode() { }
    public NationalCode(string value) =>
        Init(value);

    private void Init(string value)
    {
        //if (!value.IsNationalCode())
        //    throw new InvalidValueObjectStateException("ValidationErrorStringFormat", nameof(NationalCode));
        Value = value;
    }

    public static NationalCode New(string value) =>
       new(value);

    public override string ToString() =>
        Value;

    protected override IEnumerable<object> GetEqualityAttributes()
    {
        yield return Value;
    }

    public static explicit operator string(NationalCode source) =>
        source.Value;

    public static implicit operator NationalCode(string value) =>
        new(value);
}
