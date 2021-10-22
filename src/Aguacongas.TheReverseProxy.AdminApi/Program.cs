// Project: Aguafrommars/TheIdServer
// Copyright (c) 2021 @Olivier Lefebvre
using Aguacongas.TheReverseProxy.Model;
using Serilog;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;
configuration.AddRedis(options => configuration.GetSection("Redis").Bind(options));

builder.Host.UseSerilog((context, config) => config.ReadFrom.Configuration(configuration));

var services = builder.Services;
// Add services to the container.
services.Configure<ReverseProxyOptions>(configuration.GetSection("ReverseProxyOptions"))
    .AddControllers()
    .AddTheReverseProxyAdmin()
    .AddJsonOptions(options =>
    {
        var serializationOptions = options.JsonSerializerOptions;
        serializationOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        serializationOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    });

services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Aguacongas.TheReverseProxy.AdminApi", Version = "v1" });
});

services.AddTransient(p => builder.Configuration as IConfigurationRoot);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger()
        .UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Aguacongas.TheReverseProxy.AdminApi v1"));
}

app.UseHttpsRedirection()
    .UseAuthorization();

app.MapControllers();

app.Run();
