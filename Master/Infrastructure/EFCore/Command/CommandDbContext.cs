namespace Master.Infrastructure.EFCore.Command;

using System.Threading;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;
using Utilities.Extentions;

public class CommandDbContext : DbContext
{
    private IDbContextTransaction _transaction;

    public CommandDbContext(DbContextOptions options) : base(options) { }

    #region save

    public override int SaveChanges()
    {
        BeforeSave();
        return base.SaveChanges();
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return base.SaveChangesAsync(cancellationToken);
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    #endregion

    #region transaction

    private void BeginTransaction() =>
       _transaction = Database.BeginTransaction();
    private async Task BeginTransactionAsync() =>
       _transaction = await Database.BeginTransactionAsync();

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

    protected virtual void BeforeSave()
    {

    }

    #endregion
}
