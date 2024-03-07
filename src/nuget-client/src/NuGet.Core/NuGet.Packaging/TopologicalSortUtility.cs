// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NuGet.Packaging.Core;

namespace NuGet.Packaging
{
    public static class TopologicalSortUtility
    {
        /// <summary>
        /// Order dependencies by children first.
        /// </summary>
        /// <param name="items">Items to sort.</param>
        /// <param name="comparer">Comparer for Ids.</param>
        /// <param name="getId">Retrieve the id of the item.</param>
        /// <param name="getDependencies">Retrieve dependency ids.</param>
        /// <returns></returns>
        public static IReadOnlyList<T> SortPackagesByDependencyOrder<T>(
            IEnumerable<T> items,
            StringComparer comparer,
            Func<T, string> getId,
            Func<T, string[]> getDependencies) where T : class
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            if (comparer == null)
            {
                throw new ArgumentNullException(nameof(comparer));
            }

            if (getId == null)
            {
                throw new ArgumentNullException(nameof(getId));
            }

            if (getDependencies == null)
            {
                throw new ArgumentNullException(nameof(getDependencies));
            }

            // De-dupe and create a lookup table for mapping items back after sorting
            var lookup = new Dictionary<string, T>(comparer);
            var itemInfos = new List<ItemDependencyInfo>();

            foreach (var item in items)
            {
                var id = getId(item);
                var deps = getDependencies(item);

                if (!lookup.ContainsKey(id))
                {
                    lookup.Add(id, item);
                    itemInfos.Add(new ItemDependencyInfo(id, deps));
                }
            }

            // Sort
            var sortedInfos = SortPackagesByDependencyOrder(itemInfos, comparer);

            // ItemInfo -> Original item
            var sorted = new List<T>(sortedInfos.Count);
            foreach (var item in sortedInfos)
            {
                sorted.Add(lookup[item.Id]);
            }

            return sorted;
        }

        /// <summary>
        /// Order dependencies by children first.
        /// </summary>
        public static IReadOnlyList<PackageDependencyInfo> SortPackagesByDependencyOrder(
            IEnumerable<PackageDependencyInfo> packages)
        {
            return SortPackagesByDependencyOrder(
                packages,
                StringComparer.OrdinalIgnoreCase,
                GetPackageDependencyInfoId,
                GetPackageDependencyInfoDependencies);
        }

        /// <summary>
        /// Order dependencies by children first.
        /// </summary>
        private static List<ItemDependencyInfo> SortPackagesByDependencyOrder(List<ItemDependencyInfo> items, StringComparer comparer)
        {
            var lookup = new Dictionary<string, ItemDependencyInfo>(comparer);
            var itemComparer = new PackageInfoComparer(comparer);

            //Deduplicate references
            foreach (var item in items)
            {
                // These are deduped before they are added here
                lookup.Add(item.Id, item);
            }

            // Extract the deduplicated values
            var toSort = lookup.Values.ToArray();
            var sorted = new List<ItemDependencyInfo>(toSort.Length);

            CalculateRelationships(toSort, lookup);

            for (var i = 0; i < toSort.Length; i++)
            {
                Array.Sort(toSort, i, toSort.Length - i, itemComparer);
                // take the child with the lowest number of children
                var package = toSort[i];
                sorted.Add(package);
                UpdateChildCounts(package);
            }

            // the list is ordered by parents first, reverse to run children first
            sorted.Reverse();

            return sorted;
        }

        private static void UpdateChildCounts(ItemDependencyInfo package)
        {
            // Decrement the parent count for each child of this package.
            var children = package.Children;
            if (children != null)
            {
                var count = children.Count;
                for (var i = 0; i < count; i++)
                {
                    children[i].ActiveParents--;
                }
            }
        }

        private static void CalculateRelationships(ItemDependencyInfo[] packages, Dictionary<string, ItemDependencyInfo> lookup)
        {
            foreach (var package in packages)
            {
                var dependencies = package.DependencyIds ?? Array.Empty<string>();

                for (var i = 0; i < dependencies.Length; i++)
                {
                    var id = dependencies[i];
                    if (lookup.TryGetValue(id, out var dependencyPackage))
                    {
                        // Mark the current package as a parent
                        var parents = dependencyPackage.Parents;
                        if (parents == null)
                        {
                            parents = new List<ItemDependencyInfo>();
                            dependencyPackage.Parents = parents;
                        }
                        parents.Add(package);

                        // Add a child package for the current package
                        var packageChildren = package.Children;
                        if (packageChildren == null)
                        {
                            packageChildren = new List<ItemDependencyInfo>(dependencies.Length - i);
                            package.Children = packageChildren;
                        }
                        packageChildren.Add(dependencyPackage);
                    }
                }
            }

            foreach (var package in packages)
            {
                package.ActiveParents = package.Parents?.Count ?? 0;
            }
        }


        private static string GetPackageDependencyInfoId(PackageDependencyInfo info)
        {
            return info.Id;
        }

        private static string[] GetPackageDependencyInfoDependencies(PackageDependencyInfo info)
        {
            return info.Dependencies.Select(e => e.Id).ToArray();
        }

        private class PackageInfoComparer : IComparer<ItemDependencyInfo>
        {
            private readonly StringComparer _comparer;

            public PackageInfoComparer(StringComparer comparer)
            {
                _comparer = comparer ?? throw new ArgumentNullException(nameof(comparer));
            }

            public int Compare(ItemDependencyInfo x, ItemDependencyInfo y)
            {
                // Order packages by parent count
                if (x.ActiveParents < y.ActiveParents)
                {
                    return -1;
                }
                if (x.ActiveParents > y.ActiveParents)
                {
                    return 1;
                }

                return _comparer.Compare(x.Id, y.Id);
            }
        }

        [DebuggerDisplay("{Package.Id} Active: {ActiveParents}")]
        private sealed class ItemDependencyInfo
        {
            public ItemDependencyInfo(string id, string[] dependencyIds)
            {
                ActiveParents = 0;
                Id = id;
                DependencyIds = dependencyIds;
            }

            public string Id;
            public string[] DependencyIds;

            public int ActiveParents;
            public List<ItemDependencyInfo> Parents;
            public List<ItemDependencyInfo> Children;
        }
    }
}
