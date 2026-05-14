// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading;
using System.Windows;
using Xunit;

namespace NuGet.PackageManagement.UI.Test.Converters
{
    public class LastItemToVisibilityConverterTests
    {
        private LastItemToVisibilityConverter _converter;

        public LastItemToVisibilityConverterTests()
        {
            _converter = new LastItemToVisibilityConverter();
        }

        [Theory]
        [InlineData(0, 1, Visibility.Collapsed)]
        [InlineData(0, 3, Visibility.Visible)]
        [InlineData(1, 3, Visibility.Visible)]
        [InlineData(2, 3, Visibility.Collapsed)]
        public void LastItemToVisibilityConverter_WithValidValues_ReturnsExpectedVisibility(int? currentIndex, int? lastIndex, Visibility expectedVisibility)
        {
            object converted = _converter.Convert(
                [currentIndex, lastIndex],
                typeof(Visibility),
                null,
                Thread.CurrentThread.CurrentCulture);

            Assert.Equal(expectedVisibility, converted);
        }

        [Theory]
        [InlineData(1, 0)]
        [InlineData(1, 1)]
        [InlineData(0, -1)]
        [InlineData(-1, 5)]
        public void LastItemToVisibilityConverter_WithInvalidValue_ReturnsUnsetValue(int? currentIndex, int? lastIndex)
        {
            var converter = new LastItemToVisibilityConverter();

            object converted = _converter.Convert(
                [currentIndex, lastIndex],
                typeof(Visibility),
                null,
                Thread.CurrentThread.CurrentCulture);

            Assert.Equal(DependencyProperty.UnsetValue, converted);
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData(5, null)]
        [InlineData(null, 20)]
        public void LastItemToVisibilityConverter_WithMissingValue_ReturnsExpectedVisibility(int? currentIndex, int? lastIndex)
        {
            var converter = new LastItemToVisibilityConverter();

            object converted = _converter.Convert(
                [currentIndex, lastIndex],
                typeof(Visibility),
                null,
                Thread.CurrentThread.CurrentCulture);

            Assert.Equal(DependencyProperty.UnsetValue, converted);
        }
    }
}
