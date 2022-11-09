// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace NuGet.PackageManagement.UI
{
    /// <summary>
    /// The purpose of this converter is to  get the total MinWidth of the content grid
    /// by adding the MinWidth of it's content
    /// `values[0]` is MinWidth of the `_leftSideGridColumn`
    /// `values[1]` is Width of the `_gridSplitter`
    /// `values[2]` is MinWidth of the `_rightSideGridColumn`
    /// </summary>
    public class AdditionConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] == DependencyProperty.UnsetValue || values[1] == DependencyProperty.UnsetValue || values[2] == DependencyProperty.UnsetValue)
            {
                return 0;
            }
            return System.Convert.ToDouble(values[0], culture) + System.Convert.ToDouble(values[1], culture) + System.Convert.ToDouble(values[2], culture);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
