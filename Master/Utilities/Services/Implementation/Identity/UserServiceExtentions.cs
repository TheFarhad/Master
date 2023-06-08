namespace Master.Utilities.Services.Implementation.Identity;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Abstraction.Identity;

public static class UserServiceExtentions
{
    public static IServiceCollection UserServiceWireup(this IServiceCollection source, IConfiguration configuration, string sectionName) =>
            source
            .Configure<UserServiceConfig>(configuration.GetSection(sectionName))
            .AddSingleton<IUserService, UserService>();
}
