# Aguacongas.DynamicConfiguration

Interfaces and services to dynamically configure .NET programs.

## Usage

### Setup

```cs
services.AddConfigurationServices(builder.Configuration, options => options.Provider = ((IConfigurationRoot)configuration).Providers.First(p => p is RedisConfigurationProvider));
```

> You need a configuration provider able to persist the configuration like [Aguacongas.DynamicConfiguration.Redis](../Aguacongas.DynamicConfiguration.Redis/README.md) in your configuration pipeline to persist configuration changes.   


### Read/Write configuration

```cs
/// <summary>
/// Configuration controller
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Authorize]
[Produces("application/json")]
public class ConfigurationController : ControllerBase
{
    private readonly IConfigurationService _service;

    /// <summary>
    /// Initialize a new instance of <see cref="ConfigurationController"/>
    /// </summary>
    /// <param name="service">A <see cref="IConfigurationService"/> instance</param>
    /// <exception cref="ArgumentNullException">If service is null</exception>
    public ConfigurationController(IConfigurationService service)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
    }

    // GET: api/<Configuration>
    /// <summary>
    /// Gets a configuration
    /// </summary>
    /// <param name="typeName">The configuration's assembly-qualified type name</param>
    /// <param name="key">The configuration's key</param>
    /// <returns>The configuration</returns>
    [HttpGet("{typeName}")]
    [HttpGet("{typeName}/{key}")]
    [Authorize(Policy = DYNAMIC_CONFIGURATION_READER_POLICY)]
    public Task<object> Get(string typeName, string? key)
    => _service.GetAsync(typeName, key);

    // PUT api/<Configuration>
    /// <summary>
    /// Sets a configuration
    /// </summary>
    /// <param name="key">The configuration's key</param>
    /// <param name="json">The JSON serialized value</param>
    /// <returns></returns>
    [HttpPut("{key}")]
    [Consumes(RawRequestBodyFormatter.CONTENTTYPE)]
    [Authorize(Policy = DYNAMIC_CONFIGURATION_WRITTER_POLICY)]
    public Task PutCluster(string key, [FromBody] string json)
    => _service.SetAsync(key, json);
}
```

> You can use [Aguacongas.DynamicConfiguration.WebApi](../Aguacongas.DynamicConfiguration.WebApi/README.md). It provides the same controller.

### Subscribe to configuration changes

```cs
var autoReloadService = app.ApplicationServices.GetRequiredService<IAutoReloadConfigurationService>();
autoReloadService.SubscribeToChanges();
```