// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace NuGet.Resolver
{
    public static class ResolverInputSort
    {
        /// <summary>
        /// Order package trees into a flattened list
        /// 
        /// Package Id (Parent count)
        /// Iteration 1: A(0) -> B(1) -> D(2)
        ///              C(0) -> D(2)
        ///             [Select A]
        /// 
        /// Iteration 2: B(0) -> D(2)
        ///              C(0) -> D(2)
        ///             [Select B]
        /// 
        /// Iteration 2: C(0) -> D(1)
        ///             [Select C]
        ///
        /// Result: A, B, C, D
        /// </summary>
        public static List<List<ResolverPackage>> TreeFlatten(List<List<ResolverPackage>> grouped, PackageResolverContext context)
        {
            var sorted = new List<List<ResolverPackage>>();

            // find all package ids
            var groupIds = grouped.Select(group => group.First().Id).ToList();

            // find all dependencies for each id
            var dependencies = grouped.Select(group => new SortedSet<string>(
                group.SelectMany(g => g.Dependencies)
                .Select(d => d.Id), StringComparer.OrdinalIgnoreCase))
                .ToList();

            //  track all parents of an id
            var parents = new Dictionary<string, SortedSet<string>>(StringComparer.OrdinalIgnoreCase);

            for (int i = 0; i < grouped.Count; i++)
            {
                var parentsForId = new SortedSet<string>(StringComparer.OrdinalIgnoreCase);

                for (int j = 0; j < grouped.Count; j++)
                {
                    if (i != j && dependencies[j].Contains(groupIds[i]))
                    {
                        parentsForId.Add(groupIds[j]);
                    }
                }

                parents.Add(groupIds[i], parentsForId);
            }

            var idsToSort = new List<string>(groupIds);

            var childrenOfLastId = new SortedSet<string>(StringComparer.OrdinalIgnoreCase);

            // Loop through the package ids taking the best one each time
            // and removing it from the parent list.
            while (idsToSort.Count > 0)
            {
                // 1. Lowest number of parents remaining goes
                // 2. Prefer children of the last id sorted next
                // 3. Installed, target, then new package
                // 4. Highest number of dependencies goes first
                // 5. Fallback to string sort
                var nextId = idsToSort.OrderBy(id => parents[id].Count)
                    .ThenBy(id => childrenOfLastId.Contains(id) ? 0 : 1)
                    .ThenBy(id => GetTreeFlattenPriority(id, context))
                    .ThenByDescending(id => parents.Values.Count(parentIds => parentIds.Contains(id)))
                    .ThenBy(id => id, StringComparer.OrdinalIgnoreCase)
                    .First();

                // Find the group for the best id
                var nextGroup = grouped.Single(group => StringComparer.OrdinalIgnoreCase.Equals(group.First().Id, nextId));
                sorted.Add(nextGroup);

                childrenOfLastId.Clear();

                // Remove the id from the parent list now that we have found a place for it
                foreach ((var childId, var parentIds) in parents)
                {
                    if (parentIds.Remove(nextId))
                    {
                        childrenOfLastId.Add(childId);
                    }
                }

                // Complete the id
                grouped.Remove(nextGroup);
                idsToSort.Remove(nextId);
            }

            return sorted;
        }

        /// <summary>
        /// Packages occuring first are more likely to get their preferred version, for this 
        /// reason installed packages should go first, then targets.
        /// </summary>
        private static int GetTreeFlattenPriority(string id, PackageResolverContext context)
        {
            // Targets go in the middle
            // this needs to be checked first since the target may also exist in the installed packages (upgrade)
            if (context.TargetIds.Contains(id, StringComparer.OrdinalIgnoreCase))
            {
                return 1;
            }

            // Installed packages go first
            if (context.PackagesConfig.Select(package => package.PackageIdentity.Id).Contains(id, StringComparer.OrdinalIgnoreCase))
            {
                return 0;
            }

            // New dependencies go last
            return 2;
        }
    }
}
