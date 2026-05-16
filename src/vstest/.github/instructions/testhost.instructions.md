---
applyTo: "src/testhost*/**"
---

# Testhost — Assembly Loading & Host Resolution Rules

Testhost processes execute user tests. Assembly loading correctness and framework resolution are the primary concerns here.

## Assembly Loading & Resolution

- Keep `testhost.deps.json` dependency versions aligned with assemblies actually shipped in the CLI package.
- Prefer bin-directory resolution over deps.json parsing for speed and correctness.
- TypeLoadException from version mismatches must produce diagnostic output identifying the missing assembly.
- Test deps.json edge cases: self-contained apps, single-file publish, RID-specific native assets.

## Framework & Architecture Resolution

- Use `COREHOST_TRACE` environment variables to inspect how the .NET host resolved the testhost.
- When older projects fail host resolution under newer tooling, emit actionable guidance (install/update Microsoft.NET.Test.Sdk).
- Architecture must match between testhost and test assemblies — validate on x86, x64, and ARM64.

## Cross-TFM Safety

- Changes must work on all targeted TFMs (net462, net8.0+).
- net462 hosts lack binding redirects in DTA scenarios — test the DtaLikeHost pattern.
- RID-specific native assets must resolve correctly on Windows, Linux, and macOS.

## Key Checks

- After dependency changes, verify `testhost.deps.json` in the built package matches shipped DLLs.
- Provide explicit host-path escape hatches when architecture detection fails on new platforms.
