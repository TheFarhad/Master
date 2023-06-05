namespace Master.Core.Contract.Infrastructure.Common;

public interface IUnitOfWork
{
    string TransactionalAction(Action action);
    Task<string> TransactionalActionAsync(Action action);
}
