// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable enable

using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Microsoft.Deployment.DotNet.Releases;
using Microsoft.DotNet.Build.Tasks.Workloads.Msi;
using NuGet.Versioning;

namespace Microsoft.DotNet.Build.Tasks.Workloads
{
    /// <summary>
    /// Represents a NuGet package for a .NET workload set.
    /// </summary>
    internal class WorkloadSetPackage : WorkloadPackageBase
    {
        /// <summary>
        /// Prefix separating the SDK feature band within the package ID.
        /// </summary>
        internal const string SdkFeatureBandSeparator = "Microsoft.NET.Workloads.";

        public override PackageExtractionMethod ExtractionMethod => PackageExtractionMethod.Unzip;

        public override Version MsiVersion
        {
            get;
        }

        /// <summary>
        /// Gets the 4-part version of the workload set.
        /// </summary>
        public string WorkloadSetVersion
        {
            get;
        }

        /// <summary>
        /// The SDK feature band version associated with this workload set package.
        /// </summary>
        public ReleaseVersion SdkFeatureBand
        {
            get;
        }

        public WorkloadSetPackage(ITaskItem package, string destinationBaseDirectory, Version msiVersion,
            ITaskItem[]? shortNames = null, TaskLoggingHelper? log = null) :
            base(package.ItemSpec, destinationBaseDirectory, shortNames, log)
        {
            MsiVersion = GetMsiVersion(package, msiVersion, nameof(CreateVisualStudioWorkloadSet),
                nameof(CreateVisualStudioWorkloadSet.WorkloadSetMsiVersion), nameof(CreateVisualStudioWorkloadSet.WorkloadSetPackageFiles));
            MsiUtils.ValidateProductVersion(MsiVersion);
            SdkFeatureBand = GetSdkFeatureBandVersion(GetSdkVersion(Id));
            SwixPackageGroupId = $"{DefaultValues.PackageGroupPrefix}.NET.Workloads-{SdkFeatureBand.ToString(3)}";
            WorkloadSetVersion = GetWorkloadSetVersion(SdkFeatureBand, PackageVersion);
        }

        /// <summary>
        /// Extracts the SDK version from the package ID.
        /// </summary>
        /// <param name="packageId">The package ID from which to extract the SDK version.</param>
        /// <returns>SDK version part of the package ID.</returns>
        /// <exception cref="FormatException" />
        internal static string GetSdkVersion(string packageId) =>
            GetSdkVersion(packageId, SdkFeatureBandSeparator);

        /// <summary>
        /// Computes the workload set version for the package.
        /// </summary>
        /// <param name="sdkFeatureBand">The featureband contained in the package ID.</param>
        /// <param name="packageVersion">The NuGet version of package.</param>
        /// <returns>The 3 or 4-part workload set version.</returns>
        internal static string GetWorkloadSetVersion(ReleaseVersion sdkFeatureBand, NuGetVersion packageVersion)
        {
            // The SDK featureband and patch level is stored as the minor version of the workload set package. The SDK
            // minor version comes from the SDK featureband version in the package ID since it's not stored in the package
            // version.
            string v = $"{sdkFeatureBand.Major}.{sdkFeatureBand.Minor}.{packageVersion.Minor}";

            // Only non-zero patch levels are included to construct 4-part versions.
            v += packageVersion.Patch > 0 ? $".{packageVersion.Patch}" : string.Empty;

            // Include the package pre-release label if present.
            v += string.IsNullOrWhiteSpace(packageVersion.Release) ? string.Empty : $"-{packageVersion.Release}";

            return v;
        }

        public override void Extract(IEnumerable<string> exclusionPatterns)
        {
            base.Extract(exclusionPatterns);

            // We only care about *.workloadset.json files. Directory harvesting doesn't support
            // globbing, so we'll delete anything we don't need and log warnings because it could indicate
            // a problem with the generated package.
            string dataDirectory = Path.Combine(DestinationDirectory, "data");

            if (!Directory.Exists(dataDirectory))
            {
                throw new Exception(string.Format(Strings.InvalidWorkloadSetPackageMissingDataDir, Id));
            }

            // Delete any sub-folders inside the data directory.
            foreach (var dir in Directory.EnumerateDirectories(dataDirectory))
            {
                Directory.Delete(dir, recursive: true);
            }

            bool hasWorkloadSetFiles = false;

            // Remove anything that is not a workload set file.
            foreach (var file in Directory.EnumerateFiles(dataDirectory))
            {
                if (!Path.GetFileName(file).EndsWith("workloadset.json"))
                {
                    Log?.LogWarning(string.Format(Strings.WarnNonWorkloadSetFileFound, Path.GetFileName(file)));
                    File.Delete(file);
                    continue;
                }
                
                hasWorkloadSetFiles = true;
            }

            // Fail if there are no workload set files present
            if (!hasWorkloadSetFiles)
            {
                throw new Exception(string.Format(Strings.InvalidWorkloadSetPackageNoWorkloadSet, Id));
            }
        }
    }
}

#nullable disable
