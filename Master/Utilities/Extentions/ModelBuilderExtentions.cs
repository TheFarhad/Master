namespace Master.Utilities.Extentions;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Core.Domain.Aggregate.Entity;
using Core.Domain.Aggregate.ValueObject;
using Infrastructure.EFCore.Common.ValueConversion;

public static class ModelBuilderExtentions
{
    public static readonly string CreatedByUserId = nameof(CreatedByUserId);
    public static readonly Func<object, string> EFPropertyCreatedByUserId = _ =>
            EF.Property<string>(_, CreatedByUserId);

    public static readonly string ModifiedByUserId = nameof(ModifiedByUserId);
    public static readonly Func<object, string> EFPropertyModifiedByUserId = _ =>
            EF.Property<string>(_, CreatedByUserId);

    public static readonly string CreatedDateTime = nameof(CreatedDateTime);
    public static readonly Func<object, DateTime?> EFPropertyCreatedDateTime = _ =>
            EF.Property<DateTime>(_, CreatedDateTime);

    public static readonly string ModifiedDateTime = nameof(ModifiedDateTime);
    public static readonly Func<object, DateTime?> EFPropertyModifiedDateTime = _ =>
            EF.Property<DateTime>(_, ModifiedDateTime);

    public static void AddAuditableShadowProperties(this ModelBuilder source)
    {
        var entityTypes = source
            .EntityTypes(_ => typeof(Entity).IsAssignableFrom(_.ClrType));

        foreach (var item in entityTypes)
        {
            var entity = source.Entity(item.ClrType);
            entity.Property<string>(CreatedByUserId).HasMaxLength(50);
            entity.Property<string>(ModifiedByUserId).HasMaxLength(50);
            entity.Property<DateTime?>(CreatedDateTime);
            entity.Property<DateTime?>(ModifiedDateTime);
        }
    }

    public static void AddCode(this ModelBuilder source)
    {
        var entities = source
            .EntityTypes(_ =>
            typeof(AggregateRoot).IsAssignableFrom(_.ClrType) ||
            typeof(Entity).IsAssignableFrom(_.ClrType));

        foreach (var item in entities)
        {
            var code = nameof(Code);
            var entity = source.Entity(item.ClrType);

            entity
                .Property<Code>(code)
                .HasConversion(new CodeConversion())
                .IsUnicode()
                .IsRequired();
            entity.HasAlternateKey(code);
        }
    }

    public static ModelBuilder UseValueConverterForType<T>(this ModelBuilder source, ValueConverter converter, int maxLength = 0)
    {
        foreach (var item in source.EntityTypes())
        {
            var properties = item
                .ClrType
                .GetProperties()
                .Where(p => p.PropertyType == typeof(T));

            foreach (var property in properties)
            {
                var entity = source
                    .Entity(item.Name)
                    .Property(property.Name);

                entity.HasConversion(converter);

                if (maxLength > 0) entity.HasMaxLength(maxLength);
            }
        }
        return source;
    }

    public static IEnumerable<IMutableEntityType> EntityTypes(this ModelBuilder source, Func<IMutableEntityType, bool> condition = null)
    {
        var result = source.Model.GetEntityTypes();
        if (condition.IsNotNull()) result.Where(condition);
        return result;
    }
}
