// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.DotNet.Build.Tasks.Workloads.Msi;

namespace Microsoft.DotNet.Build.Tasks.Workloads
{
    internal class WorkloadPackGroupPackage
    {
        public List<WorkloadPackPackage> Packs { get; set; } = new();

        public Dictionary<string, List<WorkloadManifestPackage>> ManifestsPerPlatform { get; } = new();

        public string WorkloadName { get; }

        public string Id { get; }

        public WorkloadPackGroupPackage(string workloadName)
        {
            WorkloadName = workloadName;
            Id = GetPackGroupID(workloadName);
        }

        public static string GetPackGroupID(string workloadName)
        {
            return Utils.ToSafeId(workloadName) + ".WorkloadPacks";
        }

        public MsiMetadata GetMsiMetadata()
        {
            //  Take latest manifest from arbitrary platform to use for metadata
            var manifestPackage = ManifestsPerPlatform.First().Value.OrderBy(m => m.Version).Last();

            return new MsiMetadata(Id, manifestPackage.PackageVersion, manifestPackage.MsiVersion, manifestPackage.Authors,
                manifestPackage.Copyright,
                description: "Workload packs for " + WorkloadName,
                title: "Workload packs for " + WorkloadName,
                manifestPackage.LicenseUrl,
                manifestPackage.ProjectUrl,
                swixPackageId: Id);
        }
    }
}
