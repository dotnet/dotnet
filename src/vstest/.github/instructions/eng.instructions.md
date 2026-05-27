---
applyTo: "eng/**"
---

# Engineering Infrastructure Rules

Build scripts, packaging validation, and CI infrastructure. Changes here affect every contributor and every build.

## Build Script Hygiene

- Scripts must check exit codes of child processes and fail fast on errors.
- Use portable OS checks — scripts should work on both Windows (PowerShell) and Unix (bash).
- Remove dead code rather than carrying it forward; keep scripts canonical and current.
- Don't break source-build mode with infrastructure changes.

## Package Integrity

- Validate that dependency version changes don't conflict with assemblies shipped to end users.
- Packaging projects must ship assemblies from the current build, not stale artifacts.

## Dependency Management

- Version upgrades must state the exact package and target version in the PR description.
- Transitive dependency versions must align between deps.json and actually-shipped assemblies.

## CI & Testing Infrastructure

- Add end-to-end tests around SDK and MSBuild insertion points to catch packaging regressions.
- Cross-platform tests must use OS-appropriate paths and realistic scenarios.
- Keep test matrices focused on unique behavior; move exhaustive coverage to specialized jobs.

## Key Checks

- Verify changes work in both `Debug` and `Release` configurations.
- Changes to Arcade SDK versions require validating the full build pipeline.
