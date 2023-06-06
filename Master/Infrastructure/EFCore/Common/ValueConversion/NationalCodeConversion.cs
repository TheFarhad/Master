namespace Master.Infrastructure.EFCore.Common.ValueConversion;

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Core.Domain.Common.ValueObjects;

public class NationalCodeConversion : ValueConverter<NationalCode, string>
{
    public NationalCodeConversion()
        : base(_ => _.Value,
            _ => NationalCode.New(_))
    { }
}
