// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Data;
using NuGet.Versioning;

namespace NuGet.PackageManagement.UI
{
    public class VersionToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var version = value as NuGetVersion;
            if (version == null)
            {
                return null;
            }

            var displayVersion = new DisplayVersion(version, string.Empty, versionFormat: parameter as string);
            return displayVersion.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // no op
            Debug.Fail("Not Implemented");
            return null;
        }
    }
}
