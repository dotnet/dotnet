// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace NuGet.PackageManagement.UI
{
    /// <summary>
    /// This converter is taking the first value and substracting the second and third value to it.
    /// This was the way I was able to get the MaxWidth of the left side of the UI by
    /// substracting the RightSideMinWidth + GridSplittlerWidth from the current
    /// ActualWidth of the UI.
    /// 
    /// `values[0]` is supposed to be the actual width of `_root` which has a minWidth
    /// of `LeftSideMinWidth + GridSplitterWidth + RightSideMinWidth`, `values[1]`
    /// is `GridSplitterWidth` and `values[2]` is `RightSideMinWidth`,
    /// therefore this converter should return `LeftSideMinWidth` as the smallest value.
    /// </summary>
    public class SubstractionConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values != null
                && values.Length == 3
                && (values[0] == DependencyProperty.UnsetValue || values[1] == DependencyProperty.UnsetValue || values[2] == DependencyProperty.UnsetValue))
            {
                return 0;
            }

            if (values[0] is double v0 && values[1] is double v1 && values[2] is double v2)
            {
                return v0 - v1 - v2;
            }

            return 0;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
