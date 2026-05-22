---
name: "Build Failure Analysis"
description: >-
  Downloads binlog artifacts from the Azure DevOps PR build when it fails,
  then delegates to the `build-failure-analyst` agent to identify root causes,
  post a PR comment summarizing them, and attach inline suggestion blocks.

# This workflow does NOT build the VMR itself — it downloads binlogs from the
# Azure DevOps build that already ran. This avoids the cost of re-building
# the entire VMR in GitHub Actions.
#
# Advisory only — does not gate the PR. The deterministic build gate lives
# in the Azure DevOps pipelines (eng/pipelines/pr.yml).

on:
  # Triggered by the AzDO pipeline via `gh workflow run` on build failure,
  # or manually by a developer to re-analyze a failed build.
  workflow_dispatch:
    inputs:
      pr-number:
        description: "GitHub PR number to analyze"
        required: true
        type: string
      azdo-build-id:
        description: "Azure DevOps build ID to download binlogs from"
        required: true
        type: string
      azdo-project:
        description: "Azure DevOps project (public or internal)"
        required: false
        type: string
        default: "public"
  roles: [admin, maintainer, write]
  reaction: "eyes"

permissions:
  contents: read
  pull-requests: read

concurrency:
  group: build-failure-analysis-${{ inputs.pr-number }}
  cancel-in-progress: true

env:
  BINLOG_MCP_VERSION: '1.0.0-preview.26272.1'
  NUGET_MCP_VERSION: '1.4.3'
  AZDO_ORG: 'dnceng-public'
  AZDO_PROJECT: ${{ inputs.azdo-project || 'public' }}

timeout-minutes: 20

network:
  allowed:
    - defaults
    - dotnet

imports:
  - shared/build-failure-analysis-shared.md

# Deterministic setup: download binlogs from AzDO, dump them as JSON,
# then hand off to the agent.
steps:
  - name: Download binlogs from Azure DevOps
    id: download
    run: |
      set -uo pipefail
      BUILD_ID="${{ inputs.azdo-build-id }}"
      AZDO_ORG="${{ env.AZDO_ORG }}"
      AZDO_PROJECT="${{ env.AZDO_PROJECT }}"

      echo "Downloading binlogs from AzDO build $BUILD_ID ($AZDO_ORG/$AZDO_PROJECT)"

      mkdir -p /tmp/azdo-artifacts

      # List artifacts from the AzDO build
      ARTIFACTS=$(curl -sf \
        "https://dev.azure.com/${AZDO_ORG}/${AZDO_PROJECT}/_apis/build/builds/${BUILD_ID}/artifacts?api-version=7.1" \
      ) || { echo "##[error]Failed to list AzDO artifacts"; exit 1; }

      # Find the failed job via the build timeline, then download its BuildLogs
      TIMELINE=$(curl -sf \
        "https://dev.azure.com/${AZDO_ORG}/${AZDO_PROJECT}/_apis/build/builds/${BUILD_ID}/timeline?api-version=7.1" \
      ) || echo "{}"

      FAILED_JOB=$(echo "$TIMELINE" | jq -r '[.records[] | select(.type=="Job" and .result=="failed")] | .[0].name // empty')
      echo "Failed job: ${FAILED_JOB:-none detected}"

      ARTIFACT_NAME=""
      if [ -n "$FAILED_JOB" ]; then
        # Find the BuildLogs artifact matching the failed job
        ARTIFACT_NAME=$(echo "$ARTIFACTS" | jq -r --arg job "$FAILED_JOB" \
          '[.value[] | select(.name | test("BuildLogs"; "i")) | select(.name | test($job; "i"))] | .[0].name // empty')
      fi

      # Fallback: smallest BuildLogs artifact
      if [ -z "$ARTIFACT_NAME" ]; then
        ARTIFACT_NAME=$(echo "$ARTIFACTS" | jq -r \
          '[.value[] | select(.name | test("BuildLogs"; "i"))] | sort_by(.resource.properties.artifactsize) | .[0].name // empty')
      fi

      if [ -z "$ARTIFACT_NAME" ] || [ "$ARTIFACT_NAME" = "null" ]; then
        echo "##[warning]No BuildLogs artifacts found"
        echo "found=false" >> "$GITHUB_OUTPUT"
        exit 0
      fi

      echo "Downloading artifact: $ARTIFACT_NAME"
      curl -sfL -o /tmp/azdo-artifacts/logs.zip \
        "https://dev.azure.com/${AZDO_ORG}/${AZDO_PROJECT}/_apis/build/builds/${BUILD_ID}/artifacts?artifactName=${ARTIFACT_NAME}&api-version=7.1&%24format=zip" \
        || { echo "##[error]Failed to download $ARTIFACT_NAME"; echo "found=false" >> "$GITHUB_OUTPUT"; exit 0; }

      echo "Extracting..."
      unzip -qo /tmp/azdo-artifacts/logs.zip -d /tmp/azdo-artifacts/ 2>/dev/null || true

      # Pick the top-level VMR orchestration binlog (no repo subdir)
      BINLOG=$(find /tmp/azdo-artifacts -path '*/artifacts/log/Release/Build.binlog' -not -path '*/Release/*/Build.binlog' -type f 2>/dev/null | head -1)
      if [ -z "$BINLOG" ]; then
        BINLOG=$(find /tmp/azdo-artifacts -name '*.binlog' -type f -printf '%s %p\n' 2>/dev/null \
          | sort -rn | head -1 | cut -d' ' -f2-)
      fi

      if [ -n "$BINLOG" ] && [ -f "$BINLOG" ]; then
        echo "Found binlog: $BINLOG ($(du -h "$BINLOG" | cut -f1))"
        echo "found=true" >> "$GITHUB_OUTPUT"
        echo "path=$BINLOG" >> "$GITHUB_OUTPUT"
      else
        echo "##[warning]No .binlog files found"
        echo "found=false" >> "$GITHUB_OUTPUT"
      fi

  - name: Install binlog-mcp
    if: steps.download.outputs.found == 'true'
    run: |
      dotnet tool install --global Microsoft.AITools.BinlogMcp \
        --add-source "https://pkgs.dev.azure.com/dnceng/public/_packaging/dotnet-tools/nuget/v3/index.json" \
        --version "$BINLOG_MCP_VERSION"
      echo "$HOME/.dotnet/tools" >> "$GITHUB_PATH"

  - name: Install NuGet MCP Server
    if: steps.download.outputs.found == 'true'
    continue-on-error: true
    run: dotnet tool install --global NuGet.Mcp.Server --version "$NUGET_MCP_VERSION"

  - name: Dump binlog as JSON
    if: steps.download.outputs.found == 'true'
    continue-on-error: true
    run: |
      mkdir -p /tmp/binlog-data
      timeout 120 dotnet run --project .github/workflows/scripts/DumpBinlog -- \
        "${{ steps.download.outputs.path }}" \
        /tmp/binlog-data

  - name: Resolve PR head SHA
    id: resolve-pr
    env:
      GH_TOKEN: ${{ github.token }}
    run: |
      PR_NUMBER="${{ inputs.pr-number }}"
      SHA=$(gh api "repos/${{ github.repository }}/pulls/${PR_NUMBER}" --jq .head.sha)
      echo "sha=$SHA" >> "$GITHUB_OUTPUT"
      echo "PR #$PR_NUMBER HEAD: $SHA"

  - name: Export agent context
    run: |
      {
        echo "GH_AW_BUILD_OUTCOME=${{ steps.download.outputs.found == 'true' && 'failure' || 'skipped' }}"
        echo "GH_AW_BINLOG_PATH=${{ steps.download.outputs.path }}"
        echo "GH_AW_PR_NUMBER=${{ inputs.pr-number }}"
        echo "GH_AW_PR_HEAD_SHA=${{ steps.resolve-pr.outputs.sha || github.sha }}"
        echo "GH_AW_WORKSPACE=${{ github.workspace }}"
        echo "GH_AW_AZDO_BUILD_URL=https://dev.azure.com/${{ env.AZDO_ORG }}/${{ env.AZDO_PROJECT }}/_build/results?buildId=${{ inputs.azdo-build-id }}"
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
    - "curl"
    - "jq"
    - "dotnet"

safe-outputs:
  add-comment:
    max: 1
    hide-older-comments: true
  create-pull-request-review-comment:
    max: 10
  noop:
    report-as-issue: false
---

<!--
  Body provided by shared/build-failure-analysis-shared.md.

  All build-failure analysis expertise (binlog parsing, error grouping,
  suggestion authoring) lives in the reusable agent at
  .github/agents/build-failure-analyst.agent.md.
-->
