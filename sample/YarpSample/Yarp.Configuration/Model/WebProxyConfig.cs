// Project: Aguafrommars/DynamicConfiguration
// Copyright (c) 2021 @Olivier Lefebvre

namespace Yarp.Configuration.Model
{
    /// <summary>
    /// Config used to construct System.Net.WebProxy instance.
    /// </summary>
    public class WebProxyConfig
    {
        /// <summary>
        /// The URI of the proxy server.
        /// </summary>
        public string? Address
        {
            get;
            set;
        }

        /// <summary>
        /// true to bypass the proxy for local addresses; otherwise, false. If null, default
        /// value will be used: false
        /// </summary>
        public bool? BypassOnLocal
        {
            get;
            set;
        }

        /// <summary>
        /// Controls whether the System.Net.CredentialCache.DefaultCredentials are sent with
        /// requests. If null, default value will be used: false
        /// </summary>
        public bool? UseDefaultCredentials
        {
            get;
            set;
        }
    }
}
