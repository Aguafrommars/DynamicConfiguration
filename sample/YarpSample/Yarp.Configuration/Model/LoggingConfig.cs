using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace Yarp.Configuration.Model
{
    public class LoggingConfig
    {
        public IDictionary<string, LogLevel>? LogLevel { get; set; }

        public ProviderLoggingConfig? Console { get; set; }

        public ProviderLoggingConfig? Debug { get; set; }
    }

}
