using Aguacongas.DynamicConfiguration.Razor.Services;
using Microsoft.AspNetCore.Components;
using System.Collections;
using System.Reflection;

namespace Aguacongas.DynamicConfiguration.Razor
{
    /// <summary>
    /// Displays the configuration
    /// </summary>
    public partial class Settings
    {
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

        [Inject]
        private IConfigurationService? Service { get; set; }

        private object? _model;

        private string? _rootPath;

        private IEnumerable<PropertyInfo>? Properties => _model?.GetType()?.GetProperties()?.Where(p => p.CanWrite);

        private IEnumerable<string>? Segments => Path?.Split(':');

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

            _model = await Service.GetAsync(Path).ConfigureAwait(false);
        }

        private string GetPath(PropertyInfo property) => string.IsNullOrEmpty(Path) ? $"{RootPath}{property.Name}" : $"{RootPath}{Path}:{property.Name}";


        private string? GetParentPath(int index)
        => Segments is not null && DisplayPathHasLink(index)
            ? string.Join(":", Segments.Take(index + 1))
            : null;

        private bool DisplayPathHasLink(int index)
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
            foreach(var segment in segments)
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

        private Task OnValidSubmit()
        => Service is not null 
            ? Service.SaveAsync(Path, default)
            : throw new InvalidOperationException($"{nameof(Service)} cannot be null");
        

        private static bool IsDictionary(Type type)
        {
            return type.IsAssignableTo(typeof(IDictionary)) ||
                (type.IsGenericType &&
                (type.GetGenericTypeDefinition() == typeof(IDictionary<,>) ||
                 type.GetGenericTypeDefinition().GetInterfaces().Any(i => i == typeof(IDictionary<,>))));
        }
    }
}
