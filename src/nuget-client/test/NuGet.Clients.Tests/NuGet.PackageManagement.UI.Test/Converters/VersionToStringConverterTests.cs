// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading;
using NuGet.Versioning;
using Xunit;

namespace NuGet.PackageManagement.UI.Test.Converters
{
    public class VersionToStringConverterTests
    {
        [Fact]
        public void Convert_WithoutConverterParameter_DefaultsToNormalizedVersionString()
        {
            var value = new NuGetVersion("1.0.0.0");
            var converter = new VersionToStringConverter();

            var converted = converter.Convert(
                value,
                typeof(string),
                null,
                Thread.CurrentThread.CurrentCulture);

            Assert.Equal(value.ToNormalizedString(), converted);
        }

        [Theory]
        [InlineData("V")]
        [InlineData("F")]
        [InlineData("N")]
        public void Convert_WithConverterParameter_UsesVersionFormatter(string converterParameter)
        {
            var value = new NuGetVersion("1.0.0+metadata");
            var converter = new VersionToStringConverter();

            var converted = converter.Convert(
                value,
                typeof(string),
                converterParameter,
                Thread.CurrentThread.CurrentCulture);

            Assert.Equal(value.ToString(converterParameter, VersionFormatter.Instance), converted);
        }
    }
}
