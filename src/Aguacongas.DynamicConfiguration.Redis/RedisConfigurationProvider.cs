// Project: Aguafrommars/TheIdServer
// Copyright (c) 2021 @Olivier Lefebvre
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using System;
using System.Linq;
using System.Text.Json;

namespace Aguacongas.DynamicConfiguration.Redis
{
    /// <summary>
    /// Provides configuration key/values for an application stored in a Redis HashKey.
    /// </summary>
    /// <seealso cref="ConfigurationProvider"/>
    public class RedisConfigurationProvider : ConfigurationProvider
    {
        private readonly IRedisConfigurationSource _source;
        private readonly IDatabase _database;
        private readonly ISubscriber _subscriber;

        /// <summary>
        /// Initialize a new instance of <see cref="RedisConfigurationProvider"/>
        /// </summary>
        /// <param name="source">A redis configuration source</param>
        /// <exception cref="ArgumentNullException"></exception>
        public RedisConfigurationProvider(IRedisConfigurationSource source)
        {
            _source = source ?? throw new ArgumentNullException(nameof(source));
            var connection = source.Connection;
            _database = connection.GetDatabase(_source.Database ?? -1);
            _subscriber = connection.GetSubscriber();
            _subscriber.Subscribe(_source.Channel).OnMessage(message =>
            {
                var value = _database.HashGet(_source.HashKey, message.Message);
                Load(new HashEntry(message.Message, value));
                OnReload();
            });
        }

        /// <summary>
        /// Sets a configuration value for the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public override void Set(string key, string value)
        {
            _database.HashSet(_source.HashKey, new[] { new HashEntry(key, value) });
            _subscriber.Publish(_source.Channel, key);
        }

        /// <summary>
        /// Loads configuration values from the source represented by this <see cref="RedisConfigurationProvider"/>.
        /// </summary>
        public override void Load()
        {
            var entryList = _database.HashGetAll(_source.HashKey).OrderBy(k => k.Name);
            
            foreach(var entry in entryList)
            {
                Load(entry);
            }
        }

        private void Load(HashEntry entry)
        {
            Data[entry.Name] = entry.Value;
            try
            {
                var data = JsonConfigurationParser.Parse(entry.Value);
                foreach (var kv in data)
                {
                    Data[$"{entry.Name}{ConfigurationPath.KeyDelimiter}{kv.Key}"] = kv.Value;
                }
            }
            catch(JsonException)
            {
                // value is not a JSON object
            }
        }
    }
}
