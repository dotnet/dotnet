---
name: "Build Failure Analysis"
description: >-
  Runs `./build.sh --binaryLog` on every PR; when the build fails, delegates
  to the `build-failure-analyst` agent (which queries the binlog live via the
  containerized `binlog-mcp` MCP server) to identify root causes, post a PR
  comment summarizing them, and attach inline `suggestion` blocks tied to
  the diff.

# This workflow is **advisory**, not gating:
#  - It posts an analysis comment / inline suggestions when the build fails.
#  - It does NOT mark the PR check as failing on its own (gh-aw has no
#    post-agent step hook). The repository's deterministic build gate lives
#    in azure-pipelines.yml; if you want a GitHub Actions-level required
#    check, add a separate non-agentic `build.yml` workflow alongside this
#    one and configure branch protection accordingly.

on:
  pull_request:
    types: [opened, synchronize, reopened]
    branches: [main, 'rel/*']
    # Fork PRs are skipped: the agent token would lack the
    # `pull-requests: write` scope needed by safe-outputs.
    forks: []
  workflow_dispatch:
    inputs:
      pr-number:
        description: "PR number to scope inline suggestion comments to (optional)"
        required: false
        type: string
  # Manual reruns and dispatch invocations are restricted to repository
  # contributors. (`pull_request` already gets fork-blocking by default
  # via `forks: []`.) For a slash-command rerun path on PR comments, see
  # the companion `build-failure-analysis-command.md` workflow.
  roles: [admin, maintainer, write]
  reaction: "eyes"
  # Make `pre_activation` and `activation` wait for the custom `build` job
  # defined below. Combined with the top-level `if:`, this gates the entire
  # AI agent pipeline on build failure — so transient Copilot AI flakes can
  # never surface as a red workflow check on a successful build.
  needs: [build]

# Skip activation (and therefore the agent job) when the build job reported
# success. gh-aw applies top-level `if:` to the `activation` job, which is a
# dependency of `agent`, so a skipped activation cascades into a skipped
# agent — no AI calls, no safe-output validation, no chance of a noop-loop
# from a transient AI server error on an otherwise green build.
if: needs.build.outputs.outcome == 'failure'

permissions:
  contents: read
  pull-requests: read
  copilot-requests: write

concurrency:
  group: build-failure-analysis-${{ github.event.pull_request.number || github.event.issue.number || github.ref }}
  cancel-in-progress: true

env:
  NUGET_MCP_VERSION: '1.4.3'

timeout-minutes: 30

network:
  allowed:
    - defaults
    - dotnet

imports:
  - shared/build-failure-analysis-shared.md

# Live binlog access for the agent. The image is published to MCR from
# `dotnet/dotnet-buildtools-prereqs-docker` (no auth required) and tracks
# the newest `Microsoft.AITools.BinlogMcp` preview via Renovate. The build
# job uploads the binlog as an artifact; the agent job downloads it to
# `/tmp/build.binlog` and the gh-aw MCP gateway mounts it read-only at the
# in-container path `/data/build.binlog`. The agent passes that path as
# `binlog_file` on every `binlog_*` tool call.
mcp-servers:
  binlog-mcp:
    container: "mcr.microsoft.com/dotnet-buildtools/prereqs:azurelinux-3.0-binlog-mcp-amd64"
    mounts:
      - "/tmp/build.binlog:/data/build.binlog:ro"
    allowed: ["*"]

# Custom build job that runs unconditionally on every PR. It produces the
# binlog and (on failure) uploads it — together with the raw build output
# log — as an artifact for the agent job, which queries the binlog live via
# the `binlog-mcp` MCP server. The agent pipeline only runs when this job
# reports `outcome == 'failure'` (see top-level `if:` above).
jobs:
  build:
    name: Build (for analysis)
    runs-on: ubuntu-latest
    timeout-minutes: 30
    # Mirror the workflow's `forks: []` trigger filter: skip fork PRs at the
    # build-job level too. Without this guard the build job would still run
    # for fork PRs even though the agent pipeline never runs for them.
    if: github.event_name != 'pull_request' || github.event.pull_request.head.repo.full_name == github.repository
    permissions:
      contents: read
    outputs:
      outcome: ${{ steps.build.outcome }}
      binlog-found: ${{ steps.find-binlog.outputs.found }}
      binlog-relative-path: ${{ steps.find-binlog.outputs.relative-path }}
    steps:
      - uses: actions/checkout@v4

      - name: Build with binary log
        id: build
        continue-on-error: true
        run: |
          set -uo pipefail
          ./build.sh --binaryLog 2>&1 | tee /tmp/build-output.log
          # `tee` is best-effort: rely on the build's own exit code so a
          # logging-pipeline glitch never misclassifies a green build as
          # failed (which would otherwise trigger the AI agent and
          # re-expose us to the Copilot-flake red-X bug).
          exit "${PIPESTATUS[0]}"

      - name: Locate binlog
        id: find-binlog
        if: always()
        run: |
          BINLOG=$(find artifacts/log -name '*.binlog' -type f -printf '%T@ %p\n' 2>/dev/null \
            | sort -rn | head -1 | cut -d' ' -f2-)
          if [ -n "$BINLOG" ] && [ -f "$BINLOG" ]; then
            REL=$(realpath --relative-to="$PWD" "$BINLOG")
            echo "found=true"             >> "$GITHUB_OUTPUT"
            echo "relative-path=$REL"     >> "$GITHUB_OUTPUT"
          else
            echo "found=false" >> "$GITHUB_OUTPUT"
          fi

      # Copy the (timestamped) binlog to a fixed name so the agent job can
      # download it deterministically and the gh-aw MCP gateway can mount it
      # at a stable in-container path (`/data/build.binlog`).
      # `continue-on-error: true` keeps the artifact upload step reachable
      # even if `cp` fails — the agent can then emit a "build failed, no
      # binlog" comment from the raw build output log.
      - name: Stage binlog for upload
        if: steps.build.outcome == 'failure' && steps.find-binlog.outputs.found == 'true'
        continue-on-error: true
        env:
          BINLOG_REL_PATH: ${{ steps.find-binlog.outputs.relative-path }}
        run: cp "$BINLOG_REL_PATH" /tmp/build.binlog

      # Upload everything the agent needs. Always upload when the build
      # failed (even if staging failed), so the agent gets the raw
      # build output log and can still emit a "build failed, no binlog"
      # comment.
      - name: Upload analysis artifact
        if: always() && steps.build.outcome == 'failure'
        continue-on-error: true
        uses: actions/upload-artifact@v4
        with:
          name: build-failure-analysis-data
          path: |
            /tmp/build.binlog
            /tmp/build-output.log
          if-no-files-found: warn
          retention-days: 1

# Steps that run in the agent job. Because the top-level `if:` gates
# activation on `needs.build.outputs.outcome == 'failure'`, these only run
# for failed builds — the agent never executes on a successful build and a
# transient Copilot AI flake can no longer surface as a red workflow check
# on a passing PR.
steps:
  - name: Download analysis artifact
    uses: actions/download-artifact@v4
    with:
      name: build-failure-analysis-data
      path: /tmp/

  - name: Setup .NET (for NuGet MCP Server)
    uses: actions/setup-dotnet@v4
    with:
      dotnet-version: '9.0.x'

  - name: Install NuGet MCP Server
    continue-on-error: true
    # Run from `/tmp` so `dotnet` does not walk into the repo's `global.json`,
    # which pins an internal-only SDK preview that is unavailable on this
    # fresh agent runner (the build job populates `.dotnet/` via `./build.sh`
    # but this is a different runner, so only the `setup-dotnet`-installed
    # SDK is present). Without this, the command exits with the custom
    # `errorMessage` from `global.json` and the whole agent job fails.
    working-directory: /tmp
    run: dotnet tool install --global NuGet.Mcp.Server --version "$NUGET_MCP_VERSION"

  # On `workflow_dispatch` runs, `github.sha` is the SHA of the dispatched ref
  # (usually the default branch), NOT the PR head. Look up the real PR head
  # SHA via the API so permalinks and inline comment placement match the PR.
  - name: Resolve PR head SHA (workflow_dispatch only)
    if: github.event_name == 'workflow_dispatch' && inputs.pr-number != ''
    id: resolve-pr-sha
    env:
      GH_TOKEN: ${{ github.token }}
      GH_AW_GITHUB_REPOSITORY: ${{ github.repository }}
      GH_AW_INPUTS_PR_NUMBER: ${{ inputs.pr-number }}
    run: |
      SHA=$(gh api "repos/${GH_AW_GITHUB_REPOSITORY}/pulls/${GH_AW_INPUTS_PR_NUMBER}" --jq .head.sha)
      echo "sha=$SHA" >> "$GITHUB_OUTPUT"

  - name: Export agent context
    env:
      GH_AW_BUILD_OUTCOME_VALUE: ${{ needs.build.outputs.outcome }}
      GH_AW_BINLOG_FOUND_VALUE: ${{ needs.build.outputs.binlog-found }}
      GH_AW_BINLOG_REL_VALUE: ${{ needs.build.outputs.binlog-relative-path }}
      GH_AW_PR_NUMBER_VALUE: ${{ github.event.pull_request.number || github.event.issue.number || inputs.pr-number }}
      GH_AW_PR_HEAD_SHA_VALUE: ${{ steps.resolve-pr-sha.outputs.sha || github.event.pull_request.head.sha || github.sha }}
      GH_AW_GITHUB_WORKSPACE: ${{ github.workspace }}
    run: |
      # The binlog itself is mounted into the binlog-mcp container at
      # `/data/build.binlog` by the gh-aw MCP gateway (see top-level
      # `mcp-servers.binlog-mcp.mounts`). The agent must pass that
      # in-container path as the `binlog_file` argument on every
      # `binlog_*` MCP tool call. `GH_AW_BINLOG_HOST_PATH` is a workspace-
      # relative reference for permalinks only; the data is read via MCP.
      BINLOG_HOST_PATH=""
      if [ -n "${GH_AW_BINLOG_REL_VALUE:-}" ]; then
        BINLOG_HOST_PATH="${GH_AW_GITHUB_WORKSPACE}/${GH_AW_BINLOG_REL_VALUE}"
      fi
      BINLOG_MCP_PATH=""
      if [ "${GH_AW_BINLOG_FOUND_VALUE:-false}" = "true" ] && [ -f /tmp/build.binlog ]; then
        BINLOG_MCP_PATH="/data/build.binlog"
      fi
      {
        echo "GH_AW_BUILD_OUTCOME=${GH_AW_BUILD_OUTCOME_VALUE}"
        echo "GH_AW_BINLOG_PATH=${BINLOG_MCP_PATH}"
        echo "GH_AW_BINLOG_HOST_PATH=${BINLOG_HOST_PATH}"
        echo "GH_AW_PR_NUMBER=${GH_AW_PR_NUMBER_VALUE}"
        echo "GH_AW_PR_HEAD_SHA=${GH_AW_PR_HEAD_SHA_VALUE}"
        echo "GH_AW_WORKSPACE=${GH_AW_GITHUB_WORKSPACE}"
      } >> "$GITHUB_ENV"

tools:
  github:
    toolsets: [pull_requests, repos]
  bash:
    - "cat"
    - "head"
    - "tail"
    - "grep"
    - "wc"
    - "sort"
    - "uniq"
    - "ls"
    - "find"
    - "dotnet"
    - "NuGet.Mcp.Server"

safe-outputs:
  # The agent runs only when the build job reports failure (see top-level
  # `if:` above). On a failed build the agent normally emits at most one
  # `noop`, one summary comment, and a small set of inline review comments,
  # but the Copilot CLI harness retries with `--continue` on
  # mid-conversation AI flakes (up to 3 retries) and each retry re-emits
  # every safe-output call it has issued so far. The caps below absorb that
  # retry budget without spurious safe-output validation warnings:
  #   - noop max=5: covers 1 happy-path + 4 retry-amplified noops.
  #   - add-comment max=5: covers 1 summary + 4 retries (hide-older-comments
  #     auto-collapses the duplicates anyway).
  #   - create-pull-request-review-comment max=25: shared body asks the
  #     agent for "top 5 highest-priority issues" per run, so 5 × (1 + 3
  #     retries) = 20 is the worst case under flake amplification.
  # We also disable `report-as-issue` / `report-failure-as-issue` so
  # transient flakes never spam tracking issues (see issue #8685).
  report-failure-as-issue: false
  add-comment:
    max: 5
    hide-older-comments: true
  create-pull-request-review-comment:
    max: 25
  noop:
    max: 5
    report-as-issue: false
---

<!--
  Body provided by shared/build-failure-analysis-shared.md.

  All build-failure analysis expertise (binlog parsing, error grouping,
  suggestion authoring) lives in the reusable agent at
  .github/agents/build-failure-analyst.agent.md.
-->
