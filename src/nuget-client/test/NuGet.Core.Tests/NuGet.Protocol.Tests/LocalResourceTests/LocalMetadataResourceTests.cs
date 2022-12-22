// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NuGet.Packaging.Core;
using NuGet.Protocol.Core.Types;
using NuGet.Test.Utility;
using NuGet.Versioning;
using Xunit;

namespace NuGet.Protocol.Tests
{
    [Collection(nameof(NotThreadSafeResourceCollection))]
    public class LocalMetadataResourceTests
    {
        [Fact]
        public async Task LocalMetadataResourceTests_GetVersions()
        {
            using (var root = TestDirectory.Create())
            {
                // Arrange
                var testLogger = new TestLogger();

                var packages = new[]
                {
                    new TestLocalPackageInfo("a", "1.0.0"),
                    new TestLocalPackageInfo("A", "1.0.0-alpha.1.2.3+a.b"),
                    new TestLocalPackageInfo("b", "2.0.0"),
                    new TestLocalPackageInfo("b", "2.0.1+githash.0faef")
                };

                var localResource = new TestFindLocalPackagesResource(packages);
                var resource = new LocalMetadataResource(localResource);

                // Act
                var result = (await resource.GetVersions("a", NullSourceCacheContext.Instance, testLogger, CancellationToken.None))
                    .OrderBy(v => v)
                    .ToList();

                // Assert
                Assert.Equal(2, result.Count);
                Assert.Equal("1.0.0-alpha.1.2.3+a.b", result[0].ToFullString());
                Assert.Equal("1.0.0", result[1].ToFullString());
            }
        }

        [Fact]
        public async Task LocalMetadataResourceTests_GetVersions_Distinct()
        {
            using (var root = TestDirectory.Create())
            {
                // Arrange
                var testLogger = new TestLogger();

                var packages = new[]
                {
                    new TestLocalPackageInfo("a", "1.0.0"),
                    new TestLocalPackageInfo("A", "1.0.0.0"),
                    new TestLocalPackageInfo("b", "2.0.0"),
                    new TestLocalPackageInfo("b", "2.0.0+githash.0faef")
                };

                var localResource = new TestFindLocalPackagesResource(packages);
                var resource = new LocalMetadataResource(localResource);

                // Act
                var resultA = (await resource.GetVersions("a", NullSourceCacheContext.Instance, testLogger, CancellationToken.None))
                    .OrderBy(v => v)
                    .ToList();

                var resultB = (await resource.GetVersions("b", NullSourceCacheContext.Instance, testLogger, CancellationToken.None))
                    .OrderBy(v => v)
                    .ToList();

                // Assert
                Assert.Equal(1, resultA.Count);
                Assert.Equal(1, resultB.Count);
            }
        }

        [Fact]
        public async Task LocalMetadataResourceTests_GetLatestVersion()
        {
            using (var root = TestDirectory.Create())
            {
                // Arrange
                var testLogger = new TestLogger();

                var packages = new[]
                {
                    new TestLocalPackageInfo("a", "1.0.0"),
                    new TestLocalPackageInfo("A", "1.0.0-alpha.1.2.3+a.b"),
                    new TestLocalPackageInfo("b", "2.0.0"),
                    new TestLocalPackageInfo("B", "2.0.1+githash.0faef"),
                    new TestLocalPackageInfo("b", "2.0.2-alpha.1.2.3+githash.0faef")
                };

                var localResource = new TestFindLocalPackagesResource(packages);
                var resource = new LocalMetadataResource(localResource);

                // Act
                var result = await resource.GetLatestVersion(
                    "b",
                    includePrerelease: false,
                    includeUnlisted: false,
                    sourceCacheContext: NullSourceCacheContext.Instance,
                    log: testLogger,
                    token: CancellationToken.None);

                // Assert
                Assert.Equal("2.0.1+githash.0faef", result.ToFullString());
            }
        }

        [Fact]
        public async Task LocalMetadataResourceTests_GetLatestVersionPrerelease()
        {
            using (var root = TestDirectory.Create())
            {
                // Arrange
                var testLogger = new TestLogger();

                var packages = new[]
                {
                    new TestLocalPackageInfo("a", "1.0.0"),
                    new TestLocalPackageInfo("A", "1.0.0-alpha.1.2.3+a.b"),
                    new TestLocalPackageInfo("b", "2.0.0"),
                    new TestLocalPackageInfo("B", "2.0.1+githash.0faef"),
                    new TestLocalPackageInfo("b", "2.0.2-alpha.1.2.3+githash.0faef")
                };

                var localResource = new TestFindLocalPackagesResource(packages);
                var resource = new LocalMetadataResource(localResource);

                // Act
                var result = await resource.GetLatestVersion(
                    "b",
                    includePrerelease: true,
                    includeUnlisted: false,
                    sourceCacheContext: NullSourceCacheContext.Instance,
                    log: testLogger,
                    token: CancellationToken.None);

                // Assert
                Assert.Equal("2.0.2-alpha.1.2.3+githash.0faef", result.ToFullString());
            }
        }

        [Fact]
        public async Task LocalMetadataResourceTests_GetLatestVersionPrerelease_Multiple()
        {
            using (var root = TestDirectory.Create())
            {
                // Arrange
                var testLogger = new TestLogger();

                var packages = new[]
                {
                    new TestLocalPackageInfo("a", "1.0.0"),
                    new TestLocalPackageInfo("A", "1.0.0-alpha.1.2.3+a.b"),
                    new TestLocalPackageInfo("b", "2.0.0"),
                    new TestLocalPackageInfo("B", "2.0.1+githash.0faef"),
                    new TestLocalPackageInfo("b", "2.0.2-alpha.1.2.3+githash.0faef")
                };

                var localResource = new TestFindLocalPackagesResource(packages);
                var resource = new LocalMetadataResource(localResource);

                // Act
                var result = (await resource.GetLatestVersions(
                    new string[] { "a", "b", "c" },
                    includePrerelease: false,
                    includeUnlisted: false,
                    sourceCacheContext: NullSourceCacheContext.Instance,
                    log: testLogger,
                    token: CancellationToken.None))
                    .OrderBy(e => e.Key)
                    .ToArray();

                // Assert
                Assert.Equal("a", result[0].Key);
                Assert.Equal("1.0.0", result[0].Value.ToFullString());
                Assert.Equal("b", result[1].Key);
                Assert.Equal("2.0.1+githash.0faef", result[1].Value.ToFullString());
                Assert.Equal("c", result[2].Key);
                Assert.Null(result[2].Value);
            }
        }

        [Fact]
        public async Task LocalMetadataResourceTests_ExistsById()
        {
            using (var root = TestDirectory.Create())
            {
                // Arrange
                var testLogger = new TestLogger();

                var packages = new[]
                {
                    new TestLocalPackageInfo("a", "1.0.0"),
                    new TestLocalPackageInfo("a", "1.0.0-alpha.1.2.3+a.b"),
                    new TestLocalPackageInfo("b", "2.0.0"),
                    new TestLocalPackageInfo("b", "2.0.1+githash.0faef")
                };

                var localResource = new TestFindLocalPackagesResource(packages);
                var resource = new LocalMetadataResource(localResource);

                // Act
                var a = await resource.Exists("A", NullSourceCacheContext.Instance, testLogger, CancellationToken.None);
                var b = await resource.Exists("b", NullSourceCacheContext.Instance, testLogger, CancellationToken.None);
                var c = await resource.Exists("c", NullSourceCacheContext.Instance, testLogger, CancellationToken.None);

                // Assert
                Assert.True(a);
                Assert.True(b);
                Assert.False(c);
            }
        }

        [Fact]
        public async Task LocalMetadataResourceTests_ExistsByIdentity()
        {
            using (var root = TestDirectory.Create())
            {
                // Arrange
                var testLogger = new TestLogger();

                var packages = new[]
                {
                    new TestLocalPackageInfo("a", "1.0.0"),
                    new TestLocalPackageInfo("a", "1.0.0-alpha.1.2.3+a.b"),
                    new TestLocalPackageInfo("b", "2.0.0"),
                    new TestLocalPackageInfo("b", "2.0.1+githash.0faef")
                };

                var localResource = new TestFindLocalPackagesResource(packages);
                var resource = new LocalMetadataResource(localResource);

                // Act
                var b1 = await resource.Exists(new PackageIdentity("b", NuGetVersion.Parse("2.0.1")), NullSourceCacheContext.Instance, testLogger, CancellationToken.None);
                var b2 = await resource.Exists(new PackageIdentity("b", NuGetVersion.Parse("2.0.1+githash.0faef")), NullSourceCacheContext.Instance, testLogger, CancellationToken.None);
                var b3 = await resource.Exists(new PackageIdentity("b", NuGetVersion.Parse("2.0.1+githash.aaaa")), NullSourceCacheContext.Instance, testLogger, CancellationToken.None);
                var b4 = await resource.Exists(new PackageIdentity("b", NuGetVersion.Parse("2.0.1-githash.0faef")), NullSourceCacheContext.Instance, testLogger, CancellationToken.None);
                var b5 = await resource.Exists(new PackageIdentity("b", NuGetVersion.Parse("2.0.01")), NullSourceCacheContext.Instance, testLogger, CancellationToken.None);

                // Assert
                Assert.True(b1);
                Assert.True(b2);
                Assert.True(b3);
                Assert.False(b4);
                Assert.True(b5);
            }
        }
    }
}
