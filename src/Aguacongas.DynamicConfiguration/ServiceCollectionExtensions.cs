using Aguacongas.DynamicConfiguration.Abstractions;
using Aguacongas.DynamicConfiguration.Options;
using Aguacongas.DynamicConfiguration.Services;
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
        /// <param name="configure">An action to cconfigure the options</param>
        /// <returns></returns>
        public static IServiceCollection AddConfigurationServices(this IServiceCollection services, Action<DynamicConfigurationOptions> configure)
        {
            services.Configure(configure);
            return services.AddTransient<IConfigurationService, ConfigurationService>();
        }
    }
}
