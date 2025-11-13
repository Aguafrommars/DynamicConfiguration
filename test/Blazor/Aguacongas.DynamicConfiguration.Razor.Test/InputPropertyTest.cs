// Project: Aguafrommars/DynamicConfiguration
// Copyright (c) 2021 @Olivier Lefebvre

using Aguacongas.DynamicConfiguration.Razor.Services;
using Bunit;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Threading;
using Xunit;

namespace Aguacongas.DynamicConfiguration.Razor.Test
{
    public class InputPropertyTest : TestContext
    {
        [Fact]
        public void WhenValueIsString_should_display_input_text()
        {
            var model = new Model
            {
                String = Guid.NewGuid().ToString()
            };

            Services.AddLocalization().AddScoped<ISettingsLocalizer, DefaultSettingsLocalizer>();

            var cut = Render<InputProperty>(parameters => parameters
                .Add(p => p.Value, model.String)
                .Add(p => p.Model, model)
                .Add(p => p.Path, "$")
                .Add(p => p.Property, model.GetType().GetProperty(nameof(Model.String)))
                .AddCascadingValue(new EditContext(model)));

            var input = cut.Find($"input[value=\"{model.String}\"]");
            Assert.NotNull(input);

            var expected = Guid.NewGuid().ToString();
            input.Change($"{expected}");

            Assert.Equal(expected, model.String);
        }

        [Fact]
        public void WhenValueIsNullableDecimal_should_display_input_number()
        {
            var model = new Model();

            Services.AddLocalization().AddScoped<ISettingsLocalizer, DefaultSettingsLocalizer>();

            var cut = Render<InputProperty>(parameters => parameters
                .Add(p => p.Value, model.NullableDecimal)
                .Add(p => p.Model, model)
                .Add(p => p.Path, "$")
                .Add(p => p.Property, model.GetType().GetProperty(nameof(Model.NullableDecimal)))
                .AddCascadingValue(new EditContext(model)));

            var input = cut.Find($"input[type=number]");
            Assert.NotNull(input);

            var expected = 1;
            input.Change($"{expected}");

            Assert.Equal(expected, model.NullableDecimal);
        }

        [Fact]
        public void WhenValueIsDecimal_should_display_input_number()
        {
            var model = new Model();

            Services.AddLocalization().AddScoped<ISettingsLocalizer, DefaultSettingsLocalizer>();

            var cut = Render<InputProperty>(parameters => parameters
                .Add(p => p.Value, model.Decimal)
                .Add(p => p.Model, model)
                .Add(p => p.Path, "$")
                .Add(p => p.Property, model.GetType().GetProperty(nameof(Model.Decimal)))
                .AddCascadingValue(new EditContext(model)));

            var input = cut.Find($"input[type=number]");
            Assert.NotNull(input);

            var expected = 1;
            input.Change($"{expected}");

            Assert.Equal(expected, model.Decimal);
        }

        [Fact]
        public void WhenValueIsNullableFloat_should_display_input_number()
        {
            var model = new Model();

            Services.AddLocalization().AddScoped<ISettingsLocalizer, DefaultSettingsLocalizer>();

            var cut = Render<InputProperty>(parameters => parameters
                .Add(p => p.Value, model.NullableFloat)
                .Add(p => p.Model, model)
                .Add(p => p.Path, "$")
                .Add(p => p.Property, model.GetType().GetProperty(nameof(Model.NullableFloat)))
                .AddCascadingValue(new EditContext(model)));

            var input = cut.Find($"input[type=number]");
            Assert.NotNull(input);

            var expected = 1;
            input.Change($"{expected}");

            Assert.Equal(expected, model.NullableFloat);
        }

        [Fact]
        public void WhenValueIsFloat_should_display_input_number()
        {
            var model = new Model();

            Services.AddLocalization().AddScoped<ISettingsLocalizer, DefaultSettingsLocalizer>();

            var cut = Render<InputProperty>(parameters => parameters
                .Add(p => p.Value, model.Float)
                .Add(p => p.Model, model)
                .Add(p => p.Path, "$")
                .Add(p => p.Property, model.GetType().GetProperty(nameof(Model.Float)))
                .AddCascadingValue(new EditContext(model)));

            var input = cut.Find($"input[type=number]");
            Assert.NotNull(input);

            var expected = 1;
            input.Change($"{expected}");

            Assert.Equal(expected, model.Float);
        }

        [Fact]
        public void WhenValueIsNullableDouble_should_display_input_number()
        {
            var model = new Model();

            Services.AddLocalization().AddScoped<ISettingsLocalizer, DefaultSettingsLocalizer>();

            var cut = Render<InputProperty>(parameters => parameters
                .Add(p => p.Value, model.NullableDouble)
                .Add(p => p.Model, model)
                .Add(p => p.Path, "$")
                .Add(p => p.Property, model.GetType().GetProperty(nameof(Model.NullableDouble)))
                .AddCascadingValue(new EditContext(model)));

            var input = cut.Find($"input[type=number]");
            Assert.NotNull(input);

            var expected = 1;
            input.Change($"{expected}");

            Assert.Equal(expected, model.NullableDouble);
        }

        [Fact]
        public void WhenValueIsDouble_should_display_input_number()
        {
            var model = new Model();

            Services.AddLocalization().AddScoped<ISettingsLocalizer, DefaultSettingsLocalizer>();

            var cut = Render<InputProperty>(parameters => parameters
                .Add(p => p.Value, model.Double)
                .Add(p => p.Model, model)
                .Add(p => p.Path, "$")
                .Add(p => p.Property, model.GetType().GetProperty(nameof(Model.Double)))
                .AddCascadingValue(new EditContext(model)));

            var input = cut.Find($"input[type=number]");
            Assert.NotNull(input);

            var expected = 1;
            input.Change($"{expected}");

            Assert.Equal(expected, model.Double);
        }

        [Fact]
        public void WhenValueIsNullableLong_should_display_input_number()
        {
            var model = new Model();

            Services.AddLocalization().AddScoped<ISettingsLocalizer, DefaultSettingsLocalizer>();

            var cut = Render<InputProperty>(parameters => parameters
                .Add(p => p.Value, model.NullableLong)
                .Add(p => p.Model, model)
                .Add(p => p.Path, "$")
                .Add(p => p.Property, model.GetType().GetProperty(nameof(Model.NullableLong)))
                .AddCascadingValue(new EditContext(model)));

            var input = cut.Find($"input[type=number]");
            Assert.NotNull(input);

            var expected = 1;
            input.Change($"{expected}");

            Assert.Equal(expected, model.NullableLong);
        }

        [Fact]
        public void WhenValueIsLong_should_display_input_number()
        {
            var model = new Model();

            Services.AddLocalization().AddScoped<ISettingsLocalizer, DefaultSettingsLocalizer>();

            var cut = Render<InputProperty>(parameters => parameters
                .Add(p => p.Value, model.Long)
                .Add(p => p.Model, model)
                .Add(p => p.Path, "$")
                .Add(p => p.Property, model.GetType().GetProperty(nameof(Model.Long)))
                .AddCascadingValue(new EditContext(model)));

            var input = cut.Find($"input[type=number]");
            Assert.NotNull(input);

            var expected = 1;
            input.Change($"{expected}");

            Assert.Equal(expected, model.Long);
        }

        [Fact]
        public void WhenValueIsNullableInt_should_display_input_number()
        {
            var model = new Model();

            Services.AddLocalization().AddScoped<ISettingsLocalizer, DefaultSettingsLocalizer>();

            var cut = Render<InputProperty>(parameters => parameters
                .Add(p => p.Value, model.NullableInt)
                .Add(p => p.Model, model)
                .Add(p => p.Path, "$")
                .Add(p => p.Property, model.GetType().GetProperty(nameof(Model.NullableInt)))
                .AddCascadingValue(new EditContext(model)));

            var input = cut.Find($"input[type=number]");
            Assert.NotNull(input);

            var expected = 1;
            input.Change($"{expected}");

            Assert.Equal(expected, model.NullableInt);
        }

        [Fact]
        public void WhenValueIsInt_should_display_input_number()
        {
            var model = new Model();

            Services.AddLocalization().AddScoped<ISettingsLocalizer, DefaultSettingsLocalizer>();

            var cut = Render<InputProperty>(parameters => parameters
                .Add(p => p.Value, model.Int)
                .Add(p => p.Model, model)
                .Add(p => p.Path, "$")
                .Add(p => p.Property, model.GetType().GetProperty(nameof(Model.Int)))
                .AddCascadingValue(new EditContext(model)));

            var input = cut.Find($"input[type=number]");
            Assert.NotNull(input);

            var expected = 1;
            input.Change($"{expected}");

            Assert.Equal(expected, model.Int);
        }

        [Fact]
        public void WhenValueIsNullableShort_should_display_input_number()
        {
            var model = new Model();

            Services.AddLocalization().AddScoped<ISettingsLocalizer, DefaultSettingsLocalizer>();

            var cut = Render<InputProperty>(parameters => parameters
                .Add(p => p.Value, model.NullableShort)
                .Add(p => p.Model, model)
                .Add(p => p.Path, "$")
                .Add(p => p.Property, model.GetType().GetProperty(nameof(Model.NullableShort)))
                .AddCascadingValue(new EditContext(model)));

            var input = cut.Find($"input[type=number]");
            Assert.NotNull(input);

            short? expected = 1;
            input.Change($"{expected}");

            Assert.Equal(expected, model.NullableShort);
        }

        [Fact]
        public void WhenValueIsShort_should_display_input_number()
        {
            var model = new Model();

            Services.AddLocalization().AddScoped<ISettingsLocalizer, DefaultSettingsLocalizer>();

            var cut = Render<InputProperty>(parameters => parameters
                .Add(p => p.Value, model.Short)
                .Add(p => p.Model, model)
                .Add(p => p.Path, "$")
                .Add(p => p.Property, model.GetType().GetProperty(nameof(Model.Short)))
                .AddCascadingValue(new EditContext(model)));

            var input = cut.Find($"input[type=number]");
            Assert.NotNull(input);

            var expected = 1;
            input.Change($"{expected}");

            Assert.Equal(expected, model.Short);
        }

        [Fact]
        public void WhenValueIsNullableByte_should_display_input_number()
        {
            var model = new Model();

            Services.AddLocalization().AddScoped<ISettingsLocalizer, DefaultSettingsLocalizer>();

            var cut = Render<InputProperty>(parameters => parameters
                .Add(p => p.Value, model.NullableByte)
                .Add(p => p.Model, model)
                .Add(p => p.Path, "$")
                .Add(p => p.Property, model.GetType().GetProperty(nameof(Model.NullableByte)))
                .AddCascadingValue(new EditContext(model)));

            var input = cut.Find($"input[type=number]");
            Assert.NotNull(input);

            byte? expected = 1;
            input.Change($"{expected}");

            Assert.Equal(expected, model.NullableByte);
        }

        [Fact]
        public void WhenValueIsByte_should_display_input_number()
        {
            var model = new Model();

            Services.AddLocalization().AddScoped<ISettingsLocalizer, DefaultSettingsLocalizer>();

            var cut = Render<InputProperty>(parameters => parameters
                .Add(p => p.Value, model.Byte)
                .Add(p => p.Model, model)
                .Add(p => p.Path, "$")
                .Add(p => p.Property, model.GetType().GetProperty(nameof(Model.Byte)))
                .AddCascadingValue(new EditContext(model)));

            var input = cut.Find($"input[type=number]");
            Assert.NotNull(input);

            byte expected = 1;
            input.Change($"{expected}");

            Assert.Equal(expected, model.Byte);
        }

        [Fact]
        public void WhenValueIsNullableULong_should_display_input_number()
        {
            var model = new Model();

            Services.AddLocalization().AddScoped<ISettingsLocalizer, DefaultSettingsLocalizer>();

            var cut = Render<InputProperty>(parameters => parameters
                .Add(p => p.Value, model.NullableULong)
                .Add(p => p.Model, model)
                .Add(p => p.Path, "$")
                .Add(p => p.Property, model.GetType().GetProperty(nameof(Model.NullableULong)))
                .AddCascadingValue(new EditContext(model)));

            var input = cut.Find($"input[type=number]");
            Assert.NotNull(input);

            ulong? expected = 1;
            input.Change($"{expected}");

            Assert.Equal(expected, model.NullableULong);
        }

        [Fact]
        public void WhenValueIsULong_should_display_input_number()
        {
            var model = new Model();

            Services.AddLocalization().AddScoped<ISettingsLocalizer, DefaultSettingsLocalizer>();

            var cut = Render<InputProperty>(parameters => parameters
                .Add(p => p.Value, model.ULong)
                .Add(p => p.Model, model)
                .Add(p => p.Path, "$")
                .Add(p => p.Property, model.GetType().GetProperty(nameof(Model.ULong)))
                .AddCascadingValue(new EditContext(model)));

            var input = cut.Find($"input[type=number]");
            Assert.NotNull(input);

            ulong expected = 1;
            input.Change($"{expected}");

            Assert.Equal(expected, model.ULong);
        }

        [Fact]
        public void WhenValueIsNullableUInt_should_display_input_number()
        {
            var model = new Model();

            Services.AddLocalization().AddScoped<ISettingsLocalizer, DefaultSettingsLocalizer>();

            var cut = Render<InputProperty>(parameters => parameters
                .Add(p => p.Value, model.NullableUInt)
                .Add(p => p.Model, model)
                .Add(p => p.Path, "$")
                .Add(p => p.Property, model.GetType().GetProperty(nameof(Model.NullableUInt)))
                .AddCascadingValue(new EditContext(model)));

            var input = cut.Find($"input[type=number]");
            Assert.NotNull(input);

            uint? expected = 1;
            input.Change($"{expected}");

            Assert.Equal(expected, model.NullableUInt);
        }

        [Fact]
        public void WhenValueIsUInt_should_display_input_number()
        {
            var model = new Model();

            Services.AddLocalization().AddScoped<ISettingsLocalizer, DefaultSettingsLocalizer>();

            var cut = Render<InputProperty>(parameters => parameters
                .Add(p => p.Value, model.UInt)
                .Add(p => p.Model, model)
                .Add(p => p.Path, "$")
                .Add(p => p.Property, model.GetType().GetProperty(nameof(Model.UInt)))
                .AddCascadingValue(new EditContext(model)));

            var input = cut.Find($"input[type=number]");
            Assert.NotNull(input);

            uint expected = 1;
            input.Change($"{expected}");

            Assert.Equal(expected, model.UInt);
        }

        [Fact]
        public void WhenValueIsNullableUShort_should_display_input_number()
        {
            var model = new Model();

            Services.AddLocalization().AddScoped<ISettingsLocalizer, DefaultSettingsLocalizer>();

            var cut = Render<InputProperty>(parameters => parameters
                .Add(p => p.Value, model.NullableUShort)
                .Add(p => p.Model, model)
                .Add(p => p.Path, "$")
                .Add(p => p.Property, model.GetType().GetProperty(nameof(Model.NullableUShort)))
                .AddCascadingValue(new EditContext(model)));

            var input = cut.Find($"input[type=number]");
            Assert.NotNull(input);

            ushort? expected = 1;
            input.Change($"{expected}");

            Assert.Equal(expected, model.NullableUShort);
        }

        [Fact]
        public void WhenValueIsUShort_should_display_input_number()
        {
            var model = new Model();

            Services.AddLocalization().AddScoped<ISettingsLocalizer, DefaultSettingsLocalizer>();

            var cut = Render<InputProperty>(parameters => parameters
                .Add(p => p.Value, model.UShort)
                .Add(p => p.Model, model)
                .Add(p => p.Path, "$")
                .Add(p => p.Property, model.GetType().GetProperty(nameof(Model.UShort)))
                .AddCascadingValue(new EditContext(model)));

            var input = cut.Find($"input[type=number]");
            Assert.NotNull(input);

            var expected = 1;
            input.Change($"{expected}");

            Assert.Equal(expected, model.UShort);
        }

        [Fact]
        public void WhenValueIsNullableSByte_should_display_input_number()
        {
            var model = new Model();

            Services.AddLocalization().AddScoped<ISettingsLocalizer, DefaultSettingsLocalizer>();

            var cut = Render<InputProperty>(parameters => parameters
                .Add(p => p.Value, model.NullableSByte)
                .Add(p => p.Model, model)
                .Add(p => p.Path, "$")
                .Add(p => p.Property, model.GetType().GetProperty(nameof(Model.NullableSByte)))
                .AddCascadingValue(new EditContext(model)));

            var input = cut.Find($"input[type=number]");
            Assert.NotNull(input);

            sbyte? expected = 1;
            input.Change($"{expected}");

            Assert.Equal(expected, model.NullableSByte);
        }

        [Fact]
        public void WhenValueIsSByte_should_display_input_number()
        {
            var model = new Model();

            Services.AddLocalization().AddScoped<ISettingsLocalizer, DefaultSettingsLocalizer>();

            var cut = Render<InputProperty>(parameters => parameters
                .Add(p => p.Value, model.SByte)
                .Add(p => p.Model, model)
                .Add(p => p.Path, "$")
                .Add(p => p.Property, model.GetType().GetProperty(nameof(Model.SByte)))
                .AddCascadingValue(new EditContext(model)));

            var input = cut.Find($"input[type=number]");
            Assert.NotNull(input);

            var expected = 1;
            input.Change($"{expected}");

            Assert.Equal(expected, model.SByte);
        }

        [Fact]
        public void WhenValueIsNullableTimespan_should_display_input_text_with_placeholer()
        {
            var model = new Model();

            Services.AddLocalization().AddScoped<ISettingsLocalizer, DefaultSettingsLocalizer>();

            var cut = Render<InputProperty>(parameters => parameters
                .Add(p => p.Value, model.NulllableTimeSpan)
                .Add(p => p.Model, model)
                .Add(p => p.Path, "$")
                .Add(p => p.Property, model.GetType().GetProperty(nameof(Model.NulllableTimeSpan)))
                .AddCascadingValue(new EditContext(model)));

            var input = cut.Find($"input[placeholder=\"00:00:00\"]");
            Assert.NotNull(input);

            var expected = TimeSpan.FromMinutes(1);
            input.Change($"{expected}");

            Assert.Equal(expected, model.NulllableTimeSpan);
        }

        [Fact]
        public void WhenValueIsTimespan_should_display_input_text_with_placeholer()
        {
            var model = new Model();

            Services.AddLocalization().AddScoped<ISettingsLocalizer, DefaultSettingsLocalizer>();

            var cut = Render<InputProperty>(parameters => parameters
                .Add(p => p.Value, model.TimeSpan)
                .Add(p => p.Model, model)
                .Add(p => p.Path, "$")
                .Add(p => p.Property, model.GetType().GetProperty(nameof(Model.TimeSpan)))
                .AddCascadingValue(new EditContext(model)));

            var input = cut.Find($"input[placeholder=\"00:00:00\"]");
            Assert.NotNull(input);

            var expected = TimeSpan.FromMinutes(1);
            input.Change($"{expected}");

            Assert.Equal(expected, model.TimeSpan);
        }

        [Fact]
        public void WhenValueIsTimespan_should_display_error_on_parse_exception_and_reset_it_on_no_exception()
        {
            var model = new Model();

            Services.AddLocalization().AddScoped<ISettingsLocalizer, DefaultSettingsLocalizer>();

            var cut = Render<InputProperty>(parameters => parameters
                .Add(p => p.Value, model.TimeSpan)
                .Add(p => p.Model, model)
                .Add(p => p.Path, "$")
                .Add(p => p.Property, model.GetType().GetProperty(nameof(Model.TimeSpan)))
                .AddCascadingValue(new EditContext(model)));

            var input = cut.Find($"input[placeholder=\"00:00:00\"]");
            Assert.NotNull(input);

            input.Change("not a timespan");

            var danger = cut.Find("div[class=\"text-danger\"]");
            Assert.Equal($"Cannot parse 'not a timespan'", danger.InnerHtml);

            var expected = TimeSpan.FromMinutes(1);
            input.Change($"{expected}");

            danger = cut.Find("div[class=\"text-danger\"]");
            Assert.Empty(danger.InnerHtml);
        }

        [Fact]
        public void WhenValueIsNullableDateTime_should_display_input_date()
        {
            var model = new Model();

            Services.AddLocalization().AddScoped<ISettingsLocalizer, DefaultSettingsLocalizer>();

            Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

            var cut = Render<InputProperty>(parameters => parameters
                .Add(p => p.Value, model.NullableDateTime)
                .Add(p => p.Model, model)
                .Add(p => p.Path, "$")
                .Add(p => p.Property, model.GetType().GetProperty(nameof(Model.NullableDateTime)))
                .AddCascadingValue(new EditContext(model)));

            var input = cut.Find($"input[type=\"date\"]");
            Assert.NotNull(input);

            var expected = DateTime.Now;
            input.Change($"{expected}");

            Assert.Equal(expected.ToString(), model.NullableDateTime.ToString());
        }

        [Fact]
        public void WhenValueIsDateTime_should_display_input_date()
        {
            var model = new Model();

            Services.AddLocalization().AddScoped<ISettingsLocalizer, DefaultSettingsLocalizer>();

            Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

            var cut = Render<InputProperty>(parameters => parameters
                .Add(p => p.Value, model.DateTime)
                .Add(p => p.Model, model)
                .Add(p => p.Path, "$")
                .Add(p => p.Property, model.GetType().GetProperty(nameof(Model.DateTime)))
                .AddCascadingValue(new EditContext(model)));

            var input = cut.Find($"input[type=\"date\"]");
            Assert.NotNull(input);

            var expected = DateTime.Now;
            input.Change($"{expected}");

            Assert.Equal(expected.ToString(), model.DateTime.ToString());
        }

        [Fact]
        public void WhenValueIsNullableDateTimeOffset_should_display_input_date()
        {
            var model = new Model();

            Services.AddLocalization().AddScoped<ISettingsLocalizer, DefaultSettingsLocalizer>();

            Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

            var cut = Render<InputProperty>(parameters => parameters
                .Add(p => p.Value, model.NullableDateTimeOffset)
                .Add(p => p.Model, model)
                .Add(p => p.Path, "$")
                .Add(p => p.Property, model.GetType().GetProperty(nameof(Model.NullableDateTimeOffset)))
                .AddCascadingValue(new EditContext(model)));

            var input = cut.Find($"input[type=\"date\"]");
            Assert.NotNull(input);

            var expected = DateTimeOffset.Now;
            input.Change($"{expected}");

            Assert.Equal(expected.ToString(), model.NullableDateTimeOffset.ToString());
        }

        [Fact]
        public void WhenValueIsDateTimeOffset_should_display_input_date()
        {
            var model = new Model();

            Services.AddLocalization().AddScoped<ISettingsLocalizer, DefaultSettingsLocalizer>();

            Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

            var cut = Render<InputProperty>(parameters => parameters
                .Add(p => p.Value, model.DateTimeOffset)
                .Add(p => p.Model, model)
                .Add(p => p.Path, "$")
                .Add(p => p.Property, model.GetType().GetProperty(nameof(Model.DateTimeOffset)))
                .AddCascadingValue(new EditContext(model)));

            var input = cut.Find($"input[type=\"date\"]");
            Assert.NotNull(input);

            var expected = DateTimeOffset.Now;
            input.Change($"{expected}");

            Assert.Equal(expected.ToString(), model.DateTimeOffset.ToString());
        }

        [Fact]
        public void WhenValueIsNullableBool_should_display_input_checkbox()
        {
            var model = new Model();

            Services.AddLocalization().AddScoped<ISettingsLocalizer, DefaultSettingsLocalizer>();

            var cut = Render<InputProperty>(parameters => parameters
                .Add(p => p.Value, model.NullableBool)
                .Add(p => p.Model, model)
                .Add(p => p.Path, "$")
                .Add(p => p.Property, model.GetType().GetProperty(nameof(Model.NullableBool)))
                .AddCascadingValue(new EditContext(model)));

            var input = cut.Find($"input[type=\"checkbox\"]");
            Assert.NotNull(input);

            var expected = true;
            input.Change(expected);

            Assert.Equal(expected, model.NullableBool);
        }

        [Fact]
        public void WhenValueIsBool_should_display_input_checkbox()
        {
            var model = new Model();

            Services.AddLocalization().AddScoped<ISettingsLocalizer, DefaultSettingsLocalizer>();

            var cut = Render<InputProperty>(parameters => parameters
                .Add(p => p.Value, model.Bool)
                .Add(p => p.Model, model)
                .Add(p => p.Path, "$")
                .Add(p => p.Property, model.GetType().GetProperty(nameof(Model.Bool)))
                .AddCascadingValue(new EditContext(model)));

            var input = cut.Find($"input[type=\"checkbox\"]");
            Assert.NotNull(input);

            var expected = true;
            input.Change(true);

            Assert.Equal(expected, model.Bool);
        }

        [Fact]
        public void WhenValueIsNullableEnum_should_display_InputEnum()
        {
            var model = new Model();

            Services.AddLocalization().AddScoped<ISettingsLocalizer, DefaultSettingsLocalizer>();

            var cut = Render<InputProperty>(parameters => parameters
                .Add(p => p.Value, model.NullableEnum)
                .Add(p => p.Model, model)
                .Add(p => p.Path, "$")
                .Add(p => p.Property, model.GetType().GetProperty(nameof(Model.NullableEnum)))
                .AddCascadingValue(new EditContext(model)));

            var input = cut.FindComponent<InputEnum>();
            Assert.NotNull(input);

            var select = cut.Find("select");
            var expected = HttpStatusCode.Redirect;
            select.Change($"{expected}");

            Assert.Equal(expected, model.NullableEnum);
        }

        [Fact]
        public void WhenValueIsEnum_should_display_InputEnum()
        {
            var model = new Model();

            Services.AddLocalization().AddScoped<ISettingsLocalizer, DefaultSettingsLocalizer>();

            var cut = Render<InputProperty>(parameters => parameters
                .Add(p => p.Value, model.Enum)
                .Add(p => p.Model, model)
                .Add(p => p.Path, "$")
                .Add(p => p.Property, model.GetType().GetProperty(nameof(Model.Enum)))
                .AddCascadingValue(new EditContext(model)));

            var input = cut.FindComponent<InputEnum>();
            Assert.NotNull(input);

            var select = cut.Find("select");
            var expected = HttpStatusCode.Redirect;
            select.Change($"{expected}");

            Assert.Equal(expected, model.Enum);
        }

        [Fact]
        public void WhenValueIsEnumerable_should_display_InputEnumerable()
        {
            var model = new Model();

            Services.AddLocalization().AddScoped<ISettingsLocalizer, DefaultSettingsLocalizer>();

            var cut = Render<InputProperty>(parameters => parameters
                .Add(p => p.Value, model.Enumerable)
                .Add(p => p.Model, model)
                .Add(p => p.Path, "$")
                .Add(p => p.Property, model.GetType().GetProperty(nameof(Model.Enumerable)))
                .AddCascadingValue(new EditContext(model)));

            var input = cut.FindComponent<InputEnumerable>();
            Assert.NotNull(input);

            var button = input.Find("button");
            button.Click();

            Assert.Single(model.Enumerable);
        }

        [Fact]
        public void WhenValueIsDictionary_should_display_InputEnumErable()
        {
            var model = new Model();

            Services.AddLocalization().AddScoped<ISettingsLocalizer, DefaultSettingsLocalizer>();


            var cut = Render<InputProperty>(parameters => parameters
                .Add(p => p.Value, model.Dictionary)
                .Add(p => p.Model, model)
                .Add(p => p.Path, "$")
                .Add(p => p.Property, model.GetType().GetProperty(nameof(Model.Dictionary)))
                .AddCascadingValue(new EditContext(model)));

            var input = cut.FindComponent<InputEnumerable>();
            Assert.NotNull(input);

            var inputKey = input.Find("input");
            var expected = Guid.NewGuid().ToString();
            inputKey.Input(expected);

            var button = input.Find("button");
            button.Click();

            Assert.Contains(expected, model.Dictionary?.Keys);
        }

        [Fact]
        public void WhenValueIsOtherObject_should_display_create_button()
        {
            var model = new Model();

            Services.AddLocalization().AddScoped<ISettingsLocalizer, DefaultSettingsLocalizer>();

            var cut = Render<InputProperty>(parameters => parameters
                .Add(p => p.Value, model.Child)
                .Add(p => p.Model, model)
                .Add(p => p.Path, "$")
                .Add(p => p.Property, model.GetType().GetProperty(nameof(Model.Child)))
                .AddCascadingValue(new EditContext(model)));

            Assert.Null(model.Child);

            var button = cut.Find("button");
            button.Click();

            Assert.NotNull(model.Child);
        }

        [Fact]
        public void WhenValueIsObjectWihtoutEmptyConstructor_should_throw_exception()
        {
            var model = new Model();

            Services.AddLocalization().AddScoped<ISettingsLocalizer, DefaultSettingsLocalizer>();

            var cut = Render<InputProperty>(parameters => parameters
                .Add(p => p.Value, model.CannotCreateChild)
                .Add(p => p.Model, model)
                .Add(p => p.Path, "$")
                .Add(p => p.Property, model.GetType().GetProperty(nameof(Model.CannotCreateChild)))
                .AddCascadingValue(new EditContext(model)));

            Assert.Null(model.CannotCreateChild);

            var button = cut.Find("button");
            Assert.Throws<InvalidOperationException>(() => button.Click());
        }

        class Model
        {
            public decimal? NullableDecimal { get; set; }
            public decimal Decimal { get; set; }

            public float? NullableFloat { get; set; }
            public float Float { get; set; }

            public double? NullableDouble { get; set; }
            public double Double { get; set; }

            public long? NullableLong { get; set; }
            public long Long { get; set; }

            public int? NullableInt { get; set; }
            public int Int { get; set; }

            public short? NullableShort { get; set; }
            public short Short { get; set; }

            public byte? NullableByte { get; set; }
            public byte Byte { get; set; }

            public ulong? NullableULong { get; set; }
            public ulong ULong { get; set; }

            public uint? NullableUInt { get; set; }
            public uint UInt { get; set; }

            public ushort? NullableUShort { get; set; }
            public ushort UShort { get; set; }

            public sbyte? NullableSByte { get; set; }
            public sbyte SByte { get; set; }

            public TimeSpan? NulllableTimeSpan { get; set; }

            public TimeSpan TimeSpan { get; set; }

            public DateTime? NullableDateTime { get; set; }

            public DateTime DateTime { get; set; }

            public DateTimeOffset? NullableDateTimeOffset { get; set; }

            public DateTimeOffset DateTimeOffset { get; set; }

            public bool? NullableBool { get; set; }

            public bool Bool { get; set; }

            public string? String { get; set; }

            public IEnumerable<Model>? Enumerable { get; set; }

            public Dictionary<string, Model>? Dictionary { get; set; }
            public HttpStatusCode? NullableEnum { get; set; }

            public HttpStatusCode Enum { get; set; }

            public Model? Child { get; set; }

            public Tuple<string, Model>? CannotCreateChild { get; set; }
        }
    }
}
