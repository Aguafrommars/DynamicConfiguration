// Project: Aguafrommars/TheIdServer
// Copyright (c) 2021 @Olivier Lefebvre
using System.Collections.Generic;

namespace Yarp.Configuration.Model
{
    public class RouteMatch
    {
        /// <summary>
        /// Only match requests that use these optional HTTP methods. E.g. GET, POST.
        /// </summary>
        public IEnumerable<string>? Methods
        {
            get;
            set;
        }

        /// <summary>
        /// Only match requests with the given Host header. Supports wildcards and ports.
        /// For unicode host names, do not use punycode.
        /// </summary>
        public IEnumerable<string>? Hosts
        {
            get;
            set;
        }

        /// <summary>
        /// Only match requests with the given Path pattern.
        /// </summary>
        public string? Path
        {
            get;
            set;
        }

        /// <summary>
        /// Only match requests that contain all of these query parameters.
        /// </summary>
        public IEnumerable<RouteQueryParameter>? QueryParameters
        {
            get;
            set;
        }

        /// <summary>
        /// Only match requests that contain all of these headers.
        /// </summary>
        public IEnumerable<RouteHeader>? Headers
        {
            get;
            set;
        }
    }
}
