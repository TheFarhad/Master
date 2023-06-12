namespace Master.Utilities.Extentions;

using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Core.Contract.Application.Event;
using Core.Contract.Application.Query;
using Core.Contract.Application.Command;
using Core.Application.Event;
using Core.Application.Query;
using Master.Core.Application.Command;

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
        source.AddTransient<EventDispatcher>();
        source.AddTransient<EventDispatcherDecorator, EventDispatcherValidationDecorator>();
        source.AddTransient<EventDispatcherDecorator, EventDispatcherDomainExceptionDecorator>();
        source.AddTransient<IEventDispatcher>(_ =>
        {
            var eventDispatcher = _.GetRequiredService<EventDispatcher>();
            var decorators = _.GetServices<EventDispatcherDecorator>();
            if (decorators?.Any() == true)
            {
                decorators
                .OrderBy(_ => _.Order)
                .ToList()
                .ForEach(_ =>
                {
                    IEventDispatcher dispatcher = _ is EventDispatcherValidationDecorator ? decorators.Single(_ => _ is EventDispatcherDomainExceptionDecorator) : eventDispatcher;

                    _.Set(dispatcher);
                });
            }
            return eventDispatcher;
        });
        return source;
    }

    public static IServiceCollection CommandDispatcherDecoratorWireup(this IServiceCollection source, IEnumerable<Assembly> assemblies)
    {
        source.AddTransient<CommandDispatcher>();
        source.AddTransient<CommandDispatcherDecorator, CommandDispatcherValidationDecorator>();
        source.AddTransient<CommandDispatcherDecorator, CommandDispatcherDomainExceptionDecorator>();
        source.AddTransient<ICommandDispatcher>(_ =>
        {
            var CommandDispatcher = _.GetRequiredService<CommandDispatcher>();
            var decorators = _.GetServices<CommandDispatcherDecorator>();
            if (decorators?.Any() == true)
            {
                decorators
                .OrderBy(_ => _.Order)
                .ToList()
                .ForEach(_ =>
                {
                    ICommandDispatcher dispatcher = _ is CommandDispatcherValidationDecorator ? decorators.Single(e => e is CommandDispatcherDomainExceptionDecorator) : CommandDispatcher;

                    _.Set(dispatcher);
                });
            }
            return CommandDispatcher;
        });
        return source;
    }

    public static IServiceCollection QueryDispatcherDecoratorWireup(this IServiceCollection source, IEnumerable<Assembly> assemblies)
    {
        source.AddTransient<QueryDispatcher>();
        source.AddTransient<QueryDispatcherDecorator, QueryDispatcherValidationDecorator>();
        source.AddTransient<QueryDispatcherDecorator, QueryDispatcherDomainExceptionDecorator>();
        source.AddTransient<IQueryDispatcher>(_ =>
        {
            var queryDispatcher = _.GetRequiredService<QueryDispatcher>();
            var decorators = _.GetServices<QueryDispatcherDecorator>();
            if (decorators?.Any() == true)
            {
                decorators
                .OrderBy(_ => _.Order)
                .ToList()
                .ForEach(_ =>
                {
                    IQueryDispatcher dispatcher = _ is QueryDispatcherValidationDecorator ? decorators.Single(e => e is QueryDispatcherDomainExceptionDecorator) : queryDispatcher;

                    _.Set(dispatcher);
                });
            }
            return queryDispatcher;
        });
        return source;
    }
}
