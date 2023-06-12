namespace Master.Utilities.Services.Abstraction.Logging;

public interface IScopedInformation
{
    Dictionary<string, string> HostScopedInfo { get; }
    Dictionary<string, string> RequestScopedInfo { get; }
}