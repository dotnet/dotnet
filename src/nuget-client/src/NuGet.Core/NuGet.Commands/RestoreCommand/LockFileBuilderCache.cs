// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using NuGet.ContentModel;
using NuGet.Frameworks;
using NuGet.LibraryModel;
using NuGet.ProjectModel;
using NuGet.Repositories;
using NuGet.Shared;
using NuGet.Versioning;

namespace NuGet.Commands
{

    /// <summary>
    /// Cache objects used for building the lock file.
    /// </summary>
    public class LockFileBuilderCache
    {
        // Package files
        private readonly ConcurrentDictionary<(string Id, NuGetVersion Version, string sha512), ContentItemCollection> _contentItems
            = new();

        // OrderedCriteria is stored per target graph + override framework.
        private readonly ConcurrentDictionary<CriteriaKey, List<(List<SelectionCriteria>, bool)>> _criteriaSets =
            new();

        private readonly ConcurrentDictionary<(CriteriaKey, string path, string aliases, LibraryIncludeFlags), Lazy<(LockFileTargetLibrary, bool)>> _lockFileTargetLibraryCache =
            new();

        /// <summary>
        /// Get ordered selection criteria.
        /// <paramref name="graph">RestoreTargetGraph to be used. Must not be null.</paramref>
        /// <paramref name="framework">Framework to be used. Must not be null.</paramref>
        /// </summary>
        /// <remarks>
        /// For performance reasons(detecting AssetTargetFallback warnings), this is not used in the current restore code. <see cref="GetLabeledSelectionCriteria(RestoreTargetGraph, NuGetFramework)"/> is used instead.
        /// This method is not being marked as obsolete despite being unused in the NuGet product, as at this point there's no reason for the replacement method needs to be public.
        /// </remarks>
        public List<List<SelectionCriteria>> GetSelectionCriteria(RestoreTargetGraph graph, NuGetFramework framework)
        {
            _ = graph ?? throw new ArgumentNullException(nameof(graph));
            _ = framework ?? throw new ArgumentNullException(nameof(framework));
            // Criteria are unique on graph and framework override.
            var key = new CriteriaKey(graph.TargetGraphName, framework);
            List<(List<SelectionCriteria> selectionCriterias, bool fallbackUsed)> result = _criteriaSets.GetOrAdd(key, _ => LockFileUtils.CreateOrderedCriteriaSets(graph.Conventions, framework, runtimeIdentifier: graph.RuntimeIdentifier));
            return result.Select(e => e.selectionCriterias).ToList();
        }

        /// <summary>
        /// Get ordered selection criteria.
        /// Each boolean of the value tuple says whether the criteria itself is a fallback criteria.
        /// </summary>
        /// <paramref name="graph">RestoreTargetGraph to be used. Must not be null.</paramref>
        /// <paramref name="framework">Framework to be used. Must not be null.</paramref>
        /// <returns>Returns a list of ordered criteria, along with a boolean that says whether the criteria is generated for a fallback framework.</returns>
        /// <exception cref="ArgumentNullException">If graph or framework is null.</exception>
        /// <remarks>This method being internal is inconsistent with the rest of the methods in this class,
        /// but given that this class is only used in the current assembly it would have been the best if it was never public, but we can't turn back time.
        /// </remarks>
        internal List<(List<SelectionCriteria>, bool)> GetLabeledSelectionCriteria(RestoreTargetGraph graph, NuGetFramework framework)
        {
            _ = graph ?? throw new ArgumentNullException(nameof(graph));
            _ = framework ?? throw new ArgumentNullException(nameof(framework));
            // Criteria are unique on graph and framework override.
            var key = new CriteriaKey(graph.TargetGraphName, framework);
            return _criteriaSets.GetOrAdd(key, _ => LockFileUtils.CreateOrderedCriteriaSets(graph.Conventions, framework, runtimeIdentifier: graph.RuntimeIdentifier));
        }

        /// <summary>
        /// Get a ContentItemCollection of the package files.
        /// </summary>
        /// <remarks>Library is optional.</remarks>
        public ContentItemCollection GetContentItems(LockFileLibrary library, LocalPackageInfo package)
        {
            if (package == null)
            {
                throw new ArgumentNullException(nameof(package));
            }

            return _contentItems.GetOrAdd((package.Id, package.Version, package.Sha512), _ =>
            {
                var collection = new ContentItemCollection();

                if (library == null)
                {
                    // Read folder
                    collection.Load(package.Files);
                }
                else
                {
                    // Use existing library
                    collection.Load(library.Files);
                }

                return collection;
            });
        }

        /// <summary>
        /// Try to get a LockFileTargetLibrary from the cache.
        /// </summary>
        internal (LockFileTargetLibrary, bool) GetLockFileTargetLibrary(RestoreTargetGraph graph, NuGetFramework framework, LocalPackageInfo localPackageInfo, string aliases, LibraryIncludeFlags libraryIncludeFlags, Func<(LockFileTargetLibrary, bool)> valueFactory)
        {
            // Comparing RuntimeGraph for equality is very expensive,
            // so in case of a request where the RuntimeGraph is not empty we avoid using the cache.
            if (!string.IsNullOrEmpty(graph.RuntimeIdentifier))
                return valueFactory();

            localPackageInfo = localPackageInfo ?? throw new ArgumentNullException(nameof(localPackageInfo));
            var criteriaKey = new CriteriaKey(graph.TargetGraphName, framework);
            var packagePath = localPackageInfo.ExpandedPath;
            return _lockFileTargetLibraryCache.GetOrAdd((criteriaKey, packagePath, aliases, libraryIncludeFlags),
                key => new Lazy<(LockFileTargetLibrary, bool)>(valueFactory)).Value;
        }

        private class CriteriaKey : IEquatable<CriteriaKey>
        {
            public string TargetGraphName { get; }

            public NuGetFramework Framework { get; }

            public AssetTargetFallbackFramework AssetTargetFallbackFramework { get; }

            public CriteriaKey(string targetGraphName, NuGetFramework frameworkOverride)
            {
                TargetGraphName = targetGraphName;
                if (frameworkOverride is AssetTargetFallbackFramework assetTargetFallbackFramework)
                {
                    Framework = null;
                    AssetTargetFallbackFramework = assetTargetFallbackFramework;
                }
                else
                {
                    Framework = frameworkOverride;
                    AssetTargetFallbackFramework = null;
                }
            }

            public bool Equals(CriteriaKey other)
            {
                if (ReferenceEquals(this, other))
                {
                    return true;
                }

                if (ReferenceEquals(other, null))
                {
                    return false;
                }

                return StringComparer.Ordinal.Equals(TargetGraphName, other.TargetGraphName)
                       && NuGetFramework.Comparer.Equals(Framework, other.Framework)
                       && (AssetTargetFallbackFramework == null && other.AssetTargetFallbackFramework == null ||
                           AssetTargetFallbackFramework != null && AssetTargetFallbackFramework.Equals(other.AssetTargetFallbackFramework));
            }

            public override bool Equals(object obj)
            {
                return Equals(obj as CriteriaKey);
            }

            public override int GetHashCode()
            {
                var combiner = new HashCodeCombiner();

                combiner.AddObject(StringComparer.Ordinal.GetHashCode(TargetGraphName));
                combiner.AddObject(Framework);
                combiner.AddObject(AssetTargetFallbackFramework);

                return combiner.CombinedHash;
            }
        }
    }
}
