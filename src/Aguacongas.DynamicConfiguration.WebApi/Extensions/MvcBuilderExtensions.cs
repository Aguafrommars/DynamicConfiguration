using Aguacongas.DynamicConfiguration.WebApi.Controllers;
using Aguacongas.DynamicConfiguration.Formatters;
using System.Text.Json;
using System.Text.Json.Serialization;

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
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IMvcBuilder AddConfigurationWebAPI(this IMvcBuilder builder)
        {
            builder.Services.AddConfigurationServices()
                .AddAuthorization();
            return builder.AddApplicationPart(typeof(ConfigurationController).Assembly)
                .AddMvcOptions(options =>
                {
                    options.InputFormatters.Insert(0, new RawRequestBodyFormatter());
                })
                .AddJsonOptions(options =>
                {
                    var serializationOptions = options.JsonSerializerOptions;
                    serializationOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault;
                    serializationOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                });
        }
    }
}
