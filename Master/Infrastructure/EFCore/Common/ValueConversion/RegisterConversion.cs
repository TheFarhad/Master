namespace Master.Infrastructure.EFCore.Common.ValueConversion;

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Core.Domain.Common.ValueObjects;

public class RegisterConversion : ValueConverter<Register, DateTime>
{
    public RegisterConversion()
        : base(_ => _.Value,
            _ => Register.New(_))
    { }
}
