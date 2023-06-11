namespace Master.Infrastructure.EFCore.Command;

using System;
using System.Threading;
using System.Globalization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Interceptor;
using Utilities.Extentions;
using Common.ValueConversion;
using Core.Domain.Common.ValueObjects;
using Core.Contract.Application.Event;
using Utilities.Services.Abstraction.Identity;

public class CommandDbContext : DbContext
{
    private IDbContextTransaction _transaction;

    protected CommandDbContext() { }
    public CommandDbContext(DbContextOptions options) : base(options) { }

    #region configration

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.AddCode();
        modelBuilder.AddAuditableShadowProperties();
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);

        configurationBuilder.Properties<Title>().HaveConversion<TitleConversion>();
        configurationBuilder.Properties<Description>().HaveConversion<DescriptionConversion>();
        configurationBuilder.Properties<NationalCode>().HaveConversion<NationalCodeConversion>();
        configurationBuilder.Properties<Priority>().HaveConversion<PriorityConversion>();
        configurationBuilder.Properties<Register>().HaveConversion<RegisterConversion>();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.AddInterceptors(new CommandDbContextInterceptor());
    }
    #endregion

    #region save

    public override int SaveChanges()
    {
        //ChangeTracker.DetectChanges();
        //BeforeSave();
        //ChangeTracker.AutoDetectChangesEnabled = false;
        var result = base.SaveChanges();
        //ChangeTracker.AutoDetectChangesEnabled = true;
        return result;
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        //ChangeTracker.DetectChanges();
        //BeforeSave();
        //ChangeTracker.AutoDetectChangesEnabled = false;
        var result = base.SaveChanges(acceptAllChangesOnSuccess);
        //ChangeTracker.AutoDetectChangesEnabled = true;
        return result;
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        //ChangeTracker.DetectChanges();
        //BeforeSave();
        //ChangeTracker.AutoDetectChangesEnabled = false;
        var result = await base.SaveChangesAsync(cancellationToken);
        //ChangeTracker.AutoDetectChangesEnabled = true;
        return result;
    }

    public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        //ChangeTracker.DetectChanges();
        //BeforeSave();
        //ChangeTracker.AutoDetectChangesEnabled = false;
        var result = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        //ChangeTracker.AutoDetectChangesEnabled = true;
        return result;
    }

    #endregion

    #region transaction

    private void BeginTransaction() => _transaction = Database.BeginTransaction();
    private async Task BeginTransactionAsync() => _transaction = await Database.BeginTransactionAsync();

    private void Commit()
    {
        ValidateTransaction();
        _transaction.Commit();
    }
    private async Task CommitAsync()
    {
        ValidateTransaction();
        await _transaction.CommitAsync();
    }

    private void Rollback()
    {
        ValidateTransaction();
        _transaction.Rollback();
    }
    private async Task RollbackAsync()
    {
        ValidateTransaction();
        await _transaction.RollbackAsync();
    }

    public string TransactionalAction(Action action)
    {
        var result = String.Empty;
        try
        {
            if (action is not null)
            {
                BeginTransaction();
                action.Invoke();
                SaveChanges();
                Commit();
            }
        }
        catch (Exception e)
        {
            Rollback();
            result = e.Message;
        }
        finally
        {
            //Dispose();
        }
        return result;
    }
    public async Task<string> TransactionalActionAsync(Action action)
    {
        var result = String.Empty;
        try
        {
            if (action is not null)
            {
                await BeginTransactionAsync();
                action.Invoke();
                await SaveChangesAsync();
                await CommitAsync();
            }
        }
        catch (Exception e)
        {
            await RollbackAsync();
            result = e.Message;
        }
        finally
        {
            //Dispose();
        }
        return result;
    }

    #endregion

    #region aggregate's relations

    public IEnumerable<string> RelationsGraph(Type clrEntityType)
    {
        var entityType = Model.FindEntityType(clrEntityType);
        var includedNavigations = new HashSet<INavigation>();
        var stack = new Stack<IEnumerator<INavigation>>();
        while (true)
        {
            var navigations = new List<INavigation>();
            var entityNavigations = entityType.GetNavigations();
            foreach (var item in entityNavigations)
            {
                if (includedNavigations.Add(item))
                    navigations.Add(item);
            }
            if (navigations.Count == 0)
            {
                if (stack.Count > 0)
                    yield return string.Join(".", stack.Reverse().Select(e => e.Current.Name));
            }
            else
            {
                foreach (var navigation in navigations)
                {
                    var inverseNavigation = navigation.Inverse;
                    if (inverseNavigation != null)
                        includedNavigations.Add(inverseNavigation);
                }
                stack.Push(navigations.GetEnumerator());
            }
            while (stack.Count > 0 && !stack.Peek().MoveNext())
                stack.Pop();
            if (stack.Count == 0) break;
            entityType = stack.Peek().Current.TargetEntityType;
        }
    }

    #endregion

    #region utilities

    private void ValidateTransaction()
    {
        if (_transaction.IsNull())
            throw new NullReferenceException("Please call `BeginTransaction()` method first.");
    }

    //protected virtual void BeforeSave()
    //{
    //    SetShadowProperties();
    //    DispatchEvents();
    //}

    //// روش بهینه این است که با اینترسپتور نوشته شود
    //private void SetShadowProperties()
    //{
    //    var service = this.GetService<IUserService>();
    //    ChangeTracker.SetAuditableEntityShadowPropertyValues(service);
    //}

    //// روش بهینه این است که با اینترسپتور نوشته شود
    //private void DispatchEvents()
    //{
    //    var dispatcher = this.GetService<IEventDispatcher>();
    //    var aggregates = ChangeTracker.AggregateWithEvents();

    //    foreach (var item in aggregates)
    //    {
    //        var events = item.Events;
    //        foreach (dynamic _ in events) dispatcher.DispatchAsync(_);
    //    }
    //}

    public T GetShadowProperty<T>(object source, string propertyName) where T : IConvertible
    {
        var value = Entry(source).Property(propertyName).CurrentValue;
        return value.IsNotNull() ? value.ChangeType<T>(CultureInfo.InvariantCulture) : default;
    }

    #endregion
}