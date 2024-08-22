// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Globalization;
using Xunit;

namespace NuGet.PackageManagement.UI.Test
{
    public class ConverterTests
    {
        [Theory]
        [InlineData(1, "1")]
        [InlineData(90, "90")]
        [InlineData(451, "451")]
        [InlineData(550, "550")]
        [InlineData(998, "998")]
        [InlineData(999, "999")]
        [InlineData(1000, "1K")]
        [InlineData(1200, "1.2K")]
        [InlineData(1230, "1.23K")]

        // at most 3 significant digits
        [InlineData(1234, "1.23K")]

        // there is rounding
        [InlineData(1239, "1.24K")]
        [InlineData(9056, "9.06K")]
        [InlineData(19_056, "19.1K")]
        [InlineData(19_556, "19.6K")]
        [InlineData(99_056, "99.1K")]
        [InlineData(99_556, "99.6K")]

        [InlineData(123_400, "123K")]
        [InlineData(899_560, "900K")]
        [InlineData(998_999, "999K")]
        [InlineData(999_001, "999K")]
        [InlineData(999_560, "999K")]
        [InlineData(1_234_000, "1.23M")]
        [InlineData(1_299_560, "1.3M")]
        [InlineData(9_995_650, "10M")]
        [InlineData(12_340_000, "12.3M")]
        [InlineData(19_991_250, "20M")]
        [InlineData(19_999_850, "20M")]
        [InlineData(99_156_050, "99.2M")]
        [InlineData(99_999_650, "100M")]
        [InlineData(999_560_050, "999M")]
        [InlineData(999_999_050, "999M")]
        [InlineData(1_000_000_001, "1B")]
        [InlineData(1_234_000_000, "1.23B")]
        [InlineData(2_100_999_050, "2.1B")]
        [InlineData(9_234_000_000, "9.23B")]
        [InlineData(9_999_900_000, "10B")]
        [InlineData(99_199_199_650, "99.2B")]
        [InlineData(99_999_999_650, "100B")]
        [InlineData(999_999_999_650, "999B")]
        [InlineData(9_999_999_999_650, "10T")]
        [InlineData(19_999_999_999_050, "20T")]
        [InlineData(99_999_999_999_650, "100T")]

        // there is localization
        [InlineData(1939, "1.94K", "en-US")]
        [InlineData(1939, "1,94K", "da-DK")]
        [InlineData(9456, "9.46K", "en-US")]
        [InlineData(9456, "9,46K", "da-DK")]
        public void DownloadCountToStringTest(long num, string expected, string culture = null)
        {
            CultureInfo localCulture = string.IsNullOrWhiteSpace(culture) ? CultureInfo.InvariantCulture : new CultureInfo(culture); // Here CultureInfo.InvariantCulture forces '.' decimal separator
            var s = UIUtility.NumberToString(num, localCulture);
            Assert.Equal(expected, s);
        }
    }
}
