// Project: Aguafrommars/TheIdServer
// Copyright (c) 2021 @Olivier Lefebvre
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using System;

namespace Aguacongas.Configuration.Redis
{
    public class RedisConfigurationSource : IConfigurationSource
    {
        public ConfigurationOptions Options { get; set; }

        public string Connection  { get; set; }

        public int? Database { get; set; }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            if (Options == null)
            {
                if (string.IsNullOrEmpty(Connection))
                {
                    throw new InvalidOperationException($"'Configuration' or 'Connection' must be set.");
                }

                Options = new ConfigurationOptions();
                var endpoints = Connection.Split(';');
                foreach(var endpoint in endpoints)
                {
                    Options.EndPoints.Add(endpoint);
                }
            }

            return new RedisConfigurationProvider(this);
        }
    }
}
