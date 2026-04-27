---
name: flow-analysis
description: >
  Analyze VMR codeflow health using maestro MCP tools and GitHub MCP tools.
  USE FOR: investigating stale codeflow PRs, checking if fixes have flowed
  through the VMR pipeline, debugging dependency update issues, checking overall
  flow status for a repo, diagnosing why backflow PRs are missing or blocked,
  subscription health, build freshness, URLs containing dotnet-maestro or
  "Source code updates from dotnet/dotnet".
  DO NOT USE FOR: CI build failures (use ci-analysis skill), code review
  (use code-review skill), general PR investigation without codeflow context,
  tracing whether a specific commit/PR has reached another repo (use
  flow-tracing skill).
  INVOKES: maestro and GitHub MCP tools, flow-health.cs script.
---

# Flow Analysis

Analyze VMR codeflow PR health using **scripts** for data collection and **MCP tools** for enrichment and remediation. For single-PR analysis, `Get-CodeflowStatus.ps1` does comprehensive data collection (VMR commit comparison, forward flow discovery, staleness detection); maestro MCP tools provide subscription triggering and build freshness. For repo-wide flow health, `flow-health.cs` handles batch GitHub scanning in parallel.

> 🚨 **NEVER** use `gh pr review --approve` or `--request-changes`. Only `--comment` is allowed.

## When to Use This Skill

Use this skill when:
- A codeflow PR (from `dotnet-maestro[bot]`) is stale or failing and you need to understand why
- You need to check if a specific fix has flowed through the VMR pipeline
- A PR has a Maestro staleness warning or conflict
- You want to check overall flow health for a repo ("what's the flow status for the sdk?")
- You need to diagnose why backflow PRs are missing or blocked
- You're asked "is this codeflow PR up to date", "why is the codeflow blocked", "what's the flow status for net11"

## Prerequisites

- **Maestro MCP server** — provides subscription health, build freshness, and codeflow management. See [lewing/maestro.mcp](https://github.com/lewing/maestro.mcp) for setup.
- **GitHub CLI (`gh`)** — must be installed and authenticated. Required by `flow-health.cs`.

## Quick Start

For **"is the backflow healthy for repo X on branch Y?"** — use PR Analysis (`Get-CodeflowStatus.ps1 -Repository -Branch`). The script finds the open backflow PR (if any), compares VMR commits, discovers forward flow, and extracts warnings — all in one call.

For **"what's the flow status across all repos?"** or **multi-repo/multi-branch scanning** — use the Codeflow Overview (MCP tools for subscription health + build freshness across many repos simultaneously).

For **investigating a specific PR** — use PR Analysis (`Get-CodeflowStatus.ps1 -PRNumber`). If **no open PR exists**, the script reports this; use MCP tools to check the subscription state.

## Codeflow Concepts

- **Backflow** (VMR → product repo): PRs titled `[branch] Source code updates from dotnet/dotnet`
- **Forward flow** (product repo → VMR): PRs titled `[branch] Source code updates from dotnet/<repo>`
- **Staleness**: Forward flow merging while backflow PR is open blocks further updates. **Merging `main` into the PR branch does NOT resolve staleness** — it only fixes git conflicts. The only resolutions are: merge the PR as-is, close it, or force trigger the subscription.
- **VMR backflow vs dependency subs**: Don't confuse stuck dependency subscriptions (from product repos) with VMR backflow problems (from `dotnet/dotnet`). See [vmr-codeflow-reference.md](references/vmr-codeflow-reference.md#vmr-backflow-vs-dependency-subscriptions) for details.

## Channel Resolution

Users refer to channels with shorthand. **.NET major = year − 2015** (2026 → .NET 11, 2025 → .NET 10).

| User says | Resolve to | Example exact channel name |
|-----------|-----------|---------------------------|
| `net11` | Filter channels for `11.0` | `.NET 11.0.1xx SDK` |
| `11.0.1xx` | Use directly | `.NET 11.0.1xx SDK` |
| `net10 3xx` | Version 10, band 3 | `.NET 10.0.3xx SDK` |
| `release/10.0.3xx` | Strip `release/` → `10.0.3xx` | `.NET 10.0.3xx SDK` |
| `main` | Current dev (major = year − 2015, band = `1xx`) | `.NET 11.0.1xx SDK` |

> ⚠️ **Channel filter requires an exact substring match.** Use the full channel name (e.g., `.NET 11.0.1xx SDK`) when filtering codeflow PRs or subscriptions. Partial names like `.NET 11.0` may not match.

### SDK Bands and Forward Flow

The **1xx band** has full source-build with runtime forward flow. **2xx/3xx bands** consume runtime as prebuilts — missing runtime forward flow on these bands is expected, not broken. See [vmr-codeflow-reference.md](references/vmr-codeflow-reference.md#sdk-bands-and-forward-flow) for the full band table.

## Analysis Modes

| Question | Mode | Approach |
|----------|------|----------|
| "What's the flow status for X?" | **Codeflow overview** | Codeflow statuses → subscription health with validate → build freshness |
| "Is backflow healthy for X on Y?" | **PR analysis** | `Get-CodeflowStatus.ps1 -Repository -Branch` → read output → MCP enrichment |
| "Why is this PR stale/blocked?" | **PR analysis** | `Get-CodeflowStatus.ps1 -PrUrl` → read output → MCP enrichment |
| "What's the flow status across all repos?" | **Codeflow overview** | Codeflow statuses for each repo → subscription health + build freshness |
| "Full flow health report for X" | **Flow health** | `flow-health.cs` script for batch GitHub scanning + maestro enrichment |

## Codeflow Overview Workflow

When the user asks "what codeflow PRs are active?" or "what's the flow status?", start with **codeflow statuses** for the target repo/branch — one call shows per-mapping forward flow and backflow status with active PRs and build staleness. Then drill into problems with subscription health and scripts.

> 🚨 **Trust "commits behind", don't trust "builds behind".** Subscription health returns two kinds of staleness numbers — **"N commits behind"** is real commit distance (trust it, report it directly) and **"~N builds behind"** (note the `~` prefix) is a meaningless BAR build ID delta that overstates staleness by 10x-300x. For any stale entry showing `~builds behind`, compute the real commit distance yourself (see Step 5).

### Step 0: Quick Status via Codeflow Statuses

For any repo or the VMR, get **codeflow statuses** for the repo and branch (defaults to `dotnet/dotnet` on `main`). This returns per-mapping forward flow and backflow status in one call — active PRs, build staleness, and subscription details. Use this as the initial triage to identify which mappings need investigation before drilling into subscription health.

> 💡 **When to skip Step 0**: If you already have a specific PR URL or subscription ID, go directly to PR Analysis or subscription health. Codeflow statuses is for "what's the overall picture?" questions.

### Step 1: Check Subscription Health

Check subscription health for the target repository. This shows which subscriptions are stale and which are current. Entries showing "N commits behind" (no `~`) have real commit distances — use those directly. Entries showing "~N builds behind" (with `~`) need commit distance computation in Step 5.

Use the `validate` option to enable **cross-validation** — this checks subscription state against GitHub PR state, detects state oscillation patterns, and traces source-manifest commits. Always use `validate` when investigating stuck subscriptions.

> ⚠️ **Output includes ALL subscriptions** (all branches and channels). For a version-specific query like "net11 status", filter the results for channels containing your target version (e.g., `11.0`) and the relevant branch (`main` for current dev).

### Step 2: Check Forward Flow

**Before drilling into backflow problems**, check for open forward flow PRs from the product repo into `dotnet/dotnet`. An open forward flow PR is the #1 cause of backflow staleness — if forward flow is pending, backflow is blocked by design.

If codeflow statuses (Step 0) shows a forward flow subscription with failures or high commit distance, investigate:

1. **Check the subscription's update history** — look for consecutive `Failed` or state oscillation (e.g., repeating `ApplyingUpdates → MergingPullRequest → ApplyingUpdates`). State oscillation means Maestro keeps retrying but something prevents completion.
2. **Check if a tracked PR exists** — a forward flow subscription may report failures but actually have a merged-then-reopened PR, or no PR at all. The tracked PR tells you what Maestro thinks is happening.
3. **Cross-validate against GitHub** — if Maestro says the subscription is failing but a PR exists and is merged, the subscription has a **bookkeeping bug** (Maestro never updated `LastAppliedBuildId`). Use `validate` on subscription health to detect this automatically.

> 🚨 **Forward flow bookkeeping bug**: When a forward flow subscription shows "N builds behind" with consecutive failures but GitHub shows PRs merging successfully, this is a known Maestro issue where `LastAppliedBuildId` doesn't update after merge. The subscription is stuck in an infinite retry loop. **Remedy**: force-trigger the subscription — this resets Maestro's state by creating a fresh PR branch.

### Step 3: List Tracked PRs

List all codeflow PRs currently tracked by Maestro, optionally filtering by channel name.

> ⚠️ **Output is large** (200+ PRs across all repos). Filter by `channelName` parameter (use exact name like `.NET 11.0.1xx SDK`), or grep/search the output for your target repo.

### Step 4: Drill Into Problems

For subscriptions that are stale — whether they have a stuck PR or no PR at all:
- Check the subscription's update history to find the failure point
- Check build freshness to rule out VMR build failures (if builds are stale, it's a VMR issue, not Maestro)
- For stuck PRs, check the PR's age and recent activity — a PR open >3 days with no progress needs attention

### Step 5: Get Real Commit Distance

For stale entries showing "~N builds behind", request **commit details** when checking subscription health. This returns the actual commit count and recent commit metadata (SHA, message, author, date) — no manual GitHub API calls needed.

Report the commit count as "N commits behind". Use the recent commit list to explain *what* is behind (e.g., "3 commits behind — latest: Fix NuGet restore race condition").

### Step 6: Enrich with GitHub Data

Use GitHub PR details to check state, comments, and merge status for any PRs flagged as problematic.

### Multi-Repo Health Check

When asked about flow health across "all repos" or a major version (e.g., "net11 status"), check the core product repos. Subscription health calls are independent — run them in parallel.

**Core repos**: `dotnet/runtime`, `dotnet/sdk`, `dotnet/aspnetcore`, `dotnet/roslyn`, `dotnet/efcore`, `dotnet/winforms`, `dotnet/wpf`, `dotnet/msbuild`

**Branch names differ across repos** — `runtime`/`aspnetcore` use `release/X.0`, `sdk` uses `release/X.0.Nxx`, `msbuild` uses `vsNN.N`, `roslyn` uses `release/devNN.0` (VS major = .NET major + 8). Current dev is `main` for all. See [vmr-codeflow-reference.md](references/vmr-codeflow-reference.md#branch-naming-per-repo) for the full table.

When asked about a major version, check **all branches** — don't ask for clarification. Present a consolidated cross-repo summary.

## PR Analysis Workflow

> 🚨 **Script-first.** Always run `Get-CodeflowStatus.ps1` first. It produces a `[CODEFLOW_SUMMARY]` JSON block with VMR commit comparison, forward flow discovery, and staleness detection that cannot be replicated by individual MCP calls. **Do NOT re-query the same data via MCP tools** — read and interpret the script output.

### Step 1: Run the Script

```powershell
# Analyze a specific PR by number
./scripts/Get-CodeflowStatus.ps1 -Repository "dotnet/runtime" -PRNumber 12345

# Check if a PR exists for a repo/branch (finds the open backflow PR)
./scripts/Get-CodeflowStatus.ps1 -Repository "dotnet/runtime" -CheckMissing -Branch "main"

# Check for missing PRs across all branches
./scripts/Get-CodeflowStatus.ps1 -Repository "dotnet/runtime" -CheckMissing
```

The script outputs a `[CODEFLOW_SUMMARY]` JSON block followed by a text summary. **Parse the JSON** — it contains:
- **`status`**: MERGED / CLOSED / NO-OP / IN_PROGRESS / STALE / ACTIVE
- **`vmrComparison.aheadBy`**: How many VMR commits behind (the *real* staleness number)
- **`forwardFlow.prs[]`**: All open forward flow PRs with their state
- **`warnings[]`**: Maestro staleness and conflict warnings extracted from PR comments
- **`subscription.id`**: For use with MCP remediation tools
- **`build.id`**: BAR build ID for triggering

### Step 2: After the Script — Use Its Output

🚨 The script already collected PR metadata, VMR commit distances, forward flow PRs, and Maestro warnings. **Do NOT re-query this data.** Instead:

1. **Read the `[CODEFLOW_SUMMARY]` JSON** and extract key facts:
   - `vmrComparison.aheadBy` = how far behind (this is VMR commits, NOT builds)
   - `forwardFlow.prs` = what's blocking backflow (open forward flow = blocked by design)
   - `warnings` = staleness/conflict details from Maestro comments
   - `commits.mergeCommitDetails` = merge commits on the PR (who merged `main` and when — note: merging `main` does NOT resolve staleness)
   - `status` = overall PR health classification

2. **Use MCP tools only for enrichment** the script can't provide:
   - Check **build freshness** — are VMR builds healthy? (channel-level, not per-PR)
   - Check **subscription history** — timeline of when the subscription got stuck (if script shows STALE)
   - **Trigger the subscription** — to remediate a stuck subscription (needs subscription ID + build ID from script output)

3. **Synthesize** script data + MCP enrichment into a diagnosis and recommendation.

> 💡 **No open PR?** If `-Repository`/`-Branch` finds no open backflow PR, the script reports this. Look up the **tracked PR for the subscription** to check Maestro's view, then check the most recently merged matching PR. A missing PR with a healthy subscription means flow is working normally.

### Step 3: Trace a Fix (Optional)

To check if a specific fix has reached the PR:
1. Read `src/source-manifest.json` from the VMR at the PR's snapshot commit — find the product repo's `commitSha`
2. Check if the fix commit is an ancestor of that SHA

## Flow Health Workflow (Script + MCP)

Flow health scanning uses a **hybrid approach**: the `flow-health.cs` script handles batch GitHub API calls in parallel, while maestro MCP tools provide subscription and build freshness data.

> 💡 **Why a script for flow health?** Scanning all branches requires 10-30+ parallel GitHub API calls (PR searches, body fetches, VMR HEAD lookups, commit comparisons). The script fires these in parallel using `Task.Run`; sequential MCP calls would be prohibitively slow.

### Step 1: Run the Script

```shell
# Scan all branches for a repo
dotnet ./scripts/flow-health.cs -- dotnet/sdk

# Scan a specific branch only
dotnet ./scripts/flow-health.cs -- dotnet/sdk --branch main
```

The script outputs structured JSON with:
- **`backflow.branches[]`**: Per-branch status (healthy/stale/conflict/missing/up-to-date/released-preview), PR numbers, VMR commit mapping, ahead-by counts, **CI status** (`ciStatus`: green/red/pending/none, `ciFailedCount`/`ciTotalCount` when red)
- **`backflow.summary`**: Counts of healthy/upToDate/blocked/missing branches
- **`forwardFlow.prs[]`**: Open forward flow PRs with health status
- **`forwardFlow.summary`**: Counts of healthy/stale/conflicted forward PRs

### Step 2: Enrich with Maestro MCP Data

After the script runs, enrich with maestro data:

1. **Build freshness**: For each `vmrBranch` found in the script output, check build freshness with the channel short name to verify official VMR builds are healthy.

2. **Subscription health**: For branches with `status: "missing"`, check subscription health for the target repository to diagnose *why* — is the subscription stuck, disabled, or is the channel frozen?

3. **Update history**: For stuck subscriptions, check the subscription's update history to see the timeline — when was the last successful application? Was there a failed attempt?

4. **Tracked PRs**: Cross-reference script results with the codeflow PR list to see Maestro's view of tracked PRs — the script sees GitHub state while Maestro may have a different picture.

5. **Latest builds**: For stuck subscriptions, find the latest build to get the buildId needed for triggering.

### Step 3: Synthesize

Combine script output (GitHub PR state) + MCP data (Maestro health) to produce the diagnosis:
- If multiple branches show `missing` AND build freshness is stale → VMR build failure (not a Maestro issue)
- If one branch is `missing` but builds are fresh → Maestro is stuck, suggest triggering
- If a branch has `status: "conflict"` → suggest `darc vmr resolve-conflict`

## Interpreting Results

### Current State
- **✅ MERGED**: No action needed
- **✖️ CLOSED**: Maestro should create a replacement; check subscription health
- **📭 NO-OP**: Empty diff — changes landed via other paths
- **🔄 IN PROGRESS**: Recent force push within 24h — someone is working on it
- **⏳ STALE**: No activity for >3 days — needs attention
- **🔴 CI-RED**: CI is failing — investigate with ci-analysis even if codeflow is otherwise healthy
- **✅ ACTIVE**: PR has content and recent activity

### CI Status on Open PRs

> 🚨 **Always report CI status for open PRs.** A "healthy" PR with red CI is NOT healthy — CI failure is a problem regardless of codeflow state. A stale PR with red CI is stale *because* CI is failing. Report `ciStatus` from the script output in every PR row.

| ciStatus | Meaning | Action |
|----------|---------|--------|
| `green` | All checks passing | PR is mergeable if no conflicts |
| `red` | CI failures (`ciFailedCount`/`ciTotalCount`) | **Always flag this** — even on "healthy" PRs. Suggest ci-analysis for deeper investigation. |
| `pending` | Checks still running | Wait for completion |
| `none` | No checks found | Unusual — may be freshly pushed |

### Subscription Health Diagnostics
- **`maestro-stuck`**: Subscription enabled, but last applied build is older than latest — Maestro isn't processing. Trigger the subscription to remediate.
- **`subscription-disabled`**: Subscription turned off — intentional or oversight
- **`channel-frozen`**: Latest build is `Released` — no action needed (preview shipped)
- **`subscription-missing`**: No subscription exists — expected for shipped previews

### Subscription History Patterns

Check **subscription history** to understand failure timelines. Key patterns: one-off `ApplyingUpdates` failures are normal retry; **alternating failures spanning weeks** indicate systemic problems (conflict, CI, blocked forward flow). Long gaps mean disabled subscription or no new builds. See [vmr-codeflow-reference.md](references/vmr-codeflow-reference.md#subscription-history-patterns) for the full pattern catalog.

> ❌ **Never assume "Unknown" means healthy.** API failures produce Unknown status — exclude from positive counts.

## Generating Recommendations

Check `isCodeflowPR` first — if the PR isn't from `dotnet-maestro[bot]`, skip codeflow advice.

| State | Action |
|-------|--------|
| MERGED | Mention Maestro will create new PR if VMR has newer content |
| CLOSED | Suggest triggering subscription if ID available |
| NO-OP | Recommend closing/merging to clear state |
| IN_PROGRESS | Wait, then check back |
| STALE | Check warnings for what's blocking |
| ACTIVE | Check freshness and warnings for nuance |

### Remediation via Maestro

| Action | When |
|--------|------|
| Trigger subscription | PR was closed or no PR exists for an enabled subscription |
| Trigger with source repo + channel | Provide source repository URL and channel name to auto-resolve latest build (eliminates manual build lookup) |
| Force-trigger subscription | Bookkeeping bug — subscription shows failures but PRs merge. Force-trigger overwrites the existing PR branch with fresh content, resetting Maestro's state |
| Check subscription history | Diagnosing when a subscription got stuck or failed |
| Check backflow status for a VMR build | Understanding which product repos received a VMR build |
| Bypass cache after action | After triggering, verify state changed using noCache |

> 💡 **Smart trigger**: When triggering a subscription, you can provide the source repository and channel name instead of looking up the latest build ID manually. The MCP server resolves the latest build automatically.

> 💡 **Force trigger**: Force-trigger overwrites the existing PR branch with fresh content. Use this when the subscription has a bookkeeping bug (consecutive failures but PRs merge) or when the tracked PR is in a broken state. Force-trigger is available via MCP.

### Darc Commands (When MCP Insufficient)

```bash
darc vmr resolve-conflict --subscription <subscription-id>   # Resolve conflicts locally
```

## Widespread Staleness Pattern

When multiple repos are missing backflow simultaneously, the root cause is usually **VMR build failures**, not Maestro:

1. Check **build freshness** across multiple channels — if all are stale, VMR builds are broken
2. Check public VMR CI builds at `dnceng-public/public` pipeline 278 for failures
3. Search `dotnet/dotnet` issues with `[Operational Issue]` label

## References

- **VMR codeflow concepts & darc commands**: [references/vmr-codeflow-reference.md](references/vmr-codeflow-reference.md)
- **VMR build topology**: [references/vmr-build-topology.md](references/vmr-build-topology.md)
- **Maestro MCP server**: [github.com/lewing/maestro.mcp](https://github.com/lewing/maestro.mcp)
