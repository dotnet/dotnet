// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Protocol.Core.Types;
using NuGet.Test.Utility;
using NuGet.Versioning;
using Xunit;

namespace NuGet.Protocol.Tests
{
    public class LocalDownloadResourceTests
    {
        [Fact]
        public async Task LocalDownloadResource_PackageIsReturnedAsync()
        {
            using (var root = TestDirectory.Create())
            {
                // Arrange
                var testLogger = new TestLogger();

                var packageB = new SimpleTestPackageContext()
                {
                    Id = "b",
                    Version = "1.0.0"
                };

                var packageA = new SimpleTestPackageContext()
                {
                    Id = "a",
                    Version = "1.0.0",
                    Dependencies = new List<SimpleTestPackageContext>() { packageB }
                };

                var packageContexts = new SimpleTestPackageContext[]
                {
                    packageA,
                    packageB
                };

                await SimpleTestPackageUtility.CreatePackagesAsync(root, packageContexts);
                string packagesFolder = null; // This is unused by the implementation.

                var localResource = new FindLocalPackagesResourceV2(root);
                var resource = new LocalDownloadResource("source", localResource);

                // Act
                using (var cacheContext = new SourceCacheContext())
                using (var result = await resource.GetDownloadResourceResultAsync(
                    packageA.Identity,
                    new PackageDownloadContext(cacheContext),
                    packagesFolder,
                    testLogger,
                    CancellationToken.None))
                {
                    // Assert
                    Assert.Equal(DownloadResourceResultStatus.Available, result.Status);
                    Assert.Equal("a", result.PackageReader.GetIdentity().Id);
                    Assert.Equal("1.0.0", result.PackageReader.GetIdentity().Version.ToFullString());
                    Assert.True(result.PackageStream.CanSeek);
                }
            }
        }

        [Fact]
        public async Task LocalDownloadResource_PackageIsReturnedNonNormalizedAsync()
        {
            using (var root = TestDirectory.Create())
            {
                // Arrange
                var testLogger = new TestLogger();

                var packageA1 = new SimpleTestPackageContext()
                {
                    Id = "a",
                    Version = "1.0"
                };

                var packageA2 = new SimpleTestPackageContext()
                {
                    Id = "a",
                    Version = "1.0.0"
                };

                var packageA3 = new SimpleTestPackageContext()
                {
                    Id = "a",
                    Version = "1.0.0.0"
                };

                var packageContexts = new SimpleTestPackageContext[]
                {
                    packageA1,
                    packageA2,
                    packageA3
                };

                await SimpleTestPackageUtility.CreatePackagesAsync(root, packageContexts);
                string packagesFolder = null; // This is unused by the implementation.

                var localResource = new FindLocalPackagesResourceV2(root);
                var resource = new LocalDownloadResource("source", localResource);

                // Act
                using (var cacheContext = new SourceCacheContext())
                {
                    var downloadContext = new PackageDownloadContext(cacheContext);

                    using (var result1 = await resource.GetDownloadResourceResultAsync(
                        packageA1.Identity,
                        downloadContext,
                        packagesFolder,
                        testLogger,
                        CancellationToken.None))
                    using (var result2 = await resource.GetDownloadResourceResultAsync(
                        packageA2.Identity,
                        downloadContext,
                        packagesFolder,
                        testLogger,
                        CancellationToken.None))
                    using (var result3 = await resource.GetDownloadResourceResultAsync(
                        packageA3.Identity,
                        downloadContext,
                        packagesFolder,
                        testLogger,
                        CancellationToken.None))
                    {
                        // Assert
                        Assert.Equal("1.0", result1.PackageReader.GetIdentity().Version.ToString());
                        Assert.Equal("1.0.0", result2.PackageReader.GetIdentity().Version.ToString());
                        Assert.Equal("1.0.0.0", result3.PackageReader.GetIdentity().Version.ToString());
                    }
                }
            }
        }

        [Fact]
        public async Task LocalDownloadResource_PackageIsReturnedSemVer2Async()
        {
            using (var root = TestDirectory.Create())
            {
                // Arrange
                var testLogger = new TestLogger();

                var packageB = new SimpleTestPackageContext()
                {
                    Id = "b",
                    Version = "1.0.0"
                };

                var packageA = new SimpleTestPackageContext()
                {
                    Id = "a",
                    Version = "1.0.0-alpha.1.2+b",
                    Dependencies = new List<SimpleTestPackageContext>() { packageB }
                };

                var packageContexts = new SimpleTestPackageContext[]
                {
                    packageA,
                    packageB
                };

                await SimpleTestPackageUtility.CreatePackagesAsync(root, packageContexts);
                string packagesFolder = null; // This is unused by the implementation.

                var localResource = new FindLocalPackagesResourceV2(root);
                var resource = new LocalDownloadResource("source", localResource);

                // Act
                using (var cacheContext = new SourceCacheContext())
                using (var result = await resource.GetDownloadResourceResultAsync(
                    packageA.Identity,
                    new PackageDownloadContext(cacheContext),
                    packagesFolder,
                    testLogger,
                    CancellationToken.None))
                {
                    // Assert
                    Assert.Equal(DownloadResourceResultStatus.Available, result.Status);
                    Assert.Equal("a", result.PackageReader.GetIdentity().Id);
                    Assert.Equal("1.0.0-alpha.1.2+b", result.PackageReader.GetIdentity().Version.ToFullString());
                    Assert.True(result.PackageStream.CanSeek);
                }
            }
        }

        [Fact]
        public async Task LocalDownloadResource_PackageNotFoundAsync()
        {
            using (var root = TestDirectory.Create())
            {
                // Arrange
                var testLogger = new TestLogger();

                var packageB = new SimpleTestPackageContext()
                {
                    Id = "b",
                    Version = "1.0.0"
                };

                var packageA = new SimpleTestPackageContext()
                {
                    Id = "a",
                    Version = "1.0.0-alpha.1.2.3+a.b.c",
                    Dependencies = new List<SimpleTestPackageContext>() { packageB }
                };

                var packageContexts = new SimpleTestPackageContext[]
                {
                    packageA,
                    packageB
                };

                await SimpleTestPackageUtility.CreatePackagesAsync(root, packageContexts);
                string packagesFolder = null; // This is unused by the implementation.

                var localResource = new FindLocalPackagesResourceV2(root);
                var resource = new LocalDownloadResource("source", localResource);

                // Act
                using (var cacheContext = new SourceCacheContext())
                using (var result = await resource.GetDownloadResourceResultAsync(
                    new PackageIdentity("a", NuGetVersion.Parse("1.0.0-beta.1.2.3+a.b")),
                    new PackageDownloadContext(cacheContext),
                    packagesFolder,
                    testLogger,
                    CancellationToken.None))
                {
                    // Assert
                    Assert.Equal(DownloadResourceResultStatus.NotFound, result.Status);
                }
            }
        }

        [Fact]
        public async Task LocalDownloadResource_PackageNotFoundEmptyFolderAsync()
        {
            using (var root = TestDirectory.Create())
            {
                // Arrange
                var testLogger = new TestLogger();
                string packagesFolder = null; // This is unused by the implementation.

                var localResource = new FindLocalPackagesResourceV2(root);
                var resource = new LocalDownloadResource("source", localResource);

                // Act
                using (var cacheContext = new SourceCacheContext())
                using (var result = await resource.GetDownloadResourceResultAsync(
                    new PackageIdentity("a", NuGetVersion.Parse("1.0.0-beta.1.2.3+a.b")),
                    new PackageDownloadContext(cacheContext),
                    packagesFolder,
                    testLogger,
                    CancellationToken.None))
                {
                    // Assert
                    Assert.Equal(DownloadResourceResultStatus.NotFound, result.Status);
                }
            }
        }

        [Fact]
        public async Task LocalDownloadResource_PackageNotFoundNoFolderAsync()
        {
            using (var root = TestDirectory.Create())
            {
                // Arrange
                var testLogger = new TestLogger();
                string packagesFolder = null; // This is unused by the implementation.

                var localResource = new FindLocalPackagesResourceV2(root);
                var resource = new LocalDownloadResource("source", localResource);

                Directory.Delete(root);

                // Act
                using (var cacheContext = new SourceCacheContext())
                using (var result = await resource.GetDownloadResourceResultAsync(
                    new PackageIdentity("a", NuGetVersion.Parse("1.0.0-beta.1.2.3+a.b")),
                    new PackageDownloadContext(cacheContext),
                    packagesFolder,
                    testLogger,
                    CancellationToken.None))
                {
                    // Assert
                    Assert.Equal(DownloadResourceResultStatus.NotFound, result.Status);
                }
            }
        }

        [Fact]
        public async Task LocalDownloadResource_PackageIsReturnedUnzippedFolderAsync()
        {
            using (var root = TestDirectory.Create())
            {
                // Arrange
                var testLogger = new TestLogger();

                var id = new PackageIdentity("a", NuGetVersion.Parse("1.0.0"));
                await SimpleTestPackageUtility.CreateFolderFeedUnzipAsync(root, id);
                string packagesFolder = null; // This is unused by the implementation.

                var localResource = new FindLocalPackagesResourceUnzipped(root);
                var resource = new LocalDownloadResource("source", localResource);

                // Act
                using (var cacheContext = new SourceCacheContext())
                using (var result = await resource.GetDownloadResourceResultAsync(
                    id,
                    new PackageDownloadContext(cacheContext),
                    packagesFolder,
                    testLogger,
                    CancellationToken.None))
                {
                    // Assert
                    Assert.Equal(DownloadResourceResultStatus.Available, result.Status);
                    Assert.Equal("a", result.PackageReader.GetIdentity().Id);
                    Assert.Equal("1.0.0", result.PackageReader.GetIdentity().Version.ToFullString());
                    Assert.True(result.PackageStream.CanSeek);
                    Assert.True(result.PackageReader is PackageFolderReader);
                }
            }
        }
    }
}
