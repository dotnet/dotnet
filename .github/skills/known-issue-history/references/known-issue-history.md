# Known Issue History Analysis

## Overview

The `Get-KnownIssueHistory.ps1` script reconstructs failure timelines for "Known Build Error" issues by analyzing the edit history of the issue body.

## How It Works

### Data Source

The dotnet Build Analysis bot periodically edits Known Build Error issue bodies to update a rolling hit count table:

```
|24-Hour Hit Count|7-Day Hit Count|1-Month Count|
|---|---|---|
|0|0|0|
```

GitHub stores the revision history via the GraphQL `userContentEdits` API. Each edit includes a diff showing what changed in the issue body; the script parses hit-count table rows from these diffs to reconstruct snapshots over time.

### Failure Detection

The script detects failure events by tracking transitions in the 24-hour hit count:

| Transition | Meaning |
|---|---|
| 24h: 0 → N (N>0) | **New failure event** — a build just matched this known issue |
| 24h: N → M (M>N) | **Additional failure** — another build matched within 24 hours |
| All counts → 0 | **Cleared** — issue hasn't matched any builds recently |

### Failure Periods

Consecutive failure events are grouped into "failure periods" — a period starts when 24h goes from 0→N and ends when all counts return to 0. Each period tracks:
- Start/end dates
- Peak 24h and 1-month hit counts (indicating severity)

## Limitations

- **Edit retention**: GitHub may not retain edits indefinitely for very old issues. The script works with whatever edit history is available.
- **Polling frequency**: The Build Analysis bot doesn't edit on a fixed schedule. Edits occur when hit counts change, so the timeline shows when the bot detected changes, not the exact moment a test failed.
- **Rolling windows**: The 24h/7d/1mo counts are rolling windows. A single failure may appear in multiple snapshots as it moves through the windows.
- **Multiple matches per edit**: If the bot updates multiple sections of the issue body in one edit, only the hit count table is extracted.

## Usage Examples

### Single issue analysis
```powershell
# Full history
./scripts/Get-KnownIssueHistory.ps1 -IssueNumber 100088

# Recent failures only
./scripts/Get-KnownIssueHistory.ps1 -IssueNumber 100088 -Since 2025-01-01

# JSON output for programmatic use
./scripts/Get-KnownIssueHistory.ps1 -IssueNumber 100088 -Json
```

### Scan all active known issues
```powershell
# Find most actively failing known issues
./scripts/Get-KnownIssueHistory.ps1 -ListActive

# Different repo
./scripts/Get-KnownIssueHistory.ps1 -ListActive -Repository dotnet/aspnetcore
```

### Interpreting Output

**Failure periods** show clusters of failures. A test with many short periods is intermittently flaky. A test with one long period had a sustained regression.

**Days since last failure** is the key metric for triage:
- 0-7 days: Actively failing, needs attention
- 7-30 days: Recently active, monitor
- 30-90 days: Dormant, consider closing the known issue
- 90+ days: Likely fixed, recommend closing

**Peak 24h count** indicates severity — a peak of 1 means occasional flakiness, while higher counts mean the test is failing frequently across many builds.
