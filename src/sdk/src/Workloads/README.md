# Emscripten package inputs

Raw Emscripten toolchain packages have a version independent of the runtime and
SDK. Manifest generation and Windows workload packaging share the selection in
[`Emscripten.props`](Emscripten.props) and the validation in
[`Emscripten.targets`](Emscripten.targets).

## 1xx selection

With `DotNet1xxWorkloadManifestVersion` empty, `EmscriptenPackageVersion` is the
standalone producer's `MicrosoftNETRuntimeEmscriptenInternalPackageVersion`.
The current Emscripten manifest is generated locally and uses this version for
its raw SDK, Cache, Node, and Python pack references. Its own package/version
remains SDK/workload-versioned. Historical manifests retain their existing pins.

The generated `data/WorkloadManifest.json` is also the upstack version contract:
all four pack entries record the selected raw package version, and their
`alias-to` IDs record the Emscripten family. No additional SDK-produced version
transport package is needed.

## Upstack consumption

With `DotNet1xxWorkloadManifestVersion` nonempty, the
[current manifest project](Manifests/Microsoft.NET.Workload.Emscripten.Current.Manifest/Microsoft.NET.Workload.Emscripten.Current.Manifest.proj)
downloads the selected 1xx manifest during ordinary restore. Its package ID uses
the same feature-band and prerelease suffix calculation as manifest production in
[`WorkloadManifestPackage.props`](Manifests/WorkloadManifestPackage.props).
The selected manifest's package/version is already known before restore; none
of its contents are needed to determine another restore input.

During manifest generation, `ResolveProjectReferences` builds `sdk-tasks`, and
[`ReadEmscriptenManifest.cs`](../Tasks/sdk-tasks/ReadEmscriptenManifest.cs) reads
the selected pack version.
The task is registered through
[`sdk-tasks.InTree.targets`](../Tasks/sdk-tasks/sdk-tasks.InTree.targets).
Manifest parsing uses
`WorkloadManifestReader.ReadWorkloadManifest` and its typed pack/alias model,
not a separate JSON parser or inline task compiler. The parser, model sources,
and resources are linked into
[`sdk-tasks.csproj`](../Tasks/sdk-tasks/sdk-tasks.csproj). There is no reader
package dependency or reader-project bootstrap dependency, including in
source-only builds. Ordinary and static-graph restore do not compile or invoke
the reader.
The reader checks the manifest version, all four pack versions, package-ID family,
and required Windows aliases. Missing packages, malformed data, inconsistent
versions, and a different family are errors. There is no fallback to an independent
emsdk pin or the runtime version.

Manifests are still generated locally, like 10.0, with the existing
`DotNet1xxWorkloadManifestVersion` package/version override. The selected 1xx
manifest is read only as an input; it is not copied over the locally generated
manifest. Existing upstack manifest publication rules remain unchanged.

## Windows workload MSI modes

[`VSInsertion/workloads.csproj`](VSInsertion/workloads.csproj) retains SDK-owned
1xx MSI generation. Upstack installer builds use `DownloadWorkloadMsis=true`:

| Mode | Acquisition and staging |
| --- | --- |
| 1xx, `DownloadWorkloadMsis != true` | Restore raw runtime packs at the runtime version and raw Emscripten packs at `EmscriptenPackageVersion`; stage them for the existing SDK MSI/SWIX generation. |
| Upstack, `DownloadWorkloadMsis == true` | Restore and stage runtime payload MSIs at `DotNet1xxRuntimeVersion` and manifest MSIs at `DotNet1xxWorkloadManifestVersion`. Do not download or stage raw Emscripten packages or Emscripten payload-MSI packages. MSI/SWIX generation remains disabled. |

All eight required current Windows raw Emscripten packs must exist before 1xx
raw staging. The 1xx-produced payload MSIs retain the raw pack's version.
The eight Emscripten payload-MSI copies previously staged by upstack are removed
from its internal artifact set; they were not consumed by SDK installer generation.
Manifest MSIs remain required: the Windows SDK installation bundle embeds them.
Downloaded MSIs and locally generated upstack manifests retain their existing
internal publication visibility.

## Stabilization and rollout

`Emscripten.Internal` is always prerelease. Validation rejects an effective
prerelease Emscripten version when the SDK label is `rtm`, `servicing`, or
`hotfix`, including upstack manifest generation and 1xx raw restore. Stable
Emscripten versions are accepted; never synthesize one by stripping a suffix.

Before stabilization, emsdk must expose a stable package-version property for
the current family (historically the
`Microsoft.NET.Runtime.Emscripten.3.1.56.Cache.win-x64.Msi.x64` property).
Change 1xx selection in `Emscripten.props` using `EmscriptenVersionCurrent`;
manifest references, 1xx raw acquisition/staging, and payload-MSI generation must advance
together. Upstack inherits that selection from the updated 1xx manifest.
Remove the temporary guard only after that transition.

This implementation does not change dependency subscriptions, standalone
producer/promotion configuration, or checked-in SDK dependency pins.
Upstack continues using the existing 1xx workload manifest selection; no new
dependency-flow asset needs onboarding. No unavailable stable producer property
is fabricated here.
