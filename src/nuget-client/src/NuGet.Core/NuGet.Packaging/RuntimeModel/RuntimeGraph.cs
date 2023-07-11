// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using NuGet.Shared;

#nullable enable

namespace NuGet.RuntimeModel
{
    public sealed class RuntimeGraph : IEquatable<RuntimeGraph>
    {
        private static readonly ReadOnlyDictionary<string, RuntimeDescription> EmptyRuntimes = new(new Dictionary<string, RuntimeDescription>());
        private static readonly ReadOnlyDictionary<string, CompatibilityProfile> EmptySupports = new(new Dictionary<string, CompatibilityProfile>());

        // These fields are null when IsEmpty is true
        private readonly ConcurrentDictionary<RuntimeCompatKey, bool>? _areCompatible;
        private readonly ConcurrentDictionary<string, HashSet<string>>? _expandCache;
        private readonly ConcurrentDictionary<RuntimeDependencyKey, List<RuntimePackageDependency>>? _dependencyCache;

        private HashSet<string>? _packagesWithDependencies;

        public static readonly string RuntimeGraphFileName = "runtime.json";

        /// <summary>
        /// Gets a singleton, immutable, empty instance of <see cref="RuntimeGraph"/>.
        /// </summary>
        public static readonly RuntimeGraph Empty = new();

        /// <summary>
        /// Gets a map of <see cref="RuntimeDescription"/> keyed by <see cref="RuntimeDescription.RuntimeIdentifier"/>.
        /// </summary>
        public IReadOnlyDictionary<string, RuntimeDescription> Runtimes { get; }

        /// <summary>
        /// Gets a map of <see cref="CompatibilityProfile"/> keyed by <see cref="CompatibilityProfile.Name"/>.
        /// </summary>
        public IReadOnlyDictionary<string, CompatibilityProfile> Supports { get; }

        public RuntimeGraph()
            : this(EmptyRuntimes, EmptySupports)
        {
        }

        public RuntimeGraph(IEnumerable<RuntimeDescription> runtimes)
            : this(runtimes.ToDictionary(r => r.RuntimeIdentifier), EmptySupports)
        {
        }

        public RuntimeGraph(IEnumerable<CompatibilityProfile> supports)
            : this(EmptyRuntimes, supports.ToDictionary(r => r.Name))
        {
        }

        public RuntimeGraph(IEnumerable<RuntimeDescription> runtimes, IEnumerable<CompatibilityProfile> supports)
            : this(
                  runtimes.ToDictionary(r => r.RuntimeIdentifier),
                  supports.ToDictionary(r => r.Name))
        {
        }

        private RuntimeGraph(IReadOnlyDictionary<string, RuntimeDescription> runtimes, IReadOnlyDictionary<string, CompatibilityProfile> supports)
        {
            Runtimes = runtimes;
            Supports = supports;

            if (Runtimes.Count != 0 || Supports.Count != 0)
            {
                _areCompatible = new();
                _expandCache = new(StringComparer.Ordinal);
                _dependencyCache = new();
            }
        }

        internal bool IsEmpty => Runtimes.Count == 0 && Supports.Count == 0;

        public RuntimeGraph Clone()
        {
            if (IsEmpty)
            {
                return this;
            }

            return new RuntimeGraph(
                runtimes: Runtimes,
                supports: Clone(Supports, s => s.Clone()));

            static IReadOnlyDictionary<string, T> Clone<T>(IReadOnlyDictionary<string, T> source, Func<T, T> cloneFunc)
            {
                if (source.Count == 0)
                {
                    // No need to allocate anything
                    return source;
                }

                Dictionary<string, T> clone = new(source.Count);

                foreach (var pair in source.NoAllocEnumerate())
                {
                    clone[pair.Key] = cloneFunc(pair.Value);
                }

                return clone;
            }
        }

        /// <summary>
        /// Merges the content of two runtime graphs and returns the combined graph.
        /// </summary>
        /// <param name="other">The other graph to merge in to this graph</param>
        public static RuntimeGraph Merge(RuntimeGraph left, RuntimeGraph right)
        {
            if (left.IsEmpty)
            {
                return right;
            }

            if (right.IsEmpty)
            {
                return left;
            }

            var runtimes = new Dictionary<string, RuntimeDescription>(capacity: left.Runtimes.Count + right.Runtimes.Count);

            foreach (var pair in left.Runtimes.NoAllocEnumerate())
            {
                runtimes[pair.Key] = pair.Value;
            }

            // Merge the right-side runtimes
            foreach (var pair in right.Runtimes.NoAllocEnumerate())
            {
                // Check if we already have the runtime defined
                if (runtimes.TryGetValue(pair.Key, out RuntimeDescription? leftRuntime))
                {
                    // Merge runtimes
                    runtimes[pair.Key] = RuntimeDescription.Merge(leftRuntime, pair.Value);
                }
                else
                {
                    runtimes[pair.Key] = pair.Value;
                }
            }

            var supports = new Dictionary<string, CompatibilityProfile>(capacity: right.Supports.Count + left.Supports.Count);

            // Copy over the right ones
            foreach (var compatProfile in right.Supports.NoAllocEnumerate())
            {
                supports[compatProfile.Key] = compatProfile.Value;
            }

            // Overwrite with non-empty profiles from left
            foreach (var compatProfile in left.Supports.NoAllocEnumerate())
            {
                if (compatProfile.Value.RestoreContexts.Count > 0)
                {
                    supports[compatProfile.Key] = compatProfile.Value;
                }
            }

            return new RuntimeGraph(runtimes, supports);
        }

        /// <summary>
        /// Find all compatible RIDs including the current RID.
        /// </summary>
        public IEnumerable<string> ExpandRuntime(string runtime)
        {
            if (IsEmpty)
            {
                return new[] { runtime };
            }

            return ExpandRuntimeCached(runtime);
        }

        private HashSet<string> ExpandRuntimeCached(string runtime)
        {
            return _expandCache!.GetOrAdd(runtime, r => new HashSet<string>(ExpandRuntimeInternal(r), StringComparer.Ordinal));

            // Expand runtimes in a BFS walk. This ensures that nearest RIDs are returned first.
            // Ordering is important for finding the nearest runtime dependency.
            IEnumerable<string> ExpandRuntimeInternal(string runtime)
            {
                yield return runtime;

                // Try to expand the runtime based on the graph
                var deduper = Cache<string>.RentHashSet();
                var expansions = Cache<string>.RentList();
                deduper.Add(runtime);
                expansions.Add(runtime);
                for (var i = 0; i < expansions.Count; i++)
                {
                    // expansions.Count will keep growing as we add items, but that's OK, we want to expand until we stop getting new items
                    if (Runtimes.TryGetValue(expansions[i], out RuntimeDescription? desc))
                    {
                        // Add the inherited runtimes to the list
                        var inheritedRuntimes = desc.InheritedRuntimes;
                        var count = inheritedRuntimes.Count;
                        for (var r = 0; r < count; r++)
                        {
                            var inheritedRuntime = inheritedRuntimes[r];
                            if (deduper.Add(inheritedRuntime))
                            {
                                yield return inheritedRuntime;
                                expansions.Add(inheritedRuntime);
                            }
                        }
                    }
                }

                Cache<string>.ReleaseHashSet(deduper);
                Cache<string>.ReleaseList(expansions);
            }
        }

        /// <summary>
        /// Determines if two runtime identifiers are compatible, based on the import graph
        /// </summary>
        /// <param name="criteria">The criteria being tested</param>
        /// <param name="provided">The value the criteria is being tested against</param>
        /// <returns>
        /// true if an asset for the runtime in <paramref name="provided" /> can be installed in a project
        /// targeting <paramref name="criteria" />, false otherwise
        /// </returns>
        public bool AreCompatible(string criteria, string provided)
        {
            // Identical runtimes are compatible
            if (StringComparer.Ordinal.Equals(criteria, provided))
            {
                return true;
            }

            if (IsEmpty)
            {
                return false;
            }

            var key = new RuntimeCompatKey(criteria, provided);

            return _areCompatible!.GetOrAdd(key, AreCompatibleInternal);

            bool AreCompatibleInternal(RuntimeCompatKey key)
            {
                return ExpandRuntimeCached(key.RuntimeName).Contains(key.Other);
            }
        }

        public IEnumerable<RuntimePackageDependency> FindRuntimeDependencies(string runtimeName, string packageId)
        {
            if (IsEmpty)
            {
                return Enumerable.Empty<RuntimePackageDependency>();
            }

            if (_packagesWithDependencies == null)
            {
                // Find all packages that have runtime dependencies and cache this index.
                _packagesWithDependencies = new HashSet<string>(
                    Runtimes.SelectMany(e => e.Value.RuntimeDependencySets.Select(f => f.Key)),
                    StringComparer.OrdinalIgnoreCase);
            }

            if (_packagesWithDependencies.Contains(packageId))
            {
                var key = new RuntimeDependencyKey(runtimeName, packageId);

                return _dependencyCache!.GetOrAdd(key, FindRuntimeDependenciesInternal);
            }

            return Enumerable.Empty<RuntimePackageDependency>();

            List<RuntimePackageDependency> FindRuntimeDependenciesInternal(RuntimeDependencyKey key)
            {
                // Find all compatible RIDs
                foreach (var expandedRuntime in ExpandRuntimeCached(key.RuntimeName))
                {
                    if (Runtimes.TryGetValue(expandedRuntime, out RuntimeDescription? runtimeDescription))
                    {
                        if (runtimeDescription.RuntimeDependencySets.TryGetValue(key.PackageId, out var dependencySet))
                        {
                            return dependencySet.Dependencies.Values.AsList();
                        }
                    }
                }
                return new List<RuntimePackageDependency>();
            }
        }

        public bool Equals(RuntimeGraph? other)
        {
            // Breaking this up to ease debugging. The optimizer should be able to handle this, so don't refactor unless you have data :).
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Runtimes.OrderedEquals(other.Runtimes, pair => pair.Key, StringComparer.Ordinal)
                && Supports.OrderedEquals(other.Supports, pair => pair.Key, StringComparer.Ordinal);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as RuntimeGraph);
        }

        public override int GetHashCode()
        {
            var hashCode = new HashCodeCombiner();

            hashCode.AddDictionary(Runtimes);
            hashCode.AddDictionary(Supports);

            return hashCode.CombinedHash;
        }

        /// <summary>
        /// Helper for renting hashsets and lists.
        /// </summary>
        private static class Cache<T>
        {
            [ThreadStatic] private static HashSet<T>? _hashSet;
            [ThreadStatic] private static List<T>? _list;

            public static HashSet<T> RentHashSet()
            {
                var hashSet = _hashSet;
                if (hashSet != null)
                {
                    _hashSet = null;
                    return hashSet;
                }

                return new HashSet<T>();
            }

            public static void ReleaseHashSet(HashSet<T> hashSet)
            {
                if (_hashSet == null)
                {
                    hashSet.Clear();
                    _hashSet = hashSet;
                }
            }

            public static List<T> RentList()
            {
                var list = _list;
                if (list != null)
                {
                    _list = null;
                    return list;
                }

                return new List<T>();
            }

            public static void ReleaseList(List<T> list)
            {
                if (_list == null)
                {
                    list.Clear();
                    _list = list;
                }
            }
        }

        /// <summary>
        /// RID + package id
        /// </summary>
        private readonly struct RuntimeDependencyKey : IEquatable<RuntimeDependencyKey>
        {
            public string RuntimeName { get; }

            public string PackageId { get; }

            public RuntimeDependencyKey(string runtimeName, string packageId)
            {
                RuntimeName = runtimeName ?? throw new ArgumentNullException(nameof(runtimeName));
                PackageId = packageId ?? throw new ArgumentNullException(nameof(packageId));
            }

            public bool Equals(RuntimeDependencyKey other)
            {
                return StringComparer.Ordinal.Equals(RuntimeName, other.RuntimeName)
                    && StringComparer.OrdinalIgnoreCase.Equals(PackageId, other.PackageId);
            }

            public override int GetHashCode()
            {
                var hashCode = new HashCodeCombiner();

                hashCode.AddObject(RuntimeName, StringComparer.Ordinal);
                hashCode.AddObject(PackageId, StringComparer.OrdinalIgnoreCase);

                return hashCode.CombinedHash;
            }
        }

        /// <summary>
        /// RID -> RID compatibility key
        /// </summary>
        private readonly struct RuntimeCompatKey : IEquatable<RuntimeCompatKey>
        {
            public string RuntimeName { get; }

            public string Other { get; }

            public RuntimeCompatKey(string runtimeName, string other)
            {
                RuntimeName = runtimeName ?? throw new ArgumentNullException(nameof(runtimeName));
                Other = other ?? throw new ArgumentNullException(nameof(other));
            }

            public bool Equals(RuntimeCompatKey other)
            {
                return StringComparer.Ordinal.Equals(RuntimeName, other.RuntimeName)
                    && StringComparer.Ordinal.Equals(Other, other.Other);
            }

            public override int GetHashCode()
            {
                var hashCode = new HashCodeCombiner();

                hashCode.AddObject(RuntimeName, StringComparer.Ordinal);
                hashCode.AddObject(Other, StringComparer.Ordinal);

                return hashCode.CombinedHash;
            }
        }
    }
}
