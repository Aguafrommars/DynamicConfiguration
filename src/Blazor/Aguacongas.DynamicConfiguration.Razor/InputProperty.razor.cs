// Project: Aguafrommars/DynamicConfiguration
// Copyright (c) 2021 @Olivier Lefebvre

using Microsoft.AspNetCore.Components;
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
        public virtual PropertyInfo? Property { get; set; }

        /// <summary>
        /// Gets or sets the configuration path.
        /// </summary>
        [Parameter]
        public virtual string? Path { get; set; }

        /// <summary>
        /// Gets the property name
        /// </summary>
        protected virtual string? PropertyName
        => (Property?.GetCustomAttribute(typeof(DescriptionAttribute)) as DescriptionAttribute)?.Description ?? Property?.Name;

        /// <inheritdoc/>
        public override Type? PropertyType
        {
            get => Property?.PropertyType ?? throw new InvalidOperationException($"{nameof(Property)} cannot be null");
            set => base.PropertyType = value;
        }

        /// <inheritdoc/>
        protected override void SetValue(object? value)
        {
            Property?.SetValue(Model, value);
            Value = value;
        }

        /// <summary>
        /// Creates a new value.
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        protected virtual void CreateValue()
        {
            var constructor = UnderlyingType.GetConstructor(Array.Empty<Type>());
            if (constructor is null)
            {
                throw new InvalidOperationException("Cannot create value empty constructor not found.");
            }

            SetValue(constructor.Invoke(null));
        }

        /// <summary>
        /// Deletes the value.
        /// </summary>
        protected virtual void DeleteValue() => SetValue(null);
    }
}
