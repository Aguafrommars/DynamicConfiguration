// Project: Aguafrommars/TheIdServer
// Copyright (c) 2021 @Olivier Lefebvre
using System.Collections.Generic;

namespace Yarp.Configuration.Model
{
    public class ClusterConfig
    {
        /// <summary>
        /// Load balancing policy.
        /// </summary>
        public string? LoadBalancingPolicy { get; set; }

        /// <summary>
        /// Session affinity config.
        /// </summary>
        public SessionAffinityConfig? SessionAffinity { get; set; }

        /// <summary>
        /// Health checking config.
        /// </summary>
        public HealthCheckConfig? HealthCheck { get; set; }

        /// <summary>
        /// Config for the HTTP client that is used to call destinations in this cluster.
        /// </summary>
        public HttpClientConfig? HttpClient { get; set; }

        /// <summary>
        /// Config for outgoing HTTP requests.
        /// </summary>
        public ForwarderRequestConfig? HttpRequest { get; set; }

        /// <summary>
        /// The set of destinations associated with this cluster.
        /// </summary>
        public IDictionary<string, DestinationConfig>? Destinations { get; set; }

        /// <summary>
        /// Arbitrary key-value pairs that further describe this cluster.
        /// </summary>
        public IDictionary<string, string>? Metadata { get; set; }
    }
}
