namespace Master.Utilities.Extentions;

using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Core.Contract.Application.Event;
using Core.Contract.Application.Query;
using Core.Contract.Application.Command;

public static class ApplicationServiceExtentions
{
    public static IServiceCollection ApplicationServiceWireup(this IServiceCollection source, IEnumerable<Assembly> assemblies) =>
        source
        .EventHandlerWireup(assemblies)
        .CommandHandlerWireup(assemblies)
        .QueryHandlerWireup(assemblies)
        .EventDispatcherDecoratorWireup(assemblies)
        .CommandDispatcherDecoratorWireup(assemblies)
        .QueryDispatcherDecoratorWireup(assemblies)
        .AddValidatorsFromAssemblies(assemblies);

    public static IServiceCollection EventHandlerWireup(this IServiceCollection source, IEnumerable<Assembly> assemblies) =>
        source.AddTransient(assemblies, typeof(IEventHandler<>));

    public static IServiceCollection CommandHandlerWireup(this IServiceCollection source, IEnumerable<Assembly> assemblies) =>
        source.AddTransient(assemblies, typeof(ICommandHandler<>), typeof(ICommandHandler<,>));

    public static IServiceCollection QueryHandlerWireup(this IServiceCollection source, IEnumerable<Assembly> assemblies) =>
        source.AddTransient(assemblies, typeof(IQueryHandler<,>));

    public static IServiceCollection EventDispatcherDecoratorWireup(this IServiceCollection source, IEnumerable<Assembly> assemblies)
    {


        return source;
    }

    public static IServiceCollection CommandDispatcherDecoratorWireup(this IServiceCollection source, IEnumerable<Assembly> assemblies)
    {


        return source;
    }

    public static IServiceCollection QueryDispatcherDecoratorWireup(this IServiceCollection source, IEnumerable<Assembly> assemblies)
    {


        return source;
    }
}
