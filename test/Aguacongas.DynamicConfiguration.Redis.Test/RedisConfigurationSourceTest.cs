using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Aguacongas.DynamicConfiguration.Redis.Test
{
    public class RedisConfigurationSourceTest
    {
        [Fact]
        public void Build_should_verify_options()
        {
            using var redisConfigurationSource = new RedisConfigurationSource();
            Assert.Throws<InvalidOperationException>(() => redisConfigurationSource.Build(null));

            var options = new RedisConfigurationOptions
            {
                ConnectionString = "localhost"
            };

            redisConfigurationSource.RedisConfigurationOptions = options;

            Assert.Throws<InvalidOperationException>(() => redisConfigurationSource.Build(null));

            options.Channel = "test";

            Assert.Throws<InvalidOperationException>(() => redisConfigurationSource.Build(null));

            options.HashKey = "test";

            try
            {
                redisConfigurationSource.Build(null);
            }
            catch(RedisConnectionException)
            {
                // 
            }
            redisConfigurationSource.Dispose();
        }
    }
}
