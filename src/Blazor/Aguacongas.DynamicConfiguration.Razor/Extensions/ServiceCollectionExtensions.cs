using Aguacongas.DynamicConfiguration.Razor.Options;
using Aguacongas.DynamicConfiguration.Razor.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IHttpClientBuilder AddConfigurationService(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<SettingsOptions>(options => configuration.Bind(options))
                .AddScoped<IConfigurationService, ConfigurationService>();
            return services.AddHttpClient(nameof(ConfigurationService))
                .ConfigureHttpClient((provider, httpClient) =>
                {
                    var options = provider.GetRequiredService<IOptions<SettingsOptions>>().Value;
                    if (options?.ApiUrl is null)
                    {
                        throw new InvalidOperationException("ApiUrl cannot be null.");
                    }
                    httpClient.BaseAddress = new Uri(options.ApiUrl);
                });
        }
    }
}
