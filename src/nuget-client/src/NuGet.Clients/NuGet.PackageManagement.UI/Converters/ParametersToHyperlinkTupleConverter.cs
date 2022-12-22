// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;
using System.Windows.Data;
using NuGet.PackageManagement.Telemetry;

namespace NuGet.PackageManagement.UI
{
    public class ParametersToHyperlinkTupleConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null || values.Length != 2)
            {
                return null;
            }

            if (values[0] is string query && values[1] is HyperlinkType hyperlinkType)
            {
                return new Tuple<string, HyperlinkType>(query, hyperlinkType);
            }

            return null;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
