using System.Runtime.Serialization;

namespace Aguacongas.Configuration.Razor.Options
{
    public class SettingsOptions
    {
        public string? TypeName { get; set; } = typeof(Dictionary<string, object>).FullName;
        public string? ApiUrl { get; set; }
    }
}
