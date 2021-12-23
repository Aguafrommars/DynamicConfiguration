// Project: Aguafrommars/DynamicConfiguration
// Copyright (c) 2021 @Olivier Lefebvre

namespace Aguacongas.DynamicConfiguration.Razor.Services
{
    /// <summary>
    /// Classes implementing this interface manage configuration.
    /// </summary>
    public interface IConfigurationService
    {
        /// <summary>
        /// Gets the root configuration.
        /// </summary>
        object? Configuration { get; }

        /// <summary>
        /// Saves the configurations.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task SaveAsync(string? key, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets a configuration
        /// </summary>
        /// <param name="key">The configuration key</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The configuration</returns>
        Task<object?> GetAsync(string? key, CancellationToken cancellationToken = default);
    }
}
