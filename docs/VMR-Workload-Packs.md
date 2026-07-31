# Adding Workload Packs to the VMR Build

This document explains how workload packs are built in the VMR (specifically the MSI wrapper
generation for Windows) and provides a checklist for adding new workload packs.

## Background

.NET SDK workloads install optional components via NuGet packages. On Windows, these packages are
wrapped in MSI installers for integration with the Windows installer infrastructure and Visual
Studio. The VMR build pipeline handles this wrapping automatically — but only for packs that are
explicitly registered in the build configuration.

## How Workload MSI Generation Works

The MSI wrapping process is driven by `src/sdk/src/Workloads/VSInsertion/workloads.csproj`. The
high-level flow is:

1. **Pack registration** — Packs that need MSI wrappers are listed in one of three item groups:
   - `RuntimeWorkloadPacksToDownload` — Runtime packs (e.g., `Microsoft.NETCore.App.Runtime.Mono.browser-wasm`)
   - `HostWorkloadPacksToDownload` — Hosting packs (e.g., `Microsoft.NETCore.App.Runtime.AOT.win-x64.Cross.browser-wasm`)
   - `TemplateWorkloadPacksToDownload` — Template packs

2. **Pack acquisition** — During the workloads build pass, registered packs are resolved from the
   build's package artifacts (either downloaded or found in previously-built artifacts).

3. **MSI generation** — The `BuildWorkloadMsis` target processes each pack into three MSI nupkgs:
   - `<PackId>.Msi.x64`
   - `<PackId>.Msi.x86`
   - `<PackId>.Msi.arm64`

4. **Exclusions** — Packs with `ExcludeAsMsi="true"` skip MSI generation (used for Windows-native
   runtimes like `win-x64`, `win-x86`, `win-arm64` that are included directly in the SDK).

## How the Pipeline Artifact Flow Works

The workloads build runs as a separate build pass (pass 2) in the VMR pipeline, defined in
`eng/pipelines/templates/stages/vmr-verticals.yml`. It needs access to packages produced by
other build jobs (short-stack builds, full SDK builds) via the `reuseBuildArtifactsFrom` mechanism.

The `Windows_Workloads` job (build pass 2) declares which prior jobs' artifacts it needs:
- `Windows_x64`, `Windows_x86`, `Windows_arm64` — Full SDK build outputs
- Various short-stack jobs — Mobile and WASM runtime builds (e.g., `Browser_Shortstack_wasm`,
  `Android_Shortstack_arm64`, `iOS_Shortstack_arm64`, `Browser_CoreCLR_Shortstack`)

The `reuseBuildArtifactsFrom` list makes packages from those jobs available as if they were
built locally, so `workloads.csproj` can find and wrap them.

## Short-Stack Builds

Short-stack builds produce runtime packages for platforms that can't be built as part of the
full SDK build (mobile, WASM). Each short-stack job builds only the runtime repo and its
dependencies for a specific target. Key examples:

| Job Name | What It Builds |
|----------|---------------|
| `Browser_Shortstack_wasm` | Mono-based browser-wasm runtime |
| `Browser_Multithreaded_Shortstack_wasm` | Multithreaded Mono browser-wasm runtime |
| `Browser_CoreCLR_Shortstack` | CoreCLR-based browser-wasm runtime |
| `Android_Shortstack_arm64` | Android ARM64 runtime |
| `iOS_Shortstack_arm64` | iOS ARM64 runtime |

## Checklist: Adding a New Workload Pack

When a workload manifest adds a new pack that requires an MSI installer on Windows, you must
update **two** files in the VMR:

### 1. Register the pack for MSI generation

**File:** `src/sdk/src/Workloads/VSInsertion/workloads.csproj`

Add the pack to the appropriate `*WorkloadPacksToDownload` item group:

```xml
<RuntimeWorkloadPacksToDownload Include="Microsoft.NETCore.App.Runtime.browser-wasm" />
```

Choose the correct item group:
- `RuntimeWorkloadPacksToDownload` — For runtime packs (`Microsoft.NETCore.App.Runtime.*`)
- `HostWorkloadPacksToDownload` — For AOT cross-compilation and hosting tools
- `TemplateWorkloadPacksToDownload` — For workload template packs

If the pack should NOT get an MSI wrapper (e.g., it targets the same OS the SDK runs on),
add `ExcludeAsMsi="true"`.

### 2. Ensure the pipeline can access the pack's build artifacts

**File:** `eng/pipelines/templates/stages/vmr-verticals.yml`

If the new pack is produced by a short-stack build job, that job must be listed in the
`reuseBuildArtifactsFrom` section of the `Windows_Workloads` job (build pass 2).

Look for the `Windows_Workloads` job definition (search for `buildName: Windows_Workloads`)
and add the producing job to `reuseBuildArtifactsFrom`:

```yaml
reuseBuildArtifactsFrom:
- Windows_x64
- Windows_x86
- Windows_arm64
- ${{ if not(parameters.excludeRuntimeDependentJobs) }}:
  - Browser_CoreCLR_Shortstack        # <-- Add new short-stack jobs here
  - Browser_Shortstack_wasm
  - Browser_Multithreaded_Shortstack_wasm
  # ... other short-stack jobs
```

> **Note:** Packs built by the main `Windows_x64` job (or other full SDK jobs already in the
> list) do NOT need a separate entry — their artifacts are already available.

### 3. Verify the workload manifest includes the pack

The workload manifest (typically in the owning repo, e.g.,
`src/runtime/src/mono/nuget/Microsoft.NET.Workload.Mono.Toolchain.Current.Manifest/WorkloadManifest.json.in`)
must define the pack with appropriate alias mappings. This is usually done by the team adding
the workload in their upstream repo.

## Troubleshooting

**Symptom:** `package <PackId>.msi.-x64 not found` when installing a workload on Windows.

**Diagnosis:**
1. Check if the pack is listed in `workloads.csproj` — if not, that's your problem.
2. Check the VMR build logs for the `Windows_Workloads` job to see if the NuGet package is
   available during the workloads build pass.
3. If the pack IS listed but not found during build, check that the producing job is in
   `reuseBuildArtifactsFrom`.

**Symptom:** Workload installs fine on non-Windows but fails on Windows.

**Diagnosis:** This is almost certainly a missing MSI wrapper. The non-Windows install uses
the NuGet package directly, while Windows needs the `.Msi.x64` variant. Follow the checklist
above.
