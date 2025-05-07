using Microsoft.Extensions.DependencyInjection;
using Utility.Options.Abstractions;

namespace Utility.Options;

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