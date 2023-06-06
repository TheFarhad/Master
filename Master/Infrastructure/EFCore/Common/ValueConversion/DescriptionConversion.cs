namespace Master.Infrastructure.EFCore.Common.ValueConversion;

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Core.Domain.Common.ValueObjects;

public class DescriptionConversion : ValueConverter<Description, string>
{
    public DescriptionConversion()
        : base(_ => _.Value,
        _ => Description.New(_))
    { }
}
