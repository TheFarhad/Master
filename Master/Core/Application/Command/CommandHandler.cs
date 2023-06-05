namespace Master.Core.Application.Command;

using System.Threading.Tasks;
using Contract.Application.Command;

public abstract class CommandHandler<TCommand> : ICommandHandler<TCommand> where TCommand : ICommand
{
    protected CommandResult Result { get; set; }

    public CommandHandler()
    {

    }

    public abstract Task<CommandResult> HandleAsync(TCommand Source);
}

public abstract class CommandHandler<TCommand, TResult> : ICommandHandler<TCommand, TResult> where TCommand : ICommand<TResult>
{
    protected CommandResult<TResult> Result { get; set; }

    public CommandHandler()
    {

    }

    public abstract Task<CommandResult<TResult>> HandleAsync(TCommand Source);
}
