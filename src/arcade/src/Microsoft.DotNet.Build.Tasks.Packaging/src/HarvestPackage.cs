// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Newtonsoft.Json;
using NuGet.ContentModel;
using NuGet.Frameworks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Microsoft.DotNet.Build.Tasks.Packaging
{
    public class HarvestPackage : BuildTask
    {
        /// <summary>
        /// Package ID to harvest
        /// </summary>
        [Required]
        public string PackageId { get; set; }

        /// <summary>
        /// Current package version.
        /// </summary>
        [Required]
        public string PackageVersion { get; set; }

        /// <summary>
        /// Folder where packages have been restored
        /// </summary>
        [Required]
        public string[] PackagesFolders { get; set; }

        /// <summary>
        /// Path to runtime.json that contains the runtime graph.
        /// </summary>
        [Required]
        public string RuntimeFile { get; set; }

        /// <summary>
        /// Additional packages to consider for evaluating support but not harvesting assets.
        ///   Identity: Package ID
        ///   Version: Package version.
        /// </summary>
        public ITaskItem[] RuntimePackages { get; set; }

        /// <summary>
        /// Set to false to suppress harvesting of files and only harvest supported framework information.
        /// </summary>
        public bool HarvestAssets { get; set; }
        
        /// <summary>
        /// Set to true to harvest all files by default.
        /// </summary>
        public bool IncludeAllPaths { get; set; }

        /// <summary>
        /// Set to partial paths to exclude from file harvesting.
        /// </summary>
        public string[] PathsToExclude { get; set; }

        /// <summary>
        /// Set to partial paths to include from file harvesting.
        /// </summary>
        public ITaskItem[] PathsToInclude { get; set; }

        /// <summary>
        /// Set to partial paths to suppress from both file and support harvesting.
        /// </summary>
        public string[] PathsToSuppress { get; set; }

        /// <summary>
        /// Frameworks to consider for support evaluation.
        ///   Identity: Framework
        ///   RuntimeIDs: Semi-colon separated list of runtime IDs
        /// </summary>
        public ITaskItem[] Frameworks { get; set; }

        /// <summary>
        /// Files already in the package.
        ///   Identity: path to file
        ///   AssemblyVersion: version of assembly
        ///   TargetFramework: target framework moniker to use for harvesting file's dependencies
        ///   TargetPath: path of file in package
        ///   IsReferenceAsset: true for files in Ref.
        /// </summary>
        public ITaskItem[] Files { get; set; }

        /// <summary>
        /// Frameworks that were supported by previous package version.
        ///   Identity: Framework
        ///   Version: Assembly version if supported
        /// </summary>
        [Output]
        public ITaskItem[] SupportedFrameworks { get; set; }

        /// <summary>
        /// Files harvested from previous package version.
        ///   Identity: path to file
        ///   AssemblyVersion: version of assembly
        ///   TargetFramework: target framework moniker to use for harvesting file's dependencies
        ///   TargetPath: path of file in package
        ///   IsReferenceAsset: true for files in Ref.
        /// </summary>
        [Output]
        public ITaskItem[] HarvestedFiles { get; set; }

        /// <summary>
        /// When Files are specified, contains the updated set of files, with removals.
        /// </summary>
        [Output]
        public ITaskItem[] UpdatedFiles { get; set; }

        private Dictionary<string, string> _packageFolders = new Dictionary<string, string>();

        /// <summary>
        /// Generates a table in markdown that lists the API version supported by 
        /// various packages at all levels of NETStandard.
        /// </summary>
        /// <returns></returns>
        public override bool Execute()
        {
            if (LocatePackages())
            {
                if (HarvestAssets)
                {
                    HarvestFilesFromPackage();
                }

                if (Frameworks != null && Frameworks.Length > 0)
                {
                    HarvestSupportedFrameworks();
                }
            }

            return !Log.HasLoggedErrors;
        }

        private bool LocatePackages()
        {
            _packageFolders.Add(PackageId, LocatePackageFolder(PackageId, PackageVersion));

            if (RuntimePackages != null)
            {
                foreach (var runtimePackage in RuntimePackages)
                {
                    _packageFolders.Add(runtimePackage.ItemSpec, LocatePackageFolder(runtimePackage.ItemSpec, runtimePackage.GetMetadata("Version")));
                }
            }

            return _packageFolders.Values.All(f => f != null);
        }

        private void HarvestSupportedFrameworks()
        {
            List<ITaskItem> supportedFrameworks = new List<ITaskItem>();

            AggregateNuGetAssetResolver resolver = new AggregateNuGetAssetResolver(RuntimeFile);
            string packagePath = _packageFolders[PackageId];

            foreach (var packageFolder in _packageFolders)
            {
                resolver.AddPackageItems(packageFolder.Key, GetPackageItems(packageFolder.Value));
            }

            // create a resolver that can be used to determine the API version for inbox assemblies
            // since inbox assemblies are represented with placeholders we can remove the placeholders
            // and use the netstandard reference assembly to determine the API version
            var filesWithoutPlaceholders = GetPackageItems(packagePath)
                .Where(f => !NuGetAssetResolver.IsPlaceholder(f));
            NuGetAssetResolver resolverWithoutPlaceholders = new NuGetAssetResolver(RuntimeFile, filesWithoutPlaceholders);

            string package = $"{PackageId}/{PackageVersion}";

            foreach (var framework in Frameworks)
            {
                var runtimeIds = framework.GetMetadata("RuntimeIDs")?.Split(';');

                NuGetFramework fx;
                try
                {
                    fx = FrameworkUtilities.ParseNormalized(framework.ItemSpec);
                }
                catch (Exception ex)
                {
                    Log.LogError($"Could not parse Framework {framework.ItemSpec}. {ex}");
                    continue;
                }

                if (fx.Equals(NuGetFramework.UnsupportedFramework))
                {
                    Log.LogError($"Did not recognize {framework.ItemSpec} as valid Framework.");
                    continue;
                }

                var compileAssets = resolver.ResolveCompileAssets(fx, PackageId);

                bool hasCompileAsset, hasCompilePlaceHolder;
                NuGetAssetResolver.ExamineAssets(Log, "Compile", package, fx.ToString(), compileAssets, out hasCompileAsset, out hasCompilePlaceHolder);

                // start by making sure it has some asset available for compile
                var isSupported = hasCompileAsset || hasCompilePlaceHolder;

                if (!isSupported)
                {
                    Log.LogMessage(LogImportance.Low, $"Skipping {fx} because it is not supported.");
                    continue;
                }

                foreach (var runtimeId in runtimeIds)
                {
                    string target = String.IsNullOrEmpty(runtimeId) ? fx.ToString() : $"{fx}/{runtimeId}";

                    var runtimeAssets = resolver.ResolveRuntimeAssets(fx, runtimeId);

                    bool hasRuntimeAsset, hasRuntimePlaceHolder;
                    NuGetAssetResolver.ExamineAssets(Log, "Runtime", package, target, runtimeAssets, out hasRuntimeAsset, out hasRuntimePlaceHolder);

                    isSupported &= hasCompileAsset == hasRuntimeAsset;
                    isSupported &= hasCompilePlaceHolder == hasRuntimePlaceHolder;

                    if (!isSupported)
                    {
                        Log.LogMessage(LogImportance.Low, $"Skipping {fx} because it is not supported on {target}.");
                        break;
                    }
                }

                if (isSupported)
                {
                    var supportedFramework = new TaskItem(framework.ItemSpec);
                    supportedFramework.SetMetadata("HarvestedFromPackage", package);

                    // set version

                    // first try the resolved compile asset for this package
                    var refAssm = compileAssets.FirstOrDefault(r => !NuGetAssetResolver.IsPlaceholder(r))?.Substring(PackageId.Length + 1);

                    if (refAssm == null)
                    {
                        // if we didn't have a compile asset it means this framework is supported inbox with a placeholder
                        // resolve the assets without placeholders to pick up the netstandard reference assembly.
                        compileAssets = resolverWithoutPlaceholders.ResolveCompileAssets(fx);
                        refAssm = compileAssets.FirstOrDefault(r => !NuGetAssetResolver.IsPlaceholder(r));
                    }

                    string version = "unknown";
                    if (refAssm != null)
                    {
                        version = VersionUtility.GetAssemblyVersion(Path.Combine(packagePath, refAssm))?.ToString() ?? version;
                    }

                    supportedFramework.SetMetadata("Version", version);

                    Log.LogMessage(LogImportance.Low, $"Validating version {version} for {supportedFramework.ItemSpec} because it was supported by {PackageId}/{PackageVersion}.");

                    supportedFrameworks.Add(supportedFramework);
                }
            }

            SupportedFrameworks = supportedFrameworks.ToArray();
        }

        public void HarvestFilesFromPackage()
        {
            string pathToPackage = _packageFolders[PackageId];

            var livePackageItems = Files.NullAsEmpty()
                .Where(f => IsIncludedExtension(f.GetMetadata("Extension")))
                .Select(f => new PackageItem(f));

            var livePackageFiles = new Dictionary<string, PackageItem>(StringComparer.OrdinalIgnoreCase);
            foreach (var livePackageItem in livePackageItems)
            {
                PackageItem existingitem;

                if (livePackageFiles.TryGetValue(livePackageItem.TargetPath, out existingitem))
                {
                    Log.LogError($"Package contains two files with same targetpath: {livePackageItem.TargetPath}, items:{livePackageItem.SourcePath}, {existingitem.SourcePath}.");
                }
                else
                {
                    livePackageFiles.Add(livePackageItem.TargetPath, livePackageItem);
                }
            }

            var harvestedFiles = new List<ITaskItem>();
            var removeFiles = new List<ITaskItem>();

            // make sure we preserve refs that match desktop assemblies
            var liveDesktopDlls = livePackageFiles.Values.Where(pi => pi.IsDll && pi.TargetFramework?.Framework == FrameworkConstants.FrameworkIdentifiers.Net);
            var desktopRefVersions = liveDesktopDlls.Where(d => d.IsRef && d.Version != null).Select(d => d.Version);
            var desktopLibVersions = liveDesktopDlls.Where(d => !d.IsRef && d.Version != null).Select(d => d.Version);
            
            // find desktop assemblies with no matching lib.
            var preserveRefVersion = new HashSet<Version>(desktopLibVersions);
            preserveRefVersion.ExceptWith(desktopRefVersions);

            foreach (var extension in s_includedExtensions)
            {
                foreach (var packageFile in Directory.EnumerateFiles(pathToPackage, $"*{extension}", SearchOption.AllDirectories))
                {
                    string harvestPackagePath = packageFile.Substring(pathToPackage.Length + 1).Replace('\\', '/');

                    // determine if we should include this file from the harvested package

                    // exclude if its specifically set for exclusion
                    if (ShouldExclude(harvestPackagePath))
                    {
                        Log.LogMessage(LogImportance.Low, $"Excluding package path {harvestPackagePath} because it is specifically excluded.");
                        continue;
                    }

                    ITaskItem includeItem = null;
                    if (!IncludeAllPaths && !ShouldInclude(harvestPackagePath, out includeItem))
                    {
                        Log.LogMessage(LogImportance.Low, $"Excluding package path {harvestPackagePath} because it is not included in {nameof(PathsToInclude)}.");
                        continue;
                    }

                    // allow for the harvested item to be moved
                    var remappedTargetPath = includeItem?.GetMetadata("TargetPath");
                    if (!String.IsNullOrEmpty(remappedTargetPath))
                    {
                        harvestPackagePath = remappedTargetPath + '/' + Path.GetFileName(packageFile);
                    }

                    List<string> targetPaths = new List<string>() { harvestPackagePath };

                    var additionalTargetPaths = includeItem?.GetMetadata("AdditionalTargetPath");
                    if (!String.IsNullOrEmpty(additionalTargetPaths))
                    {
                        foreach (var additionalTargetPath in additionalTargetPaths.Split(';'))
                        {
                            if (!String.IsNullOrEmpty(additionalTargetPath))
                            {
                                targetPaths.Add(additionalTargetPath + '/' + Path.GetFileName(packageFile));
                            }
                        }
                    }

                    var assemblyVersion = extension == s_dll ? VersionUtility.GetAssemblyVersion(packageFile) : null;
                    PackageItem liveFile = null;

                    foreach (var livePackagePath in targetPaths)
                    {
                        // determine if the harvested file clashes with a live built file
                        // we'll prefer the harvested reference assembly so long as it's the same API
                        // version and not required to match implementation 1:1 as is the case for desktop
                        if (livePackageFiles.TryGetValue(livePackagePath, out liveFile))
                        {
                            // Not a dll, or not a versioned assembly: prefer live built file.
                            if (extension != s_dll || assemblyVersion == null || liveFile.Version == null)
                            {
                                // we don't consider this an error even for explicitly included files 
                                Log.LogMessage(LogImportance.Low, $"Preferring live build of package path {livePackagePath} over the asset from last stable package because the file is not versioned.");
                                continue;
                            }

                            // not a ref
                            if (!liveFile.IsRef)
                            {
                                LogSkipIncludedFile(livePackagePath, " because it is a newer implementation.");
                                continue;
                            }

                            // preserve desktop references to ensure bindingRedirects will work.
                            if (liveFile.TargetFramework.Framework == FrameworkConstants.FrameworkIdentifiers.Net)
                            {
                                LogSkipIncludedFile(livePackagePath, " because it is desktop reference.");
                                continue;
                            }

                            // as above but handle the case where a netstandard ref may be used for a desktop impl.
                            if (preserveRefVersion.Contains(liveFile.Version))
                            {
                                LogSkipIncludedFile(livePackagePath, " because it will be applicable for desktop projects.");
                                continue;
                            }

                            // preserve references with a different major.minor version
                            if (assemblyVersion.Major != liveFile.Version.Major ||
                                assemblyVersion.Minor != liveFile.Version.Minor)
                            {
                                LogSkipIncludedFile(livePackagePath, $" because it is a different API version ( {liveFile.Version.Major}.{liveFile.Version.Minor} vs {assemblyVersion.Major}.{assemblyVersion.Minor}.");
                                continue;
                            }

                            // preserve references that specifically set the preserve metadata.
                            bool preserve = false;
                            bool.TryParse(liveFile.OriginalItem.GetMetadata("Preserve"), out preserve);
                            if (preserve)
                            {
                                LogSkipIncludedFile(livePackagePath, " because it set metadata Preserve=true.");
                                continue;
                            }

                            // replace the live file with the harvested one, removing both the live file and PDB from the
                            // file list.
                            Log.LogMessage($"Using reference {livePackagePath} from last stable package {PackageId}/{PackageVersion} rather than the built reference {liveFile.SourcePath} since it is the same API version.  Set <Preserve>true</Preserve> on {liveFile.SourceProject} if you'd like to avoid this..");
                            removeFiles.Add(liveFile.OriginalItem);

                            PackageItem livePdbFile;
                            if (livePackageFiles.TryGetValue(Path.ChangeExtension(livePackagePath, ".pdb"), out livePdbFile))
                            {
                                removeFiles.Add(livePdbFile.OriginalItem);
                            }
                        }
                        else
                        {
                            Log.LogMessage(LogImportance.Low, $"Including {livePackagePath} from last stable package {PackageId}/{PackageVersion}.");
                        }

                        var item = new TaskItem(packageFile);

                        if (liveFile?.OriginalItem != null)
                        {
                            // preserve all the meta-data from the live file that was replaced.
                            liveFile.OriginalItem.CopyMetadataTo(item);
                        }
                        else
                        {
                            if (includeItem != null)
                            {
                                includeItem.CopyMetadataTo(item);
                            }
                            var targetPath = Path.GetDirectoryName(livePackagePath).Replace('\\', '/');
                            item.SetMetadata("TargetPath", targetPath);
                            string targetFramework = GetTargetFrameworkFromPackagePath(targetPath);
                            item.SetMetadata("TargetFramework", targetFramework);
                            // only harvest for non-portable frameworks, matches logic in packaging.targets.
                            bool harvestDependencies = !targetFramework.StartsWith("portable-");
                            item.SetMetadata("HarvestDependencies", harvestDependencies.ToString());
                            item.SetMetadata("IsReferenceAsset", IsReferencePackagePath(targetPath).ToString());
                        }

                        if (assemblyVersion != null)
                        {
                            // overwrite whatever metadata may have been copied from the live file.
                            item.SetMetadata("AssemblyVersion", assemblyVersion.ToString());
                        }

                        item.SetMetadata("HarvestedFrom", $"{PackageId}/{PackageVersion}/{harvestPackagePath}");

                        harvestedFiles.Add(item);
                    }
                }
            }

            HarvestedFiles = harvestedFiles.ToArray();

            if (_pathsNotIncluded != null)
            {
                foreach (var pathNotIncluded in _pathsNotIncluded)
                {
                    Log.LogError($"Path '{pathNotIncluded}' was specified in {nameof(PathsToInclude)} but was not found in the package {PackageId}/{PackageVersion}.");
                }
            }

            if (Files != null)
            {
                UpdatedFiles = Files.Except(removeFiles).ToArray();
            }
        }

        private string LocatePackageFolder(string packageId, string packageVersion)
        {
            foreach (var packageFolder in PackagesFolders)
            {
                var candidateFolder = Path.Combine(packageFolder, packageId, packageVersion);

                if (Directory.Exists(candidateFolder))
                {
                    return candidateFolder;
                }

                // handle lower-case restore path
                candidateFolder = Path.Combine(packageFolder, packageId.ToLowerInvariant(), packageVersion.ToLowerInvariant());

                if (Directory.Exists(candidateFolder))
                {
                    return candidateFolder;
                }
            }

            Log.LogError($"Cannot locate package '{PackageId}' version '{PackageVersion}' under '{string.Join(", ", PackagesFolders)}'.  Harvesting is needed to redistribute assets and ensure compatibility with the previous release.  You can disable this by setting HarvestStablePackage=false.");

            return null;
        }

        private void LogSkipIncludedFile(string packagePath, string reason)
        {
            if (IncludeAllPaths)
            {
                Log.LogMessage(LogImportance.Low, $"Preferring live build of package path {packagePath} over the asset from last stable package{reason}.");
            }
            else
            {
                Log.LogError($"Package path {packagePath} was specified to be harvested but it conflicts with live build{reason}.");
            }
        }

        private HashSet<string> _pathsToExclude = null;
        private bool ShouldExclude(string packagePath)
        {
            if (_pathsToExclude == null)
            {
                _pathsToExclude = new HashSet<string>(PathsToExclude.NullAsEmpty().Select(NormalizePath), StringComparer.OrdinalIgnoreCase);
            }

            return ShouldSuppress(packagePath) || ProbePath(packagePath, _pathsToExclude);
        }

        private Dictionary<string, ITaskItem> _pathsToInclude = null;
        private HashSet<string> _pathsNotIncluded = null;
        private bool ShouldInclude(string packagePath, out ITaskItem includeItem)
        {
            if (_pathsToInclude == null)
            {
                _pathsToInclude = PathsToInclude.NullAsEmpty().ToDictionary(i => NormalizePath(i.ItemSpec), i=> i, StringComparer.OrdinalIgnoreCase);
                _pathsNotIncluded = new HashSet<string>(_pathsToInclude.Keys);
            }

            return ProbePath(packagePath, _pathsToInclude, _pathsNotIncluded, out includeItem);
        }

        private HashSet<string> _pathsToSuppress = null;
        private bool ShouldSuppress(string packagePath)
        {
            if (_pathsToSuppress == null)
            {
                _pathsToSuppress = new HashSet<string>(PathsToSuppress.NullAsEmpty().Select(NormalizePath));
            }

            return ProbePath(packagePath, _pathsToSuppress);
        }

        private static bool ProbePath(string path, ICollection<string> pathsIncluded)
        {
            for (var probePath = NormalizePath(path); 
                !String.IsNullOrEmpty(probePath);
                probePath = NormalizePath(Path.GetDirectoryName(probePath)))
            {
                if (pathsIncluded.Contains(probePath))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool ProbePath<T>(string path, IDictionary<string, T> pathsIncluded, ICollection<string> pathsNotIncluded, out T result)
        {
            result = default(T);

            for (var probePath = NormalizePath(path);
                !String.IsNullOrEmpty(probePath);
                probePath = NormalizePath(Path.GetDirectoryName(probePath)))
            {
                if (pathsIncluded.TryGetValue(probePath, out result))
                {
                    pathsNotIncluded.Remove(probePath);
                    return true;
                }
            }

            return false;
        }

        private static string NormalizePath(string path)
        {
            return path?.Replace('\\', '/')?.Trim();
        }

        private static string GetTargetFrameworkFromPackagePath(string path)
        {
            var parts = path.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

            if (parts.Length >= 2)
            {
                if (parts[0].Equals("lib", StringComparison.OrdinalIgnoreCase) ||
                    parts[0].Equals("ref", StringComparison.OrdinalIgnoreCase))
                {
                    return parts[1];
                }

                if (parts.Length >= 4 &&
                    parts[0].Equals("runtimes", StringComparison.OrdinalIgnoreCase) &&
                    parts[2].Equals("lib", StringComparison.OrdinalIgnoreCase))
                {
                    return parts[3];
                }
            }

            return null;
        }

        private static string s_dll = ".dll";
        private static string[] s_includedExtensions = new[] { s_dll, ".pdb", ".xml", "._" };
        private static bool IsIncludedExtension(string extension)
        {
            return extension != null && extension.Length > 0 && s_includedExtensions.Contains(extension, StringComparer.OrdinalIgnoreCase);
        }

        private static bool IsReferencePackagePath(string path)
        {
            return path.StartsWith("ref", StringComparison.OrdinalIgnoreCase);
        }

        private IEnumerable<string> GetPackageItems(string packageFolder)
        {
            return Directory.EnumerateFiles(packageFolder, "*", SearchOption.AllDirectories)
                .Select(f => f.Substring(packageFolder.Length + 1).Replace('\\', '/'))
                .Where(f => !ShouldSuppress(f));
        }
    }
}
