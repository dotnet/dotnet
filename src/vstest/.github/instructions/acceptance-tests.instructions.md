---
applyTo: "test/Microsoft.TestPlatform.Acceptance.IntegrationTests/**"
---

# Acceptance & Integration Tests Rules

These tests exercise vstest end-to-end across runners, TFMs, and platforms. They are slow and expensive — design them carefully.

## Test Coverage Design

- Each test should exercise unique behavior, not duplicate coverage from unit tests.
- Keep matrices focused — move exhaustive compatibility coverage to specialized CI jobs.
- New platform behavior must have an automated acceptance test so regressions are caught.
- Performance claims must be backed by measurements separating platform overhead from adapter overhead.

## Cross-Platform & Cross-TFM

- Run supported scenarios on every target OS; explicitly validate Linux and macOS, not just Windows.
- Avoid APIs and runtime assumptions unavailable on some targeted frameworks or runtimes.
- Use OS-appropriate paths and focus on realistic scenarios rather than mocked impossible inputs.

## Parallel Test Safety

- Tests must not rely on timing, shared mutable state, or specific CPU availability.
- Parallel infrastructure needs synchronization covering both build and post-build phases.
- If tests share processes or state, make sharing decisions explicit and documented.

## Assertions & Diagnostics

- Assertions should verify specific behavior, not incidental output (e.g., don't assert on log line counts).
- Include enough diagnostic context in test names and failure messages to identify the scenario.

## Key Checks

- New tests must pass in CI (`-c Release`) — not just locally with roll-forward enabled.
- Don't add broad `*test*` globs that accidentally pull in infrastructure assemblies.
