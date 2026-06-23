# VMR Quarterback (QB) Guide

## Overview

The VMR QB is responsible for keeping code flow into the [VMR](https://github.com/dotnet/dotnet) up-to-date and ensuring its official build is healthy. This is a daily role requiring approximately **30–60 minutes per day**. The QB's job is to **identify and route issues**, not to investigate or fix them — though you're welcome to do so if you choose.

## Daily Checklist

Each morning:

1. **Check build health** — Run the pipeline health assessment (see [Build Health Monitoring](#build-health-monitoring))
2. **Check dependency flow** — Run the flow analysis and check the [Maestro dashboard](https://maestro.dot.net/codeflows?branch=main) (see [Code Flow Monitoring](#code-flow-monitoring))
3. **Merge green code flow PRs** — Approve and merge any [code flow](https://github.com/dotnet/dotnet/pulls/app%2Fdotnet-maestro) PRs that are green (see [Merging Code Flow PRs](#merging-code-flow-prs))
4. **Merge re-bootstrap PRs** — Check for open re-bootstrap PRs opened by `dotnet-bot` (e.g. [PR #7149](https://github.com/dotnet/dotnet/pull/7149)) and merge any that are green (see [Merging Re-bootstrap PRs](#merging-re-bootstrap-prs))
5. **Triage failures** — Identify broken builds or stale PRs and ping the appropriate owner (see [Repo Ownership](#repo-ownership))
6. **Communicate** — Post updates in the appropriate Teams channels for any hot issues

## Before Your Shift

- Make sure the previous QB has added you to the required [Teams channels](#communication-channels)
- Make sure you're in the **".NET VMR Owners" security group (netvmr-lt)** — you'll need this to admin-merge PRs on release day. Join the group via [aka.ms/idweb](https://aka.ms/idweb)
- Make sure the previous QB has scheduled a **handoff sync** with you before your shift starts

## Build Health Monitoring

### Using the Pipeline Investigation Skill

The [pipeline-investigation skill](https://github.com/dotnet/arcade-skills/tree/main/plugins/dotnet-dnceng/skills/pipeline-investigation) can generate a daily build health report. Use this Copilot prompt:

> *"I'd like to understand the health of the public and internal dotnet-unified-build pipelines in the last 24 hours. Make sure to use the pipeline-investigation skill."*

This will generate a report covering pass/fail rates, failure patterns, and trends. Compare against the previous day's report to spot new issues.

#### Authentication for Internal Builds

The pipeline investigation skill needs an Azure token to access internal (dnceng/internal) build results. The first time you run it, do so interactively so you can observe the authentication flow — it will prompt you to authenticate via `azureauth` or `az account get-access-token`.

### Drilling Deeper

Once you have the health report, you can ask Copilot to drill into specific issues. Here are two examples from real QB sessions:

#### Example 1: Getting links to specific failures

**Prompt:** *"I'd like to understand the health of the public and internal dotnet-unified-build pipelines in the last 24 hours. Make sure to use the pipeline-investigation skill."*

**Copilot response (summarized):**
> ### Internal: 0% clean pass rate (0/21) ⚠️⚠️
> **DevDiv package feed outage** is the dominant failure — 15 of 19 failures are `NU1301: Unable to load the service index for https://pkgs.dev.azure.com/devdiv/_packaging/...`. This is hitting roslyn and msbuild on **every release branch** (10.0.1xx, 10.0.3xx, 10.0.4xx).

**Follow-up:** *"Can you link to one of those devdiv feed outage failures?"*

**Copilot response:**
> - **release/10.0.1xx**: https://dev.azure.com/dnceng/internal/_build/results?buildId=2970304
> - **release/10.0.4xx**: https://dev.azure.com/dnceng/internal/_build/results?buildId=2969662

From there, you'd post the issue in the **First Responders** channel since this is a non-VMR-specific infra issue (NuGet feed auth).

#### Example 2: Investigating a specific build failure

**Prompt:** *"Can you investigate the build failure here: https://dev.azure.com/dnceng-public/public/_build/results?buildId=1401955"*

**Copilot response (summarized):**
> Build 1401955 has 3 distinct failures:
> 1. **vstest signing error (Windows x64 + x86):** `Sign.proj(74,5): error: Multiple certificates for extension '.cab'` — Arcade signing config conflict
> 2. **EFCore analyzer crash (Ubuntu):** xunit `MemberDataShouldReferenceValidMember` threw `InvalidOperationException: Sequence contains no elements`
> 3. **MSBuild build failure (Ubuntu):** Cascade from EFCore failure
>
> Both root causes stem from the Arcade update — the signing config conflict and xunit analyzer incompatibility.

**Follow-up:** *"For the vstest signing error, does it have signing config in its Signing.props file that is now redundant?"*

**Copilot response:** Found the conflict — new Arcade added `.cab` to `Microsoft400` signing, but vstest's `Signing.props` already defines `.cab` as `None`. Two entries = collision.

---

This pattern — health report → identify failure → drill into details → route to owner — is the core QB workflow.

## Code Flow Monitoring

### Using the Flow Analysis Skill

The [flow-analysis skill](https://github.com/dotnet/arcade-skills/tree/main/plugins/dotnet-dnceng/skills/flow-analysis) can generate a dependency flow health report. Use this Copilot prompt:

> *"I'd like to understand the health of dependency flow into the dotnet/dotnet repo. Make sure to use the flow-analysis skill."*

### Maestro Dashboard

The [Maestro code flows page](https://maestro.dot.net/codeflows?branch=main) lets you visually monitor flow health for any branch. **Check every branch you're responsible for:**

- https://maestro.dot.net/codeflows?branch=main
- https://maestro.dot.net/codeflows?branch=release%2F10.0.1xx
- https://maestro.dot.net/codeflows?branch=release%2F10.0.3xx
- (etc. — one for each active release branch)

You can also trigger subscriptions from this page to create new code flow PRs.

### Forward Flow vs. Backflow

- **Forward flow** (repo → VMR) is the QB's responsibility
- **Backflow** (VMR → repo) is the repo owner's responsibility — but if you notice super stale backflow, ping the owners

### Staleness Thresholds

- **Target:** PRs merged within **24 hours**
- **Stale:** Anything older than **7 days** should be prioritized and escalated

### Key Repos to Watch

All repos in the VMR are the QB's responsibility, but the following are the **highest priority** — especially right before code complete for servicing branches, and right before the release branch is snapped for previews/RCs:

**Product repos:** aspnetcore, efcore, runtime, sdk, windowsdesktop, winforms, wpf

**Tooling repos:** arcade, roslyn

## Merging Code Flow PRs

Most [code flow PRs](https://github.com/dotnet/dotnet/pulls/app%2Fdotnet-maestro) contain code that's already been reviewed in the source repo. If CI is green, **approve and merge**.

### Exceptions — Do NOT merge without repo owner review:

- **High file count (1000+ files)** — unusual, may indicate a problem
- **"Unsafe code flow"** PRs (the description will say so, e.g. [PR #6383](https://github.com/dotnet/dotnet/pull/6383)) — these need review from the owning team to verify the automated conflict resolution is correct

### Merge Conflicts (0 files changed)

Some PRs will open with 0 files changed due to a merge conflict (e.g. [PR #6106 comment](https://github.com/dotnet/dotnet/pull/6106#issuecomment-4269705839)). In these cases:

1. Follow the instructions in the PR comment to resolve conflicts locally
2. If the resolution is obvious, go ahead and push the fix
3. If it's unclear, ping the repo owner

## Merging Re-bootstrap PRs

Re-bootstrap PRs are periodic automated PRs opened by `dotnet-bot` that update versioning and package references on `main` (for preview releases) and `release/*` servicing branches. They are titled something like ".NET X.Y.Z-... Updates" (e.g. [PR #7149](https://github.com/dotnet/dotnet/pull/7149) — ".NET 11.0.100-preview.5.26302.115 June 2026 Updates").

### How to identify them

- Author: `dotnet-bot`
- Title pattern: `.NET X.Y.Z-... Updates` or `.NET X.Y.Z-... <Month> <Year> Updates`
- Target branch: `main` (for preview releases) or a `release/*` servicing branch

### Merge criteria

If CI is green, **approve and merge**.

> **Note:** Like internal → public merge PRs, re-bootstrap PRs may fail the "Validate user changes in VMR / Run Validation Script" CI job because `Version.Details.props` was touched. This is expected — **merge anyway** by elevating to admin at https://repos.opensource.microsoft.com/orgs/dotnet/repos/dotnet (requires netvmr-lt membership).

## Which Branches to Monitor

Monitor **all branches**: `main` and all `release/*` branches.

### Branch-specific guidance:

| Branch Type | Lockdown? | Priority |
|-------------|-----------|----------|
| `main` | Never locked | Normal |
| `release/X.Y.Nxx` (servicing) | Never locked | Normal |
| `release/X.Y.1xx-previewN` | Locked after snap | **Top priority while open** |
| `release/X.Y.1xx-rcN` | Locked after snap | **Top priority while open** |

### Preview/RC Branches

While a preview or RC branch is open, it should be your **top priority**. Merge approved PRs as soon as you can.

**Before a preview snap:**
- Ensure dependency flow from ALL product repos is up to date
- Communicate with people in the Preview/RC chat about what PRs still need to go in
- The state of the VMR's main branch at snap time becomes the preview branch — any unmerged PRs will NOT be included
- **There is no "catch-up" code flow after the snap.** Even if a change was already merged into a product or tooling repo well before the snap, it won't be included in the preview if its corresponding code flow PR wasn't merged into the VMR in time

**After a preview branch snaps:**
- Confirm in the Preview/RC Teams chat that the branch is locked down
- Once confirmed, close any remaining PRs targeting the locked branch

### Schedule

For preview and RC branches, check the release schedule to know code complete dates:
- **Current:** https://dev.azure.com/devdiv/DevDiv/_wiki/wikis/DevDiv.wiki/49885/NET-11
- (Will become NET-12 in 2027, etc.)

You can merge PRs into the preview branch **up until EOD on the day marked code complete**.

## Release Day Duties

On release day, monitor these automated PRs for servicing branches:

- **Internal → public merge PRs** (e.g. [PR #6182](https://github.com/dotnet/dotnet/pull/6182))
- **Re-bootstrap PRs** (e.g. [PR #6174](https://github.com/dotnet/dotnet/pull/6174)) — see [Merging Re-bootstrap PRs](#merging-re-bootstrap-prs) for merge criteria

Ensure they are merged or escalated if blocked.

> **Note:** Internal → public merge PRs may fail the "Validate user changes in VMR / Run Validation Script" CI job because a user touched `Version.Details.props`, which normally only the code flow bot modifies. This is expected — **merge anyway** by elevating to admin at https://repos.opensource.microsoft.com/orgs/dotnet/repos/dotnet (requires netvmr-lt membership).

## Communication Channels

| Channel | Use For |
|---------|---------|
| **Unified Build Technical and General Discussions** | VMR-specific hot issues (all builds broken, super stale PR, snap-blocking issues) |
| **.NET Tactics chat** | General coordination |
| **Preview/RC chat** | Preview/RC build health, snap coordination, general preview discussions |
| **Servicing chat** | Servicing branch coordination |
| **First Responders** | Non-VMR-specific infra issues (NuGet feed auth, machine provisioning, etc.) |

## Repo Ownership

When you identify a failure, ping the appropriate team. This table is derived from [assign_ownership.yml](https://github.com/dotnet/dotnet/blob/main/.github/policies/assign_ownership.yml):

| Repo | Owner |
|------|-------|
| arcade | [@dotnet/dnceng](https://github.com/orgs/dotnet/teams/dnceng) |
| aspnetcore | [@dotnet/aspnet-build](https://github.com/orgs/dotnet/teams/aspnet-build) |
| command-line-api | [@dotnet/system-commandline](https://github.com/orgs/dotnet/teams/system-commandline) |
| deployment-tools | [@dotnet/deployment-tools-admins](https://github.com/orgs/dotnet/teams/deployment-tools-admins) |
| efcore | [@dotnet/efteam](https://github.com/orgs/dotnet/teams/efteam) |
| emsdk | [@dotnet/dnr-codeflow](https://github.com/orgs/dotnet/teams/dnr-codeflow) |
| fsharp | [@dotnet/fsharp](https://github.com/orgs/dotnet/teams/fsharp) |
| msbuild | [@dotnet/msbuild](https://github.com/orgs/dotnet/teams/msbuild) |
| nuget-client | [@dotnet/nuget-team](https://github.com/orgs/dotnet/teams/nuget-team) |
| razor | [@dotnet/roslyn-infrastructure](https://github.com/orgs/dotnet/teams/roslyn-infrastructure) |
| roslyn | [@dotnet/roslyn-infrastructure](https://github.com/orgs/dotnet/teams/roslyn-infrastructure) |
| runtime | [@dotnet/dnr-codeflow](https://github.com/orgs/dotnet/teams/dnr-codeflow) |
| sdk | [@dotnet/dotnet-cli](https://github.com/orgs/dotnet/teams/dotnet-cli) |
| source-build-assets | [@dotnet/source-build](https://github.com/orgs/dotnet/teams/source-build) |
| templating | [@dotnet/templating-engine-maintainers](https://github.com/orgs/dotnet/teams/templating-engine-maintainers) |
| vstest | [@dotnet/dotnet-testing-admin](https://github.com/orgs/dotnet/teams/dotnet-testing-admin) |
| windowsdesktop | [@dotnet/wpf-developers](https://github.com/orgs/dotnet/teams/wpf-developers) |
| winforms | [@dotnet/dotnet-winforms-admin](https://github.com/orgs/dotnet/teams/dotnet-winforms-admin) |
| wpf | [@dotnet/wpf-developers](https://github.com/orgs/dotnet/teams/wpf-developers) |
| *General VMR infra* | [@dotnet/prodconsvcs](https://github.com/orgs/dotnet/teams/prodconsvcs) |

### Tips for finding the right person:

- Check the **recent commit history** for the affected repo to find active contributors
- Look at **team membership** for the GitHub team listed above and ping someone directly
- For **general VMR infrastructure issues** (not repo-specific), ping **@dotnet/prodconsvcs**

## End-of-Shift Handoff

Before the end of your shift:

1. Set up a **handoff sync** with the next QB on the schedule (TODO: Shawn Rothlisberger to put together a schedule)
2. Share what's currently hot:
   - What's broken in the build
   - What PRs have been open too long, who owns them, and why they're stuck
   - Any upcoming preview snaps or release day activities
