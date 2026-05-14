// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Xunit;

namespace NuGet.Versioning.Test
{
    /// <summary>
    /// Tests specific to the SemVer 2.0.1-rc spec
    /// </summary>
    public class SemVer201SpecTests
    {
        // A normal version number MUST take the form X.Y.Z
        [Theory]
        [InlineData("1", false)]
        [InlineData("1.2", false)]
        [InlineData("1.2.3", true)]
        [InlineData("10.2.3", true)]
        [InlineData("13234.223.32222", true)]
        [InlineData("1.2.3.4", false)]
        [InlineData("1.2. 3", false)]
        [InlineData("1. 2.3", false)]
        [InlineData("X.2.3", false)]
        [InlineData("1.2.Z", false)]
        [InlineData("X.Y.Z", false)]
        public void SemVerVersionMustBe3Parts(string version, bool expected)
        {
            // Arrange & act
            SemanticVersion? semVer = null;
            var valid = SemanticVersion.TryParse(version, out semVer);

            // Assert
            Assert.Equal(expected, valid);
        }

        // X, Y, and Z are non-negative integers
        [Theory]
        [InlineData("-1.2.3")]
        [InlineData("1.-2.3")]
        [InlineData("1.2.-3")]
        public void SemVerVersionNegativeNumbers(string versionString)
        {
            // Arrange & act
            SemanticVersion? semVer = null;
            var valid = SemanticVersion.TryParse(versionString, out semVer);

            // Assert
            Assert.False(valid);
        }

        // X, Y, and Z MUST NOT contain leading zeroes
        [Theory]
        [InlineData("01.2.3")]
        [InlineData("1.02.3")]
        [InlineData("1.2.03")]
        [InlineData("00.2.3")]
        [InlineData("1.2.0030")]
        public void SemVerVersionLeadingZeros(string versionString)
        {
            // Arrange & act
            SemanticVersion? semVer = null;
            var valid = SemanticVersion.TryParse(versionString, out semVer);

            // Assert
            Assert.False(valid);
        }

        // Major version zero (0.y.z) is for initial development
        [Theory]
        [InlineData("0.1.2")]
        [InlineData("1.0.0")]
        [InlineData("0.0.0")]
        public void SemVerVersionValidZeros(string versionString)
        {
            // Arrange & act
            SemanticVersion? semVer = null;
            var valid = SemanticVersion.TryParse(versionString, out semVer);

            // Assert
            Assert.True(valid);
        }

        // valid release labels
        [Theory]
        [InlineData("0.1.2-Alpha")]
        [InlineData("0.1.2-Alpha.2.34.5.453.345.345.345.345.A.B.bbbbbbb.Csdfdfdf")]
        [InlineData("0.1.2-Alpha-2-5Bdd")]
        [InlineData("0.1.2--")]
        [InlineData("0.1.2--B-C-")]
        [InlineData("0.1.2--B2.-.C.-A0-")]
        [InlineData("0.1.2+NoReleaseLabel")]
        public void SemVerVersionValidReleaseLabels(string versionString)
        {
            // Arrange & act
            SemanticVersion? semVer = null;
            var valid = SemanticVersion.TryParse(versionString, out semVer);

            // Assert
            Assert.True(valid);
        }

        // Release label identifiers MUST NOT be empty
        [Theory]
        [InlineData("0.1.2-Alpha..2")]
        [InlineData("0.1.2-Alpha.")]
        [InlineData("0.1.2-.AA")]
        [InlineData("0.1.2-")]
        public void SemVerVersionInvalidReleaseId(string versionString)
        {
            // Arrange & act
            SemanticVersion? semVer = null;
            var valid = SemanticVersion.TryParse(versionString, out semVer);

            // Assert
            Assert.False(valid);
        }

        // Identifiers MUST comprise only ASCII alphanumerics and hyphen [0-9A-Za-z-]
        [Theory]
        [InlineData("0.1.2-alp=ha")]
        [InlineData("0.1.2-alp┐jj")]
        [InlineData("0.1.2-a&444")]
        [InlineData("0.1.2-a.&.444")]
        public void SemVerVersionInvalidReleaseLabelChars(string versionString)
        {
            // Arrange & act
            SemanticVersion? semVer = null;
            var valid = SemanticVersion.TryParse(versionString, out semVer);

            // Assert
            Assert.False(valid);
        }

        // Numeric identifiers MUST NOT include leading zeroes
        [Theory]
        [InlineData("0.1.2-02")]
        [InlineData("0.1.2-2.02")]
        [InlineData("0.1.2-2.A.02")]
        [InlineData("0.1.2-02.A")]
        public void SemVerVersionReleaseLabelZeros(string versionString)
        {
            // Arrange & act
            SemanticVersion? semVer = null;
            var valid = SemanticVersion.TryParse(versionString, out semVer);

            // Assert
            Assert.False(valid);
        }

        // Numeric identifiers MUST NOT include leading zeroes
        [Theory]
        [InlineData("0.1.2-02A")]
        [InlineData("0.1.2-2.02B")]
        [InlineData("0.1.2-2.A.02-")]
        [InlineData("0.1.2-A02.A")]
        public void SemVerVersionReleaseLabelValidZeros(string versionString)
        {
            // Arrange & act
            SemanticVersion? semVer = null;
            var valid = SemanticVersion.TryParse(versionString, out semVer);

            // Assert
            Assert.True(valid);
        }

        // Identifiers MUST comprise only ASCII alphanumerics and hyphen [0-9A-Za-z-]
        [Theory]
        [InlineData("0.1.2+02A")]
        [InlineData("0.1.2+A")]
        [InlineData("0.1.2+20349244.233.344.0")]
        [InlineData("0.1.2+203-49244.23-3.34-4.0-.-.-")]
        [InlineData("0.1.2+AAaaaaAAAaaaa")]
        [InlineData("0.1.2+-")]
        [InlineData("0.1.2+----.-.-.-")]
        [InlineData("0.1.2----+----")]
        public void SemVerVersionMetadataValidChars(string versionString)
        {
            // Arrange & act
            SemanticVersion? semVer = null;
            var valid = SemanticVersion.TryParse(versionString, out semVer);

            // Assert
            Assert.True(valid);
        }

        // Identifiers MUST comprise only ASCII alphanumerics and hyphen [0-9A-Za-z-]
        [Theory]
        [InlineData("0.1.2+ÄÄ")]
        [InlineData("0.1.2+22.2ÄÄ")]
        [InlineData("0.1.2+2+A")]
        public void SemVerVersionMetadataInvalidChars(string versionString)
        {
            // Arrange & act
            SemanticVersion? semVer = null;
            var valid = SemanticVersion.TryParse(versionString, out semVer);

            // Assert
            Assert.False(valid);
        }

        // Identifiers MUST NOT be empty
        [Theory]
        [InlineData("0.1.2+02A.")]
        [InlineData("0.1.2+02..A")]
        [InlineData("0.1.2+")]
        public void SemVerVersionMetadataNonEmptyParts(string versionString)
        {
            // Arrange & act
            SemanticVersion? semVer = null;
            var valid = SemanticVersion.TryParse(versionString, out semVer);

            // Assert
            Assert.False(valid);
        }

        // Leading zeros are fine for metadata
        [Theory]
        [InlineData("0.1.2+02.02-02")]
        [InlineData("0.1.2+02")]
        [InlineData("0.1.2+02A")]
        [InlineData("0.1.2+000000")]
        public void SemVerVersionMetadataLeadingZeros(string versionString)
        {
            // Arrange & act
            SemanticVersion? semVer = null;
            var valid = SemanticVersion.TryParse(versionString, out semVer);

            // Assert
            Assert.True(valid);
        }

        [Theory]
        [InlineData("0.1.2+AA-02A")]
        [InlineData("0.1.2+A.-A-02A")]
        public void SemVerVersionMetadataOrder(string versionString)
        {
            // Arrange & act
            SemanticVersion? semVer = null;
            var valid = SemanticVersion.TryParse(versionString, out semVer);

            // Assert
            Assert.True(valid);
            Assert.False(semVer!.IsPrerelease);
        }

        // Precedence is determined by the first difference when comparing each of these identifiers from left to right as follows: Major, minor, and patch versions are always compared numerically
        [Theory]
        [InlineData("1.2.3", "1.2.4")]
        [InlineData("1.2.3", "2.0.0")]
        [InlineData("9.9.9", "10.1.1")]
        public void SemVerSortVersion(string lower, string higher)
        {
            // Arrange & act
            SemanticVersion? lowerSemVer = null, higherSemVer = null;
            SemanticVersion.TryParse(lower, out lowerSemVer);
            SemanticVersion.TryParse(higher, out higherSemVer);

            // Assert
            Assert.True(VersionComparer.Default.Compare(lowerSemVer!, higherSemVer!) < 0);
        }

        // a pre-release version has lower precedence than a normal version
        [Theory]
        [InlineData("1.2.3-alpha", "1.2.3")]
        public void SemVerSortRelease(string lower, string higher)
        {
            // Arrange & act
            SemanticVersion? lowerSemVer = null, higherSemVer = null;
            SemanticVersion.TryParse(lower, out lowerSemVer);
            SemanticVersion.TryParse(higher, out higherSemVer);

            // Assert
            Assert.True(VersionComparer.Default.Compare(lowerSemVer!, higherSemVer!) < 0);
        }

        // identifiers consisting of only digits are compared numerically
        [Theory]
        [InlineData("1.2.3-2", "1.2.3-3")]
        [InlineData("1.2.3-1.9", "1.2.3-1.50")]
        public void SemVerSortReleaseNumeric(string lower, string higher)
        {
            // Arrange & act
            SemanticVersion? lowerSemVer = null, higherSemVer = null;
            SemanticVersion.TryParse(lower, out lowerSemVer);
            SemanticVersion.TryParse(higher, out higherSemVer);

            // Assert
            Assert.True(VersionComparer.Default.Compare(lowerSemVer!, higherSemVer!) < 0);
        }

        // identifiers with letters or hyphens are compared lexically in ASCII sort order
        [Theory]
        [InlineData("1.2.3-2A", "1.2.3-3A")]
        [InlineData("1.2.3-1.50A", "1.2.3-1.9A")]
        public void SemVerSortReleaseAlpha(string lower, string higher)
        {
            // Arrange & act
            SemanticVersion? lowerSemVer = null, higherSemVer = null;
            SemanticVersion.TryParse(lower, out lowerSemVer);
            SemanticVersion.TryParse(higher, out higherSemVer);

            // Assert
            Assert.True(VersionComparer.Default.Compare(lowerSemVer!, higherSemVer!) < 0);
        }

        // Numeric identifiers always have lower precedence than non-numeric identifiers
        [Theory]
        [InlineData("1.2.3-999999", "1.2.3-Z")]
        [InlineData("1.2.3-A.999999", "1.2.3-A.56-2")]
        public void SemVerSortNumericAlpha(string lower, string higher)
        {
            // Arrange & act
            SemanticVersion? lowerSemVer = null, higherSemVer = null;
            SemanticVersion.TryParse(lower, out lowerSemVer);
            SemanticVersion.TryParse(higher, out higherSemVer);

            // Assert
            Assert.True(VersionComparer.Default.Compare(lowerSemVer!, higherSemVer!) < 0);
        }

        // A larger set of pre-release fields has a higher precedence than a smaller set
        [Theory]
        [InlineData("1.2.3-a", "1.2.3-a.2")]
        [InlineData("1.2.3-a.2.3.4", "1.2.3-a.2.3.4.5")]
        public void SemVerSortReleaseLabelCount(string lower, string higher)
        {
            // Arrange & act
            SemanticVersion? lowerSemVer = null, higherSemVer = null;
            SemanticVersion.TryParse(lower, out lowerSemVer);
            SemanticVersion.TryParse(higher, out higherSemVer);

            // Assert
            Assert.True(VersionComparer.Default.Compare(lowerSemVer!, higherSemVer!) < 0);
        }

        // ignore release label casing
        [Theory]
        [InlineData("1.2.3-a", "1.2.3-A")]
        [InlineData("1.2.3-A-b2-C", "1.2.3-a-B2-c")]
        public void SemVerSortIgnoreReleaseCasing(string a, string b)
        {
            // Arrange & act
            SemanticVersion? semVerA = null, semVerB = null;
            SemanticVersion.TryParse(a, out semVerA);
            SemanticVersion.TryParse(b, out semVerB);

            // Assert
            Assert.True(VersionComparer.Default.Equals(semVerA!, semVerB!));
        }
    }
}
