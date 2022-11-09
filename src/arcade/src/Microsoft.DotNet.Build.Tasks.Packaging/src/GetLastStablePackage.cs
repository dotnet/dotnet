using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using NuGet.Versioning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.DotNet.Build.Tasks.Packaging
{
    public class GetLastStablePackage : BuildTask
    {
        /// <summary>
        /// List of packages to look up.
        /// The closest StablePackage version that is less than the version of each of these packages will be returned in LastStablePackages.
        ///   Identity: Package ID
        ///   Version: Package version.
        /// </summary>
        [Required]
        public ITaskItem[] LatestPackages { get; set; }

        /// <summary>
        /// List of previously shipped packages.
        ///   Identity: Package ID
        ///   Version: Package version.
        /// </summary>
        public ITaskItem[] StablePackages { get; set; }

        /// <summary>
        /// Package index files used to define stable packages.
        /// </summary>
        public ITaskItem[] PackageIndexes { get; set; }

        /// <summary>
        /// <see langword="true"/> if the result version can be a version from the same release.
        /// <see langword="false"/> otherwise. Defaults to false.
        /// </summary>
        public bool DoNotAllowVersionsFromSameRelease { get; set; }
        

        /// <summary>
        /// Latest version from StablePackages for all packages in LatestPackages.
        /// If a version isn't found for an item in LatestPackage that will not be included in this set.
        /// </summary>
        [Output]
        public ITaskItem[] LastStablePackages { get; set; }

        public override bool Execute()
        {
            if (LatestPackages == null || LatestPackages.Length == 0)
            {
                return true;
            }

            if (PackageIndexes != null && PackageIndexes.Length > 0)
            {
                GetLastStablePackagesFromIndex();
            }
            else
            {
                GetLastStablePackagesFromStablePackages();
            }

            return !Log.HasLoggedErrors;
        }

        public void GetLastStablePackagesFromStablePackages()
        {
            Dictionary<string, ITaskItem> originalItems = new Dictionary<string, ITaskItem>();
            Dictionary<string, Version> latestPackages = new Dictionary<string, Version>();
            Dictionary<string, Version> lastStablePackages = new Dictionary<string, Version>();

            foreach (var latestPackage in LatestPackages)
            {
                var packageId = latestPackage.ItemSpec;

                var versionString = latestPackage.GetMetadata("Version");
                NuGetVersion nuGetVersion = null;
                if (versionString == null || !NuGetVersion.TryParse(versionString, out nuGetVersion))
                {
                    Log.LogMessage($"Could not parse version {versionString} for LatestPackage {packageId}, will use latest stable.");
                }

                latestPackages[packageId] = nuGetVersion?.Version;
                originalItems[packageId] = latestPackage;
            }

            foreach (var stablePackage in StablePackages.NullAsEmpty())
            {
                var packageId = stablePackage.ItemSpec;

                Version latestVersion;
                if (!latestPackages.TryGetValue(packageId, out latestVersion))
                {
                    continue;
                }

                var versionString = stablePackage.GetMetadata("Version");
                Version stableVersion;
                if (versionString == null || !Version.TryParse(versionString, out stableVersion))
                {
                    Log.LogError($"Could not parse version {versionString} for StablePackage {packageId}");
                    continue;
                }
                stableVersion = VersionUtility.As4PartVersion(stableVersion);

                // only consider a stable version less or equal to than current version
                if (latestVersion != null && stableVersion >= latestVersion)
                {
                    continue;
                }

                Version lastStableVersion;
                if (!lastStablePackages.TryGetValue(packageId, out lastStableVersion) || lastStableVersion < stableVersion)
                {
                    lastStablePackages[packageId] = stableVersion;
                }
            }

            LastStablePackages = lastStablePackages.Select(p => CreateItem(originalItems[p.Key], p.Value)).ToArray();
        }

        public void GetLastStablePackagesFromIndex()
        {
            var index = PackageIndex.Load(PackageIndexes.Select(pi => pi.GetMetadata("FullPath")));

            List<ITaskItem> lastStablePackages = new List<ITaskItem>();

            foreach (var latestPackage in LatestPackages)
            {
                var packageId = latestPackage.ItemSpec;

                var versionString = latestPackage.GetMetadata("Version");
                NuGetVersion nuGetVersion = null;
                if (versionString == null || !NuGetVersion.TryParse(versionString, out nuGetVersion))
                {
                    Log.LogMessage($"Could not parse version {versionString} for LatestPackage {packageId}, will use latest stable.");
                }

                var latestVersion = (DoNotAllowVersionsFromSameRelease) ? VersionUtility.As2PartVersion(nuGetVersion?.Version) : nuGetVersion?.Version;

                PackageInfo info;
                if (index.Packages.TryGetValue(packageId, out info))
                {
                    IEnumerable<Version> candidateVersions = (latestVersion == null) ? info.StableVersions : info.StableVersions.Where(sv => VersionUtility.As4PartVersion(sv) < latestVersion);
                    if (candidateVersions.Any())
                    {
                        lastStablePackages.Add(CreateItem(latestPackage, candidateVersions.Max()));
                    }
                }
            }

            LastStablePackages = lastStablePackages.ToArray();

        }

        private ITaskItem CreateItem(ITaskItem originalItem, Version version)
        {
            var item = new TaskItem(originalItem);
            item.SetMetadata("Version", version.ToString(3));
            return item;
        }
    }
}
