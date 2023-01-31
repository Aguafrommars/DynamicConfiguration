// Project: Aguafrommars/DynamicConfiguration
// Copyright (c) 2021 @Olivier Lefebvre

using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
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
            _subscriber.Subscribe(_source.Channel, (channel, value) =>
            {
                Load();
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
            base.Set(key, Sanitized(value));
            var oldJson = _database.HashGet(_source.HashKey, key);
            if (oldJson != default)
            {
                var data = JsonConfigurationParser.Parse(oldJson);
                foreach (var kv in data)
                {
                    _database.HashDelete(_source.HashKey, $"{key}{ConfigurationPath.KeyDelimiter}{kv.Key}");
                }
            }
            _database.HashSet(_source.HashKey, new[] { new HashEntry(key, value) });
            _subscriber.Publish(_source.Channel, key);
        }

        /// <summary>
        /// Loads configuration values from the source represented by this <see cref="RedisConfigurationProvider"/>.
        /// </summary>
        public override void Load()
        {
            Data = new Dictionary<string, string>();
            base.Load();
            var entryList = _database.HashGetAll(_source.HashKey).OrderBy(k => k.Name);

            foreach (var entry in entryList)
            {
                Load(entry);
            }
        }

        private void Load(HashEntry entry)
        {
            Data[entry.Name] = Sanitized(entry.Value);
            try
            {
                var data = JsonConfigurationParser.Parse(entry.Value);
                foreach (var kv in data)
                {
                    Data[$"{entry.Name}{ConfigurationPath.KeyDelimiter}{kv.Key}"] = kv.Value;
                }
            }
            catch (JsonException)
            {
                // value is not a JSON object
            }
        }

        private static string Sanitized(string value)
        => value.StartsWith("\"") ? value.Substring(1, value.Length - 2) : value;
    }
}
