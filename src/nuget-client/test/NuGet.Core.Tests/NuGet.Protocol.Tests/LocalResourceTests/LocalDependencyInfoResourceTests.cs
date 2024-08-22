// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using NuGet.Frameworks;
using NuGet.Packaging.Core;
using NuGet.Protocol.Core.Types;
using NuGet.Test.Utility;
using NuGet.Versioning;
using Xunit;

namespace NuGet.Protocol.Tests
{
    public class LocalDependencyInfoResourceTests
    {

        [Fact]
        public async Task LocalDependencyInfoResource_BasicAsync()
        {
            using (var root = TestDirectory.Create())
            {
                // Arrange
                var testLogger = new TestLogger();

                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };

                var packageD = new SimpleTestPackageContext()
                {
                    Id = "d",
                    Version = "1.0.0"
                };

                var packageD2 = new SimpleTestPackageContext()
                {
                    Id = "d",
                    Version = "2.0.0"
                };

                var packageD3 = new SimpleTestPackageContext()
                {
                    Id = "d",
                    Version = "0.1.0"
                };

                var packageC = new SimpleTestPackageContext()
                {
                    Id = "c",
                    Version = "1.0.0",
                    Dependencies = new List<SimpleTestPackageContext>() { packageD }
                };

                var packageB = new SimpleTestPackageContext()
                {
                    Id = "b",
                    Version = "1.0.0",
                    Dependencies = new List<SimpleTestPackageContext>() { packageD2 }
                };

                var packageA = new SimpleTestPackageContext()
                {
                    Id = "a",
                    Version = "1.0.0",
                    Dependencies = new List<SimpleTestPackageContext>() { packageC, packageB }
                };

                var packageContexts = new SimpleTestPackageContext[]
                {
                    packageA,
                    packageB,
                    packageC,
                    packageD,
                    packageD2,
                    packageD3,
                    packageX
                };

                await SimpleTestPackageUtility.CreatePackagesAsync(root, packageContexts);

                var source = Repository.Factory.GetCoreV3(root);
                var localResource = new FindLocalPackagesResourceV2(root);
                var resource = new LocalDependencyInfoResource(localResource, source);

                // Act
                var resultsA = (await resource.ResolvePackages("a", NuGetFramework.Parse("net46"), NullSourceCacheContext.Instance, testLogger, CancellationToken.None)).ToList();
                var resultsX = (await resource.ResolvePackages("x", NuGetFramework.Parse("net46"), NullSourceCacheContext.Instance, testLogger, CancellationToken.None)).ToList();
                var resultsY = (await resource.ResolvePackages("y", NuGetFramework.Parse("net46"), NullSourceCacheContext.Instance, testLogger, CancellationToken.None)).ToList();

                // Assert
                Assert.Equal(1, resultsA.Count);
                Assert.Equal(packageA.Identity, resultsA[0], PackageIdentity.Comparer);
                Assert.True(resultsA[0].Listed);
                Assert.Contains("a.1.0.0.nupkg", resultsA[0].DownloadUri.LocalPath);
                Assert.Equal(2, resultsA[0].Dependencies.Count());
                Assert.Equal("c", resultsA[0].Dependencies.First().Id);
                Assert.Equal("[1.0.0, )", resultsA[0].Dependencies.First().VersionRange.ToNormalizedString());
                Assert.Equal("b", resultsA[0].Dependencies.Skip(1).First().Id);
                Assert.Equal("[1.0.0, )", resultsA[0].Dependencies.Skip(1).First().VersionRange.ToNormalizedString());

                // no dependencies
                Assert.Equal(1, resultsX.Count);
                Assert.Equal(packageX.Identity, resultsX[0], PackageIdentity.Comparer);
                Assert.True(resultsX[0].Listed);
                Assert.Contains("x.1.0.0.nupkg", resultsX[0].DownloadUri.LocalPath);
                Assert.Equal(0, resultsX[0].Dependencies.Count());

                // not found
                Assert.Equal(0, resultsY.Count);
            }
        }

        [Fact]
        public async Task LocalDependencyInfoResource_NoDependenciesAsync()
        {
            using (var root = TestDirectory.Create())
            {
                // Arrange
                var testLogger = new TestLogger();

                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };

                var packageA = new SimpleTestPackageContext()
                {
                    Id = "a",
                    Version = "1.0.0",
                    Dependencies = new List<SimpleTestPackageContext>() { packageX }
                };

                var packageContexts = new SimpleTestPackageContext[]
                {
                    packageA,
                    packageX
                };

                await SimpleTestPackageUtility.CreatePackagesAsync(root, packageContexts);

                var source = Repository.Factory.GetCoreV3(root);
                var localResource = new FindLocalPackagesResourceV2(root);
                var resource = new LocalDependencyInfoResource(localResource, source);

                // Act
                var results = (await resource.ResolvePackages("x", NuGetFramework.Parse("net46"), NullSourceCacheContext.Instance, testLogger, CancellationToken.None)).ToList();

                // Assert
                // no dependencies
                Assert.Equal(1, results.Count);
                Assert.Equal(packageX.Identity, results[0], PackageIdentity.Comparer);
                Assert.True(results[0].Listed);
                Assert.Contains("x.1.0.0.nupkg", results[0].DownloadUri.LocalPath);
                Assert.Equal(0, results[0].Dependencies.Count());
            }
        }

        [Fact]
        public async Task LocalDependencyInfoResource_IdNotFoundAsync()
        {
            using (var root = TestDirectory.Create())
            {
                // Arrange
                var testLogger = new TestLogger();

                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };

                var packageContexts = new SimpleTestPackageContext[]
                {
                    packageX
                };

                await SimpleTestPackageUtility.CreatePackagesAsync(root, packageContexts);

                var source = Repository.Factory.GetCoreV3(root);
                var localResource = new FindLocalPackagesResourceV2(root);
                var resource = new LocalDependencyInfoResource(localResource, source);

                // Act
                var resultsY = (await resource.ResolvePackages("y", NuGetFramework.Parse("net46"), NullSourceCacheContext.Instance, testLogger, CancellationToken.None)).ToList();

                // Assert
                Assert.Equal(0, resultsY.Count);
            }
        }

        [Fact]
        public async Task LocalDependencyInfoResource_EmptyFolderAsync()
        {
            using (var root = TestDirectory.Create())
            {
                // Arrange
                var testLogger = new TestLogger();

                var source = Repository.Factory.GetCoreV3(root);
                var localResource = new FindLocalPackagesResourceV2(root);
                var resource = new LocalDependencyInfoResource(localResource, source);

                // Act
                var results = (await resource.ResolvePackages("notfound", NuGetFramework.Parse("net46"), NullSourceCacheContext.Instance, testLogger, CancellationToken.None)).ToList();

                // Assert
                Assert.Equal(0, results.Count);
            }
        }

        [Fact]
        public async Task LocalDependencyInfoResource_FolderDoesNotExistAsync()
        {
            using (var root = TestDirectory.Create())
            {
                // Arrange
                var testLogger = new TestLogger();

                var source = Repository.Factory.GetCoreV3(root);
                var localResource = new FindLocalPackagesResourceV2(root);
                var resource = new LocalDependencyInfoResource(localResource, source);

                Directory.Delete(root);

                // Act
                var results = (await resource.ResolvePackages("notfound", NuGetFramework.Parse("net46"), NullSourceCacheContext.Instance, testLogger, CancellationToken.None)).ToList();

                // Assert
                Assert.Equal(0, results.Count);
            }
        }

        [Fact]
        public async Task LocalDependencyInfoResource_MissingDependencyAsync()
        {
            using (var root = TestDirectory.Create())
            {
                // Arrange
                var testLogger = new TestLogger();

                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };

                var packageA = new SimpleTestPackageContext()
                {
                    Id = "a",
                    Version = "1.0.0",
                    Dependencies = new List<SimpleTestPackageContext>() { packageX }
                };

                var packageContexts = new SimpleTestPackageContext[]
                {
                    packageA,
                    packageX
                };

                await SimpleTestPackageUtility.CreatePackagesAsync(root, packageContexts);

                foreach (var file in Directory.GetFiles(root))
                {
                    if (file.Contains("x.1.0.0"))
                    {
                        File.Delete(file);
                    }
                }

                var source = Repository.Factory.GetCoreV3(root);
                var localResource = new FindLocalPackagesResourceV2(root);
                var resource = new LocalDependencyInfoResource(localResource, source);

                // Act
                var results = (await resource.ResolvePackages("a", NuGetFramework.Parse("net46"), NullSourceCacheContext.Instance, testLogger, CancellationToken.None)).ToList();
                var resultsX = (await resource.ResolvePackages("x", NuGetFramework.Parse("net46"), NullSourceCacheContext.Instance, testLogger, CancellationToken.None)).ToList();

                // Assert
                Assert.Equal(1, results.Count);
                Assert.Equal(packageA.Identity, results[0], PackageIdentity.Comparer);
                Assert.True(results[0].Listed);
                Assert.Contains("a.1.0.0.nupkg", results[0].DownloadUri.LocalPath);
                Assert.Equal(1, results[0].Dependencies.Count());

                Assert.Equal(0, resultsX.Count);
            }
        }

        [Fact]
        public async Task LocalDependencyInfoResource_SinglePackageAsync()
        {
            using (var root = TestDirectory.Create())
            {
                // Arrange
                var testLogger = new TestLogger();

                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };

                var packageA = new SimpleTestPackageContext()
                {
                    Id = "a",
                    Version = "1.0.0",
                    Dependencies = new List<SimpleTestPackageContext>() { packageX }
                };

                var packageContexts = new SimpleTestPackageContext[]
                {
                    packageA,
                    packageX
                };

                await SimpleTestPackageUtility.CreatePackagesAsync(root, packageContexts);

                var source = Repository.Factory.GetCoreV3(root);
                var localResource = new FindLocalPackagesResourceV2(root);
                var resource = new LocalDependencyInfoResource(localResource, source);

                // Act
                var result = (await resource.ResolvePackage(
                    packageA.Identity,
                    NuGetFramework.Parse("net46"),
                    NullSourceCacheContext.Instance,
                    testLogger,
                    CancellationToken.None));

                // Assert
                Assert.Equal(packageA.Identity, result, PackageIdentity.Comparer);
                Assert.True(result.Listed);
                Assert.Contains("a.1.0.0.nupkg", result.DownloadUri.LocalPath);
                Assert.Equal(1, result.Dependencies.Count());
                Assert.Equal("x", result.Dependencies.Single().Id);
            }
        }

        [Fact]
        public async Task LocalDependencyInfoResource_SinglePackageNotFoundAsync()
        {
            using (var root = TestDirectory.Create())
            {
                // Arrange
                var testLogger = new TestLogger();

                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };

                var packageA = new SimpleTestPackageContext()
                {
                    Id = "a",
                    Version = "1.0.0",
                    Dependencies = new List<SimpleTestPackageContext>() { packageX }
                };

                var packageContexts = new SimpleTestPackageContext[]
                {
                    packageA,
                    packageX
                };

                await SimpleTestPackageUtility.CreatePackagesAsync(root, packageContexts);

                var source = Repository.Factory.GetCoreV3(root);
                var localResource = new FindLocalPackagesResourceV2(root);
                var resource = new LocalDependencyInfoResource(localResource, source);

                // Act
                var result = (await resource.ResolvePackage(
                    new PackageIdentity("z", NuGetVersion.Parse("1.0.0")),
                    NuGetFramework.Parse("net46"),
                    NullSourceCacheContext.Instance,
                    testLogger,
                    CancellationToken.None));

                // Assert
                Assert.Null(result);
            }
        }

        [Fact]
        public async Task LocalDependencyInfoResource_SinglePackageNotFoundEmptyFolderAsync()
        {
            using (var root = TestDirectory.Create())
            {
                // Arrange
                var testLogger = new TestLogger();

                var source = Repository.Factory.GetCoreV3(root);
                var localResource = new FindLocalPackagesResourceV2(root);
                var resource = new LocalDependencyInfoResource(localResource, source);

                // Act
                var result = (await resource.ResolvePackage(
                    new PackageIdentity("z", NuGetVersion.Parse("1.0.0")),
                    NuGetFramework.Parse("net46"),
                    NullSourceCacheContext.Instance,
                    testLogger,
                    CancellationToken.None));

                // Assert
                Assert.Null(result);
            }
        }

        [Fact]
        public async Task LocalDependencyInfoResource_SinglePackageNearestDependencyGroupAsync()
        {
            using (var root = TestDirectory.Create())
            {
                // Arrange
                var testLogger = new TestLogger();

                var packageA = new SimpleTestPackageContext()
                {
                    Id = "a",
                    Version = "1.0",
                    Nuspec = XDocument.Parse($@"<?xml version=""1.0"" encoding=""utf-8""?>
                        <package>
                        <metadata>
                            <id>a</id>
                            <version>1.0</version>
                            <title />
                            <dependencies>
                                <group>
                                    <dependency id=""b"" version=""1.0"" />
                                </group>
                                <group targetFramework=""net46"">
                                    <dependency id=""x"" />
                                </group>
                                <group targetFramework=""net461"">
                                </group>
                            </dependencies>
                        </metadata>
                        </package>")
                };

                var packageContexts = new SimpleTestPackageContext[]
                {
                    packageA
                };

                await SimpleTestPackageUtility.CreatePackagesAsync(root, packageContexts);

                var source = Repository.Factory.GetCoreV3(root);
                var localResource = new FindLocalPackagesResourceV2(root);
                var resource = new LocalDependencyInfoResource(localResource, source);

                // Act
                var resultNet462 = (await resource.ResolvePackage(
                    packageA.Identity,
                    NuGetFramework.Parse("net462"),
                    NullSourceCacheContext.Instance,
                    testLogger,
                    CancellationToken.None));

                var resultNet46 = (await resource.ResolvePackage(
                    packageA.Identity,
                    NuGetFramework.Parse("net46"),
                    NullSourceCacheContext.Instance,
                    testLogger,
                    CancellationToken.None));

                var resultWin8 = (await resource.ResolvePackage(
                    packageA.Identity,
                    NuGetFramework.Parse("win8"),
                    NullSourceCacheContext.Instance,
                    testLogger,
                    CancellationToken.None));

                // Assert
                Assert.Equal(0, resultNet462.Dependencies.Count());
                Assert.Equal(1, resultNet46.Dependencies.Count());
                Assert.Equal(1, resultWin8.Dependencies.Count());

                Assert.Equal("x", resultNet46.Dependencies.Single().Id);
                Assert.Equal(VersionRange.All, resultNet46.Dependencies.Single().VersionRange);
                Assert.Equal("b", resultWin8.Dependencies.Single().Id);
                Assert.Equal(VersionRange.Parse("1.0"), resultWin8.Dependencies.Single().VersionRange);
            }
        }

        [Fact]
        public async Task LocalDependencyInfoResource_ResolvePackagesNearestDependencyGroupAsync()
        {
            using (var root = TestDirectory.Create())
            {
                // Arrange
                var testLogger = new TestLogger();

                var packageA = new SimpleTestPackageContext()
                {
                    Id = "a",
                    Version = "1.0",
                    Nuspec = XDocument.Parse($@"<?xml version=""1.0"" encoding=""utf-8""?>
                        <package>
                        <metadata>
                            <id>a</id>
                            <version>1.0</version>
                            <title />
                            <dependencies>
                                <group>
                                    <dependency id=""b"" version=""1.0"" />
                                </group>
                                <group targetFramework=""net46"">
                                    <dependency id=""x"" />
                                </group>
                                <group targetFramework=""net461"">
                                </group>
                            </dependencies>
                        </metadata>
                        </package>")
                };

                var packageContexts = new SimpleTestPackageContext[]
                {
                    packageA
                };

                await SimpleTestPackageUtility.CreatePackagesAsync(root, packageContexts);

                var source = Repository.Factory.GetCoreV3(root);
                var localResource = new FindLocalPackagesResourceV2(root);
                var resource = new LocalDependencyInfoResource(localResource, source);

                // Act
                var resultNet462 = (await resource.ResolvePackages(
                    "a",
                    NuGetFramework.Parse("net462"),
                    NullSourceCacheContext.Instance,
                    testLogger,
                    CancellationToken.None)).Single();

                var resultNet46 = (await resource.ResolvePackages(
                    "a",
                    NuGetFramework.Parse("net46"),
                    NullSourceCacheContext.Instance,
                    testLogger,
                    CancellationToken.None)).Single();

                var resultWin8 = (await resource.ResolvePackages(
                    "a",
                    NuGetFramework.Parse("win8"),
                    NullSourceCacheContext.Instance,
                    testLogger,
                    CancellationToken.None)).Single();

                // Assert
                Assert.Equal(0, resultNet462.Dependencies.Count());
                Assert.Equal(1, resultNet46.Dependencies.Count());
                Assert.Equal(1, resultWin8.Dependencies.Count());

                Assert.Equal("x", resultNet46.Dependencies.Single().Id);
                Assert.Equal(VersionRange.All, resultNet46.Dependencies.Single().VersionRange);
                Assert.Equal("b", resultWin8.Dependencies.Single().Id);
                Assert.Equal(VersionRange.Parse("1.0"), resultWin8.Dependencies.Single().VersionRange);
            }
        }
    }
}
