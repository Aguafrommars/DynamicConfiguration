// Project: Aguafrommars/DynamicConfiguration
// Copyright (c) 2021 @Olivier Lefebvre

using Microsoft.Extensions.Localization;

namespace Aguacongas.DynamicConfiguration.Razor.Services
{
    /// <summary>
    /// Represents the string localizer service use by library's components.
    /// </summary>
    public interface ISettingsLocalizer
    {
        /// <summary>
        /// Gets the string resource with the given name.
        /// </summary>
        /// <param name="name">The name of the string resource.</param>
        /// <returns>The string resource as a Microsoft.Extensions.Localization.LocalizedString.</returns>
        LocalizedString this[string name] { get; }
    }
}