// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using Xunit;

namespace NuGet.Versioning.Test
{
    public class ExternalComparerTests
    {
        [Theory]
        [InlineData("(2.3.1-RC+srv01-a5c5ff9, 2.3.1-RC+srv02-dbf5ec0)", "2.3.1-RC+srv03-d9375a6")]
        [InlineData("(2.3.1-RC+srv01-a5c5ff9, 2.3.1-RC+srv02-dbf5ec0)", "2.3.1-RC+srv04-0ed1eb0")]
        [InlineData("(2.3.1-RC+srv01-a5c5ff9, 2.3.1-RC+srv02-dbf5ec0)", "2.3.1-RC+srv04-cc5438c")]
        public void NuGetVersionRangeWithGitCommit(string verSpec, string ver)
        {
            // Arrange
            var versionInfo = VersionRange.Parse(verSpec);
            var version = NuGetVersion.Parse(ver);
            var comparer = new GitMetadataComparer();

            // Act
            var result = versionInfo.Satisfies(version, comparer);

            // Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData("(2.3.1-RC+srv02-dbf5ec0, )", "2.3.1-RC+srv03-d9375a6")]
        [InlineData("(2.3.1-RC+srv02-dbf5ec0, )", "2.3.1-RC+srv04-0ed1eb0")]
        [InlineData("(2.3.1-RC+srv02-dbf5ec0, )", "2.3.1-RC+srv04-cc5438c")]
        [InlineData("[2.3.1-RC+srv02-dbf5ec0, )", "2.3.1-RC+srv00-a5c5ff9")]
        public void NuGetVersionRangeWithGitCommitNotInRange(string verSpec, string ver)
        {
            // Arrange
            var versionInfo = VersionRange.Parse(verSpec);
            var version = NuGetVersion.Parse(ver);
            var comparer = new GitMetadataComparer();

            // Act
            var result = versionInfo.Satisfies(version, comparer);

            // Assert
            Assert.False(result);
        }

        [Theory]
        [InlineData("2.3.1.0-RC+srv03-d9375a6", "2.3.1-RC+srv04-d9375a6")]
        [InlineData("2.3.1.0-RC+srv04-d9375a6", "2.3.1-RC+srv01-d9375a6")]
        public void MixedVersionCompare(string version1, string version2)
        {
            // Arrange
            var semVer1 = NuGetVersion.Parse(version1);
            var semVer2 = NuGetVersion.Parse(version2);
            var comparer = new GitMetadataComparer();

            // Act
            var result = comparer.Compare(semVer1, semVer2);

            // Assert
            Assert.Equal(0, result);
        }

        [Theory]
        [InlineData("2.3.1.1-RC+srv03-d9375a6", "2.3.1-RC+srv04-d9375a6")]
        [InlineData("2.3.1.0-RC+srv04-d9375a6", "2.3.1-RC.2+srv01-d9375a6")]
        public void MixedVersionCompareNotEqual(string version1, string version2)
        {
            // Arrange
            var semVer1 = NuGetVersion.Parse(version1);
            var semVer2 = NuGetVersion.Parse(version2);
            var comparer = new GitMetadataComparer();

            // Act
            var result = comparer.Compare(semVer1, semVer2) == 0;

            // Assert
            Assert.False(result);
        }

        [Theory]
        [InlineData("2.3.1-RC+srv03-d9375a6", "2.3.1-RC+srv04-d9375a6")]
        [InlineData("2.3.1-RC+srv04-d9375a6", "2.3.1-RC+srv01-d9375a6")]
        public void DictionaryWithGitCommit(string version1, string version2)
        {
            // Arrange
            var semVer1 = NuGetVersion.Parse(version1);
            var semVer2 = NuGetVersion.Parse(version2);
            var comparer = new GitMetadataComparer();
            var gitHash = new HashSet<NuGetVersion>(comparer);

            // Act
            gitHash.Add(semVer1);

            // Assert
            Assert.Contains(semVer2, gitHash);
        }
    }
}
