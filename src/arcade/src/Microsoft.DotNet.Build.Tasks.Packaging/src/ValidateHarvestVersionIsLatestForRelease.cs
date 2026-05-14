// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using NuGet.Versioning;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DotNet.Build.Tasks.Packaging
{
    /// <summary>
    /// MSBuild Task that validates if a package is harvesting
    /// the latest package version for a specific package release.
    /// </summary>
    public class ValidateHarvestVersionIsLatestForRelease : BuildTask
    {
        /// <summary>
        /// Item containing all package reports where the item spec is the path to the report.
        /// </summary>
        [Required]
        public ITaskItem[] PackageReports { get; set; }

        public override bool Execute()
        {
            // Run all validations in parallel
            Parallel.ForEach(PackageReports, (reportPath) => ValidateHarvestVersionForReport(reportPath.ItemSpec));
            return !Log.HasLoggedErrors;
        }

        private void ValidateHarvestVersionForReport(string packageReportPath)
        {
            PackageReport packageReport = GetPackageReportFromPath(packageReportPath);
            bool isHarvestingAssetsFromPackage = TryGetHarvestVersionFromReport(packageReport, out string harvestVersion, out int harvestMajor, out int harvestMinor);

            if (isHarvestingAssetsFromPackage)
            {
                if (packageReport.Version.StartsWith($"{harvestMajor}.{harvestMinor}."))
                {
                    Log.LogError($"Validation Failed: {packageReport.Id} is harvesting package version {harvestVersion} which belongs to the current package release: {packageReport.Version}");
                    return;
                }

                string latestPatchVersion = GetLatestStableVersionForPackageRelease(packageReport.Id, harvestMajor, harvestMinor);
                if (latestPatchVersion.CompareTo(harvestVersion) != 0)
                {
                    Log.LogError($"Validation Failed: {packageReport.Id} is harvesting assets from package version {harvestVersion} which is not the latest for that package release. Latest package version from that release is {latestPatchVersion}. In order to fix this, run `dotnet msbuild {packageReport.Id}.pkgproj /t:UpdateHarvestVersionOnPackageIndex /p:UpdateStablePackageInfo=true`");
                }
                else
                {
                    Log.LogMessage(LogImportance.Normal, $"Validation Succeeded: {packageReport.Id} is harvesting assets from package version {harvestVersion} which is the latest for that package erreleasea.");
                }
            }
            else
            {
                Log.LogMessage(LogImportance.Normal, $"Validation Succeeded: {packageReport.Id} is not harvesting any assets.");
            }
        }

        // Making this method protected virtual for tests.
        protected virtual PackageReport GetPackageReportFromPath(string path)
        {
            return PackageReport.Load(path);
        }

        private bool TryGetHarvestVersionFromReport(PackageReport report, out string harvestVersion, out int harvestMajor, out int harvestMinor)
        {
            harvestVersion = string.Empty;
            harvestMajor = harvestMinor = 0;

            foreach (KeyValuePair<string, Target> packageTarget in report.Targets.NullAsEmpty())
            {
                foreach (PackageAsset compileAsset in packageTarget.Value.CompileAssets.NullAsEmpty())
                {
                    if (!string.IsNullOrEmpty(compileAsset.HarvestedFrom))
                    {
                        return GetHarvestVersionFromString(compileAsset.HarvestedFrom, report.Id, out harvestVersion, out harvestMajor, out harvestMinor);
                    }
                }

                foreach (PackageAsset runtimeAsset in packageTarget.Value.RuntimeAssets.NullAsEmpty())
                {
                    if (!string.IsNullOrEmpty(runtimeAsset.HarvestedFrom))
                    {
                        return GetHarvestVersionFromString(runtimeAsset.HarvestedFrom, report.Id, out harvestVersion, out harvestMajor, out harvestMinor);
                    }
                }

                foreach (PackageAsset nativeAsset in packageTarget.Value.NativeAssets.NullAsEmpty())
                {
                    if (!string.IsNullOrEmpty(nativeAsset.HarvestedFrom))
                    {
                        return GetHarvestVersionFromString(nativeAsset.HarvestedFrom, report.Id, out harvestVersion, out harvestMajor, out harvestMinor);
                    }
                }
            }

            return false;
        }

        private bool GetHarvestVersionFromString(string harvestedFrom, string packageId, out string harvestVersion, out int harvestMajor, out int harvestMinor)
        {
            harvestVersion = string.Empty;
            harvestMajor = harvestMinor = 0;
            string patternToSearchFor = $"{packageId}/";
            int startIndex = harvestedFrom.IndexOf(patternToSearchFor);
            if (startIndex != -1)
            {
                startIndex += patternToSearchFor.Length;
                int endIndex = harvestedFrom.IndexOf("/", startIndex);
                if (endIndex != -1)
                {
                    harvestVersion = harvestedFrom.Substring(startIndex, endIndex - startIndex);
                    NuGetVersion harvestPackageVersion = new NuGetVersion(harvestVersion);
                    harvestMajor = harvestPackageVersion.Major;
                    harvestMinor = harvestPackageVersion.Minor;
                    return true;
                }
                else
                {
                    Log.LogError($"Failed to parse package version from string: {harvestedFrom}");
                    return false;
                }
            }
            else
            {
                Log.LogError($"Failed to parse package version from string: {harvestedFrom}");
                return false;
            }
        }

        // Making this method protected virtual for tests.
        protected virtual string GetLatestStableVersionForPackageRelease(string packageId, int releaseMajorVersion, int releaseMinorVersion)
        {
            IEnumerable<Version> packageVersions = NuGetUtility.GetAllVersionsForPackageId(packageId, includePrerelease: false, includeUnlisted: false, Log, CancellationToken.None); 
            Version latestPatchVersion = packageVersions.GetLatestPatchStableVersionForRelease(releaseMajorVersion, releaseMinorVersion);
            return (latestPatchVersion == null) ? string.Empty : $"{latestPatchVersion.Major}.{latestPatchVersion.Minor}.{latestPatchVersion.Build}";
        }
    }
}
