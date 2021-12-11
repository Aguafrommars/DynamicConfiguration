namespace Aguacongas.Configuration.Razor.Services
{
    public interface IConfigurationService
    {
        /// <summary>
        /// Gets the root configuration
        /// </summary>
            object? Configuration { get; }
        /// <summary>
        /// Sets a configurations.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="key">The key.</param>
        /// <param name="setting">The setting</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task SetAsync<T>(string? key, T setting, CancellationToken cancellationToken);

        /// <summary>
        /// Gets a configuration
        /// </summary>
        /// <param name="key">The configuration key</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The configuration</returns>
        Task<object?> GetAsync(string? key, CancellationToken cancellationToken);
    }
}
