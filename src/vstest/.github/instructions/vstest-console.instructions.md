---
applyTo: "src/vstest.console/**"
---

# vstest.console — CLI Entry Point Rules

This is the main entry point for `dotnet test` and `vstest.console.exe`. It handles argument parsing, RunSettings inference, and host process orchestration.

## RunSettings Validation & Inference

- Missing RunSettings nodes must fall back to documented defaults, not crash.
- Invalid setting values must produce actionable errors, not silent misbehavior.
- Multiple settings sources (CLI, runsettings file, project) must have clear, deterministic precedence.
- Account for host-shell parsing differences — CLI fragments transit through dotnet and MSBuild.

## Architecture & Host Resolution

- Choose runner architecture from OS and target-framework compatibility, not the current host process alone.
- Architecture-switch fallbacks must not assume a second SDK exists — locate compatible testhosts from shipped paths.
- When inferring frameworks per source, maintain a compatibility path that reproduces old runsettings behavior.

## Environment Variable Contracts

- New env vars must follow `VSTEST_DISABLE_*` / rarely `VSTEST_OPTIN_*` naming.
- Environment inheritance to child processes must be explicit and predictable.
- Feature flags need a clear default and documented migration path before shipping.

## Key Checks

- Test with `-c Release` to match CI behavior (warnings-as-errors differ).
- Validate on all platforms — path separators and process launching differ between Windows and Unix.
