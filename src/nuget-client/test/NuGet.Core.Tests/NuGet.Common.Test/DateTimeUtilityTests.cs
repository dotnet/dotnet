// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using Xunit;

namespace NuGet.Common.Test
{
    public class DateTimeUtilityTests
    {
        public static IEnumerable<object[]> GetData()
        {
            return new[]
            {
                new object[] { "1.23", TimeSpan.FromSeconds(1.23d) },
                new object[] { "0", TimeSpan.FromMilliseconds(0.01d) }, // round down
                new object[] { "1", TimeSpan.FromMilliseconds(1.21d) }, // round down
                new object[] { "1", TimeSpan.FromMilliseconds(0.96d) }, // round up
                new object[] { "92183.91", TimeSpan.FromHours(92183.91d) },
                new object[] { "1", TimeSpan.FromSeconds(3600.0d) },
                new object[] { "3.6", TimeSpan.FromMinutes(3.6d) },
                new object[] { "72", TimeSpan.FromDays(3.0d) },
            };
        }

        [Theory]
        [MemberData(nameof(GetData))]
        public void ToReadableTimeFormat_Localized_AssumesCurrentCulture_ContainsTimeNumber_Succeeds(string timeNumber, TimeSpan time)
        {
            // Arrange
            // Convert expected string number to decimal and back to string in a local regional format
            string expected = decimal.Parse(timeNumber, CultureInfo.InvariantCulture).ToString(CultureInfo.CurrentCulture);

            // Act
            string actual = DatetimeUtility.ToReadableTimeFormat(time);

            // Assert
            Assert.Contains(expected, actual);
        }

        [Fact]
        public void ToReadableTimeFormat_Localized_AssumesSpainLocale_ContainsTimeNumber_Succeeds()
        {
            // Act
            string actual = DatetimeUtility.ToReadableTimeFormat(TimeSpan.FromMinutes(3.6d), new CultureInfo("es-ES"));

            // Assert
            Assert.Equal("3,6 min", actual);
        }
    }
}
