// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NuGet.Common;
using NuGet.Frameworks;
using NuGet.LibraryModel;
using NuGet.Packaging.Core;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;
using Xunit;

namespace NuGet.DependencyResolver.Tests
{
    public class LocalDependencyProviderTests
    {
        [Fact]
        public void Constructor_ThrowsForNullDependencyProvider()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => new LocalDependencyProvider(dependencyProvider: null));

            Assert.Equal("dependencyProvider", exception.ParamName);
        }

        [Fact]
        public void IsHttp_IsFalse()
        {
            var provider = new LocalDependencyProvider(Mock.Of<IDependencyProvider>());

            Assert.False(provider.IsHttp);
        }

        [Fact]
        public void Source_IsNull()
        {
            var provider = new LocalDependencyProvider(Mock.Of<IDependencyProvider>());

            Assert.Null(provider.Source);
        }

        [Fact]
        public async Task FindPackageAsync_ThrowsForNullLibraryRange()
        {
            var provider = new LocalDependencyProvider(Mock.Of<IDependencyProvider>());

            using (var sourceCacheContext = new SourceCacheContext())
            {
                var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                    () => provider.FindLibraryAsync(
                        libraryRange: null,
                        targetFramework: NuGetFramework.Parse("net45"),
                        cacheContext: sourceCacheContext,
                        logger: NullLogger.Instance,
                        cancellationToken: CancellationToken.None));

                Assert.Equal("libraryRange", exception.ParamName);
            }
        }

        [Fact]
        public async Task FindPackageAsync_ThrowsForNullTargetFramework()
        {
            var provider = new LocalDependencyProvider(Mock.Of<IDependencyProvider>());

            using (var sourceCacheContext = new SourceCacheContext())
            {
                var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                    () => provider.FindLibraryAsync(
                        new LibraryRange(name: "a", typeConstraint: new LibraryDependencyTarget()),
                        targetFramework: null,
                        cacheContext: sourceCacheContext,
                        logger: NullLogger.Instance,
                        cancellationToken: CancellationToken.None));

                Assert.Equal("targetFramework", exception.ParamName);
            }
        }

        [Fact]
        public async Task FindPackageAsync_ReturnsNullIfDependencyProviderReturnsNull()
        {
            var dependencyProvider = new Mock<IDependencyProvider>();

            dependencyProvider.Setup(x => x.GetLibrary(
                    It.IsNotNull<LibraryRange>(),
                    It.IsNotNull<NuGetFramework>()))
                .Returns<Library>(null);

            var provider = new LocalDependencyProvider(dependencyProvider.Object);

            using (var sourceCacheContext = new SourceCacheContext())
            {
                var libraryIdentity = await provider.FindLibraryAsync(
                    new LibraryRange(name: "a", typeConstraint: new LibraryDependencyTarget()),
                    NuGetFramework.Parse("net45"),
                    sourceCacheContext,
                    NullLogger.Instance,
                    CancellationToken.None);

                Assert.Null(libraryIdentity);
            }
        }

        [Fact]
        public async Task FindPackageAsync_ReturnsLibraryIdentity()
        {
            var library = new Library()
            {
                Identity = new LibraryIdentity("a", new NuGetVersion(1, 0, 0), LibraryType.Package),
                LibraryRange = new LibraryRange(name: "a"),
                Dependencies = Array.Empty<LibraryDependency>()
            };
            var dependencyProvider = new Mock<IDependencyProvider>();

            dependencyProvider.Setup(x => x.GetLibrary(
                    It.IsNotNull<LibraryRange>(),
                    It.IsNotNull<NuGetFramework>()))
                .Returns(library);

            var provider = new LocalDependencyProvider(dependencyProvider.Object);

            using (var sourceCacheContext = new SourceCacheContext())
            {
                var libraryIdentity = await provider.FindLibraryAsync(
                    new LibraryRange(name: "a", typeConstraint: new LibraryDependencyTarget()),
                    NuGetFramework.Parse("net45"),
                    sourceCacheContext,
                    NullLogger.Instance,
                    CancellationToken.None);

                Assert.Same(library.Identity, libraryIdentity);
            }
        }

        [Fact]
        public async Task GetDependenciesAsync_ThrowsForNullLibraryIdentity()
        {
            var provider = new LocalDependencyProvider(Mock.Of<IDependencyProvider>());

            using (var sourceCacheContext = new SourceCacheContext())
            {
                var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                    () => provider.GetDependenciesAsync(
                        libraryIdentity: null,
                        targetFramework: NuGetFramework.Parse("net45"),
                        cacheContext: sourceCacheContext,
                        logger: NullLogger.Instance,
                        cancellationToken: CancellationToken.None));

                Assert.Equal("libraryIdentity", exception.ParamName);
            }
        }

        [Fact]
        public async Task GetDependenciesAsync_ThrowsForNullTargetFramework()
        {
            var provider = new LocalDependencyProvider(Mock.Of<IDependencyProvider>());

            using (var sourceCacheContext = new SourceCacheContext())
            {
                var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                    () => provider.GetDependenciesAsync(
                        new LibraryIdentity(name: "a", version: NuGetVersion.Parse("1.0.0"), type: LibraryType.Package),
                        targetFramework: null,
                        cacheContext: sourceCacheContext,
                        logger: NullLogger.Instance,
                        cancellationToken: CancellationToken.None));

                Assert.Equal("targetFramework", exception.ParamName);
            }
        }

        [Fact]
        public async Task GetDependenciesAsync_ReturnsLibraryIdentity()
        {
            var library = new Library()
            {
                Identity = new LibraryIdentity(
                    name: "a",
                    version: NuGetVersion.Parse("1.0.0"),
                    type: LibraryType.Package),
                Dependencies = new List<LibraryDependency>()
                    {
                        new LibraryDependency(new LibraryRange("b"))
                    },
                LibraryRange = new LibraryRange("a")
            };
            var dependencyProvider = new Mock<IDependencyProvider>();

            dependencyProvider.Setup(x => x.GetLibrary(
                    It.IsNotNull<LibraryRange>(),
                    It.IsNotNull<NuGetFramework>()))
                .Returns(library);

            var provider = new LocalDependencyProvider(dependencyProvider.Object);

            using (var sourceCacheContext = new SourceCacheContext())
            {
                var targetFramework = NuGetFramework.Parse("net45");

                var dependencyInfo = await provider.GetDependenciesAsync(
                    library.Identity,
                    targetFramework,
                    sourceCacheContext,
                    NullLogger.Instance,
                    CancellationToken.None);

                Assert.NotNull(dependencyInfo);
                Assert.Same(library.Dependencies, dependencyInfo.Dependencies);
                Assert.Equal(targetFramework, dependencyInfo.Framework);
                Assert.Same(library.Identity, dependencyInfo.Library);
                Assert.False(dependencyInfo.Resolved);
            }
        }

        [Fact]
        public async Task GetPackageDownloaderAsync_Throws()
        {
            var provider = new LocalDependencyProvider(Mock.Of<IDependencyProvider>());

            using (var sourceCacheContext = new SourceCacheContext())
            {
                await Assert.ThrowsAsync<NotSupportedException>(
                    () => provider.GetPackageDownloaderAsync(
                        new PackageIdentity(id: "a", version: NuGetVersion.Parse("1.0.0")),
                        sourceCacheContext,
                        NullLogger.Instance,
                        CancellationToken.None));
            }
        }
    }
}
