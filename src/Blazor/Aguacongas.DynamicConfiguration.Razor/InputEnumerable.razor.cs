using Microsoft.AspNetCore.Components;
using System.Collections;
using System.Reflection;

namespace Aguacongas.DynamicConfiguration.Razor
{
    public partial class InputEnumerable
    {
        [Parameter]
        public object? Model { get; set; }

        [Parameter]
        public object? Value { get; set; }

        [Parameter]
        public PropertyInfo? Property { get; set; }

        [Parameter]
        public string? Path { get; set; }

        [Parameter]
        public Type? ValueType { get; set; }

        private string? Key { get; set; }

        private Type PropertyType => ValueType ?? Property?.PropertyType ?? throw new InvalidOperationException("PropertyType or ValueType must be set");

        private Type UnderlyingType => Nullable.GetUnderlyingType(PropertyType) ?? PropertyType;

        private bool IsDictionary => UnderlyingType.IsAssignableTo(typeof(IDictionary)) ||
            UnderlyingType.GetGenericTypeDefinition() == typeof(IDictionary<,>) ||
            UnderlyingType.GetGenericTypeDefinition().GetInterfaces().Any( i => i == typeof(IDictionary<,>));

        private IDictionary? ValueAsDictionary => Value as IDictionary;

        private IList? ValueAsEnumerable
        {
            get
            {
                if (Value is IList list && !list.IsFixedSize)
                {
                    return list;
                }
                list = CreateList();
                return list;
            }
        }

        private void RemoveItem(object key)
        {
            ValueAsDictionary?.Remove(key);
        }

        private void RemoveItemAt(int index)
        {
            if (ValueAsEnumerable is IList list && !list.IsFixedSize)
            {
                list.RemoveAt(index);
                return;
            }

            list = CreateList();            
            list.RemoveAt(index);
        }

        private void AddDictionaryItem()
        {
            if (Key is null)
            {
                throw new InvalidOperationException("Key cannot be null");
            }

            if (Value is null)
            {
                CreateDictionary();
            }

            var types = UnderlyingType.GetGenericArguments();

            var valueType = types[1];
            var constructor = valueType.GetConstructor(Array.Empty<Type>());
            if (constructor is not null)
            {
                ValueAsDictionary?.Add(Key, constructor?.Invoke(null));
                return;
            }

            ValueAsDictionary?.Add(Key, GetDefaultValue(valueType));
            Key = null;
            StateHasChanged();
        }

        private void AddListItem()
        {
            var types = UnderlyingType.GetGenericArguments();

            if (Value is not IList list || list.IsFixedSize)
            {
                list = CreateList();
            }

            var valueType = types[0];
            var constructor = valueType.GetConstructor(Array.Empty<Type>());
            if (constructor is not null)
            {
                list.Add(constructor.Invoke(null));
                return;
            }

            list.Add(GetDefaultValue(valueType));
        }

        private IList CreateList()
        {
            var types = UnderlyingType.GetGenericArguments();
            if (typeof(List<>).MakeGenericType(types)?.GetConstructor(Array.Empty<Type>())?.Invoke(null) is not IList list)
            {
                throw new InvalidOperationException("Cannot create list.");
            }

            if (Value is IEnumerable enumerable)
            {
                foreach (var item in enumerable)
                {
                    list.Add(item);
                }
            }
            
            Property?.SetValue(Model, list);
            Value = list;
            return list;
        }

        private IDictionary CreateDictionary()
        {
            var types = UnderlyingType.GetGenericArguments();
            if (typeof(Dictionary<,>).MakeGenericType(types)?.GetConstructor(Array.Empty<Type>())?.Invoke(null) is not IDictionary dictionary)
            {
                throw new InvalidOperationException("Cannot create list.");
            }

            if (Value is IDictionary enumerable)
            {
                foreach (var key in enumerable.Keys)
                {
                    dictionary.Add(key, enumerable[key]);
                }
            }

            Property?.SetValue(Model, dictionary);
            Value = dictionary;
            return dictionary;
        }

        private static object? GetDefaultValue(Type t)
        {
            if (t.IsValueType)
                return Activator.CreateInstance(t);

            return null;
        }
    }
}
