using Aguacongas.Configuration.Abstractions;
using Aguacongas.Configuration.Services;

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
        /// <returns></returns>
        public static IServiceCollection AddConfigurationServices(this IServiceCollection services)
        {
            return services.AddTransient<IConfigurationService, ConfigurationService>();
        }
    }
}
