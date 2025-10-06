// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using NuGet.Versioning;
using Xunit;

namespace NuGet.LibraryModel.Tests
{
    public class LibraryRangeTests
    {
        [Theory]
        [InlineData("1.0.0", "packageA >= 1.0.0")]
        [InlineData("1.0.0-*", "packageA >= 1.0.0-*")]
        [InlineData("[ , ]", "packageA")]
        [InlineData("[ , 1.0.0 ]", "packageA <= 1.0.0")]
        [InlineData("[ , 1.0.0 )", "packageA < 1.0.0")]
        [InlineData("[1.0.0 , 2.0.0]", "packageA >= 1.0.0 <= 2.0.0")]
        [InlineData("(1.0.0 , 2.0.0)", "packageA > 1.0.0 < 2.0.0")]
        [InlineData("(1.0.0 , 2.0.0]", "packageA > 1.0.0 <= 2.0.0")]
        [InlineData(null, "packageA")]
        public void LibraryRange_ToLockFileDependencyGroupString(string versionRange, string expected)
        {
            // Arrange
            LibraryRange range = new LibraryRange()
            {
                Name = "packageA",
                VersionRange = versionRange != null ? VersionRange.Parse(versionRange) : null,
                TypeConstraint = LibraryDependencyTarget.Project
            };

            // Act and Assert
            Assert.Equal(expected, range.ToLockFileDependencyGroupString());
        }

        [Theory]
        [InlineData("packageA", "packageA", true)] // same
        [InlineData("packageA", "PackageA", true)] // same except different case
        [InlineData("packageA", "packageB", false)] // different
        [InlineData("packageA", "packageA ", false)] // different
        public void LibraryRange_Equals_Name(string nameA, string nameB, bool expected)
        {
            // Arrange
            var rangeA = new LibraryRange(nameA, LibraryDependencyTarget.All);
            var rangeB = new LibraryRange(nameB, LibraryDependencyTarget.All);

            // Act
            var actual = rangeA.Equals(rangeB);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("1.0.0-beta", "1.0.0-beta", true)] // same
        [InlineData("1.0.0-beta", "1.0.0-BETA", true)] // same except different case
        [InlineData("1.0.0-beta", "2.0.0-beta", false)] // different
        public void LibraryRange_Equals_Version(string versionA, string versionB, bool expected)
        {
            // Arrange
            var rangeA = new LibraryRange("packageA", VersionRange.Parse(versionA), LibraryDependencyTarget.All);
            var rangeB = new LibraryRange("packageA", VersionRange.Parse(versionB), LibraryDependencyTarget.All);

            // Act
            var actual = rangeA.Equals(rangeB);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(LibraryDependencyTarget.Package, LibraryDependencyTarget.Package, true)] // same
        [InlineData(LibraryDependencyTarget.Package, LibraryDependencyTarget.All, false)] // subset
        [InlineData(LibraryDependencyTarget.Package, LibraryDependencyTarget.Assembly, false)] // different
        public void LibraryRange_Equals_TypeConstraint(
            LibraryDependencyTarget typeConstraintA,
            LibraryDependencyTarget typeConstraintB,
            bool expected)
        {
            // Arrange
            var version = VersionRange.Parse("1.0.0");
            var rangeA = new LibraryRange("packageA", version, typeConstraintA);
            var rangeB = new LibraryRange("packageA", version, typeConstraintB);

            // Act
            var actual = rangeA.Equals(rangeB);

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}
