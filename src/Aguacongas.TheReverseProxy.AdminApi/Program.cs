// Project: Aguafrommars/TheIdServer
// Copyright (c) 2021 @Olivier Lefebvre
using Aguacongas.Configuration.Redis;
using Aguacongas.TheReverseProxy.Model;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerUI;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;
configuration.AddRedis(options => configuration.GetSection(nameof(RedisConfigurationOptions)).Bind(options));

builder.Host.UseSerilog((context, config) => config.ReadFrom.Configuration(configuration));

var services = builder.Services;
// Add services to the container.
services.Configure<ReverseProxyOptions>(configuration.GetSection(nameof(ReverseProxyOptions)))
    .AddTheReverserProxyApiAuthorization(configuration);

services.AddControllersWithViews()
    .AddConfigurationWebAPI();
services.AddRazorPages();

services.AddSwaggerGenFromConfiguration(configuration)
    .AddTransient(p => builder.Configuration as IConfigurationRoot);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger()
        .UseSwaggerUI(c =>
        {
            configuration.GetSection(nameof(SwaggerUIOptions)).Bind(c);
        });
}

app.UseHttpsRedirection()
    .UseBlazorFrameworkFiles()
    .UseStaticFiles()
    .UseAuthentication()
    .UseAuthorization();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
