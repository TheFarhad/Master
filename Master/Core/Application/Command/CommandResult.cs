namespace Master.Core.Application.Command;

using Contract.Application.Common;

public class CommandResult : ServiceResult { }

public class CommandResult<TData> : ServiceResult
{
    public TData? Data { get; set; }
}
