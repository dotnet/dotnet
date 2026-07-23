# Visual Studio Workload Build Tasks

## CreateVisualStudioWorkload

An MSBuild task used to create workload artifacts including MSIs and SWIX projects for Visual Studio Installer. It processes workload manifest packages to produce per-platform MSI installers for workload packs and manifests, NuGet package wrapper projects, and SWIX authoring for Visual Studio insertion.

### Input Parameters

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `WorkloadManifestPackageFiles` | `ITaskItem[]` | Yes | A set of packages containing workload manifests. |
| `PackageSource` | `string` | Yes | The directory to use for locating workload pack packages. |
| `ComponentResources` | `ITaskItem[]` | No | Items that provide metadata associated with the Visual Studio components derived from workload manifests. |
| `ShortNames` | `ITaskItem[]` | No | Items used to shorten the names and identifiers of setup packages. |
| `ManifestMsiVersion` | `string` | No | The version to assign to workload manifest installers. |
| `EnableSideBySideManifests` | `bool` | No | When `true`, manifest installers generate a non-stable UpgradeCode and a unique dependency provider key to ensure side-by-side installs. Default: `false`. |
| `IsOutOfSupportInVisualStudio` | `bool` | No | When `true`, the component and related packs are flagged as out-of-support in Visual Studio. |
| `CreateWorkloadPackGroups` | `bool` | No | When `true`, multiple packs are combined into a single installer. |
| `SkipRedundantMsiCreation` | `bool` | No | When `true`, skips creating MSIs for workload packs that are part of a pack group. |
| `DisableParallelPackageGroupProcessing` | `bool` | No | When `true`, workload pack groups are built sequentially instead of in parallel. |
| `AllowMissingPacks` | `bool` | No | When `true`, allows VS workload generation to proceed if any nupkgs declared in the manifest are not found on disk. Default: `false`. |

#### Inherited from VisualStudioWorkloadTaskBase

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `BaseIntermediateOutputPath` | `string` | Yes | Root intermediate output directory used for generating installer sources and other projects. |
| `BaseOutputPath` | `string` | Yes | Root output directory for compiled artifacts such as MSIs. |
| `WixExe` | `string` | Yes | Path to the WiX CLI (`wix.exe`). |
| `HeatExe` | `string` | Yes | Path to the harvesting tool (`heat.exe`). |
| `WixExtensions` | `ITaskItem[]` | Yes | Set of WiX extension assemblies needed to build MSIs. Items must specify the full path to the extension assemblies. |
| `CreateWixPacks` | `bool` | No | When `true`, wixpack archives are generated for each workload MSI. Default: `true`. |

### Output Parameters

| Parameter | Type | Description |
|-----------|------|-------------|
| `Msis` | `ITaskItem[]` | All MSI installers that were generated (workload packs and manifests). |
| `SwixProjects` | `ITaskItem[]` | SWIX projects (.swixproj) that can be built to generate Visual Studio Installer packages. |

---

## CreateVisualStudioWorkloadSet

Build task for generating workload set MSI installers, including projects for building the NuGet package wrappers and SWIX projects for inserting into Visual Studio.

### Input Parameters

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `WorkloadSetPackageFiles` | `ITaskItem[]` | No | Set of NuGet packages containing workload sets. |
| `WorkloadSetMsiVersion` | `string` | No | The version to assign to workload set installers. |

#### Inherited from VisualStudioWorkloadTaskBase

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `BaseIntermediateOutputPath` | `string` | Yes | Root intermediate output directory used for generating installer sources and other projects. |
| `BaseOutputPath` | `string` | Yes | Root output directory for compiled artifacts such as MSIs. |
| `WixExe` | `string` | Yes | Path to the WiX CLI (`wix.exe`). |
| `HeatExe` | `string` | Yes | Path to the harvesting tool (`heat.exe`). |
| `WixExtensions` | `ITaskItem[]` | Yes | Set of WiX extension assemblies needed to build MSIs. Items must specify the full path to the extension assemblies. |
| `CreateWixPacks` | `bool` | No | When `true`, wixpack archives are generated for each workload MSI. Default: `true`. |

### Output Parameters

| Parameter | Type | Description |
|-----------|------|-------------|
| `Msis` | `ITaskItem[]` | All MSI installers that were generated. |
| `SwixProjects` | `ITaskItem[]` | SWIX projects (.swixproj) that can be built to generate Visual Studio Installer packages. |

---

## Output Item Metadata

### `Msis` Item Metadata

Each item in the `Msis` output represents a built MSI file. The `ItemSpec` is the full path to the MSI. The following custom metadata is attached:

| Metadata Key | Description |
|--------------|-------------|
| `Platform` | The target platform of the MSI (e.g. `x86`, `x64`, `arm64`). |
| `Version` | The MSI product version. |
| `SwixPackageId` | The SWIX package identifier used for Visual Studio insertion. |
| `PackageType` | The type of MSI package (set by the MSI builder, e.g. the pack type). |
| `SourcePath` | The file system path to the generated WiX source files used to build the MSI. |
| `WixPack` | The file system path of the generated wixpack archive used for signing. Only present when `CreateWixPacks` is `true`. |
| `PackageProject` | Path to a generated `.csproj` that packages the MSI and its manifest into a NuGet package for CLI-based installs. |

### `SwixProjects` Item Metadata

Each item in the `SwixProjects` output represents a generated `.swixproj` file. The `ItemSpec` is the full path to the project file. The following custom metadata is attached:

| Metadata Key | Description |
|--------------|-------------|
| `SdkFeatureBand` | The SDK feature band the SWIX project targets (e.g. `9.0.100`). |
| `PackageType` | Classifies the SWIX project. Possible values differ by task and context (see table below). |
| `IsPreview` | `true` if the package represents a preview component; otherwise `false`. |

#### `PackageType` Values

| Value | Source Task | Description |
|-------|------------|-------------|
| `msi-pack` | `CreateVisualStudioWorkload` | SWIX project wrapping a workload pack MSI. |
| `msi-manifest` | `CreateVisualStudioWorkload` | SWIX project wrapping a workload manifest MSI. |
| `component` | `CreateVisualStudioWorkload` | SWIX project for a Visual Studio Installer component representing a workload. |
| `manifest-package-group` | `CreateVisualStudioWorkload` | SWIX project for a manifest package group (used with side-by-side manifests). |
| `msi-workload-set` | `CreateVisualStudioWorkloadSet` | SWIX project wrapping a workload set MSI. |
| `workloadset-package-group` | `CreateVisualStudioWorkloadSet` | SWIX project for a workload set package group. |
