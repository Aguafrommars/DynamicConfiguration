// Project: Aguafrommars/DynamicConfiguration
// Copyright (c) 2021 @Olivier Lefebvre

using System;

namespace Yarp.Configuration.Model
{
    /// <summary>
    /// Active health check config.
    /// </summary>
    public class ActiveHealthCheckConfig
    {
        /// <summary>
        /// Whether active health checks are enabled.
        /// </summary>
        public bool? Enabled
        {
            get;
            set;
        }

        /// <summary>
        /// Health probe interval.
        /// </summary>
        public TimeSpan? Interval
        {
            get;
            set;
        }

        /// <summary>
        /// Health probe timeout, after which a destination is considered unhealthy.
        /// </summary>
        public TimeSpan? Timeout
        {
            get;
            set;
        }

        /// <summary>
        /// Active health check policy.
        /// </summary>
        public string? Policy
        {
            get;
            set;
        }

        /// <summary>
        /// HTTP health check endpoint path.
        /// </summary>
        public string? Path
        {
            get;
            set;
        }
    }
}
