// Project: Aguafrommars/TheIdServer
// Copyright (c) 2021 @Olivier Lefebvre

namespace Yarp.Configuration.Model
{
    /// <summary>
    /// Defines the configuration we can override
    /// </summary>
    public class ServerConfig
    {
        /// <summary>
        /// Gets or sets the server urls
        /// </summary>
        public string? Urls { get; set; }

        /// <summary>
        /// Gets or sets the reverse proxy configuration
        /// </summary>
        public ProxyConfig? ReverseProxy { get; set; }

        /// <summary>
        /// Gets or sets the logging configuration
        /// </summary>
        public LoggingConfig? Logging { get; set; }
    }
}
