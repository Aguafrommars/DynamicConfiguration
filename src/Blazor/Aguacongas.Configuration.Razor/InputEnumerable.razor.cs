using Microsoft.AspNetCore.Components;
using System.Collections;
using System.Reflection;

namespace Aguacongas.Configuration.Razor
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

        [Inject]
        private NavigationManager? NavigationManager { get; set; }

        private string? Key { get; set; }

        private Type PropertyType => Property?.PropertyType ?? typeof(object);

        private bool IsDictionary => PropertyType.IsAssignableTo(typeof(IDictionary));

        private IDictionary? ValueAsDictionary => Value as IDictionary;

        private IEnumerable? ValueAsEnumerable => Value as IEnumerable;

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

        private void AddItem(string? key)
        {
            if (key is null)
            {
                return;
            }

            if (Value is null)
            {
                Value = PropertyType.GetConstructor(Array.Empty<Type>())?
                    .Invoke(null);
                Property?.SetValue(Model, Value);
            }
            var types = PropertyType.GetGenericArguments();

            var constructor = types[1].GetConstructor(Array.Empty<Type>());
            ValueAsDictionary?.Add(key, constructor?.Invoke(null));

            NavigationManager?.NavigateTo($"./{Path}:{key}");
        }

        private void AddItem()
        {
            var types = PropertyType.GetGenericArguments();

            if (Value is not IList list || list.IsFixedSize)
            {
                list = CreateList();
            }

            var constructor = types[0].GetConstructor(Array.Empty<Type>());
            list.Add(constructor?.Invoke(null));

            NavigationManager?.NavigateTo($"./{Path}:{list.Count}");
        }

        private IList CreateList()
        {
            var types = PropertyType.GetGenericArguments();
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
    }
}
