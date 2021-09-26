// Project: Aguafrommars/TheIdServer
// Copyright (c) 2021 @Olivier Lefebvre
using Serilog;
using System.Linq;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;
configuration.AddRedis(configuration.GetValue<string>("Redis:Connection"), configuration.GetValue<int?>("Redis:Database"));

builder.Host.UseSerilog((context, config) => config.ReadFrom.Configuration(builder.Configuration));

// Add services to the container.
builder.Services.AddTransient(p => builder.Configuration as IConfigurationRoot);
builder.Services.AddControllers();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Aguacongas.TheReverseProxy.AdminApi", Version = "v1" });
}); 

var proxyBuilder = builder.Services.AddReverseProxy();

// Initialize the reverse proxy from the "ReverseProxy" section of configuration
proxyBuilder.LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Aguacongas.TheReverseProxy.AdminApi v1"));
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
