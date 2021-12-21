
using Aguacongas.DynamicConfiguration.Redis;

var builder = WebApplication.CreateBuilder(args);

// Configure the reverse proxy with redis.
var configuration = builder.Configuration;
configuration.AddRedis(options => configuration.GetSection("Redis").Bind(options));

// Add services to the container.
builder.Services.AddConfigurationServices(configuration,
    options => options.Provider = ((IConfigurationRoot)configuration).Providers.First(p => p is RedisConfigurationProvider))
    .AddReverseProxy()
    .LoadFromConfig(configuration.GetSection("ReverseProxy"));

var app = builder.Build();

// Auto reload the configuration when changes occur.
app.SubscribeToConfigurationChanges();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.MapReverseProxy();

app.Run();
