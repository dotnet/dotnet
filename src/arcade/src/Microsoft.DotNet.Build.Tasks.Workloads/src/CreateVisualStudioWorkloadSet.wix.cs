// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Microsoft.DotNet.Build.Tasks.Workloads.Msi;
using Microsoft.DotNet.Build.Tasks.Workloads.Swix;
using Parallel = System.Threading.Tasks.Parallel;

namespace Microsoft.DotNet.Build.Tasks.Workloads
{
    /// <summary>
    /// Build task for generating workload set MSI installers, including projects for
    /// building the NuGet package wrappers and SWIX projects for inserting into Visual Studio.
    /// </summary>
    public class CreateVisualStudioWorkloadSet : VisualStudioWorkloadTaskBase
    {
        /// <summary>
        /// The version to assign to workload set installers.
        /// </summary>
        public string WorkloadSetMsiVersion
        {
            get;
            set;
        }

        /// <summary>
        /// Set of NuGet packages containing workload sets.
        /// </summary>
        public ITaskItem[] WorkloadSetPackageFiles
        {
            get;
            set;
        }

        protected override bool ExecuteCore()
        {
            Version msiVersion = string.IsNullOrWhiteSpace(WorkloadSetMsiVersion) ? null : new Version(WorkloadSetMsiVersion);
            List<WorkloadSetMsi> workloadSetMsisToBuild = new();
            List<ITaskItem> msiItems = new();
            List<ITaskItem> swixProjectItems = new();
            HashSet<SwixPackageGroup> swixPackageGroups = new();

            foreach (ITaskItem workloadSetPackageFile in WorkloadSetPackageFiles)
            {
                WorkloadSetPackage workloadSetPackage = new(workloadSetPackageFile, PackageRootDirectory,
                    msiVersion, shortNames: null, Log);

                foreach (string platform in SupportedPlatforms)
                {
                    var workloadSetMsi = new WorkloadSetMsi(workloadSetPackage, platform, BuildEngine,
                        WixToolsetPath, BaseIntermediateOutputPath);
                    workloadSetMsisToBuild.Add(workloadSetMsi);
                }

                SwixPackageGroup packageGroup = new(workloadSetPackage);

                if (!swixPackageGroups.Add(packageGroup))
                {
                    Log.LogError(Strings.ManifestPackageGroupExists, workloadSetPackage.Id, packageGroup.Name);
                }

                Log.LogMessage(MessageImportance.High, "Extracting workload set");
                workloadSetPackage.Extract();

                _ = Parallel.ForEach(workloadSetMsisToBuild, msi =>
                {
                    ITaskItem msiOutputItem = msi.Build(MsiOutputPath, IceSuppressions);

                    // Generate a .csproj to package the MSI and its manifest for CLI installs.
                    MsiPayloadPackageProject csproj = new(msi.Metadata, msiOutputItem, BaseIntermediateOutputPath, BaseOutputPath, msi.NuGetPackageFiles);
                    msiOutputItem.SetMetadata(Metadata.PackageProject, csproj.Create());

                    lock (msiItems)
                    {
                        msiItems.Add(msiOutputItem);
                    }

                    // Generate a .swixproj for packaging the MSI in Visual Studio. We'll default to using machineArch always. Workload sets
                    // are being introduced in .NET 8 and the corresponding versions of VS all support the machineArch property.
                    MsiSwixProject swixProject = new(msiOutputItem, BaseIntermediateOutputPath, BaseOutputPath, workloadSetPackage.SdkFeatureBand, chip: null, machineArch: msiOutputItem.GetMetadata(Metadata.Platform));

                    string swixProj = swixProject.Create();

                    ITaskItem swixProjectItem = new TaskItem(swixProj);
                    swixProjectItem.SetMetadata(Metadata.SdkFeatureBand, $"{workloadSetPackage.SdkFeatureBand}");
                    swixProjectItem.SetMetadata(Metadata.PackageType, DefaultValues.PackageTypeMsiWorkloadSet);
                    swixProjectItem.SetMetadata(Metadata.IsPreview, "false");

                    lock (swixProjectItems)
                    {
                        swixProjectItems.Add(swixProjectItem);
                    }
                });

                foreach (var swixPackageGroup in swixPackageGroups)
                {
                    swixProjectItems.Add(PackageGroupSwixProject.CreateProjectItem(swixPackageGroup, BaseIntermediateOutputPath, BaseOutputPath,
                        DefaultValues.PackageTypeWorkloadSetPackageGroup));
                }
            }

            Msis = msiItems.ToArray();
            SwixProjects = swixProjectItems.ToArray();

            return true;
        }
    }
}
