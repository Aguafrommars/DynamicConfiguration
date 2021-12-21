// Project: Aguafrommars/DynamicConfiguration
// Copyright (c) 2021 @Olivier Lefebvre

using Aguacongas.DynamicConfiguration.Abstractions;
using Aguacongas.DynamicConfiguration.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;

namespace Aguacongas.DynamicConfiguration.Services
{
    /// <summary>
    /// Reloads the root configuration when the provider chnaged
    /// </summary>
    public class AutoReloadConfigurationService : IAutoReloadConfigurationService, IDisposable
    {
        private readonly IConfigurationRoot _rootConfiguration;
        private readonly IConfigurationProvider _provider;
        private IDisposable _subscription;
        private bool disposedValue;

        /// <summary>
        /// Initialize a new instance of <see cref="ConfigurationService"/>
        /// </summary>
        /// <param name="rootConfiguration">A <see cref="IConfigurationRoot"/> instance</param>
        /// <param name="options">A <see cref="IOptions{DynamicConfigurationOptions}"/> instance</param>
        /// <exception cref="ArgumentNullException">If rootConfiguration is null</exception>
        public AutoReloadConfigurationService(IConfigurationRoot rootConfiguration, IOptions<DynamicConfigurationOptions> options)
        {
            _rootConfiguration = rootConfiguration ?? throw new ArgumentNullException(nameof(rootConfiguration));
            _provider = options?.Value?.Provider ?? throw new ArgumentNullException(nameof(options));
        }

        /// <inheritdoc/>
        public void SubscribeToChanges()
        {
            _subscription = _provider.GetReloadToken().RegisterChangeCallback(state =>
            {
                _rootConfiguration.Reload();
                _subscription?.Dispose();
                SubscribeToChanges();
            }, null);
        }

        /// <inheritdoc/>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _subscription?.Dispose();
                }

                disposedValue = true;
            }
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
