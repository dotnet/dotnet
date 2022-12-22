// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace NuGet.PackageManagement.UI
{
    /// <summary>
    /// If the value is an empty or null IEnumerable, returns Visibility.Collapsed.
    /// Otherwise, returns Visibility.Visible.
    /// When Inverted is true, the returned values are reversed.
    /// </summary>
    public class EnumerableToVisibilityConverter : IValueConverter
    {
        public bool Inverted { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType == typeof(Visibility))
            {
                var list = value as IEnumerable;
                var isNullOrEmpty = IsNullOrEmpty(list);
                if (Inverted)
                {
                    isNullOrEmpty = !isNullOrEmpty;
                }

                return isNullOrEmpty ?
                    Visibility.Collapsed :
                    Visibility.Visible;
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // no op
            Debug.Fail("Not Implemented");
            return null;
        }

        private static bool IsNullOrEmpty(IEnumerable list)
        {
            if (list == null)
            {
                return true;
            }

            var enumerator = list.GetEnumerator();
            return enumerator.MoveNext() == false;
        }
    }
}
