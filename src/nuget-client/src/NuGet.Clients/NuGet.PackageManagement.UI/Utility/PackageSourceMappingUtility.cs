// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.PackageManagement.VisualStudio;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Protocol.Core.Types;

namespace NuGet.PackageManagement.UI
{
    internal static class PackageSourceMappingUtility
    {
        /// <summary>
        /// Determines if new package source mappings should be written to the <paramref name="sourceMappingProvider"/> which is returned as a count.
        /// </summary>
        /// <param name="userAction"></param>
        /// <param name="sourceMappingPreviewResult"></param>
        /// <param name="sourceMappingProvider"></param>
        /// <param name="existingPackageSourceMappingSourceItems"></param>
        /// <param name="countCreatedTopLevelSourceMappings">For Top-Level packages: <see langword="null" /> if not applicable; 0 if none needed to be added; > 0 is the count of new package source mappings added.</param>
        /// <param name="countCreatedTransitiveSourceMappings">For Transitive packages: <see langword="null" /> if not applicable; 0 if none needed to be added; > 0 is the count of new package source mappings added.</param>
        internal static void ConfigureNewPackageSourceMappings(
            UserAction? userAction,
            PreviewResult? sourceMappingPreviewResult,
            PackageSourceMappingProvider sourceMappingProvider,
            IReadOnlyList<PackageSourceMappingSourceItem> existingPackageSourceMappingSourceItems,
            out int? countCreatedTopLevelSourceMappings,
            out int? countCreatedTransitiveSourceMappings)
        {
            countCreatedTopLevelSourceMappings = null;
            countCreatedTransitiveSourceMappings = null;

            if (userAction?.SelectedSourceName is null || sourceMappingPreviewResult is null
                || sourceMappingPreviewResult.NewSourceMappings is null || sourceMappingPreviewResult.NewSourceMappings.IsEmpty)
            {
                return;
            }

            countCreatedTopLevelSourceMappings = 0;
            countCreatedTransitiveSourceMappings = 0;
            string topLevelPackageId = userAction.PackageId;

            Dictionary<string, IReadOnlyList<string>> patternsReadOnly = existingPackageSourceMappingSourceItems
                .ToDictionary(pair => pair.Key, pair => (IReadOnlyList<string>)(pair.Patterns.Select(p => p.Pattern).ToList()));

            PackageSourceMapping packageSourceMapping = new(patternsReadOnly);

            List<string> addedPackageIdsWithoutExistingMappings =
                new(capacity: sourceMappingPreviewResult.NewSourceMappings.Count + 1);
            List<PackageSourceMappingSourceItem> newAndExistingPackageSourceMappingItems = new(existingPackageSourceMappingSourceItems);

            foreach (KeyValuePair<string, SortedSet<string>> newSourceMapping in sourceMappingPreviewResult.NewSourceMappings)
            {
                string addedSourceName = newSourceMapping.Key;
                SortedSet<string> addedPackageIds = newSourceMapping.Value;

                foreach (var addedPackageId in addedPackageIds)
                {
                    // Expand all patterns/globs so we can check if this package ID was already mapped.
                    IReadOnlyList<string> configuredSource = packageSourceMapping.GetConfiguredPackageSources(addedPackageId);

                    // Top-level package was looked up.
                    if (addedPackageId == topLevelPackageId)
                    {
                        // The top-level package is not already mapped to the selected source.
                        if (configuredSource.Count == 0 || !configuredSource.Contains(userAction.SelectedSourceName))
                        {
                            countCreatedTopLevelSourceMappings++;
                            addedPackageIdsWithoutExistingMappings.Add(topLevelPackageId);
                        }
                    }
                    // Transitive package was looked up.
                    else if (configuredSource.Count == 0)
                    {
                        countCreatedTransitiveSourceMappings++;
                    }

                    MergePackageSourceMappings(addedSourceName, addedPackageIds, sourceMappingProvider, newAndExistingPackageSourceMappingItems);
                }
            }

            sourceMappingProvider.SavePackageSourceMappings(newAndExistingPackageSourceMappingItems);
        }

        private static string? FindSourceForPackageInGlobalPackagesFolder(
            PackageIdentity package,
            VersionFolderPathResolver resolver)
        {
            if (package is null)
            {
                throw new ArgumentNullException(nameof(package));
            }

            if (resolver is null)
            {
                throw new ArgumentNullException(nameof(resolver));
            }

            try
            {
                string nupkgMetadataPath = resolver.GetNupkgMetadataPath(package.Id, package.Version);
                NupkgMetadataFile nupkgMetadata = NupkgMetadataFileFormat.Read(nupkgMetadataPath);

                if (string.IsNullOrEmpty(nupkgMetadata.Source))
                {
                    return null;
                }

                return nupkgMetadata.Source;
            }
            catch
            {
                return null;
            }
        }

        private static void MergePackageSourceMappings(
            string sourceName,
            SortedSet<string> newPackageIdsToSourceMap,
            PackageSourceMappingProvider mappingProvider,
            List<PackageSourceMappingSourceItem> newAndExistingPackageSourceMappingItems)
        {
            if (string.IsNullOrWhiteSpace(sourceName) || newPackageIdsToSourceMap is null || newPackageIdsToSourceMap.Count == 0)
            {
                return;
            }

            PackageSourceMappingSourceItem packageSourceMappingItemForSource =
                newAndExistingPackageSourceMappingItems
                .Where(mappingItem => mappingItem.Key == sourceName)
                .FirstOrDefault();

            IEnumerable<PackagePatternItem> newPackagePatternItems = newPackageIdsToSourceMap.Select(packageId => new PackagePatternItem(packageId));

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
        }

        internal static string? GetNewSourceMappingSourceName(PackageSourceMapping packageSourceMapping, PackageSourceMoniker activePackageSourceMoniker)
        {
            string? sourceMappingSourceName = packageSourceMapping.IsEnabled
                && !activePackageSourceMoniker.IsAggregateSource
                ? activePackageSourceMoniker.PackageSourceNames.First() : null;

            return sourceMappingSourceName;
        }

        /// <summary>
        /// Builds a dictionary of source mappings to be created based on the <paramref name="added"/> packages.
        /// </summary>
        /// <returns>True if successful; false if any error occurred.</returns>
        internal static bool AddNewSourceMappingsFromAddedPackages(
            ref Dictionary<string, SortedSet<string>>? newSourceMappings,
            string selectedSourceName,
            string topLevelPackageId,
            List<AccessiblePackageIdentity> added,
            PackageSourceMapping packageSourceMapping,
            IReadOnlyList<SourceRepository>? globalPackageFolders,
            IReadOnlyList<SourceRepository> enabledSourceRepositories,
            INuGetUILogger logger)
        {
            if (logger is null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            if (selectedSourceName is null || added.Count == 0 || packageSourceMapping is null)
            {
                return true;
            }

            bool isSuccessful = true;
            string? globalPackageFolderName = globalPackageFolders?.Where(folder => folder.PackageSource.IsLocal).FirstOrDefault()?.PackageSource.Name;
            VersionFolderPathResolver? resolver = globalPackageFolderName != null ? new(globalPackageFolderName) : null;

            foreach (AccessiblePackageIdentity addedPackage in added)
            {
                string? sourceNameToMap = null;
                string? sourceFoundInGlobalPackagesFolder = null;
                IReadOnlyList<string> configuredSources = packageSourceMapping.GetConfiguredPackageSources(packageId: addedPackage.Id);

                if (configuredSources.Count > 0)
                {
                    continue;
                }

                if (topLevelPackageId == addedPackage.Id)
                {
                    sourceNameToMap = selectedSourceName;
                }
                // Check whether the Transitive Dependency exists in the GPF on an enabled package source for this project.
                else if (globalPackageFolderName != null && resolver != null && enabledSourceRepositories.Count > 0)
                {
                    sourceFoundInGlobalPackagesFolder = FindSourceForPackageInGlobalPackagesFolder(addedPackage, resolver);

                    // The package was found in the GPF.
                    if (!string.IsNullOrEmpty(sourceFoundInGlobalPackagesFolder))
                    {
                        SourceRepository enabledSourceFoundInGlobalPackagesFolder = enabledSourceRepositories.FirstOrDefault(sourceRepository =>
                            string.Equals(sourceRepository.PackageSource.Source, sourceFoundInGlobalPackagesFolder, StringComparison.Ordinal));

                        if (enabledSourceFoundInGlobalPackagesFolder != null)
                        {
                            // Map to the GPF source.
                            sourceNameToMap = enabledSourceFoundInGlobalPackagesFolder.PackageSource.Name;
                        }
                        else // GPF source is not enabled for this solution, so this is an error.
                        {
                            isSuccessful = false;

                            string formattedError = string.Format(CultureInfo.CurrentCulture,
                                Resources.Error_SourceMapping_GPF_NotEnabled,
                                addedPackage.Id,
                                sourceFoundInGlobalPackagesFolder);
                            LogMessage logError = new LogMessage(LogLevel.Error, formattedError, NuGetLogCode.NU1110);

                            logger.Log(logError);
                        }
                    }
                    else // The transitive dependency doesn't exist in GPF, so attempt to map to the selected source from the UI.
                    {
                        sourceNameToMap = selectedSourceName;
                    }
                }

                // Default to the selected source from the UI if not found in the GPF.
                if (sourceNameToMap is null)
                {
                    sourceNameToMap = selectedSourceName;
                }

                if (newSourceMappings is null)
                {
                    newSourceMappings = new Dictionary<string, SortedSet<string>>(capacity: 1)
                    {
                        { sourceNameToMap, new SortedSet<string>(new List<string>(capacity: added.Count) { addedPackage.Id }) }
                    };
                }
                else if (newSourceMappings.TryGetValue(sourceNameToMap, out SortedSet<string>? newMappingPackageIds))
                {
                    newMappingPackageIds.Add(addedPackage.Id);
                }
                else
                {
                    newSourceMappings.Add(sourceNameToMap, new SortedSet<string>(new List<string>(capacity: added.Count) { addedPackage.Id }));
                }
            }

            return isSuccessful;
        }
    }
}
