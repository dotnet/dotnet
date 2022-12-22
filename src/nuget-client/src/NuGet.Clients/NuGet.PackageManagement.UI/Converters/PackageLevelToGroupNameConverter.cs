// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;
using System.Windows.Data;
using NuGet.VisualStudio;

namespace NuGet.PackageManagement.UI
{
    internal class PackageLevelToGroupNameConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values != null
                && values.Length == 3
                && values[0] is PackageLevel pkgLevel
                && values[1] is int topLevelCount
                && values[2] is int transitiveCount)
            {
                if (pkgLevel == PackageLevel.TopLevel)
                {
                    return string.Format(CultureInfo.CurrentCulture, Resources.PackageLevel_TopLevelPackageHeaderText, topLevelCount);
                }
                else if (pkgLevel == PackageLevel.Transitive)
                {
                    return string.Format(CultureInfo.CurrentCulture, Resources.PackageLevel_TransitivePackageHeaderText, transitiveCount);
                }
            }

            return null;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
