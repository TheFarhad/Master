namespace Master.Core.Contract.Infrastructure.Command;

public interface IUnitOfWork
{
    string TransactionalAction(Action action);
    Task<string> TransactionalActionAsync(Action action);
}
