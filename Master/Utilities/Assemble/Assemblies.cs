namespace Master.Utilities.Assemble;

using System.Reflection;
using Microsoft.Extensions.DependencyModel;

public static class Assemblies
{
    public static List<Assembly> Get(params string[] assemblies)
    {
        var result = new List<Assembly>();
        var libraries = DependencyContext.Default.RuntimeLibraries;
        foreach (var item in libraries)
        {
            if (IsCandidateCompilationLibrary(item, assemblies))
            {
                var assembly = Assembly.Load(new AssemblyName(item.Name));
                result.Add(assembly);
            }
        }
        return result;
    }

    private static bool IsCandidateCompilationLibrary(RuntimeLibrary compilationLibrary, string[] assmblyName)
    {
        var result = assmblyName
            .Any(d => compilationLibrary.Name.Contains(d))
                    || compilationLibrary.Dependencies.Any(d => assmblyName.Any(c => d.Name.Contains(c)));

        return result;
    }
}
