// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.ComponentModel;
using System.Globalization;

namespace NuGet.Versioning
{
    /// <summary>
    /// Provides a type converter to convert <see cref="SemanticVersion"/> objects to and from various other representations.
    /// </summary>
    public class SemanticVersionConverter : TypeConverter
    {
        /// <summary>
        /// Gets a value indicating whether this converter can convert an object in the given source type to a
        /// <see cref="SemanticVersion"/> using the specified context.
        /// </summary>
        /// <param name="context">An <see cref="ITypeDescriptorContext"/> that provides a format context.</param>
        /// <param name="sourceType">A <see cref="Type"/> that represents the type you wish to convert from.</param>
        /// <returns><c>true</c> if this object can perform the conversion; otherwise, <c>false</c>.</returns>
        public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        {
            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }

        /// <summary>
        /// Converts the given object to the type of this converter, using the specified context and culture information.
        /// </summary>
        /// <param name="context">An <see cref="ITypeDescriptorContext"/> that provides a format context.</param>
        /// <param name="culture">The <see cref="CultureInfo"/> to use as the current culture.</param>
        /// <param name="value">The <see cref="object"/> to convert.</param>
        /// <returns>An <see cref="object" /> that represents the converted value.</returns>
        /// <exception cref="ArgumentException"><c>value</c> is not a valid value for the target type.</exception>
        /// <exception cref="NotSupportedException">The conversion cannot be performed.</exception>
        public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
        {
            if (value is null)
            {
                return null;
            }

            if (value is string versionString)
            {
                return SemanticVersion.Parse(versionString);
            }

            return base.ConvertFrom(context, culture, value);
        }

        /// <summary>
        /// Gets a value indicating whether this converter can convert an object to the given destination type using the context.
        /// </summary>
        /// <param name="context">An <see cref="ITypeDescriptorContext"/> that provides a format context.</param>
        /// <param name="destinationType">A <see cref="Type"/> that represents the type you wish to convert to.</param>
        /// <returns><c>true</c> if this object can perform the conversion; otherwise, <c>false</c>.</returns>
        public override bool CanConvertTo(ITypeDescriptorContext? context, Type? destinationType)
        {
            return destinationType == typeof(string) || base.CanConvertTo(context, destinationType);
        }

        /// <summary>
        /// Converts the given value object to a <see cref="SemanticVersion"/> object using the arguments.
        /// </summary>
        /// <param name="context">An <see cref="ITypeDescriptorContext"/> that provides a format context.</param>
        /// <param name="culture">The <see cref="CultureInfo"/> to use as the current culture.</param>
        /// <param name="value">The <see cref="object"/> to convert.</param>
        /// <param name="destinationType">The <see cref="Type"/> to convert the value to.</param>
        /// <returns>An <see cref="object" /> that represents the converted value.</returns>
        /// <exception cref="NotSupportedException">The conversion cannot be performed.</exception>
        public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
        {
            if (value is null)
            {
                return null;
            }

            if (destinationType == typeof(string) && value is SemanticVersion version)
            {
                return version.ToFullString();
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
