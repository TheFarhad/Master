namespace Master.Core.Contract.Application.Command;

using Common;

public class CommandResult : ServiceResult { }
public class CommandResult<TData> : ServiceResult
{
    public TData? Payload { get; set; }
}
