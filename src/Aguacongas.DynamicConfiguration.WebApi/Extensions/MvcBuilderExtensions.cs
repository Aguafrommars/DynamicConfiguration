// Project: Aguafrommars/DynamicConfiguration
// Copyright (c) 2021 @Olivier Lefebvre

using Aguacongas.DynamicConfiguration.Formatters;
using Aguacongas.DynamicConfiguration.Options;
using Aguacongas.DynamicConfiguration.WebApi.Controllers;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// <see cref="IMvcBuilder"/> extensions.
    /// </summary>
    public static class MvcBuilderExtensions
    {
        /// <summary>
        /// Adds configuration web API services and controlles in DI
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="configurationRoot">The root configuration.</param>
        /// <param name="configure">An action to cconfigure the options.</param>
        /// <returns></returns>
        public static IMvcBuilder AddConfigurationWebAPI(this IMvcBuilder builder,
            IConfigurationRoot configurationRoot,
            Action<DynamicConfigurationOptions> configure)
        {
            builder.Services.AddConfigurationServices(configurationRoot, configure);
            return builder.AddApplicationPart(typeof(ConfigurationController).Assembly)
                .AddMvcOptions(options =>
                {
                    options.InputFormatters.Insert(0, new RawRequestBodyFormatter());
                });
        }
    }
}
