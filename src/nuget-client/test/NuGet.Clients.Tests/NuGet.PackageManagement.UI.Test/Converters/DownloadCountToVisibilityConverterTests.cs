// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading;
using System.Windows;
using Xunit;

namespace NuGet.PackageManagement.UI.Test.Converters
{
    public class DownloadCountToVisibilityConverterTests
    {
        [Theory]
        [InlineData(0L)]
        [InlineData(1L)]
        [InlineData(2147483657L)]
        public void Convert_ValidLongValue_Return_VisibilityVisible(long? downloadCount)
        {
            var converter = new DownloadCountToVisibilityConverter();

            object converted = converter.Convert(
                downloadCount,
                typeof(Visibility),
                null,
                Thread.CurrentThread.CurrentCulture);

            Assert.Equal(Visibility.Visible, converted);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(-1L)]
        public void Convert_ValidLongValue_Return_VisibilityCollapsed(long? downloadCount)
        {
            var converter = new DownloadCountToVisibilityConverter();

            object converted = converter.Convert(
                downloadCount,
                typeof(Visibility),
                null,
                Thread.CurrentThread.CurrentCulture);

            Assert.Equal(Visibility.Collapsed, converted);
        }
    }
}
