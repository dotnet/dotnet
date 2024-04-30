// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace NuGet.PackageManagement.UI
{
    internal class LastItemToVisibilityConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values != null && values.Length == 2
                && values[0] is int currentIndex
                && values[1] is int length)
            {
                if (currentIndex < 0 || length < 1 || currentIndex >= length)
                {
                    return DependencyProperty.UnsetValue;
                }

                int lastIndex = length - 1;
                return Equals(currentIndex, lastIndex) ? Visibility.Collapsed : Visibility.Visible;
            }

            return DependencyProperty.UnsetValue;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            Debug.Fail("Not Implemented");
            return null;
        }
    }
}
