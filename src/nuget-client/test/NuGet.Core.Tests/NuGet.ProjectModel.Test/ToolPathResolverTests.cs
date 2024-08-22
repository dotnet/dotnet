// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.IO;
using NuGet.Commands;
using NuGet.Frameworks;
using NuGet.Versioning;
using Xunit;

namespace NuGet.ProjectModel.Test
{
    public class ToolPathResolverTests
    {
        [Fact]
        public void ToolPathResolver_BuildsLowercaseLockFilePath()
        {
            // Arrange
            var target = new ToolPathResolver("packages", isLowercase: true);
            var expected = Path.Combine(
                "packages",
                ".tools",
                "packagea",
                "3.1.4-beta",
                "netstandard1.3",
                "project.assets.json");

            // Act
            var actual = target.GetLockFilePath(
                "PackageA",
                NuGetVersion.Parse("3.1.4-BETA"),
                FrameworkConstants.CommonFrameworks.NetStandard13);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ToolPathResolver_BuildsLowercaseCacheFileDirectoryPath()
        {
            // Arrange
            var target = new ToolPathResolver("packages", isLowercase: true);
            var expected = Path.Combine(
                "packages",
                ".tools",
                "packagea",
                "3.1.4-beta",
                "netstandard1.3");
            // Act

            var actual = target.GetToolDirectoryPath("packagea", NuGetVersion.Parse("3.1.4-beta"), NuGetFramework.Parse("netstandard1.3"));

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ToolPathResolver_BuildsOriginalcaseCacheFileDirectoryPath()
        {
            // Arrange
            var target = new ToolPathResolver("packages", isLowercase: false);
            var expected = Path.Combine(
                "packages",
                ".tools",
                "packagea",
                "3.1.4-BETA",
                "netstandard1.3");
            // Act

            var actual = target.GetToolDirectoryPath("packagea", NuGetVersion.Parse("3.1.4-BETA"), NuGetFramework.Parse("netstandard1.3"));

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ToolPathResolver_BuildsOriginalCaseLockFilePath()
        {
            // Arrange
            var target = new ToolPathResolver("packages", isLowercase: false);
            var expected = Path.Combine(
                "packages",
                ".tools",
                "PackageA",
                "3.1.4-BETA",
                "netstandard1.3",
                "project.assets.json");

            // Act
            var actual = target.GetLockFilePath(
                "PackageA",
                NuGetVersion.Parse("3.1.4-BETA"),
                FrameworkConstants.CommonFrameworks.NetStandard13);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void NoOpRestoreUtility_CacheFileToolNameIsLowercase()
        {
            var package = "PackageA";
            // Arrange
            var target = new ToolPathResolver("packages", isLowercase: true);
            var expected = Path.Combine(
                "packages",
                ".tools",
                "packagea",
                "3.1.4-beta",
                "netstandard1.3",
                "packagea.nuget.cache");

            // Act
            var actual = NoOpRestoreUtilities.GetToolCacheFilePath(
                target.GetToolDirectoryPath(
                package,
                NuGetVersion.Parse("3.1.4-beta"),
                FrameworkConstants.CommonFrameworks.NetStandard13), package);

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}
