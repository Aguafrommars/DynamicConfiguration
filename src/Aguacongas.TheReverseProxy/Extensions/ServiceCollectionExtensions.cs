using Aguacongas.TheReverseProxy.Abstractions;
using Aguacongas.TheReverseProxy.Services;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddTheReverseProxy(this IServiceCollection services)
        {
            return services.AddTransient<IConfigurationService, ConfigurationService>();
        }
    }
}
