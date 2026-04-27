---
name: known-issue-history
description: >
  Analyze historical failure rates for Known Build Error issues by mining the
  edit history of issue bodies. Use when asked "when did this last fail",
  "failure history", "failure rate", "is this issue still active",
  "flaky test history", "known issue activity", or "most active known issues".
---

# Known Build Error History Analysis

Reconstruct failure timelines for "Known Build Error" issues by analyzing the edit history of the issue body's hit count table. The `build-analysis` bot periodically edits these issues to update rolling hit counts (24h/7d/1mo) — this skill mines those edits to recover the full failure history.

## When to Use This Skill

Use this skill when:
- Investigating whether a known issue is still actively failing ("when did this last fail?")
- Checking failure frequency and trends for a flaky test
- Triaging known issues — identifying which are dormant vs actively hitting
- Deciding whether to close a known issue that appears inactive
- Scanning all open known issues to find the most problematic ones

## Workflow Steps

### Single Issue Analysis

1. Determine the issue number and repository from the user's request
2. Run the script: `./scripts/Get-KnownIssueHistory.ps1 -IssueNumber <N> -Repository <owner/repo>`
3. Add `-Since <date>` if the user only wants recent failures, or `-Json` for structured output
4. Summarize the results: total failure events, failure periods, last failure date, and days since last failure
5. If posting results to an issue or PR, include a link to this skill (see Posting Results below)

### Scanning All Active Known Issues

1. Run the script: `./scripts/Get-KnownIssueHistory.ps1 -ListActive -Repository <owner/repo>`
2. Add `-MaxIssues <N>` to limit scope (default 50, max 100)
3. Summarize the ranked table highlighting the most actively failing issues and any dormant candidates for closing

## Quick Start

```powershell
# Analyze a single known issue
./scripts/Get-KnownIssueHistory.ps1 -IssueNumber 100088

# Only show recent failures
./scripts/Get-KnownIssueHistory.ps1 -IssueNumber 100088 -Since 2025-01-01

# JSON output for programmatic use
./scripts/Get-KnownIssueHistory.ps1 -IssueNumber 100088 -Json

# Scan all open Known Build Error issues, ranked by activity
./scripts/Get-KnownIssueHistory.ps1 -ListActive

# Different repository
./scripts/Get-KnownIssueHistory.ps1 -IssueNumber 12345 -Repository dotnet/aspnetcore
```

## Key Parameters

| Parameter | Description |
|-----------|-------------|
| `-IssueNumber` | Known Build Error issue number to analyze |
| `-Repository` | Target repo in `owner/repo` format (default: `dotnet/runtime`) |
| `-ListActive` | Scan all open Known Build Error issues and rank by recent activity |
| `-MaxIssues` | Max issues to scan in ListActive mode (default: 50) |
| `-Since` | Only show failures after this date (ISO 8601, e.g. `2025-01-01`) |
| `-Json` | Output structured JSON (`[KNOWN_ISSUE_HISTORY]` block) instead of table |

## Modes

### Single Issue Mode (`-IssueNumber`)

Queries the GitHub GraphQL `userContentEdits` API for the issue, parses each edit's hit count table, and detects failure events by tracking when the 24-hour count transitions from 0 to a positive value.

**Output includes:**
- **Failure summary** — total failure events, failure periods, last failure date, days since last failure
- **Failure periods** — clusters of failures grouped by the gap between all-clear events, with peak hit counts
- **Edit timeline** — chronological table of every hit count snapshot with failure event markers

### List Active Mode (`-ListActive`)

Scans all open issues with the "Known Build Error" label in the repository and analyzes each one's edit history. Outputs a ranked table sorted by most recent failure.

Use this to find:
- Which known issues are still actively failing
- Which known issues haven't failed in months (candidates for closing)
- Overall flaky test health of the repository

## Interpreting Results

### Failure Events

| Marker | Meaning |
|--------|---------|
| `<-- NEW FAILURE` | 24h count went from 0 to N — a build just matched this known issue |
| `<-- ADDITIONAL FAILURE` | 24h count increased — another build matched within 24 hours |
| `(cleared)` | All counts returned to 0 — no recent matches |

### Failure Periods

Consecutive failure events are grouped into periods. A period starts when 24h goes from 0→N and ends when all counts return to 0.

- **Many short periods** → intermittently flaky test
- **One long period** → sustained regression
- **Peak 24h count** → severity indicator (1 = occasional, higher = frequent across builds)

### Triage Guidelines

| Days Since Last Failure | Status | Action |
|------------------------|--------|--------|
| 0-7 | Actively failing | Needs investigation |
| 7-30 | Recently active | Monitor |
| 30-90 | Dormant | Consider closing |
| 90+ | Likely fixed | Recommend closing |

### Posting Results

When posting results to an issue or PR comment, always include a link back to this skill so readers know where the analysis came from:

```
Analysis generated by [known-issue-history](https://github.com/dotnet/arcade-skills/tree/main/plugins/dotnet-dnceng/skills/known-issue-history)
```

## How It Works

The `build-analysis` bot edits Known Build Error issue bodies whenever hit counts change. GitHub stores the revision history via the GraphQL `userContentEdits` API. Each edit includes a diff showing what changed in the issue body. The script parses hit-count table rows from these diffs to reconstruct snapshots:

```markdown
|24-Hour Hit Count|7-Day Hit Count|1-Month Count|
|---|---|---|
|0|0|0|
```

The script extracts these snapshots chronologically and detects state transitions to reconstruct when failures actually occurred — recovering historical data that the rolling windows would otherwise hide.

### Limitations

- **Edit retention**: GitHub may not retain edits indefinitely for very old issues
- **Polling frequency**: Edits occur when hit counts change, not on a fixed schedule — timestamps show when the bot detected changes, not the exact test failure time
- **Rolling windows**: A single failure appears across multiple snapshots as it moves through the 24h→7d→1mo windows

## References

- **Detailed approach and interpretation**: See [references/known-issue-history.md](references/known-issue-history.md)
- **Known Issues documentation**: See [dotnet/arcade Known Issues guide](https://github.com/dotnet/arcade/blob/main/Documentation/Projects/Build%20Analysis/KnownIssues.md)
- **Build Analysis website**: https://helix.dot.net/BuildAnalysis
- **Known Issues board**: https://github.com/orgs/dotnet/projects/111
