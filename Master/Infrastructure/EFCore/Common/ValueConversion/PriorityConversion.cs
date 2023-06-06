namespace Master.Infrastructure.EFCore.Common.ValueConversion;

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Core.Domain.Common.ValueObjects;

public class PriorityConversion : ValueConverter<Priority, int>
{
    public PriorityConversion()
        : base(_ => _.Value,
            _ => Priority.New(_))
    { }
}
