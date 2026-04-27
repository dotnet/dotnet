# XHarness Device Test Patterns

Domain knowledge for investigating XHarness test failures on iOS/tvOS devices, captured from real investigations.

## XHarness Command Structure

Typical XHarness invocation for device tests:

```
xharness apple run --target tvos-device --timeout 00:12:00 --signal-app-end --expected-exit-code 42
```

Key flags:
- `--target tvos-device` / `ios-device` — physical device (not simulator)
- `--timeout 00:12:00` — XHarness-level timeout (720 seconds)
- `--signal-app-end` — mechanism for detecting app completion
- `--expected-exit-code 42` — what the test app returns on success

## Exit Code Detection on tvOS/iOS 15+

XHarness explicitly warns:

```
warn: Exit code detection is not working on iOS/tvOS 15+
      so the run will fail to match it with the expected value
```

**This warning is often a red herring.** The real issue is typically device log streaming:

### Pass/Fail Divergence Pattern

| Aspect | Passing Machine | Failing Machine |
|--------|----------------|-----------------|
| mlaunch exit code | 42 ✅ | 42 ✅ |
| Device log file | Non-empty | **Empty** (removed) |
| XHarness reads logs | Yes | **No** — hangs waiting |
| Uses mlaunch exit code? | Yes → success | **No** — blocked on log reader |
| Result | Success in ~2 sec | **Timeout after 720 sec** |

**Root cause**: When `devicectl` can't stream console output from the device (USB/connectivity issue), XHarness blocks on the log-reading task and never reaches the fallback code path that would use `mlaunch`'s exit code.

## Timeout Chain

Device test timeouts involve multiple layers:

```
XHarness timeout (--timeout, e.g., 720s)
  ↓ if exceeded
Helix workload timeout (~21 min total)
  ↓ sends SIGTERM
Process exit code = 143
```

When you see `exit_code=143`, the timeline is:
1. App launched and exited in <1 second with expected exit code
2. XHarness didn't detect completion → waited 720 seconds
3. XHarness timed out → "Run timed out after 720 seconds"
4. Helix workload timeout reached → "WORKLOAD TIMED OUT - Killing user command"
5. SIGTERM → exit 143

## Machine Naming Conventions

| Pattern | Role |
|---------|------|
| `DNCENGMAC###` | macOS host machine |
| `DNCENGTVOS-###` | Apple TV device attached to host |
| `DNCENGIPHONE-###` | iPhone device attached to host |

A macOS host (e.g., DNCENGMAC036) has a physical device attached (e.g., DNCENGTVOS-036). If the device's log streaming is broken, ALL tests on that host/device pair will fail with timeouts.

## Failure Rate Correlation

If a test fails ~10% of the time:
- It's likely machine-specific, not a test bug
- Calculate: (broken machines in pool) / (total machines in pool) ≈ failure rate
- Example: 1 broken Apple TV out of ~10 in the pool → ~10% failure rate

## Known Issues

- [dotnet/xharness#1548](https://github.com/dotnet/xharness/issues/1548) — tvOS: Device communication failures cause false TIMED_OUT and APP_CRASH results after successful test runs
- [dotnet/runtime#60713](https://github.com/dotnet/runtime/issues/60713) — iOS/tvOS test suite timeouts on devices (open since 2021)
- [dotnet/runtime#124069](https://github.com/dotnet/runtime/issues/124069) — System.Runtime.Tests APP_CRASH on tvOS
- [dotnet/runtime#117807](https://github.com/dotnet/runtime/issues/117807) — AppleTV queue includes machines with higher OS version

## False Failure: APP_CRASH After Successful Tests

XHarness can report `APP_CRASH` (exit 80) even when **all tests passed**. This is a distinct manifestation from the timeout issue above but shares the same root cause: device communication failure.

### The Sequence

```
1. ✅ Tests launch and ALL pass (e.g., 67,000+ tests, 0 failures)
2. ✅ "Detected test end tag in application's output" — XHarness knows tests finished
3. ✅ "Test run completed" — mlaunch exits (SIGKILL, expected)
4. ❌ XHarness tries to copy result XML from device via devicectl
5. ❌ devicectl fails: "Mercury error 1000" / "RSD error 0xE8000003"
6. ❌ XHarness reports APP_CRASH (exit 80) despite knowing tests completed
```

### Two Manifestations, Same Root Cause

| | TIMED_OUT (exit 143) | APP_CRASH (exit 80) |
|---|---|---|
| **Tests pass?** | Yes | Yes |
| **XHarness knows?** | No — blocks before checking | **Yes** — logs "Test run completed" |
| **What breaks** | Device log stream empty | Device file copy fails (devicectl) |
| **Device error** | Log file 0 bytes | `Mercury error 1000` / `RSD error 0xE8000003` |

Both are tracked in [xharness#1548](https://github.com/dotnet/xharness/issues/1548). The fix: XHarness should honor its own completion detection rather than overriding the result when post-completion device communication fails.

### How to Detect

When investigating an APP_CRASH, always search the console log for:
- `"Detected test end tag"` — if present, tests actually completed
- `"Test run completed"` — confirms XHarness saw the completion
- `"Mercury error"` or `"RSD error"` — device communication failure after completion

If all three are present, it's a false failure, not a real crash.

## XML Truncation Pattern

Large test suites (e.g., System.Runtime.Tests with 67K+ tests) can trigger another false failure mode:

1. All tests pass
2. App starts writing results XML
3. App is killed mid-write (tvOS memory pressure / EXC_RESOURCE)
4. Truncated XML → XHarness can't parse → reports crash

Signal: Exit code 1, very large test count (60K+), look for truncated XML in the work item files.
