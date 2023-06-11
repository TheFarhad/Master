namespace Master.Infrastructure.EFCore.Command;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Core.Contract.Infrastructure.Command;

public class OutboxEventConfiguration : IEntityTypeConfiguration<OutboxEvent>
{
    public void Configure(EntityTypeBuilder<OutboxEvent> builder)
    {
        builder.Property(c => c.UserId).HasMaxLength(255);
        builder.Property(c => c.EventName).HasMaxLength(255);
        builder.Property(c => c.AggregateName).HasMaxLength(255);
        builder.Property(c => c.EventTypeName).HasMaxLength(500);
        builder.Property(c => c.AggregateTypeName).HasMaxLength(500);
        builder.Property(c => c.TraceId).HasMaxLength(100);
        builder.Property(c => c.SpanId).HasMaxLength(100);
    }
}
