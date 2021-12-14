using Aguacongas.DynamicConfiguration.Abstractions;
using IdentityModel.AspNetCore.OAuth2Introspection;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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
                var scheme = configuration.GetSection(nameof(OpenApiSecurityScheme)).Get<OpenApiSecurityScheme>();
                c.AddSecurityDefinition(scheme.Reference.Id, scheme);
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        scheme, new List<string>()
                    }
                });

                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                var xmlPath = Path.Combine(basePath, $"{typeof(IConfigurationService).Assembly.GetName().Name}.xml");

                c.IncludeXmlComments(xmlPath);
            });
        }

        public static AuthenticationBuilder AddTheReverserProxyApiAuthorization(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    configuration.GetSection(nameof(JwtBearerOptions)).Bind(options);
                    options.ForwardDefaultSelector = context =>
                    {
                        var request = context.Request;
                        var token = TokenRetrieval.FromAuthorizationHeader()(request);
                        if (token?.Contains('.') == false)
                        {
                            return "introspection";
                        }

                        return null;
                    };
                })
                .AddOAuth2Introspection("introspection", options => configuration.GetSection(nameof(OAuth2IntrospectionOptions)).Bind(options));
        }
    }
}
