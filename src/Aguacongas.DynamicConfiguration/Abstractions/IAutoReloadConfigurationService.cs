// Project: Aguafrommars/DynamicConfiguration
// Copyright (c) 2021 @Olivier Lefebvre

namespace Aguacongas.DynamicConfiguration.Abstractions
{
    /// <summary>
    /// Classes implementing this interface auto reload configuration
    /// </summary>
    public interface IAutoReloadConfigurationService
    {
        /// <summary>
        /// Subscribe to configuration changes
        /// </summary>
        void SubscribeToChanges();
    }
}