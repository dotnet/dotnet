// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.LibraryModel;
using NuGet.Protocol.Core.Types;

namespace NuGet.DependencyResolver
{
    public class RemoteWalkContext
    {
        public RemoteWalkContext(SourceCacheContext cacheContext, PackageSourceMapping packageSourceMapping, ILogger logger)
        {
            CacheContext = cacheContext ?? throw new ArgumentNullException(nameof(cacheContext));
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));

            ProjectLibraryProviders = new List<IDependencyProvider>();
            LocalLibraryProviders = new List<IRemoteDependencyProvider>();
            RemoteLibraryProviders = new List<IRemoteDependencyProvider>();
            PackageSourceMapping = packageSourceMapping ?? throw new ArgumentNullException(nameof(packageSourceMapping));

            FindLibraryEntryCache = new TaskResultCache<LibraryRangeCacheKey, GraphItem<RemoteResolveResult>>();
            ResolvePackageLibraryMatchCache = new TaskResultCache<LibraryRange, Tuple<LibraryRange, RemoteMatch>>();

            LockFileLibraries = new Dictionary<LockFileCacheKey, IList<LibraryIdentity>>();
        }

        public SourceCacheContext CacheContext { get; }
        public ILogger Logger { get; }
        public IList<IDependencyProvider> ProjectLibraryProviders { get; }
        public IList<IRemoteDependencyProvider> LocalLibraryProviders { get; }
        public IList<IRemoteDependencyProvider> RemoteLibraryProviders { get; }
        public PackageSourceMapping PackageSourceMapping { get; }

        /// <summary>
        /// Packages lock file libraries to be used while generating restore graph.
        /// </summary>
        public IDictionary<LockFileCacheKey, IList<LibraryIdentity>> LockFileLibraries { get; }

        /// <summary>
        /// Library entry cache.
        /// </summary>
        internal TaskResultCache<LibraryRangeCacheKey, GraphItem<RemoteResolveResult>> FindLibraryEntryCache { get; }

        internal TaskResultCache<LibraryRange, Tuple<LibraryRange, RemoteMatch>> ResolvePackageLibraryMatchCache { get; }

        /// <summary>
        /// True if this is a csproj or similar project. Xproj should be false.
        /// </summary>
        public bool IsMsBuildBased { get; set; }

        /// <summary>
        /// Applies source mapping pattern filtering for a given package
        /// </summary>
        /// <param name="libraryRange"></param>
        /// <returns>Returns a subset of sources when source mapping patterns are configured otherwise returns all the sources</returns>
        public IList<IRemoteDependencyProvider> FilterDependencyProvidersForLibrary(LibraryRange libraryRange)
        {
            if (libraryRange == default)
                throw new ArgumentNullException(nameof(libraryRange));

            // filter package patterns if enabled            
            if (PackageSourceMapping?.IsEnabled == true && libraryRange.TypeConstraintAllows(LibraryDependencyTarget.Package))
            {
                IReadOnlyList<string> sources = PackageSourceMapping.GetConfiguredPackageSources(libraryRange.Name);

                if (sources.Count == 0)
                {
                    return Array.Empty<IRemoteDependencyProvider>();
                }

                // PERF: Avoid Linq in hot paths.
                var filteredLibraryProviders = new List<IRemoteDependencyProvider>(sources.Count);
                for (int i = 0; i < RemoteLibraryProviders.Count; ++i)
                {
                    var current = RemoteLibraryProviders[i];
                    for (int j = 0; j < sources.Count; ++j)
                    {
                        if (StringComparer.Ordinal.Equals(sources[j], current.Source.Name))
                        {
                            filteredLibraryProviders.Add(current);
                            break;
                        }
                    }
                }
                return filteredLibraryProviders;
            }
            return RemoteLibraryProviders;
        }

        /// <summary>
        /// Returns a list of unresolved remote matches.
        /// </summary>
        /// <remarks>
        /// The <see cref="FindLibraryEntryCache" /> is internal but the dependency resolver needs to know what packages were unresolved after walking the dependency graph.
        /// </remarks>
        /// <returns>A <see cref="HashSet{T}" /> containing the <see cref="RemoteMatch" /> objects representing unresolved packages.</returns>
        public async Task<HashSet<RemoteMatch>> GetUnresolvedRemoteMatchesAsync()
        {
            HashSet<RemoteMatch> packagesToInstall = new();

            foreach (LibraryRangeCacheKey key in FindLibraryEntryCache.Keys.NoAllocEnumerate())
            {
                if (!FindLibraryEntryCache.TryGetValue(key, out Task<GraphItem<RemoteResolveResult>>? task))
                {
                    continue;
                }

                GraphItem<RemoteResolveResult> item = await task;

                if (item.Key.Type == LibraryType.Unresolved || !RemoteLibraryProviders.Contains(item.Data.Match.Provider))
                {
                    continue;
                }

                packagesToInstall.Add(item.Data.Match);
            }

            return packagesToInstall;
        }
    }
}
