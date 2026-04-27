# Servicing Branch Dependency Topology

## Overview

Not all VMR branches build all components from source. The **1xx band** is the "full source build" band for each major version, while higher bands (2xx, 3xx) consume runtime and other components as **prebuilt packages** from the 1xx band.

## Branch Hierarchy

```
release/X.Y.1xx  ← Full source build: runtime + SDK + all components
    │
    │  runtime packages flow as prebuilts
    ▼
release/X.Y.2xx  ← Source builds: SDK, roslyn, fsharp, nuget, arcade
    │               Consumes: runtime, aspnetcore as packages from 1xx
    │
    │  inherits the same runtime prebuilts
    ▼
release/X.Y.3xx  ← Source builds: SDK, roslyn, fsharp, nuget, arcade
                    Consumes: runtime, aspnetcore as packages from 1xx
```

## What This Means for Version Tracing

When tracing a component SHA through a **1xx branch** build:
- Check `source-manifest.json` at the VMR commit — runtime will be listed directly

When tracing through a **2xx or 3xx branch** build:
- `source-manifest.json` will NOT list runtime (it's not source-built)
- Instead, check `eng/Version.Details.xml` for `MicrosoftNETCoreAppRefPackageVersion`
- That dependency's `<Sha>` element points to the **1xx VMR commit** that produced the runtime packages
- Then check `source-manifest.json` at THAT 1xx VMR commit for the actual runtime SHA

### Example Chain (10.0.300 SDK)

```
10.0.300-preview.26117.103
    → VMR branch: release/10.0.3xx
    → VMR commit: 120a956a...
    → Version.Details.xml: MicrosoftNETCoreAppRefPackageVersion = 10.0.2
        → Source: dotnet-dotnet SHA 44525024...  (this is a release/10.0.1xx commit)
        → source-manifest.json at 44525024...:
            → dotnet/runtime: 9ffface2f3fa...
```

## Source-Build vs Prebuilt Components by Branch

| Component | 1xx band | 2xx band | 3xx band |
|-----------|----------|----------|----------|
| runtime | ✅ Source | 📦 Prebuilt | 📦 Prebuilt |
| aspnetcore | ✅ Source | 📦 Prebuilt | 📦 Prebuilt |
| SDK | ✅ Source | ✅ Source | ✅ Source |
| roslyn | ✅ Source | ✅ Source | ✅ Source |
| fsharp | ✅ Source | ✅ Source | ✅ Source |
| nuget | ✅ Source | ✅ Source | ✅ Source |
| arcade | ✅ Source | ✅ Source | ✅ Source |

## Forward Flow Implications

- The `release/X.Y.1xx` branch receives forward flows from **all** component repos (including runtime)
- The `release/X.Y.2xx` and `release/X.Y.3xx` branches receive forward flows from SDK, roslyn, fsharp, nuget, arcade — but **NOT** runtime or aspnetcore
- Runtime version updates in 2xx/3xx branches come through **Version.Details.xml** updates, not forward flow PRs

## Key Files in the VMR

| File | Purpose | Location |
|------|---------|----------|
| `src/source-manifest.json` | Lists all source-built component SHAs | Root of VMR repo |
| `eng/Version.Details.xml` | Lists package dependencies with source SHAs | Root of VMR repo |
| `eng/Versions.props` | Package version properties | Root of VMR repo |
