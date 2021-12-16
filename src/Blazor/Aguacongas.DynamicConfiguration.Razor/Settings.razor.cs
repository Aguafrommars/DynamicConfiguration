using Aguacongas.DynamicConfiguration.Razor.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using System.Collections;
using System.Reflection;

namespace Aguacongas.DynamicConfiguration.Razor
{
    [Authorize(Policy = DYNAMIC_CONFIGURATION_READER_POLICY)]
    public partial class Settings
    {
        /// <summary>
        /// Defines the dynamic configuration reader authorization policy
        /// </summary>
        public const string DYNAMIC_CONFIGURATION_READER_POLICY = "DynamicConfigurationReaderPolicy";
        /// <summary>
        /// Defines the dynamic configuration writter authorization policy
        /// </summary>
        public const string DYNAMIC_CONFIGURATION_WRITTER_POLICY = "DynamicConfigurationWritterPolicy";

        [Parameter]
        public string? Path { get; set; }

        [Inject]
        private IConfigurationService? Service { get; set; }

        private object? _model;

        private IEnumerable<PropertyInfo>? Properties => _model?.GetType()?.GetProperties();

        private IEnumerable<string>? Segments => Path?.Split(':');

        protected async override Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync().ConfigureAwait(false);

            if (Service is null)
            {
                throw new InvalidOperationException($"{nameof(Service)} cannot be null");
            }

            _model = await Service.GetAsync(Path, default).ConfigureAwait(false);
        }

        private string GetPath(PropertyInfo property) => string.IsNullOrEmpty(Path) ? $"/settings/{property.Name}" : $"/settings/{Path}:{property.Name}";

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
            ? Service.SetAsync(Path, _model, default)
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
