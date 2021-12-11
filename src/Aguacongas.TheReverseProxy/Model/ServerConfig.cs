// Project: Aguafrommars/TheIdServer
// Copyright (c) 2021 @Olivier Lefebvre
namespace Aguacongas.TheReverseProxy.Model
{
    public class ServerConfig
    {
        public string? AllowedHosts { get; set; }

        public ProxyConfig? ReverseProxy { get; set; }
    }
}
