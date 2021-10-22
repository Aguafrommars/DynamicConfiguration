// Project: Aguafrommars/TheIdServer
// Copyright (c) 2021 @Olivier Lefebvre
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using System;

namespace Aguacongas.Configuration.Redis
{
    public class RedisConfigurationSource : IConfigurationSource, IRedisConfigurationSource
    {
        public string ConnectionString => RedisConfigurationOptions.ConnectionString;

        public int? Database => RedisConfigurationOptions.Database;

        public string Channel => RedisConfigurationOptions.Channel;

        public string HashKey => RedisConfigurationOptions.HashKey;

        public ConfigurationOptions ConfigurationOptions { get; private set; }

        public RedisConfigurationOptions RedisConfigurationOptions { get; set; }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            ValidateOptions();
            return new RedisConfigurationProvider(this);
        }

        private void ValidateOptions()
        {
            if (string.IsNullOrEmpty(ConnectionString))
            {
                throw new InvalidOperationException($"'ConnectionString' must be set.");
            }

            if (string.IsNullOrEmpty(ConnectionString))
            {
                throw new InvalidOperationException($"'Channel' must be set.");
            }

            if (string.IsNullOrEmpty(HashKey))
            {
                throw new InvalidOperationException($"'HashKey' must be set.");
            }

            ConfigurationOptions = ConfigurationOptions.Parse(ConnectionString);
        }

        public virtual IConnectionMultiplexer Connect()
        => ConnectionMultiplexer.Connect(ConfigurationOptions);
    }
}
