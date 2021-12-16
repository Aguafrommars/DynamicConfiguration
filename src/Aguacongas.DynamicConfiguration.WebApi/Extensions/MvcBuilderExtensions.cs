using Aguacongas.DynamicConfiguration.WebApi.Controllers;
using Aguacongas.DynamicConfiguration.Formatters;
using System.Text.Json;
using System.Text.Json.Serialization;
using Aguacongas.DynamicConfiguration.Options;

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
        /// <param name="configure">An action to cconfigure the options</param>
        /// <returns></returns>
        public static IMvcBuilder AddConfigurationWebAPI(this IMvcBuilder builder, Action<DynamicConfigurationOptions> configure)
        {
            builder.Services.AddConfigurationServices(configure);
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
