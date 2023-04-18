// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace NuGet.Versioning.Test
{
    public class VersionExtensionsTests
    {
        [Fact]
        public void FindBestMatch_TwoChoicesButOnlyOneInRange_SelectsVersionThatSatisfiesRange()
        {
            // Arrange
            List<Tuple<bool, NuGetVersion>> available = new()
            {
                new(true, NuGetVersion.Parse("1.0.0")),
                new(true, NuGetVersion.Parse("2.0.0"))
            };
            VersionRange range = VersionRange.Parse("[2.0.0, )");

            // Act
            Tuple<bool, NuGetVersion>? result = VersionExtensions.FindBestMatch(available, range, t => t.Item2);

            // Assert
            result.Should().NotBeNull();
            result!.Item2.ToNormalizedString().Should().Be("2.0.0");
        }

        [Fact]
        public void FindBestMatch_NoVersionSatisfiesRange_ReturnsNull()
        {
            // Arrange
            List<Tuple<bool, NuGetVersion>> available = new()
            {
                new(true, NuGetVersion.Parse("1.0.0")),
            };
            VersionRange range = VersionRange.Parse("[2.0.0, )");

            // Act
            Tuple<bool, NuGetVersion>? result = VersionExtensions.FindBestMatch(available, range, t => t.Item2);

            // Assert
            result.Should().BeNull();
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void FindBestMatch_FeedWithPrereleaseVersions_PrereleaseUsedOnlyIfRangeAllows(bool usePrerelease)
        {
            List<NuGetVersion> available = new()
            {
                NuGetVersion.Parse("1.0.1-beta.1"),
                NuGetVersion.Parse("1.0.1")
            };
            string rangeString = usePrerelease ? "1.0.0-*" : "1.0.0";
            VersionRange range = VersionRange.Parse(rangeString);

            // Act
            NuGetVersion? result = available.FindBestMatch(range, v => v);

            // Assert
            result.Should().NotBeNull();
            if (usePrerelease)
            {
                result!.OriginalVersion.Should().Be("1.0.1-beta.1");
            }
            else
            {
                result!.OriginalVersion.Should().Be("1.0.1");
            }
        }
    }
}
