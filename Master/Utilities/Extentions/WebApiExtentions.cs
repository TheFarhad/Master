namespace Master.Utilities.Extentions;

using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using Assemble;

public static class WebApiExtentions
{
    public static IServiceCollection WebApiWireup(this IServiceCollection source, params string[] assembliesToSearch)
    {
        var assemblies = Assemblies.Get(assembliesToSearch);
        source
          .AddControllers()
          .AddFluentValidation();

        return
            source
            .MasterWireup(assemblies);
    }
}