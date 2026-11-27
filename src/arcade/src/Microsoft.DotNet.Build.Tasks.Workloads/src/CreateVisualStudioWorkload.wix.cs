// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Microsoft.Deployment.DotNet.Releases;
using Microsoft.DotNet.Build.Tasks.Workloads.Msi;
using Microsoft.DotNet.Build.Tasks.Workloads.Swix;
using Microsoft.NET.Sdk.WorkloadManifestReader;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Versioning;
using static Microsoft.DotNet.Build.Tasks.Workloads.Msi.WorkloadManifestMsi;
using Parallel = System.Threading.Tasks.Parallel;

namespace Microsoft.DotNet.Build.Tasks.Workloads;

/// <summary>
/// An MSBuild task used to create workload artifacts including MSIs and SWIX projects for Visual Studio Installer.
/// </summary>
public class CreateVisualStudioWorkload : VisualStudioWorkloadTaskBase
{
    /// <summary>
    /// Used to track which feature bands support the machineArch property.
    /// </summary>
    private Dictionary<ReleaseVersion, bool> _supportsMachineArch = new();

    /// <summary>
    /// A set of items that provide metadata associated with the Visual Studio components derived from
    /// workload manifests. 
    /// </summary>
    public ITaskItem[] ComponentResources
    {
        get;
        set;
    }

    /// <summary>
    /// When <see langword="true" />, manifest installers will generate a non-stable UpgradeCode
    /// and a unique dependency provider key to ensure side-by-side installs.
    /// </summary>
    public bool EnableSideBySideManifests
    {
        get;
        set;
    } = false;

    /// <summary>
    /// Determines whether the component (and related packs) should be flagged as
    /// out-of-support in Visual Studio.
    /// </summary>
    public bool IsOutOfSupportInVisualStudio
    {
        get;
        set;
    }

    /// <summary>
    /// The version to assign to workload manifest installers.
    /// </summary>
    public string ManifestMsiVersion
    {
        get;
        set;
    }

    /// <summary>
    /// A set of items used to shorten the names and identifiers of setup packages.
    /// </summary>
    public ITaskItem[] ShortNames
    {
        get;
        set;
    }

    /// <summary>
    /// A set of packages containing workload manifests.
    /// </summary>
    [Required]
    public ITaskItem[] WorkloadManifestPackageFiles
    {
        get;
        set;
    }

    /// <summary>
    /// When <see langword="true"/>, multiple packs are combined into a single installer.
    /// </summary>
    public bool CreateWorkloadPackGroups
    {
        get;
        set;
    }

    /// <summary>
    /// The directory to use for locating workload pack packages.
    /// </summary>
    [Required]
    public string PackageSource
    {
        get;
        set;
    }

    /// <summary>
    /// If true, will skip creating MSIs for workload packs if they are part of a pack group
    /// </summary>
    public bool SkipRedundantMsiCreation
    {
        get;
        set;
    }

    /// <summary>
    /// If true, workload pack groups are built sequentially.
    /// </summary>
    public bool DisableParallelPackageGroupProcessing
    {
        get;
        set;
    }

    /// <summary>
    /// Allow VS workload generation to proceed if any nupkgs declared in the manifest are not found on disk.
    /// </summary>
    public bool AllowMissingPacks
    {
        get;
        set;
    } = false;

    /// <summary>
    /// A set of previously built workload pack MSI payload packages, e.g. <c>Microsoft.NET.Runtime.Emscripten.Node.win-x64.Msi.x64</c>.
    /// When a package matching a workload pack's ID, version and platform is found, the MSI it contains is used for the
    /// SWIX project instead of building a new MSI. No MSI item, payload package project or wixpack is produced for those packs.
    /// This is only supported when <see cref="CreateWorkloadPackGroups"/> is false.
    /// </summary>
    public ITaskItem[] PrebuiltPackMsiPackages
    {
        get;
        set;
    }

    /// <summary>
    /// Maps "{PackageId}|{NormalizedVersion}" of the prebuilt MSI payload packages to their paths.
    /// </summary>
    private readonly Dictionary<string, string> _prebuiltPackMsiPackages = new(StringComparer.OrdinalIgnoreCase);

    protected override bool ExecuteCore()
    {
        if (PrebuiltPackMsiPackages?.Length > 0)
        {
            if (CreateWorkloadPackGroups)
            {
                Log.LogError($"{nameof(PrebuiltPackMsiPackages)} is not supported when {nameof(CreateWorkloadPackGroups)} is true.");
                return false;
            }

            foreach (ITaskItem prebuiltPackage in PrebuiltPackMsiPackages)
            {
                string packagePath = prebuiltPackage.GetMetadata(Metadata.FullPath);
                using PackageArchiveReader reader = new(packagePath);
                PackageIdentity identity = reader.GetIdentity();
                _prebuiltPackMsiPackages[GetPrebuiltPackMsiPackageKey(identity.Id, identity.Version)] = packagePath;
            }
        }

        // TODO: trim out duplicate manifests.
        List<WorkloadManifestPackage> manifestPackages = new();
        List<WorkloadManifestMsi> manifestMsisToBuild = new();
        HashSet<SwixComponent> swixComponents = new();
        HashSet<SwixPackageGroup> swixPackageGroups = new();
        Dictionary<string, BuildData> buildData = new();
        Dictionary<string, WorkloadPackGroupPackage> packGroupPackages = new();

        // First construct sets of everything that needs to be built. This includes
        // all the packages (manifests and workload packs) that need to be extracted along
        // with the different installer types. 
        foreach (ITaskItem workloadManifestPackageFile in WorkloadManifestPackageFiles)
        {
            // 1. Process the manifest package and create a set of installers.
            WorkloadManifestPackage manifestPackage = new(workloadManifestPackageFile, PackageRootDirectory,
                string.IsNullOrWhiteSpace(ManifestMsiVersion) ? null : new Version(ManifestMsiVersion), ShortNames, Log, EnableSideBySideManifests);
            manifestPackages.Add(manifestPackage);

            if (!_supportsMachineArch.ContainsKey(manifestPackage.SdkFeatureBand))
            {
                // Log the original setting and manifest that created the machineArch setting for the featureband.
                Log.LogMessage(MessageImportance.Low, $"Setting {nameof(_supportsMachineArch)} to {manifestPackage.SupportsMachineArch} for {Path.GetFileName(manifestPackage.PackageFileName)}");
                _supportsMachineArch[manifestPackage.SdkFeatureBand] = manifestPackage.SupportsMachineArch;
            }
            else if (_supportsMachineArch[manifestPackage.SdkFeatureBand] != manifestPackage.SupportsMachineArch)
            {
                // If multiple manifest packages for the same feature band have conflicting machineArch values
                // then we'll treat it as an warning. It will likely fail the build.
                Log.LogWarning($"{_supportsMachineArch} was previously set to {_supportsMachineArch[manifestPackage.SdkFeatureBand]}");
            }

            Dictionary<string, WorkloadManifestMsi> manifestMsisByPlatform = new();
            string[] supportedPlatforms = WorkloadPackageBase.GetSupportedPlatforms(manifestPackage.SdkFeatureBand);
            foreach (string platform in supportedPlatforms)
            {
                WorkloadManifestMsi manifestMsi = new(manifestPackage, platform, BuildEngine, WixToolsetConfig, BaseIntermediateOutputPath, 
                    EnableSideBySideManifests, CreateWixPacks);
                manifestMsisToBuild.Add(manifestMsi);
                manifestMsisByPlatform[platform] = manifestMsi;
            }

            // If we're supporting SxS manifests, generate a package group to wrap the manifest VS packages
            // so we don't deal with unstable package IDs during VS insertions.
            if (EnableSideBySideManifests)
            {
                SwixPackageGroup packageGroup = new(manifestPackage);

                if (!swixPackageGroups.Add(packageGroup))
                {
                    Log.LogError(Strings.ManifestPackageGroupExists, manifestPackage.Id, packageGroup.Name);
                }
            }

            // 2. Process the manifest itself to determine the set of packs involved and create
            //    installers for all the packs. Duplicate packs will be ignored, example, when
            //    workloads in two manifests targeting different feature bands contain the
            //    same pack dependencies. Building multiple copies of MSIs will cause
            //    problems (ref counting, repair operations, etc.) and also increases the build time.
            //
            //    When building multiple manifests, it's possible for feature bands to have
            //    different sets of packs. For example, the same manifest for different feature bands
            //    can add additional platforms that requires generating additional SWIX projects, while
            //    ensuring that the pack and MSI is only generated once.
            WorkloadManifest manifest = manifestPackage.GetManifest();

            List<WorkloadPackGroupJson> packGroupJsonList = new();

            foreach (WorkloadDefinition workload in manifest.Workloads.Values)
            {
                if ((workload is WorkloadDefinition wd) && (wd.Platforms == null || wd.Platforms.Any(platform => platform.StartsWith("win"))) && (wd.Packs != null))
                {
                    Dictionary<string, List<WorkloadPackPackage>> packsInWorkloadByPlatform = new();

                    string packGroupId = null;
                    WorkloadPackGroupJson packGroupJson = null;
                    if (CreateWorkloadPackGroups)
                    {
                        packGroupId = WorkloadPackGroupPackage.GetPackGroupId(workload.Id);
                        packGroupJson = new WorkloadPackGroupJson()
                        {
                            GroupPackageId = packGroupId,
                            GroupPackageVersion = manifestPackage.PackageVersion.ToString()
                        };
                    }

                    foreach (WorkloadPackId packId in wd.Packs)
                    {
                        WorkloadPack pack = manifest.Packs[packId];

                        if (CreateWorkloadPackGroups)
                        {
                            packGroupJson.Packs.Add(new WorkloadPackJson()
                            {
                                PackId = pack.Id,
                                PackVersion = pack.Version
                            });
                        }

                        foreach ((string sourcePackage, string[] platforms) in WorkloadPackPackage.GetSourcePackages(PackageSource, pack, supportedPlatforms))
                        {
                            if (!File.Exists(sourcePackage))
                            {
                                if (AllowMissingPacks)
                                {
                                    Log.LogMessage($"Pack {sourcePackage} - {string.Join(",", platforms)} could not be found, it will be skipped.");
                                    continue;
                                }
                                else
                                {
                                    throw new FileNotFoundException(message: "NuGet package not found", fileName: sourcePackage);
                                }
                            }

                            // Create new build data and add the pack if we haven't seen it previously.
                            if (!buildData.ContainsKey(sourcePackage))
                            {
                                buildData[sourcePackage] = new BuildData(WorkloadPackPackage.Create(pack, sourcePackage, platforms, PackageRootDirectory,
                                    ShortNames, Log));
                            }

                            foreach (string platform in platforms)
                            {
                                // If we haven't seen the platform, create a new entry, then add
                                // the current feature band. This allows us to track platform specific packs
                                // across multiple feature bands and manifests.
                                if (!buildData[sourcePackage].FeatureBands.ContainsKey(platform))
                                {
                                    buildData[sourcePackage].FeatureBands[platform] = new();
                                }

                                _ = buildData[sourcePackage].FeatureBands[platform].Add(manifestPackage.SdkFeatureBand);

                                if (!packsInWorkloadByPlatform.ContainsKey(platform))
                                {
                                    packsInWorkloadByPlatform[platform] = new();
                                }
                                packsInWorkloadByPlatform[platform].Add(buildData[sourcePackage].Package);
                            }

                            //  TODO: Find a better way to track this
                            if (SkipRedundantMsiCreation)
                            {
                                buildData.Remove(sourcePackage);
                            }
                        }
                    }

                    if (CreateWorkloadPackGroups)
                    {
                        if (packsInWorkloadByPlatform.Count == 0)
                        {
                            packGroupId = null;
                        }
                        else
                        {
                            packGroupJsonList.Add(packGroupJson);
                        }

                        //  TODO: Support passing in data to skip creating pack groups for certain packs (possibly EMSDK, because it's large)
                        foreach (var kvp in packsInWorkloadByPlatform)
                        {
                            string platform = kvp.Key;

                            //  The key is the paths to the packages included in the pack group, sorted in alphabetical order
                            string uniquePackGroupKey = string.Join("\r\n", kvp.Value.Select(p => p.PackagePath).OrderBy(p => p));
                            if (!packGroupPackages.TryGetValue(uniquePackGroupKey, out var groupPackage))
                            {
                                groupPackage = new WorkloadPackGroupPackage(workload.Id, manifestPackage);
                                groupPackage.Packs.AddRange(kvp.Value);
                                packGroupPackages[uniquePackGroupKey] = groupPackage;
                            }

                            if (!groupPackage.ManifestsPerPlatform.ContainsKey(platform))
                            {
                                groupPackage.ManifestsPerPlatform[platform] = new();
                            }
                            groupPackage.ManifestsPerPlatform[platform].Add(manifestPackage);
                        }                            
                    }

                    // Finally, add a component for the workload in Visual Studio.
                    SwixComponent component = SwixComponent.Create(manifestPackage.SdkFeatureBand, workload, manifest, packGroupId,
                        ComponentResources, ShortNames);
                    // Create an additional component for shipping previews
                    SwixComponent previewComponent = SwixComponent.Create(manifestPackage.SdkFeatureBand, workload, manifest, packGroupId,
                        ComponentResources, ShortNames, "pre");

                    // Check for duplicates, e.g. manifests that were copied without changing workload definition IDs and
                    // provide a more usable error message so users can track down the duplication.
                    if (!swixComponents.Add(component))
                    {
                        Log.LogError(Strings.WorkloadComponentExists, workload.Id, component.Name);
                    }

                    if (!swixComponents.Add(previewComponent))
                    {
                        Log.LogError(Strings.WorkloadComponentExists, workload.Id, previewComponent.Name);
                    }
                }
            }

            if (CreateWorkloadPackGroups)
            {
                foreach (var manifestMsi in manifestMsisByPlatform.Values)
                {
                    manifestMsi.WorkloadPackGroups.AddRange(packGroupJsonList);
                }
            }
        }

        // Deduplicate packages and extract them. Building and extracting in parallel can cause issues as the same
        // source package can be shared across multiple workloads and platforms.
        Parallel.ForEach(buildData.Values.Where(d => !IsFullyPrebuilt(d)).Select(d => d.Package).Distinct(), package =>
        {
            Log.LogMessage(MessageImportance.Low, string.Format(Strings.BuildExtractingPackage, package.PackagePath));
            package.Extract();
        });

        List<ITaskItem> msiItems = new();
        List<ITaskItem> swixProjectItems = new();

        if (!CreateWorkloadPackGroups)
        {
            _ = Parallel.ForEach(buildData.Values, data =>
            {
                if (!IsFullyPrebuilt(data))
                {
                    // Extract the contents of the workload pack package.
                    Log.LogMessage(MessageImportance.Low, string.Format(Strings.BuildExtractingPackage, data.Package.PackagePath));
                    data.Package.Extract();
                }

                // Enumerate over the platforms and build each MSI once.
                _ = Parallel.ForEach(data.FeatureBands.Keys, platform =>
                {
                    if (!TryGetPrebuiltPackMsi(data.Package, platform, out ITaskItem msiOutputItem))
                    {
                        WorkloadPackMsi msi = new(data.Package, platform, BuildEngine, WixToolsetConfig, BaseIntermediateOutputPath, CreateWixPacks);
                        msiOutputItem = msi.Build(MsiOutputPath);

                        // Generate a .csproj to package the MSI and its manifest for CLI installs.
                        MsiPayloadPackageProject csproj = new(msi.Metadata, msiOutputItem, BaseIntermediateOutputPath, BaseOutputPath, msi.NuGetPackageFiles);
                        msiOutputItem.SetMetadata(Metadata.PackageProject, csproj.Create());

                        lock (msiItems)
                        {
                            msiItems.Add(msiOutputItem);
                        }
                    }

                    foreach (ReleaseVersion sdkFeatureBand in data.FeatureBands[platform])
                    {
                        // Don't generate a SWIX package if the MSI targets arm64 and VS doesn't support machineArch
                        if (_supportsMachineArch[sdkFeatureBand] || !string.Equals(msiOutputItem.GetMetadata(Metadata.Platform), DefaultValues.arm64))
                        {
                            MsiSwixProject swixProject = _supportsMachineArch[sdkFeatureBand] ?
                                new(msiOutputItem, BaseIntermediateOutputPath, BaseOutputPath, sdkFeatureBand, chip: null, machineArch: msiOutputItem.GetMetadata(Metadata.Platform), outOfSupport: IsOutOfSupportInVisualStudio) :
                                new(msiOutputItem, BaseIntermediateOutputPath, BaseOutputPath, sdkFeatureBand, chip: msiOutputItem.GetMetadata(Metadata.Platform), outOfSupport: IsOutOfSupportInVisualStudio);
                            string swixProj = swixProject.Create();

                            ITaskItem swixProjectItem = new TaskItem(swixProj);
                            swixProjectItem.SetMetadata(Metadata.SdkFeatureBand, $"{sdkFeatureBand}");
                            swixProjectItem.SetMetadata(Metadata.PackageType, DefaultValues.PackageTypeMsiPack);
                            swixProjectItem.SetMetadata(Metadata.IsPreview, "false");
                            swixProjectItem.SetMetadata(Metadata.Platform, msiOutputItem.GetMetadata(Metadata.Platform));
                            swixProjectItem.SetMetadata(Metadata.SwixPackageId, msiOutputItem.GetMetadata(Metadata.SwixPackageId));

                            lock (swixProjectItems)
                            {
                                swixProjectItems.Add(swixProjectItem);
                            }
                        }
                    }
                });
            });
        }
        else
        {
            //  Parallel processing of pack groups was causing file access errors for heat in an earlier version of this code
            //  So we support a flag to disable the parallelization if that starts happening again
            PossiblyParallelForEach(!DisableParallelPackageGroupProcessing, packGroupPackages.Values, packGroup =>
            {
                foreach (var platform in packGroup.ManifestsPerPlatform.Keys)
                {
                    WorkloadPackGroupMsi msi = new(packGroup, platform, BuildEngine, WixToolsetConfig, BaseIntermediateOutputPath, CreateWixPacks);
                    ITaskItem msiOutputItem = msi.Build(MsiOutputPath);

                    // Generate a .csproj to package the MSI and its manifest for CLI installs.
                    MsiPayloadPackageProject csproj = new(msi.Metadata, msiOutputItem, BaseIntermediateOutputPath, BaseOutputPath, msi.NuGetPackageFiles);
                    msiOutputItem.SetMetadata(Metadata.PackageProject, csproj.Create());

                    lock (msiItems)
                    {
                        msiItems.Add(msiOutputItem);
                    }

                    PossiblyParallelForEach(!DisableParallelPackageGroupProcessing, packGroup.ManifestsPerPlatform[platform], manifestPackage =>
                    {
                        // Don't generate a SWIX package if the MSI targets arm64 and VS doesn't support machineArch
                        if (_supportsMachineArch[manifestPackage.SdkFeatureBand] || !string.Equals(msiOutputItem.GetMetadata(Metadata.Platform), DefaultValues.arm64))
                        {
                            MsiSwixProject swixProject = _supportsMachineArch[manifestPackage.SdkFeatureBand] ?
                                new(msiOutputItem, BaseIntermediateOutputPath, BaseOutputPath, manifestPackage.SdkFeatureBand, chip: null, machineArch: msiOutputItem.GetMetadata(Metadata.Platform), outOfSupport: IsOutOfSupportInVisualStudio) :
                                new(msiOutputItem, BaseIntermediateOutputPath, BaseOutputPath, manifestPackage.SdkFeatureBand, chip: msiOutputItem.GetMetadata(Metadata.Platform), outOfSupport: IsOutOfSupportInVisualStudio);
                            string swixProj = swixProject.Create();

                            ITaskItem swixProjectItem = new TaskItem(swixProj);
                            swixProjectItem.SetMetadata(Metadata.SdkFeatureBand, $"{manifestPackage.SdkFeatureBand}");
                            swixProjectItem.SetMetadata(Metadata.PackageType, DefaultValues.PackageTypeMsiPack);
                            swixProjectItem.SetMetadata(Metadata.IsPreview, "false");
                            swixProjectItem.SetMetadata(Metadata.Platform, msiOutputItem.GetMetadata(Metadata.Platform));
                            swixProjectItem.SetMetadata(Metadata.SwixPackageId, msiOutputItem.GetMetadata(Metadata.SwixPackageId));

                            lock (swixProjectItems)
                            {
                                swixProjectItems.Add(swixProjectItem);
                            }
                        }
                    });
                }
            });
        }

        // Generate MSIs for the workload manifests along with a .csproj to package the MSI and a SWIX project for
        // Visual Studio.
        _ = Parallel.ForEach(manifestMsisToBuild, msi =>
        {
            ITaskItem msiOutputItem = msi.Build(MsiOutputPath);

            // Don't generate a SWIX package if the MSI targets arm64 and VS doesn't support machineArch
            if (_supportsMachineArch[msi.Package.SdkFeatureBand] || !string.Equals(msiOutputItem.GetMetadata(Metadata.Platform), DefaultValues.arm64))
            {
                // Generate SWIX authoring for the MSI package. Do not flag manifest MSI packages for out-of-support.
                // These are typically pulled in through .NET SDK components.
                MsiSwixProject swixProject = _supportsMachineArch[msi.Package.SdkFeatureBand] ?
                    new(msiOutputItem, BaseIntermediateOutputPath, BaseOutputPath, msi.Package.SdkFeatureBand, chip: null, machineArch: msiOutputItem.GetMetadata(Metadata.Platform)) :
                    new(msiOutputItem, BaseIntermediateOutputPath, BaseOutputPath, msi.Package.SdkFeatureBand, chip: msiOutputItem.GetMetadata(Metadata.Platform));
                ITaskItem swixProjectItem = new TaskItem(swixProject.Create());
                swixProjectItem.SetMetadata(Metadata.SdkFeatureBand, $"{((WorkloadManifestPackage)msi.Package).SdkFeatureBand}");
                swixProjectItem.SetMetadata(Metadata.PackageType, DefaultValues.PackageTypeMsiManifest);
                swixProjectItem.SetMetadata(Metadata.IsPreview, "false");
                swixProjectItem.SetMetadata(Metadata.Platform, msiOutputItem.GetMetadata(Metadata.Platform));
                swixProjectItem.SetMetadata(Metadata.SwixPackageId, msiOutputItem.GetMetadata(Metadata.SwixPackageId));

                lock (swixProjectItems)
                {
                    swixProjectItems.Add(swixProjectItem);
                }
            }

            // Generate a .csproj to package the MSI and its manifest for CLI installs.
            MsiPayloadPackageProject csproj = new(msi.Metadata, msiOutputItem, BaseIntermediateOutputPath, BaseOutputPath, msi.NuGetPackageFiles);
            msiOutputItem.SetMetadata(Metadata.PackageProject, csproj.Create());

            lock (msiItems)
            {
                msiItems.Add(msiOutputItem);
            }
        });

        // Generate SWIX projects for the Visual Studio components. These are driven by the manifests, so
        // they need to be ordered based on feature bands to avoid pulling in unnecessary packs into the drop
        // artifacts.                
        _ = Parallel.ForEach(swixComponents, swixComponent =>
        {
            ComponentSwixProject swixComponentProject = new(swixComponent, BaseIntermediateOutputPath, BaseOutputPath, IsOutOfSupportInVisualStudio);
            ITaskItem swixProjectItem = new TaskItem(swixComponentProject.Create());
            swixProjectItem.SetMetadata(Metadata.SdkFeatureBand, $"{swixComponent.SdkFeatureBand}");
            swixProjectItem.SetMetadata(Metadata.PackageType, DefaultValues.PackageTypeComponent);
            swixProjectItem.SetMetadata(Metadata.IsPreview, swixComponent.Name.EndsWith(".pre").ToString().ToLowerInvariant());

            lock (swixProjectItems)
            {
                swixProjectItems.Add(swixProjectItem);
            }
        });

        if (EnableSideBySideManifests)
        {
            // Generate SWIX projects for the Visual Studio package groups.
            _ = Parallel.ForEach(swixPackageGroups, swixPackageGroup =>
            {
                lock (swixProjectItems)
                {
                    swixProjectItems.Add(PackageGroupSwixProject.CreateProjectItem(swixPackageGroup, BaseIntermediateOutputPath, BaseOutputPath,
                        DefaultValues.PackageTypeManifestPackageGroup));
                }
            });
        }

        Msis = msiItems.ToArray();
        SwixProjects = swixProjectItems.ToArray();

        return true;
    }

    private static string GetPrebuiltPackMsiPackageKey(string packageId, NuGetVersion version) =>
        $"{packageId}|{version.ToNormalizedString()}";

    private static string GetPrebuiltPackMsiPackageId(WorkloadPackageBase package, string platform) =>
        $"{package.Id}.Msi.{platform}";

    private bool IsFullyPrebuilt(BuildData data) =>
        _prebuiltPackMsiPackages.Count > 0 &&
        data.FeatureBands.Keys.All(platform => _prebuiltPackMsiPackages.ContainsKey(
            GetPrebuiltPackMsiPackageKey(GetPrebuiltPackMsiPackageId(data.Package, platform), data.Package.PackageVersion)));

    /// <summary>
    /// Extracts the MSI from a prebuilt payload package matching the workload pack and platform and creates
    /// an item with the same metadata as an MSI built by <see cref="WorkloadPackMsi"/>.
    /// </summary>
    private bool TryGetPrebuiltPackMsi(WorkloadPackPackage package, string platform, out ITaskItem msiItem)
    {
        msiItem = null;
        string payloadPackageId = GetPrebuiltPackMsiPackageId(package, platform);

        if (!_prebuiltPackMsiPackages.TryGetValue(GetPrebuiltPackMsiPackageKey(payloadPackageId, package.PackageVersion), out string packagePath))
        {
            return false;
        }

        using PackageArchiveReader reader = new(packagePath);
        string[] msiFiles = reader.GetFiles("data").Where(f => f.EndsWith(".msi", StringComparison.OrdinalIgnoreCase)).ToArray();

        if (msiFiles.Length != 1)
        {
            throw new Exception($"Expected exactly one MSI in the data folder of '{packagePath}', but found {msiFiles.Length}.");
        }

        string msiPath = Path.Combine(MsiOutputPath, "prebuilt", payloadPackageId, $"{package.PackageVersion}", Path.GetFileName(msiFiles[0]));
        Directory.CreateDirectory(Path.GetDirectoryName(msiPath));

        using (Stream source = reader.GetStream(msiFiles[0]))
        using (FileStream destination = File.Create(msiPath))
        {
            source.CopyTo(destination);
        }

        Log.LogMessage(MessageImportance.High, $"Using prebuilt MSI from '{packagePath}' for {package.Id} ({platform}).");

        msiItem = new TaskItem(msiPath);
        msiItem.SetMetadata(Metadata.Platform, platform);
        msiItem.SetMetadata(Metadata.Version, $"{package.MsiVersion}");
        msiItem.SetMetadata(Metadata.SwixPackageId, package.SwixPackageId);
        msiItem.SetMetadata(Metadata.PackageType, DefaultValues.WorkloadPackMsi);

        return true;
    }

    static void PossiblyParallelForEach<T>(bool runInParallel, IEnumerable<T> source, Action<T> body)
    {
        if (runInParallel)
        {
            _ = Parallel.ForEach(source, body);
        }
        else
        {
            foreach (var item in source)
            {
                body(item);
            }
        }
    }
}
