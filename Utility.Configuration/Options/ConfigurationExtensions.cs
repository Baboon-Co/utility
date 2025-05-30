﻿using BaboonCo.Utility.Configuration.Options.Abstractions;
using Microsoft.Extensions.Configuration;

namespace BaboonCo.Utility.Configuration.Options;

public static class ConfigurationExtensions
{
    public static T GetRequired<T>(this IConfiguration configuration) where T: IConfigurationOptions
    {
        var options = configuration.GetSection(T.SectionName).Get<T>();
        if (options is null)
            throw new InvalidOperationException("There is no configuration for " + typeof(T).Name);

        return options;
    }
}