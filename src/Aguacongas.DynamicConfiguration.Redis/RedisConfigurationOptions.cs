namespace Aguacongas.DynamicConfiguration.Redis
{
    /// <summary>
    /// Redis configuration options.
    /// </summary>
    public class RedisConfigurationOptions
    {
        /// <summary>
        /// Gets or sets the connection string.
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the pulblication channel,
        /// </summary>
        public string Channel { get; set; }

        /// <summary>
        /// Gets or sets the hash key.
        /// </summary>
        public string HashKey { get; set; }

        /// <summary>
        /// Gets or sets the database.
        /// </summary>
        public int? Database { get; set; }
    }
}
