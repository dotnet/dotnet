// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using FluentAssertions;
using NuGet.Versioning;
using Xunit;

namespace NuGet.Commands.Test
{
    public class SdkAnalysisLevelMinimumsTests
    {
        [Fact]
        public void IsEnabled_WhenSdkAnalysisLevelIsNullAndUsingMicrosoftNetSdkIsFalse_ShouldReturnTrue()
        {
            var result = SdkAnalysisLevelMinimums.IsEnabled(null, false, new NuGetVersion("9.0.100"));
            result.Should().BeTrue();
        }

        [Fact]
        public void IsEnabled_WhenSdkAnalysisLevelIsNullAndUsingMicrosoftNetSdkIsTrue_ShouldReturnFalse()
        {
            var result = SdkAnalysisLevelMinimums.IsEnabled(null, true, new NuGetVersion("9.0.100"));
            result.Should().BeFalse();
        }

        [Fact]
        public void IsEnabled_WhenSdkAnalysisLevelIsLessThanMinSdkVersion_ShouldReturnFalse()
        {
            var result = SdkAnalysisLevelMinimums.IsEnabled(new NuGetVersion("8.0.900"), true, new NuGetVersion("9.0.100"));
            result.Should().BeFalse();
        }

        [Fact]
        public void IsEnabled_WhenSdkAnalysisLevelIsEqualToMinSdkVersion_ShouldReturnTrue()
        {
            var result = SdkAnalysisLevelMinimums.IsEnabled(new NuGetVersion("9.0.100"), true, new NuGetVersion("9.0.100"));
            result.Should().BeTrue();
        }

        [Fact]
        public void IsEnabled_WhenSdkAnalysisLevelIsGreaterThanMinSdkVersion_ShouldReturnTrue()
        {
            var result = SdkAnalysisLevelMinimums.IsEnabled(new NuGetVersion("9.0.101"), true, new NuGetVersion("9.0.100"));
            result.Should().BeTrue();
        }
    }
}
