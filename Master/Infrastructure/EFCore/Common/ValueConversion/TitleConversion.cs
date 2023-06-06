namespace Master.Infrastructure.EFCore.Common.ValueConversion;

using Master.Core.Domain.Common.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

public class TitleConversion : ValueConverter<Title, string>
{
    public TitleConversion()
        : base(_ => _.Value,
        _ => Title.New(_))
    { }
}