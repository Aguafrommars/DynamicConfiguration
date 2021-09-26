// Project: Aguafrommars/TheIdServer
// Copyright (c) 2021 @Olivier Lefebvre
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;
configuration.AddRedis(configuration.GetValue<string>("Redis:Connection"), configuration.GetValue<int?>("Redis:Database"));

builder.Host.UseSerilog((context, config) => config.ReadFrom.Configuration(builder.Configuration));

var proxyBuilder = builder.Services.AddReverseProxy();
// Initialize the reverse proxy from the "ReverseProxy" section of configuration
proxyBuilder.LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var app = builder.Build();

if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseRouting()
    .UseEndpoints(endpoints =>
    {
        endpoints.MapReverseProxy();
    });

await app.RunAsync().ConfigureAwait(false);
