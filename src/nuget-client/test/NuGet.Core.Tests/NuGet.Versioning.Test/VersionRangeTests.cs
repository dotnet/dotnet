// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using Xunit;

namespace NuGet.Versioning.Test
{
    public class VersionRangeTests
    {
        [Theory]
        [InlineData("1.0.0", "(>= 1.0.0)")]
        [InlineData("[1.0.0]", "(= 1.0.0)")]
        [InlineData("[1.0.0, ]", "(>= 1.0.0)")]
        [InlineData("[1.0.0, )", "(>= 1.0.0)")]
        [InlineData("(1.0.0, )", "(> 1.0.0)")]
        [InlineData("(1.0.0, ]", "(> 1.0.0)")]
        [InlineData("(1.0.0, 2.0.0)", "(> 1.0.0 && < 2.0.0)")]
        [InlineData("[1.0.0, 2.0.0]", "(>= 1.0.0 && <= 2.0.0)")]
        [InlineData("[1.0.0, 2.0.0)", "(>= 1.0.0 && < 2.0.0)")]
        [InlineData("(1.0.0, 2.0.0]", "(> 1.0.0 && <= 2.0.0)")]
        [InlineData("(, 2.0.0]", "(<= 2.0.0)")]
        [InlineData("(, 2.0.0)", "(< 2.0.0)")]
        [InlineData("[, 2.0.0)", "(< 2.0.0)")]
        [InlineData("[, 2.0.0]", "(<= 2.0.0)")]
        [InlineData("1.0.0-beta*", "(>= 1.0.0-beta)")]
        [InlineData("[1.0.0-beta*, 2.0.0)", "(>= 1.0.0-beta && < 2.0.0)")]
        [InlineData("[1.0.0-beta.1, 2.0.0-alpha.2]", "(>= 1.0.0-beta.1 && <= 2.0.0-alpha.2)")]
        [InlineData("[1.0.0+beta.1, 2.0.0+alpha.2]", "(>= 1.0.0 && <= 2.0.0)")]
        [InlineData("[1.0, 2.0]", "(>= 1.0.0 && <= 2.0.0)")]
        public void VersionRange_PrettyPrintTests(string versionString, string expected)
        {
            // Arrange
            var formatter = new VersionRangeFormatter();
            var range = VersionRange.Parse(versionString);

            // Act
            var s = string.Format(formatter, "{0:P}", range);
            var s2 = range.ToString("P", formatter);
            var s3 = range.PrettyPrint();

            // Assert
            Assert.Equal(expected, s);
            Assert.Equal(expected, s2);
            Assert.Equal(expected, s3);
        }

        [Theory]
        [InlineData("1.0.0", false)]
        [InlineData("1.*", false)]
        [InlineData("*", false)]
        [InlineData("[*, )", true)]
        [InlineData("[1.*, ]", false)]
        [InlineData("[1.*, 2.0.0)", true)]
        [InlineData("(, )", true)]
        public void VersionRange_NormalizationRoundTripsTest(string versionString, bool isOriginalStringNormalized)
        {
            // Arrange
            var originalParsedRange = VersionRange.Parse(versionString);
            // Act
            var normalizedRangeRepresentation = originalParsedRange.ToNormalizedString();

            var roundTrippedRange = VersionRange.Parse(normalizedRangeRepresentation);
            // Assert
            Assert.Equal(originalParsedRange, roundTrippedRange);
            Assert.Equal(originalParsedRange.ToNormalizedString(), roundTrippedRange.ToNormalizedString());
            if (isOriginalStringNormalized)
            {
                Assert.Equal(originalParsedRange.ToNormalizedString(), versionString);
            }
            else
            {
                Assert.NotEqual(originalParsedRange.ToNormalizedString(), versionString);
            }
        }

        [Fact]
        public void VersionRange_PrettyPrintAllRange()
        {
            // Arrange
            var formatter = new VersionRangeFormatter();
            var range = VersionRange.All;

            // Act
            var s = string.Format(formatter, "{0:P}", range);
            var s2 = range.ToString("P", formatter);
            var s3 = range.PrettyPrint();

            // Assert
            Assert.Equal("", s);
            Assert.Equal("", s2);
            Assert.Equal("", s3);
        }

        [Theory]
        [InlineData("1.0.0", "1.0.1-beta", false)]
        [InlineData("1.0.0", "1.0.1", true)]
        [InlineData("1.0.0-*", "1.0.0-beta", true)]
        [InlineData("1.0.0-beta.*", "1.0.0-beta.1", true)]
        [InlineData("1.0.0-beta-*", "1.0.0-beta-01", true)]
        [InlineData("1.0.0-beta-*", "2.0.0-beta", true)]
        [InlineData("1.0.*", "1.0.0-beta", false)]
        [InlineData("1.*", "1.0.0-beta", false)]
        [InlineData("*", "1.0.0-beta", false)]
        [InlineData("[1.0.0, 2.0.0]", "1.5.0-beta", false)]
        [InlineData("[1.0.0, 2.0.0-beta]", "1.5.0-beta", true)]
        [InlineData("[1.0.0-beta, 2.0.0]", "1.5.0-beta", true)]
        [InlineData("[1.0.0-beta, 2.0.0]", "3.5.0-beta", false)]
        [InlineData("[1.0.0-beta, 2.0.0]", "0.5.0-beta", false)]
        [InlineData("[1.0.0-beta, 2.0.0)", "2.0.0", false)]
        [InlineData("[1.0.0-beta, 2.0.0)", "2.0.0-beta", true)]
        [InlineData("[1.0.*, 2.0.0-beta]", "2.0.0-beta", true)]
        public void VersionRange_IsBetter_Prerelease(string rangeString, string versionString, bool expected)
        {
            // Arrange 
            var range = VersionRange.Parse(rangeString);
            var considering = NuGetVersion.Parse(versionString);

            // Act
            var result = range.IsBetter(current: null, considering: considering);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void VersionRange_MetadataIsIgnored_Satisfy()
        {
            // Arrange
            var noMetadata = VersionRange.Parse("[1.0.0, 2.0.0]");
            var lowerMetadata = VersionRange.Parse("[1.0.0+A, 2.0.0]");
            var upperMetadata = VersionRange.Parse("[1.0.0, 2.0.0+A]");
            var bothMetadata = VersionRange.Parse("[1.0.0+A, 2.0.0+A]");

            var versionNoMetadata = NuGetVersion.Parse("1.0.0");
            var versionMetadata = NuGetVersion.Parse("1.0.0+B");

            // Act & Assert
            Assert.True(noMetadata.Satisfies(versionNoMetadata));
            Assert.True(noMetadata.Satisfies(versionMetadata));
            Assert.True(lowerMetadata.Satisfies(versionNoMetadata));
            Assert.True(lowerMetadata.Satisfies(versionMetadata));
            Assert.True(upperMetadata.Satisfies(versionNoMetadata));
            Assert.True(upperMetadata.Satisfies(versionMetadata));
            Assert.True(bothMetadata.Satisfies(versionNoMetadata));
            Assert.True(bothMetadata.Satisfies(versionMetadata));
        }

        [Fact]
        public void VersionRange_MetadataIsIgnored_Equality()
        {
            // Arrange
            var noMetadata = VersionRange.Parse("[1.0.0, 2.0.0]");
            var lowerMetadata = VersionRange.Parse("[1.0.0+A, 2.0.0]");
            var upperMetadata = VersionRange.Parse("[1.0.0, 2.0.0+A]");
            var bothMetadata = VersionRange.Parse("[1.0.0+A, 2.0.0+A]");

            // Act & Assert
            Assert.True(noMetadata.Equals(lowerMetadata));
            Assert.True(lowerMetadata.Equals(upperMetadata));
            Assert.True(upperMetadata.Equals(bothMetadata));
            Assert.True(bothMetadata.Equals(noMetadata));
        }

        [Fact]
        public void VersionRange_MetadataIsIgnored_FormatRemovesMetadata()
        {
            // Arrange
            var bothMetadata = VersionRange.Parse("[1.0.0+A, 2.0.0+A]");

            // Act & Assert
            Assert.Equal("[1.0.0, 2.0.0]", bothMetadata.ToString());
            Assert.Equal("[1.0.0, 2.0.0]", bothMetadata.ToNormalizedString());
            Assert.Equal("[1.0.0, 2.0.0]", bothMetadata.ToLegacyString());
        }

        [Fact]
        public void VersionRange_FloatAllStable_ReturnsCorrectPrints()
        {
            // Arrange
            var bothMetadata = VersionRange.Parse("*");

            // Act & Assert
            Assert.Equal("[*, )", bothMetadata.ToString());
            Assert.Equal("[*, )", bothMetadata.ToNormalizedString());
            Assert.Equal("[0.0.0, )", bothMetadata.ToLegacyString()); // Note that this matches version strings generated by other version ranges such as 0.*, 0.0.*
        }


        [Fact]
        public void VersionRange_AllSpecialCases_NormalizeSame()
        {
            // Act & Assert
            Assert.Equal("(, )", VersionRange.All.ToNormalizedString());
#pragma warning disable CS0618 // Type or member is obsolete
            Assert.Equal("[*-*, )", VersionRange.AllFloating.ToNormalizedString());
            Assert.Equal("(, )", VersionRange.AllStable.ToNormalizedString());
            Assert.Equal("[*, )", VersionRange.AllStableFloating.ToNormalizedString());
#pragma warning restore CS0618 // Type or member is obsolete

        }

        [Fact]
        public void VersionRange_MetadataIsIgnored_FormatRemovesMetadata_Short()
        {
            // Arrange
            var bothMetadata = VersionRange.Parse("[1.0.0+A, )");

            // Act & Assert
            Assert.Equal("1.0.0", bothMetadata.ToLegacyShortString());
        }

        [Fact]
        public void VersionRange_MetadataIsIgnored_FormatRemovesMetadata_PrettyPrint()
        {
            // Arrange
            var bothMetadata = VersionRange.Parse("[1.0.0+A, )");

            // Act & Assert
            Assert.Equal("(>= 1.0.0)", bothMetadata.PrettyPrint());
        }

        [Theory]
        [InlineData("1.0.0", "1.0.0")]
        [InlineData("1.0.0-beta", "1.0.0-beta")]
        [InlineData("1.0.0-*", "1.0.0")]
        [InlineData("2.0.0-*", "2.0.0")]
        [InlineData("1.0.0-rc1-*", "1.0.0-rc1")]
        [InlineData("1.0.0-5.1.*", "1.0.0-5.1")]
        [InlineData("1.0.0-5.1.0-*", "1.0.0-5.1.0")]
        [InlineData("1.0.*", "1.0.0")]
        [InlineData("1.*", "1.0.0")]
        [InlineData("*", "0.0.0")]
        public void VersionRange_VerifyNonSnapshotVersion(string snapshot, string expected)
        {
            // Arrange
            var range = VersionRange.Parse(snapshot);

            // Act
            var updated = range.ToNonSnapshotRange();

            // Assert
            Assert.Equal(expected, updated.ToLegacyShortString());
        }

        [Theory]
        [InlineData("[1.0.0]")]
        [InlineData("[1.0.0, 2.0.0]")]
        [InlineData("1.0.0")]
        [InlineData("1.0.0-beta")]
        [InlineData("(1.0.0-beta, 2.0.0-alpha)")]
        [InlineData("(1.0.0-beta, 2.0.0)")]
        [InlineData("(1.0.0, 2.0.0-alpha)")]
        [InlineData("1.0.0-beta-*")]
        [InlineData("[1.0.0-beta-*, ]")]
        public void VersionRange_IncludePrerelease(string s)
        {
            // Arrange
            var range = VersionRange.Parse(s);

            // Act && Assert
            Assert.Equal(range.IsFloating, range.IsFloating);
            Assert.Equal(range.Float, range.Float);
            Assert.Equal(range.ToNormalizedString(), range.ToNormalizedString());
        }

        [Fact]
        public void ParseVersionRangeSingleDigit()
        {
            // Act
            var versionInfo = VersionRange.Parse("[1,3)");
            Assert.Equal("1.0.0", versionInfo.MinVersion?.ToNormalizedString());
            Assert.True(versionInfo.IsMinInclusive);
            Assert.Equal("3.0.0", versionInfo.MaxVersion?.ToNormalizedString());
            Assert.False(versionInfo.IsMaxInclusive);
        }

        [Fact]
        public void VersionRange_Exact()
        {
            // Act 
            var versionInfo = new VersionRange(new NuGetVersion(4, 3, 0), true, new NuGetVersion(4, 3, 0), true);

            // Assert
            Assert.True(versionInfo.Satisfies(NuGetVersion.Parse("4.3.0")));
        }

        [Theory]
        [InlineData("0", "0.0")]
        [InlineData("1", "1.0.0")]
        [InlineData("02", "2.0.0.0")]
        [InlineData("123.456", "123.456.0.0")]
        [InlineData("[2021,)", "[2021.0.0.0,)")]
        [InlineData("[,2021)", "[,2021.0.0.0)")]
        public void VersionRange_MissingVersionComponents_DefaultToZero(string shortVersionSpec, string longVersionSpec)
        {
            // Act
            var versionRange1 = VersionRange.Parse(shortVersionSpec);
            var versionRange2 = VersionRange.Parse(longVersionSpec);

            // Assert
            Assert.Equal(versionRange2, versionRange1);
        }

        [Theory]
        [InlineData("1.0.0", "0.0.0")]
        [InlineData("[1.0.0, 2.0.0]", "2.0.1")]
        [InlineData("[1.0.0, 2.0.0]", "0.0.0")]
        [InlineData("[1.0.0, 2.0.0]", "3.0.0")]
        [InlineData("[1.0.0-beta+meta, 2.0.0-beta+meta]", "1.0.0-alpha")]
        [InlineData("[1.0.0-beta+meta, 2.0.0-beta+meta]", "1.0.0-alpha+meta")]
        [InlineData("[1.0.0-beta+meta, 2.0.0-beta+meta]", "2.0.0-rc")]
        [InlineData("[1.0.0-beta+meta, 2.0.0-beta+meta]", "2.0.0+meta")]
        [InlineData("(1.0.0-beta+meta, 2.0.0-beta+meta)", "2.0.0-beta+meta")]
        [InlineData("(, 2.0.0-beta+meta)", "2.0.0-beta+meta")]
        public void ParseVersionRangeDoesNotSatisfy(string spec, string version)
        {
            // Act
            var versionInfo = VersionRange.Parse(spec);
            var middleVersion = NuGetVersion.Parse(version);

            // Assert
            Assert.False(versionInfo.Satisfies(middleVersion));
            Assert.False(versionInfo.Satisfies(middleVersion, VersionComparison.Default));
            Assert.False(versionInfo.Satisfies(middleVersion, VersionComparer.Default));
            Assert.False(versionInfo.Satisfies(middleVersion, VersionComparison.VersionRelease));
            Assert.False(versionInfo.Satisfies(middleVersion, VersionComparer.VersionRelease));
            Assert.False(versionInfo.Satisfies(middleVersion, VersionComparison.VersionReleaseMetadata));
            Assert.False(versionInfo.Satisfies(middleVersion, VersionComparer.VersionReleaseMetadata));
        }

        [Theory]
        [InlineData("1.0.0", "2.0.0")]
        [InlineData("[1.0.0, 2.0.0]", "2.0.0")]
        [InlineData("(2.0.0,)", "2.1.0")]
        [InlineData("[2.0.0]", "2.0.0")]
        [InlineData("(,2.0.0]", "2.0.0")]
        [InlineData("(,2.0.0]", "1.0.0")]
        [InlineData("[2.0.0, )", "2.0.0")]
        [InlineData("1.0.0", "1.0.0")]
        [InlineData("[1.0.0]", "1.0.0")]
        [InlineData("[1.0.0, 1.0.0]", "1.0.0")]
        [InlineData("[1.0.0, 2.0.0]", "1.0.0")]
        [InlineData("[1.0.0-beta+meta, 2.0.0-beta+meta]", "1.0.0")]
        [InlineData("[1.0.0-beta+meta, 2.0.0-beta+meta]", "1.0.0-beta+meta")]
        [InlineData("[1.0.0-beta+meta, 2.0.0-beta+meta]", "2.0.0-beta")]
        [InlineData("[1.0.0-beta+meta, 2.0.0-beta+meta]", "1.0.0+meta")]
        [InlineData("(1.0.0-beta+meta, 2.0.0-beta+meta)", "1.0.0")]
        [InlineData("(1.0.0-beta+meta, 2.0.0-beta+meta)", "2.0.0-alpha+meta")]
        [InlineData("(1.0.0-beta+meta, 2.0.0-beta+meta)", "2.0.0-alpha")]
        [InlineData("(, 2.0.0-beta+meta)", "2.0.0-alpha")]
        public void ParseVersionRangeSatisfies(string spec, string version)
        {
            // Act
            var versionInfo = VersionRange.Parse(spec);
            var middleVersion = NuGetVersion.Parse(version);

            // Assert
            Assert.True(versionInfo.Satisfies(middleVersion));
            Assert.True(versionInfo.Satisfies(middleVersion, VersionComparison.Default));
            Assert.True(versionInfo.Satisfies(middleVersion, VersionComparer.VersionRelease));
        }

        [Theory]
        [InlineData("1.0.0", "2.0.0", true, true)]
        [InlineData("1.0.0", "1.0.1", false, false)]
        [InlineData("1.0.0-beta+0", "2.0.0", false, true)]
        [InlineData("1.0.0-beta+0", "2.0.0+99", false, false)]
        [InlineData("1.0.0-beta+0", "2.0.0+99", true, true)]
        [InlineData("1.0.0", "2.0.0+99", true, true)]
        public void ParseVersionRangeParts(string minString, string maxString, bool minInc, bool maxInc)
        {
            // Arrange
            var min = NuGetVersion.Parse(minString);
            var max = NuGetVersion.Parse(maxString);

            // Act
            var versionInfo = new VersionRange(min, minInc, max, maxInc);

            // Assert
            Assert.Equal(min, versionInfo.MinVersion!, VersionComparer.Default);
            Assert.Equal(max, versionInfo.MaxVersion!, VersionComparer.Default);
            Assert.Equal(minInc, versionInfo.IsMinInclusive);
            Assert.Equal(maxInc, versionInfo.IsMaxInclusive);
        }

        [Theory]
        [InlineData("1.0.0", "2.0.0", true, true)]
        [InlineData("1.0.0", "1.0.1", false, false)]
        [InlineData("1.0.0-beta+0", "2.0.0", false, true)]
        [InlineData("1.0.0-beta+0", "2.0.0+99", false, false)]
        [InlineData("1.0.0-beta+0", "2.0.0+99", true, true)]
        [InlineData("1.0.0", "2.0.0+99", true, true)]
        public void ParseVersionRangeToStringReParse(string minString, string maxString, bool minInc, bool maxInc)
        {
            // Arrange
            var min = NuGetVersion.Parse(minString);
            var max = NuGetVersion.Parse(maxString);

            // Act
            var original = new VersionRange(min, minInc, max, maxInc);
            var versionInfo = VersionRange.Parse(original.ToString());

            // Assert
            Assert.Equal(min, versionInfo.MinVersion!, VersionComparer.Default);
            Assert.Equal(max, versionInfo.MaxVersion!, VersionComparer.Default);
            Assert.Equal(minInc, versionInfo.IsMinInclusive);
            Assert.Equal(maxInc, versionInfo.IsMaxInclusive);
        }

        [Theory]
        [InlineData("1.2.0", "1.2.0")]
        [InlineData("1.2.3", "1.2.3")]
        [InlineData("1.2.3-beta", "1.2.3-beta")]
        [InlineData("1.2.3-beta+900", "1.2.3-beta")]
        [InlineData("1.2.3-beta.2.4.55.X+900", "1.2.3-beta.2.4.55.X")]
        [InlineData("1.2.3-0+900", "1.2.3-0")]
        [InlineData("[1.2.0]", "[1.2.0]")]
        [InlineData("[1.2.3]", "[1.2.3]")]
        [InlineData("[1.2.3-beta]", "[1.2.3-beta]")]
        [InlineData("[1.2.3-beta+900]", "[1.2.3-beta]")]
        [InlineData("[1.2.3-beta.2.4.55.X+900]", "[1.2.3-beta.2.4.55.X]")]
        [InlineData("[1.2.3-0+90]", "[1.2.3-0]")]
        [InlineData("(, 1.2.0]", "(, 1.2.0]")]
        [InlineData("(, 1.2.3]", "(, 1.2.3]")]
        [InlineData("(, 1.2.3-beta]", "(, 1.2.3-beta]")]
        [InlineData("(, 1.2.3-beta+900]", "(, 1.2.3-beta]")]
        [InlineData("(, 1.2.3-beta.2.4.55.X+900]", "(, 1.2.3-beta.2.4.55.X]")]
        [InlineData("(, 1.2.3-0+900]", "(, 1.2.3-0]")]
        public void ParseVersionRangeToStringShortHand(string version, string expected)
        {
            // Act
            var versionInfo = VersionRange.Parse(version);

            // Assert
            Assert.Equal(expected, versionInfo.ToString("S", new VersionRangeFormatter()));
        }

        [Theory]
        [InlineData("1.2.0", "[1.2.0, )")]
        [InlineData("1.2.3-beta.2.4.55.X+900", "[1.2.3-beta.2.4.55.X, )")]
        public void ParseVersionRangeToString(string version, string expected)
        {
            // Act
            var versionInfo = VersionRange.Parse(version);

            // Assert
            Assert.Equal(expected, versionInfo.ToString());
        }

        [Fact]
        public void ParseVersionRangeWithNullThrows()
        {
            // Act & Assert
            ExceptionAssert.ThrowsArgNull(() => VersionRange.Parse(null!), "value");
        }

        [Theory]
        [InlineData("")]
        [InlineData("      ")]
        [InlineData("-1")]
        [InlineData("+1")]
        [InlineData("1.")]
        [InlineData(".1")]
        [InlineData("1,")]
        [InlineData(",1")]
        [InlineData(",")]
        [InlineData("-")]
        [InlineData("+")]
        [InlineData("a")]
        public void ParseVersionRangeWithBadVersionThrows(string version)
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => VersionRange.Parse(version));
            Assert.Equal($"'{version}' is not a valid version string.", exception.Message);
        }

        [Fact]
        public void ParseVersionRangeSimpleVersionNoBrackets()
        {
            // Act
            var versionInfo = VersionRange.Parse("1.2");

            // Assert
            Assert.Equal("1.2", versionInfo.MinVersion?.ToString());
            Assert.True(versionInfo.IsMinInclusive);
            Assert.Null(versionInfo.MaxVersion);
            Assert.False(versionInfo.IsMaxInclusive);
        }

        [Fact]
        public void ParseVersionRangeSimpleVersionNoBracketsExtraSpaces()
        {
            // Act
            var versionInfo = VersionRange.Parse("  1  .   2  ");

            // Assert
            Assert.Equal("1.2", versionInfo.MinVersion?.ToString());
            Assert.True(versionInfo.IsMinInclusive);
            Assert.Null(versionInfo.MaxVersion);
            Assert.False(versionInfo.IsMaxInclusive);
        }

        [Fact]
        public void ParseVersionRangeMaxOnlyInclusive()
        {
            // Act
            var versionInfo = VersionRange.Parse("(,1.2]");

            // Assert
            Assert.Null(versionInfo.MinVersion);
            Assert.False(versionInfo.IsMinInclusive);
            Assert.Equal("1.2", versionInfo.MaxVersion?.ToString());
            Assert.True(versionInfo.IsMaxInclusive);
        }

        [Fact]
        public void ParseVersionRangeMaxOnlyExclusive()
        {
            var versionInfo = VersionRange.Parse("(,1.2)");
            Assert.Null(versionInfo.MinVersion);
            Assert.False(versionInfo.IsMinInclusive);
            Assert.Equal("1.2", versionInfo.MaxVersion?.ToString());
            Assert.False(versionInfo.IsMaxInclusive);
        }

        [Fact]
        public void ParseVersionRangeExactVersion()
        {
            // Act
            var versionInfo = VersionRange.Parse("[1.2]");

            // Assert
            Assert.Equal("1.2", versionInfo.MinVersion?.ToString());
            Assert.True(versionInfo.IsMinInclusive);
            Assert.Equal("1.2", versionInfo.MaxVersion?.ToString());
            Assert.True(versionInfo.IsMaxInclusive);
        }

        [Fact]
        public void ParseVersionRangeMinOnlyExclusive()
        {
            // Act
            var versionInfo = VersionRange.Parse("(1.2,)");

            // Assert
            Assert.Equal("1.2", versionInfo.MinVersion?.ToString());
            Assert.False(versionInfo.IsMinInclusive);
            Assert.Null(versionInfo.MaxVersion);
            Assert.False(versionInfo.IsMaxInclusive);
        }

        [Fact]
        public void ParseVersionRangeExclusiveExclusive()
        {
            // Act
            var versionInfo = VersionRange.Parse("(1.2,2.3)");

            // Assert
            Assert.Equal("1.2", versionInfo.MinVersion?.ToString());
            Assert.False(versionInfo.IsMinInclusive);
            Assert.Equal("2.3", versionInfo.MaxVersion?.ToString());
            Assert.False(versionInfo.IsMaxInclusive);
        }

        [Fact]
        public void ParseVersionRangeExclusiveInclusive()
        {
            // Act
            var versionInfo = VersionRange.Parse("(1.2,2.3]");

            // Assert
            Assert.Equal("1.2", versionInfo.MinVersion?.ToString());
            Assert.False(versionInfo.IsMinInclusive);
            Assert.Equal("2.3", versionInfo.MaxVersion?.ToString());
            Assert.True(versionInfo.IsMaxInclusive);
        }

        [Fact]
        public void ParseVersionRangeInclusiveExclusive()
        {
            // Act
            var versionInfo = VersionRange.Parse("[1.2,2.3)");
            Assert.Equal("1.2", versionInfo.MinVersion?.ToString());
            Assert.True(versionInfo.IsMinInclusive);
            Assert.Equal("2.3", versionInfo.MaxVersion?.ToString());
            Assert.False(versionInfo.IsMaxInclusive);
        }

        [Fact]
        public void ParseVersionRangeInclusiveInclusive()
        {
            // Act
            var versionInfo = VersionRange.Parse("[1.2,2.3]");

            // Assert
            Assert.Equal("1.2", versionInfo.MinVersion?.ToString());
            Assert.True(versionInfo.IsMinInclusive);
            Assert.Equal("2.3", versionInfo.MaxVersion?.ToString());
            Assert.True(versionInfo.IsMaxInclusive);
        }

        [Fact]
        public void ParseVersionRangeInclusiveInclusiveExtraSpaces()
        {
            // Act
            var versionInfo = VersionRange.Parse("   [  1 .2   , 2  .3   ]  ");

            // Assert
            Assert.Equal("1.2", versionInfo.MinVersion?.ToString());
            Assert.True(versionInfo.IsMinInclusive);
            Assert.Equal("2.3", versionInfo.MaxVersion?.ToString());
            Assert.True(versionInfo.IsMaxInclusive);
        }

        [Theory]
        [InlineData("*")]
        [InlineData("1.*")]
        [InlineData("1.0.0")]
        [InlineData(" 1.0.0")]
        [InlineData("[1.0.0]")]
        [InlineData("[1.0.0] ")]
        [InlineData("[1.0.0, 2.0.0)")]
        public void ParsedVersionRangeHasOriginalString(string range)
        {
            // Act
            var versionInfo = VersionRange.Parse(range);

            // Assert
            Assert.Equal(range, versionInfo.OriginalString);
        }

        [Fact]
        public void ParseVersionRangeIntegerRanges()
        {
            // Assert
            var exception = Assert.Throws<ArgumentException>(() => VersionRange.Parse("   [-1, 2]  "));
            Assert.Equal("'   [-1, 2]  ' is not a valid version string.", exception.Message);
        }

        [Fact]
        public void ParseVersionRangeNegativeIntegerRanges()
        {
            // Act
            VersionRange? versionInfo;
            var parsed = VersionRange.TryParse("   [-1, 2]  ", out versionInfo);

            Assert.False(parsed);
            Assert.Null(versionInfo);
        }

        [Fact]
        public void TryParseNullVersionRange()
        {
            // Arrange
            VersionRange? output;

            // Act
            var parsed = VersionRange.TryParse(null!, out output);

            // Assert
            Assert.False(parsed);
            Assert.Null(output);
        }

        [Fact]
        public void ParseVersionThrowsIfExclusiveMinAndMaxVersionRangeContainsNoValues()
        {
            // Arrange
            var versionString = "(,)";

            // Assert
            var exception = Assert.Throws<ArgumentException>(() => VersionRange.Parse(versionString));
            Assert.Equal("'(,)' is not a valid version string.", exception.Message);
        }

        [Fact]
        public void ParseVersionThrowsIfInclusiveMinAndMaxVersionRangeContainsNoValues()
        {
            // Arrange
            var versionString = "[,]";

            // Assert
            var exception = Assert.Throws<ArgumentException>(() => VersionRange.Parse(versionString));
            Assert.Equal("'[,]' is not a valid version string.", exception.Message);
        }

        [Fact]
        public void ParseVersionThrowsIfInclusiveMinAndExclusiveMaxVersionRangeContainsNoValues()
        {
            // Arrange
            var versionString = "[,)";

            // Assert
            var exception = Assert.Throws<ArgumentException>(() => VersionRange.Parse(versionString));
            Assert.Equal("'[,)' is not a valid version string.", exception.Message);
        }

        [Fact]
        public void ParseVersionThrowsIfExclusiveMinAndInclusiveMaxVersionRangeContainsNoValues()
        {
            // Arrange
            var versionString = "(,]";

            // Assert
            var exception = Assert.Throws<ArgumentException>(() => VersionRange.Parse(versionString));
            Assert.Equal("'(,]' is not a valid version string.", exception.Message);
        }

        [Fact]
        public void ParseVersionThrowsIfVersionRangeIsMissingVersionComponent()
        {
            // Arrange
            var versionString = "(,1.3..2]";

            // Assert
            var exception = Assert.Throws<ArgumentException>(() => VersionRange.Parse(versionString));
            Assert.Equal("'(,1.3..2]' is not a valid version string.", exception.Message);
        }

        [Fact]
        public void ParseVersionThrowsIfVersionRangeContainsMoreThen4VersionComponents()
        {
            // Arrange
            var versionString = "(1.2.3.4.5,1.2]";

            // Assert
            var exception = Assert.Throws<ArgumentException>(() => VersionRange.Parse(versionString));
            Assert.Equal("'(1.2.3.4.5,1.2]' is not a valid version string.", exception.Message);
        }

        [Fact]
        public void ParseVersionToNormalizedVersion()
        {
            // Arrange
            var versionString = "(1.0,1.2]";

            // Assert
            Assert.Equal("(1.0.0, 1.2.0]", VersionRange.Parse(versionString).ToString());
        }

        [Theory]
        [InlineData("1.2.0")]
        [InlineData("1.2.3")]
        [InlineData("1.2.3-beta")]
        [InlineData("1.2.3-beta+900")]
        [InlineData("1.2.3-beta.2.4.55.X+900")]
        [InlineData("1.2.3-0+900")]
        [InlineData("[1.2.0]")]
        [InlineData("[1.2.3]")]
        [InlineData("[1.2.3-beta]")]
        [InlineData("[1.2.3-beta+900]")]
        [InlineData("[1.2.3-beta.2.4.55.X+900]")]
        [InlineData("[1.2.3-0+900]")]
        [InlineData("(, 1.2.0)")]
        [InlineData("(, 1.2.3)")]
        [InlineData("(, 1.2.3-beta)")]
        [InlineData("(, 1.2.3-beta+900)")]
        [InlineData("(, 1.2.3-beta.2.4.55.X+900)")]
        [InlineData("(, 1.2.3-0+900)")]
        public void StringFormatNullProvider(string range)
        {
            // Arrange
            var versionRange = VersionRange.Parse(range);
            var actual = string.Format("{0}", versionRange);
            var expected = versionRange.ToString();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("1.2.0")]
        [InlineData("1.2.3")]
        [InlineData("1.2.3-beta")]
        [InlineData("1.2.3-beta+900")]
        [InlineData("1.2.3-beta.2.4.55.X+900")]
        [InlineData("1.2.3-0+900")]
        [InlineData("[1.2.0]")]
        [InlineData("[1.2.3]")]
        [InlineData("[1.2.3-beta]")]
        [InlineData("[1.2.3-beta+900]")]
        [InlineData("[1.2.3-beta.2.4.55.X+900]")]
        [InlineData("[1.2.3-0+90]")]
        [InlineData("(, 1.2.0]")]
        [InlineData("(, 1.2.3]")]
        [InlineData("(, 1.2.3-beta]")]
        [InlineData("(, 1.2.3-beta+900]")]
        [InlineData("(, 1.2.3-beta.2.4.55.X+900]")]
        [InlineData("(, 1.2.3-0+900]")]
        public void StringFormatNullProvider2(string range)
        {
            // Arrange
            var versionRange = VersionRange.Parse(range);
            var actual = string.Format(CultureInfo.InvariantCulture, "{0}", versionRange);
            var expected = versionRange.ToString();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("(1.2.3.4, 3.2)", "1.2.3.4", false, "3.2", false)]
        [InlineData("(1.2.3.4, 3.2]", "1.2.3.4", false, "3.2", true)]
        [InlineData("[1.2, 3.2.5)", "1.2", true, "3.2.5", false)]
        [InlineData("[2.3.7, 3.2.4.5]", "2.3.7", true, "3.2.4.5", true)]
        [InlineData("(, 3.2.4.5]", null, false, "3.2.4.5", true)]
        [InlineData("(1.6, ]", "1.6", false, null, true)]
        [InlineData("[2.7]", "2.7", true, "2.7", true)]
        public void ParseVersionParsesTokensVersionsCorrectly(string versionString, string? min, bool incMin, string? max, bool incMax)
        {
            // Arrange
            var versionRange = new VersionRange(min == null ? null : NuGetVersion.Parse(min), incMin,
                max == null ? null : NuGetVersion.Parse(max), incMax);

            // Act
            var actual = VersionRange.Parse(versionString);

            // Assert
            Assert.Equal(versionRange.IsMinInclusive, actual.IsMinInclusive);
            Assert.Equal(versionRange.IsMaxInclusive, actual.IsMaxInclusive);
            Assert.Equal(versionRange.MinVersion, actual.MinVersion);
            Assert.Equal(versionRange.MaxVersion, actual.MaxVersion);
        }

        [Theory]
        [InlineData("1.0.0", "1.0.*", false)]
        [InlineData("[1.0.0,)", "[1.0.*, )", false)]
        [InlineData("1.1.*", "1.0.*", false)]
        public void VersionRange_Equals(string versionString1, string versionString2, bool isEquals)
        {
            var range1 = VersionRange.Parse(versionString1);
            var range2 = VersionRange.Parse(versionString2);

            Assert.Equal(isEquals, range1.Equals(range2));
        }

        [Fact]
        public void VersionRange_ToStringRevPrefix()
        {
            var range = VersionRange.Parse("1.1.1.*-*");

            Assert.Equal("[1.1.1.*-*, )", range.ToNormalizedString());
        }

        [Fact]
        public void VersionRange_ToStringPatchPrefix()
        {
            var range = VersionRange.Parse("1.1.*-*");

            Assert.Equal("[1.1.*-*, )", range.ToNormalizedString());
        }

        [Fact]
        public void VersionRange_ToStringMinorPrefix()
        {
            var range = VersionRange.Parse("1.*-*");

            Assert.Equal("[1.*-*, )", range.ToNormalizedString());
        }

        [Fact]
        public void VersionRange_ToStringAbsoluteLatest()
        {
            var range = VersionRange.Parse("*-*");

            Assert.Equal("[*-*, )", range.ToNormalizedString());
            Assert.Equal("0.0.0-0", range.MinVersion?.ToNormalizedString());
            Assert.Equal("0.0.0-0", range.Float?.MinVersion.ToNormalizedString());
            Assert.Equal(NuGetVersionFloatBehavior.AbsoluteLatest, range.Float?.FloatBehavior);
        }

        [Fact]
        public void VersionRange_ToStringPrereleaseMajor()
        {
            var range = VersionRange.Parse("*-rc.*");

            Assert.Equal("[*-rc.*, )", range.ToNormalizedString());
            Assert.Equal("0.0.0-rc.0", range.MinVersion?.ToNormalizedString());
            Assert.Equal("0.0.0-rc.0", range.Float?.MinVersion.ToNormalizedString());
            Assert.Equal(NuGetVersionFloatBehavior.PrereleaseMajor, range.Float?.FloatBehavior);
        }

        [Fact]
        public void FindBestMatch_FloatingPrereleaseRevision_OutsideOfRange()
        {
            var range = VersionRange.Parse("[1.0.0.*-*, 2.0.0)");

            var versions = new List<NuGetVersion>()
                {
                    NuGetVersion.Parse("0.1.0"),
                    NuGetVersion.Parse("2.0.0"),
                    NuGetVersion.Parse("2.2.0"),
                    NuGetVersion.Parse("3.0.0"),
                };

            Assert.Null(range.FindBestMatch(versions));
        }

        [Fact]
        public void FindBestMatch_FloatingPrereleaseRevision_NotMatchingPrefix_OutsideOfRange()
        {
            var range = VersionRange.Parse("[1.0.0.*-beta*, 2.0.0)");

            var versions = new List<NuGetVersion>()
                {
                    NuGetVersion.Parse("0.1.0"),
                    NuGetVersion.Parse("1.0.0-alpha.2"),
                    NuGetVersion.Parse("2.0.0"),
                    NuGetVersion.Parse("2.2.0"),
                    NuGetVersion.Parse("3.0.0"),
                };

            Assert.Null(range.FindBestMatch(versions));
        }

        [Fact]
        public void FindBestMatch_FloatingPrereleaseRevision_OutsideOfRange_Lower()
        {
            var range = VersionRange.Parse("[1.1.1.*-*, 2.0.0)");

            var versions = new List<NuGetVersion>()
                {
                    NuGetVersion.Parse("0.1.0"),
                    NuGetVersion.Parse("0.8.0"),
                    NuGetVersion.Parse("0.9.0"),
                    NuGetVersion.Parse("1.0.0"),
                };

            Assert.Null(range.FindBestMatch(versions));
        }

        [Fact]
        public void FindBestMatch_FloatingPrereleaseRevision_OutsideOfRange_Higher()
        {
            var range = VersionRange.Parse("[1.1.1.*-*, 2.0.0)");

            var versions = new List<NuGetVersion>()
                {
                    NuGetVersion.Parse("2.0.0"),
                    NuGetVersion.Parse("3.1.0"),
                };

            Assert.Null(range.FindBestMatch(versions));
        }

        [Fact]
        public void FindBestMatch_FloatingPrereleaseRevision_OnlyMatching()
        {
            var range = VersionRange.Parse("[1.0.1.*-alpha*, 2.0.0)");

            var versions = new List<NuGetVersion>()
                {
                    NuGetVersion.Parse("0.1.0"),
                    NuGetVersion.Parse("1.0.1-alpha.2"),
                    NuGetVersion.Parse("2.0.0"),
                    NuGetVersion.Parse("2.2.0"),
                    NuGetVersion.Parse("3.0.0"),
                };

            Assert.Equal("1.0.1-alpha.2", range.FindBestMatch(versions)?.ToNormalizedString());
        }

        [Fact]
        public void FindBestMatch_FloatingPrereleaseRevision_BestMatching()
        {
            var range = VersionRange.Parse("[1.0.1.*-alpha*, 2.0.0)");

            var versions = new List<NuGetVersion>()
                {
                    NuGetVersion.Parse("0.1.0"),
                    NuGetVersion.Parse("1.0.0-alpha.2"),
                    NuGetVersion.Parse("1.0.1.9-alpha.1"),
                    NuGetVersion.Parse("1.1.0-alpha.1"),
                    NuGetVersion.Parse("2.0.0"),
                    NuGetVersion.Parse("2.2.0"),
                    NuGetVersion.Parse("3.0.0"),
                };

            Assert.Equal("1.0.1.9-alpha.1", range.FindBestMatch(versions)?.ToNormalizedString());
        }

        [Fact]
        public void FindBestMatch_FloatingPrereleaseRevision_BestMatchingStable()
        {
            var range = VersionRange.Parse("[1.0.1.*-alpha*, 2.0.0)");

            var versions = new List<NuGetVersion>()
                {
                    NuGetVersion.Parse("0.1.0"),
                    NuGetVersion.Parse("1.0.0-alpha.2"),
                    NuGetVersion.Parse("1.0.1.9"),
                    NuGetVersion.Parse("1.0.9-alpha.1"),
                    NuGetVersion.Parse("1.1.0-alpha.1"),
                    NuGetVersion.Parse("2.0.0"),
                    NuGetVersion.Parse("2.2.0"),
                    NuGetVersion.Parse("3.0.0"),
                };

            Assert.Equal("1.0.1.9", range.FindBestMatch(versions)?.ToNormalizedString());
        }

        [Fact]
        public void FindBestMatch_FloatingPrereleaseRevision_BestMatchingFloating()
        {
            var range = VersionRange.Parse("[1.0.1.*-alpha*, 2.0.0)");

            var versions = new List<NuGetVersion>()
                {
                    NuGetVersion.Parse("0.1.0"),
                    NuGetVersion.Parse("1.0.0-alpha.2"),
                    NuGetVersion.Parse("1.0.1.8"),
                    NuGetVersion.Parse("1.0.1.9-alpha.1"),
                    NuGetVersion.Parse("1.1.0-alpha.1"),
                    NuGetVersion.Parse("2.0.0"),
                    NuGetVersion.Parse("2.2.0"),
                    NuGetVersion.Parse("3.0.0"),
                };

            Assert.Equal("1.0.1.9-alpha.1", range.FindBestMatch(versions)?.ToNormalizedString());
        }

        [Fact]
        public void FindBestMatch_FloatingPrereleasePatch_OutsideOfRange()
        {
            var range = VersionRange.Parse("[1.0.*-*, 2.0.0)");

            var versions = new List<NuGetVersion>()
                {
                    NuGetVersion.Parse("0.1.0"),
                    NuGetVersion.Parse("2.0.0"),
                    NuGetVersion.Parse("2.2.0"),
                    NuGetVersion.Parse("3.0.0"),
                };

            Assert.Null(range.FindBestMatch(versions));
        }

        [Fact]
        public void FindBestMatch_FloatingPrereleasePatch_NotMatchingPrefix_OutsideOfRange()
        {
            var range = VersionRange.Parse("[1.0.*-beta*, 2.0.0)");

            var versions = new List<NuGetVersion>()
                {
                    NuGetVersion.Parse("0.1.0"),
                    NuGetVersion.Parse("1.0.0-alpha.2"),
                    NuGetVersion.Parse("2.0.0"),
                    NuGetVersion.Parse("2.2.0"),
                    NuGetVersion.Parse("3.0.0"),
                };

            Assert.Null(range.FindBestMatch(versions));
        }

        [Fact]
        public void FindBestMatch_FloatingPrereleasePatch_OutsideOfRange_Lower()
        {
            var range = VersionRange.Parse("[1.1.*-*, 2.0.0)");

            var versions = new List<NuGetVersion>()
                {
                    NuGetVersion.Parse("0.1.0"),
                    NuGetVersion.Parse("0.8.0"),
                    NuGetVersion.Parse("0.9.0"),
                    NuGetVersion.Parse("1.0.0"),
                };

            Assert.Null(range.FindBestMatch(versions));
        }

        [Fact]
        public void FindBestMatch_FloatingPrereleasePatch_OutsideOfRange_Higher()
        {
            var range = VersionRange.Parse("[1.1.*-*, 2.0.0)");

            var versions = new List<NuGetVersion>()
                {
                    NuGetVersion.Parse("2.0.0"),
                    NuGetVersion.Parse("3.1.0"),
                };

            Assert.Null(range.FindBestMatch(versions));
        }

        [Fact]
        public void FindBestMatch_FloatingPrereleasePatch_OnlyMatching()
        {
            var range = VersionRange.Parse("[1.0.*-alpha*, 2.0.0)");

            var versions = new List<NuGetVersion>()
                {
                    NuGetVersion.Parse("0.1.0"),
                    NuGetVersion.Parse("1.0.0-alpha.2"),
                    NuGetVersion.Parse("2.0.0"),
                    NuGetVersion.Parse("2.2.0"),
                    NuGetVersion.Parse("3.0.0"),
                };

            Assert.Equal("1.0.0-alpha.2", range.FindBestMatch(versions)?.ToNormalizedString());
        }

        [Fact]
        public void FindBestMatch_FloatingPrereleasePatch_BestMatching()
        {
            var range = VersionRange.Parse("[1.0.*-alpha*, 2.0.0)");

            var versions = new List<NuGetVersion>()
                {
                    NuGetVersion.Parse("0.1.0"),
                    NuGetVersion.Parse("1.0.0-alpha.2"),
                    NuGetVersion.Parse("1.0.9-alpha.1"),
                    NuGetVersion.Parse("1.1.0-alpha.1"),
                    NuGetVersion.Parse("2.0.0"),
                    NuGetVersion.Parse("2.2.0"),
                    NuGetVersion.Parse("3.0.0"),
                };

            Assert.Equal("1.0.9-alpha.1", range.FindBestMatch(versions)?.ToNormalizedString());
        }

        [Fact]
        public void FindBestMatch_FloatingPrereleasePatch_BestMatchingStable()
        {
            var range = VersionRange.Parse("[1.0.*-alpha*, 2.0.0)");

            var versions = new List<NuGetVersion>()
                {
                    NuGetVersion.Parse("0.1.0"),
                    NuGetVersion.Parse("1.0.0-alpha.2"),
                    NuGetVersion.Parse("1.0.9-alpha.1"),
                    NuGetVersion.Parse("1.0.9"),
                    NuGetVersion.Parse("1.1.0-alpha.1"),
                    NuGetVersion.Parse("2.0.0"),
                    NuGetVersion.Parse("2.2.0"),
                    NuGetVersion.Parse("3.0.0"),
                };

            Assert.Equal("1.0.9", range.FindBestMatch(versions)?.ToNormalizedString());
        }

        [Fact]
        public void FindBestMatch_FloatingPrereleasePatch_BestMatchingPrerelease()
        {
            var range = VersionRange.Parse("[1.0.*-alpha*, 2.0.0)");

            var versions = new List<NuGetVersion>()
                {
                    NuGetVersion.Parse("0.1.0"),
                    NuGetVersion.Parse("1.0.0-alpha.2"),
                    NuGetVersion.Parse("1.0.8"),
                    NuGetVersion.Parse("1.0.9-alpha.1"),
                    NuGetVersion.Parse("1.1.0-alpha.1"),
                    NuGetVersion.Parse("2.0.0"),
                    NuGetVersion.Parse("2.2.0"),
                    NuGetVersion.Parse("3.0.0"),
                };

            Assert.Equal("1.0.9-alpha.1", range.FindBestMatch(versions)?.ToNormalizedString());
        }

        [Fact]
        public void FindBestMatch_FloatingPrereleaseMinor_NotMatchingPrefix_OutsideOfRange()
        {
            var range = VersionRange.Parse("[1.*-beta*, 2.0.0)");

            var versions = new List<NuGetVersion>()
                {
                    NuGetVersion.Parse("0.1.0"),
                    NuGetVersion.Parse("1.0.0-alpha.2"),
                    NuGetVersion.Parse("2.0.0"),
                    NuGetVersion.Parse("2.2.0"),
                    NuGetVersion.Parse("3.0.0"),
                };

            Assert.Null(range.FindBestMatch(versions));
        }

        [Fact]
        public void FindBestMatch_FloatingPrereleaseMinor_OutsideOfRange_Lower()
        {
            var range = VersionRange.Parse("[1.*-*, 2.0.0)");

            var versions = new List<NuGetVersion>()
                {
                    NuGetVersion.Parse("0.1.0"),
                    NuGetVersion.Parse("0.8.0"),
                    NuGetVersion.Parse("0.9.0"),
                };

            Assert.Null(range.FindBestMatch(versions));
        }

        [Fact]
        public void FindBestMatch_FloatingPrereleaseMinor_OutsideOfRange_Higher()
        {
            var range = VersionRange.Parse("[1.*-*, 2.0.0)");

            var versions = new List<NuGetVersion>()
                {
                    NuGetVersion.Parse("2.0.0"),
                    NuGetVersion.Parse("3.1.0"),
                };

            Assert.Null(range.FindBestMatch(versions));
        }

        [Fact]
        public void FindBestMatch_FloatingPrereleaseMinor_OnlyMatching()
        {
            var range = VersionRange.Parse("[1.*-alpha*, 2.0.0)");

            var versions = new List<NuGetVersion>()
                {
                    NuGetVersion.Parse("0.1.0"),
                    NuGetVersion.Parse("1.0.0-alpha.2"),
                    NuGetVersion.Parse("2.0.0"),
                    NuGetVersion.Parse("2.2.0"),
                    NuGetVersion.Parse("3.0.0"),
                };

            Assert.Equal("1.0.0-alpha.2", range.FindBestMatch(versions)?.ToNormalizedString());
        }

        [Fact]
        public void FindBestMatch_FloatingPrereleaseMinor_BestMatching()
        {
            var range = VersionRange.Parse("[1.*-alpha*, 2.0.0)");

            var versions = new List<NuGetVersion>()
                {
                    NuGetVersion.Parse("0.1.0"),
                    NuGetVersion.Parse("1.0.0-alpha.2"),
                    NuGetVersion.Parse("1.0.9-alpha.1"),
                    NuGetVersion.Parse("1.1.0-alpha.1"),
                    NuGetVersion.Parse("2.0.0"),
                    NuGetVersion.Parse("2.2.0"),
                    NuGetVersion.Parse("3.0.0"),
                };

            Assert.Equal("1.1.0-alpha.1", range.FindBestMatch(versions)?.ToNormalizedString());
        }

        [Fact]
        public void FindBestMatch_FloatingPrereleaseMinor_BestMatchingStable()
        {
            var range = VersionRange.Parse("[1.*-alpha*, 2.0.0)");

            var versions = new List<NuGetVersion>()
                {
                    NuGetVersion.Parse("0.1.0"),
                    NuGetVersion.Parse("1.0.0-alpha.2"),
                    NuGetVersion.Parse("1.0.9-alpha.1"),
                    NuGetVersion.Parse("1.1.0-alpha.1"),
                    NuGetVersion.Parse("1.10.1"),
                    NuGetVersion.Parse("2.0.0"),
                    NuGetVersion.Parse("2.2.0"),
                    NuGetVersion.Parse("3.0.0"),
                };

            Assert.Equal("1.10.1", range.FindBestMatch(versions)?.ToNormalizedString());
        }

        [Fact]
        public void FindBestMatch_FloatingPrereleaseMinor_BestMatchingPrerelease()
        {
            var range = VersionRange.Parse("[1.*-alpha*, 2.0.0)");

            var versions = new List<NuGetVersion>()
                {
                    NuGetVersion.Parse("0.1.0"),
                    NuGetVersion.Parse("1.0.0-alpha.2"),
                    NuGetVersion.Parse("1.0.9-alpha.1"),
                    NuGetVersion.Parse("1.1.0-alpha.1"),
                    NuGetVersion.Parse("1.10.1"),
                    NuGetVersion.Parse("1.99.1-alpha1"),
                    NuGetVersion.Parse("2.0.0"),
                    NuGetVersion.Parse("2.2.0"),
                    NuGetVersion.Parse("3.0.0"),
                };

            Assert.Equal("1.99.1-alpha1", range.FindBestMatch(versions)?.ToNormalizedString());
        }

        [Fact]
        public void FindBestMatch_PrereleaseRevision_RangeOpen()
        {
            var range = VersionRange.Parse("[1.0.0.*-*, )");

            var versions = new List<NuGetVersion>()
                {
                    NuGetVersion.Parse("0.1.0"),
                    NuGetVersion.Parse("0.2.0"),
                    NuGetVersion.Parse("1.0.0-alpha.2"),
                    NuGetVersion.Parse("1.0.0.1-alpha.1"),
                    NuGetVersion.Parse("1.0.1-alpha.1"),
                    NuGetVersion.Parse("101.0.0")
                };

            Assert.Equal("1.0.0.1-alpha.1", range.FindBestMatch(versions)?.ToNormalizedString());
        }

        [Fact]
        public void FindBestMatch_PrereleasePatch_RangeOpen()
        {
            var range = VersionRange.Parse("[1.0.*-*, )");

            var versions = new List<NuGetVersion>()
                {
                    NuGetVersion.Parse("0.1.0"),
                    NuGetVersion.Parse("0.2.0"),
                    NuGetVersion.Parse("1.0.0-alpha.2"),
                    NuGetVersion.Parse("1.0.1-beta.2")
                };

            Assert.Equal("1.0.1-beta.2", range.FindBestMatch(versions)?.ToNormalizedString());
        }

        [Fact]
        public void FindBestMatch_PrereleaseMinor_RangeOpen()
        {
            var range = VersionRange.Parse("[1.*-*, )");

            var versions = new List<NuGetVersion>()
                {
                    NuGetVersion.Parse("0.1.0"),
                    NuGetVersion.Parse("0.2.0"),
                    NuGetVersion.Parse("1.0.0-alpha.2"),
                    NuGetVersion.Parse("1.9.0-alpha.2"),
                    NuGetVersion.Parse("2.0.0-alpha.2"),
                    NuGetVersion.Parse("101.0.0")
                };

            Assert.Equal("1.9.0-alpha.2", range.FindBestMatch(versions)?.ToNormalizedString());
        }

        [Fact]
        public void FindBestMatch_PrereleaseMinor_IgnoresPartialPrereleaseMatches()
        {
            var range = VersionRange.Parse("[1.*-alpha*, )");

            var versions = new List<NuGetVersion>()
                {
                    NuGetVersion.Parse("0.1.0"),
                    NuGetVersion.Parse("0.2.0"),
                    NuGetVersion.Parse("1.0.0-alpha.2"),
                    NuGetVersion.Parse("1.9.0"),
                    NuGetVersion.Parse("1.20.0-alph.3"),
                    NuGetVersion.Parse("101.0.0")
                };

            Assert.Equal("1.9.0", range.FindBestMatch(versions)?.ToNormalizedString());
        }

        [Fact]
        public void FindBestMatch_PrereleaseMinor_NotMatching_SelectsFirstInRange()
        {
            var range = VersionRange.Parse("[1.*-alpha*, )");

            var versions = new List<NuGetVersion>()
                {
                    NuGetVersion.Parse("0.1.0"),
                    NuGetVersion.Parse("0.2.0"),
                    NuGetVersion.Parse("1.0.0-alph3.2"),
                    NuGetVersion.Parse("1.20.0-alph.3"),
                    NuGetVersion.Parse("101.0.0")
                };

            Assert.Equal("1.20.0-alph.3", range.FindBestMatch(versions)?.ToNormalizedString());
        }

        [Fact]
        public void FindBestMatch_PrereleaseMajor_IgnoresPartialPrereleaseMatches()
        {
            var range = VersionRange.Parse("[*-alpha*, )");

            var versions = new List<NuGetVersion>()
                {
                    NuGetVersion.Parse("0.1.0"),
                    NuGetVersion.Parse("0.2.0"),
                    NuGetVersion.Parse("1.0.0-alpha.2"),
                    NuGetVersion.Parse("1.9.0"),
                    NuGetVersion.Parse("1.20.0-alph.3"),
                };

            Assert.Equal("1.9.0", range.FindBestMatch(versions)?.ToNormalizedString());
        }

        [Fact]
        public void FindBestMatch_PrereleaseMajor_NotMatching_SelectsFirstInRange()
        {
            var range = VersionRange.Parse("[*-rc*, )");

            var versions = new List<NuGetVersion>()
                {
                    NuGetVersion.Parse("0.1.0-beta"),
                    NuGetVersion.Parse("1.0.0-alpha.2"),
                    NuGetVersion.Parse("1.9.0-alpha.2"),
                    NuGetVersion.Parse("2.0.0-alpha.2"),
                };

            Assert.Equal("0.1.0-beta", range.FindBestMatch(versions)?.ToNormalizedString());
        }

        [Fact]
        public void FindBestMatch_PrereleaseMajor_BestMatching()
        {
            var range = VersionRange.Parse("*-rc*");

            var versions = new List<NuGetVersion>()
                {
                    NuGetVersion.Parse("1.1.0"),
                    NuGetVersion.Parse("1.2.0-rc.1"),
                    NuGetVersion.Parse("1.2.0-rc.2"),
                    NuGetVersion.Parse("1.2.0-rc1"),
                    NuGetVersion.Parse("2.0.0"),
                    NuGetVersion.Parse("3.0.0-beta.1")
                };

            Assert.Equal("2.0.0", range.FindBestMatch(versions)?.ToNormalizedString());
        }

        [Fact]
        public void FindBestMatch_FloatingPrereleaseRevision_WithPartialMatch()
        {
            var range = VersionRange.Parse("[1.1.1.1*-*, 2.0.0)");

            var versions = new List<NuGetVersion>()
                {
                    NuGetVersion.Parse("1.1.1.10"),
                    NuGetVersion.Parse("3.1.0"),
                };

            Assert.Equal("1.1.1.10", range.FindBestMatch(versions)?.ToNormalizedString());
        }

        [Fact]
        public void FindBestMatch_FloatingRevision_WithPartialMatch()
        {
            var range = VersionRange.Parse("[1.1.1.1*, 2.0.0)");

            var versions = new List<NuGetVersion>()
                {
                    NuGetVersion.Parse("1.1.1.10"),
                    NuGetVersion.Parse("3.1.0"),
                };

            Assert.Equal("1.1.1.10", range.FindBestMatch(versions)?.ToNormalizedString());
        }

        [Fact]
        public void FindBestMatch_FloatingPrereleasePatch_WithPartialMatch()
        {
            var range = VersionRange.Parse("[1.1.1*-*, 2.0.0)");

            var versions = new List<NuGetVersion>()
                {
                    NuGetVersion.Parse("1.1.10"),
                    NuGetVersion.Parse("3.1.0"),
                };

            Assert.Equal("1.1.10", range.FindBestMatch(versions)?.ToNormalizedString());
        }

        [Fact]
        public void FindBestMatch_FloatingPatch_WithPartialMatch()
        {
            var range = VersionRange.Parse("[1.1.1*, 2.0.0)");

            var versions = new List<NuGetVersion>()
                {
                    NuGetVersion.Parse("1.1.10"),
                    NuGetVersion.Parse("3.1.0"),
                };

            Assert.Equal("1.1.10", range.FindBestMatch(versions)?.ToNormalizedString());
        }

        [Fact]
        public void FindBestMatch_FloatingPrereleaseMinor_WithPartialMatch()
        {
            var range = VersionRange.Parse("[1.1*-*, 2.0.0)");

            var versions = new List<NuGetVersion>()
                {
                    NuGetVersion.Parse("1.10.1"),
                    NuGetVersion.Parse("3.1.0"),
                };

            Assert.Equal("1.10.1", range.FindBestMatch(versions)?.ToNormalizedString());
        }

        [Fact]
        public void FindBestMatch_FloatingMinor_WithPartialMatch()
        {
            var range = VersionRange.Parse("[1.1*, 2.0.0)");

            var versions = new List<NuGetVersion>()
                {
                    NuGetVersion.Parse("1.10.1"),
                    NuGetVersion.Parse("3.1.0"),
                };

            Assert.Equal("1.10.1", range.FindBestMatch(versions)?.ToNormalizedString());
        }

        [Fact]
        public void FindBestMatch_FloatingPrerelease_WithExtraDashes()
        {
            var range = VersionRange.Parse("[1.0.0--*, 2.0.0)");

            var versions = new List<NuGetVersion>()
                {
                    NuGetVersion.Parse("1.0.0--alpha"),
                    NuGetVersion.Parse("1.0.0--beta"),
                    NuGetVersion.Parse("3.1.0"),
                };

            Assert.Equal("1.0.0--beta", range.FindBestMatch(versions)?.ToNormalizedString());
        }

        [Fact]
        public void FindBestMatch_FloatingPrereleaseMinor_WithExtraDashes()
        {
            var range = VersionRange.Parse("[1.*--*, 2.0.0)");

            var versions = new List<NuGetVersion>()
                {
                    NuGetVersion.Parse("1.0.0--alpha"),
                    NuGetVersion.Parse("1.0.0--beta"),
                    NuGetVersion.Parse("1.9.0--beta"),
                    NuGetVersion.Parse("3.1.0"),
                };

            Assert.Equal("1.9.0--beta", range.FindBestMatch(versions)?.ToNormalizedString());
        }

        [Theory]
        [InlineData("[1.1.4, 1.1.2)")]
        [InlineData("[1.1.4, 1.1.2]")]
        [InlineData("(1.1.4, 1.1.2)")]
        [InlineData("(1.1.4, 1.1.2]")]
        [InlineData("[1.0.0, 1.0.0)")]
        [InlineData("(1.0.0, 1.0.0]")]
        [InlineData("(1.0, 1.0.0]")]
        [InlineData("(*, *]")]
        [InlineData("[1.0.0-beta, 1.0.0-beta+900)")]
        [InlineData("(1.0.0-beta+600, 1.0.0-beta]")]
        [InlineData("(1.0)")]
        [InlineData("(1.0.0)")]
        [InlineData("[2.0.0)")]
        [InlineData("(2.0.0]")]
        public void Parse_Illogical_VersionRange_Throws(string range)
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => VersionRange.Parse(range));
            Assert.Equal($"'{range}' is not a valid version string.", exception.Message);
        }
    }
}
