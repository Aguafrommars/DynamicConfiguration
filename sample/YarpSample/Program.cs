var builder = WebApplication.CreateBuilder(args);

// Configure the reverse proxy with redis.
var configuration = builder.Configuration;
configuration.AddRedis(options => configuration.GetSection("Redis").Bind(options));

// Add services to the container.
builder.Services.AddReverseProxy()
    .LoadFromConfig(configuration.GetSection("ReverseProxy"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.MapReverseProxy();

app.Run();
