// Project: Aguafrommars/DynamicConfiguration
// Copyright (c) 2021 @Olivier Lefebvre

using Microsoft.AspNetCore.Components;
using System.Collections;

namespace Aguacongas.DynamicConfiguration.Razor
{
    /// <summary>
    /// Property value base component.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class PropertyValueBase<T> : ComponentBase
    {
        /// <summary>
        /// Gets or sets the model.
        /// </summary>
        [Parameter]
        public T? Model { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        [Parameter]
        public object? Value { get; set; }

        /// <summary>
        /// Gets or sets the property type.
        /// </summary>
        public virtual Type? PropertyType { get; set; }

        /// <summary>
        /// Gets or sets the error messaegs.
        /// </summary>
        protected virtual string? Error { get; set; }

        /// <summary>
        /// Sets the property value.
        /// </summary>
        /// <param name="value">The value</param>
        protected abstract void SetValue(object? value);

        /// <summary>
        /// Gets the underlying property type.
        /// </summary>
        protected virtual Type UnderlyingType => PropertyType is not null
            ? Nullable.GetUnderlyingType(PropertyType) ?? PropertyType
            : throw new InvalidOperationException("PropertyType cannot be null");

        /// <summary>
        /// Gets the place holder.
        /// </summary>
        protected virtual string? Placeholder => IsTimeSpan ? "00:00:00" : null;

        /// <summary>
        /// Return true if the property type is a <see cref="TimeSpan"/>.
        /// </summary>
        protected virtual bool IsTimeSpan => UnderlyingType.IsAssignableTo(typeof(TimeSpan));

        /// <summary>
        /// Return true if the property type is a <see cref="string"/>.
        /// </summary>
        protected virtual bool IsString => UnderlyingType.IsAssignableTo(typeof(string)) || IsTimeSpan;

        /// <summary>
        /// Return true if the property type is a number.
        /// </summary>
        protected virtual bool IsNumber => Type.GetTypeCode(UnderlyingType) switch
        {
            TypeCode.Int16 or
            TypeCode.Int32 or
            TypeCode.Int64 or
            TypeCode.Decimal or
            TypeCode.Double or
            TypeCode.Single or
            TypeCode.Byte or
            TypeCode.SByte or
            TypeCode.UInt16 or
            TypeCode.UInt32 or
            TypeCode.UInt64 => true,
            _ => false,
        };

        /// <summary>
        /// Return true if the property type is a date.
        /// </summary>
        protected virtual bool IsDate => UnderlyingType.IsAssignableTo(typeof(DateTime)) || UnderlyingType.IsAssignableTo(typeof(DateTimeOffset));

        /// <summary>
        /// Return true if the property type is a <see cref="bool"/>.
        /// </summary>
        protected virtual bool IsBool => UnderlyingType.IsAssignableTo(typeof(bool));

        /// <summary>
        /// Return true if the property type is an <see cref="Enum"/>.
        /// </summary>
        protected virtual bool IsEnum => UnderlyingType.IsEnum;

        /// <summary>
        /// Return true if the property type is an <see cref="IEnumerable"/>.
        /// </summary>
        protected virtual bool IsEnumerable => UnderlyingType.IsAssignableTo(typeof(IEnumerable));

        /// <summary>
        /// Gets or sets the value as <see cref="string"/>.
        /// </summary>
        protected virtual string? ValueAsString
        {
            get { return Value?.ToString(); }
            set
            {
                if (UnderlyingType.IsAssignableTo(typeof(string)))
                {
                    SetValue(value);
                    return;
                }
                if (IsTimeSpan)
                {
                    if (!TimeSpan.TryParse(value, out TimeSpan timeSpan))
                    {
                        Error = $"Cannot parse '{value}'";
                        return;
                    }
                    SetValue(timeSpan);
                }
                Error = null;
            }
        }

        /// <summary>
        /// Gets or sets the value as <see cref="double"/>.
        /// </summary>
        protected virtual double? ValueAsDouble
        {
            get
            {
                if (Value is null)
                {
                    return null;
                }
                return Convert.ToDouble(Value);
            }
            set
            {
                if (value is null)
                {
                    SetValue(null);
                    return;
                }

                Value = Type.GetTypeCode(UnderlyingType) switch
                {
                    TypeCode.Int16 => Convert.ToInt16(value),
                    TypeCode.Int32 => Convert.ToInt32(value),
                    TypeCode.Int64 => Convert.ToInt64(value),
                    TypeCode.Decimal => Convert.ToDecimal(value),
                    TypeCode.Double => Convert.ToDouble(value),
                    TypeCode.Single => Convert.ToSingle(value),
                    TypeCode.Byte => Convert.ToByte(value),
                    TypeCode.SByte => Convert.ToSByte(value),
                    TypeCode.UInt16 => Convert.ToUInt16(value),
                    TypeCode.UInt32 => Convert.ToUInt32(value),
                    TypeCode.UInt64 => Convert.ToUInt64(value),
                    _ => null
                };

                SetValue(Value);
            }
        }

        /// <summary>
        /// Gets or sets the value as <see cref="bool"/>.
        /// </summary>
        protected virtual bool ValueAsBool
        {
            get
            {
                if (Value is null)
                {
                    return false;
                }
                return (bool)Value;
            }
            set
            {
                SetValue(value);
            }
        }

        /// <summary>
        /// Gets or sets the value as <see cref="DateTimeOffset"/>.
        /// </summary>
        protected virtual DateTimeOffset? ValueAsDate
        {
            get
            {
                if (Value is null)
                {
                    return null;
                }
                if (UnderlyingType.IsAssignableTo(typeof(DateTimeOffset)))
                {
                    return (DateTimeOffset)Value;
                }
                var dateTime = (DateTime)Value;
                if (dateTime.ToUniversalTime() <= DateTimeOffset.MinValue.UtcDateTime)
                {
                    return DateTimeOffset.MinValue;
                }
                if (dateTime.ToUniversalTime() >= DateTimeOffset.MaxValue.UtcDateTime)
                {
                    return DateTimeOffset.MaxValue;
                }
                return new DateTimeOffset(dateTime);
            }
            set
            {
                if (UnderlyingType.IsAssignableTo(typeof(DateTimeOffset)))
                {
                    SetValue(value);
                    return;
                }
                SetValue(value?.DateTime);
            }
        }

        /// <summary>
        /// Gets or sets the value as <see cref="Enum"/>.
        /// </summary>
        protected virtual Enum? ValueAsEnum
        {
            get
            {
                if (Value is null)
                {
                    return null;
                }
                return (Enum)Value;
            }
            set
            {
                SetValue(value);
            }
        }

    }
}
