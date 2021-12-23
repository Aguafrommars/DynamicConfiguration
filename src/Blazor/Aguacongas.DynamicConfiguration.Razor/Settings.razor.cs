// Project: Aguafrommars/DynamicConfiguration
// Copyright (c) 2021 @Olivier Lefebvre

using Aguacongas.DynamicConfiguration.Razor.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using System.Collections;
using System.Reflection;

namespace Aguacongas.DynamicConfiguration.Razor
{
    /// <summary>
    /// Displays the configuration
    /// </summary>
    public partial class Settings
    {
        private string? _rootPath;

        /// <summary>
        /// Defines the dynamic configuration writter authorization policy.
        /// </summary>
        public const string DYNAMIC_CONFIGURATION_WRITTER_POLICY = "DynamicConfigurationWritterPolicy";

        /// <summary>
        /// Gets or sets the configuration path.
        /// </summary>
        [Parameter]
        public string? Path { get; set; }

        /// <summary>
        /// Gets or sets the root page path.
        /// </summary>
        [Parameter]
        public string? RootPath
        {
            get { return _rootPath; }
            set
            {
                if (value?.StartsWith('/') == false)
                {
                    throw new InvalidOperationException($"{nameof(RootPath)} must start with '/");
                }
                if (value?.EndsWith('/') == false)
                {
                    throw new InvalidOperationException($"{nameof(RootPath)} must end with '/");
                }
                _rootPath = value;
            }
        }

        /// <summary>
        /// Gets or sets the configuration service.
        /// </summary>
        [Inject]
        protected virtual IConfigurationService? Service { get; set; }

        /// <summary>
        /// Gets or sets the component localizer
        /// </summary>
        [Inject]
        protected virtual ISettingsLocalizer? Localizer { get; set; }

        /// <summary>
        /// Gets or sets the model
        /// </summary>
        protected virtual object? Model { get; set; }

        /// <summary>
        /// Gets the model's writable property list
        /// </summary>
        protected virtual IEnumerable<PropertyInfo>? Properties => Model?.GetType()?.GetProperties()?.Where(p => p.CanWrite);

        /// <summary>
        /// Gets the path's segments splited by ':'.
        /// </summary>
        protected virtual IEnumerable<string>? Segments => Path?.Split(':');

        /// <summary>
        /// Method invoked when the component has received parameters from its parent in
        /// the render tree, and the incoming values have been assigned to properties.
        /// </summary>
        /// <returns>A <see cref="Task" /> representing any asynchronous operation.</returns>
        /// <exception cref="InvalidOperationException">If <see cref="IConfigurationService" /> is not injected in Service property.</exception>
        protected async override Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync().ConfigureAwait(false);

            if (Service is null)
            {
                throw new InvalidOperationException($"{nameof(Service)} cannot be null");
            }

            Model = await Service.GetAsync(Path).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the property's path
        /// </summary>
        /// <param name="property">The propety</param>
        /// <returns>The property path</returns>
        protected virtual string GetPath(PropertyInfo property) => string.IsNullOrEmpty(Path) ? $"{RootPath}{property.Name}" : $"{RootPath}{Path}:{property.Name}";

        /// <summary>
        /// Gets the parent path at index
        /// </summary>
        /// <param name="index">The index</param>
        /// <returns>The parent path</returns>
        protected virtual string? GetParentPath(int index)
        => Segments is not null && DisplayPathHasLink(index)
            ? string.Join(":", Segments.Take(index + 1))
            : null;

        /// <summary>
        /// Returns true if the path should be display as linkk
        /// </summary>
        /// <param name="index">The path index</param>
        /// <returns>True if the path should be display as linkk</returns>
        /// <exception cref="InvalidOperationException">If Segments is null</exception>
        protected virtual bool DisplayPathHasLink(int index)
        {
            if (Service?.Configuration is null)
            {
                return false;
            }

            var segments = Segments;
            if (segments is null)
            {
                throw new InvalidOperationException($"{nameof(Segments)} cannot be null");
            }
            if (index == segments.Count() - 1)
            {
                return false;
            }

            var type = Service?.Configuration.GetType();

            var currentIndex = 0;
            foreach (var segment in segments)
            {
                if (type is null)
                {
                    throw new InvalidOperationException($"{nameof(type)} cannot be null");
                }

                if (IsDictionary(type))
                {
                    type = type.GetGenericArguments()[1];
                }
                else
                {
                    var property = type.GetProperty(segment);
                    if (property is null)
                    {
                        throw new InvalidOperationException($"{nameof(property)} cannot be null");
                    }
                    type = property.PropertyType;
                }

                if (currentIndex++ < index)
                {
                    continue;
                }
                type = Nullable.GetUnderlyingType(type) ?? type;
                return !type.IsAssignableTo(typeof(IEnumerable));
            }
            return false;
        }

        /// <summary>
        /// Handles valid submit
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">If Service is null</exception>
        protected virtual Task OnValidSubmit()
        => Service is not null
            ? Service.SaveAsync(Path, default)
            : throw new InvalidOperationException($"{nameof(Service)} cannot be null");

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
        /// Returns true is the type is a dictionary
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>True is the type is a dictionary</returns>
        protected static bool IsDictionary(Type type) => type.IsAssignableTo(typeof(IDictionary)) ||
                (type.IsGenericType &&
                (type.GetGenericTypeDefinition() == typeof(IDictionary<,>) ||
                 type.GetGenericTypeDefinition().GetInterfaces().Any(i => i == typeof(IDictionary<,>))));
    }
}
