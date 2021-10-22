// Project: Aguafrommars/TheIdServer
// Copyright (c) 2021 @Olivier Lefebvre
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Aguacongas.Configuration.Redis
{
    public class RedisConfigurationProvider : IConfigurationProvider
    {
        private ConfigurationReloadToken _reloadToken = new ConfigurationReloadToken();
        private readonly IRedisConfigurationSource _source;
        private IConnectionMultiplexer _redis;
        private IDatabase _database;
        private ISubscriber _subscriber;
        private bool disposedValue;

        public RedisConfigurationProvider(IRedisConfigurationSource source)
        {
            _source = source ?? throw new ArgumentNullException(nameof(source));
        }

        public void Set(string key, string value)
        {
            ThrowIfDisposed();
            var data = JsonConfigurationParser.Parse(value);
            data.Add(key, value);
            _database.HashSet(_source.HashKey, data.Select(kv => new HashEntry($"{key}{ConfigurationPath.KeyDelimiter}{kv.Key}", kv.Value))
                .ToArray());
            _subscriber.Publish(_source.Channel, DateTime.UtcNow.Ticks);
        }

        public bool TryGet(string key, out string value)
        {
            ThrowIfDisposed();
            if (!_database.HashExists(_source.HashKey, key))
            {
                value = null;
                return false;
            }

            value = _database.HashGet(_source.HashKey, key);
            return true;
        }

        public IEnumerable<string> GetChildKeys(IEnumerable<string> earlierKeys, string parentPath)
        {
            ThrowIfDisposed();
            string prefix = (parentPath == null) ? string.Empty : (parentPath + ConfigurationPath.KeyDelimiter);
            var keyList = _database.HashKeys(_source.HashKey).Select(k => k.ToString());
            return (from key in keyList
                    where key.StartsWith(prefix, StringComparison.OrdinalIgnoreCase)
                    select Segment(key, prefix.Length)).Concat(earlierKeys).OrderBy((string k) => k, ConfigurationKeyComparer.Instance);
        }

        private void ThrowIfDisposed()
        {
            if (disposedValue)
            {
                throw new ObjectDisposedException(nameof(RedisConfigurationProvider));
            }
        }

        public void Load()
        {
            ThrowIfDisposed();
            if (_redis == null)
            {
                _redis = _source.Connect();
                _database = _redis.GetDatabase(_source.Database ?? -1);
                _subscriber = _redis.GetSubscriber();
                _subscriber.Subscribe(_source.Channel).OnMessage(message =>
                {
                    OnReload();
                });
            }
        }

        public IChangeToken GetReloadToken() => _reloadToken;

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _redis?.Dispose();
                    _redis = null;
                }

                disposedValue = true;
            }
        }

        private void OnReload()
        {
            var previousToken = Interlocked.Exchange(ref _reloadToken, new ConfigurationReloadToken());
            previousToken.OnReload();
        }

        private static string Segment(string key, int prefixLength)
        {
            int num = key.IndexOf(ConfigurationPath.KeyDelimiter, prefixLength, StringComparison.OrdinalIgnoreCase);
            if (num >= 0)
            {
                return key.Substring(prefixLength, num - prefixLength);
            }

            return key.Substring(prefixLength);
        }

    }
}
