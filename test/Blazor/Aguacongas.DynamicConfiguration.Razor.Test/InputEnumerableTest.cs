using Bunit;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
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

            public IEnumerable<object>? Enumerable { get; set; }
        }

        class FixedSizeList
        {

        }
    }
}
