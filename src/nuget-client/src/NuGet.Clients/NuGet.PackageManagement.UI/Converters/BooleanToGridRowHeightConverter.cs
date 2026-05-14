// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace NuGet.PackageManagement.UI
{
    public class BooleanToGridRowHeightConverter : IValueConverter
    {
        public bool Inverted { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var boolValue = (bool)value;
            if (Inverted)
            {
                boolValue = !boolValue;
            }

            return boolValue ? GetGridLength(parameter) : new GridLength(0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        private static GridLength GetGridLength(object parameter)
        {
            return parameter as GridLength? ?? new GridLength(1, GridUnitType.Star);
        }
    }
}
