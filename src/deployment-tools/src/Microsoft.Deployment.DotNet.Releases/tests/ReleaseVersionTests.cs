// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Xunit;

namespace Microsoft.Deployment.DotNet.Releases.Tests
{
    public class ReleaseVersionTests
    {
        [Theory]
        [InlineData(-1, -1, -1, "*", "$$", typeof(ArgumentOutOfRangeException))]
        [InlineData(1, 0, 0, null, "$$", typeof(FormatException))]
        [InlineData(1, 0, 0, null, "+12345", typeof(FormatException))]
        public void CtorThrowsIfVersionPartsAreInvalid(int major, int minor, int patch, string prerelease, string buildMetadata, Type expectedException)
        {
            Exception e = Assert.Throws(expectedException, () =>
            {
                ReleaseVersion v = new ReleaseVersion(major, minor, patch, prerelease, buildMetadata);
            });
        }

        [Theory]
        [InlineData("3.0.100-preview1", 100, 0)]
        [InlineData("2.1.807", 800, 7)]
        public void ItSupportsFeatureBandsAndPatchLevelsForSdkVersions(string version, int expectedFeatureBand, int expectedPatchLevel)
        {
            ReleaseVersion v = new ReleaseVersion(version);

            Assert.Equal(expectedFeatureBand, v.SdkFeatureBand);
            Assert.Equal(expectedPatchLevel, v.SdkPatchLevel);
        }

        [Theory]
        [InlineData("1.2.3-preview.4", 1, 2, 3, "preview.4")]
        [InlineData("5.0.0-preview.6.20305.6", 5, 0, 0, "preview.6.20305.6")]
        public void CtorFromString(string versionString, int expectedMajor, int expectedMinor, int expectedPatch, string expectedPrerelease)
        {
            ReleaseVersion v = new ReleaseVersion(versionString);

            Assert.Equal(expectedMajor, v.Major);
            Assert.Equal(expectedMinor, v.Minor);
            Assert.Equal(expectedPatch, v.Patch);
            Assert.Equal(expectedPrerelease, v.Prerelease);
        }

        [Theory]
        [InlineData(1, 2, 3, "preview.4", "1.2.3-preview.4", -1, -1)]
        [InlineData(5, 0, 782, "preview.6.20305.6", "5.0.782-preview.6.20305.6", 700, 82)]
        public void CtorFromVersionParts(int major, int minor, int patch, string prerelease, string expectedVersion,
            int expectedSdkFeatureBand, int expectedSdkPatchLevel)
        {
            ReleaseVersion v = new ReleaseVersion(major, minor, patch, prerelease);

            Assert.Equal(expectedVersion, v.ToString());

            if (patch > 99)
            {
                Assert.Equal(expectedSdkFeatureBand, v.SdkFeatureBand);
                Assert.Equal(expectedSdkPatchLevel, v.SdkPatchLevel);
            }
        }

        [Fact]
        public void CompareReturnZeroIfReferenceEqual()
        {
            ReleaseVersion v1 = new ReleaseVersion("1.0.0");
            ReleaseVersion v2 = v1;

            Assert.Equal(0, ReleaseVersion.Compare(v1, v2));
        }

        [Fact]
        public void CompareNullAndNotNullVersion()
        {
            ReleaseVersion v1 = null;
            ReleaseVersion v2 = new ReleaseVersion("1.0.0");

            Assert.Equal(-1, ReleaseVersion.Compare(v1, v2));
        }

        [Fact]
        public void CompareNotNullAndNullVersion()
        {
            ReleaseVersion v1 = new ReleaseVersion("1.0.0");
            ReleaseVersion v2 = null;

            Assert.Equal(1, ReleaseVersion.Compare(v1, v2));
        }

        [Fact]
        public void GetHashCodeUsesAllProperties()
        {
            ReleaseVersion v1 = new ReleaseVersion("10.6.8-alpha3+556");
            ReleaseVersion v2 = new ReleaseVersion("10.6.8-alpha3+556");
            ReleaseVersion v3 = new ReleaseVersion("10.6.8-alpha3");
            ReleaseVersion v4 = new ReleaseVersion("10.6.8");

            Assert.Equal(v1.GetHashCode(), v2.GetHashCode());
            Assert.NotEqual(v1.GetHashCode(), v3.GetHashCode());
            Assert.NotEqual(v1.GetHashCode(), v4.GetHashCode());
            Assert.NotEqual(v3.GetHashCode(), v4.GetHashCode());
        }

        [Fact]
        public void ItThrowsFormatExceptionIfStringVersionInvalid()
        {
            Exception e = Assert.Throws<FormatException>(
                delegate
                {
                    ReleaseVersion v = new ReleaseVersion("1");
                }
                );

            Assert.Equal("The specified version does not contain a valid major, minor, and patch version: '1'.", e.Message);
        }

        [Fact]
        public void NumericIdentifiersCanBeLargerThanInt()
        {
            // Test for values that would break max int
            ReleaseVersion v1 = new ReleaseVersion("1.0.0-2147483647");
            ReleaseVersion v2 = new ReleaseVersion("1.0.0-2147483648");

            Assert.True(v1.ComparePrecedenceTo(v2) < 0);
            Assert.True(v2.ComparePrecedenceTo(v1) > 0);
        }

        [Theory]
        [InlineData(null, "1.0.0-beta4", 1)]
        [InlineData("1.0.0-beta4", "1.0.0-beta4", 0)]
        [InlineData("1.0.0-beta5", "1.0.0-beta4", -1)]
        public void ComparePrecedenceToReturnsRelativeSortOrder(string version1, string version2, int expectedPrecedence)
        {
            ReleaseVersion v1 = string.IsNullOrWhiteSpace(version1) ? null : new ReleaseVersion(version1);
            ReleaseVersion v2 = string.IsNullOrWhiteSpace(version2) ? null : new ReleaseVersion(version2);

            if (expectedPrecedence == 1)
            {
                Assert.True(v2.ComparePrecedenceTo(v1) > 0);
            }
            else if (expectedPrecedence == 0)
            {
                Assert.True(v2.ComparePrecedenceTo(v1) == 0);
            }
            else if (expectedPrecedence == -1)
            {
                Assert.True(v2.ComparePrecedenceTo(v1) < 0);
            }
        }

        [Theory]
        [InlineData("1.10.15-RC2+55487", 0, "")]
        [InlineData("1.10.15-RC2+55487", 1, "1")]
        [InlineData("1.10.15-RC2+55487", 2, "1.10")]
        [InlineData("1.10.15-RC2+55487", 3, "1.10.15")]
        [InlineData("1.10.15-RC2+55487", 4, "1.10.15-RC2")]
        [InlineData("1.10.15-RC2+55487", 5, "1.10.15-RC2+55487")]

        public void ToStringReturnsExpectedNumberOfFields(string version, int fieldCount, string expectedValue)
        {
            ReleaseVersion v = new ReleaseVersion(version);

            Assert.Equal(expectedValue, v.ToString(fieldCount));
        }

        [Fact]
        public void OperatorEquals()
        {
            ReleaseVersion v1 = new ReleaseVersion("1.0.0");
            ReleaseVersion v2 = new ReleaseVersion("1.0.0");

            Assert.True(v1 == v2);
        }

        [Fact]
        public void OperatorEqualsDoesNotIgnoreBuildMetadata()
        {
            ReleaseVersion v1 = new ReleaseVersion("1.0.0+b3045");
            ReleaseVersion v2 = new ReleaseVersion("1.0.0+b3046");

            Assert.True(v1 != v2);
        }

        [Fact]
        public void OperatorEqualsDoesNotIgnorePrelease()
        {
            ReleaseVersion v1 = new ReleaseVersion("1.0.0-preview1");
            ReleaseVersion v2 = new ReleaseVersion("1.0.0-preview1");

            Assert.True(v1 == v2);
        }

        [Theory]
        [InlineData("1.0.0-preview.4", "1.0.0-preview.5", -1)]
        [InlineData("1.0.0-preview.4", "1.0.0-preview.4", 0)]
        [InlineData("1.0.0-preview.1234567890123456", "1.0.0-preview.12345678901234567", -1)]
        [InlineData("1.0.0-preview.1.2", "1.0.0-preview.2", -1)]
        [InlineData("1.0.0-preview.1.2.3.4.6", "1.0.0-preview.1.2.3.4.5", 1)]
        public void CompareToIncludesPrecedence(string version1, string version2, int expectedResult)
        {
            ReleaseVersion v1 = new ReleaseVersion(version1);
            ReleaseVersion v2 = new ReleaseVersion(version2);

            Assert.Equal(expectedResult, v1.CompareTo(v2));
        }

        [Fact]
        public void OperatorNotEquals()
        {
            ReleaseVersion v1 = new ReleaseVersion("1.0.0-preview1");
            ReleaseVersion v2 = new ReleaseVersion("2.0.0-preview1");

            Assert.True(v1 != v2);
        }

        [Theory]
        [InlineData("1.0.0-preview1", "2.0.0-preview1")]
        [InlineData("1.1.0-preview1", "1.2.0-preview1")]
        [InlineData("1.1.1-preview1", "1.1.2-preview1")]
        [InlineData("1.1.1-preview1", "1.1.1-preview2")]
        public void OperatorGreater(string version1, string version2)
        {
            ReleaseVersion v1 = new ReleaseVersion(version1);
            ReleaseVersion v2 = new ReleaseVersion(version2);

            Assert.True(v2 > v1);
        }

        [Fact]
        public void OperatorGreaterOrEqual()
        {
            ReleaseVersion v1 = new ReleaseVersion("1.0.0+b3045");
            ReleaseVersion v2 = new ReleaseVersion("1.0.0+b3046");

            Assert.True(v2 >= v1);
        }

        [Fact]
        public void OperatorLess()
        {
            ReleaseVersion v1 = new ReleaseVersion("1.0.0-alpha1");
            ReleaseVersion v2 = new ReleaseVersion("1.0.0-beta2");

            Assert.True(v1 < v2);
        }

        [Fact]
        public void OperatorLessOrEqual()
        {
            ReleaseVersion v1 = new ReleaseVersion("1.0.0-alpha2");
            ReleaseVersion v2 = new ReleaseVersion("1.0.0-alpha3");

            Assert.True(v1 <= v2);
        }

        [Fact]
        public void VersionsThatDifferInBuildMetadataHaveEqualPrecedence()
        {
            ReleaseVersion v1 = new ReleaseVersion("1.0.0+b3045");
            ReleaseVersion v2 = new ReleaseVersion("1.0.0+b3046");

            Assert.True(v1.PrecedenceEquals(v2));
        }

        [Fact]
        public void ToStringContainsMajorMinorPatch()
        {
            ReleaseVersion v1 = new ReleaseVersion("4.9.7");

            Assert.Equal("4.9.7", v1.ToString());
        }

        [Fact]
        public void ToStringContainsPrereleaseLabels()
        {
            ReleaseVersion v1 = new ReleaseVersion("4.9.7-a.b.c.d.e.f");

            Assert.Equal("4.9.7-a.b.c.d.e.f", v1.ToString());
        }

        [Fact]
        public void ToStringContainsBuildMetadata()
        {
            ReleaseVersion v1 = new ReleaseVersion("4.9.7+build.12345");

            Assert.Equal("4.9.7+build.12345", v1.ToString());
        }

        [Theory]
        [InlineData("15.16.18", true)]
        [InlineData("15.16.18-preview.8", true)]
        [InlineData("15.16.18+000000", true)]
        [InlineData("15.16.18-preview.8+000000", true)]
        [InlineData("x", false)]
        [InlineData("", false)]
        [InlineData(null, false)]
        [InlineData("1.2.3.", false)]
        [InlineData("5.3.00", false)]
        [InlineData("0.1.1-", false)]
        [InlineData("0.1.1+", false)]
        [InlineData("0.1.1-+", false)]
        [InlineData("0.1.1+-", true)]
        [InlineData("0.1.1--", true)]
        [InlineData("0.1.1++", false)]
        [InlineData("0.1.1-+abcdef", false)]
        [InlineData("0.1.1-+abcdef.", false)]
        public void ItCanParseAVersionString(string input, bool expectedResult)
        {
            bool result = ReleaseVersion.TryParse(input, false, out ReleaseVersion _);

            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(null, false)]
        [InlineData("", false)]
        [InlineData("123a+#@!", false)]
        [InlineData("01", false)]
        [InlineData("0", true)]
        [InlineData("123456", true)]
        public void ItCanClassifyNumericIdentifiers(string input, bool expectedResult)
        {
            Assert.Equal(expectedResult, ReleaseVersion.IsNumericIdentifier(input));
        }

        [Theory]
        [InlineData(null, false)]
        [InlineData("", false)]
        [InlineData("123a+#@!", false)]
        [InlineData("01", true)]
        [InlineData("0", true)]
        [InlineData("123456", true)]
        [InlineData("--3--", true)]
        public void ItCanClassifyPrereleaseIdentifiers(string input, bool expectedResult)
        {
            Assert.Equal(expectedResult, ReleaseVersion.IsPrereleaseIdentifier(input));
        }

        [Theory]
        [InlineData("00", false, -1)]
        [InlineData("-1", false, -1)]
        [InlineData("0", true, 0)]
        [InlineData("12345", true, 12345)]
        [InlineData(null, false, -1)]
        [InlineData("", false, -1)]
        [InlineData("hello", false, -1)]
        public void ItCanParseNumericIdentifiers(string input, bool expectedResult, int expectedValue)
        {
            bool result = ReleaseVersion.TryParseCoreVersionPart(input, false, out int value);

            Assert.Equal(expectedResult, result);
            Assert.Equal(expectedResult ? expectedValue : -1, value);
        }
    }
}
