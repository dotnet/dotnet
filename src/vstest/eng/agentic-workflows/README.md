# Agentic workflow state

Checked-in state owned by the agentic workflows in [`.github/workflows`](../../.github/workflows).

These files live here, rather than next to the workflows that read them, because an agent has
to be able to **commit** them. The `create-pull-request` safe output compiles with
`protect_top_level_dot_folders: true`, and gh-aw rejects any patch touching a path whose first
segment starts with `.` — so a fingerprint under `.github/` could be read by the workflow but
never updated by the agent. The write fell back to opening an issue instead, leaving the
fingerprint stale and re-triggering the same expensive agent run every week. See
[#16422](https://github.com/microsoft/vstest/issues/16422).

Any future agentic workflow that needs to commit its own state should put it here for the same
reason.

## Contents

| File | Owner | Purpose |
| --- | --- | --- |
| `known-broken-links.txt` | [`http-link-check-probe.yml`](../../.github/workflows/http-link-check-probe.yml) | Accepted set of broken absolute HTTP(S) links. |

`tmp/` is gitignored scratch for local runs of the checkers. Pipeline runs write to the
runner's temp directory instead and never touch the working tree.

## Editing

Regenerate these files; do not hand-write entries. A line that does not match the checker's
output byte for byte looks like a change to the probe, which dispatches the agent every week —
exactly the cost the fingerprint exists to avoid. Each probe run also publishes its freshly
generated set as `broken-links.txt` in the run's artifact, which you can copy over wholesale.

This is the same discipline as [`eng/expected-nupkg-file-counts.json`](../expected-nupkg-file-counts.json)
and [`eng/expected-dll-frameworks.json`](../expected-dll-frameworks.json).
