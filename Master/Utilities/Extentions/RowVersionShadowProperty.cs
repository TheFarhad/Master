namespace Master.Utilities.Extentions;

using Microsoft.EntityFrameworkCore.Metadata.Builders;

public static class RowVersionShadowProperty
{
    public static readonly string RowVersion = nameof(RowVersion);
    public static void AddRowVersionShadowProperty<TEntity>(this EntityTypeBuilder<TEntity> source) where TEntity : class
=> source.Property<byte[]>(RowVersion).IsRowVersion();
}
