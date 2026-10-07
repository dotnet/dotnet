# External emsdk consumption in .NET 12

The VMR no longer schedules raw emsdk production through the SDK or browser-runtime
build graph. Runtime still consumes Emscripten to build WebAssembly binaries.
The SDK retains local workload manifest generation and 1xx payload/manifest-MSI
generation, consistent with 10.0. Raw toolchain production moves out of the VMR.
Upstack reuses manifest MSIs but no longer downloads or stages Emscripten
toolchain payloads or their payload-MSI wrappers.

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
| Current Emscripten manifest | Locally generated in all bands, SDK/workload-versioned; pack references use the standalone pin in 1xx or the selected 1xx manifest's pack versions in upstack. |
| Emscripten payload MSI NuGets | SDK 1xx-owned, retain the raw-pack version contract and existing generation cadence. Upstack does not download or stage them. |
| Upstack manifest MSI NuGets | Downloaded and staged at the selected `DotNet1xxWorkloadManifestVersion`. Runtime payload MSIs continue using `DotNet1xxRuntimeVersion`. |

The 1xx manifest already records the raw emsdk package version and family.
Upstack restores that manifest using the existing
`DotNet1xxWorkloadManifestVersion` selection, then reads and validates its pack
versions and family during manifest generation, after ordinary restore.
The reader parser, model sources, and resources are compiled directly into
`sdk-tasks`, avoiding a reader-package or reader-project bootstrap dependency.
No task compilation or manifest-content reading takes place during ordinary
or static-graph restore.
There is no additional SDK version transport package or independent upstack
emsdk subscription, coherence relationship, or runtime-version inference.

Manifest generation remains local, including existing upstack SDK-version
overrides. The selected 1xx manifest is downloaded as an input, not copied over
the locally generated manifest. Upstack continues downloading **manifest MSIs**, which the Windows SDK
installation bundle embeds. The eight unused Emscripten payload-MSI copies are
removed from its internal artifact set. Runtime payload-MSI staging is unchanged.
Upstack installer builds require the existing `DownloadWorkloadMsis=true` mode;
MSI/SWIX generation remains a 1xx operation. Downloaded MSI packages and generated
upstack manifests keep their existing internal publication visibility.

An effective prerelease Emscripten version is rejected for SDK labels `rtm`,
`servicing`, and `hotfix`, including upstack manifest generation. Stable
versions are accepted. Since Internal is always prerelease, emsdk must eventually
publish a stable version property for the `EmscriptenVersionCurrent` family.
Switch 1xx version selection, manifest references, raw
acquisition/staging, and payload-MSI generation together; never strip a prerelease suffix.
Remove the temporary guard after that transition.

See the [SDK workload input contract](../src/sdk/src/Workloads/README.md) for
project/import paths, package layout, and publication details.

## Rollout prerequisites and validation

Standalone emsdk production, subscription changes, promotion/signing, and future
stable-version properties are separate work. Upstack uses its existing selected
1xx workload manifest; no new SDK dependency-flow asset needs registration.
No nonexistent package version is pinned in this change.

Validate official workload/VS packaging, later-band manifest-MSI reuse,
fresh-cache `wasm-tools` installation and native/AOT publishing, and shared-component/
offline source-build behavior. Local investigation exercised MSBuild selection,
staging, release guards, and manifest generation. Manifest-derived selection was
also exercised through ordinary and static-graph restore from fresh package
caches, with isolated payload/tool metadata fixtures, and real 1xx/upstack manifest
packs. Those checks do not establish real installer generation or signing; the
investigation test harness is not included in this change.
Metadata-only pack smoke used `TargetFramework=net11.0` because the installed
12-branded stage-0 SDK still rejects net12 targets; this is not full product validation.

The experiments in [dotnet/dotnet#9717](https://github.com/dotnet/dotnet/pull/9717)
and [dotnet/dotnet#9723](https://github.com/dotnet/dotnet/pull/9723) demonstrated
build-time potential, not shipping validation of this independent package contract.
