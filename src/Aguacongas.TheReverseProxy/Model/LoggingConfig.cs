using Aguacongas.TheReverseProxy.Converters;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Aguacongas.TheReverseProxy.Model
{
    [JsonConverter(typeof(LoggingConfigConverter))]
    public class LoggingConfig
    {
        public IDictionary<string, LogLevel>? LogLevel { get; set; }

        public IDictionary<string, ProviderLoggingConfig?>? Providers { get; set; }
}
}
