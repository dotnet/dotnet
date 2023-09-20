// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System.Collections.Generic;
using System.Linq;
using NuGet.Configuration;
using NuGet.PackageManagement.VisualStudio;

namespace NuGet.PackageManagement.UI
{
    internal static class PackageSourceMappingUtility
    {
        /// <summary>
        /// Determines if new package source mappings should be written to the <paramref name="sourceMappingProvider"/> which is returned as a count.
        /// </summary>
        /// <param name="userAction"></param>
        /// <param name="addedPackageIds"></param>
        /// <param name="sourceMappingProvider"></param>
        /// <param name="existingPackageSourceMappingSourceItems"></param>
        /// <param name="countCreatedTopLevelSourceMappings">For Top-Level packages: <see langword="null" /> if not applicable; 0 if none needed to be added; > 0 is the count of new package source mappings added.</param>
        /// <param name="countCreatedTransitiveSourceMappings">For Transitive packages: <see langword="null" /> if not applicable; 0 if none needed to be added; > 0 is the count of new package source mappings added.</param>
        internal static void ConfigureNewPackageSourceMapping(
            UserAction? userAction,
            IReadOnlyList<string>? addedPackageIds,
            PackageSourceMappingProvider sourceMappingProvider,
            IReadOnlyList<PackageSourceMappingSourceItem> existingPackageSourceMappingSourceItems,
            out int? countCreatedTopLevelSourceMappings,
            out int? countCreatedTransitiveSourceMappings)
        {
            countCreatedTopLevelSourceMappings = null;
            countCreatedTransitiveSourceMappings = null;

            if (userAction?.SelectedSourceName is null || addedPackageIds is null)
            {
                return;
            }

            countCreatedTopLevelSourceMappings = 0;
            countCreatedTransitiveSourceMappings = 0;

            string topLevelPackageId = userAction.PackageId;
            Dictionary<string, IReadOnlyList<string>> patternsReadOnly = existingPackageSourceMappingSourceItems
                .ToDictionary(pair => pair.Key, pair => (IReadOnlyList<string>)(pair.Patterns.Select(p => p.Pattern).ToList()));

            PackageSourceMapping packageSourceMapping = new(patternsReadOnly);

            // Expand all patterns/globs so we can later check if this package ID was already mapped.
            List<string> addedPackageIdsWithoutExistingMappings = new(capacity: addedPackageIds.Count + 1);

            foreach (string addedPackageId in addedPackageIds)
            {
                IReadOnlyList<string> configuredSource = packageSourceMapping.GetConfiguredPackageSources(addedPackageId);

                // Top-level package was looked up.
                if (addedPackageId == topLevelPackageId)
                {
                    // The top-level package is not already mapped to the selected source.
                    if (configuredSource.Count == 0 || !configuredSource.Contains(userAction.SelectedSourceName))
                    {
                        countCreatedTopLevelSourceMappings = 1;
                        addedPackageIdsWithoutExistingMappings.Add(topLevelPackageId);
                    }
                }
                // Transitive package was looked up.
                else if (configuredSource.Count == 0)
                {
                    addedPackageIdsWithoutExistingMappings.Add(addedPackageId);
                }
            }

            countCreatedTransitiveSourceMappings = addedPackageIdsWithoutExistingMappings.Count - countCreatedTopLevelSourceMappings;

            CreateAndSavePackageSourceMappings(
                userAction.SelectedSourceName,
                addedPackageIdsWithoutExistingMappings,
                sourceMappingProvider,
                existingPackageSourceMappingSourceItems);
        }

        private static void CreateAndSavePackageSourceMappings(
            string sourceName,
            List<string> newPackageIdsToSourceMap,
            PackageSourceMappingProvider mappingProvider,
            IReadOnlyList<PackageSourceMappingSourceItem> existingPackageSourceMappingSourceItems)
        {
            if (string.IsNullOrWhiteSpace(sourceName) || newPackageIdsToSourceMap is null || newPackageIdsToSourceMap.Count == 0)
            {
                return;
            }

            IEnumerable<PackagePatternItem> newPackagePatternItems = newPackageIdsToSourceMap.Select(packageId => new PackagePatternItem(packageId));

            List<PackageSourceMappingSourceItem> newAndExistingPackageSourceMappingItems = new(existingPackageSourceMappingSourceItems);

            PackageSourceMappingSourceItem packageSourceMappingItemForSource =
                existingPackageSourceMappingSourceItems
                .Where(mappingItem => mappingItem.Key == sourceName)
                .FirstOrDefault();

            // Source is being mapped for the first time.
            if (packageSourceMappingItemForSource is null)
            {
                packageSourceMappingItemForSource = new PackageSourceMappingSourceItem(sourceName, newPackagePatternItems);
                newAndExistingPackageSourceMappingItems.Add(packageSourceMappingItemForSource);
            }
            else // Source already had an existing mapping.
            {
                foreach (PackagePatternItem newPattern in newPackagePatternItems)
                {
                    if (!packageSourceMappingItemForSource.Patterns.Contains(newPattern))
                    {
                        packageSourceMappingItemForSource.Patterns.Add(newPattern);
                    }
                }
            }

            mappingProvider.SavePackageSourceMappings(newAndExistingPackageSourceMappingItems);
        }

        internal static string? GetNewSourceMappingSourceName(PackageSourceMapping packageSourceMapping, PackageSourceMoniker activePackageSourceMoniker)
        {
            string? sourceMappingSourceName = packageSourceMapping.IsEnabled
                && !activePackageSourceMoniker.IsAggregateSource
                ? activePackageSourceMoniker.PackageSourceNames.First() : null;

            return sourceMappingSourceName;
        }

        internal static void AddNewSourceMappingsFromAddedPackages(ref Dictionary<string, SortedSet<string>>? newSourceMappings, string newMappingSourceName, List<AccessiblePackageIdentity> added, PackageSourceMapping packageSourceMapping)
        {
            if (newMappingSourceName is null || added.Count == 0 || packageSourceMapping is null)
            {
                return;
            }

            foreach (AccessiblePackageIdentity addedPackage in added)
            {
                IReadOnlyList<string> configuredSources = packageSourceMapping.GetConfiguredPackageSources(packageId: addedPackage.Id);

                if (configuredSources.Count > 0)
                {
                    continue;
                }

                if (newSourceMappings is null)
                {
                    newSourceMappings = new Dictionary<string, SortedSet<string>>(capacity: 1)
                    {
                        { newMappingSourceName, new SortedSet<string>(new List<string>(capacity: added.Count) { addedPackage.Id }) }
                    };
                }
                else if (newSourceMappings.TryGetValue(newMappingSourceName, out SortedSet<string>? newMappingPackageIds))
                {
                    newMappingPackageIds.Add(addedPackage.Id);
                }
                else
                {
                    newSourceMappings.Add(newMappingSourceName, new SortedSet<string>(new List<string>(capacity: added.Count) { addedPackage.Id }));
                }
            }
        }
    }
}
