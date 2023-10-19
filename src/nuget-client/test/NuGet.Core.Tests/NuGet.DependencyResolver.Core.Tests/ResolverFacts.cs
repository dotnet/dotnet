// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Frameworks;
using NuGet.LibraryModel;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;
using Test.Utility;
using Xunit;

namespace NuGet.DependencyResolver.Core.Tests
{
    public class ResolverFacts
    {
        [Fact]
        public async Task FasterProviderReturnsResultsBeforeSlowOnesIfExactMatchFound()
        {
            // A 
            var slowProvider = new TestProvider(TimeSpan.FromSeconds(2));
            slowProvider.AddLibrary(new LibraryIdentity
            {
                Name = "A",
                Version = new NuGetVersion("1.0.0"),
                Type = LibraryType.Package
            });

            var fastProvider = new TestProvider(TimeSpan.Zero);
            fastProvider.AddLibrary(new LibraryIdentity
            {
                Name = "A",
                Version = new NuGetVersion("1.0.0"),
                Type = LibraryType.Package
            });

            var context = new TestRemoteWalkContext();
            context.RemoteLibraryProviders.Add(slowProvider);
            context.RemoteLibraryProviders.Add(fastProvider);

            var walker = new RemoteDependencyWalker(context);
            var result = await walker.WalkAsync(new LibraryRange
            {
                Name = "A",
                VersionRange = VersionRange.Parse("1.0.0"),
            },
            NuGetFramework.Parse("net45"),
            runtimeIdentifier: null,
            runtimeGraph: null,
            recursive: true);

            Assert.NotNull(result.Item.Data.Match);
            Assert.NotNull(result.Item.Data.Match.Library);
            Assert.Equal("A", result.Item.Data.Match.Library.Name);
            Assert.Equal(new NuGetVersion("1.0.0"), result.Item.Data.Match.Library.Version);
            Assert.Equal(fastProvider, result.Item.Data.Match.Provider);
        }

        [Fact]
        public async Task SlowerFeedWinsIfBetterMatchExists()
        {
            // A 
            var slowProvider = new TestProvider(TimeSpan.FromSeconds(2));
            slowProvider.AddLibrary(new LibraryIdentity
            {
                Name = "A",
                Version = new NuGetVersion("1.0.0"),
                Type = LibraryType.Package
            });

            var fastProvider = new TestProvider(TimeSpan.Zero);
            fastProvider.AddLibrary(new LibraryIdentity
            {
                Name = "A",
                Version = new NuGetVersion("1.1.0"),
                Type = LibraryType.Package
            });

            var context = new TestRemoteWalkContext();
            context.RemoteLibraryProviders.Add(slowProvider);
            context.RemoteLibraryProviders.Add(fastProvider);

            var walker = new RemoteDependencyWalker(context);
            var result = await walker.WalkAsync(new LibraryRange
            {
                Name = "A",
                VersionRange = VersionRange.Parse("1.0.0"),
            },
            NuGetFramework.Parse("net45"),
            runtimeIdentifier: null,
            runtimeGraph: null,
            recursive: true);

            Assert.NotNull(result.Item.Data.Match);
            Assert.NotNull(result.Item.Data.Match.Library);
            Assert.Equal("A", result.Item.Data.Match.Library.Name);
            Assert.Equal(new NuGetVersion("1.0.0"), result.Item.Data.Match.Library.Version);
            Assert.Equal(slowProvider, result.Item.Data.Match.Provider);
        }

        public class TestProvider : IRemoteDependencyProvider
        {
            private readonly TimeSpan _delay;
            private readonly List<LibraryIdentity> _libraries = new List<LibraryIdentity>();

            public TestProvider(TimeSpan delay)
            {
                _delay = delay;
            }

            public void AddLibrary(LibraryIdentity identity)
            {
                _libraries.Add(identity);
            }

            public bool IsHttp => true;

            public PackageSource Source => new PackageSource("Test");

            public SourceRepository SourceRepository => throw new NotImplementedException();

            public async Task<LibraryIdentity> FindLibraryAsync(
                LibraryRange libraryRange,
                NuGetFramework targetFramework,
                SourceCacheContext cacheContext,
                ILogger logger,
                CancellationToken cancellationToken)
            {
                if (_delay != TimeSpan.Zero)
                {
                    await Task.Delay(_delay, cancellationToken);
                }

                return _libraries.FindBestMatch(libraryRange.VersionRange, l => l?.Version);
            }

            public Task<LibraryDependencyInfo> GetDependenciesAsync(
                LibraryIdentity match,
                NuGetFramework targetFramework,
                SourceCacheContext cacheContext,
                ILogger logger,
                CancellationToken cancellationToken)
            {
                return Task.FromResult(LibraryDependencyInfo.Create(match, targetFramework, Enumerable.Empty<LibraryDependency>()));
            }

            public Task<IEnumerable<NuGetVersion>> GetAllVersionsAsync(
                string id,
                SourceCacheContext cacheContext,
                ILogger logger,
                CancellationToken token)
            {
                return Task.FromResult(_libraries.Select(e => e.Version));
            }

            public Task<IPackageDownloader> GetPackageDownloaderAsync(
                PackageIdentity packageIdentity,
                SourceCacheContext cacheContext,
                ILogger logger,
                CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }
        }
    }
}
