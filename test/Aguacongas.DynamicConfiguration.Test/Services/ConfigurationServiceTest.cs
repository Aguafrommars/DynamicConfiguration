// Project: Aguafrommars/DynamicConfiguration
// Copyright (c) 2021 @Olivier Lefebvre

using Aguacongas.DynamicConfiguration.Options;
using Aguacongas.DynamicConfiguration.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Memory;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace Aguacongas.DynamicConfiguration.Test.Services
{
    public class ConfigurationServiceTest
    {
        [Fact]
        public void Constructor_should_verify_arguments()
        {
            Assert.Throws<ArgumentNullException>(() => new ConfigurationService(null, null));
            Assert.Throws<ArgumentNullException>(() => new ConfigurationService(new Mock<IConfigurationRoot>().Object, null));
        }

        [Fact]
        public async Task GetAsync_should_verify_arguments()
        {
            var configuration = new ConfigurationBuilder().AddInMemoryCollection().Build();

            var sut = new ConfigurationService(configuration, Microsoft.Extensions.Options.Options.Create(new DynamicConfigurationOptions
            {
                Provider = configuration.Providers.First()
            }));

            await Assert.ThrowsAsync<ArgumentException>(() => sut.GetAsync(null)).ConfigureAwait(false);

            await Assert.ThrowsAsync<ArgumentException>(() => sut.GetAsync(string.Empty)).ConfigureAwait(false);
        }

        [Fact]
        public async Task GetAsync_should_get_value_type_from_configuration_key()
        {
            var configuration = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string>
            {
                ["key"] = "0"
            }).Build();

            var sut = new ConfigurationService(configuration, Microsoft.Extensions.Options.Options.Create(new DynamicConfigurationOptions
            {
                Provider = configuration.Providers.First()
            }));

            var value = (int)await sut.GetAsync(typeof(int).AssemblyQualifiedName, "key").ConfigureAwait(false);

            Assert.Equal(0, value);
        }

        [Fact]
        public async Task GetAsync_should_get_string_from_configuration_key()
        {
            var configuration = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string>
            {
                ["key"] = "string"
            }).Build();

            var sut = new ConfigurationService(configuration, Microsoft.Extensions.Options.Options.Create(new DynamicConfigurationOptions
            {
                Provider = configuration.Providers.First()
            }));

            var value = (string)await sut.GetAsync(typeof(string).AssemblyQualifiedName, "key").ConfigureAwait(false);

            Assert.Equal("string", value);
        }

        [Fact]
        public async Task GetAsync_should_get_typed_object_from_configuration()
        {
            var configuration = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string>
            {
                ["key"] = "string"
            }).Build();

            var sut = new ConfigurationService(configuration, Microsoft.Extensions.Options.Options.Create(new DynamicConfigurationOptions
            {
                Provider = configuration.Providers.First()
            }));

            var config = (Dictionary<string, string>)await sut.GetAsync(typeof(Dictionary<string, string>).AssemblyQualifiedName).ConfigureAwait(false);

            Assert.Contains(config, kv => kv.Key == "key" && kv.Value == "string");
        }

        [Fact]
        public async Task GetAsync_should_get_typed_object_from_configuration_key()
        {
            var configuration = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string>
            {
                ["key:key"] = "string"
            }).Build();

            var sut = new ConfigurationService(configuration, Microsoft.Extensions.Options.Options.Create(new DynamicConfigurationOptions
            {
                Provider = configuration.Providers.First()
            }));

            var config = (Dictionary<string, string>)await sut.GetAsync(typeof(Dictionary<string, string>).AssemblyQualifiedName, "key").ConfigureAwait(false);

            Assert.Contains(config, kv => kv.Key == "key" && kv.Value == "string");
        }

        [Fact]
        public async Task SetAsync_should_set_configuration_at_key()
        {
            var configuration = new ConfigurationBuilder().AddInMemoryCollection().Build();

            var provider = configuration.Providers.First(p => p is MemoryConfigurationProvider);
            var sut = new ConfigurationService(configuration, Microsoft.Extensions.Options.Options.Create(new DynamicConfigurationOptions
            {
                Provider = provider
            }));

            var json = JsonSerializer.Serialize(new Dictionary<string, string>
            {
                ["key"] = "string"
            });

            await sut.SetAsync("key", json).ConfigureAwait(false);

            Assert.True(provider.TryGet("key", out string value));
            Assert.Equal(json, value);
        }
    }
}
