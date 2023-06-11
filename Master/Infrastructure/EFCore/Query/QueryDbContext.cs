namespace Master.Infrastructure.EFCore.Query;

using Microsoft.EntityFrameworkCore;

public abstract class QueryDbContext : DbContext
{
    public QueryDbContext(DbContextOptions options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    }

    public override int SaveChanges() => throw new NotSupportedException("");

    public override int SaveChanges(bool acceptAllChangesOnSuccess) => throw new NotSupportedException("");

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default) => throw new NotSupportedException("");

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) => throw new NotSupportedException("");
}
