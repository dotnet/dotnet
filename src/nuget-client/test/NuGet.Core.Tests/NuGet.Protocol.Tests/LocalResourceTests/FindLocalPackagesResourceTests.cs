// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.Packaging.Core;
using NuGet.Test.Utility;
using NuGet.Versioning;
using Xunit;

namespace NuGet.Protocol.Tests
{
    [Collection(nameof(NotThreadSafeResourceCollection))]
    public class FindLocalPackagesResourceTests
    {
        private static readonly PackageIdentity PackageA1 = new PackageIdentity("a", NuGetVersion.Parse("1.0.0"));
        private static readonly PackageIdentity PackageA2 = new PackageIdentity("a", NuGetVersion.Parse("1.0-beta"));
        private static readonly PackageIdentity PackageA3 = new PackageIdentity("a", NuGetVersion.Parse("1.0.0-beta.1.2"));
        private static readonly PackageIdentity PackageA4 = new PackageIdentity("a", NuGetVersion.Parse("1.0.0-beta.1.3+a.b.c"));
        private static readonly PackageIdentity PackageA5 = new PackageIdentity("A", NuGetVersion.Parse("2.0.0.0"));

        private static readonly PackageIdentity PackageB = new PackageIdentity("b", NuGetVersion.Parse("1.0"));
        private static readonly PackageIdentity PackageC = new PackageIdentity("c", NuGetVersion.Parse("0.0.1-alpha.1.2"));

        private readonly static PackageIdentity[] PackageSet1 = new[]
        {
            PackageA1,
            PackageA2,
            PackageA3,
            PackageA4,
            PackageA5,
            PackageB,
            PackageC
        };

        [Fact]
        public async Task FindLocalPackagesResource_GetPackagesBasicAsync()
        {
            using (var rootPackagesConfig = TestDirectory.Create())
            using (var rootUnzip = TestDirectory.Create())
            using (var rootV3 = TestDirectory.Create())
            using (var rootV2 = TestDirectory.Create())
            {
                // Arrange
                var testLogger = new TestLogger();
                await CreateFeedsAsync(rootV2, rootV3, rootUnzip, rootPackagesConfig, PackageSet1);
                var expected = new HashSet<PackageIdentity>(PackageSet1);

                var resources = new FindLocalPackagesResource[]
                {
                    new FindLocalPackagesResourcePackagesConfig(rootPackagesConfig),
                    new FindLocalPackagesResourceUnzipped(rootUnzip),
                    new FindLocalPackagesResourceV2(rootV2),
                    new FindLocalPackagesResourceV3(rootV3),
                    new FindLocalPackagesResourcePackagesConfig(UriUtility.CreateSourceUri(rootPackagesConfig).AbsoluteUri),
                    new FindLocalPackagesResourceUnzipped(UriUtility.CreateSourceUri(rootUnzip).AbsoluteUri),
                    new FindLocalPackagesResourceV2(UriUtility.CreateSourceUri(rootV2).AbsoluteUri),
                    new FindLocalPackagesResourceV3(UriUtility.CreateSourceUri(rootV3).AbsoluteUri)
                };

                foreach (var resource in resources)
                {
                    // Act
                    var result = resource.GetPackages(testLogger, CancellationToken.None).ToList();

                    // Assert
                    Assert.True(expected.SetEquals(result.Select(p => p.Identity)));
                    Assert.True(expected.SetEquals(result.Select(p =>
                    {
                        using (var reader = p.GetReader())
                        {
                            return reader.GetIdentity();
                        }
                    })));
                    Assert.True(expected.SetEquals(result.Select(p => p.Nuspec.GetIdentity())));
                    Assert.True(result.All(p => p.IsNupkg));
                }
            }
        }

        [Fact]
        public void FindLocalPackagesResource_EmptyFolders()
        {
            using (var emptyDir = TestDirectory.Create())
            {
                // Arrange
                var testLogger = new TestLogger();
                var doesNotExist = Path.Combine(emptyDir, "doesNotExist");

                var resources = new FindLocalPackagesResource[]
                {
                    new FindLocalPackagesResourcePackagesConfig(doesNotExist),
                    new FindLocalPackagesResourcePackagesConfig(emptyDir),
                    new FindLocalPackagesResourceUnzipped(doesNotExist),
                    new FindLocalPackagesResourceV2(doesNotExist),
                    new FindLocalPackagesResourceV3(doesNotExist),
                    new FindLocalPackagesResourceUnzipped(emptyDir),
                    new FindLocalPackagesResourceV2(emptyDir),
                    new FindLocalPackagesResourceV3(emptyDir),
                    new FindLocalPackagesResourcePackagesConfig(UriUtility.CreateSourceUri(doesNotExist).AbsoluteUri),
                    new FindLocalPackagesResourceUnzipped(UriUtility.CreateSourceUri(doesNotExist).AbsoluteUri),
                    new FindLocalPackagesResourceV2(UriUtility.CreateSourceUri(doesNotExist).AbsoluteUri),
                    new FindLocalPackagesResourceV3(UriUtility.CreateSourceUri(doesNotExist).AbsoluteUri),
                };

                foreach (var resource in resources)
                {
                    // Act
                    var getPackages = resource.GetPackages(testLogger, CancellationToken.None).ToList();
                    var findPackages = resource.FindPackagesById("a", testLogger, CancellationToken.None).ToList();
                    var package = resource.GetPackage(PackageA1, testLogger, CancellationToken.None);

                    // Assert
                    Assert.Equal(0, getPackages.Count);
                    Assert.Equal(0, findPackages.Count);
                    Assert.Null(package);
                }
            }
        }

        [Fact]
        public async Task FindLocalPackagesResource_FindPackagesByIdBasicAsync()
        {
            using (var rootPackagesConfig = TestDirectory.Create())
            using (var rootUnzip = TestDirectory.Create())
            using (var rootV3 = TestDirectory.Create())
            using (var rootV2 = TestDirectory.Create())
            {
                // Arrange
                var testLogger = new TestLogger();
                await CreateFeedsAsync(rootV2, rootV3, rootUnzip, rootPackagesConfig, PackageSet1);
                var expected = new HashSet<PackageIdentity>(new[]
                {
                    PackageA1,
                    PackageA2,
                    PackageA3,
                    PackageA4,
                    PackageA5,
                });

                var resources = new FindLocalPackagesResource[]
                {
                    new FindLocalPackagesResourcePackagesConfig(rootPackagesConfig),
                    new FindLocalPackagesResourceUnzipped(rootUnzip),
                    new FindLocalPackagesResourceV2(rootV2),
                    new FindLocalPackagesResourceV3(rootV3),
                    new FindLocalPackagesResourcePackagesConfig(UriUtility.CreateSourceUri(rootPackagesConfig).AbsoluteUri),
                    new FindLocalPackagesResourceUnzipped(UriUtility.CreateSourceUri(rootUnzip).AbsoluteUri),
                    new FindLocalPackagesResourceV2(UriUtility.CreateSourceUri(rootV2).AbsoluteUri),
                    new FindLocalPackagesResourceV3(UriUtility.CreateSourceUri(rootV3).AbsoluteUri)
                };

                foreach (var resource in resources)
                {
                    // Act
                    var result = resource.FindPackagesById("a", testLogger, CancellationToken.None).ToList();

                    // Assert
                    Assert.True(expected.SetEquals(result.Select(p => p.Identity)));
                    Assert.True(expected.SetEquals(result.Select(p =>
                    {
                        using (var reader = p.GetReader())
                        {
                            return reader.GetIdentity();
                        }
                    })));
                    Assert.True(expected.SetEquals(result.Select(p => p.Nuspec.GetIdentity())));
                    Assert.True(result.All(p => p.IsNupkg));
                }
            }
        }

        [Fact]
        public async Task FindLocalPackagesResource_GetPackageBasicAsync()
        {
            using (var rootPackagesConfig = TestDirectory.Create())
            using (var rootUnzip = TestDirectory.Create())
            using (var rootV3 = TestDirectory.Create())
            using (var rootV2 = TestDirectory.Create())
            {
                // Arrange
                var testLogger = new TestLogger();
                await CreateFeedsAsync(rootV2, rootV3, rootUnzip, rootPackagesConfig, PackageSet1);
                var expected = new HashSet<PackageIdentity>(new[]
                {
                    PackageA1,
                    PackageA2,
                    PackageA3,
                    PackageA4,
                    PackageA5,
                });

                var resources = new FindLocalPackagesResource[]
                {
                    new FindLocalPackagesResourcePackagesConfig(rootPackagesConfig),
                    new FindLocalPackagesResourceUnzipped(rootUnzip),
                    new FindLocalPackagesResourceV2(rootV2),
                    new FindLocalPackagesResourceV3(rootV3),
                    new FindLocalPackagesResourcePackagesConfig(UriUtility.CreateSourceUri(rootPackagesConfig).AbsoluteUri),
                    new FindLocalPackagesResourceUnzipped(UriUtility.CreateSourceUri(rootUnzip).AbsoluteUri),
                    new FindLocalPackagesResourceV2(UriUtility.CreateSourceUri(rootV2).AbsoluteUri),
                    new FindLocalPackagesResourceV3(UriUtility.CreateSourceUri(rootV3).AbsoluteUri)
                };

                foreach (var resource in resources)
                {
                    // Act
                    var result = resource.GetPackage(PackageA3, testLogger, CancellationToken.None);
                    var result2 = resource.GetPackage(UriUtility.CreateSourceUri(result.Path), testLogger, CancellationToken.None);

                    // Assert
                    Assert.Equal(PackageA3, result.Identity);
                    Assert.Equal(PackageA3, result2.Identity);
                }
            }
        }

        private async Task CreateFeedsAsync(string rootV2, string rootV3, string rootUnzip, string rootPackagesConfig, params PackageIdentity[] packages)
        {
            foreach (var package in packages)
            {
                await SimpleTestPackageUtility.CreateFolderFeedV2Async(rootV2, package);
                await SimpleTestPackageUtility.CreateFolderFeedV3Async(rootV3, package);
                await SimpleTestPackageUtility.CreateFolderFeedUnzipAsync(rootUnzip, package);
                await SimpleTestPackageUtility.CreateFolderFeedPackagesConfigAsync(rootPackagesConfig, package);
            }
        }
    }
}
