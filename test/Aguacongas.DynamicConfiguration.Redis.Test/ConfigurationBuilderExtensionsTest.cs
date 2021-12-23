// Project: Aguafrommars/DynamicConfiguration
// Copyright (c) 2021 @Olivier Lefebvre

using Microsoft.Extensions.Configuration;
using System;
using Xunit;

namespace Aguacongas.DynamicConfiguration.Redis.Test
{
    public class ConfigurationBuilderExtensionsTest
    {
        [Fact]
        public void AddRedis_should_add_RedisConfigurationSource()
        {
            var sut = new ConfigurationBuilder();

            sut.AddRedis(options =>
            {

            });

            Assert.Contains(sut.Sources, s => s is RedisConfigurationSource);
        }

        [Fact]
        public void AddRedis_should_add_verify_arguments()
        {
            var sut = new ConfigurationBuilder();

            Assert.Throws<ArgumentNullException>(() => sut.AddRedis(options: null));
            Assert.Throws<ArgumentNullException>(() => sut.AddRedis(configure: null));
        }
    }
}
