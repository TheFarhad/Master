namespace Master.Core.Contract.Application.Common;

using System.Collections.Generic;

public class ServiceResult : IServiceResult
{
    public List<string> Errors { get; }
    public ServiceStatus Status { get; set; }

    public ServiceResult()
    {
        Errors = new List<string>();
        Status = ServiceStatus.Ok;
    }

    public void SetErrors(List<string> errors) =>
         Errors.AddRange(errors);

    public void SetError(string error) =>
       Errors.Add(error);

    public void ClearErrors() =>
        Errors.Clear();
}
