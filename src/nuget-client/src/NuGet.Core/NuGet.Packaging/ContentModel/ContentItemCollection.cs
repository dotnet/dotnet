// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using NuGet.Client;
using NuGet.Shared;

namespace NuGet.ContentModel
{
    public class ContentItemCollection
    {
        private static readonly ReadOnlyMemory<char> Dll = ".dll".AsMemory();
        private static readonly ReadOnlyMemory<char> Exe = ".exe".AsMemory();
        private static readonly ReadOnlyMemory<char> Winmd = ".winmd".AsMemory();

        private static readonly SimplePool<List<Asset>> ListAssetPool = new(() => new List<Asset>());
        private static readonly SimplePool<Dictionary<ContentItem, List<Asset>>> GroupAssetsPool = new(() => new(GroupComparer.DefaultComparer));

        private List<Asset>? _assets;
        private ConcurrentDictionary<ReadOnlyMemory<char>, string?>? _assemblyRelatedExtensions;
        /// <summary>
        /// True if lib/contract exists
        /// </summary>
        public bool HasContract { get; private set; }

        public void Load(IEnumerable<string> paths)
        {
            if (paths == null)
            {
                throw new ArgumentNullException(nameof(paths));
            }
            // Cache for assembly and it's related file extensions.
            _assemblyRelatedExtensions = new();

            // Read already loaded assets
            _assets = new List<Asset>();

            foreach (var path in paths.NoAllocEnumerate())
            {
                // Skip files in the root of the directory
                if (IsValidAsset(path))
                {
                    _assets.Add(new Asset()
                    {
                        Path = path
                    });

                    if (path.StartsWith("lib/contract", StringComparison.Ordinal))
                    {
                        HasContract = true;

                        _assets.Add(new Asset
                        {
                            Path = "ref/any" + path.Substring("lib/contract".Length)
                        });
                    }
                }
            }
        }

        public IEnumerable<ContentItem> FindItems(PatternSet definition)
        {
            if (definition == null)
            {
                throw new ArgumentNullException(nameof(definition));
            }
            if (_assets != null)
            {
                return FindItemsImplementation(definition, _assets);
            }
            return Enumerable.Empty<ContentItem>();
        }

        [Obsolete("This method causes excessive memory allocation with yield return. Use ContentItemCollection.PopulateItemGroups instead.")]
        public IEnumerable<ContentItemGroup> FindItemGroups(PatternSet definition)
        {
            if (definition == null)
            {
                throw new ArgumentNullException(nameof(definition));
            }
            if (_assets != null && _assets.Count > 0)
            {
                var groupPatterns = definition.GroupExpressions;

                Dictionary<ContentItem, List<Asset>>? groupAssets = null;
                foreach (var asset in _assets)
                {
                    foreach (var groupPattern in groupPatterns)
                    {
                        var item = groupPattern.Match(asset.Path, definition.PropertyDefinitions);
                        if (item != null)
                        {
                            groupAssets ??= GroupAssetsPool.Allocate();
                            if (!groupAssets.TryGetValue(item, out var assets))
                            {
                                assets = ListAssetPool.Allocate();
                                groupAssets[item] = assets;
                            }

                            assets.Add(asset);
                        }
                    }
                }

                if (groupAssets != null)
                {
                    foreach (var (item, assets) in groupAssets)
                    {
                        yield return new ContentItemGroup(
                            properties: item.Properties,
                            items: FindItemsImplementation(definition, assets));

                        assets.Clear();
                        ListAssetPool.Free(assets);
                    }

                    groupAssets.Clear();
                    GroupAssetsPool.Free(groupAssets);
                }
            }
        }
        /// <summary>
        /// Populate the provided list with ContentItemGroups based on a provided pattern set.
        /// </summary>
        /// <param name="definition">The pattern set to match</param>
        /// <param name="contentItemGroupList">The list that will be mutated and populated with the item groups</param>
        public void PopulateItemGroups(PatternSet definition, IList<ContentItemGroup> contentItemGroupList)
        {
            if (definition == null)
            {
                throw new ArgumentNullException(nameof(definition));
            }
            if (contentItemGroupList == null)
            {
                throw new ArgumentNullException(nameof(contentItemGroupList));
            }
            if (_assets != null && _assets.Count > 0)
            {
                var groupPatterns = definition.GroupExpressions;

                Dictionary<ContentItem, List<Asset>>? groupAssets = null;
                foreach (var asset in _assets)
                {
                    foreach (var groupPattern in groupPatterns)
                    {
                        var item = groupPattern.Match(asset.Path, definition.PropertyDefinitions);
                        if (item != null)
                        {
                            groupAssets ??= GroupAssetsPool.Allocate();
                            if (!groupAssets.TryGetValue(item, out var assets))
                            {
                                assets = ListAssetPool.Allocate();
                                groupAssets[item] = assets;
                            }

                            assets.Add(asset);
                        }
                    }
                }

                if (groupAssets != null)
                {
                    foreach (var (item, assets) in groupAssets)
                    {
                        contentItemGroupList.Add(new ContentItemGroup(
                            properties: item.Properties,
                            items: FindItemsImplementation(definition, assets)));

                        assets.Clear();
                        ListAssetPool.Free(assets);
                    }

                    groupAssets.Clear();
                    GroupAssetsPool.Free(groupAssets);
                }
            }
        }

        [Obsolete("Unused and will be removed in a future version.")]
        public bool HasItemGroup(SelectionCriteria criteria, params PatternSet[] definitions)
        {
            return FindBestItemGroup(criteria, definitions) != null;
        }

        public ContentItemGroup? FindBestItemGroup(SelectionCriteria criteria, params PatternSet[] definitions)
        {
            if (criteria is null)
            {
                throw new ArgumentNullException(nameof(criteria));
            }
            if (definitions.Length == 0)
            {
                return null;
            }

            List<ContentItemGroup> itemGroups = new List<ContentItemGroup>();
            foreach (var definition in definitions)
            {
                itemGroups.Clear();
                PopulateItemGroups(definition, itemGroups);
                foreach (var criteriaEntry in criteria.Entries.NoAllocEnumerate())
                {
                    ContentItemGroup? bestGroup = null;
                    var bestAmbiguity = false;

                    foreach (var itemGroup in itemGroups)
                    {
                        var groupIsValid = true;
                        foreach (var criteriaProperty in criteriaEntry.Properties.NoAllocEnumerate())
                        {
                            if (criteriaProperty.Value == null)
                            {
                                if (itemGroup.Properties.ContainsKey(criteriaProperty.Key))
                                {
                                    groupIsValid = false;
                                    break;
                                }
                            }
                            else
                            {
                                object? itemProperty;
                                if (!itemGroup.Properties.TryGetValue(criteriaProperty.Key, out itemProperty))
                                {
                                    groupIsValid = false;
                                    break;
                                }
                                ContentPropertyDefinition? propertyDefinition;
                                if (!definition.PropertyDefinitions.TryGetValue(criteriaProperty.Key, out propertyDefinition))
                                {
                                    groupIsValid = false;
                                    break;
                                }
                                if (!propertyDefinition.IsCriteriaSatisfied(criteriaProperty.Value, itemProperty))
                                {
                                    groupIsValid = false;
                                    break;
                                }
                            }
                        }
                        if (groupIsValid)
                        {
                            if (bestGroup == null)
                            {
                                bestGroup = itemGroup;
                            }
                            else
                            {
                                var groupComparison = 0;
                                foreach (var criteriaProperty in criteriaEntry.Properties.NoAllocEnumerate())
                                {
                                    if (criteriaProperty.Value == null)
                                    {
                                        continue;
                                    }
                                    else
                                    {
                                        var bestGroupValue = bestGroup.Properties[criteriaProperty.Key];
                                        var itemGroupValue = itemGroup.Properties[criteriaProperty.Key];
                                        var propertyDefinition = definition.PropertyDefinitions[criteriaProperty.Key];
                                        groupComparison = propertyDefinition.Compare(criteriaProperty.Value, bestGroupValue, itemGroupValue);
                                        if (groupComparison != 0)
                                        {
                                            break;
                                        }
                                    }
                                }
                                if (groupComparison > 0)
                                {
                                    bestGroup = itemGroup;
                                    bestAmbiguity = false;
                                }
                                else if (groupComparison == 0)
                                {
                                    bestAmbiguity = true;
                                }
                            }
                        }
                    }
                    if (bestGroup != null)
                    {
                        if (bestAmbiguity)
                        {
                            // var x = 5;
                        }
                        return bestGroup;
                    }
                }
            }
            return null;
        }

        private List<ContentItem> FindItemsImplementation(PatternSet definition, IEnumerable<Asset> assets)
        {
            var pathPatterns = definition.PathExpressions;

            List<ContentItem> items = new();

            foreach (var asset in assets.NoAllocEnumerate())
            {
                var path = asset.Path;

                foreach (var pathPattern in pathPatterns)
                {
                    var contentItem = pathPattern.Match(path, definition.PropertyDefinitions);
                    if (contentItem != null)
                    {
                        //If the item is assembly, populate the "related files extensions property".
                        if (contentItem.TryGetValue(ManagedCodeConventions.PropertyNames.ManagedAssembly, out _))
                        {
                            string? relatedFileExtensionsProperty = GetRelatedFileExtensionProperty(contentItem.Path, assets);
                            if (relatedFileExtensionsProperty is not null)
                            {
                                contentItem.Add("related", relatedFileExtensionsProperty);
                            }
                        }
                        items.Add(contentItem);
                        break;
                    }
                }
            }

            return items;
        }

        internal string? GetRelatedFileExtensionProperty(string assemblyPath, IEnumerable<Asset> assets)
        {
            //E.g. if path is "lib/net472/A.B.C.dll", the prefix will be "lib/net472/A.B.C."
            ReadOnlyMemory<char> assemblyPrefix = assemblyPath.AsMemory(0, assemblyPath.LastIndexOf('.') + 1);

            if (_assemblyRelatedExtensions != null && _assemblyRelatedExtensions.TryGetValue(assemblyPrefix, out string? relatedProperty))
            {
                return relatedProperty;
            }

            List<string>? relatedFileExtensionList = null;
            foreach (Asset asset in assets.NoAllocEnumerate())
            {
                if (asset.Path is not null)
                {
                    var extension = GetExtension(asset);

                    if (extension.Length > 0 &&
                        //Assembly properties are files with extensions ".dll", ".winmd", ".exe", see ManagedCodeConventions.
                        !ReadOnlyMemoryEquals(extension, Dll) &&
                        !ReadOnlyMemoryEquals(extension, Exe) &&
                        !ReadOnlyMemoryEquals(extension, Winmd) &&
                        !asset.Path.Equals(assemblyPath, StringComparison.OrdinalIgnoreCase) &&
                        //The prefix should match exactly (case sensitive), as file names are case sensitive on certain OSes.
                        //E.g. for lib/net472/A.B.C.dll and lib/net472/a.b.c.xml, if we generate related property '.xml', the related file path is not predictable on case sensitive OSes.
                        asset.Path.AsMemory().Span.StartsWith(assemblyPrefix.Span, StringComparison.Ordinal))
                    {
                        if (relatedFileExtensionList is null)
                        {
                            relatedFileExtensionList = new List<string>();
                        }
                        relatedFileExtensionList.Add(asset.Path.Substring(assemblyPrefix.Length - 1));
                    }
                }
            }

            // If no related files found.
            if (relatedFileExtensionList is null || relatedFileExtensionList.Count == 0)
            {
                if (_assemblyRelatedExtensions != null)
                {
                    _assemblyRelatedExtensions.TryAdd(assemblyPrefix, null);
                }
                return null;
            }
            else
            {
                relatedFileExtensionList.Sort();
                string relatedFileExtensionsProperty = string.Join(";", relatedFileExtensionList);
                if (_assemblyRelatedExtensions != null)
                {
                    _assemblyRelatedExtensions.TryAdd(assemblyPrefix, relatedFileExtensionsProperty);
                }
                return relatedFileExtensionsProperty;
            }

            static bool ReadOnlyMemoryEquals(ReadOnlyMemory<char> x, ReadOnlyMemory<char> y)
            {
                return x.Span.Equals(y.Span, StringComparison.OrdinalIgnoreCase);
            }

            static ReadOnlyMemory<char> GetExtension(Asset asset)
            {
                int lastIndexOfDot = asset.Path.LastIndexOf('.');
                if (lastIndexOfDot != -1)
                {
                    return asset.Path.AsMemory(lastIndexOfDot);
                }
                return default;
            }
        }

        /// <summary>
        /// False if the path would not match any patterns.
        /// </summary>
        private static bool IsValidAsset(string path)
        {
            // Verify that the file is not in the root. All patterns are for sub directories.
            for (var i = 1; i < path.Length; i++)
            {
                if (path[i] == '/')
                {
                    return true;
                }
            }

            return false;
        }

        internal class GroupComparer : IEqualityComparer<ContentItem>
        {
            public static readonly GroupComparer DefaultComparer = new GroupComparer();

            public int GetHashCode(ContentItem obj)
            {
                var hashCode = 0;
                if (obj._properties != null)
                {
                    foreach (var property in obj._properties)
                    {
#if NETFRAMEWORK || NETSTANDARD
                        hashCode ^= property.Key.GetHashCode();
#else
                        hashCode ^= property.Key.GetHashCode(StringComparison.Ordinal);
#endif
                        hashCode ^= property.Value.GetHashCode();
                    }
                }
                else
                {
                    if (obj._assembly != null)
                    {
                        hashCode ^= obj._assembly.GetHashCode();
                    }
                    if (obj._locale != null)
                    {
                        hashCode ^= obj._locale.GetHashCode();
                    }
                    if (obj._related != null)
                    {
                        hashCode ^= obj._related.GetHashCode();
                    }
                    if (obj._msbuild != null)
                    {
                        hashCode ^= obj._msbuild.GetHashCode();
                    }
                    if (obj._tfm != null)
                    {
                        hashCode ^= obj._tfm.GetHashCode();
                    }
                    if (obj._rid != null)
                    {
                        hashCode ^= obj._rid.GetHashCode();
                    }
                    if (obj._any != null)
                    {
                        hashCode ^= obj._any.GetHashCode();
                    }
                    if (obj._satelliteAssembly != null)
                    {
                        hashCode ^= obj._satelliteAssembly.GetHashCode();
                    }
                    if (obj._codeLanguage != null)
                    {
                        hashCode ^= obj._codeLanguage.GetHashCode();
                    }
                }
                return hashCode;
            }

            public bool Equals(ContentItem? x, ContentItem? y)
            {
                if (ReferenceEquals(x, y))
                {
                    return true;
                }
                if (x is null || y is null)
                {
                    return false;
                }

                if (x._properties == null && y._properties != null)
                {
                    return false;
                }

                if (x._properties != null && y._properties == null)
                {
                    return false;
                }

                if (x._properties?.Count != y._properties?.Count)
                {
                    return false;
                }

                if (x._properties != null && y._properties != null)
                {
                    foreach (var xProperty in x._properties)
                    {
                        object? yValue;
                        if (!y._properties.TryGetValue(xProperty.Key, out yValue))
                        {
                            return false;
                        }
                        if (!Equals(xProperty.Value, yValue))
                        {
                            return false;
                        }
                    }
                    foreach (var yProperty in y._properties)
                    {
                        object? xValue;
                        if (!x._properties.TryGetValue(yProperty.Key, out xValue))
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    if (!EqualityUtility.EqualsWithNullCheck(x._assembly, y._assembly))
                    {
                        return false;
                    }
                    if (!EqualityUtility.EqualsWithNullCheck(x._locale, y._locale))
                    {
                        return false;
                    }
                    if (!EqualityUtility.EqualsWithNullCheck(x._related, y._related))
                    {
                        return false;
                    }
                    if (!EqualityUtility.EqualsWithNullCheck(x._msbuild, y._msbuild))
                    {
                        return false;
                    }
                    if (!EqualityUtility.EqualsWithNullCheck(x._tfm, y._tfm))
                    {
                        return false;
                    }
                    if (!EqualityUtility.EqualsWithNullCheck(x._rid, y._rid))
                    {
                        return false;
                    }
                    if (!EqualityUtility.EqualsWithNullCheck(x._any, y._any))
                    {
                        return false;
                    }
                    if (!EqualityUtility.EqualsWithNullCheck(x._satelliteAssembly, y._satelliteAssembly))
                    {
                        return false;
                    }
                    if (!EqualityUtility.EqualsWithNullCheck(x._codeLanguage, y._codeLanguage))
                    {
                        return false;
                    }
                }
                return true;
            }
        }
    }
}
