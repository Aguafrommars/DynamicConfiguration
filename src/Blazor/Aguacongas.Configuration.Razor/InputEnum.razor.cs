using Microsoft.AspNetCore.Components;
using System.Reflection;

namespace Aguacongas.Configuration.Razor
{
    public partial class InputEnum
    {
        [Parameter]
        public Enum? Value { get; set; }

        [Parameter]
        public EventCallback<Enum?> ValueChanged { get; set; } = new EventCallback<Enum?>();

        [Parameter]
        public Type? PropertyType { get; set; }
       
        private Type UnderlyingType => PropertyType is not null
            ? Nullable.GetUnderlyingType(PropertyType) ?? PropertyType
            : throw new InvalidOperationException("PropertyType cannot be null");

        private bool IsFlag => UnderlyingType.GetCustomAttribute(typeof(FlagsAttribute), true) != null;

        private bool IsNullable => PropertyType is not null
            ? Nullable.GetUnderlyingType(PropertyType) is not null
            : throw new InvalidOperationException("PropertyType cannot be null");

        private IEnumerable<string> Names => Enum.GetNames(UnderlyingType);

        private string? Name => Value != null ? Enum.GetName(UnderlyingType, Value) : null;

        private Enum GetValue(string name) => (Enum)Enum.Parse(UnderlyingType, name);

        private bool IsChecked(string name) => Value?.HasFlag(GetValue(name)) ?? false;

        private void OnValueChanged(bool value, string name)
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

        private Task OnSelectChanged(string name)
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
