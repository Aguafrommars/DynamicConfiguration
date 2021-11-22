using Aguacongas.Configuration.Razor.Options;
using Aguacongas.Configuration.Razor.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RichardSzalay.MockHttp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace Aguacongas.Configuration.Razor.Test.Services
{
    public class ConfigurationServiceTest
    {
        [Fact]
        public void constructor_should_validate_parameters()
        {
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            Assert.Throws<ArgumentNullException>(() => new ConfigurationService(null, null));
            Assert.Throws<ArgumentNullException>(() => new ConfigurationService(new HttpClient(), null));
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
        }

        [Fact]
        public async Task GetAsync_should_throw_when_setting_type_name_is_null()
        {
            var options = Microsoft.Extensions.Options.Options.Create(new SettingsOptions
            {
                TypeName = null
            });

            var sut = new ConfigurationService(new HttpClient(), options);

            await Assert.ThrowsAsync<InvalidOperationException>(() => sut.GetAsync(null, default));
        }

        [Fact]
        public async Task GetAsync_should_throw_when_setting_type_is_unknown()
        {
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["ApiUrl"] = "https://localhost/settings",
                    ["TypeName"] = "unknow"
                })
                .Build();
            var services = new ServiceCollection().AddConfigurationService(configuration).BuildServiceProvider();
            var sut = services.GetRequiredService<IConfigurationService>();

            await Assert.ThrowsAsync<InvalidOperationException>(() => sut.GetAsync(null, default));
        }

        [Fact]
        public async Task GetAsync_should_return_configuration_when_key_is_null()
        {
            var options = Microsoft.Extensions.Options.Options.Create(new SettingsOptions());

            var expected = new Dictionary<string, object>
            {
                ["test"] = "test"
            };

            var mockHttpHandler = new MockHttpMessageHandler();
            var httpClient = mockHttpHandler.ToHttpClient();
            httpClient.BaseAddress = new Uri("https://localhost/settings");
            mockHttpHandler.When("https://localhost/settings").Respond(req =>
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new StringContent(JsonSerializer.Serialize(expected));
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                return response;
            });
            var sut = new ConfigurationService(httpClient, options);

            var congiguration = await sut.GetAsync(null, default);
            Assert.NotNull(congiguration);
            var dictionary = congiguration as Dictionary<string, object>;
            Assert.NotNull(dictionary);
            Assert.Contains("test", dictionary?.Keys);
        }
    }
}
