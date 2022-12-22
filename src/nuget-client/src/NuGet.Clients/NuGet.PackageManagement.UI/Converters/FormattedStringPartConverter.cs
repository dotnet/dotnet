// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;
using System.Windows.Data;

namespace NuGet.PackageManagement.UI
{
    /// <summary>
    /// Parameter for <see cref="FormattedStringPartConverter"/>
    /// </summary>
    public enum FormattedStringPart
    {
        Prefix,
        Suffix,
    }

    /// <summary>
    /// Extracts from a formatted string with one '{0}' placeholder to either left or right side of the placeholder
    /// </summary>
    public class FormattedStringPartConverter : IValueConverter
    {
        /// <summary>
        /// Extracts either left or right side of a string with one placeholder '{0}'
        /// </summary>
        /// <param name="value">string with placeholder</param>
        /// <param name="targetType">Not used</param>
        /// <param name="parameter">A <see cref="FormattedStringPart"/> value</param>
        /// <param name="culture">Not used</param>
        /// <returns><c>null </c> if invalid value or parameter, otherwise a string with either left or right side</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string formattedString = value as string;
            if (formattedString == null)
            {
                return null;
            }

            var mode = parameter as FormattedStringPart?;
            if (!mode.HasValue)
            {
                return null;
            }

            int placeholderIndex = formattedString.IndexOf("{0}", StringComparison.Ordinal);
            if (placeholderIndex < 0)
            {
                return null;
            }

            switch (mode)
            {
                case FormattedStringPart.Prefix:
                    return formattedString.Substring(0, placeholderIndex);
                case FormattedStringPart.Suffix:
                    return formattedString.Substring(placeholderIndex + "{0}".Length);
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
