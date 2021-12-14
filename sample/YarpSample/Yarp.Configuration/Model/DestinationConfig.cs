// Project: Aguafrommars/TheIdServer
// Copyright (c) 2021 @Olivier Lefebvre
using System.Collections.Generic;

namespace Yarp.Configuration.Model
{
    /// <summary>
    /// Describes a destination of a cluster.
    /// </summary>
    public class DestinationConfig
    {
        /// <summary>
        /// Address of this destination. E.g. https://127.0.0.1:123/abcd1234/. This field
        /// is required.
        /// </summary>
        public string? Address
        {
            get;
            set;
        }

        /// <summary>
        /// Endpoint accepting active health check probes. E.g. http://127.0.0.1:1234/.
        /// </summary>
        public string? Health
        {
            get;
            set;
        }

        /// <summary>
        /// Arbitrary key-value pairs that further describe this destination.
        /// </summary>
        public IDictionary<string, string>? Metadata
        {
            get;
            set;
        }
    }
}
