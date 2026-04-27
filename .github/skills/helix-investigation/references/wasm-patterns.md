# WASM / Browser Failure Patterns

Failure patterns specific to `browser-wasm` Helix queues. These run in Docker containers (`mcr.microsoft.com/dotnet-buildtools/prereqs:ubuntu-22.04-helix-webassembly`) on Linux AMD64.

## Work Item Naming

WASM build tests use the pattern: `WBT-{Config}-{Runtime}-ST-{TestClass}`

| Segment | Values |
|---------|--------|
| Config | `NoWebcil`, `NoWorkload`, `Workloads`, `NoFingerprint` |
| Runtime | `CLR` (CoreCLR), `Mono` |
| TestClass | e.g., `Wasm.Build.Tests.DebugLevelTests` |

## Failure Mode 1: SDK / MSBuild Errors

**Signal**: `dotnet build` or `dotnet publish` returns exit code 1. 100% failure rate across all tests in the work item.

**Example**: `System.InvalidOperationException: Unknown source type 'Framework'` in `Microsoft.NET.Sdk.WebAssembly.Browser.targets` — the `DefineStaticWebAssets` MSBuild task doesn't recognize a new sourceType value.

**Key indicators**:
- Every test in the work item fails with same assertion: `Expected 0 exit code but got 1`
- The error is in SDK targets, not test code
- `NoWorkload` config uses `dotnet-none` SDK (no WASM workload installed) — most susceptible to SDK-level breaks

**Diagnosis**: Download `testResults.xml` and look at the failure message. Then download the `.binlog` from the Helix files endpoint for MSBuild-level diagnosis.

**Root cause owner**: Typically dotnet/aspnetcore (StaticWebAssets) or dotnet/sdk depending on the failing target.

## Failure Mode 2: Playwright Timeouts

**Signal**: `System.TimeoutException: Timeout 30000ms exceeded` waiting for a Blazor UI element (e.g., `Locator("text=Counter")`).

**Key indicators**:
- The app builds and launches successfully
- Playwright connects to the browser
- Timeout occurs waiting for a specific DOM element
- Intermittent — some runs pass, some don't

**Diagnosis**: This is a browser automation timing issue, not a build failure. The Blazor app starts but doesn't render fast enough for the 30-second timeout.

**Possible causes**:
- Resource contention on the Helix machine (container CPU/memory limits)
- Blazor rendering regression (app takes longer to hydrate)
- Playwright version mismatch or browser startup delay

**Root cause owner**: dotnet/runtime (test infrastructure or Blazor runtime).

## Distinguishing Build vs Runtime Failures

When investigating a `Wasm.Build.Tests` work item:

1. **Check pass/fail ratio**: If 100% of tests fail → likely build infrastructure. If a few fail → likely runtime/timing.
2. **Check failure message**: `Expected 0 exit code but got 1` = build failure. `TimeoutException` = runtime.
3. **Check the config**: `NoWorkload` builds are more fragile because they lack the full WASM toolchain.
