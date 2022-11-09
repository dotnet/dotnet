// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Newtonsoft.Json;
using NuGet.Frameworks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Microsoft.DotNet.Build.Tasks.Packaging
{
    public class GetApplicableAssetsFromPackages : BuildTask
    {
        private Dictionary<string, List<PackageItem>> _packageToPackageItems;
        private Dictionary<string, PackageItem> _targetPathToPackageItem;
        private AggregateNuGetAssetResolver _resolver;

        /// <summary>
        /// All items that make up packages to resolve from.  
        /// Must have the following metadata
        ///     Identity - path to the file in the binary directory
        ///         TargetPath - path to the file in the package
        ///         PackageId - ID of the package containing the file
        /// </summary>
        [Required]
        public ITaskItem[] PackageAssets { get; set; }

        /// <summary>
        /// TargetMoniker to use when resolving assets.  EG: netcoreapp1.0, netstandard1.4
        /// </summary>
        [Required]
        public string[] TargetMonikers { get; set; }

        /// <summary>
        /// runtime.json that defines the RID graph
        /// </summary>
        [Required]
        public string RuntimeFile { get; set; }

        /// <summary>
        /// If specified will be used when resolving runtime assets, otherwise TargetMoniker will be used.
        /// </summary>
        public string[] RuntimeTargetMonikers { get; set; }

        /// <summary>
        /// If specified will be used when resolving runtime assets, otherwise no RID will be used.
        /// </summary>
        public string TargetRuntime { get; set; }

        [Output]
        public ITaskItem[] CompileAssets { get; set; }

        [Output]
        public ITaskItem[] RuntimeAssets { get; set; }

        [Output]
        public ITaskItem[] BuildProjects { get; set; }

        /// <summary>
        /// Generates a table in markdown that lists the API version supported by 
        /// various packages at all levels of NETStandard.
        /// </summary>
        /// <returns></returns>
        public override bool Execute()
        {
            if (PackageAssets == null || PackageAssets.Length == 0)
            {
                Log.LogError("PackageAssets argument must be specified");
                return false;
            }

            if (TargetMonikers == null || TargetMonikers.Length == 0)
            {
                Log.LogError("TargetMoniker argument must be specified");
                return false;
            }

            NuGetFramework[] compileFxs = TargetMonikers.Select(fx => NuGetFramework.Parse(fx)).ToArray();
            NuGetFramework[] runtimeFxs = compileFxs;

            if (RuntimeTargetMonikers != null && RuntimeTargetMonikers.Length > 0)
            {
                runtimeFxs = RuntimeTargetMonikers.Select(fx => NuGetFramework.Parse(fx)).ToArray();
            }

            LoadFiles();

            var buildProjects = new List<BuildProject>();
            CompileAssets = null;
            // find the best framework
            foreach (var compileFx in compileFxs)
            {
                var compileAssets = _resolver.ResolveCompileAssets(compileFx);

                if (compileAssets.Any())
                {
                    var compileItems = compileAssets.Where(ca => !NuGetAssetResolver.IsPlaceholder(ca))
                                                    .Select(ca => _targetPathToPackageItem[ca]);

                    buildProjects.AddRange(compileItems.Select(ci => BuildProjectFromPackageItem(ci)).Where(bp => bp != null));

                    CompileAssets = compileItems.Select(ci => PackageItemAsResolvedAsset(ci)).ToArray();

                    Log.LogMessage($"Resolved compile assets from {compileFx.ToString()}: {String.Join(";", CompileAssets.Select(c => c.ItemSpec))}");
                    break;
                }
            }

            if (CompileAssets == null)
            {
                Log.LogError($"Could not locate compile assets for any of the frameworks {String.Join(";", compileFxs.Select(fx => fx.ToString()))}");
            }

            RuntimeAssets = null;
            foreach (var runtimeFx in runtimeFxs)
            {
                var runtimeAssets = _resolver.ResolveRuntimeAssets(runtimeFx, TargetRuntime);

                if (runtimeAssets.Any())
                {
                    var runtimeItems = runtimeAssets.Where(ra => !NuGetAssetResolver.IsPlaceholder(ra))
                                                    .Select(ra => _targetPathToPackageItem[ra]);

                    buildProjects.AddRange(runtimeItems.Select(ri => BuildProjectFromPackageItem(ri)).Where(bp => bp != null));

                    RuntimeAssets = runtimeItems.SelectMany(ri => PackageItemAndSymbolsAsResolvedAsset(ri)).ToArray();

                    Log.LogMessage($"Resolved runtime assets from {runtimeFx.ToString()}: {String.Join(";", RuntimeAssets.Select(r => r.ItemSpec))}");
                    break;
                }
            }

            if (!string.IsNullOrEmpty(TargetRuntime) && RuntimeAssets == null)
            {
                Log.LogError($"Could not locate runtime assets for any of the frameworks {String.Join(";", runtimeFxs.Select(fx => fx.ToString()))}");
            }

            BuildProjects = buildProjects.Distinct().Select(bp => bp.ToItem()).ToArray();

            return !Log.HasLoggedErrors;
        }

        private void LoadFiles()
        {
            _packageToPackageItems = new Dictionary<string, List<PackageItem>>();
            foreach (var file in PackageAssets)
            {
                try
                {
                    var packageItem = new PackageItem(file);

                    if (String.IsNullOrWhiteSpace(packageItem.TargetPath))
                    {
                        Log.LogError($"{packageItem.TargetPath} is missing TargetPath metadata");
                    }

                    if (!_packageToPackageItems.ContainsKey(packageItem.Package))
                    {
                        _packageToPackageItems[packageItem.Package] = new List<PackageItem>();
                    }
                    _packageToPackageItems[packageItem.Package].Add(packageItem);
                }
                catch (Exception ex)
                {
                    Log.LogError($"Could not parse File {file.ItemSpec}. {ex}");
                    // skip it.
                }
            }

            // build a map to translate back to source file from resolved asset
            // we use package-specific paths since we're resolving a set of packages.
            _targetPathToPackageItem = new Dictionary<string, PackageItem>();
            foreach (var packageFiles in _packageToPackageItems)
            {
                foreach (PackageItem packageFile in packageFiles.Value)
                {
                    string packageSpecificTargetPath = AggregateNuGetAssetResolver.AsPackageSpecificTargetPath(packageFiles.Key, packageFile.TargetPath);

                    if (_targetPathToPackageItem.ContainsKey(packageSpecificTargetPath))
                    {
                        Log.LogError($"Files {_targetPathToPackageItem[packageSpecificTargetPath].SourcePath} and {packageFile.SourcePath} have the same TargetPath {packageSpecificTargetPath}.");
                    }
                    _targetPathToPackageItem[packageSpecificTargetPath] = packageFile;
                }
            }

            _resolver = new AggregateNuGetAssetResolver(RuntimeFile);
            foreach (string packageId in _packageToPackageItems.Keys)
            {
                _resolver.AddPackageItems(packageId, _packageToPackageItems[packageId].Select(f => f.TargetPath));
            }
        }

        private static ITaskItem PackageItemAsResolvedAsset(PackageItem packageItem)
        {
            return SetPackageMetadata(new TaskItem(packageItem.OriginalItem), packageItem);
        }

        private static IEnumerable<ITaskItem> PackageItemAndSymbolsAsResolvedAsset(PackageItem packageItem)
        {
            yield return PackageItemAsResolvedAsset(packageItem);

            string pdbPath = Path.ChangeExtension(packageItem.SourcePath, ".pdb");
            if (File.Exists(pdbPath))
            {
                var pdbItem = new TaskItem(Path.ChangeExtension(packageItem.OriginalItem.ItemSpec, ".pdb"));
                packageItem.OriginalItem.CopyMetadataTo(pdbItem);
                SetPackageMetadata(pdbItem, packageItem);

                if (!String.IsNullOrEmpty(packageItem.TargetPath))
                {
                    pdbItem.SetMetadata("TargetPath", Path.ChangeExtension(packageItem.TargetPath, ".pdb"));
                }

                yield return pdbItem;
            }
        }

        private static ITaskItem SetPackageMetadata(ITaskItem item, PackageItem packageItem)
        {
            item.SetMetadata("Private", "false");
            item.SetMetadata("FromPkgProj", "true");
            item.SetMetadata("NuGetPackageId", packageItem.Package);
            item.SetMetadata("NuGetPackageVersion", packageItem.PackageVersion);
            return item;
        }

        public static BuildProject BuildProjectFromPackageItem(PackageItem packageItem)
        {
            if (packageItem.SourceProject == null)
            {
                return null;
            }

            return new BuildProject()
            {
                Project = packageItem.SourceProject,
                AdditionalProperties = packageItem.AdditionalProperties,
                UndefineProperties = packageItem.UndefineProperties
            };
        }
    }
}
