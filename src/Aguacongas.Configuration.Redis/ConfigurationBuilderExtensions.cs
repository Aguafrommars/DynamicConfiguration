// Project: Aguafrommars/TheIdServer
// Copyright (c) 2021 @Olivier Lefebvre
using Aguacongas.Configuration.Redis;
using StackExchange.Redis;

namespace Microsoft.Extensions.Configuration
{
    public static class ConfigurationBuilderExtensions
    {
        public static IConfigurationBuilder AddRedis(this IConfigurationBuilder builder, string connection, int? database)
        {
            return builder.Add(new RedisConfigurationSource
            {
                Connection = connection,
                Database = database
            });            
        }

        public static IConfigurationBuilder AddRedis(this IConfigurationBuilder builder, ConfigurationOptions options, int? database)
        {
            return builder.Add(new RedisConfigurationSource
            {
                Options = options,
                Database = database
            });
        }
    }
}
