// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Xunit;

namespace NuGet.Protocol.Plugins.Tests
{
    public class TimeoutUtilitiesTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("a")]
        [InlineData("0")]
        [InlineData("-1")]
        [InlineData("4294967295")]
        public void GetTimeout_ReturnsFallbackForInvalidValues(string timeoutInSeconds)
        {
            var fallbackTimeout = new TimeSpan(days: 0, hours: 1, minutes: 2, seconds: 3, milliseconds: 4);

            var actualTimeout = TimeoutUtilities.GetTimeout(timeoutInSeconds, fallbackTimeout);

            Assert.Equal(fallbackTimeout, actualTimeout);
        }

        [Fact]
        public void GetTimeout_ThrowsForZeroFallback()
        {
            var exception = Assert.Throws<ArgumentOutOfRangeException>(
                () => TimeoutUtilities.GetTimeout("1", TimeSpan.Zero));

            Assert.Equal("fallbackTimeout", exception.ParamName);
        }

        [Fact]
        public void GetTimeout_ThrowsForNegativeFallback()
        {
            var exception = Assert.Throws<ArgumentOutOfRangeException>(
                () => TimeoutUtilities.GetTimeout("1", TimeSpan.MinValue));

            Assert.Equal("fallbackTimeout", exception.ParamName);
        }

        [Fact]
        public void GetTimeout_ThrowsForTooLargeFallback()
        {
            var exception = Assert.Throws<ArgumentOutOfRangeException>(
                () => TimeoutUtilities.GetTimeout("1", TimeSpan.MaxValue));

            Assert.Equal("fallbackTimeout", exception.ParamName);
        }

        [Fact]
        public void GetTimeout_ReturnsTimeout()
        {
            var actualTimeout = TimeoutUtilities.GetTimeout("123", ProtocolConstants.MaxTimeout);

            Assert.Equal(TimeSpan.FromSeconds(123), actualTimeout);
        }

        [Fact]
        public void IsValid_ReturnsFalseForTimeSpanZero()
        {
            Assert.False(TimeoutUtilities.IsValid(TimeSpan.Zero));
        }

        [Fact]
        public void IsValid_ReturnsFalseForNegativeTimeSpan()
        {
            Assert.False(TimeoutUtilities.IsValid(TimeSpan.FromSeconds(-1)));
        }

        [Fact]
        public void IsValid_ReturnsFalseForTooLargeTimeSpan()
        {
            var milliseconds = int.MaxValue + 1L;

            Assert.False(TimeoutUtilities.IsValid(TimeSpan.FromMilliseconds(milliseconds)));
        }

        [Fact]
        public void IsValid_ReturnsTrueForMinimumAcceptableTimeSpan()
        {
            Assert.True(TimeoutUtilities.IsValid(ProtocolConstants.MinTimeout));
        }

        [Fact]
        public void IsValid_ReturnsTrueForMaximumAcceptableTimeSpan()
        {
            Assert.True(TimeoutUtilities.IsValid(ProtocolConstants.MaxTimeout));
        }
    }
}
