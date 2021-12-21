// Project: Aguafrommars/DynamicConfiguration
// Copyright (c) 2021 @Olivier Lefebvre

using System.Collections.Generic;

namespace Yarp.Configuration.Model
{
    public class RouteConfig
    {
        public RouteMatch Match { get; set; } = default!;

        /// <summary>
        /// Optionally, an order value for this route. Routes with lower numbers take precedence over higher numbers.
        /// </summary>
        public int? Order { get; set; }

        /// <summary>
        /// Gets or sets the cluster that requests matching this route
        /// should be proxied to.
        /// </summary>
        public string? ClusterId { get; set; }

        /// <summary>
        /// The name of the AuthorizationPolicy to apply to this route.
        /// If not set then only the FallbackPolicy will apply.
        /// Set to "Default" to enable authorization with the applications default policy.
        /// Set to "Anonymous" to disable all authorization checks for this route.
        /// </summary>
        public string? AuthorizationPolicy { get; set; }

        /// <summary>
        /// The name of the CorsPolicy to apply to this route.
        /// If not set then the route won't be automatically matched for cors preflight requests.
        /// Set to "Default" to enable cors with the default policy.
        /// Set to "Disable" to refuses cors requests for this route.
        /// </summary>
        public string? CorsPolicy { get; set; }

        /// <summary>
        /// Arbitrary key-value pairs that further describe this route.
        /// </summary>
        public IDictionary<string, string>? Metadata { get; set; }

        /// <summary>
        /// Parameters used to transform the request and response. See <see cref="Transforms.Builder.ITransformBuilder"/>.
        /// </summary>
        public IEnumerable<Dictionary<string, string>>? Transforms { get; set; }

    }
}
