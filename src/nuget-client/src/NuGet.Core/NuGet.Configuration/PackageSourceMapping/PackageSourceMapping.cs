// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace NuGet.Configuration
{
    public class PackageSourceMapping
    {
        /// <summary>
        /// Max allowed length for package Id.
        /// In case update this value please update in src\NuGet.Core\NuGet.Packaging\PackageCreation\Utility\PackageIdValidator.cs too.
        /// </summary>
        internal static int PackageIdMaxLength { get; } = 100;

        /// <summary>
        /// Source name to package patterns list.
        /// </summary>
        internal IReadOnlyDictionary<string, IReadOnlyList<string>> Patterns { get; }

        private Lazy<SearchTree> SearchTree { get; }

        /// <summary>
        /// Indicate if any packageSource exist in package source mapping section
        /// </summary>
        public bool IsEnabled { get; }

        /// <summary>
        /// Get package source names with matching prefix "packageId" from package source mapping section.
        /// </summary>
        /// <param name="packageId">Search packageId. Cannot be null, empty, or whitespace only. </param>
        /// <returns>Package source names with matching prefix "packageId" from package patterns.</returns>
        /// <exception cref="ArgumentException"> if <paramref name="packageId"/> is null, empty, or whitespace only.</exception>
        public IReadOnlyList<string> GetConfiguredPackageSources(string packageId)
        {
            return SearchTree.Value?.GetConfiguredPackageSources(packageId);
        }

        public PackageSourceMapping(IReadOnlyDictionary<string, IReadOnlyList<string>> patterns)
        {
            Patterns = patterns ?? throw new ArgumentNullException(nameof(patterns));
            IsEnabled = Patterns.Count > 0;
            SearchTree = new Lazy<SearchTree>(() => GetSearchTree());
        }

        /// <summary>
        /// Generates a <see cref="PackageSourceMapping"/> based on the settings object.
        /// </summary>
        /// <param name="settings">Search packageId. Cannot be null, empty, or whitespace only. </param>
        /// <returns>A <see cref="PackageSourceMapping"/> based on the settings.</returns>
        /// <exception cref="ArgumentNullException"> if <paramref name="settings"/> is null.</exception>
        public static PackageSourceMapping GetPackageSourceMapping(ISettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            var packageSourceMappingProvider = new PackageSourceMappingProvider(settings);

            var patterns = new Dictionary<string, IReadOnlyList<string>>();

            foreach (PackageSourceMappingSourceItem packageSourceNamespaceItem in packageSourceMappingProvider.GetPackageSourceMappingItems())
            {
                patterns.Add(packageSourceNamespaceItem.Key, new List<string>(packageSourceNamespaceItem.Patterns.Select(e => e.Pattern)));
            }

            return new PackageSourceMapping(patterns);
        }

        private SearchTree GetSearchTree()
        {
            SearchTree patternsLookup = null;

            if (IsEnabled)
            {
                patternsLookup = new SearchTree(this);
            }

            return patternsLookup;
        }
    }
}
