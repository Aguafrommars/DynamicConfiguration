// Project: Aguafrommars/DynamicConfiguration
// Copyright (c) 2021 @Olivier Lefebvre

using Aguacongas.DynamicConfiguration.Razor.Options;
using Aguacongas.DynamicConfiguration.Razor.Services;
using Bunit;
using Bunit.TestDoubles;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using RichardSzalay.MockHttp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace Aguacongas.DynamicConfiguration.Razor.Test
{
    public class SettingsTest : TestContext
    {
        [Fact]
        public async Task OnParametersSetAsync_should_throw_when_Service_is_null()
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

            Services.AddLocalization().AddScoped<ISettingsLocalizer, DefaultSettingsLocalizer>().AddTransient<IConfigurationService, ConfigurationService>();
            Services.AddTransient(p => options);
            var cut = RenderComponent<Settings>();

            var instance = cut.Instance;
            if (instance is null)
            {
                throw new InvalidOperationException("Instance cannot be null.");
            }

            instance.GetType().GetProperty("Service", BindingFlags.Instance | BindingFlags.NonPublic)?.SetValue(cut.Instance, null);

            await Assert.ThrowsAsync<InvalidOperationException>(() => instance.SetParametersAsync(ParameterView.FromDictionary(new Dictionary<string, object?>
            {
                ["Path"] = null
            })));
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

            Services.AddLocalization().AddScoped<ISettingsLocalizer, DefaultSettingsLocalizer>().AddTransient<IConfigurationService, ConfigurationService>();
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

        [Fact]
        public void Should_not_display_link_on_dictionary_property_but_key()
        {
            var authContext = this.AddTestAuthorization();
            authContext.SetAuthorized("test");

            var model = new Model
            {
                Child = new Model
                {
                    Dictionary = new Dictionary<string, string>
                    {
                        ["test"] = "test"
                    }
                }
            };

            var mockHttpHandler = new MockHttpMessageHandler();
            mockHttpHandler.When(HttpMethod.Get, $"http://localhost/{model.GetType().AssemblyQualifiedName}")
                .Respond(() =>
                {
                    var response = new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new StringContent(JsonSerializer.Serialize(model))
                    };
                    response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    return Task.FromResult(response);
                });
            var httpClient = mockHttpHandler.ToHttpClient();
            httpClient.BaseAddress = new Uri("http://localhost");
            var httpClientFactoryMock = new Mock<IHttpClientFactory>();
            httpClientFactoryMock.Setup(m => m.CreateClient(It.IsAny<string>())).Returns(httpClient);

            Services.AddSingleton(p => httpClientFactoryMock.Object);

            var options = Microsoft.Extensions.Options.Options.Create(new SettingsOptions
            {
                TypeName = model.GetType().AssemblyQualifiedName
            });

            Services.AddLocalization().AddScoped<ISettingsLocalizer, DefaultSettingsLocalizer>().AddTransient<IConfigurationService, ConfigurationService>();
            Services.AddTransient(p => options);
            var cut = RenderComponent<Settings>(parameters => parameters
                .Add(p => p.RootPath, "/")
                .Add(p => p.Path, "Child:Dictionary:test"));

            cut.WaitForState(() => !cut.Markup.Contains("Loading..."), TimeSpan.FromSeconds(2));

            Assert.Contains("<span>Dictionary</span>",
                cut.Markup);
        }

        class Model
        {
            public IDictionary<string, string>? Dictionary { get; set; }

            public Model? Child { get; set; }
        }
    }
}
