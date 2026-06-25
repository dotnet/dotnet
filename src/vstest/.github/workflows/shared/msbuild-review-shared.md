---
# Shared configuration for MSBuild authoring review workflows.
#
# Imported by msbuild-quality-review.md. Keeps permissions, tools and
# safe-outputs in one place so any future trigger (slash command,
# `/review-msbuild`, etc.) can reuse the same contract.

description: "Shared configuration for MSBuild authoring review workflows"

permissions:
  contents: read
  issues: read
  pull-requests: read

tools:
  github:
    toolsets: [issues, repos]
  bash: ["git", "grep", "find", "cat", "head", "tail", "sed", "wc", "sort", "date"]

safe-outputs:
  # The scan-mode msbuild-reviewer agent self-posts. Provide a generous-enough
  # budget for a single weekly report plus an optional draft PR with safe fixes.
  create-issue:
    title-prefix: "[msbuild-quality] "
    labels: ["agentic-workflows", "Area: Engineering"]
    max: 1
    expires: 7d
  create-pull-request:
    draft: true
    title-prefix: "[msbuild-quality] "
    labels: ["agentic-workflows", "Area: Engineering"]
    max: 1
    protected-files: fallback-to-issue
  # NOTE: Consumers must also define this explicitly until workflow import/merge
  # preserves `report-as-issue: false` in compiled lock files.
  noop:
    report-as-issue: false
---

# MSBuild Quality Review

Review the MSBuild authoring quality of this repository using the
`msbuild-reviewer` agent defined at `.github/agents/msbuild-reviewer.agent.md`.

## Instructions

1. Launch the `msbuild-reviewer` agent in **`scan` mode** as a **background** task
   (`task` tool, `agent_type: "general-purpose"`, `model: "claude-opus-4.6"`,
   `mode: "background"`). In the sub-agent prompt include:
   - The string `MODE: scan` on the first line so the agent picks the correct
     operating mode.
   - The repository owner/name and the current date.
   - A reminder that the parent workflow `noop`s immediately and therefore the
     sub-agent itself is responsible for calling `create_issue` (or
     `create_pull_request` for safe auto-fixes only).
2. **Immediately after launching the background task** — do NOT wait for it to
   finish and do NOT read its result — call `noop` with a brief status message
   such as `"MSBuild reviewer launched in background. It will post the report directly."`.
   Then stop.

> **Important**: Reading the background agent result would pull its entire
> conversation (including every file it inspected) into your context. Do not
> call `read_agent` or any equivalent after calling `noop`.
