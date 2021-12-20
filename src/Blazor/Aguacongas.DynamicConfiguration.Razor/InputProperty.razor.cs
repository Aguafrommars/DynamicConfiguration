using Microsoft.AspNetCore.Components;
using System.Collections;
using System.ComponentModel;
using System.Reflection;

namespace Aguacongas.DynamicConfiguration.Razor
{
    /// <summary>
    /// Displays a configuration property
    /// </summary>
    public partial class InputProperty
    {
        /// <summary>
        /// Gets or sets the property info.
        /// </summary>
        [Parameter]
        public PropertyInfo? Property { get; set; }

        /// <summary>
        /// Gets or sets the configuration path.
        /// </summary>
        [Parameter]
        public string? Path { get; set; }

        private string? PropertyName
            => (Property?.GetCustomAttribute(typeof(DescriptionAttribute)) as DescriptionAttribute)?.Description ?? Property?.Name;

        public override Type? PropertyType 
        { 
            get => Property?.PropertyType ?? throw new InvalidOperationException($"{nameof(Property)} cannot be null"); 
            set => base.PropertyType = value; 
        }

        protected override void SetValue(object? value)
        {
            Property?.SetValue(Model, value);
            Value = value;
        }

        private void CreateValue()
        {
            var constructor = UnderlyingType.GetConstructor(Array.Empty<Type>());
            if (constructor is null)
            {
                throw new InvalidOperationException("Cannot create value empty constructor not found.");
            }

            SetValue(constructor.Invoke(null));
        }

        private void DeleteValue()
        {
            SetValue(null);
        }
    }
}
