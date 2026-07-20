---
applyTo: "src/Microsoft.TestPlatform.Extensions.BlameDataCollector/**"
---

# Blame Data Collector — Dump Reliability Rules

This collector captures crash and hang dumps. It operates under extreme conditions (process dying, concurrent crashes) and must remain robust.

## Crash & Hang Dump Reliability

- Handle concurrent crash and hang scenarios without corruption — a crash during hang-dump is a real scenario.
- Detach the dumper from the testhost process before testhost termination.
- Session-end must not interrupt an in-progress dump write.
- Use agent-managed temporary storage; ensure dump file paths are valid and writable on all platforms.

## Environment Variable Contracts

- Every env-var switch must have documented ownership, precedence, and side-effect contract.
- Follow `VSTEST_DISABLE_*` (opt-out) or very rarely `VSTEST_OPTIN_*` (opt-in) naming conventions for new flags.
- Variable propagation from console → testhost → child processes must be verified.

## Diagnostic Clarity

- When dump collection fails, report WHY — don't just swallow the error.
- Collect dumps with a helper process matching the target process bitness (x86 vs x64).
- Prefer narrowly scoped dump collection in shared CI environments.

## Key Checks

- Timeout configurations must have sensible defaults and be documented.
- Validate on Windows and Linux — dump APIs differ significantly between platforms.
- Confirm that blame changes don't kill slow-starting hosts or confuse early exits with connection timeouts.
