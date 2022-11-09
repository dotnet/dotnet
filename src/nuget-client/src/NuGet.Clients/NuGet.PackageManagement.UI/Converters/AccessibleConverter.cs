// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace NuGet.PackageManagement.UI
{
    /// <summary>
    /// This AccessibleConverter is a workaround for a WPF issue that prevents AccessibleProperties.Name from working well for databound TreeViewItems.
    /// </summary>
    public class AccessibleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var tvi = value as TreeViewItem;
            if (tvi != null)
            {
                var packageDependencySet = tvi.DataContext as PackageDependencySetMetadata;
                if (packageDependencySet != null)
                {
                    // we could return this value so it gets set as AccessibleProperties.Name, or set the header, both fix our problem with AccessibleProperties.Name
                    // Filed a bug on WPF about this problem: https://github.com/dotnet/wpf/issues/2552
                    tvi.Header = packageDependencySet.TargetFrameworkDisplay;
                }
            }
            // don't have databinding actually set the value. Setting treeViewItem.Header works around the problem.
            return Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // no op
            Debug.Fail("Not Implemented");
            return null;
        }
    }
}
