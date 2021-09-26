// Project: Aguafrommars/TheIdServer
// Copyright (c) 2021 @Olivier Lefebvre
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using System;

namespace Aguacongas.Configuration.Redis
{
    public class RedisConfigurationProvider : ConfigurationProvider, IDisposable
    {
        private static readonly string CHANNEL = typeof(RedisConfigurationProvider).FullName;
        private readonly RedisConfigurationSource _source;
        private ConnectionMultiplexer _redis;
        private IDatabase _database;
        private ISubscriber _subscriber;
        private bool disposedValue;

        public RedisConfigurationProvider(RedisConfigurationSource source)
        {
            _source = source ?? throw new ArgumentNullException(nameof(source));
        }

        public override void Set(string key, string value)
        {
            var data = JsonConfigurationParser.Parse(value);
            foreach(var kv in data)
            {
                _database.StringSet($"{key}{ConfigurationPath.KeyDelimiter}{kv.Key}", kv.Value);
            }
            
            _subscriber.Publish(CHANNEL, DateTime.UtcNow.Ticks);
        }

        public override bool TryGet(string key, out string value)
        {
            if (!_database.KeyExists(key) || (value = _database.StringGet(key)) == null)
            {
                value = null;
                return false;
            }

            return true;
        }

        public override void Load()
        {
            if (_redis == null)
            {
                _redis = ConnectionMultiplexer.Connect(_source.Options);
                _database = _redis.GetDatabase(_source.Database ?? _source.Options.DefaultDatabase ?? -1);
                _subscriber = _redis.GetSubscriber();
                _subscriber.Subscribe(CHANNEL).OnMessage(message =>
                {
                    OnReload();
                });
            }
            base.Load();
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

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
