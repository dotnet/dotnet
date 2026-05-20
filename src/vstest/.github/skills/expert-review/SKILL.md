---
name: expert-reviewing
description: "Route code changes to relevant review dimensions based on affected files and folders. Use when reviewing PRs, analyzing code quality, or performing targeted reviews of specific vstest subsystems."
---

# Expert Review Routing

This skill maps changed folders to review dimensions, enabling focused expert reviews of vstest PRs.

## Folder → Dimension Routing Table

| Folder | Primary Dimensions |
|--------|-------------------|
| `eng/` | Build Script & Infrastructure Hygiene, Dependency & Package Integrity, Source Build & Cross-Platform Compliance |
| `src/Microsoft.TestPlatform.CrossPlatEngine/` | Parallel Execution & Scheduling Safety, Error Reporting & Diagnostic Clarity, Process Architecture & Host Resolution |
| `src/vstest.console/` | RunSettings Validation & Inference, Process Architecture & Host Resolution, Environment Variable & Feature Flag Contracts |
| `test/Microsoft.TestPlatform.Acceptance.IntegrationTests/` | Acceptance Test Coverage Design, Cross-TFM & Framework Resolution, Parallel Execution & Scheduling Safety |
| `src/Microsoft.TestPlatform.Extensions.BlameDataCollector/` | Crash & Hang Dump Reliability, Error Reporting & Diagnostic Clarity, Environment Variable & Feature Flag Contracts |
| `src/testhost/` | Testhost Assembly Loading & Resolution, Cross-TFM & Framework Resolution, Process Architecture & Host Resolution |
| `src/Microsoft.TestPlatform.CommunicationUtilities/` | IPC Transport & Protocol Stability, Error Reporting & Diagnostic Clarity, Dependency & Package Integrity |
| `src/Microsoft.TestPlatform.ObjectModel/` | Public API Surface Protection, Backward Compatibility & Rollback Safety, Cross-TFM & Framework Resolution |
| `src/package/` | Dependency & Package Integrity, Build Script & Infrastructure Hygiene, Cross-TFM & Framework Resolution |
| `src/Microsoft.TestPlatform.Common/` | Backward Compatibility & Rollback Safety, Error Reporting & Diagnostic Clarity, Null Safety & Boundary Validation |
| `src/Microsoft.TestPlatform.VsTestConsole.TranslationLayer/` | IPC Transport & Protocol Stability, Error Reporting & Diagnostic Clarity, Backward Compatibility & Rollback Safety |
| `src/Microsoft.TestPlatform.Client/` | IPC Transport & Protocol Stability, Backward Compatibility & Rollback Safety, Public API Surface Protection |
| `src/datacollector/` | Crash & Hang Dump Reliability, Dependency & Package Integrity, Error Reporting & Diagnostic Clarity |
| `src/Microsoft.TestPlatform.CoreUtilities/` | Process Architecture & Host Resolution, Environment Variable & Feature Flag Contracts, Error Reporting & Diagnostic Clarity |
| `src/Microsoft.TestPlatform.TestHostProvider/` | Process Architecture & Host Resolution, Environment Variable & Feature Flag Contracts, Testhost Assembly Loading & Resolution |

## Quick Reference: Dimension Summaries

| Dimension | Focus |
|-----------|-------|
| Dependency & Package Integrity | Version conflicts, transitive deps, package content correctness |
| Cross-TFM & Framework Resolution | Multi-TFM builds, binding redirects, framework-specific behavior |
| Process Architecture & Host Resolution | x86/x64/ARM64 selection, DOTNET_ROOT propagation, muxer resolution |
| Parallel Execution & Scheduling Safety | Race conditions, lock scopes, worker slot assignment, cancellation |
| IPC Transport & Protocol Stability | Wire compatibility, connection timeouts, protocol versioning |
| Crash & Hang Dump Reliability | Dump capture under compound failures, detach timing, file validity |
| Error Reporting & Diagnostic Clarity | Actionable messages, trace formatting, exception propagation |
| Environment Variable & Feature Flag Contracts | Naming conventions, propagation, stable contract semantics |
| RunSettings Validation & Inference | Defensive XML parsing, defaults, source precedence |
| Backward Compatibility & Rollback Safety | Disable flags, version-guarded behavior, migration paths |
| Public API Surface Protection | Unintentional exposure, PublicAPI.txt management, interface evolution |
| Acceptance Test Coverage Design | Focused matrices, OS coverage, measurement-backed claims |
| Testhost Assembly Loading & Resolution | deps.json alignment, RID assets, TypeLoadException handling |
| Build Script & Infrastructure Hygiene | Exit codes, cross-platform scripts, source-build compatibility |
| Null Safety & Boundary Validation | Boundary checks, nullable annotations, defensive parsing |
| Source Build & Cross-Platform Compliance | Source-build mode, Linux/macOS paths, shell compatibility |

## Common Review Scenarios

**PR touches `src/Microsoft.TestPlatform.CrossPlatEngine/`**
→ Activate: Parallel Execution & Scheduling Safety, Error Reporting & Diagnostic Clarity, Process Architecture & Host Resolution

**PR modifies `eng/` scripts or build infrastructure**
→ Activate: Build Script & Infrastructure Hygiene, Dependency & Package Integrity, Source Build & Cross-Platform Compliance

**PR changes `ObjectModel` public API**
→ Activate: Public API Surface Protection, Backward Compatibility & Rollback Safety, Cross-TFM & Framework Resolution

**PR updates `CommunicationUtilities` or `TranslationLayer`**
→ Activate: IPC Transport & Protocol Stability, Backward Compatibility & Rollback Safety, Error Reporting & Diagnostic Clarity

**PR modifies blame/datacollector components**
→ Activate: Crash & Hang Dump Reliability, Environment Variable & Feature Flag Contracts, Error Reporting & Diagnostic Clarity

**PR adds or bumps package dependencies**
→ Activate: Dependency & Package Integrity, Cross-TFM & Framework Resolution, Backward Compatibility & Rollback Safety

**PR touches testhost or TestHostProvider**
→ Activate: Testhost Assembly Loading & Resolution, Process Architecture & Host Resolution, Cross-TFM & Framework Resolution

**PR modifies RunSettings handling in vstest.console**
→ Activate: RunSettings Validation & Inference, Environment Variable & Feature Flag Contracts, Backward Compatibility & Rollback Safety

## Integration with @expert-reviewer

This skill provides the **routing configuration** that tells the expert-reviewer agent which dimensions to activate for a given PR. The agent owns the full CHECK methodology and review logic.

**Invocation:** Reference `@expert-reviewer` in a PR comment or use the `pr-expert-reviewer` workflow.

**How routing works:**
1. Agent identifies changed files/folders in the PR
2. This skill's routing table maps folders to applicable dimensions
3. Agent activates the matched dimensions and applies their CHECK items
4. Agent produces findings limited to activated dimensions only

**Scope boundaries:**
- This skill: folder-to-dimension mapping, dimension summaries, scenario examples
- The agent: full CHECK items, severity classification, comment formatting, review decisions
- `vstest-build-test` skill: build/test commands (not review logic)
- `trx-analysis` skill: test result file parsing (not code review)
