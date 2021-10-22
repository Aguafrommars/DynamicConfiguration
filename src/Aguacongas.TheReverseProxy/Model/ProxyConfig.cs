// Project: Aguafrommars/TheIdServer
// Copyright (c) 2021 @Olivier Lefebvre
using System.Collections.Generic;

namespace Aguacongas.TheReverseProxy.Model
{
    public class ProxyConfig
    {
        public IDictionary<string, RouteConfig> Routes { get; set; } = new Dictionary<string, RouteConfig>();

        public IDictionary<string, ClusterConfig> Clusters { get; set; } = new Dictionary<string, ClusterConfig>();
    }
}
