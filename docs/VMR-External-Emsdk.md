# External emsdk consumption in .NET 12

The VMR no longer schedules raw emsdk production through the SDK or browser-runtime
build graph. Runtime still consumes Emscripten to build WebAssembly binaries.
The SDK retains workload manifest generation, payload-MSI wrapping, manifest-MSI
generation, and the existing `DownloadWorkloadMsis` modes/cadence, consistent with
10.0. Only raw toolchain production moves out of the VMR.

## Scope

- Keep `src/emsdk`, its source mapping, and `repo-projects/emsdk.proj`; source
  removal is deferred. Sparse checkouts need not materialize the source directory.
- Remove emsdk build edges from runtime and SDK and remove its shared-component
  classification. Older shared-component archives must not override its external pin.
- Keep subscriptions, standalone producer configuration, and the SDK's upstream
  `eng/Version.Details.xml`/`.props` unchanged.

## Version and artifact contract

| Artifact | Producer / version selection |
| --- | --- |
| Raw SDK, Cache, Node, Python packages | Standalone emsdk; runtime and SDK 1xx select its approved release through dependency flow. |
| `Microsoft.NET.Sdk.Emscripten.Version.Internal` | SDK 1xx republishes the selected emsdk version as a new non-shipping transport package. Its package version equals the raw emsdk version; `data/EmscriptenVersion.txt` records the package-ID family and is packed through a `None` item with the standard generated nuspec. |
| Current Emscripten manifest | Locally generated in all bands, SDK/workload-versioned; pack references use the standalone pin in 1xx or the SDK marker's flowed version in upstack. |
| Emscripten payload MSI NuGets | SDK-owned, retain the raw-pack version contract and existing generation cadence. Upstack downloads and stages them at the SDK marker's version. |
| Upstack manifest MSI NuGets | Downloaded and staged at the selected `DotNet1xxWorkloadManifestVersion`. Runtime payload MSIs continue using `DotNet1xxRuntimeVersion`. |

The marker is connected to the SDK's existing manifest restore/build/pack
traversal, reached through redist and workload packaging.
The producer selects the 1xx standalone emsdk input directly and imports
`Emscripten.targets` for shared validation, without importing consumer props or
referencing its own package.
Standard Arcade/VMR publication exposes its non-shipping package as an `External` asset for dependency
flow. Upstack obtains `MicrosoftNETSdkEmscriptenVersionInternalPackageVersion` from this
SDK-produced asset; the VMR forwards it across the separate SDK-process boundary.
Upstack restores the marker and checks its family before generation/staging.
It must flow with the selected 1xx workload set, not from a separate emsdk
subscription, coherence relationship, or runtime version.

Manifest generation remains local, including existing upstack SDK-version
overrides. There is no selected-manifest copy/download path. Upstack continues
downloading **both Emscripten payload MSIs and manifest MSIs**. MSI/SWIX generation
is still controlled by the existing modes; no build-once installer policy is added.

An effective prerelease Emscripten version is rejected for SDK labels `rtm`,
`servicing`, and `hotfix`, including marker consumption/MSI restore. Stable
versions are accepted. Since Internal is always prerelease, emsdk must eventually
publish a stable version property for the `EmscriptenVersionCurrent` family.
Switch producer and consumer version selection, manifest references, marker production, raw
acquisition/staging, and MSI downloads together; never strip a prerelease suffix.
Remove the temporary guard after that transition.

See the [SDK workload input contract](../src/sdk/src/Workloads/README.md) for
project/import paths, package layout, and publication details.

## Rollout prerequisites and validation

Standalone emsdk production, subscription changes, promotion/signing, and future
stable-version properties are separate work. Publish a real marker from SDK 1xx
before registering that asset in the consuming upstack branch's dependency
metadata. No nonexistent package version is pinned in this change.

Validate official workload/VS packaging, later-band payload/manifest-MSI reuse,
fresh-cache `wasm-tools` installation and native/AOT publishing, and shared-component/
offline source-build behavior. Local investigation exercised MSBuild selection,
staging, release guards, marker publication/traversal, and a real local
marker-pack -> generated version props -> restore -> manifest-pack path; the
investigation test harness is not included in this change.
Metadata-only pack smoke used `TargetFramework=net11.0` because the installed
12-branded stage-0 SDK still rejects net12 targets; this is not full product validation.

The experiments in [dotnet/dotnet#9717](https://github.com/dotnet/dotnet/pull/9717)
and [dotnet/dotnet#9723](https://github.com/dotnet/dotnet/pull/9723) demonstrated
build-time potential, not shipping validation of this independent package contract.
