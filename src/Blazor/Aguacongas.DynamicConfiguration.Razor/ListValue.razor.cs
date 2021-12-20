using Microsoft.AspNetCore.Components;

namespace Aguacongas.DynamicConfiguration.Razor
{
    /// <summary>
    /// Displays a list item property value.
    /// </summary>
    public partial class ListValue
    {

        /// <summary>
        /// Gets or sets the property type.
        /// </summary>
        [Parameter]
        public override Type? PropertyType { get; set; }

        /// <summary>
        /// Gets or sets the configuration path.
        /// </summary>
        [Parameter]
        public string? Path { get; set; }

        /// <summary>
        /// Gets or sets the item index.
        /// </summary>
        [Parameter]
        public int Index { get; set; }


        protected override void SetValue(object? value)
        {
            if (Model is null)
            {
                throw new InvalidOperationException("Model cannot be null");
            }
            Model[Index] = value;
        }
    }
}
