// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Diagnostics;
using System.Threading;
using NuGet.Versioning;
using Xunit;

namespace NuGet.LibraryModel.Tests
{
    public class LibraryIdentityTests
    {
        [Theory]
        [InlineData("packageA", "packageA", true)] // same
        [InlineData("packageA", "PackageA", true)] // same except different case
        [InlineData("packageA", "packageB", false)] // different
        [InlineData("packageA", "packageA ", false)] // different
        public void LibraryIdentity_Equals_Name(string nameA, string nameB, bool expected)
        {
            // Arrange
            var version = new NuGetVersion("1.0.0");
            var identityA = new LibraryIdentity(nameA, version, LibraryType.Package);
            var identityB = new LibraryIdentity(nameB, version, LibraryType.Package);

            // Act
            var actual = identityA.Equals(identityB);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("1.0.0-beta", "1.0.0-beta", true)] // same
        [InlineData("1.0.0-beta", "1.0.0-BETA", true)] // same except different case
        [InlineData("1.0.0-beta", "2.0.0-beta", false)] // different
        public void LibraryIdentity_Equals_Version(string versionA, string versionB, bool expected)
        {
            // Arrange
            var identityA = new LibraryIdentity("packageA", new NuGetVersion(versionA), LibraryType.Package);
            var identityB = new LibraryIdentity("packageA", new NuGetVersion(versionB), LibraryType.Package);

            // Act
            var actual = identityA.Equals(identityB);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("package", "package", true)] // same
        [InlineData("package", "PACKAGE", true)] // same except different case
        [InlineData("package", "assembly", false)] // different
        public void LibraryIdentity_Equals_LibraryType(string typeA, string typeB, bool expected)
        {
            // Arrange
            var version = new NuGetVersion("1.0.0");
            var identityA = new LibraryIdentity("packageA", version, LibraryType.Parse(typeA));
            var identityB = new LibraryIdentity("packageA", version, LibraryType.Parse(typeB));

            // Act
            var actual = identityA.Equals(identityB);

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}
