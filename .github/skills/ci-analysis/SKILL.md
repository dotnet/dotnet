---
name: ci-analysis
description: >
  Analyze CI build and test status from Azure DevOps and Helix for dotnet repository PRs.
  Use when checking CI status, investigating failures, determining if a PR is ready to merge,
  or given URLs containing dev.azure.com or helix.dot.net. Also use when asked "why is CI red",
  "test failures", "retry CI", "rerun tests", "is CI green", "build failed", "checks failing",
  or "flaky tests". DO NOT USE FOR: investigating stale codeflow PRs or dependency update health,
  tracing whether a commit has flowed from one repo to another, reviewing code changes for
  correctness or style.
---

# Azure DevOps and Helix CI Analysis

Analyze CI build status and test failures in Azure DevOps and Helix for dotnet repositories (runtime, sdk, aspnetcore, roslyn, and more).

> 🚨 **NEVER** use `gh pr review --approve` or `--request-changes`. Only `--comment` is allowed. Approval and blocking are human-only actions.

**Workflow**: Gather PR context (Step 0) → collect failure data → synthesize recommendations. The agent drives the investigation; tools provide the data.

**Accessing services**: Start with MCP tools if available. Get repo-specific CI guidance early — it provides the investigation workflow, tool selection, failure patterns, and classification algorithm for that repo. The guidance evolves with the toolset, so it always reflects current capabilities.

If MCP tools aren't loaded, the Helix CLI tool provides the same capabilities via bash with progressive discovery.

For AzDO, multiple tool sets may exist for different organizations — match the org in the build URL to the correct tools (see [references/azdo-helix-reference.md](references/azdo-helix-reference.md#azure-devops-organizations)). If queries return null, check the org before trying other approaches. For complex investigations, track what you've tried in SQL to avoid repeating failed approaches.

## When to Use This Skill

- Checking CI status ("is CI passing?", "why is CI red?")
- Investigating CI failures or determining merge readiness
- Debugging Helix test issues or build errors
- URLs containing `dev.azure.com`, `helix.dot.net`, or GitHub PR links with failing checks
- Questions like "retry CI", "rerun tests", "test failures", "checks failing"
- Investigating canceled or timed-out jobs for recoverable results

**Not for**: GitHub Actions workflows, non-Helix repos, or build performance (use binlog analysis).

> 💡 **Per-repo CI patterns differ significantly.** Get repo-specific guidance early — it tells you which tools to use, what log patterns to search for, and what gotchas to expect. This is the fastest path and prevents wasted calls.

## Quick Start

1. **Get repo-specific CI guidance** — investigation workflow, tool selection, search patterns, and failure classification for that repo
2. **Find builds** for the PR or use a build ID directly
3. **Follow the investigation order** from the guidance — it tells you what to check and in what sequence

If MCP tools aren't available, the Helix CLI tool provides the same capabilities via bash. A legacy PowerShell script is also available for environments that support it.

For full parameter reference and mode details, see [references/script-modes.md](references/script-modes.md).

## Step 0: Gather Context (before running anything)

1. **Read PR metadata** — title, description, author, labels, linked issues
2. **Classify the PR type**:

| PR Type | How to detect | Interpretation shift |
|---------|--------------|---------------------|
| **Code PR** | Human author, code changes | Failures likely relate to the changes |
| **Flow/Codeflow PR** | Author is `dotnet-maestro[bot]`, "Update dependencies" | Missing packages may be behavioral, not infrastructure |
| **Backport** | Title mentions "backport", targets release branch | Check if test exists on target branch |
| **Merge PR** | Merging between branches | Conflicts cause failures, not individual changes |
| **Dependency update** | Bumps package versions, global.json | Build failures often trace to the dependency |

3. **Check existing comments** — has someone already diagnosed failures or is a retry pending?
4. **Note the changed files** — you'll use these for failure correlation

## After Data Collection: Synthesize

> 🚨 **Don't re-fetch data you already have.** Only make additional calls for deeper investigation (Helix log searches, binlog analysis, build progression).

**Classify each failure.** Determine whether it's a build error, test failure, crash, timeout, or infrastructure issue. Exit codes, log patterns, and Helix work item state all contribute — the repo-specific CI guidance includes a classification algorithm with the patterns and recommended next steps for each category. Crashes (exit code -4, 139, 134) don't always mean tests failed — check for recoverable test results before concluding.

**Cross-reference with known issues.** Check which failures are already matched by Build Analysis — green means all failures are accounted for, red means some are unmatched. For each unmatched failure, search for related known issues by error message, test name, or job type. The user needs a per-failure verdict, not two separate lists.

**Correlate with PR changes.** If the same files appear in both the PR diff and the failure messages, the failure is likely PR-related. If not, check whether the same test fails on the target branch — that distinguishes pre-existing flakes from regressions.

**Verify before claiming.** Don't call it "infrastructure" without a Build Analysis match or target-branch verification. Don't call it "safe to retry" unless ALL failures are accounted for.

> 🚨 **Check build progression on multi-commit PRs.** If the PR has multiple commits, query AzDO for builds on `refs/pull/{PR}/merge` (sorted by queue time, top 10-20). Present a progression table showing which builds passed/failed at which SHAs — this narrows failures to the commit that introduced them. See [references/build-progression-analysis.md](references/build-progression-analysis.md).

For interpreting error categories, crash recovery, and canceled jobs: [references/failure-interpretation.md](references/failure-interpretation.md)

For generating recommendations from `[CI_ANALYSIS_SUMMARY]` JSON: [references/recommendation-generation.md](references/recommendation-generation.md)

## Presenting Results

> 🚨 **Keep tables narrow — 4 short columns max (# | Job | Verdict | Issue).** Put error descriptions, work item lists, and evidence in **detail bullets below the table**, not in cells. Wide tables wrap and become unreadable in terminals.

> 🚨 **Use markdown links** for PRs (`[#121195](url)`), builds (`[Build 1305302](url)`), and jobs (`[job name](azdo-job-url)`). The script output and MCP tools provide URLs — thread them through.

Lead with a 1-2 sentence verdict, then the summary table, then detail bullets (one per failure), then recommended actions. For the full format example: [references/recommendation-generation.md](references/recommendation-generation.md).

## Anti-Patterns

> 🚨 **Every failure verdict needs evidence — no "Likely flaky" without proof.** Each row in your summary table must cite a specific source: known issue number, Build Analysis match, or target-branch verification. If Build Analysis didn't match it and you haven't verified the target branch, the verdict is **"Unmatched — needs investigation"**, not "Likely flaky." A test that *looks* like it could be flaky is not the same as one you've *verified* is flaky.

> ❌ **Don't label failures "infrastructure" without evidence.** Requires: Build Analysis match, identical failure on target branch, or confirmed outage. Exception: `tests-passed-reporter-failed` is genuinely infrastructure.

> ❌ **Don't dismiss timed-out builds.** A build "failed" due to AzDO timeout can have 100% passing Helix work items. Check Helix job status before concluding failure.

> ❌ **Missing packages on flow PRs ≠ infrastructure.** Flow PRs request *different* packages. Check *which* package and *why* before assuming feed delay.

> ❌ **Don't present failures and known issues as separate lists.** Cross-reference them: for each `failedJobDetails` entry, state whether it matches a `knownIssues` entry or is unmatched. An `unclassified` failure can still match a known issue by error pattern.

> ❌ **Don't say "safe to retry" with Build Analysis red.** Map each failing job to a specific known issue first.

> ❌ **Don't execute `gh issue create` without explicit user approval.** Always present the draft command as text and ask the user to confirm before running it. This applies to KBE issues and any other GitHub issue creation.

> ❌ **Don't use raw REST APIs when higher-level tools are available.** Check your available tools for Azure DevOps and Helix operations first. REST API fallback is for when those tools are genuinely unavailable, not a first resort.

## References

- **Script modes & parameters**: [references/script-modes.md](references/script-modes.md)
- **Failure interpretation**: [references/failure-interpretation.md](references/failure-interpretation.md)
- **Recommendation generation**: [references/recommendation-generation.md](references/recommendation-generation.md)
- **Analysis workflow (Steps 1–3)**: [references/analysis-workflow.md](references/analysis-workflow.md)
- **Helix artifacts & binlogs**: [references/helix-artifacts.md](references/helix-artifacts.md)
- **Binlog comparison**: For cross-build binlog diffs, use deep investigation techniques from [references/delegation-patterns.md](references/delegation-patterns.md)
- **Build progression analysis**: [references/build-progression-analysis.md](references/build-progression-analysis.md)
- **Subagent delegation**: [references/delegation-patterns.md](references/delegation-patterns.md)
- **Azure CLI investigation**: [references/azure-cli.md](references/azure-cli.md)
- **Manual investigation**: [references/manual-investigation.md](references/manual-investigation.md)
- **SQL tracking**: [references/sql-tracking.md](references/sql-tracking.md)
- **Known Build Error issue creation**: [references/kbe-issue-creation.md](references/kbe-issue-creation.md)
- **AzDO/Helix details**: [references/azdo-helix-reference.md](references/azdo-helix-reference.md)

## Tips

1. Get repo-specific CI guidance first — it gives you the investigation order, search patterns, and gotchas
2. Check if same test fails on target branch before assuming transient
3. Look for `[ActiveIssue]` attributes for known skipped tests
4. Search for related issues across dotnet repos when failures don't match known patterns
5. "Canceled" ≠ "Failed" — canceled jobs may have recoverable Helix results. Helix data may persist even when AzDO builds have expired.
