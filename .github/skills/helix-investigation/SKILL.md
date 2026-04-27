---
name: helix-investigation
description: >
  Deep-dive investigation of Helix test failures starting from AzDO build legs.
  USE FOR: investigating recurring Helix test failures, downloading and analyzing
  Helix console logs, comparing passing vs failing runs, identifying machine-specific
  issues, XHarness timeout analysis, Android emulator DEVICE_NOT_FOUND errors,
  bulk failure aggregation across legs, "why does this test fail on some machines",
  "top 5 failing tests in the last 2 days", "download helix logs for build X",
  "compare passing and failing helix runs", "what are the most common failures".
  DO NOT USE FOR: high-level CI status checks (use ci-analysis), codeflow PRs
  (use flow-analysis).
  INVOKES: Helix and AzDO MCP tools, curl, gh CLI.
---

# Helix Investigation

Deep-dive into Helix test failures starting from an AzDO build leg. Goes beyond CI status checks to download raw console logs, compare passing vs failing runs, and identify root causes at the machine/device level.

## When to Use This Skill

- User has an AzDO build URL with a failing test leg and wants to understand why
- User wants to see raw Helix console logs for a specific work item
- User notices a test fails intermittently and wants pass/fail comparison
- User asks "why does this test only fail on some machines?"
- User wants to correlate AzDO build → Helix job → work item → console output
- User mentions XHarness, device tests, or Helix timeouts
- User asks for a "top N" list of failures across a pipeline or time range
- User wants to know if failures are machine-specific or systemic
- User sees Android DEVICE_NOT_FOUND or emulator boot failures

## Prerequisites

- Helix MCP tools (hlx-* prefix) for querying jobs, work items, and logs
- AzDO MCP tools (ado-* prefix) for querying builds and timelines
- `curl` for downloading raw log files when MCP search is insufficient
- Access to `https://helix.dot.net/` API endpoints

## Workflow

### Step 1: Start from the AzDO build

Given an AzDO build URL or pipeline definition + build ID:

1. Query the build timeline to find the failing job/leg
2. Identify the Helix job ID from the timeline records — look for `HelixJobId` in record properties or Helix URLs in the log output
3. Note the job name, queue, and failure category

```
AzDO Build → Timeline API → Failing job record → Helix job ID
```

> ⚠️ AzDO MCP tools may become unavailable mid-session. Fall back to the REST API: `https://dev.azure.com/{org}/{project}/_apis/build/builds/{buildId}/timeline?api-version=7.1`

### Step 2: Enumerate Helix work items

Query the Helix job for its work items. Each work item is a test group that ran on a specific machine.

Key fields to capture per work item:
- **Work item name** (e.g., `iOS.CoreCLR.R2R.Test`, `System.Runtime.Tests`)
- **Exit code** (0 = pass, 143 = SIGTERM/timeout, 71 = infra error)
- **Machine name** (e.g., `DNCENGMAC036`, `DNCENGTVOS-106`)
- **Duration** — long durations with exit 143 indicate timeout kills
- **Failure category** — `Crash`, `InfrastructureError`, etc.

### Step 3: Download console logs

The Helix console log URL pattern:

```
https://helix.dot.net/api/2019-06-17/jobs/{jobId}/workitems/{workItemName}/console
```

Download with curl for local analysis — this is often more reliable than Helix MCP search tools for finding specific patterns:

```bash
curl -s "https://helix.dot.net/api/2019-06-17/jobs/{jobId}/workitems/{workItemName}/console" > /tmp/{jobIdPrefix}-console.log
```

Also check for additional log files (test output, device logs):

```
https://helix.dot.net/api/2019-06-17/jobs/{jobId}/workitems/{workItemName}/files
```

**testResults.xml**: Many work items produce xUnit/NUnit result XML. Download it for structured failure data — test names, failure messages, stack traces, and pass/fail counts — especially when console logs lack detail:

```bash
# List files to find the results XML
curl -s "https://helix.dot.net/api/2019-06-17/jobs/{jobId}/workitems/{workItemName}/files" | python3 -c "import sys,json; [print(f['Name']) for f in json.load(sys.stdin)]"
# Download (path varies: testResults.xml, xharness-output/testResults.xml, etc.)
curl -sL "https://helix.dot.net/api/2019-06-17/jobs/{jobId}/workitems/{workItemName}/files/{fileName}" > /tmp/test-results.xml
# Quick failure summary
grep -c 'result="Fail"' /tmp/test-results.xml
grep 'result="Fail"' -A5 /tmp/test-results.xml | grep 'name='
```

> 💡 Helix MCP search tools (`hlx_search_log`, `hlx_search_file`) may miss patterns — they can be case-sensitive or have limited regex support. When searching for specific strings, download the file and use local `grep` for reliability.

### Step 4: Analyze the console log

Look for these key patterns in order:

1. **Setup phase** — machine name, queue, environment (OS version, Xcode version, device model)
2. **Test execution** — command line used (e.g., XHarness `apple run` with flags), app launch, test output
3. **Completion signals** — exit codes, timeout messages, crash reports
4. **Helix wrapper** — `WORKLOAD TIMED OUT`, `exit_code=`, `Command exited with`

Common failure signatures:

| Pattern | Meaning |
|---------|---------|
| `exit_code=143` + `WORKLOAD TIMED OUT` | Helix killed the process after timeout (SIGTERM) |
| `exit_code=71` | Infrastructure error |
| `exit_code=81` / `DEVICE_NOT_FOUND` | Android emulator not running or not responding |
| `exit_code=80` / `APP_CRASH` | XHarness device error — **may be false** (see below) |
| `Run timed out after N seconds` | XHarness hit its own timeout before Helix |
| `TIMED_OUT` (XHarness) | XHarness-level timeout (distinct from Helix timeout) |
| `mlaunch exited with {code}` | App completed — check if XHarness noticed |
| `Detected test end tag` then `APP_CRASH` | **False failure** — tests passed but device communication failed afterward |
| `Mercury error 1000` / `RSD error 0xE8000003` | devicectl file copy failure (tvOS/iOS) |
| Device log file empty/removed | Device communication issue (tvOS/iOS) |
| `emulator-5554` found but not `emulator-5556` | Wrong emulator architecture booted |
| `Attempt.3` in work item name | All retries exhausted |
| `Expected 0 exit code but got 1` (dotnet build) | SDK/MSBuild build failure — 100% fail rate, not a test bug |
| `System.TimeoutException: Timeout 30000ms exceeded` | Playwright browser automation timeout (Blazor) |
| `exit_code=133` (128+5=SIGTRAP) + `ASSERT FAILED` | Runtime assertion failure / core dump — every test suite crashes |

> ⚠️ **False failure detection is critical.** XHarness can report APP_CRASH (exit 80) or TIMED_OUT (exit 143) even when **all tests pass**. Always check for `"Detected test end tag"` or `"Test run completed"` in the log before trusting the exit code. If tests completed successfully but XHarness still reports failure, the issue is post-completion device communication, not a test bug.

### Step 5: Compare passing vs failing runs

This is the highest-value step for intermittent failures. Find a passing run of the same work item and download its console log too.

Compare side-by-side:
1. **Machine/device** — are failures concentrated on specific machines?
2. **Environment** — OS version, SDK version, device model differences
3. **Log divergence point** — where does the passing run succeed and the failing run hang or crash?
4. **File artifacts** — does the passing run produce files (device logs, test results) that the failing run doesn't?

Use a SQL table to track findings across multiple runs:

```sql
CREATE TABLE helix_runs (
  job_id TEXT, work_item TEXT, machine TEXT, exit_code INT,
  duration_sec INT, queue TEXT, os_version TEXT, passed BOOLEAN,
  notes TEXT
);
```

### Step 6: Identify root cause pattern

Common root cause categories:

| Category | Signal | Action |
|----------|--------|--------|
| **False failure** | Tests pass (`"Test run completed"`) but exit 80/143 reported | Device communication failed post-completion — file/update on dotnet/xharness |
| **Machine-specific** | Same test, same code, different outcomes by machine | Report machine name, suggest pool removal |
| **Device communication** | Empty device logs, log stream hangs, `Mercury error 1000`, `RSD error 0xE8000003` | USB/connectivity issue on host |
| **Emulator failure** | DEVICE_NOT_FOUND (exit 81), wrong emulator arch | Systemic if spread across machines; file on dotnet/xharness for recovery logic |
| **Test infrastructure** | XHarness/Helix wrapper bug | File issue on dotnet/xharness or dotnet/arcade |
| **Deterministic test bug** | 100% failure rate, same assertion, every run | Test expectation is wrong, not infra — file on dotnet/runtime, check for `blocking-clean-ci` label |
| **Genuine test failure** | Test code exercises broken runtime behavior | File issue on dotnet/runtime |
| **Timeout misconfiguration** | Test passes but harness timeout too short | Adjust timeout parameter |
| **Retry exhaustion** | `Attempt.3` still failing, retries hit same broken machine | Helix retries on same machine before reboot takes effect |
| **XML truncation** | All tests pass but app killed mid-result-write → truncated XML → crash reported | Large test suites on constrained devices (tvOS memory limits) |
| **SDK/build break** | `dotnet build` exit 1, 100% fail rate, error in SDK targets | Not a test bug — file on dotnet/aspnetcore or dotnet/sdk |
| **Browser automation** | Playwright timeout on UI element, app builds fine | Resource contention or Blazor rendering regression |
| **Poison PR** | Exit 133 (SIGTRAP), ALL suites fail on same build, 1x per platform per suite | Runtime assertion failure — single bad commit crashes before tests run. Filter out this build; failures are misleading |

### Step 7: Report findings

Provide:
1. **Summary table** — all analyzed runs with machine, exit code, pass/fail, key observation
2. **Root cause** — which category, with evidence from the logs
3. **Direct links** — Helix console log URLs, AzDO build URLs
4. **Recommended action** — machine removal, issue to file, timeout adjustment, etc.

## Fix Verification

When a PR is merged to fix a test failure, verify it's working by checking builds **queued after** the merge — not builds that **finished** after it.

### Why queue time matters

A build queued 10 minutes before a PR merges runs against the **old** source, even if it finishes hours later. Only builds queued after the merge include the fix.

### Workflow

1. Find the fix PR and its merge timestamp: `gh pr view {number} --repo {owner/repo} --json mergedAt`
2. Query recent builds for the pipeline — **include all statuses** (completed, inProgress, notStarted). If you only check completed builds, you'll miss active builds currently testing the fix.
3. For each build with the relevant failure, check its **queueTime** (not finishTime)
4. Partition into pre-merge and post-merge builds
5. If post-merge builds still show the failure → fix didn't work or there's a second issue
6. If only pre-merge builds fail → fix is confirmed clean

> ⚠️ **Common mistake**: Seeing a failure "after the merge" and concluding the fix didn't work. Always check queue time — the build may have started before the merge landed.

### Ancestry check shortcut

When queue time is ambiguous (merge happened mid-build), use commit ancestry:

```bash
gh api repos/{owner}/{repo}/compare/{merge_commit}...{build_source_version} --jq '.status'
# "ahead" or "identical" = fix is included; "behind" = fix is NOT included
```

## Bulk Failure Aggregation

When the user asks "what are the top N failures" across a pipeline or time range, use a different approach than single-build investigation.

### Approach

1. Query AzDO for all failed builds in the time range (e.g., last 2 days)
2. For each failed build, query the timeline to find which jobs/legs failed
3. For Helix-based legs, query work item results to get test names, exit codes, machines
4. Aggregate into a SQL table for analysis:

```sql
CREATE TABLE failure_survey (
  build_id INT, leg TEXT, work_item TEXT, exit_code INT,
  machine TEXT, failure_category TEXT, helix_job TEXT,
  is_pr_specific BOOLEAN, notes TEXT
);
```

5. Group by work item to find the most frequent failures
6. Separate PR-specific build errors (hit all legs at once) from recurring test failures

### Classification

| Type | Signal | Priority |
|------|--------|----------|
| **Recurring test failure** | Same work item fails across many PRs | High — investigate root cause |
| **PR-specific build error** | Multiple legs fail on same build only | Low — PR author's problem |
| **Poison PR** | ALL suites fail on one build, exit 133, 1x per platform | Low — filter out; single bad commit crashes runtime before tests run |
| **Infrastructure error** | exit 71, exit 81, DEVICE_NOT_FOUND | Medium — systemic infra issue |
| **Sporadic infra** | Single occurrence, no pattern | Ignore — noise |

### Rolling Builds vs PR Builds

When assessing **main branch health**, filter to rolling (scheduled) builds using `reasonFilter=schedule`. PR builds include broken PR branches that dominate failure counts and obscure systemic issues.

Real-world example: over the same 2-day window, PR builds produced **423 work item failures** (dominated by a single poison PR with 119 failures). Rolling builds had only **46 failures** — all genuine systemic issues.

```bash
# Rolling builds only (scheduled, typically 2x daily on main)
curl -s "https://dev.azure.com/{org}/{project}/_apis/build/builds?definitions={defId}&reasonFilter=schedule&resultFilter=failed&minTime={iso8601}&$top=50&api-version=7.1"
```

Use rolling builds to:
- Assess current main branch health
- Identify systemic test failures vs PR-introduced regressions
- Verify fix impact (compare rolling failure counts before/after merge)

### Cross-Leg View

Some tests fail on multiple platforms. When reporting top-N, group by test name but break out by leg to show which platforms are affected:

```
System.Runtime.Tests: 61 total
  → ios-arm64 Mono (13), tvos-arm64 Mono (20), android (9), ...
```

This reveals whether a failure is platform-specific or cross-platform.

## Machine Distribution Analysis

After identifying a recurring failure, determine whether it's **machine-specific** or **systemic**:

1. Collect the machine name from each failing work item
2. Count failures per machine
3. Compare to the pool size

| Distribution | Diagnosis | Action |
|-------------|-----------|--------|
| 1-2 machines account for most failures | **Machine-specific** — hardware/config issue | Report machines, suggest pool removal |
| Failures spread evenly across many machines | **Systemic** — software/infra issue | File infrastructure bug |
| Slight concentration on a few + spread | **Mixed** — systemic with some worse machines | File bug + flag worst offenders |

Example from real investigation: tvOS failures concentrated on DNCENGTVOS-036 and DNCENGTVOS-022 = machine-specific. Android DEVICE_NOT_FOUND spread across 17 machines = systemic emulator issue.

## Filing Issues from Investigation

When investigation reveals a clear root cause, file issues on the appropriate repo:

| Root Cause | File On | Include |
|------------|---------|---------|
| XHarness bug (timeout handling, exit code detection, device recovery) | [dotnet/xharness](https://github.com/dotnet/xharness) | Pass/fail comparison evidence, console log URLs, affected machines |
| Helix infrastructure (retry behavior, machine management) | [dotnet/arcade](https://github.com/dotnet/arcade) or dnceng issue tracker | Failure counts, machine distribution, retry logs |
| Test code bug | [dotnet/runtime](https://github.com/dotnet/runtime) | Failing test name, stack trace, repro steps |
| Machine hardware | dnceng infrastructure team | Machine names, failure frequency, environment details |

Always include:
- **Evidence**: Helix console log URLs and AzDO build URLs
- **Failure count**: How many times this occurred in the analysis window
- **Machine list**: Which machines are affected
- **Pass/fail comparison**: If intermittent, what differs between passing and failing runs

## Stop Signals

- **Stop downloading logs** after you have 1 clear failing run + 1 clear passing run for comparison. Don't download all 50 builds.
- **Stop analyzing** when you can explain why the test fails on machine A but passes on machine B. That's the root cause.
- **Stop if no passing runs exist** — the failure is 100% reproducible, not intermittent. Shift to analyzing the test code itself.

## Anti-Patterns

> 🚨 **Don't rely solely on Helix MCP search tools for log analysis.** Download the console log with curl and use local grep — it's faster and more reliable for pattern matching.

> 🚨 **Don't assume "flaky" without evidence.** If the same machine fails every time, it's machine-specific, not flaky. Capture the machine name.

> 🚨 **Don't ignore the "Exit code detection" warning as the root cause.** On tvOS/iOS 15+, XHarness logs this warning but it may be a red herring — the real issue can be device log streaming, not exit code detection itself.

> 🚨 **Don't analyze all builds in a pipeline.** Filter to the specific job/leg first, then look at 3-5 recent runs maximum.

## Reference: Known Helix Queue Patterns

| Queue Pattern | Platform |
|---------------|----------|
| `osx.*.amd64.appletv.open` | Apple TV devices (tvOS) |
| `osx.*.amd64.iphone.open` | iPhone devices (iOS) |
| `ubuntu.*.amd64.android.*.open` | Android emulators |
| `ubuntu.*` | Linux |
| `windows.*` | Windows |
| `browser-wasm.*` | WASM (Chrome/Firefox) |

Queue name is in the first line of every Helix console log: `workitem ... ({queue}) executed on machine {machine}`.

## Reference: Platform-Specific Resources

- [references/xharness-patterns.md](references/xharness-patterns.md) — tvOS/iOS XHarness exit code detection, device log streaming, timeout chain
- [references/android-patterns.md](references/android-patterns.md) — Android emulator DEVICE_NOT_FOUND, retry/reboot flow, common test failures
- [references/wasm-patterns.md](references/wasm-patterns.md) — WASM/browser SDK build errors, Playwright timeouts, work item naming
