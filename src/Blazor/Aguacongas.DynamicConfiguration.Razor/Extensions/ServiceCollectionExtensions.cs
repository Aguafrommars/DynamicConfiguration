// Project: Aguafrommars/DynamicConfiguration
// Copyright (c) 2021 @Olivier Lefebvre

using Aguacongas.DynamicConfiguration.Razor.Options;
using Aguacongas.DynamicConfiguration.Razor.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// <see cref="IServiceCollection"/> extensions.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add configuration services to the service collection.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static IHttpClientBuilder AddConfigurationService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddLocalization()
                .Configure<SettingsOptions>(options => configuration.Bind(options));
            services.TryAddScoped<IConfigurationService, ConfigurationService>();
            services.TryAddScoped<ISettingsLocalizer, DefaultSettingsLocalizer>();

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
