// Project: Aguafrommars/DynamicConfiguration
// Copyright (c) 2021 @Olivier Lefebvre

using Microsoft.AspNetCore.Components;
using System.Reflection;

namespace Aguacongas.DynamicConfiguration.Razor
{
    /// <summary>
    /// Displays an enum property value.
    /// </summary>
    public partial class InputEnum
    {
        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        [Parameter]
        public virtual Enum? Value { get; set; }

        /// <summary>
        /// Gets or sets the value changed event callback.
        /// </summary>
        [Parameter]
        public virtual EventCallback<Enum?> ValueChanged { get; set; } = new EventCallback<Enum?>();

        /// <summary>
        /// Gets or sets the property type.
        /// </summary>
        [Parameter]
        public virtual Type? PropertyType { get; set; }

        /// <summary>
        /// Gets the underlying property type. 
        /// </summary>
        /// <remarks>
        /// The nullable underlying property type if property type is nullable oherwise the property type.
        /// </remarks>
        protected virtual Type UnderlyingType => PropertyType is not null
            ? Nullable.GetUnderlyingType(PropertyType) ?? PropertyType
            : throw new InvalidOperationException("PropertyType cannot be null");

        /// <summary>
        /// True if the enum has <see cref="FlagsAttribute"/>.
        /// </summary>
        protected virtual bool IsFlag => UnderlyingType.GetCustomAttribute(typeof(FlagsAttribute), true) != null;

        /// <summary>
        /// True if the property type is nullable.
        /// </summary>
        protected virtual bool IsNullable => PropertyType is not null
            ? Nullable.GetUnderlyingType(PropertyType) is not null
            : throw new InvalidOperationException("PropertyType cannot be null");

        /// <summary>
        /// Gets the enum names.
        /// </summary>
        protected virtual IEnumerable<string> Names => Enum.GetNames(UnderlyingType);

        /// <summary>
        /// Gets the value's name.
        /// </summary>
        protected virtual string? Name => Value != null ? Enum.GetName(UnderlyingType, Value) : null;

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <param name="name">The enum name</param>
        /// <returns>The value</returns>
        protected virtual Enum GetValue(string name) => (Enum)Enum.Parse(UnderlyingType, name);

        /// <summary>
        /// Returns true if the value has the flag.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>True if the value has the flag.</returns>
        protected virtual bool IsChecked(string name) => Value?.HasFlag(GetValue(name)) ?? false;

        /// <summary>
        /// Process checkboxes changes.
        /// </summary>
        /// <param name="value">The new value.</param>
        /// <param name="name">The enum name.</param>
        protected virtual void OnValueChanged(bool value, string name)
        {
            if (value)
            {
                if (Value == null)
                {
                    Value = GetValue(name);
                }
                else
                {
                    Value = (Enum)Enum.ToObject(UnderlyingType, (int)Enum.ToObject(UnderlyingType, Value) | (int)Enum.ToObject(UnderlyingType, GetValue(name)));
                }
            }
            else if (Value != null)
            {
                Value = (Enum)Enum.ToObject(UnderlyingType, (int)Enum.ToObject(UnderlyingType, Value) & ~(int)Enum.ToObject(UnderlyingType, GetValue(name)));
            }

            if (Value != null && (int)Enum.ToObject(UnderlyingType, Value) == 0 && IsNullable)
            {
                Value = null;
            }

            ValueChanged.InvokeAsync(Value);
        }

        /// <summary>
        /// Process input select changes
        /// </summary>
        /// <param name="name">The enum name.</param>
        /// <returns></returns>
        protected virtual Task OnSelectChangedAsyn(string name)
        {
            if (string.IsNullOrEmpty(name) && IsNullable)
            {
                Value = null;
                return ValueChanged.InvokeAsync(Value);
            }

            Value = GetValue(name);
            return ValueChanged.InvokeAsync(Value);
        }
    }
}
