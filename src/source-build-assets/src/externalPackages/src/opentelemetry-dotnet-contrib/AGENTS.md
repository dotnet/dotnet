# OpenTelemetry .NET Contrib - Agent Instructions

## What This Repo Is

A monorepo of **independently versioned** community-maintained OpenTelemetry
components for .NET: instrumentations, exporters, resource detectors, extensions,
samplers, and more. Each component under `src/` is its own NuGet package with its
own version, changelog, and owners.

## Build & Test Commands

```sh
# Build everything
dotnet build

# Run all tests (skip rebuild)
dotnet test --no-build

# Run tests for a single component
dotnet test test/OpenTelemetry.Instrumentation.AspNetCore.Tests/ --no-build

# Run a filtered subset of tests
dotnet test test/OpenTelemetry.Exporter.Geneva.Tests/ --no-build --filter "CategoryName=Geneva"

# Pack NuGet packages
dotnet pack --no-build
```

**Linting:**

```sh
markdownlint .
yamllint --no-warnings .
dotnet format --verify-no-changes
```

## Repository Layout

```text
src/
  OpenTelemetry.{Type}.{Name}/     # One directory per NuGet package
    .publicApi/                    # PublicAPI.Shipped.txt, PublicAPI.Unshipped.txt
    CHANGELOG.md
    README.md
    *.csproj
test/
  OpenTelemetry.{Type}.{Name}.Tests/
  OpenTelemetry.{Type}.{Name}.Benchmarks/   # optional
  Shared/                          # Shared test helpers (linked via <Compile Include=...>)
src/Shared/                        # Shared production helpers (linked via <Compile Include=...>)
build/
  Common.props                     # Global build properties
  Common.prod.props                # Production package properties (MinVer, metadata)
  Common.nonprod.props             # Test/bench project properties
  Projects/Component.proj          # Template used by CI per component
Directory.Packages.props           # Central Package Management - all versions here
```

## Component Conventions

### Structure

Every `src/` component must have:

- `CHANGELOG.md` - updated on every release
- `README.md` - installation and usage guide
- `.publicApi/PublicAPI.Shipped.txt` and `PublicAPI.Unshipped.txt` - tracked
  by `Microsoft.CodeAnalysis.PublicApiAnalyzers`

New public API surface must be declared in `PublicAPI.Unshipped.txt`. Adding a
public member without updating this file causes a build error.

### Project File Patterns

```xml
<!-- Required in every src .csproj -->
<MinVerTagPrefix>Instrumentation.AspNetCore-</MinVerTagPrefix>
<PackageValidationBaselineVersion>1.15.1</PackageValidationBaselineVersion>

<!-- Shared source files are linked, not copied -->
<Compile Include="$(RepoRoot)\src\Shared\Guard.cs" Link="Shared\Guard.cs" />
```

- **Target frameworks for libraries:** `$(TargetFrameworksForLibraries)` - All supported
  .NET LTS versions plus `net462` for Windows only.
- **Minimum .NET Framework:** `net462`
- **LangVersion:** `latest` | **Nullable:** `enable` | **ImplicitUsings:** `enable`
- **StyleCop.Analyzers** is a global package reference applied to all projects
- `TreatWarningsAsErrors=true` in Release builds; code style is enforced at build
  time (`EnforceCodeStyleInBuild=true`)

### Versioning

Versioning is fully automated via **MinVer** using Git tags. Tag format:
`{Type}.{Name}-{SemVer}` (e.g., `Instrumentation.AspNetCore-1.15.1`).

Never manually set `<Version>` in a csproj.

### Package Management

All NuGet versions are centralized in `Directory.Packages.props`. Do not add
`Version="..."` attributes to `<PackageReference>` in individual project files;
add or update the version only in `Directory.Packages.props`.

Key version variables defined there:

- `$(OpenTelemetryCoreLatestVersion)` - stable OTel SDK/API version
- `$(OpenTelemetryCoreUnstableLatestVersion)` - pre-release OTel SDK/API version

### Shared Source

Code shared across components lives in `src/Shared/` (production) or `test/Shared/`
(test). It is included in individual projects via `<Compile Include="..." Link="..."/>`,
not as a project reference. Do not add a project reference to a Shared project;
link individual files instead.

## CI Behavior

The CI workflow (`ci.yml`) uses **path-based change detection**: only the component(s)
touched by a PR get built and tested. The reusable workflow `Component.BuildTest.yml`
runs a matrix across:

- OS: Windows, Ubuntu
- TFM: Each supported .NET version (e.g. `net10.0`) and `net462` (Windows only)

Component owners are defined in `.github/component_owners.yml` (not `CODEOWNERS`).

## Adding a New Component

1. Create `src/OpenTelemetry.{Type}.{Name}/` with `.publicApi/`, `CHANGELOG.md`,
   `README.md`, and a `.csproj` importing `$(RepoRoot)/build/Common.prod.props`.
2. Create `test/OpenTelemetry.{Type}.{Name}.Tests/` importing `Common.nonprod.props`.
3. Add both projects to `opentelemetry-dotnet-contrib.slnx` (hand-maintained).
4. Add the component path to `.github/component_owners.yml`.
5. Add path filters for the new component to `.github/workflows/ci.yml`.
