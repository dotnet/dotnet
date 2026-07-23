---
description: Weekly automated link checker that finds and fixes broken links in documentation files
on:
  schedule: weekly on Friday
permissions:
  actions: read
  attestations: read
  checks: read
  contents: read
  copilot-requests: write
  deployments: read
  discussions: read
  issues: read
  models: read
  packages: read
  pages: read
  pull-requests: read
  repository-projects: read
  security-events: read
  statuses: read
  vulnerability-alerts: read
timeout-minutes: 60
network:
  allowed:
    - github
steps:
  - name: Checkout repository
    uses: actions/checkout@v4
    with:
      fetch-depth: 0
      persist-credentials: false

  - name: Check and test all documentation links
    id: link-check
    run: python3 .github/workflows/scripts/check-md-links.py
    shell: bash

tools:
  github:
    toolsets: [default]
  cache-memory: true
  bash: true
  edit:

safe-outputs:
  create-pull-request:
    title-prefix: "[link-checker] "
    labels: ["Area: Documentation", "agentic-workflows"]
    draft: false
    protected-files: fallback-to-issue
    if-no-changes: "warn"
  create-issue:
    max: 1
    title-prefix: "[link-checker] "
    labels: ["Area: Documentation", "agentic-workflows"]
  noop:
    report-as-issue: false
---

# Weekly Relative Link Checker & Fixer

You are an automated link checker and fixer agent. Your job is to fix broken links between documentation files in this repository. Link extraction and testing are done by the shared script `.github/workflows/scripts/check-md-links.py`, which already ran in the previous step and wrote the results. The rules for *fixing* a broken link — scope, anchor matching, and how to repair it — live once in the `@md-link-checker` agent (`.github/agents/md-link-checker.md`), which this workflow delegates to rather than restating them. In short: only links to other `.md` files and in-file/cross-file heading anchors are in scope, and absolute URLs (http/https/mailto/etc.) are intentionally ignored.

## Your Mission

Your workflow has already collected and tested all relative documentation links in the previous step. Use the test results to identify broken links and fix them where possible.

## Step 1: Review Link Check Results

The link check step has already run. Read `/tmp/gh-aw/agent/broken-links.md` to see all broken links that need fixing:
- Each line lists a broken relative link or anchor and the source file where it appears

Use bash to read the file:
```bash
cat /tmp/gh-aw/agent/broken-links.md
```

## Step 2: Load Cache Memory

Check cache memory for previously identified unfixable broken links:
- Load the cache memory to see if there are any broken links we've tried to fix before but couldn't
- These are links whose target file or anchor no longer exists and has no obvious replacement
- Skip these links to avoid repeated attempts

The cache memory should store a JSON object with this structure:
```json
{
  "unfixable_links": [
    {
      "url": "../old/removed-page.md",
      "source_file": "docs/index.md",
      "reason": "Target file removed and no replacement found",
      "first_seen": "2026-02-17"
    }
  ],
  "last_run": "2026-02-17"
}
```

## Step 3: Fix Broken Links — delegate to `@md-link-checker`

For each broken link found in the test results (but NOT in the unfixable list),
invoke `@md-link-checker` in its **delegated mode** and let it apply the repository's
authoritative fixing rules. Pass it the broken-links list from `/tmp/gh-aw/agent/broken-links.md`.

The agent owns the fix logic so it is never duplicated here: searching for a renamed
target file by basename for broken relative links, and matching a broken anchor against
the target's heading slugs (under a strict two-`grep`-per-anchor token budget, never
reading whole files). It edits the source markdown files directly and tells you which
links it fixed and which remain unfixable.

If the agent reports a link as unfixable:
- Add it to the `unfixable_links` list in cache memory
- Include the URL, source file, reason, and date
- This prevents future runs from wasting time on the same broken link

## Step 4: Update Cache Memory

After processing all broken links:
- Update the cache memory with any new unfixable links
- Update the "last_run" timestamp
- Save the updated cache memory

## Step 5: Create Pull Request or Noop

Based on your work:

**If you fixed any links:**
- Use the `create-pull-request` safe output to create a PR with your fixes
- In the PR body, include:
  - A summary of how many links were fixed
  - A list of the broken links and their replacements
  - Any links that were added to the unfixable list
- Title format: "Fix broken documentation links"

**If you could not fix anchors**
- Use the `create-issue` safe output to create an issue with broken links
- In the issue description, include:
   - A summary of how many links could not be fixed
   - A list of the broken anchors
- Title format: "Invalid markdown links"

**If no links needed fixing:**
- Use the `noop` safe output with a clear message like:
  - "All documentation links are working correctly" (if no broken links found)
  - "All broken links are in the unfixable list, no new fixes available" (if broken links exist but can't be fixed)

## Important Guidelines

The checking, fixing, scope, and "preserve equivalent content" rules are owned by
`@md-link-checker` — follow them there. This workflow adds only the cache-memory
discipline on top:

- **Document everything:** Keep the cache memory up to date with unfixable links
- **Be selective:** Only add links to the unfixable list once `@md-link-checker` has
  genuinely tried and failed to find an alternative

## Example Cache Memory Update

```json
{
  "unfixable_links": [
    {
      "url": "../old/removed-page.md",
      "source_file": "docs/index.md",
      "reason": "Target file removed and no replacement found in the repo",
      "first_seen": "2026-02-17"
    }
  ],
  "last_run": "2026-02-17"
}
```

## Context

- Repository: `${{ github.repository }}`
- Run weekly on Fridays to catch broken links early
- Link test results are available at `/tmp/gh-aw/agent/link-check-results.md`
