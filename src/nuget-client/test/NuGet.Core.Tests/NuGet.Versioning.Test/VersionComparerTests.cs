// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using Xunit;

namespace NuGet.Versioning.Test
{
    public class VersionComparerTests
    {
        [Theory]
        [InlineData("1.0.0", "1.0.0")]
        [InlineData("1.0.0-BETA", "1.0.0-beta")]
        [InlineData("1.0.0-BETA+AA", "1.0.0-beta+aa")]
        [InlineData("1.0.0-BETA.X.y.5.77.0+AA", "1.0.0-beta.x.y.5.77.0+aa")]
        [InlineData("1.0.0", "1.0.0+beta")]
        public void VersionComparisonDefaultEqual(string version1, string version2)
        {
            // Arrange & Act
            var match = Equals(VersionComparer.Default, version1, version2);

            // Assert
            Assert.True(match);
        }

        [Theory]
        [InlineData("1.0", "1.0.0.0")]
        [InlineData("1.0+test", "1.0.0.0")]
        [InlineData("1.0.0.1-1.2.A", "1.0.0.1-1.2.a+A")]
        [InlineData("1.0.01", "1.0.1.0")]
        public void VersionComparisonDefaultEqualWithNuGetVersion(string version1, string version2)
        {
            // Arrange & Act
            var match = EqualsWithNuGetVersion(VersionComparer.Default, version1, version2);

            // Assert
            Assert.True(match);
        }

        [Theory]
        [InlineData("1.0", "1.0.0.1")]
        [InlineData("1.0+test", "1.0.0.1")]
        [InlineData("1.0.0.1-1.2.A", "1.0.0.1-1.2.a.A+A")]
        [InlineData("1.0.01", "1.0.1.2")]
        public void VersionComparisonDefaultNotEqualWithNuGetVersion(string version1, string version2)
        {
            // Arrange & Act
            var match = EqualsWithNuGetVersion(VersionComparer.Default, version1, version2);

            // Assert
            Assert.False(match);
        }

        [Theory]
        [InlineData("0.0.0", "1.0.0")]
        [InlineData("1.1.0", "1.0.0")]
        [InlineData("1.0.1", "1.0.0")]
        [InlineData("1.0.0-BETA", "1.0.0-beta2")]
        [InlineData("1.0.0+AA", "1.0.0-beta+aa")]
        [InlineData("1.0.0-BETA+AA", "1.0.0-beta")]
        [InlineData("1.0.0-BETA.X.y.5.77.0+AA", "1.0.0-beta.x.y.5.79.0+aa")]
        public void VersionComparisonDefaultNotEqual(string version1, string version2)
        {
            // Arrange & Act
            var match = !Equals(version1, version2);

            // Assert
            Assert.True(match);
        }

        [Theory]
        [InlineData("0.0.0", "1.0.0")]
        [InlineData("1.0.0", "1.1.0")]
        [InlineData("1.0.0", "1.0.1")]
        [InlineData("1.999.9999", "2.1.1")]
        [InlineData("1.0.0-BETA", "1.0.0-beta2")]
        [InlineData("1.0.0-beta+AA", "1.0.0+aa")]
        [InlineData("1.0.0-BETA", "1.0.0-beta.1+AA")]
        [InlineData("1.0.0-BETA.X.y.5.77.0+AA", "1.0.0-beta.x.y.5.79.0+aa")]
        [InlineData("1.0.0-BETA.X.y.5.79.0+AA", "1.0.0-beta.x.y.5.790.0+abc")]
        public void VersionComparisonDefaultLess(string version1, string version2)
        {
            // Arrange & Act
            var result = Compare(VersionComparer.Default, version1, version2);

            // Assert
            Assert.True(result < 0);
        }

        private static int Compare(IVersionComparer comparer, string version1, string version2)
        {
            // Act
            var x = CompareOneWay(comparer, version1, version2);
            var y = CompareOneWay(comparer, version2, version1) * -1;

            // Assert
            Assert.Equal(x, y);

            return x;
        }

        private static int CompareOneWay(IVersionComparer comparer, string version1, string version2)
        {
            // Arrange
            var a = NuGetVersion.Parse(version1);
            var b = NuGetVersion.Parse(version2);
            var c = SemanticVersion.Parse(version1);
            var d = SemanticVersion.Parse(version2);

            // Act
            var results = new List<int>();
            results.Add(comparer.Compare(a, b));
            results.Add(comparer.Compare(a, d));
            results.Add(comparer.Compare(c, b));
            results.Add(comparer.Compare(c, d));

            // Assert
            Assert.True(results.FindAll(x => x == results[0]).Count == results.Count);

            return results[0];
        }

        private static bool Equals(IVersionComparer comparer, string version1, string version2)
        {
            return EqualsOneWay(comparer, version1, version2) && EqualsOneWay(comparer, version2, version1);
        }

        private static bool EqualsOneWay(IVersionComparer comparer, string version1, string version2)
        {
            // Arrange
            var a = NuGetVersion.Parse(version1);
            var b = NuGetVersion.Parse(version2);
            SemanticVersion c = NuGetVersion.Parse(version1);
            SemanticVersion d = NuGetVersion.Parse(version2);

            // Act
            var match = Compare(comparer, version1, version2) == 0;
            match &= comparer.Equals(a, b);
            match &= comparer.Equals(a, d);
            match &= comparer.Equals(c, d);
            match &= comparer.Equals(c, b);
            match &= comparer.GetHashCode(a) == comparer.GetHashCode(b);
            match &= comparer.GetHashCode(a) == comparer.GetHashCode(d);
            match &= comparer.GetHashCode(c) == comparer.GetHashCode(d);
            match &= comparer.GetHashCode(c) == comparer.GetHashCode(b);

            return match;
        }

        private static bool EqualsWithNuGetVersion(IVersionComparer comparer, string version1, string version2)
        {
            return EqualsOneWayWithNuGetVersion(comparer, version1, version2) && EqualsOneWayWithNuGetVersion(comparer, version2, version1);
        }

        private static bool EqualsOneWayWithNuGetVersion(IVersionComparer comparer, string version1, string version2)
        {
            // Arrange
            var a = NuGetVersion.Parse(version1);
            var b = NuGetVersion.Parse(version2);

            // Act
            var match = comparer.Compare(a, b) == 0;
            match &= comparer.Equals(a, b);
            match &= comparer.GetHashCode(a) == comparer.GetHashCode(b);

            return match;
        }
    }
}
