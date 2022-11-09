// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;
using System.Windows.Data;
using NuGet.VisualStudio.Internal.Contracts;

namespace NuGet.PackageManagement.UI
{
    /// <summary>
    /// Converts from <see cref="PackageDeprecationMetadataContextInfo"/> to <see cref="PackageItemDeprecationLabelState"/>
    /// </summary>
    public class DeprecationToDeprecationLabelStateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is PackageDeprecationMetadataContextInfo deprecation)
            {
                if (!string.IsNullOrEmpty(deprecation.AlternatePackage?.PackageId))
                {
                    return PackageItemDeprecationLabelState.AlternativeAvailable;
                }

                return PackageItemDeprecationLabelState.Deprecation;
            }

            return PackageItemDeprecationLabelState.Invisible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
