// Project: Aguafrommars/TheIdServer
// Copyright (c) 2021 @Olivier Lefebvre
using StackExchange.Redis;

namespace Aguacongas.DynamicConfiguration.Redis
{
    /// <summary>
    /// Represents a Redis configuration source.
    /// </summary>
    public interface IRedisConfigurationSource
    {
        /// <summary>
        /// Gets the publication channel.
        /// </summary>
        string Channel { get; }

        /// <summary>
        /// Gets the database.
        /// </summary>
        int? Database { get; }

        /// <summary>
        /// Gets the hash key.
        /// </summary>
        string HashKey { get; }

        /// <summary>
        /// Get the connection.
        /// </summary>
        IConnectionMultiplexer Connection { get; }
    }
}