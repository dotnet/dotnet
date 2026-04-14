---
name: source-build-investigation
description: Investigate source-build failures in dotnet/dotnet VMR CI builds. USE FOR any mention of "prebuilt", "source-build/source-only failure", "source-build-reference-packages (SBRP)", "poison", source-build repo build ordering problems, or any build leg failures that only occur in `SB_*` job names. DO NOT USE FOR regular CI test failures, codeflow staleness, dependency flow tracing, crash dumps, or general NuGet package management unrelated to source-build.
---

# Source-Build Investigation

Reference for investigating source-build failures in the dotnet/dotnet VMR (Virtual Monolithic Repository). Linux distributions build .NET entirely from source — the source-only build (SB) legs in CI validate this. SB leg names start with `SB_` (e.g., `SB_CentOSStream10_Online_MsftSdk_x64`).

## Build artifacts

SB legs produce two artifacts:
- **`<leg>_BuildLogs_Attempt<N>`** — logs, binlogs, prebuilt reports
- **`<leg>_Artifacts`** on success, or **`<leg>_Artifacts_Attempt<N>`** on failure — built packages (nupkgs)

### Downloading specific files

Find pipeline artifacts, then download individual files from the `downloadUrl` using `format=file&subPath=` (URL-encode path separators as `%2F`):

```
<downloadUrl>?format=file&subPath=%2Fartifacts%2Flog%2FRelease%2Fprebuilt-usage.xml
<downloadUrl>?format=file&subPath=%2Fartifacts%2Flog%2FRelease%2Farcade%2FBuild.binlog
<downloadUrl>?format=file&subPath=%2Fpackages%2FRelease%2FNonShipping%2Farcade%2FMicrosoft.DotNet.Arcade.Sdk.11.0.0-ci.nupkg
```

### Key files

**In BuildLogs:**
| Path | Description |
|---|---|
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

The high-level flow:
1. **Prep the Build** — downloads previously-source-built (PSB) artifacts and sets up package feeds
2. **Build repos in order** — each repo restores from SBRP, PSB, and source-built packages from earlier repos, then builds and publishes its own intermediates
3. **Finish** (`eng/finish-source-only.proj`) — validates no prebuilt or poisoned packages leaked into the final output

Repo build order is determined by `<RepositoryReference>` items in each repo's `.proj` file under `repo-projects/`. These declare dependencies between repos — MSBuild uses them to compute the build graph. If a repo needs a package produced by another repo, the producing repo must build first. When a package isn't available from any source-build feed, it becomes a **prebuilt**.

To see the actual build order, look at the `repo-projects/` directory in the VMR — each `.proj` file corresponds to a repo and its `<RepositoryReference>` items declare which repos must build before it.

## Package sources

Source-build resolves packages from several sources. Each repo gets its own NuGet cache at `src/<repo>/artifacts/.packages/`.

### SBRP (source-build-reference-packages)

API-only stubs at `src/source-build-reference-packages/src/referencePackages/src/`. These contain `[assembly: ReferenceAssembly]` and `throw null` implementations — they satisfy compile-time references but **cannot be loaded for execution**.

SBRP also has **external packages** built from real source (e.g., Newtonsoft.Json) at `src/source-build-reference-packages/src/externalPackages/`.

### PSB (previously-source-built)

Real, executable nupkgs from the previous source-build. Downloaded during `Prep the Build`. Find the URL in that step's log:
```
Downloading previously source-built artifacts from
  https://ci.dot.net/public/source-build/Private.SourceBuilt.Artifacts.<version>.<rid>.tar.gz
```

### Source-built packages

Packages produced by earlier repos in the current build (e.g., `nuget-client` produces NuGet packages consumed by later repos).

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
- Older servicing version requested (e.g., 9.0.6) while source-build produces a newer one (e.g., 11.0.0-ci)
- New dependency introduced with no source-build equivalent
- Transitive dependency pulled in by a top-level version change

## Inspecting built packages

Nupkgs are zip archives. Compare between working and failing builds to find differences.

The `deps.json` inside a task nupkg shows runtime dependency resolution — what DLLs MSBuild will load when the task runs.

## Poisoning

Source-build uses **package poisoning** (leak detection) to detect when Microsoft-built (non-source-built) binaries leak into the final output. PSB assemblies are marked with a poison payload; if any appear in the final build output, the `eng/finish-source-only.proj` step fails.

For full details on how poisoning and leak detection work, see: https://github.com/dotnet/source-build/blob/main/Documentation/leak-detection.md

## Key VMR files

| File | Purpose |
|---|---|
| `eng/finish-source-only.proj` | Prebuilt detection at end of SB build |
| `src/source-build-reference-packages/src/referencePackages/src/` | SBRP reference package stubs |
| `src/source-build-reference-packages/src/externalPackages/` | External packages built from source |
| `src/source-manifest.json` | Submodule commit SHAs for external packages |
| `src/<repo>/eng/Versions.props` | Per-repo version properties |
| `src/<repo>/Directory.Packages.props` | Central package management versions |
| `src/arcade/eng/BuildTask.targets` | Controls arcade task package publish output |

## Tools

| Tool | Use for |
|---|---|
| `github-mcp-server` | PR details, file contents, commits, code search |
| `helix-mcp` | Get build logs/artifacts |
| `binlog-mcp` | Load and query MSBuild binlogs |
