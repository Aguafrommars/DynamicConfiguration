// Project: Aguafrommars/TheIdServer
// Copyright (c) 2021 @Olivier Lefebvre
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using System;

namespace Aguacongas.DynamicConfiguration.Redis
{
    /// <summary>
    /// Represents a Redis source of configuration key/values for an application.
    /// </summary>
    public class RedisConfigurationSource : IConfigurationSource, IRedisConfigurationSource, IDisposable
    {
        private bool disposedValue;
        private string ConnectionString => RedisConfigurationOptions?.ConnectionString;

        /// <summary>
        /// Gets the database.
        /// </summary>
        public int? Database => RedisConfigurationOptions?.Database;

        /// <summary>
        /// Gets the publication channel.
        /// </summary>
        public string Channel => RedisConfigurationOptions?.Channel;

        /// <summary>
        /// Gets the hash key.
        /// </summary>

        public string HashKey => RedisConfigurationOptions?.HashKey;

        /// <summary>
        /// Get the connection.
        /// </summary>
        public IConnectionMultiplexer Connection { get; private set; }

        /// <summary>
        /// Gets or sets the Redis configuration options
        /// </summary>
        public RedisConfigurationOptions RedisConfigurationOptions { get; set; }

        /// <summary>
        /// Builds the <see cref="RedisConfigurationProvider"/> for this source.
        /// </summary>
        /// <param name="builder">The <see cref="IConfigurationBuilder"/>.</param>
        /// <returns></returns>
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

            if (string.IsNullOrEmpty(Channel))
            {
                throw new InvalidOperationException($"'Channel' must be set.");
            }

            if (string.IsNullOrEmpty(HashKey))
            {
                throw new InvalidOperationException($"'HashKey' must be set.");
            }

            Connection = ConnectionMultiplexer.Connect(ConfigurationOptions.Parse(ConnectionString));
        }

        /// <inheritdoc/>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Connection?.Dispose();
                }

                disposedValue = true;
            }
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
