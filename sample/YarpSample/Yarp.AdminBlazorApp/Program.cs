using Aguacongas.DynamicConfiguration.Razor;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Yarp.AdminBlazorApp;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
var rootComponents = builder.RootComponents;
rootComponents.Add<App>("#app");
rootComponents.Add<HeadOutlet>("head::after");

var services = builder.Services;
var configuration = builder.Configuration;

// The Settings components requires authentication.
// Update this code with the remote authorization service of your choice
services.AddOidcAuthentication(options => 
{
    options.ProviderOptions.Authority = builder.HostEnvironment.BaseAddress;
});

// Configure the httpClientBuilder to meet your requirement by adding an authorization message handler.
var httpClientBuilder = services.AddConfigurationService(configuration.GetSection("SettingsOptions"));

services.AddAuthorizationCore(options =>
{
    // The Settings component requires the policiy, DYNAMIC_CONFIGURATION_WRITTER_POLICY
    // In this sample this policy requires nothing, all users can save data.
    // You should update this code to meet your requirement.
    options.AddPolicy(Settings.DYNAMIC_CONFIGURATION_WRITTER_POLICY,
        builder => builder.RequireAssertion(context => true));
});
await builder.Build().RunAsync();
