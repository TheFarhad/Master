namespace Master.Core.Application.Common;

using System.Collections.Generic;
using Contract.Application.Common;

public class ServiceResult : IServiceResult
{
    public List<string> Errors { get; }
    public ServiceStatus Status { get; set; }

    public void SetErrors(List<string> errors) =>
         Errors.AddRange(errors);
}
