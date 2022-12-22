// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace NuGet.Frameworks
{
    public class FrameworkRangeTests
    {
        [Theory]
        [InlineData("net45")]
        [InlineData("net20")]
        [InlineData("net")]
        [InlineData("net451")]
        [InlineData("net40")]
        public void FrameworkRange_BasicSatisfies(string framework)
        {
            // Arrange
            var test = NuGetFramework.ParseFolder(framework);
            var range = new FrameworkRange(NuGetFramework.ParseFolder("net"), NuGetFramework.ParseFolder("net451"));

            // Act & Assert
            Assert.True(range.Satisfies(test));
        }

        [Theory]
        [InlineData("dotnet")]
        [InlineData("dnx452")]
        [InlineData("net452")]
        [InlineData("net20")]
        [InlineData("net")]
        public void FrameworkRange_BasicDoesNotSatisfy(string framework)
        {
            // Arrange
            var test = NuGetFramework.ParseFolder(framework);
            var range = new FrameworkRange(NuGetFramework.ParseFolder("net35"), NuGetFramework.ParseFolder("net451"));

            // Act & Assert
            Assert.False(range.Satisfies(test));
        }

        [Theory]
        [InlineData("dotnet")]
        [InlineData("dnx452")]
        [InlineData("dnx451")]
        [InlineData("net452")]
        [InlineData("net35")]
        [InlineData("net20")]
        [InlineData("net")]
        public void FrameworkRange_BasicDoesNotSatisfyExclusive(string framework)
        {
            // Arrange
            var test = NuGetFramework.ParseFolder(framework);
            var range = new FrameworkRange(
                NuGetFramework.ParseFolder("net35"),
                NuGetFramework.ParseFolder("net451"),
                includeMin: false,
                includeMax: false);

            // Act & Assert
            Assert.False(range.Satisfies(test));
        }

        [Theory]
        [InlineData("net45")]
        [InlineData("net36")]
        [InlineData("net40")]
        public void FrameworkRange_BasicSatisfiesExclusive(string framework)
        {
            // Arrange
            var test = NuGetFramework.ParseFolder(framework);
            var range = new FrameworkRange(
                NuGetFramework.ParseFolder("net35"),
                NuGetFramework.ParseFolder("net451"),
                includeMin: false,
                includeMax: false);

            // Act & Assert
            Assert.True(range.Satisfies(test));
        }

        [Fact]
        public void FrameworkRange_HashCodeDiffersOnExlusiveness()
        {
            // Arrange
            var range1 = new FrameworkRange(
                NuGetFramework.ParseFolder("net35"),
                NuGetFramework.ParseFolder("net451"),
                includeMin: false,
                includeMax: false);

            var range2 = new FrameworkRange(
                NuGetFramework.ParseFolder("net35"),
                NuGetFramework.ParseFolder("net451"),
                includeMin: true,
                includeMax: false);

            var range3 = new FrameworkRange(
                NuGetFramework.ParseFolder("net35"),
                NuGetFramework.ParseFolder("net451"),
                includeMin: true,
                includeMax: true);

            var range4 = new FrameworkRange(
                NuGetFramework.ParseFolder("net35"),
                NuGetFramework.ParseFolder("net451"),
                includeMin: false,
                includeMax: true);

            // Act
            // Find the unique set of hash codes
            var hashCodes = new HashSet<int>()
            {
                range1.GetHashCode(),
                range2.GetHashCode(),
                range3.GetHashCode(),
                range4.GetHashCode(),
            };

            // Assert
            Assert.Equal(4, hashCodes.Count);
            Assert.NotEqual(range1, range2);
            Assert.NotEqual(range1, range3);
            Assert.NotEqual(range1, range4);
            Assert.NotEqual(range2, range4);
            Assert.NotEqual(range2, range3);
            Assert.NotEqual(range3, range4);
        }
    }
}
