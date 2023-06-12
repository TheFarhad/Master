namespace Master.Utilities.Extentions;

using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

public static class MasterExtentions
{
    public static IServiceCollection MasterWireup(this IServiceCollection source, IEnumerable<Assembly> assemblies) =>
        source
            .ApplicationServiceWireup(assemblies)
            .DataAccessWireup(assemblies)
            .AddCustomeDepenecies(assemblies);
}