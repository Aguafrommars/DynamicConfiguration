using Aguacongas.DynamicConfiguration.Abstractions;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.OpenApi.Models;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSwaggerGenFromConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddSwaggerGen(c =>
            {
                var info = configuration.GetSection(nameof(OpenApiInfo)).Get<OpenApiInfo>();
                c.SwaggerDoc(info.Version, info);
                
                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                var xmlPath = Path.Combine(basePath, $"{typeof(IConfigurationService).Assembly.GetName().Name}.xml");

                c.IncludeXmlComments(xmlPath);
            });
        }
    }
}
