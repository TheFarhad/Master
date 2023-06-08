namespace Master.Core.Contract.Application.Command;

public interface ICommandDispatcher
{
    Task<CommandResult> DispatchAsync<TCommand>(TCommand source) where TCommand : ICommand;
    Task<CommandResult<TPayload>> DispatchAsync<TCommand, TPayload>(TCommand source) where TCommand : ICommand<TPayload>;
}
