namespace Master.Utilities.Services.Abstraction.Logging;

using System.Reflection;

public class ScopedInformation : IScopedInformation
{
    public Dictionary<string, string> HostScopedInfo { get; }
    public Dictionary<string, string> RequestScopedInfo { get; }

    public ScopedInformation()
    {
        HostScopedInfo = new Dictionary<string, string>
        {
              {"MachineName", Environment.MachineName },
             {"EntryPoint", Assembly.GetEntryAssembly().GetName().Name }
        };

        RequestScopedInfo = new Dictionary<string, string>
        {
                {"RequestId", Guid.NewGuid().ToString() }
        };
    }
}
