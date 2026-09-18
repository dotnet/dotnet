---
name: source-build-investigation
description: Investigate source-build failures in local dotnet/dotnet VMR builds and `SB_*` CI legs. USE FOR VMR source-build issues involving prebuilt packages, source-build/source-only failures, source-build-assets (SBRP), package poisoning, repo build ordering, binary detection/removal, or failures limited to `SB_*` jobs. DO NOT USE FOR regular CI test failures, codeflow staleness, dependency flow tracing, crash dumps, or general NuGet package management unrelated to source-build.
---

# Source-Build Investigation

Reference for investigating local and CI source-build failures in the dotnet/dotnet VMR (Virtual Monolithic Repository). Linux distributions build .NET entirely from source — the source-only build (SB) legs in CI validate this. SB leg names start with `SB_` (e.g., `SB_CentOSStream10_Online_MsftSdk_x64`). Local investigations use local inputs and diagnostics; CI access is not required.

For foundational source-build concepts, see [Understanding .NET Source-Build](https://github.com/dotnet/source-build/blob/main/Documentation/understanding-source-build.md).

## Build artifacts

SB legs produce two artifacts:
- **`<leg>_BuildLogs_Attempt<N>`** — logs, binlogs, prebuilt reports
- **`<leg>_Artifacts`** on success, or **`<leg>_Artifacts_Attempt<N>`** on failure — built assets/packages

### Downloading specific files

Find pipeline artifacts, then download individual files from the `downloadUrl` using `format=file&subPath=` (URL-encode path separators as `%2F`):

```
<downloadUrl>?format=file&subPath=%2Fartifacts%2Flog%2FRelease%2Fprebuilt-usage.xml
<downloadUrl>?format=file&subPath=%2Fartifacts%2Flog%2FRelease%2Farcade%2FBuild.binlog
<downloadUrl>?format=file&subPath=%2Fpackages%2FRelease%2FNonShipping%2Farcade%2FMicrosoft.DotNet.Arcade.Sdk.11.0.0-ci.nupkg
```

These examples assume a URL without query parameters. If the returned URL already has a query string, set the format value to `file` and update `subPath` in that query rather than appending another `?`. Preserve all other parameters and the endpoint's `format` or `$format` spelling.

### Key files

**In BuildLogs:**
| Path | Description |
|---|---|
| `artifacts/log/Release/Build.binlog` | Outer VMR orchestration binlog |
| `artifacts/log/Release/prebuilt-usage.xml` | Prebuilt package usages across all repos |
| `artifacts/log/Release/<repo>/prebuilt-usage.xml` | Per-repo prebuilt usage |
| `artifacts/log/Release/<repo>/Build.binlog` | Per-repo MSBuild binlog |

**In Artifacts:**
| Path | Description |
|---|---|
| `packages/Release/NonShipping/<repo>/*.nupkg` | Non-shipping packages |
| `packages/Release/Shipping/<repo>/*.nupkg` | Shipping packages |

## Build flow

The source-build builds repos sequentially in dependency order. Each repo produces intermediate nupkgs that feed into downstream repos. The build order is defined by the repo dependency graph — earlier repos' outputs become available as **source-built** packages for later repos.

For builds targeting different SDK feature bands (e.g., 10.0.2xx, 10.0.3xx), see [Feature Band Source Building](https://github.com/dotnet/source-build/blob/main/Documentation/feature-band-source-building.md).

The high-level flow:
1. **Prep the Build** — downloads previously-source-built (PSB) artifacts and sets up package feeds
2. **Build repos in order** — each repo restores from SBRP, PSB, and source-built packages from earlier repos, then builds and publishes its own intermediates
3. **Finish** (`eng/finish-source-only.proj`) — reports prebuilt usage; when poisoning is enabled, publishing also checks completed outputs via `eng/PublishSourceBuild.props`

Repo build order is determined by `<RepositoryReference>` items in each repo's `.proj` file under `repo-projects/`. These declare dependencies between repos — MSBuild uses them to compute the build graph. If a repo needs a package produced by another repo, the producing repo must build first. When a package isn't available from any source-build feed, it becomes a **prebuilt**.

To see the actual build order, look at the `repo-projects/` directory in the VMR — each `.proj` file corresponds to a repo and its `<RepositoryReference>` items declare which repos must build before it.

### Stage 2 builds

A **stage 2 build** (also called bootstrapping) is when you take the SDK and packages produced by a source-build (stage 1) and use them to rebuild the entire product again. This validates that the source-built product is fully self-hosting — it can build itself without any Microsoft-built inputs.

**CI leg naming:**
- Stage 1 legs: `SB_<distro>_Online_MsftSdk_x64` — builds using the Microsoft SDK
- Previous-source-built SDK legs: `SB_<distro>_<mode>_PreviousSourceBuiltSdk` — use the previously published SDK and PSB.
- Stage 2 legs: `SB_<distro>_Offline_CurrentSourceBuiltSdk` — rebuilds using the SDK from stage 1

Stage 2 legs have `reuseBuildArtifactsFrom` set to their corresponding stage 1 leg (e.g., `SB_CentOSStream10_Offline_CurrentSourceBuiltSdk` depends on `SB_CentOSStream10_Online_MsftSdk_x64`).

When a failure occurs only in stage 2 but not stage 1, the issue is likely:
- A dependency that works when Microsoft-built but not when source-built
- An analyzer or tool that behaves differently with source-built inputs
- A version mismatch between what the SDK expects and what source-build produces

For more info about stage 2 builds, see [How to Build Stage2](https://github.com/dotnet/source-build/blob/main/Documentation/how-to-stage2-build.md).

## Package sources

Source-build resolves packages from several sources. Each repo gets its own NuGet cache at `src/<repo>/artifacts/.packages/`.

### SBRP (source-build-assets)

API-only stubs at `src/source-build-assets/src/referencePackages/src/`. These contain `[assembly: ReferenceAssembly]` and `throw null` implementations — they satisfy compile-time references but **cannot be loaded for execution**.

SBRP also has **external packages** built from real source (e.g., Newtonsoft.Json) at `src/source-build-assets/src/externalPackages/`.

### PSB (previously-source-built)

Real, executable nupkgs from the previous source-build. Downloaded during `Prep the Build`. Find the URL in that step's log:
```
Downloading previously source-built artifacts from
  https://ci.dot.net/public/source-build/Private.SourceBuilt.Artifacts.<version>.<rid>.tar.gz
```

### Source-built packages

Packages produced by earlier repos in the current build (e.g., `nuget-client` produces NuGet packages consumed by later repos).

## Package-version precedence and bootstrap mismatches

Follow this process when restore or build diagnostics, or a prebuilt report, point to unexpected package-version selection or incompatible bootstrap inputs. Examples include a requested version missing from source-build feeds despite an available alternative, or an assembly or tool failure that implicates mismatched PSB, current, or shared-component dependencies. Trace only the affected repo and property families. Skip this analysis for unrelated test, network, signing, or checked-in binary failures.

For general package flow, see [Package Dependency Flow](https://github.com/dotnet/source-build/blob/main/Documentation/package-dependency-flow.md). For VMR-specific version selection:

1. Inspect both the outer VMR `Build.binlog` and the affected repo's `artifacts/log/Release/<repo>/Build.binlog`. The outer log records orchestration inputs and the launched command. The inner restore or build runs in a separate process, so use per-repo diagnostics to inspect its evaluated properties.
2. Read the aggregate `artifacts/obj/PackageVersions/PackageVersions.<repo>.props` and every file it imports. Neither checked-in versions such as `Version.Details.props` nor a single generated file establish the consumed value.
3. Record every assignment to the affected `*Version`, `*PackageVersion`, and `*PreviousVersion` properties in **Previous -> Current -> SharedComponents** import order. Shared-component imports are conditional: follow the actual aggregate, including assignments that appear irrelevant to the consumer. Across these generated imports, the last assignment wins. Shared-components props also emit and can overwrite `*PreviousVersion`, so that suffix does not guarantee PSB provenance.
4. Trace the resulting values through repo-specific orchestration into the inner restore or build. Record the outer evaluation and exact inner command arguments, including overrides and the requested restore version. Confirm the consumed values in the affected repo's binlog.

An unchanged PSB with updated current or shared-component inputs can indicate a bootstrap mismatch; it does not prove causation. Do not hardcode servicing versions, edit Maestro-generated files, or validate with property fixtures that omit an aggregate import.

## Comparing passing and failing builds

When using a passing build to investigate a failure, record the following for both builds:
- Source revision and content identity.
- PSB and shared-component archives, current SDK and runtime inputs, and effective package versions.
- Exact commands, environment, image digest, RID, architecture, configuration, and behavior switches.

Use verified content digests or authoritative write-once build or snapshot identities as immutable evidence. Versioned filenames alone are insufficient. Do not assume a passing PR build or older build is equivalent.

Rule out a difference as a cause only when at least one of these conditions holds:
- Evaluated data flow shows that the difference cannot affect the failure.
- A controlled comparison changes only that input without changing the behavior.
- The difference affects only steps after the observed failure.

Otherwise, describe the comparison as suggestive, list evidence gaps, and lower confidence.

## Prebuilt reports

The error from `eng/finish-source-only.proj` lists detected prebuilt packages. The `prebuilt-usage.xml` files provide details:

```xml
<UsageData>
  <Usages>
    <Usage Id="NuGet.Commands" Version="7.0.1"
           File="artifacts/obj/arcade/project-assets-json/obj/Microsoft.DotNet.Build.Tasks.Packaging/project.assets.json"
           IsDirectDependency="true" />
  </Usages>
</UsageData>
```

| Field | Meaning |
|---|---|
| `Id` / `Version` | The prebuilt package |
| `File` | Path encodes the repo: `artifacts/obj/<repo>/project-assets-json/obj/<project>/project.assets.json` |
| `IsDirectDependency` | `true` = `<PackageReference>` in csproj; absent = transitive |

Common prebuilt causes:
- Package version bumped to one not in SBRP or PSB
- Older servicing version requested while source-build produces a newer one
- New dependency introduced with no source-build equivalent
- Transitive dependency pulled in by a top-level version change

For a detailed guide on eliminating prebuilts, see [Eliminating Pre-Builts](https://github.com/dotnet/source-build/blob/main/Documentation/eliminating-pre-builts.md).

## Inspecting built packages

Nupkgs are zip archives. Compare between working and failing builds to find differences.

The `deps.json` inside a task nupkg shows runtime dependency resolution — what DLLs MSBuild will load when the task runs.

## Poisoning

Source-build uses **package poisoning** (leak detection) to detect disallowed binary inputs in final outputs. `eng/init-poison.proj` marks prebuilt and PSB packages, plus selected shared-component tooling packages. `eng/PublishSourceBuild.props` checks completed outputs.

For full details on how poisoning and leak detection work, see [Leak Detection](https://github.com/dotnet/source-build/blob/main/Documentation/leak-detection.md). For understanding the format of poison reports, see [Poison Report Format](https://github.com/dotnet/source-build/blob/main/Documentation/poison-report-format.md).

When poisoning is enabled:
1. Verify that `PoisonPackages` executed and produced the intended catalogs and markers. Restoring or evaluating `eng/init-poison.proj` does not initialize poison.
2. Verify that `ReportPoisonUsage` executed after the publishing `Execute` target. Confirm that it consumed the prebuilt, PSB, and shared-component catalogs and markers and produced `ReportPoisonUsage.complete`. Catalog generation alone does not prove that completed outputs were checked.
3. Inspect produced nupkgs to distinguish a declared package dependency from a bundled implementation assembly. Dependency-only use can be valid; bundling a PSB implementation binary can be a leak.

## Reproduction workflow

1. **Read the target revision's contract.** Inspect that revision's `prep-source-build.sh`, `build.sh`, relevant sourced scripts, MSBuild imports, and pipeline templates. Read implementations and defaults, not just help text.
2. **Recover the evidence.** Use known inputs for local builds. For CI, retain build metadata, logs for the exact job and attempt, the artifact inventory, and outer and per-repo binlogs. Reuse retained evidence when available. Recover the actual prep and build commands, command prefixes, environment, image, and input acquisition steps. Identify the synchronized VMR content; a constituent-repo pipeline revision alone does not identify that content. If exact source or inputs are unavailable, record the gap rather than substituting a similar PR or build.
3. **Preserve build behavior.** Keep the original commands alongside the reproduction. Preserve offline restrictions, network isolation, and publishing behavior. For source-archive builds, follow the target revision's source-identity requirements. Trace whether prep consumes, replaces, or transforms bootstrap packages; copying an archive alone does not reproduce those transformations. Document path mappings and other deviations.
4. **Handle masked values safely.** Never execute masked `***` values or expose credentials in recorded commands, URLs, or diagnostics. If required values cannot be recovered safely, stop or explicitly label a credential-redacted reproduction as lower-confidence.
5. **Execute with isolated state.** Use isolated source, fresh caches, and fresh container volumes where applicable. Preserve the recorded inputs and environment without contaminating the working checkout. Retain executed commands, exit results, and outer and per-repo diagnostics. Treat private evidence as sensitive. Before retrying, inspect prep and build failures; they may be the behavior under investigation.
6. **Check the result.** `Building <repo>...done` plus the expected packages establishes that repo's checkpoint, not full-build success. Before claiming end-to-end success, verify the full graph through `eng/finish-source-only.proj` and applicable publishing and poison checks. Report unrelated later failures, including known prebuilt-report failures, separately from the behavior under investigation.

Report source and input provenance, the failing repo or checkpoint, effective package versions or arguments, evidence gaps, deviations, and root-cause confidence. Cite both outer and per-repo diagnostics. Distinguish an executed reproduction from commands that were only inspected.

## Binary detection

Source-build tracks new binaries introduced by changes. During the validation stage, a `NewBinaries.txt` report is produced and downloaded as part of the build artifacts under `log/NewBinaries.txt`. This report lists binaries that were not present in the baseline.

Binary detection failures indicate that a change introduced a checked-in binary or a binary that wasn't expected. To resolve, either remove the binary and build it from source, or if the binary is intentionally included, add it to the allowed binaries baseline at `eng/allowed-sb-binaries.txt`.

## Post-build signing

In source-only builds, signing does **not** happen during the build itself. Instead, source-built artifacts are signed after the build completes via `eng/sign-source-built-artifacts.proj`. This is controlled by the `/p:DotNetSourceOnlyPostBuildSign=true` property.

The post-build signing step runs MicroBuild/ESRP signing on the source-built packages. If signing fails, check the `outer-sign-source-built-artifacts.binlog` in the build logs. Common issues include missing signing certificates, ESRP connectivity problems, or packages containing unsigned assemblies that require signing.

Legs with `disableSigning: true` in the pipeline definition skip signing entirely — most SB legs use this. Only legs with `disableSigning: false` produce signed output.

### Signing verification

The `VMR_Validation` stage includes `ValidateSigning` jobs that run on Windows, Mac, and Linux. These jobs (defined in `eng/pipelines/templates/steps/vmr-validate-signing.yml`) verify that all assemblies in the build output are properly signed. They only run when the sign type is `real` (i.e., production builds).

Signing verification failures indicate unsigned or incorrectly signed binaries in the output.

## Outer-loop builds

The source-build outer-loop pipeline (`eng/pipelines/source-build-outer-loop.yml`) runs additional SB legs that are too expensive or specialized for the main PR/CI pipeline. These include Mono runtime builds.

## Key VMR files

| File | Purpose |
|---|---|
| `eng/finish-source-only.proj` | Prebuilt detection at end of SB build |
| `eng/init-poison.proj` | Marks binary inputs and writes poison catalogs before the graph build |
| `eng/PublishSourceBuild.props` | Checks completed source-build outputs for poison |
| `repo-projects/Directory.Build.targets` | Generates and orders aggregate package-version imports |
| `repo-projects/<repo>.proj` | Repo-specific orchestration and package-version inputs |
| `artifacts/obj/PackageVersions/PackageVersions.<repo>*.props` | Generated aggregate and per-source package-version inputs |
| `src/source-build-assets/src/referencePackages/src/` | SBRP reference package stubs |
| `src/source-build-assets/src/externalPackages/` | External packages built from source |
| `src/source-manifest.json` | Submodule commit SHAs for external packages |
| `src/<repo>/eng/Versions.props` | Per-repo version properties |
| `src/<repo>/Directory.Packages.props` | Central package management versions |
| `src/arcade/eng/BuildTask.targets` | Controls arcade task package publish output |

## Tools

| Tool | Use for |
|---|---|
| `github-mcp-server` | PR details, file contents, commits, code search |
| Azure DevOps MCP server | Get build logs/artifacts |
| `binlog-mcp` | Load and query MSBuild binlogs |
