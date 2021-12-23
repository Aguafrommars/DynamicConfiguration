// Project: Aguafrommars/DynamicConfiguration
// Copyright (c) 2021 @Olivier Lefebvre

using Bunit;
using Microsoft.AspNetCore.Components.Forms;
using System;
using Xunit;

namespace Aguacongas.DynamicConfiguration.Razor.Test
{
    public class InputEnumTest : TestContext
    {
        [Fact]
        public void WhenValueIsFlag_should_renders_all_checkboxes()
        {
            var model = new Model
            {
                Flag = FlagEnum.Value1
            };
            var cut = RenderComponent<InputEnum>(parameters => parameters
                .Add(p => p.Value, model.Flag)
                .Add(p => p.PropertyType, model.GetType().GetProperty(nameof(Model.Flag))?.PropertyType));

            Assert.Contains("checked", cut.Markup);
            Assert.Contains($"<label class=\"form-check-label\" for=\"{nameof(FlagEnum.Value1)}\">", cut.Markup);
            Assert.Contains($"<label class=\"form-check-label\" for=\"{nameof(FlagEnum.Value2)}\">", cut.Markup);
        }

        [Fact]
        public void WhenValueIsFlagNullable_should_not_renders_cheked_checkbox()
        {
            var model = new Model();
            var cut = RenderComponent<InputEnum>(parameters => parameters
                .Add(p => p.Value, model.FlagNullable)
                .Add(p => p.PropertyType, model.GetType().GetProperty(nameof(Model.FlagNullable))?.PropertyType));

            Assert.DoesNotContain("checked", cut.Markup);
            Assert.Contains($"<label class=\"form-check-label\" for=\"{nameof(FlagEnum.Value1)}\">", cut.Markup);
            Assert.Contains($"<label class=\"form-check-label\" for=\"{nameof(FlagEnum.Value2)}\">", cut.Markup);
        }

        [Fact]
        public void WhenValueIsBasic_should_renders_all_options()
        {
            var model = new Model();
            var cut = RenderComponent<InputEnum>(parameters => parameters
                .Add(p => p.Value, model.Basic)
                .Add(p => p.PropertyType, model.GetType().GetProperty(nameof(Model.Basic))?.PropertyType)
                .AddCascadingValue(new EditContext(model)));

            Assert.DoesNotContain("<option></option>", cut.Markup);
            Assert.Contains($"<option value=\"{nameof(BasicEnum.Value1)}\" selected>", cut.Markup);
            Assert.Contains($"<option value=\"{nameof(BasicEnum.Value2)}\">", cut.Markup);
        }

        [Fact]
        public void WhenValueIsBasicNullable_should_renders_all_options()
        {
            var model = new Model();
            var cut = RenderComponent<InputEnum>(parameters => parameters
                .Add(p => p.Value, model.BasicNullable)
                .Add(p => p.PropertyType, model.GetType().GetProperty(nameof(Model.BasicNullable))?.PropertyType)
                .AddCascadingValue(new EditContext(model)));

            Assert.Contains("<option></option>", cut.Markup);
            Assert.Contains($"<option value=\"{nameof(BasicEnum.Value1)}\">", cut.Markup);
            Assert.Contains($"<option value=\"{nameof(BasicEnum.Value2)}\">", cut.Markup);
        }

        [Fact]
        public void WhenCheckBoxClicked_should_update_flag_model()
        {
            var model = new Model
            {
                Flag = FlagEnum.Value1
            };

            var cut = RenderComponent<InputEnum>(parameters => parameters
                .Add(p => p.Value, model.Flag)
                .Add(p => p.PropertyType, model.GetType().GetProperty(nameof(Model.Flag))?.PropertyType)
                .Add(p => p.ValueChanged, value =>
                {
                    if (value is FlagEnum flag)
                    {
                        model.Flag = flag;
                    }
                }));

            var checkedCheckbox = cut.Find("input[checked]");
            checkedCheckbox.Change(false);

            cut.WaitForState(() => !cut.Find($"#{FlagEnum.Value1}").HasAttribute("checked"));
            Assert.Equal(0, (int)model.Flag);

            var checkbox = cut.Find($"#{FlagEnum.Value2}");
            checkbox.Change(true);

            cut.WaitForState(() => cut.Find($"#{FlagEnum.Value2}").HasAttribute("checked"));
            Assert.Equal(FlagEnum.Value2, model.Flag);

            checkbox = cut.Find($"#{FlagEnum.Value1}");
            checkbox.Change(true);

            cut.WaitForState(() => cut.Find($"#{FlagEnum.Value1}").HasAttribute("checked"));
            Assert.Equal(FlagEnum.Value2 | FlagEnum.Value1, model.Flag);
        }

        [Fact]
        public void WhenCheckBoxClicked_should_update_nullable_flag_model()
        {
            var model = new Model();

            var cut = RenderComponent<InputEnum>(parameters => parameters
                .Add(p => p.Value, model.FlagNullable)
                .Add(p => p.PropertyType, model.GetType().GetProperty(nameof(Model.FlagNullable))?.PropertyType)
                .Add(p => p.ValueChanged, value =>
                {
                    if (value is FlagEnum flag)
                    {
                        model.FlagNullable = flag;
                    }
                    if (value == null)
                    {
                        model.FlagNullable = null;
                    }
                }));

            var checkbox = cut.Find($"#{FlagEnum.Value2}");
            checkbox.Change(true);

            cut.WaitForState(() => cut.Find($"#{FlagEnum.Value2}").HasAttribute("checked"));
            Assert.Equal(FlagEnum.Value2, model.FlagNullable);

            checkbox = cut.Find($"#{FlagEnum.Value1}");
            checkbox.Change(true);

            cut.WaitForState(() => cut.Find($"#{FlagEnum.Value1}").HasAttribute("checked"));
            Assert.Equal(FlagEnum.Value2 | FlagEnum.Value1, model.FlagNullable);

            checkbox = cut.Find($"#{FlagEnum.Value1}");
            checkbox.Change(false);

            cut.WaitForState(() => !cut.Find($"#{FlagEnum.Value1}").HasAttribute("checked"));
            Assert.Equal(FlagEnum.Value2, model.FlagNullable);

            checkbox = cut.Find($"#{FlagEnum.Value2}");
            checkbox.Change(false);

            cut.WaitForState(() => !cut.Find($"#{FlagEnum.Value2}").HasAttribute("checked"));
            Assert.Null(model.FlagNullable);
        }

        [Fact]
        public void WhenSelected_should_update_model()
        {
            var model = new Model();
            var cut = RenderComponent<InputEnum>(parameters => parameters
                .Add(p => p.Value, model.Basic)
                .Add(p => p.PropertyType, model.GetType().GetProperty(nameof(Model.Basic))?.PropertyType)
                .AddCascadingValue(new EditContext(model))
                .Add(p => p.ValueChanged, value =>
                {
                    if (value is BasicEnum basic)
                    {
                        model.Basic = basic;
                    }
                }));

            var select = cut.Find("select");
            select.Change(nameof(BasicEnum.Value2));

            Assert.Equal(BasicEnum.Value2, model.Basic);
        }

        [Fact]
        public void WhenSelected_should_update_nullable_model()
        {
            var model = new Model();
            var cut = RenderComponent<InputEnum>(parameters => parameters
                .Add(p => p.Value, model.BasicNullable)
                .Add(p => p.PropertyType, model.GetType().GetProperty(nameof(Model.BasicNullable))?.PropertyType)
                .AddCascadingValue(new EditContext(model))
                .Add(p => p.ValueChanged, value =>
                {
                    if (value is BasicEnum basic)
                    {
                        model.BasicNullable = basic;
                    }

                    if (value == null)
                    {
                        model.BasicNullable = null;
                    }
                }));

            var select = cut.Find("select");
            select.Change(nameof(BasicEnum.Value1));

            Assert.Equal(BasicEnum.Value1, model.BasicNullable);

            select = cut.Find("select");
            select.Change(string.Empty);

            Assert.Null(model.BasicNullable);
        }

        class Model
        {
            public FlagEnum Flag { get; set; }

            public FlagEnum? FlagNullable { get; set; }

            public BasicEnum Basic { get; set; }

            public BasicEnum? BasicNullable { get; set; }
        }

        [Flags]
        enum FlagEnum
        {
            Value1 = 1,
            Value2 = 2
        }

        enum BasicEnum
        {
            Value1,
            Value2
        }
    }
}
