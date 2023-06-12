namespace Master.Utilities.Extentions;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Endpoint.ModelBinding;

public static class NonValidatingValidatorExtensions
{
    public static IServiceCollection AddNonValidatingValidator(this IServiceCollection source)
    {
        var validator = source.FirstOrDefault(s => s.ServiceType == typeof(IObjectModelValidator));
        if (validator.IsNotNull())
        {
            source.Remove(validator);
            source.Add(new ServiceDescriptor(typeof(IObjectModelValidator), _ => new NonValidatingValidator(), ServiceLifetime.Singleton));
        }
        return source;
    }
}

