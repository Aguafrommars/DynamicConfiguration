// Project: Aguafrommars/DynamicConfiguration
// Copyright (c) 2021 @Olivier Lefebvre

using Aguacongas.DynamicConfiguration.Razor.Services;
using Microsoft.AspNetCore.Components;
using System.Collections;
using System.Reflection;

namespace Aguacongas.DynamicConfiguration.Razor
{
    /// <summary>
    /// Display an enumerable property value.
    /// </summary>
    public partial class InputEnumerable
    {
        /// <summary>
        /// Gets or sets the model.
        /// </summary>
        [Parameter]
        public virtual object? Model { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        [Parameter]
        public virtual object? Value { get; set; }

        /// <summary>
        /// Gets or sets the property info.
        /// </summary>
        /// <remarks>Either ValueType or Property must be set.</remarks>
        [Parameter]
        public virtual PropertyInfo? Property { get; set; }

        /// <summary>
        /// Gets or sets the configuration path.
        /// </summary>
        [Parameter]
        public virtual string? Path { get; set; }

        /// <summary>
        /// Gets or sets the value type.
        /// </summary>
        /// <remarks>Either ValueType or Property must be set.</remarks>
        [Parameter]
        public virtual Type? ValueType { get; set; }

        /// <summary>
        /// Gets or sets the component localizer
        /// </summary>
        [Inject]
        protected virtual ISettingsLocalizer? Localizer { get; set; }

        /// <summary>
        /// Binds the dictionary key.
        /// </summary>
        protected virtual string? Key { get; set; }

        /// <summary>
        /// Gets the property type
        /// </summary>
        protected virtual Type PropertyType => ValueType ?? Property?.PropertyType ?? throw new InvalidOperationException("Either PropertyType or ValueType must be set.");

        /// <summary>
        /// Gets the underlying property type
        /// </summary>
        protected virtual Type UnderlyingType => Nullable.GetUnderlyingType(PropertyType) ?? PropertyType;

        /// <summary>
        /// Gets the property type writable properties
        /// </summary>
        protected virtual IEnumerable<PropertyInfo>? Properties => UnderlyingType.GetProperties()?.Where(p => p.CanWrite && p.Name != "Item");

        /// <summary>
        /// Gets the property path
        /// </summary>
        /// <param name="property">The property</param>
        /// <returns>The path</returns>
        /// <exception cref="InvalidOperationException"></exception>
        protected virtual string GetPath(PropertyInfo property) => !string.IsNullOrEmpty(Path)
            ? $"{Path}{property.Name}"
            : throw new InvalidOperationException($"{nameof(Path)} cannot be null");

        /// <summary>
        /// Returns true if the property type is a dictionary
        /// </summary>
        protected virtual bool IsDictionary => UnderlyingType.IsAssignableTo(typeof(IDictionary)) ||
            UnderlyingType.GetGenericTypeDefinition() == typeof(IDictionary<,>) ||
            UnderlyingType.GetGenericTypeDefinition().GetInterfaces().Any(i => i == typeof(IDictionary<,>));

        /// <summary>
        /// Gets the value as a dictionary
        /// </summary>
        protected virtual IDictionary? ValueAsDictionary => Value as IDictionary;

        /// <summary>
        /// Gets the value as enumerable
        /// </summary>
        protected virtual IList? ValueAsEnumerable
        {
            get
            {
                if (Value is IList list && !list.IsFixedSize)
                {
                    return list;
                }
                return CreateList();
            }
        }

        /// <summary>
        /// Remove a dictionary item by its key
        /// </summary>
        /// <param name="key">The key</param>
        protected virtual void RemoveItem(object key)
        => ValueAsDictionary?.Remove(key);

        /// <summary>
        /// Remove an enumable item at index
        /// </summary>
        /// <param name="index">The index</param>
        private void RemoveItemAt(int index)
        => ValueAsEnumerable?.RemoveAt(index);

        /// <summary>
        /// Adds a dictionary item
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        protected virtual void AddDictionaryItem()
        {
            if (Key is null)
            {
                throw new InvalidOperationException("Key cannot be null");
            }

            if (ValueAsDictionary is null)
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

        /// <summary>
        /// Add an enumerable item
        /// </summary>
        protected virtual void AddListItem()
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

        /// <summary>
        /// Create a new list and set the value.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        protected virtual IList CreateList()
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

        /// <summary>
        /// Create a new dictionary and set the value.
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        protected virtual void CreateDictionary()
        {
            var types = UnderlyingType.GetGenericArguments();
            if (typeof(Dictionary<,>).MakeGenericType(types)?.GetConstructor(Array.Empty<Type>())?.Invoke(null) is not IDictionary dictionary)
            {
                throw new InvalidOperationException("Cannot create list.");
            }

            Property?.SetValue(Model, dictionary);
            Value = dictionary;
        }

        /// <summary>
        /// Localize a value.
        /// </summary>
        /// <param name="value">The value</param>
        /// <returns>The value localized.</returns>
        protected virtual string? Localize(string? value)
        {
            if (value is null)
            {
                return null;
            }

            if (Localizer is null)
            {
                return value;
            }

            return Localizer[value];
        }

        /// <summary>
        /// Gets the type's default value
        /// </summary>
        /// <param name="type">The type</param>
        /// <returns>The default value.</returns>
        protected static object? GetDefaultValue(Type type)
        {
            if (type.IsValueType)
            {
                return Activator.CreateInstance(type);
            }

            return null;
        }
    }
}
