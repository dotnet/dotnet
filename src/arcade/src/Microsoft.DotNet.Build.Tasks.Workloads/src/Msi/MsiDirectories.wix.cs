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
        /// The directory reference to use when harvesting the package contents for upgradable manifest installers
        /// </summary>
        public static readonly string ManifestIdDirectory = "ManifestIdDir";

        /// <summary>
        /// Directory reference to use when harvesting the package contents for SxS manifest installers.
        /// </summary>
        public static readonly string ManifestVersionDirectory = "ManifestVersionDir";

        /// <summary>
        /// Directory reference to use when harvesting package contents for workload sets.
        /// </summary>
        public static readonly string WorkloadSetVersionDirectory = "WorkloadSetVersionDir";
    }
}
