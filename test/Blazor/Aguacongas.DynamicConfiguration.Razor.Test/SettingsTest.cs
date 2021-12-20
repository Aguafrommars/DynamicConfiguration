using Aguacongas.DynamicConfiguration.Razor.Options;
using Aguacongas.DynamicConfiguration.Razor.Services;
using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using RichardSzalay.MockHttp;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;

namespace Aguacongas.DynamicConfiguration.Razor.Test
{
    public class SettingsTest : TestContext
    {
        [Fact]
        public void OnParametersSetAsync_should_throw_when_Service_is_null()
        {
            var mockHttpHandler = new MockHttpMessageHandler();
            var httpClient = mockHttpHandler.ToHttpClient();
            httpClient.BaseAddress = new Uri("http://localhost");
            var httpClientFactoryMock = new Mock<IHttpClientFactory>();
            httpClientFactoryMock.Setup(m => m.CreateClient(It.IsAny<string>())).Returns(httpClient);

            Services.AddSingleton(p => httpClientFactoryMock.Object);

            var options = Microsoft.Extensions.Options.Options.Create(new SettingsOptions
            { 
                TypeName = typeof(SettingsTest).AssemblyQualifiedName
            });

            Services.AddTransient<IConfigurationService, ConfigurationService>();
            Services.AddTransient(p => options);
            var cut = RenderComponent<Settings>();

            var instance = cut.Instance;
            if (instance is null)
            {
                throw new InvalidOperationException("Instance cannot be null.");
            }

            instance.GetType().GetProperty("Service", BindingFlags.Instance | BindingFlags.NonPublic)?.SetValue(cut.Instance, null);

            Assert.Throws<InvalidOperationException>(() => cut.SetParametersAndRender(ComponentParameter.CreateParameter("Path", null)));
        }

        [Fact]
        public async Task SetRootPath_should_verify_root_path()
        {
            var mockHttpHandler = new MockHttpMessageHandler();
            var httpClient = mockHttpHandler.ToHttpClient();
            httpClient.BaseAddress = new Uri("http://localhost");
            var httpClientFactoryMock = new Mock<IHttpClientFactory>();
            httpClientFactoryMock.Setup(m => m.CreateClient(It.IsAny<string>())).Returns(httpClient);

            Services.AddSingleton(p => httpClientFactoryMock.Object);

            var options = Microsoft.Extensions.Options.Options.Create(new SettingsOptions
            {
                TypeName = typeof(SettingsTest).AssemblyQualifiedName
            });

            Services.AddTransient<IConfigurationService, ConfigurationService>();
            Services.AddTransient(p => options);
            var cut = RenderComponent<Settings>();

            await Assert.ThrowsAsync<InvalidOperationException>(() => cut.Instance.SetParametersAsync(ParameterView.FromDictionary(new Dictionary<string, object?>
            {
                ["RootPath"] = "/test"
            })));
            await Assert.ThrowsAsync<InvalidOperationException>(() => cut.Instance.SetParametersAsync(ParameterView.FromDictionary(new Dictionary<string, object?>
            {
                ["RootPath"] = ""
            })));
        }
    }
}
