using System.Threading.Tasks;

namespace Aguacongas.Configuration.Abstractions
{
    /// <summary>
    /// Class implementing this interface configures application
    /// </summary>
    public interface IConfigurationService
    {
        /// <summary>
        /// Sets the configuration
        /// </summary>
        /// <param name="key">The configuration key</param>
        /// <param name="json">The configuration json serialized value</param>
        /// <returns></returns>
        Task SetAsync(string key, string json);

        /// <summary>
        /// Gets a configuration
        /// </summary>
        /// <param name="typeName">The assembly-qualified type name</param>
        /// <param name="key">The configuration key</param>
        /// <returns>The configuration</returns>
        Task<object> GetAsync(string typeName, string key);
    }
}
