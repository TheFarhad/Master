namespace Master.Core.Application.Command;

using System.Threading.Tasks;
using Utilities.Extentions;
using Contract.Application.Command;
using Contract.Application.Common;

public abstract class CommandHandler<TCommand> : ICommandHandler<TCommand> where TCommand : ICommand
{
    protected CommandResult Result = new();

    public abstract Task<CommandResult> HandleAsync(TCommand Source);

    protected CommandResult OK() => Result;

    protected Task<CommandResult> NotFound()
    {
        Result.SetError("");
        Result.Status = ServiceStatus.NotFound;
        return Task.FromResult(Result);
    }

    protected Task<CommandResult> ResultAsync(ServiceStatus status, string error = null)
    {
        if (!error.IsNull()) Result.SetError(error);
        Result.Status = status;
        return Task.FromResult(Result);
    }

    protected Task<CommandResult> ResultAsync(ServiceStatus status, string error = null, params string[] arguments)
    {
        if (!error.IsNull()) Result.SetError(error.PlaceHolder(arguments));
        Result.Status = status;
        return Task.FromResult(Result);
    }
}

public abstract class CommandHandler<TCommand, TResult> : ICommandHandler<TCommand, TResult> where TCommand : ICommand<TResult>
{
    protected CommandResult<TResult> Result = new();

    public abstract Task<CommandResult<TResult>> HandleAsync(TCommand Source);

    protected Task<CommandResult<TResult>> OK(TResult payload)
    {
        Result.Payload = payload;
        return Task.FromResult(Result);
    }

    protected Task<CommandResult<TResult>> NotFound()
    {
        Result.SetError("");
        Result.Status = ServiceStatus.NotFound;
        Result.Payload = default;
        return Task.FromResult(Result);
    }

    protected Task<CommandResult<TResult>> ResultAsync(TResult payload, ServiceStatus status, string error = null)
    {
        if (!error.IsNull()) Result.SetError(error);
        Result.Status = status;
        Result.Payload = payload;
        return Task.FromResult(Result);
    }
}
