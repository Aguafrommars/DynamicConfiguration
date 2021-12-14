// Project: Aguafrommars/TheIdServer
// Copyright (c) 2021 @Olivier Lefebvre
namespace Yarp.Configuration.Model
{
    /// <summary>
    /// Session affinity options.
    /// </summary>
    public class SessionAffinityConfig
    {
        /// <summary>
        /// Indicates whether session affinity is enabled.
        /// </summary>
        public bool? Enabled
        {
            get;
            set;
        }

        /// <summary>
        /// The session affinity policy to use.
        /// </summary>
        public string? Policy
        {
            get;
            set;
        }

        /// <summary>
        /// Strategy handling missing destination for an affinitized request.
        /// </summary>
        public string? FailurePolicy
        {
            get;
            set;
        }

        /// <summary>
        ///  Identifies the name of the field where the affinity value is stored. For the
        ///     cookie affinity policy this will be the cookie name. For the header affinity
        ///     policy this will be the header name. The policy will give its own default if
        ///     no value is set. This value should be unique across clusters to avoid affinity
        ///     conflicts. https://github.com/microsoft/reverse-proxy/issues/976 This field is
        ///     required.
        /// </summary>
        public string? AffinityKeyName
        {
            get;
            set;
        }

        /// <summary>
        /// Configuration of a cookie storing the session affinity key in case the Yarp.ReverseProxy.Configuration.SessionAffinityConfig.Policy
        ///     is set to 'Cookie'.
        /// </summary>
        public SessionAffinityCookieConfig? Cookie
        {
            get;
            set;
        }
    }
}
