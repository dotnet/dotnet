// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.DependencyResolver;
using NuGet.Frameworks;
using NuGet.LibraryModel;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;

namespace Test.Utility
{
    public class DependencyProvider : IRemoteDependencyProvider, IDependencyProvider
    {
        private readonly Dictionary<LibraryIdentity, List<LibraryDependency>> _graph = new Dictionary<LibraryIdentity, List<LibraryDependency>>();

        public bool IsHttp => false;

        public PackageSource Source => new PackageSource("Test");

        public SourceRepository SourceRepository => throw new NotImplementedException();

        public Task<IPackageDownloader> GetPackageDownloaderAsync(
            PackageIdentity packageIdentity,
            SourceCacheContext cacheContext,
            ILogger logger,
            CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<LibraryIdentity> FindLibraryAsync(
            LibraryRange libraryRange,
            NuGetFramework targetFramework,
            SourceCacheContext cacheContext,
            ILogger logger,
            CancellationToken cancellationToken)
        {
            var packages = _graph.Keys.Where(p => p.Name == libraryRange.Name);

            // Yield the task to help uncovering concurrency issues in tests
            await Task.Yield();

            return packages.FindBestMatch(libraryRange.VersionRange, i => i?.Version);
        }

        public Task<IEnumerable<NuGetVersion>> GetAllVersionsAsync(string id, SourceCacheContext cacheContext, ILogger logger, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public async Task<LibraryDependencyInfo> GetDependenciesAsync(
            LibraryIdentity match,
            NuGetFramework targetFramework,
            SourceCacheContext cacheContext,
            ILogger logger,
            CancellationToken cancellationToken)
        {
            List<LibraryDependency> dependencies;

            // Yield the task to help uncovering concurrency issues in tests
            await Task.Yield();

            if (_graph.TryGetValue(match, out dependencies))
            {
                return LibraryDependencyInfo.Create(match, targetFramework, dependencies);
            }

            return LibraryDependencyInfo.Create(match, targetFramework, Enumerable.Empty<LibraryDependency>());
        }

        public bool SupportsType(LibraryDependencyTarget libraryType)
        {
            return (libraryType & (LibraryDependencyTarget.Project | LibraryDependencyTarget.ExternalProject)) != LibraryDependencyTarget.None;
        }

        public TestPackage Package(string id, string version)
        {
            return Package(id, NuGetVersion.Parse(version), LibraryType.Package);
        }

        public TestPackage Package(string id, string version, LibraryType type)
        {
            return Package(id, NuGetVersion.Parse(version), type);
        }

        public TestPackage Package(string id, NuGetVersion version, LibraryType type)
        {
            var libraryIdentity = new LibraryIdentity { Name = id, Version = version, Type = type };

            List<LibraryDependency> dependencies;
            if (!_graph.TryGetValue(libraryIdentity, out dependencies))
            {
                dependencies = new List<LibraryDependency>();
                _graph[libraryIdentity] = dependencies;
            }

            return new TestPackage(dependencies);
        }

        public Library GetLibrary(LibraryRange libraryRange, NuGetFramework targetFramework)
        {
            var packages = _graph.Keys.Where(p => p.Name == libraryRange.Name);
            var identity = packages.FindBestMatch(libraryRange.VersionRange, i => i?.Version);

            if (identity != null)
            {
                var dependency = _graph.TryGetValue(identity, out var dependencies) ? dependencies : Enumerable.Empty<LibraryDependency>();

                return new Library
                {
                    LibraryRange = libraryRange,
                    Identity = identity,
                    Path = null,
                    Dependencies = dependency,
                    Resolved = true
                };
            }

            return null;
        }

        public class TestPackage
        {
            private List<LibraryDependency> _dependencies;

            public TestPackage(List<LibraryDependency> dependencies)
            {
                _dependencies = dependencies;
            }

            public TestPackage DependsOn(string id, LibraryDependencyTarget target = LibraryDependencyTarget.All)
            {
                _dependencies.Add(new LibraryDependency
                {
                    LibraryRange = new LibraryRange
                    {
                        Name = id,
                        TypeConstraint = target
                    }
                });

                return this;
            }

            public TestPackage DependsOn(string id, string version, LibraryDependencyTarget target = LibraryDependencyTarget.All, bool versionCentrallyManaged = false, LibraryDependencyReferenceType? libraryDependencyReferenceType = null, LibraryIncludeFlags? privateAssets = null)
            {
                var suppressParent = privateAssets != null ? privateAssets.Value : LibraryIncludeFlagUtils.DefaultSuppressParent;
                var referenceType = libraryDependencyReferenceType != null ? libraryDependencyReferenceType.Value : LibraryDependencyReferenceType.Direct;
                var libraryDependency = new LibraryDependency
                {
                    LibraryRange =
                        new LibraryRange
                        {
                            Name = id,
                            VersionRange = VersionRange.Parse(version),
                            TypeConstraint = target
                        },
                    ReferenceType = referenceType,
                    SuppressParent = suppressParent,
                    VersionCentrallyManaged = versionCentrallyManaged,
                };

                _dependencies.Add(libraryDependency);

                return this;
            }
        }
    }
}
