using Aguacongas.DynamicConfiguration.Abstractions;
using Aguacongas.DynamicConfiguration.Options;
using Aguacongas.DynamicConfiguration.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// <see cref="IServiceCollection"/> extensions
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds configuration services in DI
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configurationRoot">The root configuration.</param>
        /// <param name="configure">An action to cconfigure the options</param>
        /// <returns></returns>
        public static IServiceCollection AddConfigurationServices(this IServiceCollection services,
            IConfigurationRoot configurationRoot,
            Action<DynamicConfigurationOptions> configure)
        {
            services.Configure(configure);
            services.TryAddTransient(p => configurationRoot);
            services.TryAddTransient<IConfigurationService, ConfigurationService>();
            services.TryAddSingleton<IAutoReloadConfigurationService, AutoReloadConfigurationService>();
            return services;
        }
    }
}
