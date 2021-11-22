using Aguacongas.Configuration.Razor.Options;
using Microsoft.Extensions.Options;
using System.Collections;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Aguacongas.Configuration.Razor.Services
{
    public class ConfigurationService : IConfigurationService
    {
        private static JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        private readonly HttpClient _httpClient;
        private readonly IOptions<SettingsOptions> _options;

        private object? _configuration;

        public ConfigurationService(HttpClient httpClient, IOptions<SettingsOptions> options)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public async Task<object?> GetAsync(string? key, CancellationToken cancellationToken)
        {
            var type = GetConfigurationType();
            _configuration ??= await GetConfigurationAsync(type, cancellationToken).ConfigureAwait(false);

            if (key is null)
            {
                return _configuration;
            }

            object? value = _configuration;
            var segmentList = key.Split(':');
            foreach(var segment in segmentList)
            {
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
                value = property?.GetValue(value, null);
            }
            return value;
        }

        public async Task SetAsync<T>(string? key, T setting, CancellationToken cancellationToken)
        {
            var content = JsonSerializer.Serialize(setting, _jsonSerializerOptions);
            await _httpClient.PutAsync(key, new StringContent(content, Encoding.UTF8, "text/plain"), cancellationToken).ConfigureAwait(false);
        }

        private async Task<object?> GetConfigurationAsync(Type type, CancellationToken cancellationToken)
        {
            using var response = await _httpClient.GetAsync(string.Empty, cancellationToken);
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
                throw new InvalidOperationException($"Cannot gat type '{setting.TypeName}'");
            }

            return type;
        }
    }
}
