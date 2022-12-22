// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using NuGet.Packaging.Core;
using NuGet.Test.Utility;
using NuGet.Versioning;
using Xunit;

namespace NuGet.Packaging.Test
{
    public class FallbackPackagePathResolverTests
    {
        [Fact]
        public void FallbackPackagePathResolver_MissingPackageNoFallbacks()
        {
            // Arrange
            using (var mockBaseDirectory = TestDirectory.Create())
            {
                var userFolder = Path.Combine(mockBaseDirectory, "global");
                var fallbackFolders = new List<string>()
                {
                };

                var resolver = new FallbackPackagePathResolver(userFolder, fallbackFolders);

                // Act
                var path = resolver.GetPackageDirectory("a", "1.0.0");

                // Assert
                Assert.Null(path);
            }
        }

        [Fact]
        public void FallbackPackagePathResolver_MissingPackageWithFallbacks()
        {
            // Arrange
            using (var mockBaseDirectory = TestDirectory.Create())
            {
                var userFolder = Path.Combine(mockBaseDirectory, "global");
                var fallbackFolders = new List<string>()
                {
                    Path.Combine(mockBaseDirectory, "fallback1"),
                    Path.Combine(mockBaseDirectory, "fallback2"),
                };

                Directory.CreateDirectory(userFolder);

                foreach (var fallback in fallbackFolders)
                {
                    Directory.CreateDirectory(fallback);
                }

                var resolver = new FallbackPackagePathResolver(userFolder, fallbackFolders);

                // Act
                var path = resolver.GetPackageDirectory("a", "1.0.0");

                // Assert
                Assert.Null(path);
            }
        }

        [Fact]
        public void FallbackPackagePathResolver_MissingFallbackFolder()
        {
            // Arrange
            using (var mockBaseDirectory = TestDirectory.Create())
            {
                var userFolder = Path.Combine(mockBaseDirectory, "global");
                var fallbackFolders = new List<string>()
                {
                    Path.Combine(mockBaseDirectory, "fallback1"),
                    Path.Combine(mockBaseDirectory, "fallback2"),
                };

                Directory.CreateDirectory(userFolder);

                // Act & Assert
                Assert.ThrowsAny<PackagingException>(() =>
                    new FallbackPackagePathResolver(userFolder, fallbackFolders));
            }
        }

        [Fact]
        public async Task FallbackPackagePathResolver_FindPackageInUserFolder()
        {
            // Arrange
            using (var mockBaseDirectory = TestDirectory.Create())
            {
                var userFolder = Path.Combine(mockBaseDirectory, "global");
                var fallbackFolders = new List<string>()
                {
                    Path.Combine(mockBaseDirectory, "fallback1"),
                    Path.Combine(mockBaseDirectory, "fallback2"),
                };

                Directory.CreateDirectory(userFolder);

                foreach (var fallback in fallbackFolders)
                {
                    Directory.CreateDirectory(fallback);
                }

                var saveMode = PackageSaveMode.Nuspec | PackageSaveMode.Files;

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    userFolder,
                    saveMode,
                    new PackageIdentity("a", NuGetVersion.Parse("1.0.0")));

                var expected = Path.Combine(userFolder, "a", "1.0.0");

                var resolver = new FallbackPackagePathResolver(userFolder, fallbackFolders);

                // Act
                var path = resolver.GetPackageDirectory("a", "1.0.0");

                // Assert
                Assert.Equal(expected, path);
            }
        }

        [Fact]
        public async Task FallbackPackagePathResolver_FindPackageInFallbackFolder()
        {
            // Arrange
            using (var mockBaseDirectory = TestDirectory.Create())
            {
                var userFolder = Path.Combine(mockBaseDirectory, "global");
                var fallbackFolders = new List<string>()
                {
                    Path.Combine(mockBaseDirectory, "fallback1"),
                    Path.Combine(mockBaseDirectory, "fallback2"),
                };

                Directory.CreateDirectory(userFolder);

                foreach (var fallback in fallbackFolders)
                {
                    Directory.CreateDirectory(fallback);
                }

                var saveMode = PackageSaveMode.Nuspec | PackageSaveMode.Files;

                var targetFolder = fallbackFolders[0];

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    targetFolder,
                    saveMode,
                    new PackageIdentity("a", NuGetVersion.Parse("1.0.0")));

                var expected = Path.Combine(targetFolder, "a", "1.0.0");

                var resolver = new FallbackPackagePathResolver(userFolder, fallbackFolders);

                // Act
                var path = resolver.GetPackageDirectory("a", "1.0.0");

                // Assert
                Assert.Equal(expected, path);
            }
        }

        [Fact]
        public async Task FallbackPackagePathResolver_FindPackageInFallbackFolder2()
        {
            // Arrange
            using (var mockBaseDirectory = TestDirectory.Create())
            {
                var userFolder = Path.Combine(mockBaseDirectory, "global");
                var fallbackFolders = new List<string>()
                {
                    Path.Combine(mockBaseDirectory, "fallback1"),
                    Path.Combine(mockBaseDirectory, "fallback2"),
                };

                Directory.CreateDirectory(userFolder);

                foreach (var fallback in fallbackFolders)
                {
                    Directory.CreateDirectory(fallback);
                }

                var saveMode = PackageSaveMode.Nuspec | PackageSaveMode.Files;

                var targetFolder = fallbackFolders[1];

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    targetFolder,
                    saveMode,
                    new PackageIdentity("a", NuGetVersion.Parse("1.0.0")));

                var expected = Path.Combine(targetFolder, "a", "1.0.0");

                var resolver = new FallbackPackagePathResolver(userFolder, fallbackFolders);

                // Act
                var path = resolver.GetPackageDirectory("a", "1.0.0");

                // Assert
                Assert.Equal(expected, path);
            }
        }

        [Fact]
        public async Task FallbackPackagePathResolver_FindPackageInFallbackFolder2SkipMissingHashes()
        {
            // Arrange
            using (var mockBaseDirectory = TestDirectory.Create())
            {
                var userFolder = Path.Combine(mockBaseDirectory, "global");
                var fallbackFolders = new List<string>()
                {
                    Path.Combine(mockBaseDirectory, "fallback1"),
                    Path.Combine(mockBaseDirectory, "fallback2"),
                };

                Directory.CreateDirectory(userFolder);

                foreach (var fallback in fallbackFolders)
                {
                    Directory.CreateDirectory(fallback);
                }

                var saveMode = PackageSaveMode.Nuspec | PackageSaveMode.Files | PackageSaveMode.Nupkg;

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    userFolder,
                    saveMode,
                    new PackageIdentity("a", NuGetVersion.Parse("1.0.0")));

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    fallbackFolders[0],
                    saveMode,
                    new PackageIdentity("a", NuGetVersion.Parse("1.0.0")));

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    fallbackFolders[1],
                    saveMode,
                    new PackageIdentity("a", NuGetVersion.Parse("1.0.0")));

                // Remove hashes from the first two folders
                foreach (var root in new[] { userFolder, fallbackFolders[0] })
                {
                    var localResolver = new VersionFolderPathResolver(root);
                    File.Delete(localResolver.GetNupkgMetadataPath("a", NuGetVersion.Parse("1.0.0")));
                }

                var expected = Path.Combine(userFolder, "a", "1.0.0");

                var resolver = new FallbackPackagePathResolver(userFolder, fallbackFolders);

                // Act
                var path = resolver.GetPackageDirectory("a", "1.0.0");

                // Assert
                Assert.Equal(expected, path);
            }
        }

        [Fact]
        public async Task FallbackPackagePathResolver_FindPackageInFallbackFolder_WithOnlyHashFile()
        {
            // Arrange
            using (var mockBaseDirectory = TestDirectory.Create())
            {
                var userFolder = Path.Combine(mockBaseDirectory, "global");
                var fallbackFolders = new List<string>()
                {
                    Path.Combine(mockBaseDirectory, "fallback1"),
                    Path.Combine(mockBaseDirectory, "fallback2"),
                };

                Directory.CreateDirectory(userFolder);

                foreach (var fallback in fallbackFolders)
                {
                    Directory.CreateDirectory(fallback);
                }

                var saveMode = PackageSaveMode.Nuspec | PackageSaveMode.Files;

                var targetFolder = fallbackFolders[0];

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    targetFolder,
                    saveMode,
                    new PackageIdentity("a", NuGetVersion.Parse("1.0.0")));

                // delete .nupkg.metadata file
                var localResolver = new VersionFolderPathResolver(targetFolder);
                File.Delete(localResolver.GetNupkgMetadataPath("a", NuGetVersion.Parse("1.0.0")));

                var expected = Path.Combine(targetFolder, "a", "1.0.0");

                var resolver = new FallbackPackagePathResolver(userFolder, fallbackFolders);

                // Act
                var path = resolver.GetPackageDirectory("a", "1.0.0");

                // Assert
                Assert.Equal(expected, path);
            }
        }
    }
}
