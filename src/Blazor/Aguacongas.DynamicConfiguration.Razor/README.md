# Aguacongas.DynamicConfiguration.Razor

Razor components to configure your .NET application dymamically.

## Usage

### Setup

Using a Blazor application.

You need to define a class to bind to your whole configuration:

```cs
/// <summary>
/// Defines the configuration we can override
/// </summary>
public class ServerConfig
{
    /// <summary>
    /// Gets or sets the reverse proxy configuration
    /// </summary>
    public ProxyConfig? ReverseProxy { get; set; }

    /// <summary>
    /// Gets or sets the logging configuration
    /// </summary>
    public LoggingConfig? Logging { get; set; }
}
```

This class must be shared by the blazor application and the web API.

The Blazor app *appsettings.json* should contains a **SettingsOptions**:

```json
{
  "SettingsOptions": {
    "TypeName": "Yarp.Configuration.Model.ServerConfig, Yarp.Configuration",
    "ApiUrl": "https://localhost:7035/api/configuration"
  }
}
```

The *Program.cs* can look like:

```cs
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
```

Review the [Yarp.AdminBlazorApp](https://github.com/Aguafrommars/DynamicConfiguration/tree/main/sample/YarpSample) sample.