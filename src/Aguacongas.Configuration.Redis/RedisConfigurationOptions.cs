namespace Aguacongas.Configuration.Redis
{
    public class RedisConfigurationOptions
    {
        public string ConnectionString { get; set; }
        public string Channel { get; set; }
        public string HashKey { get; set; }
        public int? Database { get; set; }
    }
}
