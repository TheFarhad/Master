namespace Master.Core.Contract.Application.Common;

public interface IServiceResult
{
    List<string> Errors { get; }
    ServiceStatus Status { get; set; }
}