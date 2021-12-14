// Project: Aguafrommars/TheIdServer
// Copyright (c) 2021 @Olivier Lefebvre
using Aguacongas.DynamicConfiguration.Redis;
using System;

namespace Microsoft.Extensions.Configuration
{
    /// <summary>
    /// <see cref="IConfigurationBuilder"/> extensions
    /// </summary>
    public static class ConfigurationBuilderExtensions
    {
        /// <summary>
        /// Adds Redis configuration.
        /// </summary>
        /// <param name="builder">The <see cref="IConfigurationBuilder"/>.</param>
        /// <param name="options">The <see cref="RedisConfigurationOptions"/>.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IConfigurationBuilder AddRedis(this IConfigurationBuilder builder, RedisConfigurationOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            return builder.Add<RedisConfigurationSource>(source => source.RedisConfigurationOptions = options);            
        }

        /// <summary>
        /// Adds Redis configuration
        /// </summary>
        /// <param name="builder">The <see cref="IConfigurationBuilder"/>.</param>
        /// <param name="configure">An action to configure <see cref="RedisConfigurationOptions"/>.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IConfigurationBuilder AddRedis(this IConfigurationBuilder builder, Action<RedisConfigurationOptions> configure)
        {
            if (configure == null)
            {
                throw new ArgumentNullException(nameof(configure));
            }

            var options = new RedisConfigurationOptions();
            configure(options);

            return builder.AddRedis(options);
        }
    }
}
