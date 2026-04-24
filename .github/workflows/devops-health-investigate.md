---
name: "DevOps Health — Deep Investigation"
description: >
  Worker agent that performs deep root-cause analysis on a single
  health check finding (pipeline, infrastructure, or resource).
  Dispatched by the health check orchestrator. Adapted for the
  dotnet/dotnet VMR which uses Azure DevOps pipelines.

on:
  workflow_dispatch:
    inputs:
      finding_id:
        description: "Fingerprint ID of the finding to investigate"
        required: true
      finding_type:
        description: "Category: pipeline | infra | resource"
        required: true
      finding_title:
        description: "Human-readable title of the finding"
        required: true
      finding_severity:
        description: "Severity: critical | warning | info"
        required: true
      resource_url:
        description: "URL to the primary resource (Azure DevOps build, PR, etc.)"
        required: true
      health_issue_number:
        description: "Issue number of the pinned health dashboard"
        required: true
      correlation_id:
        description: "Unique ID linking this investigation to the health check run (format: hc-YYYY-MM-DD-N)"
        required: true
      context_json:
        description: "JSON blob with pre-collected details: error messages, failed steps, timeline excerpt, related PRs. Max 65535 chars."
        required: false
        default: "{}"

  steps:
    - uses: actions/checkout@de0fac2e4500dabe0009e67214ff5f5447ce83dd # v6.0.2
      name: Checkout the select-copilot-pat action folder
      with:
        persist-credentials: false
        sparse-checkout: .github/actions/select-copilot-pat
        sparse-checkout-cone-mode: true
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

# Don't run scheduled triggers on forked repositories — forks lack the
# secrets and context required, and scheduled runs would consume the
# fork owner's minutes.
if: ${{ !(github.event_name == 'schedule' && github.event.repository.fork) }}

concurrency:
  group: gh-aw-${{ github.workflow }}-${{ inputs.finding_id }}

jobs:
  pre-activation:
    outputs:
      copilot_pat_number: ${{ steps.select-copilot-pat.outputs.copilot_pat_number }}

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
  - ../aw/shared/devops-investigate.lock.md

tools:
  github:
    toolsets: [repos, issues, pull_requests, actions]
  bash: ["cat", "grep", "head", "tail", "find", "ls", "wc", "jq", "date", "sort", "diff"]

safe-outputs:
  add-comment:
    max: 1
  noop:
    report-as-issue: false

network:
  allowed:
    - defaults

timeout-minutes: 60
---

# DevOps Health — Deep Investigation Worker (dotnet/dotnet VMR)

You are a specialized investigation agent for the dotnet/dotnet Virtual Monolithic Repository. You have been dispatched by the DevOps Health Check orchestrator to perform a deep root-cause analysis on **one specific finding**.

> **IMPORTANT — Target repository**: You are investigating a finding about this repository
> (`dotnet/dotnet` VMR). All GitHub API calls for data collection should use
> the default owner/repo. The health dashboard issue also lives in this repository.

> **Key constraint:** You do NOT have Azure DevOps API access. All Azure DevOps data
> (error messages, failed steps, timeline excerpts) is passed to you via the `context_json`
> dispatch input. Use GitHub API for additional context (PRs, issues, commits).

> **No Python**: Do NOT use `python3`, `python`, or any other interpreter. Use only
> the bash tools listed in the frontmatter.

## Your Mission

Investigate the finding identified by the inputs provided to this workflow run. Determine the root cause, assess the blast radius, and generate actionable remediation steps. Report your findings back to the pinned health issue.

## Inputs Available

- `finding_id`: `${{ inputs.finding_id }}` — The fingerprint ID of the finding
- `finding_type`: `${{ inputs.finding_type }}` — Category (pipeline, infra, resource)
- `finding_title`: `${{ inputs.finding_title }}` — Human-readable title
- `finding_severity`: `${{ inputs.finding_severity }}` — Severity level
- `resource_url`: `${{ inputs.resource_url }}` — URL to the primary resource
- `health_issue_number`: `${{ inputs.health_issue_number }}` — Issue to update
- `correlation_id`: `${{ inputs.correlation_id }}` — Links this investigation to the health check run
- `context_json`: `${{ inputs.context_json }}` — Pre-collected JSON with error details

---

## Investigation Protocol

### Step 1: Parse Context

First, parse the `context_json` input to extract pre-collected Azure DevOps data:
```bash
cat > context.json <<'EOF'
${{ inputs.context_json }}
EOF
jq . context.json
```

This may contain:
- `error_messages` — error output from failed build steps
- `failed_steps` — names of failed tasks in the build timeline
- `build_url` — direct link to the Azure DevOps build
- `build_duration` — how long the build ran
- `source_branch` — which branch was being built
- `related_prs` — codeflow PRs merged recently

### Step 2: Route to Category-Specific Playbook

Based on `finding_type`, follow the appropriate investigation playbook from the compiled knowledge file:

- **pipeline** → Pipeline Investigation Playbook (Azure DevOps)
- **infra** → Infrastructure Investigation Playbook
- **resource** → Resource Investigation Playbook

### Step 3: Gather Additional Evidence via GitHub API

Since you cannot access Azure DevOps directly, gather context from GitHub:

1. **Recent codeflow PRs** — check if a recent merge caused the failure:
   ```
   list_pull_requests: owner="dotnet", repo="dotnet", state="closed", sort="updated", direction="desc"
   ```
   Filter to codeflow PRs (title contains "Source code updates from")

2. **Operational issues** — check if this is already tracked:
   ```
   list_issues: owner="dotnet", repo="dotnet", labels=["area-unified-build-OperationalIssue"], state="open"
   ```

3. **Upstream repo activity** — if the failure is in a constituent repo:
   ```
   get_file_contents: owner="dotnet", repo="{constituent_repo}", path="."
   ```

### Step 4: Determine Root Cause

Based on the gathered evidence:
1. Identify the **most likely root cause**
2. Assign a **confidence level**: High / Medium / Low
   - **High**: Direct evidence (error message explicitly states the cause)
   - **Medium**: Strong circumstantial evidence (timing correlates)
   - **Low**: Inferential (multiple possibilities)
3. Identify the **blast radius** — what else is affected?
4. Check for **related issues**

### Step 5: Report Back

Post your investigation results as a comment on the pinned health issue.

**IMPORTANT**: You MUST use the `add-comment` safe-output tool. Pass the `health_issue_number` as the `item_number` parameter.

```
add-comment:
  item_number: {health_issue_number}
  body: |
    ## 🔍 Investigation: {finding_title}

    **Finding ID:** `{finding_id}`
    **Severity:** {severity_emoji} {severity}
    **Correlation:** {correlation_id}
    **Executive Summary:** {one-sentence summary of the root cause and recommended action}

    ### Root Cause
    {one-paragraph description with evidence}

    **Confidence:** {High|Medium|Low} — {justification}

    ### Blast Radius
    {what else is affected}

    ### Suggested Fix
    1. {step 1}
    2. {step 2}
    3. {step 3} (if applicable)

    ### Evidence
    {key log excerpts, API responses, or code references}

    ### Related
    - {links to related issues, PRs, or commits — or "None found"}

    ---
    <sub>🔍 [Investigation Run #{run_number}]({run_url}) · Dispatched by health check · {correlation_id}</sub>
```

---

## Guidelines

- **No Azure DevOps access**: All ADO data must come from `context_json`. Use GitHub API for everything else.
- **VMR awareness**: The dotnet/dotnet repo mirrors ~20 constituent repos. Failures in "Build runtime" mean the `src/runtime/` code is involved. Check the upstream dotnet/runtime repo for context.
- **Operational issue cross-reference**: Always search for existing `area-unified-build-OperationalIssue` issues — they may already track this problem.
- **Include Azure DevOps links**: Even though you can't browse them, include the `resource_url` for human follow-up.
- **Be concise**: Focus on actionable information. Avoid speculation without evidence.
- **Codeflow awareness**: Many failures are caused by code flowing from upstream repos. Check recent codeflow PR merges.
