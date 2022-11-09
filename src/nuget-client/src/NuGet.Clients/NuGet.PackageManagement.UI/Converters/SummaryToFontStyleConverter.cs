// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace NuGet.PackageManagement.UI
{
    // This class is used to return the FontStyle used to display the package summary.
    public class SummaryToFontStyleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var summary = (string)value;
            if (String.IsNullOrEmpty(summary))
            {
                // The package metadata is not available from the selected package source.
                // In this case, display a message "not available in this source"
                // in italic.
                return FontStyles.Italic;
            }
            else
            {
                return FontStyles.Normal;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // no op
            Debug.Fail("Not Implemented");
            return null;
        }
    }
}
