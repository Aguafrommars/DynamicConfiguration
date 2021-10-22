// Project: Aguafrommars/TheIdServer
// Copyright (c) 2021 @Olivier Lefebvre
using Aguacongas.Configuration.Redis;
using System;

namespace Microsoft.Extensions.Configuration
{
    public static class ConfigurationBuilderExtensions
    {
        public static IConfigurationBuilder AddRedis(this IConfigurationBuilder builder, RedisConfigurationOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            return builder.Add<RedisConfigurationSource>(source => source.RedisConfigurationOptions = options);            
        }

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
