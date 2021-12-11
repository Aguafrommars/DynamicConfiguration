// Project: Aguafrommars/TheIdServer
// Copyright (c) 2021 @Olivier Lefebvre
using System.Collections.Generic;

namespace Aguacongas.TheReverseProxy.Model
{
    /// <summary>
    /// Route criteria for a query parameter that must be present on the incoming request.
    /// </summary>
    public class RouteQueryParameter
    {
        /// <summary>
        /// Name of the query parameter to look for. This field is case insensitive and required.
        /// </summary>
        public string? Name
        {
            get;
            set;
        }

        /// <summary>
        /// A collection of acceptable query parameter values used during routing.
        /// </summary>
        public IEnumerable<string>? Values
        {
            get;
            set;
        }

        /// <summary>
        /// Specifies how query parameter values should be compared (e.g. exact matches Vs.
        /// contains). Defaults to Yarp.ReverseProxy.Configuration.QueryParameterMatchMode.Exact.
        /// </summary>
        public QueryParameterMatchMode Mode
        {
            get;
            set;
        }

        /// <summary>
        /// Specifies whether query parameter value comparisons should ignore case. When
        /// true, System.StringComparison.Ordinal is used. When false, System.StringComparison.OrdinalIgnoreCase
        /// is used. Defaults to false.
        /// </summary>
        public bool IsCaseSensitive
        {
            get;
            set;
        }
    }
}
