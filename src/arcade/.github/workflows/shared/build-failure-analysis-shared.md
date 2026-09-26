---
# Shared prompt and authentication for the build-failure-analysis workflows.
#
# Imported by build-failure-analysis.md (check_run + workflow_dispatch
# triggers) and build-failure-analysis-command.md (slash command). Keeps the
# prompt and Copilot credential selection in one place. Per-trigger
# wiring (steps, env, mcp-servers, permissions) lives in each caller because
# gh-aw merges those fields from imports but each main workflow must still
# re-declare its top-level permissions.

description: "Shared prompt and authentication for build-failure-analysis workflows"

# Reuse the PAT-pool selector from dotnet/arcade#17050, unchanged. Both the
# selector and its consumers must use the same environment to resolve the
# same COPILOT_PAT_0..9 secrets. An empty pool fails before agent execution;
# never fall back to the legacy COPILOT_GITHUB_TOKEN or the Actions token.
# Environment is not importable: each caller must also declare
# environment: copilot-pat-pool. Compile both with gh-aw v0.86.2 and:
# --action-mode action --action-tag 6aab9e5b5c91c615506061f09bedd81a23babe3c
# Release-tag compilation prunes setup pins still used by other workflow locks.
imports:
  - uses: pat_pool.md
    with:
      environment: copilot-pat-pool

model: ${{ vars.GH_AW_MODEL_AGENT_COPILOT || vars.GH_AW_DEFAULT_MODEL_COPILOT || 'claude-sonnet-4.6' }}

engine:
  id: copilot
  env: &copilot-pool-auth
    COPILOT_GITHUB_TOKEN: |
      ${{ case(
        needs.pat_pool.outputs.pat_number == '0', secrets.COPILOT_PAT_0,
        needs.pat_pool.outputs.pat_number == '1', secrets.COPILOT_PAT_1,
        needs.pat_pool.outputs.pat_number == '2', secrets.COPILOT_PAT_2,
        needs.pat_pool.outputs.pat_number == '3', secrets.COPILOT_PAT_3,
        needs.pat_pool.outputs.pat_number == '4', secrets.COPILOT_PAT_4,
        needs.pat_pool.outputs.pat_number == '5', secrets.COPILOT_PAT_5,
        needs.pat_pool.outputs.pat_number == '6', secrets.COPILOT_PAT_6,
        needs.pat_pool.outputs.pat_number == '7', secrets.COPILOT_PAT_7,
        needs.pat_pool.outputs.pat_number == '8', secrets.COPILOT_PAT_8,
        needs.pat_pool.outputs.pat_number == '9', secrets.COPILOT_PAT_9,
        'NO COPILOT PAT AVAILABLE')
      }}

# Preserve the per-phase model overrides when regenerating with a compiler
# whose default Copilot model is now "auto".
safe-outputs:
  threat-detection:
    engine:
      id: copilot
      model: ${{ vars.GH_AW_MODEL_DETECTION_COPILOT || vars.GH_AW_DEFAULT_MODEL_COPILOT || 'claude-sonnet-4.6' }}
      # An explicit detection engine must also receive the credential override.
      env: *copilot-pool-auth

jobs:
  # Activation cannot see environment secrets. Validate before either consumer;
  # detection's always() steps would otherwise ignore a failed local pre-step.
  validate_copilot_pat:
    needs: [pat_pool]
    runs-on: ubuntu-slim
    environment: copilot-pat-pool
    permissions:
      contents: read
    steps:
      - name: Setup Scripts
        uses: github/gh-aw-actions/setup@6aab9e5b5c91c615506061f09bedd81a23babe3c # v0.86.2
        with:
          destination: ${{ runner.temp }}/gh-aw/actions
          job-name: ${{ github.job }}
      - name: Validate selected Copilot PAT
        shell: bash
        env:
          <<: *copilot-pool-auth
          COPILOT_PAT_NUMBER: ${{ needs.pat_pool.outputs.pat_number }}
        run: |
          case "${COPILOT_PAT_NUMBER}" in
            [0-9]) ;;
            *)
              echo "::error::No Copilot PAT was selected. Configure COPILOT_PAT_0..9 in the copilot-pat-pool environment."
              exit 1
              ;;
          esac
          if [[ ! "$COPILOT_GITHUB_TOKEN" =~ [^[:space:]] ]]; then
            echo "::error::The selected Copilot PAT is empty in the copilot-pat-pool environment."
            exit 1
          fi
          bash "${RUNNER_TEMP}/gh-aw/actions/check_oauth_tokens.sh"
  agent:
    needs: [validate_copilot_pat]
    if: needs.validate_copilot_pat.result == 'success'
  detection:
    needs: [validate_copilot_pat]
    # The compiler retains always(), allowing failed agents with partial output.
    if: >-
      needs.validate_copilot_pat.result == 'success' &&
      (needs.agent.outputs.output_types != '' || needs.agent.outputs.has_patch == 'true')
---

# Build Failure Analyst

You are the **build-failure analyst**. Analyze the binary logs of the Azure
DevOps build that just failed and produce a PR review using the safe-output
tools (a later `safe_outputs` job performs the actual GitHub write).
Do **not** try to spawn a sub-agent: the `task` tool is intentionally not
available here. Work directly with the tools you do have: `binlog-mcp` to
read the logs, the `github` tools to read PR/repo context (the GitHub MCP
server is **read-only** here), the `safeoutputs` tools (`add_comment`,
`create_pull_request_review_comment`, `noop`) to post results, and a small set
of read-only `shell` commands (including `cat`).

## Instructions

1. Read the agent-context environment variables: `GH_AW_BUILD_OUTCOME`,
   `GH_AW_BINLOG_LIST`, `GH_AW_BINLOG_DIR`, `GH_AW_BINLOG_PATH`,
   `GH_AW_BINLOG_HOST_PATH`, `GH_AW_PR_NUMBER`, `GH_AW_PR_HEAD_SHA`,
   `GH_AW_PR_MERGE_SHA`, `GH_AW_WORKSPACE`.

2. If `GH_AW_BUILD_OUTCOME == 'success'`, the build did not actually fail —
   there is nothing to analyze. Call `noop` with the message
   `"Build succeeded — no analysis required."` and stop.

3. Load your detailed playbook: `cat .github/agents/build-failure-analyst.agent.md`
   (it is checked out with the repository config). Follow that methodology —
   root-cause grouping, source-context reading via the GitHub API at
   `GH_AW_PR_HEAD_SHA`, comment/suggestion formatting, and defensive behavior.
   In summary:
   - Iterate **every** path in `GH_AW_BINLOG_LIST` (newline-separated
     in-container binlog paths, one per failed-build leg, under
     `GH_AW_BINLOG_DIR` = `/data/binlogs`) and query the `binlog-mcp` MCP
     server (`binlog_errors`, `binlog_overview`, `binlog_warnings`, …) with
     `binlog_file` set to each leg's path — a failure usually surfaces in only
     one leg, so do not analyse just the first. `binlog_errors`,
     `binlog_overview`, `binlog_warnings`, … are **MCP tools** provided by the
     `binlog-mcp` server: prefer calling them **directly as MCP tools** (with a
     `binlog_file` argument). A CLI wrapper is also mounted and allowlisted, so
     you may alternatively run `binlog-mcp <tool> --binlog_file <path>` via the
     shell. If no leg shows errors **and**
     no failed-target/process evidence, the build compiled cleanly — the
     pipeline failure is then a **non-build** (test/Helix/publishing) failure,
     which is **out of scope**. This workflow analyses build failures only, so
     **post nothing**: call `noop` with a short reason and stop. Do **not**
     post a summary comment and do **not** invent fixes.
   - Post exactly one summary via `add_comment` and any inline
     `suggestion` blocks via `create_pull_request_review_comment`, **targeting
     the pull request `GH_AW_PR_NUMBER` explicitly** (these workflows use
     `target: "*"`, so there is no implicit "triggering PR" — pass the number
     on every safe-output call).
   - `submit_pull_request_review` is **not** a safe output for this workflow;
     inline comments stand alone.

4. When you have posted the analysis for a genuine build failure (or called
   `noop` for a clean-compile / non-build failure), stop.
