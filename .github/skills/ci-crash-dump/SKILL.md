---
name: ci-crash-dump
description: >
  Download and debug crash dumps from CI test failures in dotnet repositories.
  Use when a CI test crashed (not just failed), when the user wants to debug a crash dump
  from a PR or build, or when asked "debug dump", "download dump", "crash dump from CI",
  "test crashed", "analyze crash in PR", or "why did the test crash".
  DO NOT USE FOR: test failures that are not crashes (use ci-analysis),
  build failures, performance analysis, or analyzing dumps you already have locally.
---

# CI Crash Dump Analysis

Dotnet repositories run tests on a distributed test infrastructure called Helix. When a test
process crashes, Helix captures a dump file and publishes it as an artifact. This skill
covers finding those artifacts, downloading them, and analyzing the dump.

## When to Use

- A CI test crashed (not just failed with assertion errors)
- User wants to debug a dump from a PR or build

## When Not to Use

- Test failed but didn't crash (normal assertion failure) — use `ci-analysis`
- User already has a dump file locally
- Build failures (no test execution occurred)

## Step 1: Identify the Crashed Work Item

**If pointed at a PR**, use this plugin's `ci-analysis` skill to find failing Helix jobs.
The `ci-analysis` skill provides `Get-CIStatus.ps1`:
```
../ci-analysis/scripts/Get-CIStatus.ps1 -PRNumber <PR> -Repository "dotnet/runtime" -ShowLogs
../ci-analysis/scripts/Get-CIStatus.ps1 -BuildId <BuildId> -ShowLogs
```

**If pointed at an issue** (not a PR), look at the issue body and comments for linked AzDO
build URLs. Multiple builds may be listed — start with the most recent, as older builds may
have expired from AzDO retention policies. Pass its build ID to
`../ci-analysis/scripts/Get-CIStatus.ps1`.
There is no associated PR in this scenario — skip PR correlation in your analysis.

> **Stacks already in the issue/PR:** The issue or PR may already contain a pasted stack trace.
> Do not simply repeat it — always perform your own independent analysis from the console log
> and/or dump. You may get better symbol resolution, find additional threads, or identify
> details the original poster missed. Use any existing stacks as a cross-reference, not a
> substitute.

Crashes are reported at the work item level. Look for work items that have dump files in
their artifacts. Note that even individual test-name failures can be crashes (not just
assertion failures) — for example, in dotnet/runtime test crashes are often attributed to
individual tests. A single work item may also contain multiple crashes (e.g., tests using
`RemoteExecutor` or process isolation). A PR may have many failures — look specifically
for work items with dump files. If multiple work items crashed, list them and ask the user
which one to investigate.

## Step 2: Check the Console Log First

Before downloading any dump files, check the work item's console log.
`../ci-analysis/scripts/Get-CIStatus.ps1 -ShowLogs`
reports the `ConsoleOutputUri`, or find it in the Helix work item Details response.
In **dotnet/runtime**, the crash handler runs `cdb` (or equivalent) on the machine before
uploading, so the console log may contain symbolicated native stacks (`~*k`) and
managed stacks (`!clrstack -all`). Other repos may not have this — the console log may only
show test output. If crash stacks are present and symbols are resolved (function names, not
just hex addresses), use them as a starting point for analysis. However, stacks alone may
not be sufficient — for corruption, heap issues, or cases where managed exceptions are
caught by the test framework (common in libs tests using xunit), downloading and analyzing
the dump provides deeper insight. If the stacks are missing, truncated, or show only
unresolved addresses, proceed directly to download the dump.

## Step 3: Query the Work Item for Crash Evidence

Query the Helix API for work item details:
```
GET https://helix.dot.net/api/2019-06-17/jobs/{jobId}/workitems/{workItemName}
```

> The work item name often contains spaces, parentheses, or other special characters.
> URL-encode it (e.g., `[uri]::EscapeDataString($workItemName)` in PowerShell) or the
> request will 404.

The response includes `ExitCode` and a `Files` array (each with `FileName` and `Uri`).
However, the `Files` URIs from the Details endpoint can be broken for subdirectory or Unicode
filenames. To get reliable download URIs, use the separate ListFiles endpoint:
```
GET https://helix.dot.net/api/2019-06-17/jobs/{jobId}/workitems/{workItemName}/files
```

**Crash vs. normal failure:** Crashes have a negative or large `ExitCode` and crash artifacts
in the `Files` array: `.dmp` files (Windows) or `.crashreport.json` files (macOS/Linux).
Normal failures have `ExitCode: 1` and no crash artifacts.
**If there are no dump or crashreport files, stop here** — this is a normal test failure,
not a crash. Report the failure details to the user and suggest using the `ci-analysis`
skill instead.

> **`.crashreport.json` files** are generated on macOS and Linux (there is no Windows
> equivalent). They contain full native call stacks and are often the most useful starting
> point for crash analysis — check these first before attempting to load the dump.

Common crash exit codes:

| Exit code | Meaning | Platform |
|-----------|---------|----------|
| `-1073740771` (`0xC000041D`) | Process abort | Windows |
| `-1073741819` (`0xC0000005`) | Access violation | Windows |
| `-532462766` (`0xE0434352`) | CLR unhandled exception | Windows |
| `134` (128+6) | SIGABRT | Linux/macOS |
| `139` (128+11) | SIGSEGV | Linux/macOS |

## Step 4: Download Artifacts

> **Check the console log first** (Step 2). If it already contains the crash stacks, you may
> not need to download the dump at all. Only proceed with download if the console log doesn't
> have sufficient detail.

Download files using the ListFiles endpoint URIs. Start with `.crashreport.json` files
(contain stack traces, especially useful for macOS) and `.dmp` files — these are directly
downloadable and often sufficient for initial analysis without needing the full payload.

> **Duplicate dumps:** Crashes often produce multiple dump files for the same crash.
> On Windows you may see e.g. `dotnet.exe.6524.dmp` and `dotnet.exe(1).6524.dmp` — one from
> Windows Error Reporting and one from `createdump`. Prefer the `createdump` variant (usually
> the `(1)` file) as it is more reliable for SOS/`dotnet-dump`. On Linux you may see e.g.
> `core.1000.33` and `coredump.33.dmp` for the same crash. Prefer the `coredump.*.dmp` file
> (produced by `createdump`) over the `core.*` file (kernel core dump). In either case, only
> analyze one and ignore the duplicate.

Download the remaining payload files (runtime binaries, test binaries) only if you need to
load the dump in a debugger. Do not use `runfo get-helix-payload` unless you actually need
the full payload — it downloads everything including large runtime binaries. If the `Files`
URIs are inaccessible (expired, 403, etc.) and you do need the payload, fall back to
[runfo](https://github.com/jaredpar/runfo): `runfo get-helix-payload -j <jobId> -w <workItem> -o <dir>`.

> **Internal Helix jobs** (identified by the org `dnceng` rather than `dnceng-public` in URLs,
> or when the Helix API returns 401/403) require authentication that the agent does not have.
> Report the job ID and work item name to the user and ask them to download manually.

Extract any archive files (`.zip`, `.tar.gz`) in the downloaded payload.

## Step 5: Debug the Dump

The dump needs matching runtime binaries (DAC, SOS) from the payload at
`shared/Microsoft.NETCore.App/<version>/`.

> **`dotnet-dump` version must match the runtime version of the dump.** A .NET 9.0
> `dotnet-dump` cannot load a .NET 11.0 DAC (fails with `0x80004002` or
> `No CLR runtime found`). `dotnet-dump` is backwards compatible, so the simplest
> approach is to install the latest version:
> `dotnet tool install -g dotnet-dump --prerelease` (or `dotnet tool update -g dotnet-dump --prerelease`).
> This usually comes from the `dotnet-tools` feed and works for all supported runtime versions.
>
> **If a matching dotnet-dump version is not available** (common for unreleased .NET versions
> where no package exists on NuGet or dev feeds), **skip dotnet-dump and use `cdb` instead**
> (see "Native crashes on Windows" below). `cdb` does not have the DAC version coupling
> problem — its `!analyze -v` and `kn` commands produce native + managed stacks without
> needing a matching SOS/DAC version.
>
> **When DAC loading fails**, run `modules -v` inside `dotnet-dump analyze` as a first
> diagnostic step. This command works without the DAC and shows the full paths of all loaded
> modules, including the exact `coreclr.dll` and `System.Private.CoreLib.dll` that were in
> use. This tells you which runtime build produced the dump and where to find the matching
> DAC. Use `setclrpath` to point to that directory.

Determine the dump's platform from the CI job name (e.g., "windows-x64", "linux-arm64").

### OS compatibility

`dotnet-dump` can analyze managed state cross-platform. Native debuggers require a matching OS.

| Dump OS | Agent on Windows | Agent on Linux | Agent on macOS |
|---------|-----------------|----------------|----------------|
| Windows | ✅ `dotnet-dump`, `cdb` | ⚠️ `dotnet-dump` managed-only | ⚠️ `dotnet-dump` managed-only |
| Linux | ⚠️ `dotnet-dump` managed-only (needs Cross DAC — see below) | ✅ `dotnet-dump`, `lldb` | ⚠️ `dotnet-dump` managed-only |
| macOS | ⚠️ Use `.crashreport.json` stacks only | ⚠️ Use `.crashreport.json` stacks only | ✅ `lldb` |

> **Cross DAC for Linux dumps on Windows**: `dotnet-dump` needs the cross-DAC binaries to read
> Linux ELF core dumps. Search the AzDO build artifacts for names containing `CrossDac` (the
> exact artifact name varies by build — e.g., `CoreCLRCrossDacArtifacts`). Copy the matching
> architecture's binaries into the runtime dir alongside the payload. If no cross-DAC artifact
> is found, report this to the user.
>
> **macOS Mach-O core dumps** generally cannot be loaded by `dotnet-dump` even with cross-DAC.
> The `.crashreport.json` files from the Helix `Files` array contain full native stacks and
> are the primary analysis path for macOS crashes on non-macOS agents.

If the agent cannot fully analyze the dump (OS mismatch), report the crash type, exit code,
dump file path, and runtime binaries path, and suggest the user debug manually.

### Managed crashes

Use `dotnet-dump analyze`. The critical Helix-specific setup:
- `setclrpath` — point to the runtime binaries from the payload
- `setsymbolserver -directory` — same path, for symbols
- `setsymbolserver` (no args) — also enables the Microsoft public symbol server for OS and framework symbols

Start with `pe` (print exception) and `clrstack -all`. See [SOS command reference](https://learn.microsoft.com/dotnet/core/diagnostics/sos-debugging-extension) for further commands.

### Native crashes on Windows

Use `cdb.exe` (command-line debugger) for native crashes, or as a **fallback when
`dotnet-dump` cannot load the DAC** (e.g., unreleased .NET versions). `cdb` does not depend
on SOS/DAC version matching for native stacks and `!analyze -v`.

`cdb.exe` may be at `C:\Program Files (x86)\Windows Kits\10\Debuggers\x64\cdb.exe`
(Windows SDK) or inside the WinDbg MSIX package (`winget install --id Microsoft.WinDbg`).
To find it in the MSIX package:
```powershell
$pkg = Get-AppxPackage -Name "*WinDbg*"
$cdb = Join-Path $pkg.InstallLocation "amd64\cdb.exe"
```

> **MSIX sandbox limitation:** `cdb.exe` from the WinDbg MSIX package may not be able to
> access files at arbitrary paths (e.g., `C:\dumps`). If it reports "file not found" for a
> dump that exists, copy the dump to `$env:TEMP` and open it from there.

Set up the Microsoft public symbol server: `.symfix+ c:\symbols`. Key commands: `!analyze -v` (automatic crash analysis), `kP` / `~*kP` (native stacks).
For mixed native+managed: `.loadby sos coreclr`, then `!setclrpath`, `!pe`, `!clrstack`.

### Native crashes on Linux/macOS

Use `lldb` with the SOS plugin for combined native + managed debugging. Point it at the dump
and the dotnet host binary from the payload. Native commands: `bt` (backtrace), `bt all`
(all threads), `frame variable` (locals). After loading the SOS plugin, use `setclrpath` /`setsymbolserver`, then `pe`, `clrstack -all`
for managed state — these SOS commands work inside `lldb` the same as in `dotnet-dump`.

For native symbol resolution: runtime binaries in CI are stripped (only `.dynsym` exports).
Use `dotnet-symbol --host-only --debugging <path-to-libcoreclr.so>` to download the matching
`.dbg` files, which resolve internal function names. Without these, native frames show only
as offsets between exported symbols.

Setup: [LLDB for .NET](https://github.com/dotnet/diagnostics/blob/main/documentation/lldb/linux-instructions.md).

### NativeAOT crashes

The CI job name will contain "NativeAOT" (e.g., "osx-arm64 Release NativeAOT_Libraries").
SOS does not work with NativeAOT. Use `cdb` or `lldb` directly.

## After Loading the Dump

**Always include the following in your report to the user:**
- A **link to the PR or build** that was analyzed
- The **exit code** and its meaning (e.g., `0xC0000005` = access violation)
- The **crashing thread's call stack** (and any other relevant threads), with symbols resolved
  as far as possible — this is the most important output
- Any **managed exception** type, message, and inner exception stack (use `pe`) if different
  from the native crash stack
- **Links to existing issues** that match this crash — search the repo for open issues (and
  also closed issues) with matching crash signatures, stack frames, or error messages. If
  found, link them so the user can see prior context and whether this is a known problem.

Without these the user cannot verify your interpretation or dig deeper.

Use the backtrace, exception info, heap state, etc. together with the PR's code changes to
understand *why* the crash happened. Correlate the crash location with what was changed — a
crash in code touched by the PR is likely caused by it; a crash elsewhere may be pre-existing
or a side effect.

If the crash is in a native binary not part of the PR, report its version metadata
(`lm v m <module>` in cdb, `image list <module>` in lldb) — the version helps identify
which build or package introduced it.

Use your judgement and knowledge of the codebase to form a diagnosis and suggest a fix.

## Common Pitfalls

- **Helix artifacts expire after 20 days.** If downloads fail with 404, the artifacts have likely expired — tell the user.
- **AzDO builds also expire** due to retention policies. If a build returns "not found", try a more recent build. When multiple builds are listed (e.g., in an issue), start with the newest.
- **`dotnet-dump` only handles managed state.** For native crashes, use `cdb`/`lldb` on matching OS.
- **32-bit dumps on 64-bit OS:** Use 32-bit dotnet SDK to install dotnet-dump.
- **Mobile/WASM dumps** are not covered — report the dump location and hand off.
- **Internal jobs** (`dnceng` org) require auth the agent doesn't have — report and hand off.

## Further Reading

- [dotnet-dump](https://learn.microsoft.com/dotnet/core/diagnostics/dotnet-dump)
- [SOS debugging extension](https://learn.microsoft.com/dotnet/core/diagnostics/sos-debugging-extension)
- [Debugging .NET core dumps](https://github.com/dotnet/diagnostics/blob/main/documentation/debugging-coredump.md)
