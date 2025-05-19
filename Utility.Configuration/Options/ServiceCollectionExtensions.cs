using BaboonCo.Utility.Configuration.Options.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace BaboonCo.Utility.Configuration.Options;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddOptionsWithFluentValidation<TOptions>(
        this IServiceCollection services
    ) where TOptions : class, IConfigurationOptions
    {
        services.AddOptions<TOptions>()
            .BindConfiguration(TOptions.SectionName)
            .ValidateFluentValidation()
            .ValidateOnStart();

        return services;
    }
}