# Pipeline Health — `dotnet-unified-build` (public + internal)

**Window:** 2026-07-28 09:18 UTC → 2026-07-29 12:45 UTC (~27h)
**Pipelines:** `dnceng-public/public` def **278** (public) · `dnceng/internal` def **1330** (internal)

> ⚠️ This file was regenerated at 12:45 after the working tree was reset and the original untracked copy was lost. All tables were rebuilt from live AzDO queries, so counts reflect the slightly longer window (builds that finished between 09:18 and 12:45 are now included).

## Headline

**Public is healthy at 79% (22/28). Internal is effectively broken at 6% (1/16).**

The two sides fail for completely different reasons. Public failures are ordinary PR-level build breaks plus one shared flaky pattern. Internal failures are dominated by **infrastructure and validation flakiness** — `Run Tests` task timeouts, publish-step errors, and macOS agent/process instability — not by bad source. The same commits that fail internally build fine in public verticals.

---

## Public — `dnceng-public/public` def 278

### Failed builds

| Build | Type | Source | Failure Detail |
|---|---|---|---|
| [1529266](https://dev.azure.com/dnceng-public/public/_build/results?buildId=1529266) | Rolling | `main` (schedule) | 4 unrelated legs: `LinuxBionic_Shortstack_x64` — `Unable to load shared library 'jitinterface_x64'`; `LinuxBionic_Shortstack_arm64` — `Packaging.targets(1282,5)` nuspec creation failure for `Microsoft.NETCore.ILAsm`; `OSX_x64` — `NU1801` unable to load service index for `dotnet-public` feed; `Windows_BuildTests_x64` — `CS8601` in `dotnet-watch.Tests/EvaluationResultTests.cs(74,75)` |
| [1529669](https://dev.azure.com/dnceng-public/public/_build/results?buildId=1529669) | Other PR | #8009 `akoeplinger` — Fix linux-bionic CI build breaks | `Windows_x86` — StaticWebAssets `Compression.targets(359,5)`: file lock on `Microsoft.AspNetCore.Components.Gateway` compressed asset. **Passed on rerun (1530009).** |
| [1530454](https://dev.azure.com/dnceng-public/public/_build/results?buildId=1530454) | Other PR | #7983 `dotnet-sb-bot` — Update Microsoft Reproducibility Exclusions | `Windows_x64` — same StaticWebAssets `Compression.targets(359,5)` file lock |
| [1530846](https://dev.azure.com/dnceng-public/public/_build/results?buildId=1530846) | Forward Flow | `main ← dotnet/roslyn` (#7891) | `SB_CentOSStream10_Online_MsftSdk_x64` — `finish-source-only.proj(141,5)`: **10 prebuilt packages detected** (`Microsoft.Build*` 18.7.1, `System.CodeDom` 10.0.8, `System.Security.Cryptography.Pkcs/Xml` 10.0.8, …). Real content break, not infra. |
| [1530982](https://dev.azure.com/dnceng-public/public/_build/results?buildId=1530982) | Forward Flow | `release/10.0.1xx ← dotnet/winforms` (#8024) | 3 jobs killed by the **240-minute job timeout**: `Android_Shortstack_arm64`, `Browser_Shortstack_wasm`, `Ubuntu2404_Ubuntu_BuildTests_x64` |
| [1530857](https://dev.azure.com/dnceng-public/public/_build/results?buildId=1530857) ⚠️ partial | Forward Flow | `main ← dotnet/aspnetcore` (#8020) | `iOSSimulator_Shortstack_arm64` — `Publish Test Results` DNS failure: `nodename nor servname provided (tcmprodcus3.vstmr.visualstudio.com:443)` |

### Summary

| Type | Completed | ✅ Pass | ❌ Fail | Pass Rate |
|---|---|---|---|---|
| Rolling | 1 | 0 | 1 | 0% |
| Forward Flow | 17 | 14 | 3 | 82% |
| Other PR | 10 | 8 | 2 | 80% |
| **Total** | **28** | **22** | **6** | **79%** |

Excludes 1 canceled build (1529431, PR #8006). `partiallySucceeded` counted as a failure.

---

## Internal — `dnceng/internal` def 1330

All in-scope internal builds are Rolling (`batchedCI` or manual on `refs/heads/*`). Personal and `dev/*` branches excluded per skill rules (`dev/veronikao/*` ×3, `mmitche/guardian-windows-x86-baseline` ×1).

### Failed builds

| Build | Type | Source | Failure Detail |
|---|---|---|---|
| [3033131](https://dev.azure.com/dnceng/internal/_build/results?buildId=3033131) | Rolling | `release/11.0.1xx-preview7` | `Windows_Pgo_arm64` — StaticWebAssets `Compression.targets(359,5)` file lock |
| [3033153](https://dev.azure.com/dnceng/internal/_build/results?buildId=3033153) ⚠️ partial | Rolling | `release/10.0.4xx` | `Validate Signing - Linux` + `Validate Signing - Mac` — SignCheck exit 1 |
| [3033154](https://dev.azure.com/dnceng/internal/_build/results?buildId=3033154) ⚠️ partial | Rolling | `internal/release/10.0.4xx` | Same two `Validate Signing` legs |
| [3033271](https://dev.azure.com/dnceng/internal/_build/results?buildId=3033271) | Rolling | `main` | `LinuxBionic_Shortstack_x64` jitinterface + `LinuxBionic_Shortstack_arm64` ILDAsm nuspec; plus `BinSkim BA2007` on `DirectWriteForwarder.dll` in `Windows_arm64` and `Windows_x86` |
| [3033330](https://dev.azure.com/dnceng/internal/_build/results?buildId=3033330) | Rolling | `release/10.0.1xx` | `Windows_Workloads_x64_BuildPass2` — `Directory.Build.targets(615,5)` MSB3073, sdk `build.cmd` failed |
| [3033331](https://dev.azure.com/dnceng/internal/_build/results?buildId=3033331) | Rolling | `internal/release/10.0.1xx` | Same `Windows_Workloads_x64_BuildPass2` MSB3073 |
| [3033367](https://dev.azure.com/dnceng/internal/_build/results?buildId=3033367) | Rolling | `release/11.0.1xx-preview7` | `SB_Fedora43_Offline_MsftSdk_Validation_x64` — `Run Tests` timed out; `Publish Using Darc` exit 1 |
| [3033373](https://dev.azure.com/dnceng/internal/_build/results?buildId=3033373) | Rolling | `main` | `OSX_arm64` — **crossgen2 exited with code 139 (SIGSEGV)** at `Microsoft.NET.CrossGen.targets(495,5)`; plus both LinuxBionic legs |
| [3033541](https://dev.azure.com/dnceng/internal/_build/results?buildId=3033541) | Rolling | `release/10.0.4xx` | `Publish ReleaseConfigs Artifact` — artifact already exists; `SB_CentOSStream10_Online_PreviousSourceBuiltSdk_Validation_x64` `Run Tests` timeout |
| [3033542](https://dev.azure.com/dnceng/internal/_build/results?buildId=3033542) | Rolling | `internal/release/10.0.4xx` | `Publish ReleaseConfigs Artifact` — artifact already exists |
| [3033555](https://dev.azure.com/dnceng/internal/_build/results?buildId=3033555) | Rolling | `release/10.0.3xx` | `Run Tests` timeouts on CentOSStream10 + AlmaLinux8 validation legs; `Publish Using Darc` exit 1 |
| [3033574](https://dev.azure.com/dnceng/internal/_build/results?buildId=3033574) | Rolling | `release/10.0.4xx` (manual) | `SB_Fedora43_Offline_CurrentSourceBuiltSdk_Validation_x64` — `Run Tests` timed out |
| [3033717](https://dev.azure.com/dnceng/internal/_build/results?buildId=3033717) | Rolling | `main` | Both LinuxBionic legs; StaticWebAssets file lock on **two** legs (`Windows_arm64`, `Windows_Pgo_x64`) |
| [3033819](https://dev.azure.com/dnceng/internal/_build/results?buildId=3033819) | Rolling | `main` | `SB_Alpine323_Offline_MsftSdk_Validation_x64` — `Run Tests` timed out |
| [3033891](https://dev.azure.com/dnceng/internal/_build/results?buildId=3033891) ⚠️ partial | Rolling | `release/11.0.1xx-preview7` | `Validate Signing - Mac` — SignCheck exit 1 |

Plus [3033556](https://dev.azure.com/dnceng/internal/_build/results?buildId=3033556) (`internal/release/10.0.3xx`) — **failed on attempt 1, succeeded on attempt 2**. See the deep dive below.

### Summary

| Type | Completed | ✅ Pass | ❌ Fail | Pass Rate |
|---|---|---|---|---|
| Rolling | 16 | 1 | 15 | 6% |
| **Total** | **16** | **1** | **15** | **6%** |

---

## Failure trends

| Pattern | Hits | Window | Status |
|---|---|---|---|
| `Run Tests` task timeout in `SB_*_Validation_*` legs | 5 builds / 6 legs | 27h, internal only | ❌ No issue filed |
| StaticWebAssets `Compression.targets(359,5)` file lock on `Components.Gateway` | 4 builds / 5 legs | 27h, both sides | ❌ No issue filed |
| `LinuxBionic_Shortstack_*` breaks (`jitinterface_x64` load failure + `Packaging.targets` nuspec) | 4 builds | 27h, both sides | ⏳ Fix in progress — [dotnet/dotnet#8009](https://github.com/dotnet/dotnet/pull/8009) *Fix linux-bionic CI build breaks* |
| `Publish Assets` stage failures (`Publish Using Darc` exit 1 ×2, `ReleaseConfigs artifact already exists` ×2) | 4 builds | 27h, internal only | ❌ No issue filed |
| `Validate Signing` SignCheck exit 1 (DO-NOT-SIGN violations) | 3 builds | 27h, internal only | 🔄 Known — [dotnet/dotnet#7943](https://github.com/dotnet/dotnet/issues/7943) |

### Lower-frequency findings

- **macOS process instability (2 distinct signals, 2 builds).** `crossgen2` exited **139** (SIGSEGV) on `OSX_arm64` in 3033373; MicroBuild signing exited **138** (SIGBUS) on `OSX_x64` in 3033556 attempt 1. Both are signal kills on mac agents. See deep dive.
- **`Windows_Workloads_x64_BuildPass2` MSB3073** hit *both* 10.0.1xx branches (3033330 public-mirror, 3033331 internal) — same commit content, so this is a genuine build break rather than flake.
- **240-minute job timeout** killed 3 legs in public 1530982 (`Android_Shortstack_arm64`, `Browser_Shortstack_wasm`, `Ubuntu2404_Ubuntu_BuildTests_x64`) — a whole-build loss from queue/agent slowness.
- **BinSkim `BA2007`** on `lib/net11.0/DirectWriteForwarder.dll` in 3033271 (`Windows_arm64`, `Windows_x86`).
- **Prebuilt packages in source-only** (public 1530846, roslyn forward flow #7891) — 10 prebuilts including `Microsoft.Build* 18.7.1`. Content issue for the flow PR to resolve.
- **Transient network**: `NU1801` service-index load failure (1529266 `OSX_x64`), and a `Publish Test Results` DNS failure (1530857).

## Recommended actions

1. **Investigate the `Run Tests` timeout epidemic first** — it is the single largest internal failure source and it is not tracked. Six legs across five builds and four different distros points at the harness or agent contention, not any one test.
2. **File an issue for the StaticWebAssets compression file lock.** It reproduces on both public and internal, on x86/x64/arm64, always on the same `Components.Gateway` asset — that shape says concurrent writers, not a machine problem.
3. **Let #8009 land** and re-check the LinuxBionic legs; they should clear on their own.
4. **`Publish Assets` needs attention on the release branches** — `ReleaseConfigs artifact already exists` implies a retry/idempotency bug in the publish step.
5. **Track the macOS signal kills.** See the deep dive for the specific evidence and issue triage.

---

## Deep Dive: `internal/release/10.0.3xx` (build 3033556)

### Branch history

Only 1 of the last 12 builds on `internal/release/10.0.3xx` had passed before this one; the branch had been red since 3032684 (07-27).

### Attempt 1 — two macOS legs, two different failures

Every Windows, Linux, and source-build leg passed. Only the two mac legs failed.

**`OSX_x64` — MicroBuild signing killed by SIGBUS.**

```
Sign.proj(76,5): error : Failed to execute MSBuild on 'Round0-Sign.proj' with exit code '138'
```

Exit **138** decodes as `128 + 10` = **SIGBUS** on macOS. The build artifact confirms the process died mid-write:

| Artifact path (`OSX_x64_BuildLogs_Attempt1`) | Size |
|---|---|
| `artifacts/log/Release/roslyn/SigningRound0.binlog` | **0 bytes** |
| `artifacts/log/Release/msbuild/SigningRound0.binlog` | 16,968 bytes |

A 0-byte binlog means MSBuild never got to flush its log — consistent with a signal kill, and inconsistent with an ESRP rejection or a certificate/config problem (those produce a complete binlog with errors in it). The msbuild repo's signing round in the *same leg* succeeded, which rules out MicroBuild setup, credentials, and cert state as causes.

**`OSX_arm64` — agent lost.**

```
We stopped hearing from agent Azure Pipelines 93
```

after 6h16m.

### Assessment

Both mac failures are environmental, not content. roslyn is the **first** repo built in the mac vertical (`roslyn.log` shows `Time Elapsed 00:10:09`), so the SIGBUS landed only ~11.5 minutes into `Build` — nothing about the source had a chance to be special.

### Existing issue triage

| Symptom | Issue | Verdict |
|---|---|---|
| "We stopped hearing from agent" on OSX | [dotnet/dotnet#3888](https://github.com/dotnet/dotnet/issues/3888) (open) | ✅ Exact match. Dormant since 2026-01-23. Comments from `mmitche` / `ViktorHofer` note it hits only *long* mac legs; disk and memory were ruled out. |
| MicroBuild signing exit 138 / SIGBUS | — | ❌ No existing issue. Ruled out #3340 (Linux, exit 1), #1063 (slowness), #5781 (exit 137 / OOM). |
| SignCheck DO-NOT-SIGN violations | [dotnet/dotnet#7943](https://github.com/dotnet/dotnet/issues/7943) (open) | ✅ Covers the `Validate Signing` trend above. |

### Recommended action

The agent-loss half is already tracked by #3888. The SIGBUS signing crash is not, and #3888's thread lacks a decoded signal — adding the exit-138 decode plus the 0-byte-binlog evidence there would be the highest-value contribution, since both symptoms correlate with long-running mac legs and may share a root cause.

### Retry outcome — confirmed non-deterministic ✅

@dkurepa retried the failed jobs at 07:41.

The `VMR Vertical Build` stage went from failed (attempt 1) to **succeeded** (attempt 2, 07:41 → 09:57), with zero failed records in the attempt. Same source commit (`940ff25eecd456b49489b4b42f80aa4a2d58309d`) — the identical inputs built and signed cleanly the second time.

**Build 3033556 completed `succeeded` at 10:35:54.** All four attempt-2 stages passed, with no failed or `succeededWithIssues` tasks anywhere in the attempt:

| Stage | Result |
|---|---|
| VMR Vertical Build | ✅ succeeded |
| Publish Assets | ✅ succeeded |
| VMR Validation | ✅ succeeded |
| VMR Source-Only Validation | ✅ succeeded |

Notably `Publish Assets` passed here — that is the same `Publish Using Darc` / `ReleaseConfigs` step that failed on the sibling `release/10.0.3xx` (3033555) and both 10.0.4xx builds, which supports reading those as transient publish/promotion failures rather than a persistent misconfiguration.

**`internal/release/10.0.3xx` is now green for the first time since 07-27.** Per @dkurepa's stated condition — create an issue only if the retry failed again — **no issue is required.**

---

## Methodology

**Data sources**

- Build lists: `GET /_apis/build/builds?definitions={id}&minTime={iso}&statusFilter=completed&$top=200&api-version=7.1`, run separately per project. In-progress builds require a second call with `statusFilter=inProgress,notStarted` — the API rejects combining it with `completed`.
- Per-build failures: the build **timeline** endpoint, filtered to `type == 'Task'` and `result in (failed, succeededWithIssues)`, with each task's `parentId` resolved to its Job record to attribute the failure to a leg.
- Task logs pulled from each record's `log.url` and grepped for `error` / `##[error]`.
- Build artifacts (`{Leg}_BuildLogs_Attempt{N}`) downloaded for the signing deep dive.

**Auth.** `az account get-access-token --resource 499b84ac-1321-427f-aa17-267ca6975798`. Tokens last about an hour. When expired, AzDO returns **HTTP 203 with an HTML sign-in page** rather than a 401 — `Invoke-RestMethod` does not throw, it silently returns HTML, so detect it by checking `Content-Type` for `json` on an `Invoke-WebRequest` response instead of relying on try/catch.

**Classification.** Public builds classified by trigger and PR author via `gh pr view <n> --json author,baseRefName`: `refs/heads/*` + `schedule`/`batchedCI` → Rolling; PR authored by `app/dotnet-maestro` → Forward Flow; everything else → Other PR. Internal `dev/*` and personal branches excluded. `partiallySucceeded` counted as a failure; `canceled` excluded from totals.

**Signal decoding.** Non-zero exit codes above 128 decode as `128 + signum`: 137 = SIGKILL (OOM), 138 = SIGBUS, 139 = SIGSEGV. This is what separated the crossgen2 and signing crashes from ordinary tool errors.

**Signing-failure heuristic (reusable).** For any MicroBuild signing failure, pull the per-leg `{Leg}_BuildLogs_Attempt{N}` artifact and compare `artifacts/log/Release/{repo}/SigningRound0.binlog` across repos. A **0-byte** binlog proves the process was killed before it could flush, and a sibling repo's healthy binlog in the same leg acts as a control that rules out ESRP, certificate, and MicroBuild configuration causes.

**Cascade handling.** `Download Previous Build` / "artifact not found" failures were treated as downstream symptoms and traced back to the first failing leg rather than reported on their own.
