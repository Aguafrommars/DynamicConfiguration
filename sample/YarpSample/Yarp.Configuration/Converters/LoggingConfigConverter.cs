using Yarp.Configuration.Model;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Yarp.Configuration.Converters
{
    public class LoggingConfigConverter : JsonConverter<LoggingConfig>
    {
        public override LoggingConfig? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException();
            }
            reader.Read();
            reader.GetString();
            var logLevel = JsonSerializer.Deserialize<IDictionary<string, LogLevel>>(ref reader, options);
            var config = new LoggingConfig
            {
                LogLevel = logLevel,
            };
            if (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
            {
                var providers = new Dictionary<string, ProviderLoggingConfig?>();
                config.Providers = providers;
                do
                {
                    var key = reader.GetString() ?? throw new InvalidOperationException("Key cannot be null");
                    var value = JsonSerializer.Deserialize<ProviderLoggingConfig>(ref reader, options);
                    providers.Add(key, value);
                } while (reader.Read() && reader.TokenType != JsonTokenType.EndObject);
            }
            return config;
        }

        public override void Write(Utf8JsonWriter writer, LoggingConfig value, JsonSerializerOptions options)
        {    
            writer.WriteStartObject();
            writer.WritePropertyName(nameof(value.LogLevel));
            JsonSerializer.Serialize(writer, value.LogLevel, options);
            if (value.Providers != null)
            {
                foreach(var kv in value.Providers)
                {
                    writer.WritePropertyName(kv.Key);
                    JsonSerializer.Serialize(writer, kv.Value, options);
                }
            }
            writer.WriteEndObject();            
        }
    }
}
