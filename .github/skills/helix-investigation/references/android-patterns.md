# Android Emulator Patterns

Domain knowledge for investigating Android emulator test failures on Helix, captured from real investigations.

## DEVICE_NOT_FOUND (Exit 81)

When XHarness can't find the expected Android emulator, the work item fails with exit code 81 (`DEVICE_NOT_FOUND`).

### Emulator Architecture

Helix machines run two emulators:
- `emulator-5554` — x86 emulator
- `emulator-5556` — x86_64 emulator (started via `systemctl start android-emulator@android_emu_86_64`)

When a test targets x86_64, XHarness calls `GetDevice()` looking for the x86_64 emulator. If only `emulator-5554` (x86) is found, the test fails with DEVICE_NOT_FOUND.

### Retry and Reboot Flow

```
Work item fails (exit 81)
  → xharness-event-processor.py catches it
  → Requests infra retry + sets reboot file
  → Retry runs on SAME machine (up to 3 attempts)
  → Machine reboots AFTER all attempts exhausted
```

**The gap**: Retries run back-to-back on the same broken machine before the reboot fires. If the emulator crashed, all 3 attempts see the same dead emulator.

You'll see this in logs as `Attempt.3` work items still failing with exit 81.

### Machine Distribution

Unlike tvOS device failures (which concentrate on specific hardware), Android DEVICE_NOT_FOUND errors are typically **spread across many machines** in the pool. This indicates a systemic emulator stability issue, not a hardware problem.

From a real investigation: 29 DEVICE_NOT_FOUND failures across 17 different machines over 2 days, with no single machine dominating.

### Diagnosis Questions

1. Is the x86_64 emulator running? Check `adb devices` output in the console log
2. Did the emulator crash? Look for emulator crash logs before the XHarness invocation
3. Are retries helping? If `Attempt.3` still fails with exit 81, the reboot-between-retries gap is the blocker
4. Is this concentrated or spread? Count failures per machine to determine machine-specific vs systemic

### Known Issues

- [dotnet/xharness#1549](https://github.com/dotnet/xharness/issues/1549) — Proposal: try emulator restart before DEVICE_NOT_FOUND exit
- [dotnet/runtime#112633](https://github.com/dotnet/runtime/issues/112633) — Android JIT test DEVICE_NOT_FOUND failures

## Common Android Test Failures

| Test | Pattern | Root Cause |
|------|---------|------------|
| System.Net.Http.Functional.Tests | XHarness TIMED_OUT (30min) | Tests too slow on emulator |
| System.Net.WebSockets.Client.Tests | `TaskCanceledException` in `ReceiveAsync` | Known flaky — [#119520](https://github.com/dotnet/runtime/issues/119520) |
| System.Security.Cryptography.Tests | DEVICE_NOT_FOUND (exit 81) | Emulator crash |
| Android.Device_Emulator.JIT tests | DEVICE_NOT_FOUND (exit 81) | Emulator crash |

## Android Helix Queue Patterns

| Queue Pattern | Platform |
|---------------|----------|
| `ubuntu.*.amd64.android.29.open` | Android API 29 emulator |
| `ubuntu.*.amd64.android.34.open` | Android API 34 emulator |

Machine names in Android pools use alphanumeric IDs (e.g., `a003B8R`, `a003B94`) rather than the `DNCENG*` naming used for Apple devices.
