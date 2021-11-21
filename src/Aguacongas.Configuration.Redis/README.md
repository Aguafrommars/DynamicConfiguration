# Aguacongas.Configuration.Redis

Configure your .NET application with [Redis](https://redis.io/).

## Usage

### Setup

```cs
var builder = new ConfigurationBuilder();
builder.AddRedis(options => 
{
	options.ConnectionString = "localhost"; // connection string to your Redis server.
	options.Channel = "Aguacongas.Configuration.Redis.Channdel"; // Publication channel to notify configuration change.
	options.HashKey = "Aguacongas.Configuration.Redis"; // Key of the Redis hash to store configuration.
	options.Database = 1; // (optional) Redis database to use. When null, the default database is used.
});

var configuration = builder.Build();
```

### Update a configuration

```cs
var redisProvider = configuration.Providers.First(p => p is RedisConfigurationProvider);
redisProvider.Set("Key", "Value");
```