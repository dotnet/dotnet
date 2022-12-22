// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Data;

namespace NuGet.PackageManagement.UI
{
    /// <summary>
    /// This will convert from DisplayVersion to tooltip string for blocked versions label
    /// </summary>
    public class TooltipConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DisplayVersion version = value as DisplayVersion;

            return version != null && !version.IsValidVersion ? Resources.ToolTip_BlockedVersion : null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // no op
            Debug.Fail("Not Implemented");
            return null;
        }
    }
}
