// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace NuGet.PackageManagement.UI
{
    public class ProjectAndSolutionViewHeightConverter : IValueConverter
    {
        private static GridLength _solutionViewHeight = new GridLength(300);

        // Returns the grid height for grid row containing the ProjectView/SolutionView.
        // The value is the IsSolution property.
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var isSolution = (bool)value;
            if (isSolution)
            {
                return _solutionViewHeight;
            }
            else
            {
                return GridLength.Auto;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // no op
            Debug.Fail("Not Implemented");
            return null;
        }
    }
}
