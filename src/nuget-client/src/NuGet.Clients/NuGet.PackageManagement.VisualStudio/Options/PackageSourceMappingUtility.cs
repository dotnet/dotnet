// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using NuGet.Configuration;
using NuGet.VisualStudio.Internal.Contracts;

namespace NuGet.PackageManagement.VisualStudio.Options
{
    internal static class PackageSourceMappingUtility
    {
        internal static Dictionary<string, List<PackageSourceContextInfo>> CreatePackageSourceMappingDictionary(IReadOnlyList<PackageSourceMappingSourceItem> originalMappings)
        {
            var packageSourceMappingDictionary = new Dictionary<string, List<PackageSourceContextInfo>>();
            foreach (PackageSourceMappingSourceItem sourceItem in originalMappings)
            {
                foreach (PackagePatternItem patternItem in sourceItem.Patterns)
                {
                    if (packageSourceMappingDictionary.TryGetValue(patternItem.Pattern, out var packageSourceContextInfos))
                    {
                        packageSourceContextInfos.Add(new PackageSourceContextInfo(sourceItem.Key));
                    }
                    else
                    {
                        packageSourceMappingDictionary[patternItem.Pattern] = [new PackageSourceContextInfo(sourceItem.Key)];
                    }
                }
            }

            return packageSourceMappingDictionary;
        }

        /// <summary>
        /// For existing package source mappings configured with package sources which don't exist, keep those invalid package source mappings in place.
        /// Add any <paramref name="originalPackageSourceMappings"/> configured with package sources not in <paramref name="existingPackageSources"/>
        /// back into <paramref name="sourceNamesToPackagePatterns"/>.
        /// </summary>
        internal static void PreserveInvalidPackageSourceMappings(
            Dictionary<string, List<PackagePatternItem>> sourceNamesToPackagePatterns,
            IReadOnlyList<PackageSource> existingPackageSources,
            IReadOnlyList<PackageSourceMappingSourceItem> originalPackageSourceMappings)
        {
            HashSet<string> configuredPackageSourceNames = existingPackageSources
                .Select(packageSource => packageSource.Name)
                .ToHashSet<string>();

            foreach (PackageSourceMappingSourceItem originalMapping in originalPackageSourceMappings)
            {
                string originalSourceName = originalMapping.Key;
                if (!configuredPackageSourceNames.Contains(originalSourceName, StringComparer.OrdinalIgnoreCase))
                {
                    // Keep this invalid package source mapping.
                    AddOrUpdateSourceWithPackagePatterns(sourceNamesToPackagePatterns, originalMapping.Patterns, originalSourceName);
                }
            }
        }

        internal static List<PackageSourceMappingSourceItem> ConvertPackageIdAndSourcesToSourceMappingSourceItems(
            Dictionary<string, List<PackagePatternItem>> sourceNamesToPackagePatterns,
            IReadOnlyList<(string packageIdOrPattern, IEnumerable<string> sources)> packagePatternToSources)
        {
            foreach ((string packageIdOrPattern, IEnumerable<string> sources) packagePatternToSource in packagePatternToSources)
            {
                foreach (string source in packagePatternToSource.sources)
                {
                    string packageIdOrPattern = packagePatternToSource.packageIdOrPattern;
                    PackagePatternItem packagePatternItem = new(packageIdOrPattern);
                    AddOrUpdateSourceWithPackagePatterns(sourceNamesToPackagePatterns, [packagePatternItem], source);
                }
            }

            var packageSourceMappingsSourceItems = new List<PackageSourceMappingSourceItem>();
            foreach (KeyValuePair<string, List<PackagePatternItem>> item in sourceNamesToPackagePatterns)
            {
                string source = item.Key;
                List<PackagePatternItem> packagePatterns = item.Value;
                packageSourceMappingsSourceItems.Add(new PackageSourceMappingSourceItem(source, packagePatterns));
            }
            return packageSourceMappingsSourceItems;
        }

        private static void AddOrUpdateSourceWithPackagePatterns(
           Dictionary<string, List<PackagePatternItem>> sourceNamesToPackagePatterns,
           IList<PackagePatternItem> packageIdOrPatterns,
           string source)
        {
            if (!sourceNamesToPackagePatterns.Keys.Contains(source, StringComparer.OrdinalIgnoreCase))
            {
                sourceNamesToPackagePatterns[source] = new List<PackagePatternItem>();
            }

            foreach (PackagePatternItem packagePatternItem in packageIdOrPatterns)
            {
                if (!sourceNamesToPackagePatterns[source].Any(id => id.Pattern == packagePatternItem.Pattern))
                {
                    sourceNamesToPackagePatterns[source].Add(packagePatternItem);
                }
            }
        }
    }
}
