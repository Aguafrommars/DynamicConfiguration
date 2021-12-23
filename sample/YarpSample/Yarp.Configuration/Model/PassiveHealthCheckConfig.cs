// Project: Aguafrommars/DynamicConfiguration
// Copyright (c) 2021 @Olivier Lefebvre

using System;

namespace Yarp.Configuration.Model
{
    /// <summary>
    /// Passive health check config.
    /// </summary>
    public class PassiveHealthCheckConfig
    {
        /// <summary>
        /// Whether passive health checks are enabled.
        /// </summary>
        public bool? Enabled
        {
            get;
            set;
        }

        /// <summary>
        /// Passive health check policy.
        /// </summary>
        public string? Policy
        {
            get;
            set;
        }

        /// <summary>
        /// Destination reactivation period after which an unhealthy destination is considered healthy again.
        /// </summary>
        public TimeSpan? ReactivationPeriod
        {
            get;
            set;
        }
    }
}
