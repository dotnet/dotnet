---
name: "DevOps Health — Groom Dashboard"
description: >
  Runs ~3 hours after the daily health check to groom the pinned health
  dashboard issue: links investigation results into the issue body,
  prunes stale comments older than 7 days, and marks resolved findings.

on:
  schedule:
    - cron: "0 8 * * *"  # 08:00 UTC daily (3h after health check)
  workflow_dispatch:

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

imports:
  - ../aw/shared/devops-health.lock.md

tools:
  github:
    toolsets: [repos, issues, actions]
  bash: ["cat", "grep", "head", "tail", "jq", "date", "sort"]

safe-outputs:
  update-issue:
    target: "*"
    max: 1
  hide-comment:
    max: 50
    allowed-reasons: [outdated, resolved]
  noop:
    report-as-issue: false

network:
  allowed:
    - defaults

timeout-minutes: 60
---

# DevOps Health — Groom Dashboard

You are a dashboard grooming agent for the dotnet/dotnet VMR health dashboard. You run after the daily health check and its dispatched investigations have had time to complete. Your job is to:

1. **Link investigation results** into the issue body so the description is self-contained
2. **Hide stale comments** to keep the issue manageable (collapsed with reason)
3. **Mark resolved investigations** so readers know what's still relevant

> **No Python**: Do NOT use `python3`, `python`, or any other interpreter. Use only
> the bash tools listed in the frontmatter.

---

## Step 1: Find the Health Dashboard Issue

Search for open issues with label `devops-health`:
```
GET /repos/{owner}/{repo}/issues?labels=devops-health&state=open&per_page=5
```
Use the most recently created one. If none exist, call `noop` with message "No health dashboard issue found — nothing to groom" and stop.

Record the `issue_number` and the **full** current issue `body`.

---

## Step 2: Fetch Recent Comments

Compute a `since` timestamp equal to **30 days ago** (ISO-8601 format, e.g. `2026-03-16T00:00:00Z`). This covers the 28-day P4 hard age cutoff plus a 2-day buffer, ensuring all comments within the retention window are fetched.

```
GET /repos/{owner}/{repo}/issues/{issue_number}/comments?per_page=100&since={since_timestamp}
```

The `since` parameter filters to comments created or updated after the timestamp.

**You MUST paginate**: If the response contains a `Link` header with `rel="next"`, you MUST fetch subsequent pages until no `rel="next"` link is present. Failure to paginate means investigation comments may be missed, which is the primary failure mode of this workflow.

Collect every comment with:
- `id` (numeric REST comment ID)
- `node_id` (GraphQL node ID, e.g. `IC_kwDO…` — required by `hide-comment`)
- `html_url` (link for the issue body)
- `body` (content to parse)
- `created_at` (timestamp for age checks)

### 2.1 Classify Comments

Parse each comment into one of these categories:

| Category | Detection Rule |
|----------|----------------|
| **Investigation** | Body starts with `## 🔍 Investigation:` |
| **Daily overview** | Body starts with `## 📋 Health Check —` |
| **Other** | Anything else (leave untouched) |

For each **Investigation** comment, extract:
- `finding_id` from the `**Finding ID:** \`{id}\`` line
- `executive_summary` from the `**Executive Summary:**` line (everything after the label)
- `correlation_id` from the `**Correlation:**` line
- `comment_url` = the comment's `html_url`
- `comment_id` = the comment's `id`
- `comment_node_id` = the comment's `node_id`
- `created_at` = the comment's timestamp

For each **Daily overview** comment, extract:
- `date` from the heading `## 📋 Health Check — {date}`
- `comment_id` = the comment's `id`
- `comment_node_id` = the comment's `node_id`
- `created_at` = the comment's timestamp

---

## Step 3: Link Investigation Results into Issue Body

### 3.1 Parse the Current Issue Body

Load the full issue body from Step 1. Look for ALL occurrences of the `## 🔍 Investigation Results` heading.

**CRITICAL — Duplicate Section Handling:** The issue body may contain MULTIPLE `## 🔍 Investigation Results` sections (one from the health check agent, one from a previous grooming run with `<!-- gh-aw-island-*-->` markers). You MUST:
1. Keep ONLY ONE Investigation Results section (the first occurrence, between `## 🆕 New Findings` and `## ✅ Resolved`)
2. REMOVE all other occurrences, including any wrapped in `<!-- gh-aw-island-start:devops-health-groom -->` / `<!-- gh-aw-island-end:devops-health-groom -->` markers
3. Remove the island markers themselves

If no `## 🔍 Investigation Results` section exists at all, create one and insert it between `## 🆕 New Findings` and `## ✅ Resolved`.

### 3.2 Build the Updated Table

Parse the existing Investigation Results table rows. Each row has the format:
```
| {finding_title} | {severity} | {status} | {result_or_correlation_id} |
```

For each row in the table:
1. Determine the finding for this row by matching the finding title against investigation comments
2. Also try matching any correlation ID (e.g. `hc-2026-04-21-1`) against the `correlation_id` extracted from investigation comments
3. If a matching investigation comment exists:
   - Change the status from `🔄 Dispatched` or `⏳ Dispatched` to `✅ Done`
   - Replace the Result cell with `[{executive_summary}]({comment_url})`
4. If no matching investigation comment exists yet, leave the row unchanged

Also check for investigation comments that don't match any existing table row. Add rows for those too:
```
| {finding_title from comment heading} | {severity from comment} | ✅ Done | [{executive_summary}]({comment_url}) |
```

### 3.3 Hold Changes (Do Not Update Yet)

Do **not** call `update-issue` yet. Keep the modified issue body in memory — Step 4 will make further edits to the same body before a single combined `update-issue` call.

---

## Step 4: Check for Newly Resolved Findings

### 4.1 Derive Current Fingerprints from Issue Body

Extract the set of currently active findings by parsing the issue body:
- **🆕 New Findings** section → these are current
- **📌 Existing Findings** section → these are current
- Extract the `Fingerprint:` line from each finding's detail block

The union of new + existing fingerprints forms the current active set.

### 4.2 Cross-Reference Investigation Comments

For each investigation comment found in Step 2:
1. Check if the `finding_id` is still present in the current fingerprint set
2. If the `finding_id` is **NOT** in the current fingerprints → the finding has been resolved

### 4.3 Mark Resolved in Investigation Results Table

For findings whose investigation is complete AND the finding is now resolved:
- Change status from `✅ Done` to `✅ Resolved`
- Keep the link to the investigation comment (still useful for historical context)

### 4.4 Write the Updated Issue Body

Now that both Step 3 (linking) and Step 4 (resolving) are applied, write the **full** issue body using a **single** `update-issue` call with `operation: "replace"`.

**CRITICAL — Use `operation: "replace"` with the FULL body**: You must pass the complete issue body (all sections) as the `body` field. This ensures the old duplicate Investigation Results sections are removed and the updated one is in place. Do NOT use `operation: "replace-island"` — it creates duplicate sections.

Only call `update-issue` if at least one change was made across Steps 3 and 4. If nothing changed, skip the call.

---

## Step 5: Hide Stale Comments

Use `hide-comment` to collapse stale comments. Hidden comments remain accessible
but are collapsed in the GitHub UI with a reason label.

**Minimum age safeguard:** NEVER hide any comment less than **72 hours** old,
regardless of which rule matches.

### 5.1 P1 — Daily Summary Comments (> 7 days)

Hide daily overview comments (`## 📋 Health Check —`) older than **7 days** with reason `OUTDATED`.

### 5.2 P2 — Resolved Investigation Comments (> 7 days)

Hide investigation comments (`## 🔍 Investigation:`) older than **7 days** whose
`finding_id` is NOT in the current active fingerprint set (resolved). Use reason `RESOLVED`.

### 5.3 P3 — Unreferenced Investigation Comments (> 7 days)

Hide investigation comments older than **7 days** whose `finding_id` does **not**
appear anywhere in the current issue body's Investigation Results table. Use reason `OUTDATED`.

### 5.4 P4 — Hard Age Cutoff (> 28 days)

Hide **any** bot comment (`github-actions[bot]` author) older than **28 days** with reason `OUTDATED`.

**Never hide human comments.**

### 5.5 Hide Order

Process hides in this priority order:
1. P2 — Resolved investigation comments (oldest first) — reason: `RESOLVED`
2. P3 — Unreferenced investigation comments (oldest first) — reason: `OUTDATED`
3. P1 — Age-expired daily overview comments (oldest first) — reason: `OUTDATED`
4. P4 — Hard age cutoff (oldest first) — reason: `OUTDATED`

### 5.6 Safety Limits

- Maximum 50 hides per run (safe-output budget)
- If more than 50 comments qualify, prioritize: resolved investigations first, then oldest first
- Hidden comments are NOT deleted — deletion is handled by `devops-health-cleanup.yml`

---

## Step 6: Summary

If no `update-issue` or `hide-comment` calls were made, call `noop` with a summary message. If changes were made, the safe-output calls are the implicit summary. Do NOT call `noop` if you already made other safe-output calls.

---

## Guidelines

- **CRITICAL — Use `operation: "replace"` NOT `replace-island`**: Pass the complete issue body to `update-issue`. The `replace-island` operation creates duplicate sections. You must replace the full body to ensure old Investigation Results sections (including island-marked duplicates) are removed.
- **Remove duplicate sections**: If the issue body has multiple `## 🔍 Investigation Results` sections, merge them into one and remove duplicates (including `<!-- gh-aw-island-*-->` markers).
- **Safe output body must be inline**: Never write to a file and reference it.
- **Minimal edits only**: Only change the Investigation Results table and resolved annotations. Preserve all other sections exactly as they are.
- **Precise comment parsing**: Match exact patterns from the investigation worker template.
- **Pagination is mandatory**: Always follow `Link: rel="next"` headers.
- **No intermediate files**: Do all work in memory.
- **No Python**: Only use bash tools from the frontmatter.
