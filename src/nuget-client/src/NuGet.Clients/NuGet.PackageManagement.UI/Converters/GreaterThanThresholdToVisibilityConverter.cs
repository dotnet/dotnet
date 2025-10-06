// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace NuGet.PackageManagement.UI
{
    public class GreaterThanThresholdToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType == typeof(Visibility))
            {
                if (value is not null && parameter is not null)
                {
                    bool valueConverted = TryConvertToInt64(value, culture, out long? initialValue);
                    bool paramConverted = TryConvertToInt64(parameter, culture, out long? thresholdValue);

                    if (valueConverted && paramConverted
                        && initialValue is not null && thresholdValue is not null
                        && initialValue > thresholdValue)
                    {
                        return Visibility.Visible;
                    }
                }

                return Visibility.Collapsed;
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // no op
            Debug.Fail("Not Implemented");
            return null;
        }

        private static bool TryConvertToInt64(object value, CultureInfo culture, out long? convertedValue)
        {
            if (value is null)
            {
                convertedValue = null;
                return false;
            }

            try
            {
                convertedValue = System.Convert.ToInt64(value, culture);
                return true;
            }
            catch (Exception e) when (e is FormatException or InvalidCastException or OverflowException)
            {
                convertedValue = null;
                return false;
            }
        }
    }
}
