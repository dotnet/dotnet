// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading;
using System.Windows;
using Xunit;

namespace NuGet.PackageManagement.UI.Test.Converters
{
    public class GreaterThanThresholdToVisibilityConverterTests
    {
        [Theory]
        [InlineData(0L)]
        [InlineData(1L)]
        [InlineData(2147483657L)]
        public void Convert_ValidLongValue_Return_VisibilityVisible(long? downloadCount)
        {
            var converter = new GreaterThanThresholdToVisibilityConverter();

            object converted = converter.Convert(
                downloadCount,
                typeof(Visibility),
                "-1",
                Thread.CurrentThread.CurrentCulture);

            Assert.Equal(Visibility.Visible, converted);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(5000)]
        public void Convert_ValidInt_Return_VisibilityVisible(int? downloadCount)
        {
            var converter = new GreaterThanThresholdToVisibilityConverter();

            object converted = converter.Convert(
                downloadCount,
                typeof(Visibility),
                "0",
                Thread.CurrentThread.CurrentCulture);

            Assert.Equal(Visibility.Visible, converted);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(-1L)]
        public void Convert_ValidLongValue_Return_VisibilityCollapsed(long? downloadCount)
        {
            var converter = new GreaterThanThresholdToVisibilityConverter();

            object converted = converter.Convert(
                downloadCount,
                typeof(Visibility),
                "-1",
                Thread.CurrentThread.CurrentCulture);

            Assert.Equal(Visibility.Collapsed, converted);
        }

        [Theory]
        [InlineData(1, null)]
        [InlineData(null, 1)]
        [InlineData(null, null)]
        [InlineData("non-numeric", 1)]
        [InlineData(1, "non-numeric")]
        [InlineData("9223372036854775808", 1)] // test overflowed Int64 number as string value
        public void Convert_InvalidLongValue_Return_VisibilityCollapsed(object value, object param)
        {
            var converter = new GreaterThanThresholdToVisibilityConverter();

            object converted = converter.Convert(
                value,
                typeof(Visibility),
                param,
                Thread.CurrentThread.CurrentCulture);

            Assert.Equal(Visibility.Collapsed, converted);
        }
    }
}
