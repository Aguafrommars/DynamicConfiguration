// Project: Aguafrommars/TheIdServer
// Copyright (c) 2021 @Olivier Lefebvre
using Aguacongas.DynamicConfiguration.Redis;
using Yarp.Configuration.Model;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerUI;
using Aguacongas.DynamicConfiguration.WebApi.Controllers;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;
configuration.AddRedis(options => configuration.GetSection(nameof(RedisConfigurationOptions)).Bind(options));

builder.Host.UseSerilog((context, config) => config.ReadFrom.Configuration(configuration));

var services = builder.Services;
// Add services to the container.
services.Configure<ReverseProxyOptions>(configuration.GetSection(nameof(ReverseProxyOptions)))
    .AddAuthorization(options =>
    {
        // The web api requires 2 policies, DYNAMIC_CONFIGURATION_READER_POLICY and DYNAMIC_CONFIGURATION_WRITTER_POLICY
        // In this sample those policies requires nothing, all users have access to the api.
        // You should update this code to meet your requirement.
        options.AddPolicy(ConfigurationController.DYNAMIC_CONFIGURATION_READER_POLICY, 
            builder => builder.RequireAssertion(context => true));
        options.AddPolicy(ConfigurationController.DYNAMIC_CONFIGURATION_WRITTER_POLICY, 
            builder => builder.RequireAssertion(context => true));
    });

services.AddAuthentication(); // this sample doesn't have authentication system but shoudl add yours.

services.AddControllersWithViews()
    .AddConfigurationWebAPI(options => options.Provider = ((IConfigurationRoot)configuration).Providers.First(p => p is RedisConfigurationProvider));
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
