// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace NuGet.Configuration
{
    public class PackageSourceMappingProvider
    {
        private readonly ISettings _settings;

        public PackageSourceMappingProvider(ISettings settings)
        {
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        /// <summary>
        /// Supports the disabling of saving to disk for any <see cref="ISettings"/> changes.
        /// </summary>
        /// <param name="settings">The settings to be used by the provider.</param>
        /// <param name="shouldSkipSave">True to avoid saving any changes to disk and only modify the <see cref="ISettings"/> in memory.
        /// Default is false.</param>
        public PackageSourceMappingProvider(ISettings settings, bool shouldSkipSave)
            : this(settings)
        {
            ShouldSkipSave = shouldSkipSave;
        }

        /// <summary>
        /// Avoid saving to disk and only modify the <see cref="ISettings"/> in memory.
        /// </summary>
        public bool ShouldSkipSave { get; }

        public IReadOnlyList<PackageSourceMappingSourceItem> GetPackageSourceMappingItems()
        {
            SettingSection? packageSourceMappingSection = _settings.GetSection(ConfigurationConstants.PackageSourceMapping);
            if (packageSourceMappingSection == null)
            {
                return Enumerable.Empty<PackageSourceMappingSourceItem>().ToList();
            }

            return packageSourceMappingSection.Items.OfType<PackageSourceMappingSourceItem>().ToList();
        }

        internal void Remove(IReadOnlyList<PackageSourceMappingSourceItem> packageSourceMappingSourceItems)
        {
            if (packageSourceMappingSourceItems == null || packageSourceMappingSourceItems.Count == 0)
            {
                throw new ArgumentException(Resources.Argument_Cannot_Be_Null_Or_Empty, nameof(packageSourceMappingSourceItems));
            }

            foreach (PackageSourceMappingSourceItem packageSourceMappingSourceItem in packageSourceMappingSourceItems)
            {
                try
                {
                    _settings.Remove(ConfigurationConstants.PackageSourceMapping, packageSourceMappingSourceItem);
                }
                // An error means the item doesn't exist or is in a machine wide config, therefore just ignore it
                catch { }
            }

            if (!ShouldSkipSave)
            {
                _settings.SaveToDisk();
            }
        }

        internal void AddOrUpdatePackageSourceMappingSourceItem(PackageSourceMappingSourceItem packageSourceMappingSourceItem)
        {
            if (packageSourceMappingSourceItem == null)
            {
                throw new ArgumentNullException(nameof(packageSourceMappingSourceItem));
            }

            _settings.AddOrUpdate(ConfigurationConstants.PackageSourceMapping, packageSourceMappingSourceItem);

            if (!ShouldSkipSave)
            {
                _settings.SaveToDisk();
            }
        }

        public void SavePackageSourceMappings(IReadOnlyList<PackageSourceMappingSourceItem> packageSourceMappingsSourceItems)
        {
            if (packageSourceMappingsSourceItems == null)
            {
                throw new ArgumentNullException(nameof(packageSourceMappingsSourceItems));
            }

            IReadOnlyList<PackageSourceMappingSourceItem> existingSettingsLookup = GetPackageSourceMappingItems();
            // Remove all old mappings not in new mappings
            List<PackageSourceMappingSourceItem> removeMappings = new List<PackageSourceMappingSourceItem>();
            foreach (PackageSourceMappingSourceItem sourceItem in existingSettingsLookup)
            {
                if (!packageSourceMappingsSourceItems.Contains(sourceItem))
                {
                    removeMappings.Add(sourceItem);
                }
            }

            if (removeMappings.Count > 0)
            {
                Remove(removeMappings);
            }

            //Adds or updates mappings
            foreach (PackageSourceMappingSourceItem sourceMappingItem in packageSourceMappingsSourceItems)
            {
                AddOrUpdatePackageSourceMappingSourceItem(sourceMappingItem);
            }
        }
    }
}
