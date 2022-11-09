// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable enable

using System;
using System.IO;
using System.Linq;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Microsoft.Deployment.DotNet.Releases;
using Microsoft.NET.Sdk.WorkloadManifestReader;

namespace Microsoft.DotNet.Build.Tasks.Workloads
{
    /// <summary>
    /// Represents a NuGet package containing a workload manifest.
    /// </summary>
    internal class WorkloadManifestPackage : WorkloadPackageBase
    {
        /// <inheritdoc />
        public override PackageExtractionMethod ExtractionMethod => PackageExtractionMethod.Unzip;

        /// <summary>
        /// Special separator value used in workload manifest package IDs.
        /// </summary>
        private const string ManifestSeparator = ".Manifest-";

        /// <summary>
        /// The filename and extension of the workload manifest file.
        /// </summary>
        private const string ManifestFileName = "WorkloadManifest.json";

        /// <summary>
        /// The workload manfiest ID.
        /// </summary>
        public string ManifestId
        {
            get;
        }

        /// <inheritdoc />
        public override Version MsiVersion
        {
            get;
        }

        /// <summary>
        /// The SDK feature band version associated with this manifest package.
        /// </summary>
        public ReleaseVersion SdkFeatureBand
        {
            get;
        }

        /// <summary>
        /// Returns <see langword="true" /> if the Visual Studio version targeted by the feature band supports the machineArch property;
        /// <see langword="false" /> otherwise.
        /// </summary>
        public bool SupportsMachineArch
        {
            get;
        }

        /// <summary>
        /// Creates a new instance of a <see cref="WorkloadManifestPackage"/>.
        /// </summary>
        /// <param name="package">A task item for the workload manifest NuGet package.</param>
        /// <param name="destinationBaseDirectory">The root directory where packages will be extracted.</param>
        /// <param name="msiVersion">The general MSI version to use when the package does not contain metadata for the installer version.</param>
        /// <param name="shortNames">A set of items used to shorten the names and identifiers of setup packages.</param>
        /// <param name="log">A <see cref="TaskLoggingHelper"/> class containing task logging methods.</param>
        /// <exception cref="Exception" />
        public WorkloadManifestPackage(ITaskItem package, string destinationBaseDirectory, Version msiVersion,
            ITaskItem[]? shortNames = null, TaskLoggingHelper? log = null) :
            base(package.ItemSpec, destinationBaseDirectory, shortNames, log)
        {
            if (!string.IsNullOrWhiteSpace(package.GetMetadata(Metadata.MsiVersion)))
            {
                // We prefer version information on the manifest package item.
                MsiVersion = new(package.GetMetadata(Metadata.MsiVersion));
            }
            else if (msiVersion != null)
            {
                // Fall back to the version provided by the task parameter, e.g. if all manifests follow the same versioing rules.
                MsiVersion = msiVersion;
            }
            else
            {
                // While we could use the major.minor.patch part of the package, manifests are upgradable, so we want
                // the user to be aware of this and explicitly tell us the value.
                throw new Exception(string.Format(Strings.NoManifestInstallerVersion, nameof(CreateVisualStudioWorkload),
                    nameof(CreateVisualStudioWorkload.ManifestMsiVersion), nameof(CreateVisualStudioWorkload.WorkloadManifestPackageFiles), Metadata.MsiVersion));
            }

            SdkFeatureBand = GetSdkFeatureBandVersion(GetSdkVersion(Id));
            ManifestId = GetManifestId(Id);
            SwixPackageId = $"{Id.Replace(shortNames)}";
            SupportsMachineArch = bool.TryParse(package.GetMetadata(Metadata.SupportsMachineArch), out bool supportsMachineArch) ? supportsMachineArch : false; 
         }

        /// <summary>
        /// Gets the path of the workload manifest file. 
        /// </summary>
        /// <returns>The path of the workload manifest file</returns>
        /// <exception cref="FileNotFoundException" />
        public string GetManifestFile()
        {
            if (!HasBeenExtracted)
            {
                Extract();
            }

            string primaryManifest = Path.Combine(DestinationDirectory, "data", ManifestFileName);
            string secondaryManifest = Path.Combine(DestinationDirectory, ManifestFileName);

            // Check the data directory first, otherwise fall back to the older format where manifests
            // were in the root of the package.
            return File.Exists(primaryManifest) ? primaryManifest :
                File.Exists(secondaryManifest) ? secondaryManifest :
                throw new FileNotFoundException(string.Format(Strings.WorkloadManifestNotFound, primaryManifest, secondaryManifest));
        }

        /// <summary>
        /// Creates a <see cref="WorkloadManifest"/> instance using the parsed contents of the workload manifest file.
        /// </summary>
        /// <returns>The parsed workload manifest.</returns>
        public WorkloadManifest GetManifest()
        {
            string workloadManifestFile = GetManifestFile();

            return WorkloadManifestReader.ReadWorkloadManifest(Path.GetFileNameWithoutExtension(workloadManifestFile), File.OpenRead(workloadManifestFile), workloadManifestFile);
        }

        /// <summary>
        /// Converts a string containing an SDK version to a semantic version that normalizes the patch level and 
        /// optionally includes the first two prerelease labels. For example, if the specified version is 6.0.105, then
        /// 6.0.100 would be returned. If the version is 6.0.301-preview.2.1234, the result would be 6.0.300-preview.1.
        /// </summary>
        /// <param name="sdkVersion">A string containing an SDK version.</param>
        /// <returns>An SDK feature band version.</returns>
        internal static ReleaseVersion GetSdkFeatureBandVersion(string sdkVersion)
        {
            ReleaseVersion version = new(sdkVersion);

            // Ignore CI and dev builds.
            if (string.IsNullOrEmpty(version.Prerelease) || version.Prerelease.Split('.').Any(s => string.Equals("ci", s) || string.Equals("dev", s)))
            {
                return new ReleaseVersion(version.Major, version.Minor, version.SdkFeatureBand);
            }

            string[] preleaseParts = version.Prerelease.Split('.');

            // Only the first two prerelease identifiers are used to support side-by-side previews.
            string prerelease = (preleaseParts.Length > 1) ?
                $"{preleaseParts[0]}.{preleaseParts[1]}" :
                preleaseParts[0];

            return new ReleaseVersion(version.Major, version.Minor, version.SdkFeatureBand, prerelease);
        }

        /// <summary>
        /// Extracts the SDK version from the package ID.
        /// </summary>
        /// <param name="packageId">The package ID from which to extract the SDK version.</param>
        /// <returns>SDK version part of the package ID.</returns>
        /// <exception cref="FormatException" />
        internal static string GetSdkVersion(string packageId) =>
            !string.IsNullOrWhiteSpace(packageId) && packageId.IndexOf(ManifestSeparator) > -1 ?
                packageId.Substring(packageId.IndexOf(ManifestSeparator) + ManifestSeparator.Length) :
                throw new FormatException(string.Format(Strings.CannotExtractSdkVersionFromPackageId, packageId));

        /// <summary>
        /// Extracts the manifest ID from the package ID.
        /// </summary>
        /// <param name="packageId">The package ID from which to extract the manifest ID.</param>
        /// <returns>The manifest ID.</returns>
        /// <exception cref="FormatException" />
        internal static string GetManifestId(string packageId) =>
            !string.IsNullOrWhiteSpace(packageId) && packageId.IndexOf(ManifestSeparator) > -1 ?
                packageId.Substring(0, packageId.IndexOf(ManifestSeparator)) :
                throw new FormatException(string.Format(Strings.CannotExtractManifestIdFromPackageId, packageId));
    }
}

#nullable disable
