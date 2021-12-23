// Project: Aguafrommars/DynamicConfiguration
// Copyright (c) 2021 @Olivier Lefebvre

using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace Aguacongas.DynamicConfiguration.Razor.Test
{
    public class InputEnumerableTest : TestContext
    {
        [Fact]
        public void WhenModelIsDictionaryAddItem_should_check_key()
        {
            var model = new Model
            {
                IDictionary = new Dictionary<string, object>()
            };
            var cut = RenderComponent<InputEnumerable>(parameters => parameters
                .Add(p => p.Model, model)
                .Add(p => p.Value, model.Dictionary)
                .Add(p => p.Path, "Model")
                .Add(p => p.Property, model.GetType().GetProperty(nameof(Model.Dictionary))));

            var input = cut.Find("input");
            input.Input(new ChangeEventArgs
            {
                Value = null
            });
            var button = cut.Find("button");
            Assert.Throws<InvalidOperationException>(() => button.Click());

            Assert.Null(model.Dictionary);

            var expected = Guid.NewGuid().ToString();
            input = cut.Find("input");
            input.Input(new ChangeEventArgs
            {
                Value = expected
            });
            button = cut.Find("button");
            button.Click();

            Assert.Contains(expected, model.Dictionary?.Keys);
            var a = cut.Find("a");
            Assert.Contains($"Model:{expected}", a.OuterHtml);
        }

        [Fact]
        public void WhenModelIsValueTypeDictionaryAddItem_should_check_key()
        {
            var model = new Model
            {
                DictionaryInt = new TestDisctionary()
            };
            var cut = RenderComponent<InputEnumerable>(parameters => parameters
                .Add(p => p.Model, model)
                .Add(p => p.Value, model.DictionaryInt)
                .Add(p => p.Path, "Model")
                .Add(p => p.Property, model.GetType().GetProperty(nameof(Model.DictionaryInt)))
                .AddCascadingValue(new EditContext(model)));

            var input = cut.Find("input");

            var expected = Guid.NewGuid().ToString();
            input = cut.Find("input");
            input.Input(new ChangeEventArgs
            {
                Value = expected
            });
            var button = cut.Find("button");
            button.Click();

            Assert.Contains(expected, model.DictionaryInt?.Keys);
        }

        [Fact]
        public void WhenModelIsDictionaryRemoveItem_should_use_key()
        {
            var key = Guid.NewGuid().ToString();
            var model = new Model
            {
                IDictionary = new Dictionary<string, object>()
                {
                    [key] = key
                }
            };
            var cut = RenderComponent<InputEnumerable>(parameters => parameters
                .Add(p => p.Model, model)
                .Add(p => p.Value, model.IDictionary)
                .Add(p => p.Path, "Model")
                .Add(p => p.Property, model.GetType().GetProperty(nameof(Model.IDictionary))));

            var button = cut.Find("button");
            button.Click();

            Assert.Empty(model.IDictionary);
        }

        [Fact]
        public void WhenModelIsNotListAddItem_should_create_list()
        {
            var model = new Model
            {
                Enumerable = Array.Empty<object>(),
            };

            var cut = RenderComponent<InputEnumerable>(parameters => parameters
                .Add(p => p.Model, model)
                .Add(p => p.Value, model.Enumerable)
                .Add(p => p.Path, "Model")
                .Add(p => p.Property, model.GetType().GetProperty(nameof(Model.Enumerable))));

            var button = cut.Find("button");
            button.Click();

            Assert.NotEmpty(model.Enumerable);

            var a = cut.Find("a");
            Assert.Contains($"Model:0", a.OuterHtml);
        }

        [Fact]
        public void WhenNotModelItemEmptyConstructor_should_create_list_item()
        {
            var model = new Model
            {
                EnumerableString = Array.Empty<string>(),
            };

            var cut = RenderComponent<InputEnumerable>(parameters => parameters
                .Add(p => p.Model, model)
                .Add(p => p.Value, model.EnumerableString)
                .Add(p => p.Path, "Model")
                .Add(p => p.Property, model.GetType().GetProperty(nameof(Model.EnumerableString)))
                .AddCascadingValue(new EditContext(model)));

            var button = cut.Find("button");
            button.Click();

            Assert.NotEmpty(model.EnumerableString);
        }

        [Fact]
        public void WhenNotModelItemIsValueType_should_create_list_item()
        {
            var model = new Model
            {
                EnumerableInt = Array.Empty<int>(),
            };

            var cut = RenderComponent<InputEnumerable>(parameters => parameters
                .Add(p => p.Model, model)
                .Add(p => p.Value, model.EnumerableInt)
                .Add(p => p.Path, "Model")
                .Add(p => p.Property, model.GetType().GetProperty(nameof(Model.EnumerableInt)))
                .AddCascadingValue(new EditContext(model)));

            var button = cut.Find("button");
            button.Click();

            Assert.NotEmpty(model.EnumerableInt);
        }

        [Fact]
        public void WhenNotModelItemEmptyConstructor_should_create_dictionary_item()
        {
            var model = new Model
            {
                DictionaryString = new Dictionary<string, string>(),
            };

            var cut = RenderComponent<InputEnumerable>(parameters => parameters
                .Add(p => p.Model, model)
                .Add(p => p.Value, model.DictionaryString)
                .Add(p => p.Path, "Model")
                .Add(p => p.Property, model.GetType().GetProperty(nameof(Model.DictionaryString)))
                .AddCascadingValue(new EditContext(model)));

            var input = cut.Find("input");
            var expected = Guid.NewGuid().ToString();
            input = cut.Find("input");
            input.Input(new ChangeEventArgs
            {
                Value = expected
            });
            var button = cut.Find("button");
            button.Click();


            Assert.NotEmpty(model.DictionaryString);
        }

        [Fact]
        public void WhenModelIsNotListRemoveItem_should_use_index()
        {
            var model = new Model
            {
                Enumerable = new object[]
                {
                    new object()
                },
            };

            var cut = RenderComponent<InputEnumerable>(parameters => parameters
                .Add(p => p.Model, model)
                .Add(p => p.Value, model.Enumerable)
                .Add(p => p.Path, "Model")
                .Add(p => p.Property, model.GetType().GetProperty(nameof(Model.Enumerable))));

            var button = cut.Find("button");
            button.Click();

            Assert.Empty(model.Enumerable);
        }

        [Fact]
        public void WhenModelIsListRemoveItem_should_use_index()
        {
            var model = new Model
            {
                Enumerable = new List<object>
                {
                    new object()
                },
            };

            var cut = RenderComponent<InputEnumerable>(parameters => parameters
                .Add(p => p.Model, model)
                .Add(p => p.Value, model.Enumerable)
                .Add(p => p.Path, "Model")
                .Add(p => p.Property, model.GetType().GetProperty(nameof(Model.Enumerable))));

            var button = cut.Find("button");
            button.Click();

            Assert.Empty(model.Enumerable);
        }

        [Fact]
        public void WhenModelIsFixedSizeListRemoveItem_should_use_index()
        {
            var model = new Model
            {
                Enumerable = new object[]
                {
                    new object()
                },
            };

            var cut = RenderComponent<InputEnumerable>(parameters => parameters
                .Add(p => p.Model, model)
                .Add(p => p.Value, model.Enumerable)
                .Add(p => p.Path, "Model")
                .Add(p => p.Property, model.GetType().GetProperty(nameof(Model.Enumerable))));

            var button = cut.Find("button");
            button.Click();

            Assert.Empty(model.Enumerable);
        }

        class Model
        {
            public IDictionary<string, object>? IDictionary { get; set; }

            public Dictionary<string, object>? Dictionary { get; set; }

            public Dictionary<string, string>? DictionaryString { get; set; }

            public IDictionary<string, int>? DictionaryInt { get; set; }

            public IEnumerable<object>? Enumerable { get; set; }

            public IEnumerable<string>? EnumerableString { get; set; }

            public IEnumerable<int>? EnumerableInt { get; set; }
        }

        class TestDisctionary : IDictionary<string, int>
        {
            private readonly Dictionary<string, int> _dictionary = new Dictionary<string, int>();

            public int this[string key] { get => _dictionary[key]; set => _dictionary[key] = value; }

            public ICollection<string> Keys => _dictionary.Keys;

            public ICollection<int> Values => _dictionary.Values;

            public int Count => _dictionary.Count;

            public bool IsReadOnly => false;

            public void Add(string key, int value)
            => _dictionary.Add(key, value);

            public void Add(KeyValuePair<string, int> item)
            => _dictionary.Add(item.Key, item.Value);

            public void Clear()
            => _dictionary.Clear();

            public bool Contains(KeyValuePair<string, int> item)
            {
                throw new NotImplementedException();
            }

            public bool ContainsKey(string key)
            => _dictionary.ContainsKey(key);

            public void CopyTo(KeyValuePair<string, int>[] array, int arrayIndex)
            {
                throw new NotImplementedException();
            }

            public IEnumerator<KeyValuePair<string, int>> GetEnumerator()
            => _dictionary.GetEnumerator();

            public bool Remove(string key)
            => (_dictionary.Remove(key));

            public bool Remove(KeyValuePair<string, int> item)
            => (_dictionary.Remove(item.Key));

            public bool TryGetValue(string key, [MaybeNullWhen(false)] out int value)
            => (_dictionary.TryGetValue(key, out value));

            IEnumerator IEnumerable.GetEnumerator()
            => _dictionary.GetEnumerator();
        }
    }
}
