// Project: Aguafrommars/DynamicConfiguration
// Copyright (c) 2021 @Olivier Lefebvre

using Microsoft.Extensions.Localization;

namespace Aguacongas.DynamicConfiguration.Razor.Services
{
    /// <summary>
    /// Deafult string localizer service for the library components.
    /// </summary>
    /// <seealso cref="ISettingsLocalizer"/>
    public class DefaultSettingsLocalizer : ISettingsLocalizer
    {
        private readonly IStringLocalizer<Settings> _stringLocalizer;

        /// <summary>
        /// Initialize a new instance of <see cref="DefaultSettingsLocalizer"/>
        /// </summary>
        /// <param name="stringLocalizer">The underlying string localizer</param>
        /// <exception cref="ArgumentNullException">If stringLocalizer is null</exception>
        public DefaultSettingsLocalizer(IStringLocalizer<Settings> stringLocalizer)
        {
            _stringLocalizer = stringLocalizer ?? throw new ArgumentNullException(nameof(stringLocalizer));
        }

        /// <summary>
        /// Gets the string resource with the given name.
        /// </summary>
        /// <param name="name">The name of the string resource.</param>
        /// <returns>The string resource as a Microsoft.Extensions.Localization.LocalizedString.</returns>
        public LocalizedString this[string name] => _stringLocalizer[name];
    }
}
