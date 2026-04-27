---
name: "DevOps Daily Health Check"
description: >
  Orchestrator workflow that analyzes pre-collected health signals for the
  dotnet/dotnet VMR, computes a fingerprint-based diff against the previous
  run, updates a pinned health dashboard issue, and dispatches investigation
  workers for new critical/warning findings.

on:
  schedule:
    - cron: "0 5 * * *"  # 05:00 UTC daily
  workflow_dispatch:

  steps:
    - uses: actions/checkout@de0fac2e4500dabe0009e67214ff5f5447ce83dd # v6.0.2
      name: Checkout config and action files
      with:
        persist-credentials: false
        sparse-checkout: |
          .github/devops-health-config.json
          .github/actions/select-copilot-pat
        sparse-checkout-cone-mode: false
        fetch-depth: 1

    - id: select-copilot-pat
      name: Select Copilot token from pool
      uses: ./.github/actions/select-copilot-pat
      env:
        # If the secret names are changed here, they must also be changed
        # in the `engine: env` case expression below
        SECRET_0: ${{ secrets.COPILOT_PAT_0 }}
        SECRET_1: ${{ secrets.COPILOT_PAT_1 }}
        SECRET_2: ${{ secrets.COPILOT_PAT_2 }}
        SECRET_3: ${{ secrets.COPILOT_PAT_3 }}
        SECRET_4: ${{ secrets.COPILOT_PAT_4 }}
        SECRET_5: ${{ secrets.COPILOT_PAT_5 }}
        SECRET_6: ${{ secrets.COPILOT_PAT_6 }}
        SECRET_7: ${{ secrets.COPILOT_PAT_7 }}
        SECRET_8: ${{ secrets.COPILOT_PAT_8 }}
        SECRET_9: ${{ secrets.COPILOT_PAT_9 }}

    - id: collect-health-data
      name: Collect health data from ADO and GitHub
      env:
        GH_TOKEN: ${{ github.token }}
      run: |
        set -euo pipefail
        CONFIG=".github/devops-health-config.json"
        NOW=$(date -u +%Y-%m-%dT%H:%M:%SZ)
        AGO_24H=$(date -u -d '24 hours ago' +%Y-%m-%dT%H:%M:%SZ)
        AGO_14D=$(date -u -d '14 days ago' +%Y-%m-%dT%H:%M:%SZ)
        UPSTREAM="dotnet/dotnet"

        # ── ADO: Public pipeline data ──────────────────────────────────
        ADO_PIPELINES='[]'
        for org in $(jq -r '.azure_devops.organizations[] | select(.requires_auth != true) | .name' "$CONFIG"); do
          project=$(jq -r --arg o "$org" '.azure_devops.organizations[] | select(.name==$o) | .project' "$CONFIG")
          for def_id in $(jq -r --arg o "$org" '.azure_devops.organizations[] | select(.name==$o) | .definitions[].id' "$CONFIG"); do
            def_name=$(jq -r --arg o "$org" --arg d "$def_id" '.azure_devops.organizations[] | select(.name==$o) | .definitions[] | select(.id==($d|tonumber)) | .name' "$CONFIG")
            for branch in $(jq -r '.branches[]' "$CONFIG"); do
              B24=$(curl -sf --retry 2 --max-time 30 \
                "https://dev.azure.com/${org}/${project}/_apis/build/builds?definitions=${def_id}&branchName=refs/heads/${branch}&statusFilter=completed&minTime=${AGO_24H}&\$top=20&api-version=7.1" 2>/dev/null || echo '{"value":[]}')
              B14=$(curl -sf --retry 2 --max-time 30 \
                "https://dev.azure.com/${org}/${project}/_apis/build/builds?definitions=${def_id}&branchName=refs/heads/${branch}&statusFilter=completed&minTime=${AGO_14D}&\$top=100&api-version=7.1" 2>/dev/null || echo '{"value":[]}')
              BUILDS=$(echo "$B24" | jq -c '[.value[:5]? | .[]? | {id, result, startTime, finishTime}]' 2>/dev/null || echo '[]')
              FAILED=$(echo "$B24" | jq -r '[.value[]? | select(.result=="failed")] | length' 2>/dev/null || echo 0)
              TOTAL_24H=$(echo "$B24" | jq -r '[.value[]?] | length' 2>/dev/null || echo 0)
              STATS=$(echo "$B14" | jq -c '{total: [.value[]?]|length, succeeded: [.value[]?|select(.result=="succeeded")]|length, failed: [.value[]?|select(.result=="failed")]|length, pass_rate: (([.value[]?|select(.result=="succeeded")]|length)*100/(([.value[]?]|length)|if .>0 then . else 1 end))}' 2>/dev/null || echo '{}')
              # Get first failed build timeline (compact)
              FAIL_DETAIL="[]"
              FIRST_FAIL_ID=$(echo "$B24" | jq -r '.value[]? | select(.result=="failed") | .id' 2>/dev/null | head -1)
              if [ -n "$FIRST_FAIL_ID" ]; then
                TL=$(curl -sf --retry 1 --max-time 20 \
                  "https://dev.azure.com/${org}/${project}/_apis/build/builds/${FIRST_FAIL_ID}/timeline?api-version=7.1" 2>/dev/null || echo '{}')
                FAIL_DETAIL=$(echo "$TL" | jq -c '[.records[]? | select(.result=="failed" and .type=="Task") | {name, error: (.issues[0]?.message // "" | .[:150])}][:3]' 2>/dev/null || echo '[]')
              fi
              ENTRY=$(jq -nc --arg o "$org" --arg d "$def_name" --arg b "$branch" --argjson bl "$BUILDS" --argjson s "$STATS" --arg f "${FAILED}" --arg t "${TOTAL_24H}" --argjson fd "$FAIL_DETAIL" \
                '{org:$o,def:$d,branch:$b,builds_24h:$bl,failed_24h:($f|tonumber),total_24h:($t|tonumber),stats_14d:$s,fail_detail:$fd}')
              ADO_PIPELINES=$(echo "$ADO_PIPELINES" | jq -c --argjson e "$ENTRY" '. + [$e]')
            done
          done
        done

        # ── GitHub: Codeflow PRs ───────────────────────────────────────
        CODEFLOW_PRS=$(gh api "repos/${UPSTREAM}/pulls?state=open&per_page=100" --jq \
          '[.[] | select(.title | test("Source code updates from")) | {number, title, created_at, updated_at}]' 2>/dev/null || echo '[]')

        # ── GitHub: Operational issues ─────────────────────────────────
        OPS_ISSUES=$(gh api "repos/${UPSTREAM}/issues?labels=area-unified-build-OperationalIssue&state=open&per_page=50" --jq \
          '[.[] | {number, title, created_at, updated_at}]' 2>/dev/null || echo '[]')

        # ── GitHub: Actions failures ───────────────────────────────────
        GHA_FAILURES=$(gh api "repos/${UPSTREAM}/actions/runs?branch=main&status=failure&per_page=10" --jq \
          '[.workflow_runs[:5] | .[]? | {name, conclusion, created_at, html_url}]' 2>/dev/null || echo '[]')

        # ── Build final summary ────────────────────────────────────────
        SUMMARY=$(jq -nc \
          --arg at "$NOW" \
          --argjson ado "$ADO_PIPELINES" \
          --argjson codeflow "$CODEFLOW_PRS" \
          --argjson ops "$OPS_ISSUES" \
          --argjson gha "$GHA_FAILURES" \
          '{collected_at:$at, ado_pipelines:$ado, codeflow_prs:$codeflow, operational_issues:$ops, gha_failures:$gha}')

        SIZE=$(echo "$SUMMARY" | wc -c)
        echo "Health data collected: ${SIZE} bytes, ADO entries: $(echo "$SUMMARY" | jq '.ado_pipelines|length'), PRs: $(echo "$SUMMARY" | jq '.codeflow_prs|length'), Issues: $(echo "$SUMMARY" | jq '.operational_issues|length')"

        {
          echo 'health_summary<<GH_AW_HEALTH_EOF'
          echo "$SUMMARY"
          echo 'GH_AW_HEALTH_EOF'
        } >> "$GITHUB_OUTPUT"

# Don't run scheduled triggers on forked repositories — forks lack the
# secrets and context required, and scheduled runs would consume the
# fork owner's minutes.
if: ${{ !(github.event_name == 'schedule' && github.event.repository.fork) }}

jobs:
  pre-activation:
    outputs:
      copilot_pat_number: ${{ steps.select-copilot-pat.outputs.copilot_pat_number }}
      health_summary: ${{ steps.collect-health-data.outputs.health_summary }}

engine:
  id: copilot
  env:
    # We cannot use line breaks in this expression as it leads to a syntax error in the compiled workflow
    # If none of the `COPILOT_PAT_#` secrets were selected, then the default COPILOT_GITHUB_TOKEN is used
    COPILOT_GITHUB_TOKEN: ${{ case(needs.pre_activation.outputs.copilot_pat_number == '0', secrets.COPILOT_PAT_0, needs.pre_activation.outputs.copilot_pat_number == '1', secrets.COPILOT_PAT_1, needs.pre_activation.outputs.copilot_pat_number == '2', secrets.COPILOT_PAT_2, needs.pre_activation.outputs.copilot_pat_number == '3', secrets.COPILOT_PAT_3, needs.pre_activation.outputs.copilot_pat_number == '4', secrets.COPILOT_PAT_4, needs.pre_activation.outputs.copilot_pat_number == '5', secrets.COPILOT_PAT_5, needs.pre_activation.outputs.copilot_pat_number == '6', secrets.COPILOT_PAT_6, needs.pre_activation.outputs.copilot_pat_number == '7', secrets.COPILOT_PAT_7, needs.pre_activation.outputs.copilot_pat_number == '8', secrets.COPILOT_PAT_8, needs.pre_activation.outputs.copilot_pat_number == '9', secrets.COPILOT_PAT_9, secrets.COPILOT_GITHUB_TOKEN) }}

permissions:
  contents: read
  actions: read
  issues: read
  pull-requests: read

imports:
  - ../aw/shared/devops-health.lock.md

tools:
  github:
    toolsets: [repos, issues, actions, pull_requests]
  cache-memory:
  bash: ["cat", "grep", "head", "tail", "find", "ls", "wc", "jq", "date", "sort", "uniq", "diff", "echo"]
  edit:

safe-outputs:
  create-issue:
    max: 1
  update-issue:
    target: "*"
    max: 1
  add-comment:
    target: "*"
    max: 1
  dispatch-workflow:
    workflows:
      - devops-health-investigate
    max: 2
  noop:
    report-as-issue: false

network:
  allowed:
    - defaults

timeout-minutes: 60
---

# DevOps Daily Health Check — Orchestrator

You are a health dashboard agent. **ALL data has been pre-collected** and is provided
below as JSON. Your ONLY job is to:

1. Parse the pre-collected data
2. Generate findings with fingerprints
3. Diff against the previous run (from `cache-memory`)
4. Update the pinned dashboard issue and post a daily comment
5. Dispatch investigation workflows for new critical findings

> **CRITICAL CONSTRAINTS:**
> - Do NOT call GitHub API for data collection — all data is pre-collected below
> - Do NOT use `python3` — only use: cat, grep, head, tail, find, ls, wc, jq, date, sort, uniq, diff, echo
> - The dashboard issue lives in THIS repository. Use default owner/repo for issue operations
> - Data was collected from this repository (dotnet/dotnet VMR)

> **Available skills:** The `dotnet-dnceng` plugin is enabled and provides specialized
> skills for pipeline-investigation, helix-investigation, ci-analysis, and flow-analysis.
> Use their domain knowledge when classifying findings and writing context for
> investigation dispatches. The investigation worker has direct access to public
> AzDO and Helix APIs, so pass structured context (build IDs, definition IDs,
> org/project info) in `context_json` to enable deep analysis.

## Pre-Collected Health Data

Save this data to a file for processing:

```bash
cat << 'HEALTH_DATA_EOF' > /tmp/health.json
${{ needs.pre_activation.outputs.health_summary }}
HEALTH_DATA_EOF
```

## Step 1: Analyze Pre-Collected Data

### ADO Pipeline Health (P1, P4, P6)

```bash
# P1 — Failed builds on main in last 24h
jq '[.ado_pipelines[] | select(.branch=="main" and .failed_24h > 0) | {def, failed_24h, total_24h, fail_detail}]' /tmp/health.json

# P4 — 14-day pass rate per pipeline/branch
jq '[.ado_pipelines[] | {def, branch, pass_rate: .stats_14d.pass_rate, total: .stats_14d.total, failed: .stats_14d.failed}]' /tmp/health.json

# P6 — Release branch health
jq '[.ado_pipelines[] | select(.branch != "main") | {def, branch, latest: .builds_24h[0].result, pass_rate: .stats_14d.pass_rate}]' /tmp/health.json
```

**Severity rules:**
- P1: 🔴 Critical if any build failed on main with >0 failed builds in 24h
- P4: 🔴 Critical if pass rate <50%, 🟡 Warning if <80%
- P6: 🔴 Critical if latest release branch build failed

**Fingerprint format:** `pipeline:ado:{org}:{definition}:{branch}:{issue_type}`

### GitHub Actions (P7)

```bash
jq '[.gha_failures[] | {name, conclusion, created_at, html_url}]' /tmp/health.json
```
- 🟡 Warning per failed run in last 24h
- Fingerprint: `pipeline:gha:{workflow_name}:{conclusion}`

### Codeflow PRs (P5, I2)

```bash
# Count and age of open codeflow PRs
jq '{count: (.codeflow_prs | length), prs: [.codeflow_prs[] | {number, title, created_at, updated_at}]}' /tmp/health.json
```

- I2: 🟡 Warning if >5 open, 🔴 Critical if any >3 days old
- P5: 🟡 Warning if codeflow backlog indicates systemic flow issues
- Fingerprint: `infra:codeflow-backlog:{severity_bucket}`

**Codeflow analysis guidance** (from `flow-analysis` skill):
- When multiple repos have stale codeflow PRs simultaneously, the root cause is usually **VMR build failures**, not Maestro issues — check build freshness
- Forward flow PRs (title "Source code updates from dotnet/{repo}") blocking backflow is the #1 cause of backflow staleness
- Distinguish backflow PRs (VMR → product repo: "[branch] Source code updates from dotnet/dotnet") from forward flow PRs (product repo → VMR: "[branch] Source code updates from dotnet/{repo}")
- A codeflow PR open >3 days with no activity is a strong signal of a stuck subscription or blocked forward flow

### Operational Issues (I1)

```bash
jq '{count: (.operational_issues | length), issues: [.operational_issues[] | {number, title, created_at, updated_at}]}' /tmp/health.json
```

- 🟡 Warning if >10 open
- 🔴 Critical if any open >3 days with no recent activity
- Fingerprint: `infra:stale-operational-issues:{severity_bucket}`

## Step 2: Fingerprint & Diff

1. Load previous fingerprints from `cache-memory` key `health-check-fingerprints`
2. Compute current fingerprints from Step 1
3. Classify: 🆕 NEW / 📌 EXISTING / ✅ RESOLVED
4. Save updated fingerprints to `cache-memory` key `health-check-fingerprints`

## Step 3: Update Dashboard Issue

Find the open issue with label `devops-health`. If none exists, create one titled
`🏥 Repository Health Dashboard` with label `devops-health`.

**Issue body template:**

```markdown
# 🏥 Daily Health Check — {date}

**Monitoring:** [dotnet/dotnet](https://github.com/dotnet/dotnet)
**Status:** 🔴 {critical_count} critical · 🟡 {warning_count} warnings · 🔵 {info_count} info
**Since yesterday:** 🆕 {new_count} new · ✅ {resolved_count} resolved · 📌 {existing_count} unchanged

> ℹ️ ADO data covers public CI (`dnceng-public`) only.

---

## 🆕 New Findings ({new_count})

{For each new finding: severity emoji, title, fingerprint, details with numbers, link, suggested action}

---

## 🔍 Investigation Results

> Deep investigations are dispatched for new critical/warning findings.
> The grooming workflow links results ~3 hours after this run.

| Finding | Severity | Status | Result |
|---------|----------|--------|--------|
{For each finding dispatched in the current run:}
| {finding_title} | {severity_emoji} {severity} | 🔄 Dispatched | [Workflow Run]({workflow_actions_url}) |
{Preserve any rows from the previous issue body that already show ✅ Done or ✅ Resolved — do not remove them}
{If no findings were dispatched AND no previous rows exist, render the table header with zero rows — the section MUST still appear}

---

## ✅ Resolved ({resolved_count})

{strikethrough resolved findings}

---

## 📌 Existing ({existing_count})

{collapsed <details> for each existing finding with first_seen and occurrences}

---

## 📊 Summary

| Category | Findings |
|----------|----------|
| ADO Pipelines (public) | {count} |
| GitHub Actions | {count} |
| Codeflow PRs | {count} |
| Infrastructure | {count} |

---

<sub>🤖 Generated by DevOps Health Check · {timestamp} UTC · Monitoring [dotnet/dotnet](https://github.com/dotnet/dotnet)</sub>
```

**CRITICAL**: The issue body must be passed directly as a string to `update-issue`. NEVER write to a file.

**CRITICAL — Investigation Results section is MANDATORY**: The `## 🔍 Investigation Results` section MUST always appear in the issue body, even if no investigations were dispatched (render the section with the table header and zero data rows). The downstream grooming workflow depends on this section to link investigation results. Never omit it. The section must appear **exactly** between the `## 🆕 New Findings` section and the `## ✅ Resolved` section. Use `🔄 Dispatched` (not `⏳ Dispatched`) as the status text. The last column header must be `Result` (not `Correlation ID`).

## Step 4: Post Daily Comment

```markdown
## 📋 Health Check — {date}

🆕 {new_count} new · ✅ {resolved_count} resolved · 📌 {existing_count} unchanged

**New:** {bullet list of new findings}
**Resolved:** {bullet list of resolved findings}

[Full report →]({issue_url})
```

## Step 5: Dispatch Investigations (MANDATORY if qualifying findings exist)

| Condition | Action |
|-----------|--------|
| 🆕 NEW + 🔴 Critical | **Always dispatch** |
| 🆕 NEW + 🟡 Warning + pipeline | **Dispatch** |
| All other | **Skip** |

Max 2 dispatches. For each:
```
dispatch-workflow:
  workflow: devops-health-investigate
  inputs:
    finding_id: "{fingerprint}"
    finding_type: "{category}"
    finding_title: "{title}"
    finding_severity: "{severity}"
    resource_url: "{link}"
    health_issue_number: "{issue_number}"
    correlation_id: "hc-{date}-{seq}"
    context_json: "{compact JSON with error details}"
```

**context_json enrichment**: Include as much pre-collected data as possible so the
investigation worker can begin analysis immediately:
- `error_messages`: Error text from failed build timeline tasks
- `failed_steps`: Names of failed tasks (e.g., "Build runtime", "Binary Analysis Scan")
- `build_url`: Direct AzDO build URL
- `build_id`: Numeric build ID for API queries
- `org` and `project`: AzDO organization and project (e.g., "dnceng-public", "public")
- `definition_id`: Pipeline definition ID for frequency analysis
- `source_branch`: Branch that was being built
- `related_prs`: Recent codeflow PR numbers merged in the last 24h

The investigation worker has direct access to public AzDO APIs and Helix APIs,
so providing the build ID and pipeline definition ID enables deeper analysis.

## Key Links

- ADO builds: `https://dev.azure.com/{org}/{project}/_build/results?buildId={id}`
- Codeflow PRs: `https://github.com/dotnet/dotnet/pull/{number}`
- Operational issues: `https://github.com/dotnet/dotnet/issues/{number}`
