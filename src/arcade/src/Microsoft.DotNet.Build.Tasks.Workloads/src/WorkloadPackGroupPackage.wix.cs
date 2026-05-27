// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using Microsoft.DotNet.Build.Tasks.Workloads.Msi;

namespace Microsoft.DotNet.Build.Tasks.Workloads
{
    internal class WorkloadPackGroupPackage : WorkloadPackageMetadata
    {
        /// <summary>
        /// A list of all the workload pack packages associated with the workload pack group.
        /// </summary>
        public List<WorkloadPackPackage> Packs { get; } = new();

        public Dictionary<string, List<WorkloadManifestPackage>> ManifestsPerPlatform { get; } = new();

        public string WorkloadName { get; }

        public WorkloadPackGroupPackage(string workloadName, WorkloadManifestPackage manifestPackage) : base(GetPackGroupId(workloadName), manifestPackage.PackageVersion, manifestPackage.MsiVersion, manifestPackage.Authors,
            manifestPackage.Copyright,
            description: "Workload packs for " + workloadName,
            title: "Workload packs for " + workloadName,
            manifestPackage.LicenseUrl,
            manifestPackage.ProjectUrl,
            swixPackageId: GetPackGroupId(workloadName))
        {
            WorkloadName = workloadName;
        }

        /// <summary>
        /// Generates a safe package identifier for the workload pack group based on the workload name.
        /// </summary>
        /// <param name="workloadName">The name of the workload.</param>
        /// <returns>A safe package identifier for the workload pack group.</returns>
        public static string GetPackGroupId(string workloadName) =>
            Utils.ToSafeId(workloadName) + ".WorkloadPacks";
    }
}
