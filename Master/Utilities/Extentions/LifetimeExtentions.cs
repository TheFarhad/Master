namespace Master.Utilities.Extentions;

using System.Reflection;
using Master.Utilities.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

public static class LifetimeExtentions
{
    public static IServiceCollection AddTransient(this IServiceCollection source, IEnumerable<Assembly> assemblies, params Type[] assignableTo)
    {
        return source
            .Scan(_ => _.FromAssemblies(assemblies)
            .AddClasses(_ => _.AssignableToAny(assignableTo))
            .AsImplementedInterfaces()
            .WithTransientLifetime());
    }

    public static IServiceCollection AddScoped(this IServiceCollection source, IEnumerable<Assembly> assemblies, params Type[] assignableTo)
    {
        return source
            .Scan(_ => _.FromAssemblies(assemblies)
            .AddClasses(_ => _.AssignableToAny(assignableTo))
            .AsImplementedInterfaces()
            .WithScopedLifetime());
    }

    public static IServiceCollection AddSingleton(this IServiceCollection source, IEnumerable<Assembly> assemblies, params Type[] assignableTo)
    {
        return source
            .Scan(_ => _.FromAssemblies(assemblies)
            .AddClasses(_ => _.AssignableToAny(assignableTo))
            .AsImplementedInterfaces()
            .WithSingletonLifetime());
    }

    public static IServiceCollection AddCustomeDepenecies(this IServiceCollection source, IEnumerable<Assembly> assemblies)
    {
        return
             source
             .AddTransient(assemblies, typeof(ITransient))
             .AddScoped(assemblies, typeof(IScope))
             .AddSingleton(assemblies, typeof(ISingleton));
    }
}
