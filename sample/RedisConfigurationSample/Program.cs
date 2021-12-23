// Project: Aguafrommars/DynamicConfiguration
// Copyright (c) 2021 @Olivier Lefebvre

using Aguacongas.DynamicConfiguration.Redis;
using Microsoft.Extensions.Configuration;

var key = args.Length > 0 ? args[0] : "Exemple";

var builder = new ConfigurationBuilder();
builder.AddRedis(options =>
{
    options.ConnectionString = "localhost"; // connection string to your Redis server.
    options.Channel = "RedisConfigurationSample.Channdel"; // Publication channel to notify configuration change.
    options.HashKey = "RedisConfigurationSample.Configuration"; // Key of the Redis hash to store configuration.
    options.Database = 1; // (optional) Redis database to use. When null, the default database is used.
});

var configuration = builder.Build();

var redisProvider = configuration.Providers.First(p => p is RedisConfigurationProvider);
redisProvider.TryGet(key, out string value);
Console.WriteLine($"Read value at '{key}': {value}");

Console.WriteLine($"Set new value at '{key}:value'");
redisProvider.Set(key, $"{{ \"value\": \"{Guid.NewGuid()}\"}}");

redisProvider.TryGet(key, out value);
Console.WriteLine($"Read value at '{key}': {value}");

redisProvider.TryGet($"{key}:value", out value);
Console.WriteLine($"Read value at '{key}:value': {value}");

Console.WriteLine($"Set null value at '{key}'");
redisProvider.Set(null, $"{{ \"{key}\": null}}");

redisProvider.TryGet(key, out value);
Console.WriteLine($"Read value at '{key}': {value}");