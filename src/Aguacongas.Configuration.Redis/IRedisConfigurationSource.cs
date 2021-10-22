// Project: Aguafrommars/TheIdServer
// Copyright (c) 2021 @Olivier Lefebvre
using StackExchange.Redis;

namespace Aguacongas.Configuration.Redis
{
    public interface IRedisConfigurationSource
    {
        string Channel { get; }
        int? Database { get; }
        string HashKey { get; }

        IConnectionMultiplexer Connect();
    }
}