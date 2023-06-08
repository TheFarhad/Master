namespace Master.Core.Contract.Application.Command;

using Common;

public class CommandResult : ServiceResult { }
public class CommandResult<TPayload> : ServiceResult
{
    public TPayload? Payload { get; set; }
}
