using System.Runtime.Serialization;

namespace Aguacongas.DynamicConfiguration.Razor.Options
{
    /// <summary>
    /// Defines the settings options
    /// </summary>
    public class SettingsOptions
    {
        /// <summary>
        /// Gets or sets the settings assembly qualified type name.
        /// </summary>
        public string? TypeName { get; set; }

        /// <summary>
        /// Gets or sets the api url.
        /// </summary>
        public string? ApiUrl { get; set; }
    }
}
