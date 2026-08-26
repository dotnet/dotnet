// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Data;

namespace NuGet.PackageManagement.UI
{
    /// <summary>
    /// Converts an installed vulnerable package count into the warning tooltip text, using the correct
    /// singular/plural form via <see cref="UIUtility.GetInstalledVulnerablePackagesWarningText(int)"/>.
    /// </summary>
    public class InstalledVulnerablePackagesWarningConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var count = (int)value;
            return UIUtility.GetInstalledVulnerablePackagesWarningText(count);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // no op
            Debug.Fail("Not Implemented");
            return null;
        }
    }
}
