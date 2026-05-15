---
applyTo: "src/Microsoft.TestPlatform.CrossPlatEngine/**"
---

# CrossPlatEngine — Execution Engine Rules

This is the core execution engine handling parallel scheduling, host management, and test orchestration. Thread safety and deterministic behavior are critical here.

## Parallel Execution & Scheduling

- Protect shared state accessed by parallel workers with minimal lock scopes matching actual shared-state boundaries.
- Never rely on timing or CPU availability for correctness — design lock-free or explicitly synchronized paths.
- When converting percentage-based parallelism to worker counts, round deterministically and clamp to valid bounds.
- Verify cancellation tokens are respected and don't leave orphaned work items.

## Process Architecture & Host Resolution

- Architecture inference must handle all valid combinations (AnyCPU, x86, x64, ARM64).
- For mixed-framework/mixed-architecture runs, centralize workload scheduling and preserve shared-host semantics.
- Propagate `DOTNET_ROOT` and `DOTNET_ROOT_<ARCH>` correctly to child processes.

## Error Reporting

- Include stack traces in adapter/executor failure logs so exceptions are diagnosable without reproduction.
- Log messages must identify component and method (e.g., `ProxyExecutionManager.StartTestRun: ...`).
- Never swallow exceptions silently — trace at verbose level at minimum.

## Key Checks

- New behavior must have a disable flag for rollback.
- Validate changes on all targeted TFMs — especially net462 where assembly loading differs.
- IPC protocol changes from this layer must remain backward-compatible with older testhosts.
