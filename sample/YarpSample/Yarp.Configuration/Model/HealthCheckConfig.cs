// Project: Aguafrommars/DynamicConfiguration
// Copyright (c) 2021 @Olivier Lefebvre

namespace Yarp.Configuration.Model
{
    /// <summary>
    /// All health check config.
    /// </summary>
    public class HealthCheckConfig
    {
        /// <summary>
        /// Passive health check config.
        /// </summary>
        public PassiveHealthCheckConfig? Passive
        {
            get;
            set;
        }

        /// <summary>
        /// Active health check config.
        /// </summary>
        public ActiveHealthCheckConfig? Active
        {
            get;
            set;
        }

        /// <summary>
        /// Available destinations policy.
        /// </summary>
        public string? AvailableDestinationsPolicy
        {
            get;
            set;
        }
    }
}
