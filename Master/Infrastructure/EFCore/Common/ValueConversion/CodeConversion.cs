namespace Master.Infrastructure.EFCore.Common.ValueConversion;

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Core.Domain.Aggregate.ValueObject;

public class CodeConversion : ValueConverter<Code, Guid>
{
    public CodeConversion()
        : base(_ => _.Value,
        _ => Code.New(_))
    { }
}