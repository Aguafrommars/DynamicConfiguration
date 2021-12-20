using Aguacongas.DynamicConfiguration.Razor.Options;
using Microsoft.Extensions.Options;
using System.Collections;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Aguacongas.DynamicConfiguration.Razor.Services
{
    /// <summary>
    /// Configuration service
    /// </summary>
    public class ConfigurationService : IConfigurationService
    {
        private static JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
            IgnoreReadOnlyProperties = true,
            IgnoreReadOnlyFields = true,
            PropertyNameCaseInsensitive = true
        };

        private readonly HttpClient _httpClient;
        private readonly IOptions<SettingsOptions> _options;
        private readonly string _baseAddress;

        /// <summary>
        /// Gets the root configuration
        /// </summary>
        public object? Configuration { get; private set; }

        /// <summary>
        /// Initializes a new instance of <see cref="ConfigurationService"/>
        /// </summary>
        /// <param name="httpClientFactory">The http client factory.</param>
        /// <param name="options">The settings options.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public ConfigurationService(IHttpClientFactory httpClientFactory, IOptions<SettingsOptions> options)
        {
            _httpClient = httpClientFactory?.CreateClient(nameof(ConfigurationService)) ?? throw new ArgumentNullException($"HttpClient for {nameof(ConfigurationService)}");
            _options = options ?? throw new ArgumentNullException(nameof(options));
            var baseAddress = $"{_httpClient.BaseAddress}";
            _baseAddress = baseAddress.EndsWith("/") ? baseAddress : baseAddress + "/";
        }

        /// <summary>
        /// Gets the configuration at key.
        /// </summary>
        /// <param name="key">The configuration key.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<object?> GetAsync(string? key, CancellationToken cancellationToken = default)
        {
            var type = GetConfigurationType();
            Configuration ??= await GetConfigurationAsync(type, cancellationToken).ConfigureAwait(false);

            if (key is null)
            {
                return Configuration;
            }

            object? value = Configuration;

            var segmentList = key.Split(':');
            foreach(var segment in segmentList)
            {
                if (value is null)
                {
                    return null;
                }

                type = value.GetType();

                if (value is IDictionary dictionary)
                {
                    value = dictionary[segment];
                    continue;
                }

                if (value is IList list)
                {
                    var index = int.Parse(segment);
                    value = list[index];
                    continue;
                }

                if (value is IEnumerable enumerable)
                {
                    var index = int.Parse(segment);
                    var i = 0;
                    foreach(var item in enumerable)
                    {
                        if (i == index)
                        {
                            value = item;
                            break;
                        }
                        i++;
                    }
                }

                var property = type.GetProperty(segment);
                if (property is null)
                {
                    throw new InvalidOperationException($"Property {segment} doesn't exist in type {type}");
                }
                value = property.GetValue(value, null);
            }
            return value;
        }

        /// <summary>
        /// Saves the configuration at key.
        /// </summary>
        /// <param name="key">The configuration key</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns></returns>
        public async Task SaveAsync(string? key, CancellationToken cancellationToken = default)
        {
            if (key is null)
            {
                var propertyList = GetConfigurationType().GetProperties().Where(p => p.CanWrite);
                foreach (var property in propertyList)
                {
                    await SaveAsync(property.Name, cancellationToken).ConfigureAwait(false);
                }
                return;
            }

            var segmentList = key.Split(':');
            var rootProperty = segmentList[0];
            var setting = await GetAsync(rootProperty).ConfigureAwait(false);
            var content = JsonSerializer.Serialize(setting, _jsonSerializerOptions);
            var response = await _httpClient.PutAsync($"{_baseAddress}{rootProperty}", new StringContent(content, Encoding.UTF8, "application/dynamic-configuration-json"), cancellationToken).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
        }

        private async Task<object?> GetConfigurationAsync(Type type, CancellationToken cancellationToken)
        {
            using var response = await _httpClient.GetAsync($"{_baseAddress}{type.AssemblyQualifiedName}", cancellationToken);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

            return JsonSerializer.Deserialize(content, type, _jsonSerializerOptions);
        }

        
        private Type GetConfigurationType()
        {
            var setting = _options.Value;
            if (setting?.TypeName is null)
            {
                throw new InvalidOperationException("Settins options must have a not null type name.");
            }
            var type = Type.GetType(setting.TypeName);
            if (type is null)
            {
                throw new InvalidOperationException($"Cannot get type '{setting.TypeName}'");
            }

            return type;
        }
    }
}
