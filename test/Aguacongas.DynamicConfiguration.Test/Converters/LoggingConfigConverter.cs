using Aguacongas.DynamicConfiguration.Model;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Xunit;

namespace Aguacongas.DynamicConfiguration.Test.Converters
{
    public class LoggingConfigConverter
    {
        [Fact]
        public void Read_should_deserialize_LoggingConfig()
        {
            var loggingConfig = new LoggingConfig
            {
                LogLevel = new Dictionary<string, LogLevel>
                {
                    ["Default"] = LogLevel.Information
                },
                Providers = new Dictionary<string, ProviderLoggingConfig?>
                {
                    ["Console"] = new ProviderLoggingConfig
                    {
                        IncludeScopes = true,
                        LogLevel = new Dictionary<string, LogLevel>
                        {
                            ["Default"] = LogLevel.Warning
                        }
                    },
                    ["Debug"] = new ProviderLoggingConfig
                    {
                        IncludeScopes = true,
                        LogLevel = new Dictionary<string, LogLevel>
                        {
                            ["Default"] = LogLevel.Debug
                        }
                    }
                }
            };

            var serialized = JsonSerializer.Serialize(loggingConfig);

            var deserializer = JsonSerializer.Deserialize<LoggingConfig>(serialized);

            Assert.NotNull(deserializer);
            Assert.NotNull(deserializer?.LogLevel);
            Assert.NotNull(deserializer?.Providers);
            Assert.Contains(deserializer?.LogLevel, kv => kv.Key == "Default" && kv.Value == LogLevel.Information);
            Assert.Contains(deserializer?.Providers, kv => kv.Key == "Console" && 
                kv.Value?.IncludeScopes == true &&
                kv.Value.LogLevel is not null &&
                kv.Value.LogLevel.Any(v => v.Key == "Default" && v.Value == LogLevel.Warning));
        }

        [Fact]
        public void Read_should_deserialize_LoggingConfig_without_providers()
        {
            var loggingConfig = new LoggingConfig
            {
                LogLevel = new Dictionary<string, LogLevel>
                {
                    ["Default"] = LogLevel.Debug
                }
            };

            var serialized = JsonSerializer.Serialize(loggingConfig);

            var deserializer = JsonSerializer.Deserialize<LoggingConfig>(serialized);

            Assert.NotNull(deserializer);
            Assert.NotNull(deserializer?.LogLevel);
            Assert.Null(deserializer?.Providers);
        }
    }
}
