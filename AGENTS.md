# .NET VMR (Virtual Monolithic Repository)

This is the [dotnet/dotnet](https://github.com/dotnet/dotnet) VMR — a single repository containing all source code needed to build the .NET SDK. It mirrors sources from individual development repositories (runtime, sdk, aspnetcore, roslyn, etc.) into `src/<repo>/` directories. Active development happens in those upstream repos; the VMR is a synchronized mirror used for unified product builds.

## Architecture

### Directory Layout

- **`src/<repo>/`** — Mirrored source from each constituent repository (e.g., `src/runtime/`, `src/sdk/`, `src/aspnetcore/`). Each has its own build system, solution files, and conventions.
- **`repo-projects/`** — MSBuild orchestration files (`<repo>.proj`) that drive each repo's build from the VMR. Each file declares `RepositoryReference` items to express inter-repo build dependencies.
- **`eng/`** — VMR-level build infrastructure, versioning, custom MSBuild tasks (`eng/tools/tasks/`), and CI pipeline definitions (`eng/pipelines/`).
- **`test/`** — VMR-level validation tests (not individual repo tests):
  - `Microsoft.DotNet.Tests` — Shared tests
  - `Microsoft.DotNet.SourceBuild.Tests` — Source-build-specific validation
  - `Microsoft.DotNet.UnifiedBuild.Tests` — Unified-build-specific validation
  - `Microsoft.DotNet.Installer.Tests` — Linux installer (deb/rpm) tests
  - Scenario tests are built/run from `repo-projects/scenario-tests.proj` using the `src/scenario-tests/` repo
- **`docs/`** — VMR design and operation documentation. Key docs: `VMR-Design-And-Operation.md`, `VMR-Code-And-Build-Workflow.md`, `Codeflow-PRs.md`.

### Source Tracking Files

- **`src/source-mappings.json`** — Defines which repos are synced, remote URLs, and file inclusion/exclusion rules.
- **`src/source-manifest.json`** — Records the exact commit SHA of each repo synced into the VMR at any given point.

### Build Orchestration Flow

The build is driven entirely by MSBuild, not solution files:

1. `build.cmd`/`build.sh` invokes MSBuild on `build.proj`
2. `build.proj` (Microsoft.Build.Traversal SDK) builds `repo-projects/dotnet.proj`
3. `dotnet.proj` declares `RepositoryReference Include="sdk"` as the root dependency
4. Each `repo-projects/<repo>.proj` declares its own `RepositoryReference` items, forming a dependency graph
5. `repo-projects/Directory.Build.targets` resolves transitive dependencies and invokes each repo's build script (`eng/common/build.sh` or `build.cmd`) with assembled arguments from `repo-projects/Directory.Build.props`.

### Build Modes

- **Microsoft-based build** (default): Uses online NuGet feeds for pre-built dependencies. Works on Windows, Linux, and macOS.
- **Source-build** (`-sb` flag): Builds everything from source with no pre-built binaries. Linux only. Requires `./prep-source-build.sh` first.
- **Short stack**: Only builds runtime and its dependencies (for mobile/WASM targets). Controlled by `ShortStack` property in `eng/RuntimeIdentifier.props`.
- **Build passes**: Some repos build in multiple passes. For example, runtime pass 2 builds cross-OS DACs on Windows x86; aspnetcore pass 2 builds hosting bundles. Controlled by `DotNetBuildPass` in `repo-projects/dotnet.proj`.

### Shared vs Non-Shared Components

Repos are categorized in `eng/VmrLayout.props` as `SharedRepositoryReference` items (aspnetcore, runtime, winforms, wpf, efcore, etc.) or non-shared (sdk, msbuild, roslyn, fsharp, etc.). Shared components are only built when `DotNetBuildSharedComponents=true` (1xx feature band builds). Non-1xx builds consume pre-built shared components from `prereqs/packages/shared-components/`.

## Build Commands

**Full SDK build (Windows):**
```cmd
.\build.cmd -bl
```

**Full SDK build (Unix):**
```bash
./build.sh -bl
```

**Source-build (Linux only):**
```bash
./prep-source-build.sh
./build.sh -sb -bl
```

**Run VMR-level tests:**
```cmd
.\build.cmd -test -bl
```
```bash
./build.sh --test -bl
```

### Build Output Locations

- **Built SDK archive**: `artifacts/assets/Release/dotnet-sdk-<version>-<rid>.zip` (or `.tar.gz`)
- **Shipping packages**: `artifacts/packages/Release/Shipping/<repo>/`
- **Build logs**: `artifacts/log/` (per-repo logs in `artifacts/log/<repo>/`)
- **Binary logs**: `artifacts/log/Build.binlog` (when `-bl` is used)
- **Intermediate objects**: `artifacts/obj/`
- **Test results**: `artifacts/TestResults/`

## Key Conventions

- The VMR uses the [Microsoft.DotNet.Arcade.Sdk](https://github.com/dotnet/arcade) for shared build infrastructure. Arcade provides the common `eng/common/` scripts, signing, packaging, and CI patterns used across all .NET repos. The `eng/common/` directory at the VMR root (and within each `src/<repo>/`) is effectively read-only — it is sourced from arcade and overwritten during re-bootstrap or code flow. Do not modify these files directly; changes to shared build scripts must be made in the [dotnet/arcade](https://github.com/dotnet/arcade) repository.
- Default build configuration is `Release` (set in root `Directory.Build.props`).
- `Nullable` is enabled globally for VMR-level projects.
- `LangVersion` is set to `latest` for VMR-level projects.
- MSBuild XML files (`.proj`, `.props`, `.targets`) use 2-space indentation. C# files use 4-space indentation. Shell scripts use 2-space indentation.
- Shell scripts (`.sh`) must use LF line endings. Batch files (`.cmd`) use CRLF.
- Code style is governed by `.editorconfig` files. The root `.editorconfig` sets basics (UTF-8, spaces, final newlines). Each repo under `src/` has its own `.editorconfig` with more specific rules.

## Per-Repo Instructions

Each constituent repo may have its own AI assistant instructions (i.e. `AGENTS.md`, `copilot-instructions.md` file) with repo-specific build, test, and coding conventions. **Always refer to these when working within a specific `src/<repo>/` directory.**
