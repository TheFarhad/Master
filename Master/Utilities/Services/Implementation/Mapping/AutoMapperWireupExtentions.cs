namespace Master.Utilities.Services.Implementation.Mapping;

using Abstraction.Mapping;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Utilities.Assembly;
using Master.Utilities.Assemble;

public static class AutoMapperWireupExtentions
{
    public static IServiceCollection AutoMapperWireup(this IServiceCollection source, IConfiguration configuration, string configSection)
    {
        source.Configure<AutoMapperConfig>(configuration.GetSection(configSection));
        var config = configuration.Get<AutoMapperConfig>();
        var assemblies = Assemblies.Get(config.AssmblyNamesForLoadProfiles);

        return source
            .AddAutoMapper(assemblies)
            .AddSingleton<IMap, AutoMapper>();
    }
}