using Microsoft.AspNetCore.Components;

namespace Aguacongas.DynamicConfiguration.Razor
{
    /// <summary>
    /// Displays a dictionary item property value
    /// </summary>
    public partial class DictionaryValue
    {
        /// <inheritdoc/>
        [Parameter]
        public override Type? PropertyType { get; set; }

        /// <summary>
        /// Gets or sets the configuration path.
        /// </summary>
        [Parameter]
        public string? Path { get; set; }

        /// <summary>
        /// Gets or sets the item key.
        /// </summary>
        [Parameter]
        public object? Key { get; set; }

        /// <inheritdoc/>
        protected override void SetValue(object? value)
        {
            if (Model is null)
            {
                throw new InvalidOperationException("Model cannot be null");
            }
            if (Key is null)
            {
                throw new InvalidOperationException("Key cannot be null");
            }
            Model[Key] = value;
        }
    }
}
