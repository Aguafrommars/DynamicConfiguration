using Aguacongas.DynamicConfiguration.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Xunit;

namespace Aguacongas.DynamicConfiguration.Test
{
    public class ServiceCollectionExtensionsTest
    {
        [Fact]
        public void AddConfigurationServices_should_add_services_in_DI()
        {
            var configuration = new ConfigurationBuilder().AddInMemoryCollection().Build();

            var services = new ServiceCollection()
                .AddConfigurationServices(configuration, options => options.Provider = configuration.Providers.First())
                .BuildServiceProvider();

            var serviceProviderIsService = services.GetRequiredService<IServiceProviderIsService>();
            Assert.True(serviceProviderIsService.IsService(typeof(IConfigurationService)));
            Assert.True(serviceProviderIsService.IsService(typeof(IAutoReloadConfigurationService)));
        }
    }
}
