namespace Master.Core.Application.Command;

using Contract.Application.Command;

public abstract class CommandDispatcherDecorator : ICommandDispatcher
{
    protected ICommandDispatcher Dispatcher;
    public abstract int Order { get; }

    public void Set(ICommandDispatcher dispatcher) => Dispatcher = dispatcher;

    public abstract Task<CommandResult> DispatchAsync<TCommand>(TCommand source) where TCommand : ICommand;

    public abstract Task<CommandResult<TPayload>> DispatchAsync<TCommand, TPayload>(TCommand source) where TCommand : ICommand<TPayload>;
}