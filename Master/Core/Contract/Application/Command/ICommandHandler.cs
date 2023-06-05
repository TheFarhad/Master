namespace Master.Core.Contract.Application.Command;

public interface ICommandHandler<TCommand> where TCommand : ICommand
{
    Task<CommandResult> HandleAsync(TCommand Source);
}

public interface ICommandHandler<TCommand, TResult> where TCommand : ICommand<TResult>
{
    Task<CommandResult<TResult>> HandleAsync(TCommand Source);
}

