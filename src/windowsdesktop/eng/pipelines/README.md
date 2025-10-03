# Pipeline Notes

This directory contains templates that compose the main Azure DevOps pipeline for WindowsDesktop.

## Runtime MSI Acquisition

The WiX bundle build depends on three runtime prerequisite MSIs being present under `artifacts/prereqs/<arch>`. Historically these were staged manually, but the CI pipeline now prepares them automatically before MSBuild executes.

Each architecture job defined in `jobs/windows-build.yml` performs the following steps prior to invoking `eng/common/cibuild.cmd`:

1. Import the internal runtime feed credentials via the `enable-internal-runtimes.yml` template, which retrieves the `dotnetbuilds-internal-container-read-token` SAS secret.
2. Pass the runtime source feed and token to Arcade (`-RuntimeSourceFeed` / `-RuntimeSourceFeedKey` and their MSBuild equivalents) so that `eng/common/build.ps1` downloads the required runtime packs on-demand.
3. Allow the bundle build to normalize the host/hostfxr/runtime MSIs into `artifacts/prereqs/<arch>` as part of the existing `StagePrereqRuntimeMsis` target execution.

During the WiX build, the `StagePrereqRuntimeMsis` target (in `src/windowsdesktop/src/bundle/Wix.targets`) detects these staged files and sets `IncludeRuntimeMSIs=true`, ensuring the bundle carries the latest runtime from Maestro.

### Local Repro

To emulate the CI behavior locally:

```powershell
$token = '<base64-encoded SAS token>'
pwsh eng/common/build.ps1 -restore -build -pack -runtimeSourceFeed "https://ci.dot.net/internal" -runtimeSourceFeedKey $token
```

The command above mirrors the CI configuration by pointing Arcade at the internal runtime feed. Provide a base64-encoded SAS token with read permissions (matching the `dotnetbuilds-internal` secret) and repeat per architecture if needed using `/p:TargetArchitecture=<arch>`.

Maintain this document if additional preparatory stages are introduced so build orchestration remains discoverable.
