// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Packaging.Core;
using NuGet.Protocol.Core.Types;
using NuGet.Test.Utility;
using NuGet.Versioning;
using Xunit;

namespace NuGet.Protocol.Tests
{
    public class LocalFolderUtilityTests
    {
        // *.nupkg
        private static readonly string NupkgFilter = $"*{NuGetConstants.PackageExtension}";

        [Fact]
        public void LocalFolderUtility_GetAndVerifyRootDirectory_WithAbsoluteFileUri()
        {
            // Arrange
            using (var root = TestDirectory.Create())
            {
                // Act
                var uri = UriUtility.CreateSourceUri(root, UriKind.Absolute);
                var actual = LocalFolderUtility.GetAndVerifyRootDirectory(uri.AbsoluteUri);

                // Assert
                Assert.Equal(root.ToString(), actual.FullName);
                Assert.True(actual.Exists, "The root directory should exist.");
            }
        }

        [Fact]
        public void LocalFolderUtility_GetAndVerifyRootDirectory_WithAbsolute()
        {
            // Arrange
            using (var root = TestDirectory.Create())
            {
                // Act
                var actual = LocalFolderUtility.GetAndVerifyRootDirectory(root);

                // Assert
                Assert.Equal(root.ToString(), actual.FullName);
                Assert.True(actual.Exists, "The root directory should exist.");
            }
        }

        [Fact]
        public void LocalFolderUtility_GetAndVerifyRootDirectory_WithNonexistentAbsolute()
        {
            // Arrange
            using (var testFolder = TestDirectory.Create())
            {
                var root = Path.Combine(testFolder, "not-real");

                // Act
                var actual = LocalFolderUtility.GetAndVerifyRootDirectory(root);

                // Assert
                Assert.Equal(root.ToString(), actual.FullName);
                Assert.False(actual.Exists, "The root directory should not exist.");
            }
        }

        [Fact]
        public void LocalFolderUtility_GetAndVerifyRootDirectory_WithNonexistentRelative()
        {
            // Arrange
            var workingDirectory = Directory.GetCurrentDirectory();
            var subdirectory = Guid.NewGuid().ToString();
            var root = Path.Combine("..", subdirectory);
            var expected = Path.Combine(Path.GetDirectoryName(workingDirectory), subdirectory);

            // Act
            var actual = LocalFolderUtility.GetAndVerifyRootDirectory(root);

            // Assert
            Assert.Equal(expected, actual.FullName);
            Assert.False(actual.Exists, "The root directory should not exist.");
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("X:Windows")]
        [InlineData("http://nuget.org")]
        public void LocalFolderUtility_GetAndVerifyRootDirectory_RejectsInvalid(string source)
        {
            // Arrange & Act & Assert
            var ex = Assert.Throws<FatalProtocolException>(() =>
                LocalFolderUtility.GetAndVerifyRootDirectory(source));
            Assert.Equal(
                $"Failed to verify the root directory of local source '{source}'.",
                ex.Message);
        }

        [Fact]
        public async Task LocalFolderUtility_GetPackagesConfigFolderPackages_AllAsync()
        {
            using (var root = TestDirectory.Create())
            {
                // Arrange
                var testLogger = new TestLogger();
                var a = new PackageIdentity("a", NuGetVersion.Parse("1.0.0"));
                var a2 = new PackageIdentity("a", NuGetVersion.Parse("1.0.0-beta"));
                var b = new PackageIdentity("b", NuGetVersion.Parse("1.0.0"));
                var c = new PackageIdentity("c", NuGetVersion.Parse("1.0.0"));

                await SimpleTestPackageUtility.CreateFolderFeedPackagesConfigAsync(root, a);
                await SimpleTestPackageUtility.CreateFolderFeedPackagesConfigAsync(root, a2);
                await SimpleTestPackageUtility.CreateFolderFeedPackagesConfigAsync(root, b);
                await SimpleTestPackageUtility.CreateFolderFeedPackagesConfigAsync(root, c);

                // Act
                var packages = LocalFolderUtility.GetPackagesConfigFolderPackages(root, testLogger)
                    .OrderBy(package => package.Identity.Id)
                    .ThenBy(package => package.Identity.Version)
                    .ToList();

                // Assert
                Assert.Equal(4, packages.Count);
                Assert.Equal("a.1.0.0-beta", packages[0].Identity.ToString());
                Assert.Equal("a.1.0.0", packages[1].Identity.ToString());
                Assert.Equal("b.1.0.0", packages[2].Identity.ToString());
                Assert.Equal("c.1.0.0", packages[3].Identity.ToString());
                Assert.Equal(0, testLogger.Messages.Count);
            }
        }

        [Fact]
        public async Task LocalFolderUtility_GetPackagesConfigFolderPackages_ByIdAsync()
        {
            using (var root = TestDirectory.Create())
            {
                // Arrange
                var testLogger = new TestLogger();
                var a = new PackageIdentity("a", NuGetVersion.Parse("1.0.0"));
                var a2 = new PackageIdentity("a", NuGetVersion.Parse("1.0.0-beta"));
                var b = new PackageIdentity("b", NuGetVersion.Parse("1.0.0"));
                var c = new PackageIdentity("c", NuGetVersion.Parse("1.0.0"));

                await SimpleTestPackageUtility.CreateFolderFeedPackagesConfigAsync(root, a);
                await SimpleTestPackageUtility.CreateFolderFeedPackagesConfigAsync(root, a2);
                await SimpleTestPackageUtility.CreateFolderFeedPackagesConfigAsync(root, b);
                await SimpleTestPackageUtility.CreateFolderFeedPackagesConfigAsync(root, c);

                // Act
                var packages = LocalFolderUtility.GetPackagesConfigFolderPackages(root, "a", testLogger)
                    .OrderBy(package => package.Identity.Id)
                    .ThenBy(package => package.Identity.Version)
                    .ToList();

                // Assert
                Assert.Equal(2, packages.Count);
                Assert.Equal("a.1.0.0-beta", packages[0].Identity.ToString());
                Assert.Equal("a.1.0.0", packages[1].Identity.ToString());
                Assert.Equal(0, testLogger.Messages.Count);
            }
        }

        [Fact]
        public async Task LocalFolderUtility_GetPackagesConfigFolderPackages_ById_NotFoundAsync()
        {
            using (var root = TestDirectory.Create())
            {
                // Arrange
                var testLogger = new TestLogger();
                var a = new PackageIdentity("a", NuGetVersion.Parse("1.0.0"));
                var a2 = new PackageIdentity("a", NuGetVersion.Parse("1.0.0-beta"));
                var b = new PackageIdentity("b", NuGetVersion.Parse("1.0.0"));
                var c = new PackageIdentity("c", NuGetVersion.Parse("1.0.0"));

                await SimpleTestPackageUtility.CreateFolderFeedPackagesConfigAsync(root, a);
                await SimpleTestPackageUtility.CreateFolderFeedPackagesConfigAsync(root, a2);
                await SimpleTestPackageUtility.CreateFolderFeedPackagesConfigAsync(root, b);
                await SimpleTestPackageUtility.CreateFolderFeedPackagesConfigAsync(root, c);

                // Act
                var packages = LocalFolderUtility.GetPackagesConfigFolderPackages(root, "z", testLogger)
                    .OrderBy(package => package.Identity.Id)
                    .ThenBy(package => package.Identity.Version)
                    .ToList();

                // Assert
                Assert.Equal(0, packages.Count);
                Assert.Equal(0, testLogger.Messages.Count);
            }
        }

        [Fact]
        public async Task LocalFolderUtility_GetPackagesConfigFolderPackageAsync()
        {
            using (var root = TestDirectory.Create())
            {
                // Arrange
                var testLogger = new TestLogger();
                var a = new PackageIdentity("a", NuGetVersion.Parse("1.0.0"));
                var b = new PackageIdentity("b", NuGetVersion.Parse("1.0.0"));
                var c = new PackageIdentity("c", NuGetVersion.Parse("1.0.0"));

                await SimpleTestPackageUtility.CreateFolderFeedPackagesConfigAsync(root, a);
                await SimpleTestPackageUtility.CreateFolderFeedPackagesConfigAsync(root, b);
                await SimpleTestPackageUtility.CreateFolderFeedPackagesConfigAsync(root, c);

                // Act
                var foundA = LocalFolderUtility.GetPackagesConfigFolderPackage(root, a, testLogger);

                // Assert
                Assert.Equal(a, foundA.Identity);
                Assert.Equal(a, foundA.Nuspec.GetIdentity());
                Assert.True(foundA.IsNupkg);
                using (var reader = foundA.GetReader())
                {
                    Assert.Equal(a, reader.GetIdentity());
                }
                Assert.Contains("a.1.0.0.nupkg", foundA.Path);
                Assert.Equal(0, testLogger.Messages.Count);
            }
        }

        [Fact]
        public async Task LocalFolderUtility_GetPackagesConfigFolderPackage_MissingAsync()
        {
            using (var root = TestDirectory.Create())
            {
                // Arrange
                var testLogger = new TestLogger();
                var a = new PackageIdentity("a", NuGetVersion.Parse("1.0.0"));
                var b = new PackageIdentity("b", NuGetVersion.Parse("1.0.0"));
                var c = new PackageIdentity("c", NuGetVersion.Parse("1.0.0"));

                await SimpleTestPackageUtility.CreateFolderFeedPackagesConfigAsync(root, b);
                await SimpleTestPackageUtility.CreateFolderFeedPackagesConfigAsync(root, c);

                // Act
                var foundA = LocalFolderUtility.GetPackagesConfigFolderPackage(root, a, testLogger);

                // Assert
                Assert.Null(foundA);
            }
        }

        [Fact]
        public void LocalFolderUtility_GetPackagesConfigFolderPackage_LongPackageIdDoesNotThrowAsync()
        {
            using (var root = TestDirectory.Create())
            {
                // Arrange
                var testLogger = new TestLogger();
                var id = "aa";

                for (var i = 0; i < 200; i++)
                {
                    id += "aa";
                }

                var a = new PackageIdentity(id, NuGetVersion.Parse("1.0.0"));

                // Act
                var foundA = LocalFolderUtility.GetPackagesConfigFolderPackage(root, a, testLogger);

                // Assert
                Assert.Null(foundA);
            }
        }

        [Fact]
        public async Task LocalFolderUtility_GetPackagesConfigFolderPackage_NonNormalizedInFolderAsync()
        {
            using (var root = TestDirectory.Create())
            {
                // Arrange
                var testLogger = new TestLogger();
                var a = new PackageIdentity("a", NuGetVersion.Parse("1.0.1"));
                var aNonNormalized = new PackageIdentity("a", NuGetVersion.Parse("1.0.01.0"));
                var b = new PackageIdentity("b", NuGetVersion.Parse("1.0.0"));
                var c = new PackageIdentity("c", NuGetVersion.Parse("1.0.0"));

                await SimpleTestPackageUtility.CreateFolderFeedPackagesConfigAsync(root, aNonNormalized);
                await SimpleTestPackageUtility.CreateFolderFeedPackagesConfigAsync(root, b);
                await SimpleTestPackageUtility.CreateFolderFeedPackagesConfigAsync(root, c);

                // Act
                var foundA = LocalFolderUtility.GetPackagesConfigFolderPackage(root, a, testLogger);

                // Assert
                Assert.Equal(a, foundA.Identity);
                Assert.Equal(a, foundA.Nuspec.GetIdentity());
                Assert.True(foundA.IsNupkg);
                using (var reader = foundA.GetReader())
                {
                    Assert.Equal(a, reader.GetIdentity());
                }
                Assert.Contains("a.1.0.01.0.nupkg", foundA.Path);
            }
        }

        [Fact]
        public async Task LocalFolderUtility_GetPackagesConfigFolderPackage_NonNormalizedInRequestAsync()
        {
            using (var root = TestDirectory.Create())
            {
                // Arrange
                var testLogger = new TestLogger();
                var a = new PackageIdentity("a", NuGetVersion.Parse("1.0.1"));
                var aNonNormalized = new PackageIdentity("a", NuGetVersion.Parse("1.0.01.0"));
                var b = new PackageIdentity("b", NuGetVersion.Parse("1.0.0"));
                var c = new PackageIdentity("c", NuGetVersion.Parse("1.0.0"));

                await SimpleTestPackageUtility.CreateFolderFeedPackagesConfigAsync(root, a);
                await SimpleTestPackageUtility.CreateFolderFeedPackagesConfigAsync(root, b);
                await SimpleTestPackageUtility.CreateFolderFeedPackagesConfigAsync(root, c);

                // Act
                var foundA = LocalFolderUtility.GetPackagesConfigFolderPackage(root, aNonNormalized, testLogger);

                // Assert
                Assert.Equal(a, foundA.Identity);
                Assert.Equal(a, foundA.Nuspec.GetIdentity());
                Assert.True(foundA.IsNupkg);
                using (var reader = foundA.GetReader())
                {
                    Assert.Equal(a, reader.GetIdentity());
                }
                Assert.Contains("a.1.0.1.nupkg", foundA.Path);
            }
        }

        [Fact]
        public async Task LocalFolderUtility_GetPackagesConfigFolderPackage_ConflictAndMissingAsync()
        {
            using (var root = TestDirectory.Create())
            {
                // Arrange
                var testLogger = new TestLogger();
                var a = new PackageIdentity("a", NuGetVersion.Parse("1.0.0"));
                var b = new PackageIdentity("b", NuGetVersion.Parse("1.0.0"));
                var c = new PackageIdentity("c", NuGetVersion.Parse("1.0.0"));

                var a2 = new PackageIdentity("a.1", NuGetVersion.Parse("0.0"));

                await SimpleTestPackageUtility.CreateFolderFeedPackagesConfigAsync(root, a2);
                await SimpleTestPackageUtility.CreateFolderFeedPackagesConfigAsync(root, b);
                await SimpleTestPackageUtility.CreateFolderFeedPackagesConfigAsync(root, c);

                // Act
                var foundA = LocalFolderUtility.GetPackagesConfigFolderPackage(root, a, testLogger);

                // Assert
                Assert.Null(foundA);
            }
        }

        [Fact]
        public async Task LocalFolderUtility_GetPackagesConfigFolderPackage_MissingNupkgsAsync()
        {
            using (var root = TestDirectory.Create())
            {
                // Arrange
                var testLogger = new TestLogger();
                var a = new PackageIdentity("a", NuGetVersion.Parse("1.0.0"));
                var b = new PackageIdentity("b", NuGetVersion.Parse("1.0.0"));
                var c = new PackageIdentity("c", NuGetVersion.Parse("1.0.0"));

                await SimpleTestPackageUtility.CreateFolderFeedPackagesConfigAsync(root, a);
                await SimpleTestPackageUtility.CreateFolderFeedPackagesConfigAsync(root, b);
                await SimpleTestPackageUtility.CreateFolderFeedPackagesConfigAsync(root, c);

                foreach (var file in Directory.GetFiles(root, "*.nupkg", SearchOption.AllDirectories))
                {
                    File.Delete(file);
                }

                // Act
                var foundA = LocalFolderUtility.GetPackagesConfigFolderPackage(root, a, testLogger);

                // Assert
                Assert.Null(foundA);
            }
        }

        [Fact]
        public async Task LocalFolderUtility_GetPackagesConfigFolderPackage_IgnoreNupkgInWrongFolderAsync()
        {
            using (var root = TestDirectory.Create())
            {
                // Arrange
                var testLogger = new TestLogger();
                var a = new PackageIdentity("a", NuGetVersion.Parse("1.0.0"));
                var b = new PackageIdentity("b", NuGetVersion.Parse("1.0.0"));
                var c = new PackageIdentity("c", NuGetVersion.Parse("1.0.0"));

                await SimpleTestPackageUtility.CreateFolderFeedPackagesConfigAsync(root, a);
                await SimpleTestPackageUtility.CreateFolderFeedPackagesConfigAsync(root, b);
                await SimpleTestPackageUtility.CreateFolderFeedPackagesConfigAsync(root, c);

                foreach (var file in Directory.GetFiles(root, "a.1.0.0.nupkg", SearchOption.AllDirectories))
                {
                    File.Delete(file);
                }

                await SimpleTestPackageUtility.CreateFolderFeedV2Async(root, a);
                await SimpleTestPackageUtility.CreateFolderFeedV2Async(Path.Combine(root, "b.1.0.0"), a);

                // Act
                var foundA = LocalFolderUtility.GetPackagesConfigFolderPackage(root, a, testLogger);

                // Assert
                Assert.Null(foundA);
            }
        }

        [Fact]
        public void LocalFolderUtility_GetPackagesConfigFolderPackage_EmptyDir()
        {
            using (var root = TestDirectory.Create())
            {
                // Arrange
                var testLogger = new TestLogger();
                var a = new PackageIdentity("a", NuGetVersion.Parse("1.0.0"));
                Directory.Delete(root);

                // Act
                var foundA = LocalFolderUtility.GetPackagesConfigFolderPackage(root, a, testLogger);

                // Assert
                Assert.Null(foundA);
                Assert.Equal(0, testLogger.Messages.Count);
            }
        }

        [Fact]
        public void LocalFolderUtility_GetPackagesConfigFolderPackages_EmptyDir()
        {
            using (var root = TestDirectory.Create())
            {
                // Arrange
                var testLogger = new TestLogger();
                Directory.Delete(root);

                // Act
                var packages = LocalFolderUtility.GetPackagesConfigFolderPackages(root, testLogger).ToList();

                // Assert
                Assert.Equal(0, packages.Count);
                Assert.Equal(0, testLogger.Messages.Count);
            }
        }

        [Fact]
        public void LocalFolderUtility_GetPackagesConfigFolderPackagesWithId_EmptyDir()
        {
            using (var root = TestDirectory.Create())
            {
                // Arrange
                var testLogger = new TestLogger();
                Directory.Delete(root);

                // Act
                var packages = LocalFolderUtility.GetPackagesConfigFolderPackages(root, "a", testLogger).ToList();

                // Assert
                Assert.Equal(0, packages.Count);
                Assert.Equal(0, testLogger.Messages.Count);
            }
        }

#if IS_DESKTOP
        // TODO: To work on coreclr we need to address https://github.com/NuGet/Home/issues/7588

        [Fact]
        public void LocalFolderUtility_GetPackagesV3MaxPathTest()
        {
            using (var root = TestDirectory.Create())
            {
                // Arrange
                var testLogger = new TestLogger();
                var longString = string.Empty;

                for (var i = 0; i < 1000; i++)
                {
                    longString += "abcdef";
                }

                var path = root + Path.DirectorySeparatorChar + longString;
                Exception actual = null;

                // Act
                try
                {
                    var packages = LocalFolderUtility.GetPackagesV3(path, testLogger).ToList();
                }
                catch (Exception ex)
                {
                    actual = ex;
                }

                // Assert
                Assert.True(actual is FatalProtocolException);
            }
        }

        [Fact]
        public void LocalFolderUtility_GetPackagesV2MaxPathTest()
        {
            using (var root = TestDirectory.Create())
            {
                // Arrange
                var testLogger = new TestLogger();
                var longString = string.Empty;

                for (var i = 0; i < 1000; i++)
                {
                    longString += "abcdef";
                }

                var path = root + Path.DirectorySeparatorChar + longString;

                Exception actual = null;

                // Act
                try
                {
                    var packages = LocalFolderUtility.GetPackagesV2(path, testLogger).ToList();
                }
                catch (Exception ex)
                {
                    actual = ex;
                }

                // Assert
                Assert.True(actual is FatalProtocolException);
            }
        }

        [Fact]
        public void LocalFolderUtility_GetPackageV3MaxPathTest()
        {
            using (var root = TestDirectory.Create())
            {
                // Arrange
                var testLogger = new TestLogger();
                var longString = string.Empty;

                for (var i = 0; i < 1000; i++)
                {
                    longString += "abcdef";
                }

                var path = root + Path.DirectorySeparatorChar + longString;
                Exception actual = null;

                // Act
                try
                {
                    var package = LocalFolderUtility.GetPackageV3(path, "A", NuGetVersion.Parse("1.0.0"), testLogger);
                }
                catch (Exception ex)
                {
                    actual = ex;
                }

                // Assert
                Assert.True(actual is FatalProtocolException);
            }
        }

        [Fact]
        public void LocalFolderUtility_GetPackageV2MaxPathTest()
        {
            using (var root = TestDirectory.Create())
            {
                // Arrange
                var testLogger = new TestLogger();
                var longString = string.Empty;

                for (var i = 0; i < 1000; i++)
                {
                    longString += "abcdef";
                }

                var path = root + Path.DirectorySeparatorChar + longString;
                Exception actual = null;

                // Act
                try
                {
                    var package = LocalFolderUtility.GetPackageV2(path, "a", NuGetVersion.Parse("1.0.0"), testLogger);
                }
                catch (Exception ex)
                {
                    actual = ex;
                }

                // Assert
                Assert.True(actual is FatalProtocolException);
            }
        }
#endif

        [Fact]
        public async Task LocalFolderUtility_GetPackagesV2ValidPackageAsync()
        {
            using (var root = TestDirectory.Create())
            {
                // Arrange
                var testLogger = new TestLogger();
                await SimpleTestPackageUtility.CreateFolderFeedV2Async(root, new PackageIdentity("a", NuGetVersion.Parse("1.0.0")));
                await SimpleTestPackageUtility.CreateFolderFeedV2Async(root, new PackageIdentity("b", NuGetVersion.Parse("1.0.0")));
                await SimpleTestPackageUtility.CreateFolderFeedV2Async(root, new PackageIdentity("c", NuGetVersion.Parse("1.0.0")));

                // Act
                var packages = LocalFolderUtility.GetPackagesV2(root, testLogger)
                    .OrderBy(p => p.Identity.Id, StringComparer.OrdinalIgnoreCase)
                    .ThenBy(p => p.Identity.Version)
                    .ToList();

                // Assert
                Assert.Equal(3, packages.Count);
                Assert.Equal(new PackageIdentity("a", NuGetVersion.Parse("1.0.0")), packages[0].Identity);
                Assert.Equal(new PackageIdentity("b", NuGetVersion.Parse("1.0.0")), packages[1].Identity);
                Assert.Equal(new PackageIdentity("c", NuGetVersion.Parse("1.0.0")), packages[2].Identity);
            }
        }

        [Fact]
        public async Task LocalFolderUtility_GetPackagesV2WithCancellationToken_Cancelled_Fails()
        {
            // Arrange
            using var pathContext = new SimpleTestPathContext();
            var testLogger = new TestLogger();
            await SimpleTestPackageUtility.CreateFolderFeedV2Async(pathContext.PackageSource, new PackageIdentity("a", NuGetVersion.Parse("1.0.0")));
            var source = new CancellationTokenSource();
            CancellationToken token = source.Token;
            source.Cancel();

            // Act & Assert
            Assert.Throws<OperationCanceledException>(() => LocalFolderUtility.GetPackagesV2(pathContext.PackageSource, testLogger, token));
        }

        [Fact]
        public async Task LocalFolderUtility_GetPackagesV2ReadWithV3Async()
        {
            using (var root = TestDirectory.Create())
            {
                // Arrange
                var testLogger = new TestLogger();
                await SimpleTestPackageUtility.CreateFolderFeedV2Async(root, new PackageIdentity("a", NuGetVersion.Parse("1.0.0")));
                await SimpleTestPackageUtility.CreateFolderFeedV2Async(root, new PackageIdentity("b", NuGetVersion.Parse("1.0.0")));
                await SimpleTestPackageUtility.CreateFolderFeedV2Async(root, new PackageIdentity("c", NuGetVersion.Parse("1.0.0")));

                // Act
                var packages = LocalFolderUtility.GetPackagesV3(root, testLogger);

                // Assert
                Assert.Equal(0, packages.Count());
                Assert.Equal(0, testLogger.Messages.Count);
            }
        }

        [Fact]
        public async Task LocalFolderUtility_GetPackagesV3ValidPackageAsync()
        {
            using (var root = TestDirectory.Create())
            {
                // Arrange
                var testLogger = new TestLogger();
                await SimpleTestPackageUtility.CreateFolderFeedV3Async(root, new PackageIdentity("A", NuGetVersion.Parse("1.0.0")));
                await SimpleTestPackageUtility.CreateFolderFeedV3Async(root, new PackageIdentity("B", NuGetVersion.Parse("1.0.0")));
                await SimpleTestPackageUtility.CreateFolderFeedV3Async(root, new PackageIdentity("C", NuGetVersion.Parse("1.0.0")));

                // Act
                var packages = LocalFolderUtility.GetPackagesV3(root, testLogger)
                    .OrderBy(p => p.Identity.Id, StringComparer.OrdinalIgnoreCase)
                    .ThenBy(p => p.Identity.Version)
                    .ToList();

                // Assert
                Assert.Equal(3, packages.Count);
                Assert.Equal(new PackageIdentity("a", NuGetVersion.Parse("1.0.0")), packages[0].Identity);
                Assert.Equal(new PackageIdentity("b", NuGetVersion.Parse("1.0.0")), packages[1].Identity);
                Assert.Equal(new PackageIdentity("c", NuGetVersion.Parse("1.0.0")), packages[2].Identity);
            }
        }

        [Fact]
        public async Task LocalFolderUtility_GetPackagesV3ReadWithV2Async()
        {
            using (var root = TestDirectory.Create())
            {
                // Arrange
                var testLogger = new TestLogger();
                await SimpleTestPackageUtility.CreateFolderFeedV3Async(root, new PackageIdentity("a", NuGetVersion.Parse("1.0.0")));
                await SimpleTestPackageUtility.CreateFolderFeedV3Async(root, new PackageIdentity("b", NuGetVersion.Parse("1.0.0")));
                await SimpleTestPackageUtility.CreateFolderFeedV3Async(root, new PackageIdentity("c", NuGetVersion.Parse("1.0.0")));

                // Act
                var packages = LocalFolderUtility.GetPackagesV2(root, testLogger);

                // Assert
                Assert.Equal(0, packages.Count());
                Assert.Equal(0, testLogger.Messages.Count);
            }
        }

        [Fact]
        public async Task LocalFolderUtility_GetPackageV2Async()
        {
            using (var root = TestDirectory.Create())
            {
                // Arrange
                var testLogger = new TestLogger();
                var a = new PackageIdentity("a", NuGetVersion.Parse("1.0.0"));
                var b = new PackageIdentity("b", NuGetVersion.Parse("1.0.0"));
                var c = new PackageIdentity("c", NuGetVersion.Parse("1.0.0"));

                await SimpleTestPackageUtility.CreateFolderFeedV2Async(root, a);
                await SimpleTestPackageUtility.CreateFolderFeedV2Async(root, b);
                await SimpleTestPackageUtility.CreateFolderFeedV2Async(root, c);

                // Act
                var foundA = LocalFolderUtility.GetPackageV2(root, a, testLogger);

                // Assert
                Assert.Equal(a, foundA.Identity);
                Assert.Equal(a, foundA.Nuspec.GetIdentity());
                Assert.True(foundA.IsNupkg);
                using (var reader = foundA.GetReader())
                {
                    Assert.Equal(a, reader.GetIdentity());
                }
                Assert.Contains("a.1.0.0.nupkg", foundA.Path);
            }
        }

        [Fact]
        public async Task LocalFolderUtility_GetPackageV2NotFoundAsync()
        {
            using (var root = TestDirectory.Create())
            {
                // Arrange
                var testLogger = new TestLogger();
                var a = new PackageIdentity("a", NuGetVersion.Parse("1.0.0"));
                var b = new PackageIdentity("b", NuGetVersion.Parse("1.0.0"));
                var c = new PackageIdentity("c", NuGetVersion.Parse("1.0.0"));

                await SimpleTestPackageUtility.CreateFolderFeedV2Async(root, b);
                await SimpleTestPackageUtility.CreateFolderFeedV2Async(root, c);

                // Act
                var foundA = LocalFolderUtility.GetPackageV2(root, a, testLogger);

                // Assert
                Assert.Null(foundA);
            }
        }

        [Fact]
        public void LocalFolderUtility_GetPackageV2NotFoundEmptyDir()
        {
            using (var root = TestDirectory.Create())
            {
                // Arrange
                var testLogger = new TestLogger();
                var a = new PackageIdentity("a", NuGetVersion.Parse("1.0.0"));

                // Act
                var foundA = LocalFolderUtility.GetPackageV2(root, a, testLogger);

                // Assert
                Assert.Null(foundA);
            }
        }

        [Fact]
        public void LocalFolderUtility_GetPackageV2NotFoundMissingDir()
        {
            using (var root = TestDirectory.Create())
            {
                // Arrange
                var testLogger = new TestLogger();
                var a = new PackageIdentity("a", NuGetVersion.Parse("1.0.0"));

                // Act
                var foundA = LocalFolderUtility.GetPackageV2(Path.Combine(root, "missing"), a, testLogger);

                // Assert
                Assert.Null(foundA);
            }
        }

        [Fact]
        public async Task LocalFolderUtility_GetPackageV2NonNormalizedVersionsAsync()
        {
            using (var root = TestDirectory.Create())
            {
                // Arrange
                var testLogger = new TestLogger();
                var a1 = new PackageIdentity("a", NuGetVersion.Parse("1.0"));
                var a2 = new PackageIdentity("a", NuGetVersion.Parse("1.0.0"));
                var a3 = new PackageIdentity("a", NuGetVersion.Parse("1.0.0.0"));
                var a4 = new PackageIdentity("a", NuGetVersion.Parse("1.0.00"));

                await SimpleTestPackageUtility.CreateFolderFeedV2Async(root, a1);
                await SimpleTestPackageUtility.CreateFolderFeedV2Async(root, a2);
                await SimpleTestPackageUtility.CreateFolderFeedV2Async(root, a3);
                await SimpleTestPackageUtility.CreateFolderFeedV2Async(root, a4);

                // Act
                var foundA1 = LocalFolderUtility.GetPackageV2(root, a1, testLogger);
                var foundA2 = LocalFolderUtility.GetPackageV2(root, a2, testLogger);
                var foundA3 = LocalFolderUtility.GetPackageV2(root, a3, testLogger);
                var foundA4 = LocalFolderUtility.GetPackageV2(root, a4, testLogger);

                // Assert
                Assert.Equal("1.0", foundA1.Nuspec.GetVersion().ToString());
                Assert.Equal("1.0.0", foundA2.Nuspec.GetVersion().ToString());
                Assert.Equal("1.0.0.0", foundA3.Nuspec.GetVersion().ToString());
                Assert.Equal("1.0.00", foundA4.Nuspec.GetVersion().ToString());
            }
        }

        [Fact]
        public void LocalFolderUtility_GetPackagesV2NotFoundEmptyDir()
        {
            using (var root = TestDirectory.Create())
            {
                // Arrange
                var testLogger = new TestLogger();

                // Act
                var packages = LocalFolderUtility.GetPackagesV2(root, testLogger).ToList();

                // Assert
                Assert.Equal(0, packages.Count);
                Assert.Equal(0, testLogger.Messages.Count);
            }
        }

        [Fact]
        public void LocalFolderUtility_GetPackagesV2NotFoundMissingDir()
        {
            using (var root = TestDirectory.Create())
            {
                // Arrange
                var testLogger = new TestLogger();

                // Act
                var packages = LocalFolderUtility.GetPackagesV2(Path.Combine(root, "missing"), testLogger).ToList();

                // Assert
                Assert.Equal(0, packages.Count);
                Assert.Equal(0, testLogger.Messages.Count);
            }
        }

        [Fact]
        public void LocalFolderUtility_GetPackagesByIdV2NotFoundEmptyDir()
        {
            using (var root = TestDirectory.Create())
            {
                // Arrange
                var testLogger = new TestLogger();

                // Act
                var packages = LocalFolderUtility.GetPackagesV2(root, "a", testLogger).ToList();

                // Assert
                Assert.Equal(0, packages.Count);
                Assert.Equal(0, testLogger.Messages.Count);
            }
        }

        [Fact]
        public void LocalFolderUtility_GetPackagesByIdV2NotFoundMissingDir()
        {
            using (var root = TestDirectory.Create())
            {
                // Arrange
                var testLogger = new TestLogger();

                // Act
                var packages = LocalFolderUtility.GetPackagesV2(root, "a", testLogger).ToList();

                // Assert
                Assert.Equal(0, packages.Count);
                Assert.Equal(0, testLogger.Messages.Count);
            }
        }

        [Fact]
        public async Task LocalFolderUtility_GetPackageV3Async()
        {
            using (var root = TestDirectory.Create())
            {
                // Arrange
                var testLogger = new TestLogger();
                var a = new PackageIdentity("A", NuGetVersion.Parse("1.0.0"));
                var b = new PackageIdentity("B", NuGetVersion.Parse("1.0.0"));
                var c = new PackageIdentity("C", NuGetVersion.Parse("1.0.0"));

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(root, a);
                await SimpleTestPackageUtility.CreateFolderFeedV3Async(root, b);
                await SimpleTestPackageUtility.CreateFolderFeedV3Async(root, c);

                // Act
                var foundA = LocalFolderUtility.GetPackageV3(root, a, testLogger);

                // Assert
                Assert.Equal(a, foundA.Identity);
                Assert.Equal(a, foundA.Nuspec.GetIdentity());
                Assert.True(foundA.IsNupkg);
                using (var reader = foundA.GetReader())
                {
                    Assert.Equal(a, reader.GetIdentity());
                }
                Assert.Contains("a.1.0.0.nupkg", foundA.Path);
            }
        }

        [Fact]
        public async Task LocalFolderUtility_GetPackageV3NotFoundAsync()
        {
            using (var root = TestDirectory.Create())
            {
                // Arrange
                var testLogger = new TestLogger();
                var a = new PackageIdentity("A", NuGetVersion.Parse("1.0.0"));
                var b = new PackageIdentity("B", NuGetVersion.Parse("1.0.0"));
                var c = new PackageIdentity("C", NuGetVersion.Parse("1.0.0"));

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(root, b);
                await SimpleTestPackageUtility.CreateFolderFeedV3Async(root, c);

                // Act
                var foundA = LocalFolderUtility.GetPackageV3(root, a, testLogger);

                // Assert
                Assert.Null(foundA);
            }
        }

        [Fact]
        public void LocalFolderUtility_GetPackageV3NotFoundEmptyDir()
        {
            using (var root = TestDirectory.Create())
            {
                // Arrange
                var testLogger = new TestLogger();
                var a = new PackageIdentity("A", NuGetVersion.Parse("1.0.0"));

                // Act
                var foundA = LocalFolderUtility.GetPackageV3(root, a, testLogger);

                // Assert
                Assert.Null(foundA);
            }
        }

        [Fact]
        public void LocalFolderUtility_GetPackageV3NotFoundMissingDir()
        {
            using (var root = TestDirectory.Create())
            {
                // Arrange
                var testLogger = new TestLogger();
                var a = new PackageIdentity("A", NuGetVersion.Parse("1.0.0"));

                // Act
                var foundA = LocalFolderUtility.GetPackageV3(Path.Combine(root, "missing"), a, testLogger);

                // Assert
                Assert.Null(foundA);
            }
        }

        [Fact]
        public void LocalFolderUtility_GetPackagesV3NotFoundEmptyDir()
        {
            using (var root = TestDirectory.Create())
            {
                // Arrange
                var testLogger = new TestLogger();

                // Act
                var packages = LocalFolderUtility.GetPackagesV3(root, testLogger).ToList();

                // Assert
                Assert.Equal(0, packages.Count);
                Assert.Equal(0, testLogger.Messages.Count);
            }
        }

        [Fact]
        public void LocalFolderUtility_GetPackagesV3NotFoundMissingDir()
        {
            using (var root = TestDirectory.Create())
            {
                // Arrange
                var testLogger = new TestLogger();

                // Act
                var packages = LocalFolderUtility.GetPackagesV3(Path.Combine(root, "missing"), testLogger).ToList();

                // Assert
                Assert.Equal(0, packages.Count);
                Assert.Equal(0, testLogger.Messages.Count);
            }
        }

        [Fact]
        public void LocalFolderUtility_GetPackagesByIdV3NotFoundEmptyDir()
        {
            using (var root = TestDirectory.Create())
            {
                // Arrange
                var testLogger = new TestLogger();

                // Act
                var packages = LocalFolderUtility.GetPackagesV3(root, "A", testLogger).ToList();

                // Assert
                Assert.Equal(0, packages.Count);
                Assert.Equal(0, testLogger.Messages.Count);
            }
        }

        [Fact]
        public void LocalFolderUtility_GetPackagesByIdV3NotFoundMissingDir()
        {
            using (var root = TestDirectory.Create())
            {
                // Arrange
                var testLogger = new TestLogger();

                // Act
                var packages = LocalFolderUtility.GetPackagesV3(root, "A", testLogger).ToList();

                // Assert
                Assert.Equal(0, packages.Count);
                Assert.Equal(0, testLogger.Messages.Count);
            }
        }

        [Fact]
        public void LocalFolderUtility_EnsurePackageFileExists_ThrowsWhenEmpty()
        {
            //Arrange
            List<string> emptyTestList = new List<string>();
            string packagePath = string.Empty;

            //Act
            var ex = Assert.Throws<ArgumentException>(() => LocalFolderUtility.EnsurePackageFileExists(packagePath, emptyTestList));

            //Assert
            //Expected an exception explaining that the file wasn't found in the list.
            string expectedError = string.Format(CultureInfo.CurrentCulture,
                                    Strings.UnableToFindFile,
                                    packagePath);

            Assert.Equal(expectedError, ex.Message);
        }

        [Fact]
        public void LocalFolderUtility_EnsurePackageFileExists_DoesNotThrowWhenExists()
        {
            //Arrange
            List<string> testList = new List<string>();
            testList.Add("existingFilePath1");
            string packagePath = string.Empty;

            //Act            
            LocalFolderUtility.EnsurePackageFileExists(packagePath, testList);

            //Assert
            //Expected no thrown Exception.
        }

        [Fact]
        public async Task LocalFolderUtility_ResolvePackageFromPath_EmptyWhenNotFound()
        {
            using (var root = TestDirectory.Create())
            {
                //Arrange
                string nonexistentPath = "nonexistentPath";
                var a = new PackageIdentity("a", NuGetVersion.Parse("1.0.0"));

                await SimpleTestPackageUtility.CreateFolderFeedPackagesConfigAsync(root, a);

                //Act
                var resolvedPaths = LocalFolderUtility.ResolvePackageFromPath(nonexistentPath);

                //Assert
                Assert.Equal(0, resolvedPaths.Count());
            }
        }

        public static IEnumerable<object[]> GetValidVersions()
        {
            foreach (var s in ValidVersions())
            {
                yield return new object[] { s };
            }
        }

        private static IEnumerable<string> ValidVersions()
        {
            yield return "0.0.0";
            yield return "1.0.0";
            yield return "0.0.1";
            yield return "1.0.0-BETA";
            yield return "1.0";
            yield return "1.0.0.0";
            yield return "1.0.1";
            yield return "1.0.01";
            yield return "00000001.000000000.0000000001";
            yield return "00000001.000000000.0000000001-beta";
            yield return "1.0.01-alpha";
            yield return "1.0.1-alpha.1.2.3";
            yield return "1.0.1-alpha.1.2.3+metadata";
            yield return "1.0.1-alpha.1.2.3+a.b.c.d";
            yield return "1.0.1+metadata";
            yield return "1.0.1+security.fix.ce38429";
            yield return "1.0.1-alpha.10.a";
            yield return "1.0.1--";
            yield return "1.0.1-a.really.long.version.release.label";
            yield return "1238234.198231.2924324.2343432";
            yield return "1238234.198231.2924324.2343432+final";
            yield return "00.00.00.00-alpha";
            yield return "0.0-alpha.1";
            yield return "9.9.9-9";
        }

        [Theory]
        [MemberData("GetValidVersions")]
        public async Task LocalFolderUtility_VerifyPackageCanBeFoundV2_NonNormalizedOnDiskAsync(string versionString)
        {
            using (var root = TestDirectory.Create())
            {
                // Arrange
                var testLogger = new TestLogger();
                var version = NuGetVersion.Parse(versionString);
                var normalizedVersion = NuGetVersion.Parse(NuGetVersion.Parse(versionString).ToNormalizedString());
                var identity = new PackageIdentity("a", version);

                await SimpleTestPackageUtility.CreateFolderFeedV2Async(root, identity);

                // Act
                var findPackage = LocalFolderUtility.GetPackageV2(root, "a", version, testLogger);
                var findPackageNormalized = LocalFolderUtility.GetPackageV2(root, "a", normalizedVersion, testLogger);
                var findById = LocalFolderUtility.GetPackagesV2(root, "a", testLogger).Single();
                var findAll = LocalFolderUtility.GetPackagesV2(root, testLogger).Single();

                // Assert
                Assert.Equal(identity, findPackage.Identity);
                Assert.Equal(identity, findPackageNormalized.Identity);
                Assert.Equal(identity, findById.Identity);
                Assert.Equal(identity, findAll.Identity);
            }
        }

        [Theory]
        [MemberData("GetValidVersions")]
        public async Task LocalFolderUtility_VerifyPackageCanBeFoundV2_NormalizedOnDiskAsync(string versionString)
        {
            using (var root = TestDirectory.Create())
            {
                // Arrange
                var testLogger = new TestLogger();
                var version = NuGetVersion.Parse(versionString);
                var normalizedVersion = NuGetVersion.Parse(NuGetVersion.Parse(versionString).ToNormalizedString());
                var identity = new PackageIdentity("a", version);
                var normalizedIdentity = new PackageIdentity("a", normalizedVersion);

                await SimpleTestPackageUtility.CreateFolderFeedV2Async(root, normalizedIdentity);

                // Act
                var findPackage = LocalFolderUtility.GetPackageV2(root, "a", version, testLogger);
                var findPackageNormalized = LocalFolderUtility.GetPackageV2(root, "a", normalizedVersion, testLogger);
                var findById = LocalFolderUtility.GetPackagesV2(root, "a", testLogger).Single();
                var findAll = LocalFolderUtility.GetPackagesV2(root, testLogger).Single();

                // Assert
                Assert.Equal(identity, findPackage.Identity);
                Assert.Equal(identity, findPackageNormalized.Identity);
                Assert.Equal(identity, findById.Identity);
                Assert.Equal(identity, findAll.Identity);
            }
        }

        [Theory]
        [MemberData("GetValidVersions")]
        public async Task LocalFolderUtility_VerifyPackageCanBeFoundV3_NonNormalizedOnDiskAsync(string versionString)
        {
            using (var root = TestDirectory.Create())
            {
                // Arrange
                var testLogger = new TestLogger();
                var version = NuGetVersion.Parse(versionString);
                var normalizedVersion = NuGetVersion.Parse(NuGetVersion.Parse(versionString).ToNormalizedString());
                var identity = new PackageIdentity("A", version);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(root, identity);

                // Act
                var findPackage = LocalFolderUtility.GetPackageV3(root, "A", version, testLogger);
                var findPackageNormalized = LocalFolderUtility.GetPackageV3(root, "A", normalizedVersion, testLogger);
                var findById = LocalFolderUtility.GetPackagesV3(root, "A", testLogger).Single();
                var findAll = LocalFolderUtility.GetPackagesV3(root, testLogger).Single();

                // Assert
                Assert.Equal(identity, findPackage.Identity);
                Assert.Equal(identity, findPackageNormalized.Identity);
                Assert.Equal(identity, findById.Identity);
                Assert.Equal(identity, findAll.Identity);
            }
        }

        [Theory]
        [MemberData("GetValidVersions")]
        public async Task LocalFolderUtility_VerifyPackageCanBeFoundV3_NormalizedOnDiskAsync(string versionString)
        {
            using (var root = TestDirectory.Create())
            {
                // Arrange
                var testLogger = new TestLogger();
                var version = NuGetVersion.Parse(versionString);
                var normalizedVersion = NuGetVersion.Parse(NuGetVersion.Parse(versionString).ToNormalizedString());
                var identity = new PackageIdentity("A", version);
                var normalizedIdentity = new PackageIdentity("A", normalizedVersion);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(root, normalizedIdentity);

                // Act
                var findPackage = LocalFolderUtility.GetPackageV3(root, "A", version, testLogger);
                var findPackageNormalized = LocalFolderUtility.GetPackageV3(root, "A", normalizedVersion, testLogger);
                var findById = LocalFolderUtility.GetPackagesV3(root, "A", testLogger).Single();
                var findAll = LocalFolderUtility.GetPackagesV3(root, testLogger).Single();

                // Assert
                Assert.Equal(identity, findPackage.Identity);
                Assert.Equal(identity, findPackageNormalized.Identity);
                Assert.Equal(identity, findById.Identity);
                Assert.Equal(identity, findAll.Identity);
            }
        }

        [Theory]
        [InlineData("packageA.1.0.0.nupkg", "packageA", "packageA.1.0.0")]
        [InlineData("packageA.1.0.nupkg", "packageA", "packageA.1.0")]
        [InlineData("packageA.1.0.0.0.nupkg", "packageA", "packageA.1.0.0.0")]
        [InlineData("packageA.1.0.0-alpha.nupkg", "packageA", "packageA.1.0.0-alpha")]
        [InlineData("packageA.1.0.0-alpha.1.2.3.nupkg", "packageA", "packageA.1.0.0-alpha.1.2.3")]
        [InlineData("packageA.1.0.0-alpha.1.2.3+a.b.c.nupkg", "packageA", "packageA.1.0.0-alpha.1.2.3")]
        [InlineData("packageA.1.0.01.nupkg", "packageA", "packageA.1.0.01")]
        [InlineData("packageA.0001.0.01.nupkg", "packageA", "packageA.0001.0.01")]
        [InlineData("packageA.1.1.1.nupkg", "packageA.1", "packageA.1.1.1")]
        [InlineData("packageA.1.1.1.1.nupkg", "packageA.1.1", "packageA.1.1.1.1")]
        [InlineData("packageA.01.1.1.nupkg", "packageA.01", "packageA.01.1.1")]
        [InlineData("packageA..1.1.nupkg", "packageA.", "packageA..1.1")]
        [InlineData("a.1.0.nupkg", "a", "a.1.0")]
        public void LocalFolderUtility_GetIdentityFromFile_Valid(string fileName, string id, string expected)
        {
            // Arrange
            var file = new FileInfo(Path.Combine(Directory.GetCurrentDirectory(), fileName));

            // Act
            var identity = LocalFolderUtility.GetIdentityFromNupkgPath(file, id);

            // Assert
            Assert.Equal(id, identity.Id);
            Assert.Equal(expected, $"{identity.Id}.{identity.Version.ToString()}");
        }

        [Theory]
        [InlineData("packageA.x.nupkg", "packageA")]
        [InlineData("packageA.nupkg", "packageA")]
        [InlineData("packageA.nupkg", "packageB")]
        [InlineData("packageA.nupkg", "packageAAA")]
        [InlineData("packageA.1.0.0.0.0.nupkg", "packageA")]
        [InlineData("packageA.1.0.0-beta-#.nupkg", "packageA")]
        [InlineData("packageA.1.0.0-beta.1.01.nupkg", "packageA")]
        [InlineData("packageA.1.0.0-beta+a+b.nupkg", "packageA")]
        [InlineData("1", "packageA")]
        [InlineData("1.nupkg", "packageA")]
        [InlineData("packageB.1.0.0.nupkg", "packageA")]
        [InlineData("packageB.1.0.0.nupkg", "packageB1.0.0.0")]
        [InlineData("packageB.1.0.0.nuspec", "packageB")]
        [InlineData("file", "packageB")]
        [InlineData("file.txt", "packageB")]
        [InlineData("packageA.1.0.0.symbols.nupkg", "packageA")]
        public void LocalFolderUtility_GetIdentityFromFile_Invalid(string fileName, string id)
        {
            // Arrange
            var file = new FileInfo(Path.Combine(Directory.GetCurrentDirectory(), fileName));

            // Act
            var identity = LocalFolderUtility.GetIdentityFromNupkgPath(file, id);

            // Assert
            Assert.Null(identity);
        }

        [Theory]
        [InlineData("packageA.1.0.0.0.0-beta.nupkg", "packageA", false)]
        [InlineData("packageA.1.0.0-beta.nupkg", "packageA", true)]
        [InlineData("packageA.1.0.0-beta.nupkg", "packageA.1", true)]
        [InlineData("packageA.1.0.0-beta-#.nupkg", "packageA", false)]
        [InlineData("packageA.1.0.0-beta.txt", "packageA.1", false)]
        public void LocalFolderUtility_IsPossiblePackageMatchNoVersion(string fileName, string id, bool expected)
        {
            // Arrange
            var file = new FileInfo(Path.Combine(Directory.GetCurrentDirectory(), fileName));

            // Act
            var result = LocalFolderUtility.IsPossiblePackageMatch(file, id);

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("packageA.1.0.0.0.0-beta.nupkg", "packageA", "1.0.0", false)]
        [InlineData("packageA.1.0.0-beta.nupkg", "packageA", "1.0.0-beta", true)]
        [InlineData("packageA.1.0.0-beta.symbols.nupkg", "packageA", "1.0.0-beta", false)]
        [InlineData("packageA.1.0.0-beta.nupkg", "packageA.1", "0.0-beta", true)]
        [InlineData("packageA.1.0.0-beta-#.nupkg", "packageA", "1.0.0", false)]
        [InlineData("packageA.1.0.0-beta.txt", "packageA.1", "1.0.0", false)]
        public void LocalFolderUtility_IsPossiblePackageMatch(string fileName, string id, string version, bool expected)
        {
            // Arrange
            var file = new FileInfo(Path.Combine(Directory.GetCurrentDirectory(), fileName));
            var identity = new PackageIdentity(id, NuGetVersion.Parse(version));

            // Act
            var result = LocalFolderUtility.IsPossiblePackageMatch(file, identity);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public async Task LocalFolderUtility_GetFilesSafeCancellationToken_NotCancelled_Succeed()
        {
            // Arrange
            using var pathContext = new SimpleTestPathContext();
            var testLogger = new TestLogger();
            var source = new CancellationTokenSource();
            CancellationToken token = source.Token;
            var packageRoot = new DirectoryInfo(pathContext.PackageSource);

            // Act
            await SimpleTestPackageUtility.CreateFolderFeedV2Async(pathContext.PackageSource, new PackageIdentity("a", NuGetVersion.Parse("1.0.0")));
            await SimpleTestPackageUtility.CreateFolderFeedV2Async(pathContext.PackageSource, new PackageIdentity("b", NuGetVersion.Parse("1.0.0")));
            await SimpleTestPackageUtility.CreateFolderFeedV2Async(pathContext.PackageSource, new PackageIdentity("c", NuGetVersion.Parse("1.0.0")));

            // Act
            var packageFiles = LocalFolderUtility.GetFilesSafe(packageRoot, NupkgFilter, testLogger, token)
                .OrderBy(p => p.Name, StringComparer.OrdinalIgnoreCase)
                .ToList();

            // Assert
            Assert.Equal(3, packageFiles.Count);
            Assert.Equal("a.1.0.0.nupkg", packageFiles[0].Name);
            Assert.Equal("b.1.0.0.nupkg", packageFiles[1].Name);
            Assert.Equal("c.1.0.0.nupkg", packageFiles[2].Name);
        }

        [Fact]
        public async Task LocalFolderUtility_GetFilesSafeWithCancellationToken_Cancelled_Fails()
        {
            // Arrange
            using var pathContext = new SimpleTestPathContext();
            var testLogger = new TestLogger();
            var packageRoot = new DirectoryInfo(pathContext.PackageSource);
            var source = new CancellationTokenSource();
            CancellationToken token = source.Token;
            source.Cancel();

            // Act
            await SimpleTestPackageUtility.CreateFolderFeedV2Async(pathContext.PackageSource, new PackageIdentity("a", NuGetVersion.Parse("1.0.0")));

            // Assert
            Assert.Throws<OperationCanceledException>(() => LocalFolderUtility.GetFilesSafe(packageRoot, NupkgFilter, testLogger, token));
        }
    }
}
