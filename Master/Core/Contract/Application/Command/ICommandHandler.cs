namespace Master.Core.Contract.Application.Command;

public interface ICommandHandler<TCommand> where TCommand : ICommand
{
    Task<CommandResult> HandleAsync(TCommand Source);
}

public interface ICommandHandler<TCommand, TPayload> where TCommand : ICommand<TPayload>
{
    Task<CommandResult<TPayload>> HandleAsync(TCommand Source);
}

