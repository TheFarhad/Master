namespace Master.Core.Application.Command;

using Common;

public class CommandResult : ServiceResult { }

public class CommandResult<TData> : ServiceResult
{
    public TData? Data { get; set; }
}
