---
name: pipeline-investigation
description: >
  Investigate AzDO pipeline failures beyond Helix — build errors, infra tooling
  crashes, validation test flakiness, artifact cascade failures.
  USE FOR: "why did the unified-build fail", "what's breaking the pipeline",
  "how often does this failure occur", "drill into build task logs",
  "1ES scan failures", "SourcelinkTests flaky", "NetAnalyzers build error",
  analyzing AzDO build timelines and task logs, failure frequency/trend analysis.
  DO NOT USE FOR: Helix test failures (use helix-investigation), CI status
  overview (use ci-analysis), codeflow PRs (use flow-analysis).
  INVOKES: AzDO, Helix, and binlog MCP tools, az CLI for internal auth, gh CLI.
---

# Pipeline Investigation

Investigate AzDO pipeline failures that aren't Helix test failures — build errors, infrastructure tooling crashes, validation test flakiness, and artifact cascade failures. Complements helix-investigation by covering everything else in the pipeline.

## When to Use This Skill

- User has an AzDO build URL with a non-Helix failure (build step, validation, infra task)
- User asks "why did the pipeline fail" and the failure is in a build/scan/validation task
- User wants to know how often a specific failure occurs (frequency/trend analysis)
- User sees `exit code null`, 1ES PT errors, or MSBuild failures
- User wants to understand artifact cascade failures ("missing artifacts from prior build")
- User asks about SourcelinkTests, Binary Analysis Scan, or installer validation failures

## Output Formats

This skill produces two distinct report types. Match the format to the request:

### Health Assessment ("pipeline health", "are builds passing", "pipeline status")

Follow [references/health-assessment-format.md](references/health-assessment-format.md). Output MUST include these two tables:

1. **Failed Builds Table** — every failed build, classified and investigated:
   | Build | Type | Source | Failure Detail |
   - **Type**: Rolling, Forward Flow, or Other PR (classify via `gh pr view`)
   - **Source**: Rolling → branch name. Forward Flow → `target ← source-repo`. Other PR → short description.

2. **Summary Table** — pass/fail breakdown by build type:
   | Type | Completed | ✅ Pass | ❌ Fail | Pass Rate |

3. **Failure Trends Table** (conditional — include when 3+ builds in scope and at least one pattern recurs; cap at top 5):
   | Pattern | Hits | Window | Status |
   - **Status**: ❌ No issue filed, ✅ Fix merged, 🔄 Known issue (link), ⏳ Fix in progress

See the reference for build classification rules, branch filtering, and codeflow analysis methodology.

**Save the report:** After presenting the health assessment, save it to `reviews/pipeline-health-<slug>-YYYY-MM-DD-HHMM.md` in the repo, where `<slug>` identifies the pipeline or scope (e.g., `unified-build`, `runtime-ci`). Include the timestamp to ensure uniqueness across multiple investigations per day. Include a Methodology section at the end documenting data sources and classification approach.

### Individual Failure Investigation ("why did this build fail", single build URL)

Use Step 7's numbered format: failure category, frequency, root cause, affected branches, owner, existing issues, recommended action.

## Prerequisites

- AzDO MCP tools (ado-* prefix) for querying builds and timelines on public projects
- Binlog MCP tools (mcp-binlog-tool-*) for analyzing MSBuild binary logs from build artifacts
- `az account get-access-token` or `azureauth ado token` for authenticated REST API access to `dnceng/internal`
- `curl` for downloading task logs and build artifacts
- `gh` CLI for searching related issues and source code

## Authentication

The `dnceng/internal` project requires authentication. Two methods:

```bash
# Method 1: Azure CLI (preferred — works in most environments)
TOKEN=$(az account get-access-token --resource 499b84ac-1321-427f-aa17-267ca6975798 --query accessToken -o tsv)

# Method 2: azureauth tool
TOKEN=$(azureauth ado token --output token --prompt-hint "copilot-cli")

# Usage
curl -s -H "Authorization: Bearer $TOKEN" "https://dev.azure.com/dnceng/internal/_apis/build/builds/{buildId}/timeline?api-version=7.1"
```

> ⚠️ Tokens expire. Re-acquire if you get a 401 or redirect to a sign-in page.
> ⚠️ AzDO MCP tools do NOT work against `dnceng/internal`. Use curl with Bearer token for all internal queries.

## Workflow

### Step 1: Get the build timeline

Given a build ID or URL, query the timeline to find all failed records:

```
https://dev.azure.com/{org}/{project}/_apis/build/builds/{buildId}/timeline?api-version=7.1
```

Filter records by `result == "failed"` or `result == "succeededWithIssues"`, with `type == "Task"`. Don't skip `succeededWithIssues` — these contain real failures (signing validation errors, Component Governance warnings) that didn't block the overall job. For health assessments, also include builds with overall result `partiallySucceeded`.

Each failed or warning task has:
- **name** — the task that failed
- **parentId** — links to the parent Job record (tells you which leg)
- **issues** — error messages with type and message fields
- **log.url** — URL to download the full task log

### Step 2: Categorize the failure

| Pattern | Category | Owner |
|---------|----------|-------|
| `Binary Analysis Scan` task, `exit code null` | 1ES infra tooling crash | 1ES PT team |
| `MSB3073` / `NetAnalyzers.Package.csproj` error | Build error | Source code / SDK |
| `Validate installer packages` — expected vs actual package list mismatch | Installer validation regex | dotnet/installer |
| `Run Tests` with `SourcelinkTests.VerifySourcelinks` | Sourcelink validation | dotnet/dotnet |
| `Run Tests` with `The task has timed out` across many `SB_*_Validation_*` legs | Run Tests timeout epidemic | dotnet/dotnet |
| `Run Tests` with locale test failure (e.g., `tr-TR`) | Scenario test bug | dotnet/templating |
| `Build` with `IBCMerge` / PGO error in `Windows_Pgo_*` | PGO optimization failure | dotnet/runtime |
| `Download Previous Build` with "Artifact not found" | Artifact cascade failure | Prior build failed first |
| `Build` with `curl` / `tar` / download failure (exit code 2, "not recoverable") | External resource fetch failure | Retry first; if persistent, check URL/version |
| `Build` with `npm ci` ETIMEDOUT / ECONNREFUSED | npm network timeout | Transient; shell retry wrapper needed |
| `Build` task with compilation errors | Source code build break | Varies by component |
| `Crossgen.targets` with `exit code 57005` (0xDEAD) | crossgen2 fatal crash | dotnet/runtime (area-crossgen2-coreclr) |
| ESRP `MacSignFailed` / `FailDoNotRetry` / notarization errors | Signing/notarization failure | dotnet/sdk or ESRP team |
| `exit code null` on cross-compilation legs in containers | Container OOM kill | Infrastructure — pool/container config |
| Build start == finish, HTTP 204 timeline, `validationResults` has errors | YAML pre-flight rejection | PR author — pipeline YAML invalid |

> **Routing: AzDO tests vs Helix tests.** If the test runs directly as an AzDO task (e.g., `Run Tests` in `SB_*_Validation_*` legs — SourcelinkTests, scenario tests), it's pipeline-investigation. If the test is submitted to Helix (has a Helix job ID, work items, console logs at helix.dot.net), use helix-investigation.

### Step 3: Download and analyze task logs

```bash
TOKEN=$(azureauth ado token --output token --prompt-hint "copilot-cli")
curl -s -H "Authorization: Bearer $TOKEN" "{log_url}" > /tmp/task-{buildId}.log
```

Key patterns to search for:
- **`exit code null`** — process terminated by signal (OOM, timeout). Check for memory-related warnings before the crash.
- **`exit code 57005`** (0xDEAD) — crossgen2 fatal error. Check which assembly was being compiled. Intermittent crashes need crash dumps for diagnosis.
- **`[ERROR]`** / **`##[error]`** — explicit error messages from the task
- **`[WARNING]` floods** — excessive warnings (e.g., 1000+ hardlink failures) indicate resource exhaustion
- **MSBuild error codes** — `MSB3073` (command failed), `NETSDK*` (SDK errors), `NU1105` (invalid target framework — often transient forward flow)
- **`MacSignFailed`** / **`FailDoNotRetry`** — ESRP signing rejection. `FailDoNotRetry` means deterministic content failure, not transient.
- **Stack traces** — .NET exceptions in test output
- **`ETIMEDOUT`** / **`ECONNREFUSED`** — TCP-level network failures in npm/curl that aren't retried by default

### Step 4: Determine if it's a one-off or recurring

Query failed builds over a time range and count how many hit the same failure:

```bash
# Get failed builds for a pipeline definition
curl -s -H "Authorization: Bearer $TOKEN" \
  "https://dev.azure.com/{org}/{project}/_apis/build/builds?definitions={defId}&resultFilter=failed&minTime={iso8601}&$top=200&api-version=7.1"
```

> 💡 **Rolling builds give cleaner signal.** Add `&reasonFilter=schedule` to filter to scheduled builds only. PR builds include broken branches that inflate failure counts and obscure systemic trends.

For each build, check its timeline for the same failure pattern. Track in a SQL table:

```sql
CREATE TABLE pipeline_failures (
  build_id INT, branch TEXT, queued TEXT, failure_category TEXT, 
  failed_task TEXT, notes TEXT
);
```

Look for:
- **Burst pattern** — many occurrences in a short window → something changed (tooling update, artifact size growth)
- **Steady trickle** — consistent low rate → chronic issue
- **Single occurrence** — likely transient, not worth investigating further

### Step 5: Check for known issues

Search the relevant repo for existing issues:

```bash
gh search issues "{failure_pattern}" --repo dotnet/dotnet --limit 5
```

If found, check if the issue is being worked on. If not found and the failure is recurring, consider filing one.

### Step 6: Root cause analysis

For non-obvious failures, pull up the source code of the failing test or tool:

```bash
gh api "repos/{owner}/{repo}/contents/{path}" --jq '.content' | base64 -d
```

Look for:
- **Unbounded parallelism** — `Parallel.ForEach` without `MaxDegreeOfParallelism`
- **Tight timeouts** — processes timing out under contention
- **Resource assumptions** — disk space, memory, network that may not hold in CI
- **Non-determinism** — race conditions, order-dependent assertions

### Step 6b: Binlog analysis for deep investigation

When task logs don't reveal enough, download the build's binlog artifacts for detailed MSBuild analysis:

```bash
# List build artifacts to find binlogs
curl -s -H "Authorization: Bearer $TOKEN" \
  "https://dev.azure.com/{org}/{project}/_apis/build/builds/{buildId}/artifacts?api-version=7.1"

# Download the artifact zip
curl -s -H "Authorization: Bearer $TOKEN" -o /tmp/logs.zip "{downloadUrl}"
unzip -o /tmp/logs.zip "*/Build.binlog" -d /tmp/binlogs/
```

Then use the binlog MCP tools:
- `load_binlog` → `get_diagnostics` for errors/warnings
- `search_binlog` to find specific patterns (e.g., `MacSignFailed`, `crossgen2`)
- `get_task_info` to see exact command lines and parameters for failed tasks
- `list_tasks_in_target` to see all tasks in a target (e.g., 126 Crossgen tasks, which one failed?)

> 💡 Binlogs capture the full MSBuild execution including command lines, environment variables, and interleaved output that task logs lose. Essential for signing, crossgen2, and SBRP failures.

### Step 7: Report findings (individual failure investigations only)

For health assessments, use the Output Formats section above instead.

Provide:
1. **Failure category** — which pattern from the table above
2. **Frequency** — how often in the last N days, trending up/down/stable
3. **Root cause** — with evidence from logs and/or source code
4. **Affected branches** — main only, or also release branches
5. **Owner** — who should fix it (1ES, dotnet/dotnet, SDK team, etc.)
6. **Existing issues** — links to any filed issues
7. **Recommended action** — file issue, retry, wait for fix, etc.

## Artifact Cascade Failures

When a build leg fails, downstream legs that depend on its artifacts will also fail with "Artifact not found." This creates a cascade:

```
SB_CentOS_Build (fails: NetAnalyzers error)
  → SB_CentOS_Validation (fails: missing artifacts)
  → SB_Fedora_Offline (fails: missing artifacts)
```

**Don't investigate the cascade — find the root failure.** Look for the first failed task chronologically, or filter out "Download Previous Build" failures to find the real cause.

## Category-Specific Investigation Techniques

For detailed investigation techniques for each failure category in the table above, see [references/investigation-techniques.md](references/investigation-techniques.md). Covers: ESRP signing/notarization, container OOM, crossgen2 crashes, YAML pre-flight rejections, and network transient failures.

## Fix Verification

When a fix is merged, verify it's tested by checking builds queued **after** the merge — not builds that finished after it.

### Query all build statuses

**Always include in-progress and not-started builds**, not just completed ones. The AzDO builds API requires explicit `statusFilter` to return active builds.

> ⚠️ **The AzDO API rejects combining `completed` with `inProgress`/`notStarted` in a single `statusFilter`.** You must make separate calls and merge the results:

```bash
# Completed builds
curl -s -H "Authorization: Bearer $TOKEN" \
  "https://dev.azure.com/{org}/{project}/_apis/build/builds?definitions={defId}&branchName=refs/heads/{branch}&statusFilter=completed&api-version=7.1"

# Active builds (in-progress and queued)
curl -s -H "Authorization: Bearer $TOKEN" \
  "https://dev.azure.com/{org}/{project}/_apis/build/builds?definitions={defId}&branchName=refs/heads/{branch}&statusFilter=inProgress,notStarted&api-version=7.1"
```

If you only query completed builds, you'll miss active builds that are currently testing the fix and incorrectly conclude "not tested yet."

### Multi-branch fixes

Fixes often need backporting to multiple release branches. For each branch:

1. Find the fix PR (or backport PR) and its merge time
2. Query builds on that branch with **all statuses**
3. Compare each build's `queueTime` to the merge time
4. Partition into pre-merge (doesn't have fix) and post-merge (has fix)

> ⚠️ **batchedCI builds pick up source at queue time.** A build queued before a PR merges runs against old source, even if it finishes hours later.

## Stop Signals

- **Stop after categorizing** if it's a known issue with an existing bug. Link to it and move on.
- **Stop frequency analysis** after 14 days of data. Longer trends are rarely actionable.
- **Stop investigating cascades** — always trace back to the root failure.
- **Present single-occurrence failures** — even one-offs deserve a summary. Let the user decide whether to dig deeper or move on.

## Anti-Patterns

> 🚨 **Don't confuse cascade failures with root causes.** "Missing artifacts" means a prior leg failed — find that leg.

> 🚨 **Don't assume `exit code null` is a code bug.** It usually means the process was terminated externally (OOM, signal). Look for resource exhaustion in the log. In containers, check memory limits.

> 🚨 **Don't investigate all 100+ failed builds.** Sample 3-5 spread across the time range to confirm the pattern, then count occurrences.

> 🚨 **Don't assume ESRP `FailDoNotRetry` is transient.** It means the binary content is deterministically invalid. Investigate the source commits, don't retry the build.

> 🚨 **Don't skip infrastructure failures.** Container OOM, agent timeouts, signing failures, and network issues deserve the same depth of investigation as code failures. They often have actionable fixes (memory tuning, retry wrappers, pool changes).

## Discovering Pipeline Definitions

Never hardcode definition IDs — they vary across projects and can change. Discover them at runtime:

```bash
# Public pipelines (AzDO MCP tools work here too)
curl -s "https://dev.azure.com/dnceng-public/public/_apis/build/definitions?name=dotnet-unified-build&api-version=7.1" | jq '.value[] | {id, name, project: .project.name}'

# Internal pipelines (requires auth)
TOKEN=$(az account get-access-token --resource 499b84ac-1321-427f-aa17-267ca6975798 --query accessToken -o tsv)
curl -s -H "Authorization: Bearer $TOKEN" \
  "https://dev.azure.com/dnceng/internal/_apis/build/definitions?name=dotnet-unified-build&api-version=7.1" \
  | jq '.value[] | {id, name, project: .project.name}'
```

If querying internal fails (401/redirect), fall back to public-only analysis and note the limitation in the report.

### Health Assessment Report Format

**Required format** for pipeline health assessments. See Output Formats section above for routing, and [references/health-assessment-format.md](references/health-assessment-format.md) for the complete specification including build classification rules, branch filtering, and codeflow analysis methodology.

