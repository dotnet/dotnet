// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using FluentAssertions;
using Xunit;

namespace NuGet.Versioning.Test
{
    [UseCulture("en-US")] // We are asserting exception messages in English
    public class NuGetVersionTest
    {
        [Fact]
        public void NuGetVersionConstructors()
        {
            // Arrange
            var versions = new HashSet<SemanticVersion>(VersionComparer.Default);

            // act
            versions.Add(new NuGetVersion("4.3.0"));
            versions.Add(new NuGetVersion(NuGetVersion.Parse("4.3.0")));
            versions.Add(new NuGetVersion(new Version(4, 3, 0)));
            versions.Add(new NuGetVersion(new Version(4, 3, 0), string.Empty, string.Empty));
            versions.Add(new NuGetVersion(4, 3, 0));
            versions.Add(new NuGetVersion(4, 3, 0, string.Empty));
            versions.Add(new NuGetVersion(4, 3, 0, null));
            versions.Add(new NuGetVersion(4, 3, 0, 0));
            versions.Add(new NuGetVersion(new Version(4, 3, 0), Array.Empty<string>(), string.Empty, "4.3"));

            versions.Add(new SemanticVersion(4, 3, 0));
            versions.Add(new SemanticVersion(4, 3, 0, string.Empty));
            versions.Add(new SemanticVersion(4, 3, 0, null));

            // Assert
            Assert.Equal<int>(1, versions.Count);
        }

        [Theory]
        [InlineData("1.0.0")]
        [InlineData("0.0.1")]
        [InlineData("1.2.3")]
        [InlineData("1.2.3-alpha")]
        [InlineData("1.2.3-X.y.3+Meta-2")]
        [InlineData("1.2.3-X.yZ.3.234.243.3242342+METADATA")]
        [InlineData("1.2.3-X.y3+0")]
        [InlineData("1.2.3-X+0")]
        [InlineData("1.2.3+0")]
        [InlineData("1.2.3-0")]
        public void NuGetVersionParseStrict(string versionString)
        {
            // Arrange
            NuGetVersion? semVer;
            bool successful = NuGetVersion.TryParseStrict(versionString, out semVer);

            // Assert
            Assert.True(successful);
            Assert.Equal<string>(versionString, semVer!.ToFullString());
            Assert.Equal<string>(semVer.ToNormalizedString(), semVer.ToString());
        }

        [Theory]
        [InlineData("1.0.0", "1.0.0.0", "", "")]
        [InlineData("2.3-alpha", "2.3.0.0", "alpha", "")]
        [InlineData("3.4.0.3-RC-3", "3.4.0.3", "RC-3", "")]
        [InlineData("1.0.0-beta.x.y.5.79.0+aa", "1.0.0.0", "beta.x.y.5.79.0", "aa")]
        [InlineData("1.0.0-beta.x.y.5.79.0+AA", "1.0.0.0", "beta.x.y.5.79.0", "AA")]
        public void StringConstructorParsesValuesCorrectly(string version, string versionValueString, string specialValue, string metadata)
        {
            // Arrange
            var versionValue = new Version(versionValueString);

            // Act
            var semanticVersion = NuGetVersion.Parse(version);

            // Assert
            Assert.Equal(versionValue, semanticVersion.Version);
            Assert.Equal(specialValue, semanticVersion.Release);
            Assert.Equal(metadata, semanticVersion.Metadata);
        }

        [Fact]
        public void ParseThrowsIfStringIsNullOrEmpty()
        {
            ExceptionAssert.ThrowsArgNullOrEmpty(() => NuGetVersion.Parse(null!), "value");
            ExceptionAssert.ThrowsArgNullOrEmpty(() => NuGetVersion.Parse(string.Empty), "value");
        }

        [Theory]
        [InlineData("         ")]
        [InlineData("1beta")]
        [InlineData("1.2Av^c")]
        [InlineData("1.2..")]
        [InlineData("1.2.3.4.5")]
        [InlineData("1.2.3.Beta")]
        [InlineData("1.2.3.4This version is full of awesomeness!!")]
        [InlineData("So.is.this")]
        [InlineData("1.34.2Alpha")]
        [InlineData("1.34.2Release Candidate")]
        [InlineData("1.4.7-")]
        [InlineData("1.4.7-*")]
        [InlineData("1.4.7+*")]
        [InlineData("1.4.7-AA.01^")]
        [InlineData("1.4.7-AA.0A^")]
        [InlineData("1.4.7-A^A")]
        [InlineData("1.4.7+AA.01^")]
        [InlineData("1.2147483648")]
        [InlineData("1.1.2147483648")]
        [InlineData("1.1.1.2147483648")]
        [InlineData("1.1.1.1.2147483648")]
        [InlineData("10000000000000000000")]
        [InlineData("1.10000000000000000000")]
        [InlineData("1.1.10000000000000000000")]
        [InlineData("1.1.1.1.10000000000000000000")]
        [InlineData("1..2")]
        [InlineData("....")]
        [InlineData("..1")]
        [InlineData("-1.1.1.1")]
        [InlineData("1.-1.1.1")]
        [InlineData("1.1.-1.1")]
        [InlineData("1.1.1.-1")]
        [InlineData("1.")]
        [InlineData("1.1.")]
        [InlineData("1.1.1.")]
        [InlineData("1.1.1.1.")]
        [InlineData("1.1.1.1.1.")]
        [InlineData("1     1.1.1.1")]
        [InlineData("1.1     1.1.1")]
        [InlineData("1.1.1     1.1")]
        [InlineData("1.1.1.1     1")]
        [InlineData(" .1.1.1")]
        [InlineData("1. .1.1")]
        [InlineData("1.1. .1")]
        [InlineData("1.1.1. ")]
        [InlineData("1 .")]
        [InlineData("1.1 .")]
        [InlineData("1.1.1 .")]
        [InlineData("1.1.1.1 .")]
        [InlineData("2147483648.2.3.4")]
        [InlineData("1.2147483648.3.4")]
        [InlineData("1.2.2147483648.4")]
        [InlineData("1.2.3.2147483648")]
        [InlineData("..1.2")]
        [InlineData("-1.2.3.4")]
        [InlineData("1.-2.3.4")]
        [InlineData("1.2.-3.4")]
        [InlineData("1.2.3.-4")]
        [InlineData("   1 9")]
        [InlineData("   19.   1 9")]
        [InlineData("   19.   19.   1 9")]
        [InlineData("   19.   19.   19.   1 9")]
        [InlineData("1 9   ")]
        [InlineData("19   .1 9   ")]
        [InlineData("19   .19   .1 9   ")]
        [InlineData("19   .19   .19   .1 9   ")]
        [InlineData("   1 9   ")]
        [InlineData("   19   .   1 9   ")]
        [InlineData("   19   .   19   .   1 9   ")]
        [InlineData("   19   .   19   .   19   .   1 9   ")]
        public void ParseThrowsIfStringIsNotAValidSemVer(string versionString)
        {
            ExceptionAssert.ThrowsArgumentException(() => NuGetVersion.Parse(versionString),
                "value",
                string.Format(CultureInfo.InvariantCulture, "'{0}' is not a valid version string.", versionString));
        }

        [Theory]
        [InlineData("1.022", "1.22.0.0")]
        [InlineData("23.2.3", "23.2.3.0")]
        [InlineData("1.3.42.10133", "1.3.42.10133")]
        public void ParseReadsLegacyStyleVersionNumbers(string versionString, string expectedString)
        {
            // Arrange
            var expected = new NuGetVersion(new Version(expectedString), "");

            // Act
            var actual = NuGetVersion.Parse(versionString);

            // Assert
            Assert.Equal(expected.Version, actual.Version);
            Assert.Equal(expected.Release, actual.Release);
        }

        [Theory]
        [InlineData("1.022-Beta", "1.22.0.0", "Beta")]
        [InlineData("23.2.3-Alpha", "23.2.3.0", "Alpha")]
        [InlineData("1.3.42.10133-PreRelease", "1.3.42.10133", "PreRelease")]
        [InlineData("1.3.42.200930-RC-2", "1.3.42.200930", "RC-2")]
        public void ParseReadsSemverAndHybridSemverVersionNumbers(string versionString, string expectedString, string releaseString)
        {
            // Arrange
            var expected = new NuGetVersion(new Version(expectedString), releaseString);

            // Act
            var actual = NuGetVersion.Parse(versionString);

            // Assert
            Assert.Equal(expected.Version, actual.Version);
            Assert.Equal(expected.Release, actual.Release);
        }

        [Theory]
        [InlineData("  1.022-Beta", "1.22.0.0", "Beta")]
        [InlineData("23.2.3-Alpha  ", "23.2.3.0", "Alpha")]
        [InlineData("    1.3.42.10133-PreRelease  ", "1.3.42.10133", "PreRelease")]
        public void ParseIgnoresLeadingAndTrailingWhitespace(string versionString, string expectedString, string releaseString)
        {
            // Arrange
            var expected = new NuGetVersion(new Version(expectedString), releaseString);

            // Act
            var actual = NuGetVersion.Parse(versionString);

            // Assert
            Assert.Equal(expected.Version, actual.Version);
            Assert.Equal(expected.Release, actual.Release);
        }

        [Theory]
        [InlineData("1.0", "1.0.1")]
        [InlineData("1.23", "1.231")]
        [InlineData("1.4.5.6", "1.45.6")]
        [InlineData("1.4.5.6", "1.4.5.60")]
        [InlineData("1.01", "1.10")]
        [InlineData("1.01-alpha", "1.10-beta")]
        [InlineData("1.01.0-RC-1", "1.10.0-rc-2")]
        [InlineData("1.01-RC-1", "1.01")]
        [InlineData("1.01", "1.2-preview")]
        public void SemVerLessThanAndGreaterThanOperatorsWorks(string versionA, string versionB)
        {
            // Arrange
            var itemA = NuGetVersion.Parse(versionA);
            var itemB = NuGetVersion.Parse(versionB);
            object objectB = itemB;

            // Act and Assert
            Assert.True(itemA < itemB);
            Assert.True(itemA <= itemB);
            Assert.True(itemB > itemA);
            Assert.True(itemB >= itemA);
            Assert.False(itemA.Equals(itemB));
            Assert.False(itemA.Equals(objectB));
        }

        [Theory]
        [InlineData(new object[] { 1 })]
        [InlineData(new object[] { "1.0.0" })]
        [InlineData(new object[] { new object[0] })]
        public void EqualsReturnsFalseIfComparingANonSemVerType(object other)
        {
            // Arrange
            var semVer = NuGetVersion.Parse("1.0.0");

            // Act and Assert
            Assert.False(semVer.Equals(other));
        }

        [Fact]
        public void EqualsIsTrueForEmptyRevision()
        {
            NuGetVersion.Parse("1.0.0.0").Equals(SemanticVersion.Parse("1.0.0")).Should().BeTrue();
            SemanticVersion.Parse("1.0.0").Equals(NuGetVersion.Parse("1.0.0.0")).Should().BeTrue();
        }

        [Theory]
        [InlineData("1.0", "1.0.0.0")]
        [InlineData("1.23.01", "1.23.1")]
        [InlineData("1.45.6", "1.45.6.0")]
        [InlineData("1.45.6-Alpha", "1.45.6-Alpha")]
        [InlineData("1.6.2-BeTa", "1.6.02-beta")]
        [InlineData("22.3.07     ", "22.3.07")]
        [InlineData("1.0", "1.0.0.0+beta")]
        [InlineData("1.0.0.0+beta.2", "1.0.0.0+beta.1")]
        public void SemVerEqualsOperatorWorks(string versionA, string versionB)
        {
            // Arrange
            var itemA = NuGetVersion.Parse(versionA);
            var itemB = NuGetVersion.Parse(versionB);
            object objectB = itemB;

            // Act and Assert
            Assert.True(itemA == itemB);
            Assert.True(itemA.Equals(itemB));
            Assert.True(itemA.Equals(objectB));
            Assert.True(itemA <= itemB);
            Assert.True(itemB == itemA);
            Assert.True(itemB >= itemA);
        }

        [Fact]
        public void SemVerEqualityComparisonsWorkForNullValues()
        {
            // Arrange
            NuGetVersion? itemA = null;
            NuGetVersion? itemB = null;

            // Act and Assert
            Assert.True(itemA == itemB);
            Assert.True(itemB == itemA);
            Assert.True(itemA! <= itemB!);
            Assert.True(itemB! <= itemA!);
            Assert.True(itemA! >= itemB!);
            Assert.True(itemB! >= itemA!);
        }

        [Theory]
        [InlineData("1.0")]
        [InlineData("1.0.0")]
        [InlineData("1.0.0.0")]
        [InlineData("1.0-alpha")]
        [InlineData("1.0.0-b")]
        [InlineData("3.0.1.2")]
        [InlineData("2.1.4.3-pre-1")]
        public void ToStringReturnsOriginalValueForNonSemVer2(string version)
        {
            // Act
            var semVer = NuGetVersion.Parse(version);

            // Assert
            Assert.Equal(version, semVer.ToString());
        }

        [Theory]
        [InlineData("1.0+A", "1.0.0", "1.0.0+A")]
        [InlineData("1.0-1.1", "1.0.0-1.1", "1.0.0-1.1")]
        [InlineData("1.0-1.1+B.B", "1.0.0-1.1", "1.0.0-1.1+B.B")]
        [InlineData("1.0.0009.01-1.1+A", "1.0.9.1-1.1", "1.0.9.1-1.1+A")]
        public void ToStringReturnsNormalizedForSemVer2(string version, string expected, string full)
        {
            // Act
            var semVer = NuGetVersion.Parse(version);

            // Assert
            Assert.Equal(expected, semVer.ToString());
            Assert.Equal(expected, semVer.ToNormalizedString());
            Assert.Equal(full, semVer.ToFullString());
        }

        [Theory]
        [InlineData("1.0", null, "1.0")]
        [InlineData("1.0.3.120", "", "1.0.3.120")]
        [InlineData("1.0.3.120", "alpha", "1.0.3.120-alpha")]
        [InlineData("1.0.3.120", "rc-2", "1.0.3.120-rc-2")]
        public void ToStringConstructedFromVersionAndSpecialVersionConstructor(string versionString, string specialVersion, string expected)
        {
            // Arrange 
            var version = new Version(versionString);

            // Act
            var semVer = new NuGetVersion(version, specialVersion);

            // Assert
            Assert.Equal(expected, semVer.ToString());
        }

        [Theory]
        [InlineData("01.42.0")]
        [InlineData("01.0")]
        [InlineData("01.42.0-alpha")]
        [InlineData("01.42.0-alpha.1")]
        [InlineData("01.42.0-alpha+metadata")]
        [InlineData("01.42.0+metadata")]
        public void NuGetVersionKeepsOriginalVersionString(string originalVersion)
        {
            var version = new NuGetVersion(originalVersion);

            Assert.Equal(originalVersion, version.OriginalVersion);
        }

        [Theory]
        [InlineData("1.0", null, "1.0")]
        [InlineData("1.0.3.120", "", "1.0.3.120")]
        [InlineData("1.0.3.120", "alpha", "1.0.3.120-alpha")]
        [InlineData("1.0.3.120", "rc-2", "1.0.3.120-rc-2")]
        public void ToStringFromStringFormat(string versionString, string specialVersion, string expected)
        {
            // Arrange 
            var version = new Version(versionString);

            // Act
            var semVer = new NuGetVersion(version, specialVersion);

            // Assert
            Assert.Equal(expected, semVer.ToString());
        }

        [Fact]
        public void TryParseStrictParsesStrictVersion()
        {
            // Arrange
            var versionString = "1.3.2-CTP-2-Refresh-Alpha";

            // Act
            NuGetVersion? version;
            var result = NuGetVersion.TryParseStrict(versionString, out version);

            // Assert
            Assert.True(result);
            Assert.Equal(new Version("1.3.2.0"), version!.Version);
            Assert.Equal("CTP-2-Refresh-Alpha", version.Release);
        }

        [Theory]
        [InlineData("")]
        [InlineData("NotAVersion")]
        [InlineData("1")]
        [InlineData("1.0")]
        [InlineData("v1.0.0")]
        [InlineData("1.0.3.120")]
        public void TryParseReturnsFalseWhenUnableToParseString(string versionString)
        {
            // Act
            NuGetVersion? version;
            var result = NuGetVersion.TryParseStrict(versionString, out version);

            // Assert
            Assert.False(result);
            Assert.Null(version);
        }

        [Theory]
        [InlineData("   19", 19, 0, 0, 0)]
        [InlineData("   19.   19", 19, 19, 0, 0)]
        [InlineData("   19.   19.   19", 19, 19, 19, 0)]
        [InlineData("   19.   19.   19.   19", 19, 19, 19, 19)]
        [InlineData("19   ", 19, 0, 0, 0)]
        [InlineData("19   .19   ", 19, 19, 0, 0)]
        [InlineData("19   .19   .19   ", 19, 19, 19, 0)]
        [InlineData("19   .19   .19   .19   ", 19, 19, 19, 19)]
        [InlineData("   19   ", 19, 0, 0, 0)]
        [InlineData("   19   .   19   ", 19, 19, 0, 0)]
        [InlineData("   19   .   19   .   19   ", 19, 19, 19, 0)]
        [InlineData("   19   .   19   .   19   .   19   ", 19, 19, 19, 19)]
        [InlineData("01.1.1.1", 1, 1, 1, 1)]
        [InlineData("1.01.1.1", 1, 1, 1, 1)]
        [InlineData("1.1.01.1", 1, 1, 1, 1)]
        [InlineData("1.1.1.01", 1, 1, 1, 1)]
        [InlineData("2147483647.1.1.1", 2147483647, 1, 1, 1)]
        [InlineData("1.2147483647.1.1", 1, 2147483647, 1, 1)]
        [InlineData("1.1.2147483647.1", 1, 1, 2147483647, 1)]
        [InlineData("1.1.1.2147483647", 1, 1, 1, 2147483647)]
        public void TryParseHandlesValidVersionPatterns(string versionString, int major = 0, int minor = 0, int patch = 0, int revision = 0)
        {
            Assert.True(NuGetVersion.TryParse(versionString, out var version));
            Assert.NotNull(version);
            Assert.Equal(major, version.Major);
            Assert.Equal(minor, version.Minor);
            Assert.Equal(patch, version.Patch);
            Assert.Equal(revision, version.Revision);
        }
    }
}
