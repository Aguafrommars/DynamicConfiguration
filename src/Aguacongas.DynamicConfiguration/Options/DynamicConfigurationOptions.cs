// Project: Aguafrommars/DynamicConfiguration
// Copyright (c) 2021 @Olivier Lefebvre

using Microsoft.Extensions.Configuration;

namespace Aguacongas.DynamicConfiguration.Options
{
    /// <summary>
    /// Defines dynamic configuration options
    /// </summary>
    public class DynamicConfigurationOptions
    {
        /// <summary>
        /// Gets or sets the configuration provider to use.
        /// </summary>
        public IConfigurationProvider Provider { get; set; }
    }
}
