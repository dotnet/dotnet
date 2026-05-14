// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Build.Framework;
using Newtonsoft.Json;
using NuGet.Frameworks;
using NuGet.Packaging;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DotNet.Build.Tasks.Packaging
{
    public class UpdatePackageIndex : BuildTask
    {
        private HashSet<string> _packageIdsToInclude;

        /// <summary>
        /// File to update or create
        /// </summary>
        [Required]
        public ITaskItem PackageIndexFile { get; set; }

        /// <summary>
        /// Specific packages to index
        /// </summary>
        public ITaskItem[] Packages { get; set; }

        /// <summary>
        /// Baseline packages to add
        ///   Identity: Package ID
        ///   Version: Package version
        /// </summary>
        public ITaskItem[] BaselinePackages { get; set; }

        /// <summary>
        /// Stable packages to add
        ///   Identity: Package ID
        ///   Version: Package version
        /// </summary>
        public ITaskItem[] StablePackages { get; set; }

        /// <summary>
        /// Module to package mappings to add
        ///   Identity: Module name without extension
        ///   Package: Package id which provides module
        /// </summary>
        public ITaskItem[] ModuleToPackages { get; set; }

        /// <summary>
        /// When used with PackageFolders restricts the set of packages indexed.
        /// </summary>
        public ITaskItem[] PackageIds { get; set; }

        /// <summary>
        /// Folders to index, can contain flat set of packages or expanded package format.
        /// </summary>
        public ITaskItem[] PackageFolders { get; set; }

        /// <summary>
        /// Root folder containing subfolders with framework lists for targeting packs
        /// Subfolders must be named by TFM.
        /// </summary>
        public ITaskItem InboxFrameworkListFolder { get; set; }

        /// <summary>
        /// Folder containing dlls that will be considered inbox
        ///   Identity: path to folder containing dlls
        ///   TargetFramework: framework which path represents
        /// </summary>
        public ITaskItem[] InboxFrameworkLayoutFolders { get; set; }

        public bool SetBaselineVersionsToLatestStableVersion { get; set; }

        public bool UpdateStablePackageInfo { get; set; }

        /// <summary>
        /// Pre-release version to use for all pre-release packages covered by this index.
        /// </summary>
        public string PreRelease { get; set; }

        public override bool Execute()
        {
            string indexFilePath = PackageIndexFile.GetMetadata("FullPath");

            PackageIndex index = File.Exists(indexFilePath) ?
                index = PackageIndex.Load(indexFilePath) :
                new PackageIndex();

            if (PackageIds != null && PackageIds.Any())
            {
                _packageIdsToInclude = new HashSet<string>(PackageIds.Select(i => i.ItemSpec), StringComparer.OrdinalIgnoreCase);
            }

            foreach(var package in Packages.NullAsEmpty().Select(f => f.GetMetadata("FullPath")))
            {
                Log.LogMessage($"Updating from {package}.");
                UpdateFromPackage(index, package);
            }

            foreach(var packageFolder in PackageFolders.NullAsEmpty().Select(f => f.GetMetadata("FullPath")))
            {
                var nupkgs = Directory.EnumerateFiles(packageFolder, "*.nupkg", SearchOption.TopDirectoryOnly);

                if (nupkgs.Any())
                {
                    foreach(var nupkg in nupkgs)
                    {
                        Log.LogMessage($"Updating from {nupkg}.");
                        UpdateFromPackage(index, nupkg, true);
                    }
                }
                else
                {
                    var nuspecFolders = Directory.EnumerateFiles(packageFolder, "*.nuspec", SearchOption.AllDirectories)
                        .Select(nuspec => Path.GetDirectoryName(nuspec));

                    foreach (var nuspecFolder in nuspecFolders)
                    {
                        Log.LogMessage($"Updating from {nuspecFolder}.");
                        UpdateFromFolderLayout(index, nuspecFolder, true);
                    }
                }
            }

            if (BaselinePackages != null)
            {
                foreach (var baselinePackage in BaselinePackages)
                {
                    var info = GetOrCreatePackageInfo(index, baselinePackage.ItemSpec);
                    var version = baselinePackage.GetMetadata("Version");

                    info.BaselineVersion = Version.Parse(version);
                }
            }

            if (StablePackages != null)
            {
                foreach (var stablePackage in StablePackages)
                {
                    var info = GetOrCreatePackageInfo(index, stablePackage.ItemSpec);
                    var version = stablePackage.GetMetadata("Version");

                    info.StableVersions.Add(Version.Parse(version));
                }
            }

            if (ModuleToPackages != null)
            {
                foreach (var moduleToPackage in ModuleToPackages)
                {
                    var package = moduleToPackage.GetMetadata("Package");
                    index.ModulesToPackages[moduleToPackage.ItemSpec] = package;
                }
            }

            if (InboxFrameworkListFolder != null)
            {
                index.MergeFrameworkLists(InboxFrameworkListFolder.GetMetadata("FullPath"));
            }

            if (InboxFrameworkLayoutFolders != null)
            {
                foreach(var inboxFrameworkLayoutFolder in InboxFrameworkLayoutFolders)
                {
                    var layoutDirectory = inboxFrameworkLayoutFolder.GetMetadata("FullPath");
                    var targetFramework = NuGetFramework.Parse(inboxFrameworkLayoutFolder.GetMetadata("TargetFramework"));

                    index.MergeInboxFromLayout(targetFramework, layoutDirectory);
                }
            }

            if (SetBaselineVersionsToLatestStableVersion)
            {
                foreach(var packageInfo in index.Packages.Values)
                {
                    var maxVersion = packageInfo.StableVersions.Max();
                    packageInfo.BaselineVersion = maxVersion;
                }
            }

            if (UpdateStablePackageInfo && Packages == null && PackageFolders == null)
            {
                // Given we will query the web for every package, we should run in parallel to try to optimize the performance.
                Parallel.ForEach(index.Packages, (package) =>
                {
                    IEnumerable<Version> stablePackageVersions = NuGetUtility.GetAllVersionsForPackageId(package.Key, includePrerelease: false, includeUnlisted: false, Log, CancellationToken.None);
                    package.Value.StableVersions.Clear();
                    package.Value.StableVersions.AddRange(stablePackageVersions);
                });
            }

            if (!String.IsNullOrEmpty(PreRelease))
            {
                index.PreRelease = PreRelease;
            }

            index.Save(indexFilePath);

            return !Log.HasLoggedErrors;
        }

        private void UpdateFromFolderLayout(PackageIndex index, string path, bool filter = false)
        {
            var version = NuGetVersion.Parse(Path.GetFileName(path));
            var id = Path.GetFileName(Path.GetDirectoryName(path));

            if (filter && !ShouldInclude(id))
            {
                return;
            }

            var dlls = Directory.EnumerateFiles(path, "*.dll", SearchOption.AllDirectories);

            var assemblyVersions = dlls.Select(f => VersionUtility.GetAssemblyVersion(f));
            var dllNames = dlls.Select(f => Path.GetFileNameWithoutExtension(f)).Distinct();

            UpdateFromValues(index, id, version, assemblyVersions, dllNames);
        }

        private void UpdateFromPackage(PackageIndex index, string packagePath, bool filter = false)
        {
            string id;
            NuGetVersion version;
            IEnumerable<Version> assemblyVersions;
            IEnumerable<string> dllNames;

            using (var reader = new PackageArchiveReader(packagePath))
            {
                var identity = reader.GetIdentity();
                id = identity.Id;
                version = identity.Version;

                if (filter && !ShouldInclude(id))
                {
                    return;
                }

                var dlls = reader.GetFiles().Where(f => Path.GetExtension(f).Equals(".dll", StringComparison.OrdinalIgnoreCase));

                assemblyVersions = dlls.Select(refFile =>
                {
                    using (var refStream = reader.GetStream(refFile))
                    using (var memStream = new MemoryStream())
                    {
                        refStream.CopyTo(memStream);
                        memStream.Seek(0, SeekOrigin.Begin);
                        return VersionUtility.GetAssemblyVersion(memStream);
                    }
                }).ToArray();

                dllNames = dlls.Select(f => Path.GetFileNameWithoutExtension(f)).Distinct().ToArray();
            }

            UpdateFromValues(index, id, version, assemblyVersions, dllNames);
        }

        private void UpdateFromValues(PackageIndex index, string id, NuGetVersion version, IEnumerable<Version> assemblyVersions, IEnumerable<string> dllNames)
        {
            PackageInfo info = GetOrCreatePackageInfo(index, id);

            if (UpdateStablePackageInfo)
            {
                try
                {
                    IEnumerable<Version> allStableVersions = NuGetUtility.GetAllVersionsForPackageId(id, includePrerelease: false, includeUnlisted: false, Log, CancellationToken.None);
                    info.StableVersions.AddRange(allStableVersions);
                }
                catch(NuGetProtocolException)
                {
                    Log.LogWarning("Failed fetching stable nuget package versions from one or more of your feeds. Make sure you are connected to the internet and that all your feeds are reachable.");
                }
            }

            var packageVersion = VersionUtility.As3PartVersion(version.Version);
            // if we have a stable version, add it to the stable versions list
            if (!version.IsPrerelease)
            {
                info.StableVersions.Add(packageVersion);
            }

            var assmVersions = new HashSet<Version>(assemblyVersions.NullAsEmpty().Where(v => v != null));

            // add any new assembly versions
            info.AddAssemblyVersionsInPackage(assmVersions, packageVersion);

            // try to find an identity package to also add a mapping in the case this is a runtime package
            if (id.StartsWith("runtime."))
            {
                foreach (var dllName in dllNames)
                {
                    PackageInfo identityInfo;
                    if (index.Packages.TryGetValue(dllName, out identityInfo))
                    {
                        identityInfo.AddAssemblyVersionsInPackage(assmVersions, packageVersion);
                    }
                }
            }

            // remove any assembly mappings which claim to be in this package version, but aren't in the assemblyList
            var orphanedAssemblyVersions = info.AssemblyVersionInPackageVersion
                                                .Where(pair => pair.Value == packageVersion && !assmVersions.Contains(pair.Key))
                                                .Select(pair => pair.Key);

            if (orphanedAssemblyVersions.Any())
            {
                // make sure these aren't coming from a runtime package.
                var runtimeAssemblyVersions = index.Packages
                    .Where(p => p.Key.StartsWith("runtime.") && p.Key.EndsWith(id))
                    .SelectMany(p => p.Value.AssemblyVersionInPackageVersion)
                    .Where(pair => pair.Value == packageVersion)
                    .Select(pair => pair.Key);

                orphanedAssemblyVersions = orphanedAssemblyVersions.Except(runtimeAssemblyVersions);
            }

            foreach (var orphanedAssemblyVersion in orphanedAssemblyVersions.ToArray())
            {
                info.AssemblyVersionInPackageVersion.Remove(orphanedAssemblyVersion);
            }

            // if no assemblies are present in this package nor were ever present
            if (assmVersions.Count == 0 &&
                info.AssemblyVersionInPackageVersion.Count == 0)
            {
                // if in the native module map
                if (index.ModulesToPackages.Values.Any(p => p.Equals(id)))
                {
                    // ensure the baseline is set
                    info.BaselineVersion = packageVersion;
                }
            }
        }

        private PackageInfo GetOrCreatePackageInfo(PackageIndex index, string id)
        {
            PackageInfo info;

            if (!index.Packages.TryGetValue(id, out info))
            {
                index.Packages[id] = info = new PackageInfo();
            }

            return info;
        }

        private bool ShouldInclude(string packageId)
        {
            return (_packageIdsToInclude != null) ? _packageIdsToInclude.Contains(packageId) : true;
        }
    }
}
