using Aguacongas.DynamicConfiguration.Abstractions;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Aguacongas.DynamicConfiguration.Services
{
    /// <summary>
    /// Configuration service
    /// </summary>
    /// <seealso cref="IConfigurationService"/>
    public class ConfigurationService : IConfigurationService
    {
        private readonly IConfigurationRoot _rootConfiguration;
        private readonly IConfigurationProvider _provider;
        /// <summary>
        /// Initialize a new instance of <see cref="ConfigurationService"/>
        /// </summary>
        /// <param name="rootConfiguration">A <see cref="IConfigurationRoot"/> instance</param>
        /// <exception cref="ArgumentNullException">If rootConfiguration is null</exception>
        public ConfigurationService(IConfigurationRoot rootConfiguration)
        {
            _rootConfiguration = rootConfiguration ?? throw new ArgumentNullException(nameof(rootConfiguration));
            _provider = rootConfiguration.Providers.First();
        }

        /// <summary>
        /// Gets a configuration
        /// </summary>
        /// <param name="typeName">The assembly-qualified type name</param>
        /// <param name="key">The configuration key</param>
        /// <returns>The configuration</returns>
        public Task<object> GetAsync(string typeName, string key)
        {
            if (string.IsNullOrWhiteSpace(typeName))
            {
                throw new ArgumentException($"{nameof(typeName)} cannot be empty.");
            }

            var type = Type.GetType(typeName, true, false);
            if (type.IsValueType || type == typeof(string))
            {
                return Task.FromResult(_rootConfiguration.GetValue(type, key));
            }

            var section = _rootConfiguration as IConfiguration;
            if (!string.IsNullOrEmpty(key))
            {
                section = _rootConfiguration.GetSection(key);
            }
            var result = section.Get(type);
            return Task.FromResult(result);
        }

        /// <summary>
        /// Sets the configuration
        /// </summary>
        /// <param name="key">The configuration key</param>
        /// <param name="json">The configuration json serialized value</param>
        /// <returns></returns>
        public Task SetAsync(string key, string json)
        {
            _provider.Set(key, json);
            return Task.CompletedTask;
        }
    }
}
