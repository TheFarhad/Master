namespace Master.Core.Contract.Application.Command;

public interface ICommandDispatcher
{
    Task<CommandResult> DispatchAsync<TCommand>(TCommand Source) where TCommand : ICommand;
    Task<CommandResult<TData>> DispatchAsync<TCommand, TData>(TCommand source) where TCommand : ICommand<TData>;
}
