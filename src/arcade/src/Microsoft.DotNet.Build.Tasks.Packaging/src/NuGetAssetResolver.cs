// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using NuGet.Client;
using NuGet.ContentModel;
using NuGet.Frameworks;
using NuGet.RuntimeModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Microsoft.DotNet.Build.Tasks.Packaging
{
    public class NuGetAssetResolver
    {
        private ManagedCodeConventions _conventions;
        private ContentItemCollection _sourceItems;


        public const string PlaceHolderFile = "_._";

        public NuGetAssetResolver(string runtimeFile, IEnumerable<string> packageItems)
        {
            RuntimeGraph runtimeGraph = null;

            if (!string.IsNullOrEmpty(runtimeFile))
            {
                runtimeGraph = JsonRuntimeFormat.ReadRuntimeGraph(runtimeFile);
            }
            _conventions = new ManagedCodeConventions(runtimeGraph);

            _sourceItems = new ContentItemCollection();
            _sourceItems.Load(packageItems);
        }

        public ContentItemGroup GetCompileItems(NuGetFramework framework)
        {
            // don't use the RID for compile assets.
            var managedCriteria = _conventions.Criteria.ForFramework(framework);

            FixCriteria(managedCriteria);

            // compile falls back to runtime pattern in the case of lib with no ref, this won't fallback
            // to a runtime asset since we have no RID passed in.
            return _sourceItems.FindBestItemGroup(managedCriteria,
                _conventions.Patterns.CompileRefAssemblies,
                _conventions.Patterns.RuntimeAssemblies);
        }

        public IEnumerable<string> ResolveCompileAssets(NuGetFramework framework)
        {
            // we only care about compile items from this package, runtime packages will
            // never contribute compile items since the runtime graph is only used for runtime assets.
            var compileItems = GetCompileItems(framework);

            return (compileItems != null) ?
                compileItems.Items.Select(ci => ci.Path) :
                Enumerable.Empty<string>();
        }

        public ContentItemGroup GetRuntimeItems(NuGetFramework framework, string runtimeIdentifier)
        {
            var managedCriteria = _conventions.Criteria.ForFrameworkAndRuntime(framework, runtimeIdentifier);

            FixCriteria(managedCriteria);

            return _sourceItems.FindBestItemGroup(managedCriteria,
                _conventions.Patterns.RuntimeAssemblies);
        }

        public IEnumerable<string> ResolveRuntimeAssets(NuGetFramework framework, string runtimeId)
        {
            var runtimeItems = GetRuntimeItems(framework, runtimeId);

            return (runtimeItems != null) ?
                runtimeItems.Items.Select(ci => ci.Path) :
                Enumerable.Empty<string>();
        }

        public static void FixCriteria(SelectionCriteria criteria)
        {
            // workaround https://github.com/NuGet/Home/issues/1457
            foreach (var criterium in criteria.Entries)
            {
                if (criterium.Properties.ContainsKey(ManagedCodeConventions.PropertyNames.TargetFrameworkMoniker) &&
                    !criterium.Properties.ContainsKey(ManagedCodeConventions.PropertyNames.RuntimeIdentifier))
                {
                    criterium.Properties.Add(ManagedCodeConventions.PropertyNames.RuntimeIdentifier, null);
                }
            }
        }

        public static IEnumerable<string> GetPackageTargetDirectories(ContentItemGroup contentGroup)
        {
            if (contentGroup == null)
            {
                yield break;
            }

            foreach (var contentItem in contentGroup.Items)
            {
                // package paths are standardized to '/'
                int dirLength = contentItem.Path.LastIndexOf(Path.AltDirectorySeparatorChar);

                if (dirLength == -1)
                {
                    yield return "";
                }
                else
                {
                    yield return contentItem.Path.Substring(0, dirLength);
                }
            }
        }

        public static void ExamineAssets(ILog logger, string assetType, string package, string target, IEnumerable<string> items, out bool hasRealAsset, out bool hasPlaceHolder)
        {
            hasPlaceHolder = false;
            hasRealAsset = false;
            StringBuilder assetLog = new StringBuilder($"{assetType} assets for {package} on {target}: ");
            if (items != null && items.Any())
            {
                foreach (var runtimeItem in items)
                {
                    assetLog.AppendLine();
                    assetLog.Append($"  {runtimeItem}");

                    if (!hasRealAsset && NuGetAssetResolver.IsPlaceholder(runtimeItem))
                    {
                        hasPlaceHolder = true;
                    }
                    else
                    {
                        hasRealAsset = true;
                        hasPlaceHolder = false;
                    }
                }
            }
            else
            {
                assetLog.AppendLine();
                assetLog.Append("  <none>");
            }
            logger.LogMessage(LogImportance.Low, assetLog.ToString());
        }

        public static bool IsPlaceholder(string path)
        {
            return Path.GetFileName(path) == PlaceHolderFile;
        }
    }

    public class AggregateNuGetAssetResolver
    {
        private ManagedCodeConventions _conventions;
        private Dictionary<string, ContentItemCollection> _packages;


        public const string PlaceHolderFile = "_._";

        public AggregateNuGetAssetResolver(string runtimeFile)
        {
            RuntimeGraph runtimeGraph = null;

            if (!String.IsNullOrEmpty(runtimeFile))
            {
                runtimeGraph = JsonRuntimeFormat.ReadRuntimeGraph(runtimeFile);
            }
            _conventions = new ManagedCodeConventions(runtimeGraph);
            _packages = new Dictionary<string, ContentItemCollection>();
        }

        public void AddPackageItems(string packageId, IEnumerable<string> packageItems)
        {
            if (!_packages.ContainsKey(packageId))
            {
                _packages[packageId] = new ContentItemCollection();
            }

            _packages[packageId].Load(packageItems);
        }

        public IReadOnlyDictionary<string, ContentItemGroup> GetCompileItems(NuGetFramework framework)
        {
            // don't use the RID for compile assets.
            var managedCriteria = _conventions.Criteria.ForFramework(framework);

            NuGetAssetResolver.FixCriteria(managedCriteria);

            Dictionary<string, ContentItemGroup> resolvedAssets = new Dictionary<string, ContentItemGroup>();
            // compile falls back to runtime pattern in the case of lib with no ref, this won't fallback
            // to a runtime asset since we have no RID passed in.
            foreach (var package in _packages.Keys)
            {
                resolvedAssets.Add(package,
                    _packages[package].FindBestItemGroup(managedCriteria,
                        _conventions.Patterns.CompileRefAssemblies,
                        _conventions.Patterns.RuntimeAssemblies));
            }

            return resolvedAssets;
        }

        public IEnumerable<string> ResolveCompileAssets(NuGetFramework framework, string referencePackageId)
        {
            // we only care about compile items from this package, runtime packages will
            // never contribute compile items since the runtime graph is only used for runtime assets.
            var allCompileItems = GetCompileItems(framework);
            ContentItemGroup thisPackageCompileItems = null;
            allCompileItems.TryGetValue(referencePackageId, out thisPackageCompileItems);

            return (thisPackageCompileItems != null) ?
                thisPackageCompileItems.Items.Select(ci => AsPackageSpecificTargetPath(referencePackageId, ci.Path)) :
                Enumerable.Empty<string>();
        }

        public IEnumerable<string> ResolveCompileAssets(NuGetFramework framework)
        {
            var allCompileItems = GetCompileItems(framework);
            foreach (var packageId in allCompileItems.Keys)
            {
                var packageAssets = allCompileItems[packageId];
                if (packageAssets == null)
                {
                    continue;
                }

                foreach (var packageAsset in packageAssets.Items)
                {
                    yield return AsPackageSpecificTargetPath(packageId, packageAsset.Path);
                }
            }
        }

        public IReadOnlyDictionary<string, ContentItemGroup> GetRuntimeItems(NuGetFramework framework, string runtimeIdentifier)
        {
            var managedCriteria = _conventions.Criteria.ForFrameworkAndRuntime(framework, runtimeIdentifier);

            NuGetAssetResolver.FixCriteria(managedCriteria);

            Dictionary<string, ContentItemGroup> resolvedAssets = new Dictionary<string, ContentItemGroup>();

            foreach (var package in _packages.Keys)
            {
                resolvedAssets.Add(package,
                    _packages[package].FindBestItemGroup(managedCriteria,
                        _conventions.Patterns.RuntimeAssemblies));
            }

            return resolvedAssets;
        }

        public IReadOnlyDictionary<string, ContentItemGroup> GetNativeItems(NuGetFramework framework, string runtimeIdentifier)
        {
            var managedCriteria = _conventions.Criteria.ForFrameworkAndRuntime(framework, runtimeIdentifier);

            NuGetAssetResolver.FixCriteria(managedCriteria);

            Dictionary<string, ContentItemGroup> resolvedAssets = new Dictionary<string, ContentItemGroup>();

            foreach (var package in _packages.Keys)
            {
                resolvedAssets.Add(package,
                    _packages[package].FindBestItemGroup(managedCriteria,
                        _conventions.Patterns.NativeLibraries));
            }

            return resolvedAssets;
        }

        [Obsolete]
        public IReadOnlyDictionary<string, IEnumerable<ContentItemGroup>> GetAllRuntimeItems()
        {
            Dictionary<string, IEnumerable<ContentItemGroup>> resolvedAssets = new Dictionary<string, IEnumerable<ContentItemGroup>>();

            foreach (var package in _packages.Keys)
            {
                resolvedAssets.Add(package,
                    _packages[package].FindItemGroups(_conventions.Patterns.RuntimeAssemblies));
            }

            return resolvedAssets;
        }

        public IEnumerable<string> ResolveRuntimeAssets(NuGetFramework framework, string runtimeId)
        {
            var allRuntimeItems = GetRuntimeItems(framework, runtimeId);
            foreach (var packageId in allRuntimeItems.Keys)
            {
                var packageAssets = allRuntimeItems[packageId];
                if (packageAssets == null)
                {
                    continue;
                }

                foreach (var packageAsset in packageAssets.Items)
                {
                    yield return AsPackageSpecificTargetPath(packageId, packageAsset.Path);
                }
            }
        }
        public IEnumerable<string> ResolveNativeAssets(NuGetFramework framework, string runtimeId)
        {
            var allNativeItems = GetNativeItems(framework, runtimeId);
            foreach (var packageId in allNativeItems.Keys)
            {
                var packageAssets = allNativeItems[packageId];
                if (packageAssets == null)
                {
                    continue;
                }

                foreach (var packageAsset in packageAssets.Items)
                {
                    yield return AsPackageSpecificTargetPath(packageId, packageAsset.Path);
                }
            }
        }

        public static string AsPackageSpecificTargetPath(string packageId, string targetPath)
        {
            return $"{packageId}/{targetPath}";
        }
    }
}
