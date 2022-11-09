// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.Protocol.Tests
{
    public class FeedTypeUtilityTests
    {
        [Theory]
        [InlineData("https://www.nuget.org/api/v2/")]
        [InlineData("https://www.nuget.org/api/v2")]
        [InlineData("http://www.nuget.org/api/v2/")]
        [InlineData("http://nuget.org")]
        [InlineData("http://")]
        [InlineData("http://nuget.org/index.xml")]
        [InlineData("http://nuget.org/index.json.html")]
        [InlineData("http://tempuri.org/api/v2/")]
        public void GetFeedType_WithV2HttpSources_ReturnsHttpV2(string source)
        {
            Assert.Equal(FeedType.HttpV2, FeedTypeUtility.GetFeedType(new PackageSource(source)));
        }

        [Theory]
        [InlineData("https://api.nuget.org/v3/index.json")]
        [InlineData("http://api.nuget.org/v3/index.json")]
        [InlineData("https://api.nuget.org/v3/INDEX.JSON")]
        [InlineData("https://pkgs.dev.azure.com/dnceng/public/_packaging/nuget-build/nuget/v3/index.json")]
        public void GetFeedType_WithV3HttpSources_ReturnsHttpV3(string source)
        {
            Assert.Equal(FeedType.HttpV3, FeedTypeUtility.GetFeedType(new PackageSource(source)));
        }

        [Fact]
        public void GetFeedType_WithUnknownSource_ReturnsFileSystemUnknown()
        {
            // Arrange & Act
            var type = FeedTypeUtility.GetFeedType(new PackageSource("\\blah"));

            // Assert
            Assert.Equal(FeedType.FileSystemUnknown, type);
        }

        [Theory]
        [InlineData("../foo/packages")]
        [InlineData(@"..\foo\packages")]
        [InlineData("packages")]
        public void GetFeedType_WithRelativePath_ReturnsFileSystemUnknown(string source)
        {
            // Arrange & Act
            var type = FeedTypeUtility.GetFeedType(new PackageSource(source));

            // Assert
            Assert.Equal(FeedType.FileSystemUnknown, type);
        }

        [Fact]
        public void GetFeedType_WithIllegalPathCharacters_ReturnsFileSystemUnknown()
        {
            // Arrange & Act
            var type = FeedTypeUtility.GetFeedType(new PackageSource("$|. \n\t"));

            // Assert
            Assert.Equal(FeedType.FileSystemUnknown, type);
        }

        [Fact]
        public void GetFeedType_WithEmptyDirectory_ReturnsFileSystemUnknown()
        {
            using (var root = TestDirectory.Create())
            {
                // Arrange & Act
                var type = FeedTypeUtility.GetFeedType(new PackageSource(root));

                // Assert
                Assert.Equal(FeedType.FileSystemUnknown, type);
            }
        }

        [Fact]
        public void GetFeedType_WithRandomFilesInRoot_ReturnsFileSystemUnknown()
        {
            using (var root = TestDirectory.Create())
            {
                // Arrange
                CreateFile(Path.Combine(root, "a.txt"));
                CreateFile(Path.Combine(root, "a", "a.txt"));
                CreateFile(Path.Combine(root, "a", "b", "a.txt"));

                // Act
                var type = FeedTypeUtility.GetFeedType(new PackageSource(root));

                // Assert
                Assert.Equal(FeedType.FileSystemUnknown, type);
            }
        }

        [Fact]
        public void GetFeedType_WithNupkgInInvalidLocation_ReturnsFileSystemUnknown()
        {
            using (var root = TestDirectory.Create())
            {
                // Arrange
                CreateFile(Path.Combine(root, "a", "b", "a.1.0.0.nupkg"));

                // Act
                var type = FeedTypeUtility.GetFeedType(new PackageSource(root));

                // Assert
                Assert.Equal(FeedType.FileSystemUnknown, type);
            }
        }

        [Fact]
        public void GetFeedType_WithFileSystemV2LayoutAndLocalFileSystemPath_ReturnsFileSystemV2()
        {
            using (var root = TestDirectory.Create())
            {
                // Arrange
                CreateFile(Path.Combine(root, "a.1.0.0.nupkg"));

                // Act
                var type = FeedTypeUtility.GetFeedType(new PackageSource(root));

                // Assert
                Assert.Equal(FeedType.FileSystemV2, type);
            }
        }

        [Fact]
        public void GetFeedType_WithFileSystemV2LayoutAndFileUri_ReturnsFileSystemV2()
        {
            using (var root = TestDirectory.Create())
            {
                // Arrange
                CreateFile(Path.Combine(root, "a.1.0.0.nupkg"));

                // Act
                var type = FeedTypeUtility.GetFeedType(new PackageSource(UriUtility.CreateSourceUri(root).AbsoluteUri));

                // Assert
                Assert.Equal(FeedType.FileSystemV2, type);
            }
        }

        [Fact]
        public void GetFeedType_WithFileSystemV3Layout_ReturnsFileSystemV3()
        {
            using (var root = TestDirectory.Create())
            {
                // Arrange
                CreateFile(Path.Combine(root, "a", "1.0.0", "a.1.0.0.nupkg.sha512"));
                CreateFile(Path.Combine(root, "a", "1.0.0", "a.nuspec"));
                CreateFile(Path.Combine(root, "a", "1.0.0", "a.1.0.0.nupkg"));

                // Act
                var type = FeedTypeUtility.GetFeedType(new PackageSource(root));

                // Assert
                Assert.Equal(FeedType.FileSystemV3, type);
            }
        }

        [Fact]
        public void GetFeedType_WithFileSystemV3LayoutAndFileUri_ReturnsFileSystemV3()
        {
            using (var root = TestDirectory.Create())
            {
                // Arrange
                CreateFile(Path.Combine(root, "a", "1.0.0", "a.1.0.0.nupkg.sha512"));
                CreateFile(Path.Combine(root, "a", "1.0.0", "a.nuspec"));
                CreateFile(Path.Combine(root, "a", "1.0.0", "a.1.0.0.nupkg"));

                // Act
                var type = FeedTypeUtility.GetFeedType(new PackageSource(UriUtility.CreateSourceUri(root).AbsoluteUri));

                // Assert
                Assert.Equal(FeedType.FileSystemV3, type);
            }
        }

        [Fact]
        public void GetFeedType_WithNupkgOnlyInVersionFolder_ReturnsFileSystemUnknown()
        {
            using (var root = TestDirectory.Create())
            {
                // Arrange
                CreateFile(Path.Combine(root, "a", "1.0.0", "a.1.0.0.nupkg"));

                // Act
                var type = FeedTypeUtility.GetFeedType(new PackageSource(root));

                // Assert
                Assert.Equal(FeedType.FileSystemUnknown, type);
            }
        }

        [Fact]
        public void GetFeedType_WithBothFileSystemV2AndV3Layouts_ReturnsFileSystemV2()
        {
            using (var root = TestDirectory.Create())
            {
                // Arrange
                CreateFile(Path.Combine(root, "a.1.0.0.nupkg"));
                CreateFile(Path.Combine(root, "a", "a.1.0.0.nupkg"));
                CreateFile(Path.Combine(root, "a", "1.0.0", "a.1.0.0.nupkg"));

                // Act
                var type = FeedTypeUtility.GetFeedType(new PackageSource(root));

                // Assert
                Assert.Equal(FeedType.FileSystemV2, type);
            }
        }

        [Fact]
        public void GetFeedType_WithFileSystemV3LayoutAndFeedTypePackageSource_ReturnsFeedTypePackageSourceFeedType()
        {
            using (var root = TestDirectory.Create())
            {
                CreateFile(Path.Combine(root, "a", "1.0.0", "a.1.0.0.nupkg.sha512"));
                CreateFile(Path.Combine(root, "a", "1.0.0", "a.nuspec"));
                CreateFile(Path.Combine(root, "a", "1.0.0", "a.1.0.0.nupkg"));

                var feedTypes = new[] { FeedType.FileSystemV2, FeedType.FileSystemV3, FeedType.FileSystemUnknown };

                foreach (var expectedFeedType in feedTypes)
                {
                    var packageSource = new FeedTypePackageSource(root, expectedFeedType);

                    var actualFeedType = FeedTypeUtility.GetFeedType(packageSource);

                    Assert.Equal(expectedFeedType, actualFeedType);
                }
            }
        }

        private void CreateFile(string path)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));

            File.WriteAllText(path, string.Empty);
        }
    }
}
