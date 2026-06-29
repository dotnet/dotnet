// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Microsoft.DotNet.Build.Tasks.Workloads.Msi
{
    /// <summary>
    /// Defines well-known directory identifiers for workload MSIs.
    /// </summary>
    internal class MsiDirectories
    {
        /// <summary>
        /// Directory reference for the .NET installation directory under Program Files.
        /// </summary>
        public static readonly string DOTNETHOME = nameof(DOTNETHOME);

        /// <summary>
        /// Directory reference for the default workload pack installation directory.
        /// </summary>
        public static readonly string InstallDir = nameof(InstallDir);

        /// <summary>
        /// The directory reference to use when harvesting the package contents for upgradable manifest installers
        /// </summary>
        public static readonly string ManifestIdDir = nameof(ManifestIdDir);

        /// <summary>
        /// Directory reference to use when harvesting the package contents for SxS manifest installers.
        /// </summary>
        public static readonly string ManifestVersionDir = nameof(ManifestVersionDir);

        /// <summary>
        /// Directory reference for the workload pack package.
        /// </summary>
        public static readonly string PackageDir = nameof(PackageDir);

        /// <summary>
        /// Directory reference for the SDK manifests directory.
        /// </summary>
        public static readonly string SdkManifestDir = nameof(SdkManifestDir);

        /// <summary>
        /// Directory reference for the SDK feature band version directory.
        /// </summary>
        public static readonly string SdkFeatureBandVersionDir = nameof(SdkFeatureBandVersionDir);

        /// <summary>
        /// Directory reference for the workload pack package version.
        /// </summary>
        public static readonly string VersionDir = nameof(VersionDir);

        /// <summary>
        /// Directory reference for the root workload set directory.
        /// </summary>
        public static readonly string WorkloadSetsDir = nameof(WorkloadSetsDir);

        /// <summary>
        /// Directory reference to use when harvesting package contents for workload sets.
        /// </summary>
        public static readonly string WorkloadSetVersionDir = nameof(WorkloadSetVersionDir);
    }
}
