---
name: "Build Failure Analysis (command)"
description: >-
  Rerun the build-failure analysis on a pull request when a maintainer
  comments `/analyze-build-failure`. Same body as `build-failure-analysis.md`
  — re-runs `./build.sh --binaryLog`, captures the binlog, and delegates to
  the `build-failure-analyst` agent (which queries the binlog live via the
  containerized `binlog-mcp` MCP server). Useful when a previous run was
  cancelled, the analysis comment was dismissed, or the agent needs another
  pass after a force-push.

on:
  slash_command:
    name: analyze-build-failure
    events: [pull_request_comment]
    strategy: centralized
  roles: [admin, maintainer, write]
  reaction: "eyes"
  # Make `pre_activation` and `activation` wait for the custom `build` job
  # defined below so the agent only runs when there is actually something
  # to analyse, mirroring the auto-trigger workflow.
  needs: [build]

# Skip activation (and therefore the agent job) when the build job reported
# success — even when invoked explicitly via `/analyze-build-failure`, there
# is nothing to analyse on a green build and running the agent would just
# expose us to transient Copilot AI flakes (see issue #8685).
if: needs.build.outputs.outcome == 'failure'

permissions:
  contents: read
  pull-requests: read
  copilot-requests: write

concurrency:
  group: build-failure-analysis-${{ github.event.issue.number }}
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

# Live binlog access for the agent — see build-failure-analysis.md for the
# rationale. The build job uploads the binlog as an artifact; the agent job
# downloads it to `/tmp/build.binlog` and the gh-aw MCP gateway mounts it
# read-only at `/data/build.binlog`.
mcp-servers:
  binlog-mcp:
    container: "mcr.microsoft.com/dotnet-buildtools/prereqs:azurelinux-3.0-binlog-mcp-amd64"
    mounts:
      - "/tmp/build.binlog:/data/build.binlog:ro"
    allowed: ["*"]

# Custom build job that runs on every slash-command invocation. Mirrors the
# `build` job in build-failure-analysis.md so the slash-command variant
# benefits from the same skip-on-success gating.
jobs:
  build:
    name: Build (for analysis)
    runs-on: ubuntu-latest
    timeout-minutes: 30
    permissions:
      contents: read
    outputs:
      outcome: ${{ steps.build.outcome }}
      binlog-found: ${{ steps.find-binlog.outputs.found }}
      binlog-relative-path: ${{ steps.find-binlog.outputs.relative-path }}
    steps:
      # `pull_request_comment` events have the `issues` event payload, so
      # the default checkout would build the default branch — NOT the PR
      # the maintainer ran `/analyze-build-failure` on. Check out the PR's
      # merge ref explicitly so we analyse the same code that the auto
      # `pull_request` workflow would build.
      - uses: actions/checkout@v4
        with:
          ref: refs/pull/${{ github.event.issue.number }}/merge

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

# Steps that run in the agent job. The top-level `if:` gates these on a
# failed build, so the slash-command never invokes the AI agent on a green
# build (and thus cannot surface as a red workflow check from a transient
# Copilot AI flake).
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

  # `pull_request_comment` events use the `issues` event payload, so
  # `github.sha` is the default branch tip — NOT the PR head. Always resolve
  # the real PR head SHA via the API so permalinks and inline comment
  # placement match the PR.
  - name: Resolve PR head SHA
    id: resolve-pr-sha
    env:
      GH_TOKEN: ${{ github.token }}
      GH_AW_GITHUB_REPOSITORY: ${{ github.repository }}
      GH_AW_GITHUB_EVENT_ISSUE_NUMBER: ${{ github.event.issue.number }}
    run: |
      SHA=$(gh api "repos/${GH_AW_GITHUB_REPOSITORY}/pulls/${GH_AW_GITHUB_EVENT_ISSUE_NUMBER}" --jq .head.sha)
      echo "sha=$SHA" >> "$GITHUB_OUTPUT"

  - name: Export agent context
    env:
      GH_AW_BUILD_OUTCOME_VALUE: ${{ needs.build.outputs.outcome }}
      GH_AW_BINLOG_FOUND_VALUE: ${{ needs.build.outputs.binlog-found }}
      GH_AW_BINLOG_REL_VALUE: ${{ needs.build.outputs.binlog-relative-path }}
      GH_AW_GITHUB_EVENT_ISSUE_NUMBER: ${{ github.event.issue.number }}
      GH_AW_PR_HEAD_SHA_VALUE: ${{ steps.resolve-pr-sha.outputs.sha || github.sha }}
      GH_AW_GITHUB_WORKSPACE: ${{ github.workspace }}
    run: |
      # See build-failure-analysis.md for the binlog path conventions.
      # The agent reads the binlog through the binlog-mcp MCP server,
      # which has it mounted at `/data/build.binlog`. The host-side path
      # is kept only for permalinks.
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
        echo "GH_AW_PR_NUMBER=${GH_AW_GITHUB_EVENT_ISSUE_NUMBER}"
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
-->
