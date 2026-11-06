<!-- AUTO-GENERATED — DO NOT EDIT -->
<!-- Source: devops-health-check.md knowledge compilation -->

# DevOps Health Check — Compiled Knowledge (dotnet/dotnet VMR)

This document contains the health check catalog, fingerprinting rules, output templates,
and operational guidance for the DevOps Daily Health Check agentic workflow.
Adapted from dotnet/skills for the dotnet/dotnet Virtual Monolithic Repository,
which primarily monitors Azure DevOps pipelines rather than GitHub Actions.

---

## 1. Fingerprinting Rules

Every health finding MUST be assigned a deterministic **fingerprint** — a string ID
derived from the finding's category and key attributes (but NOT timestamps, run IDs,
or other ephemeral data). The same real-world issue MUST produce the same fingerprint
on every run.

### 1.1 Pipeline Fingerprints

For Azure DevOps pipeline failures:
```
fingerprint = "pipeline:ado:{org}:{definition_name}:{failed_task}:{result}"
```

For Azure DevOps timeouts:
```
fingerprint = "pipeline:ado:{org}:{definition_name}:timeout"
```

For build duration trends:
```
fingerprint = "pipeline:ado:{definition_name}:duration:{bucket}"
```
where bucket = "warning" or "critical"

For rolling build pass rate:
```
fingerprint = "pipeline:ado:rolling-build:pass-rate:{bucket}"
```

For codeflow PR failures:
```
fingerprint = "pipeline:codeflow:{source_repo}:failing"
```

For release branch health:
```
fingerprint = "pipeline:ado:{branch}:{definition_name}:latest-failed"
```
Normalize branch names: replace `/` with `-` (e.g., `release-10.0.2xx`)

For GitHub Actions failures:
```
fingerprint = "pipeline:gha:{workflow_name}:{conclusion}"
```

For source-build failures:
```
fingerprint = "pipeline:source-build:{branch}:failing"
```

**Normalization rules:**
- Lowercase all components
- Replace spaces with hyphens
- Replace `/` with `-` in branch names
- Same pipeline + same failure mode = same fingerprint across runs

**Examples:**
| Finding | Fingerprint |
|---------|-------------|
| dotnet-dotnet rolling build failed, "Build runtime" task | `pipeline:ado:dnceng-public:dotnet-dotnet:build-runtime:failed` |
| dotnet-dotnet-official timed out | `pipeline:ado:dnceng:dotnet-dotnet-official:timeout` |
| Rolling build pass rate < 50% | `pipeline:ado:rolling-build:pass-rate:critical` |
| Codeflow PR from runtime failing | `pipeline:codeflow:runtime:failing` |
| Source-build failing on main | `pipeline:source-build:main:failing` |
| backport.yml failed | `pipeline:gha:backport:failure` |

### 1.2 Infrastructure Fingerprints

```
fingerprint = "infra:{config_key}"
  where config_key ∈ {
    "stale-operational-issues:{bucket}",
    "codeflow-backlog:{bucket}",
    "no-codeowners",
    "no-dependabot",
    "unpinned-action:{action_name}",
    "nuget-feed-down:{feed_name}",
    "stale-manifest:{repo_name}",
    "branch-inventory"
  }
```

### 1.3 Resource Fingerprints

```
fingerprint = "resource:{metric}:{threshold_breach}"
```

- `resource:cost-increase` — weekly compute hours up >25%
- `resource:agent-pool:{pool_name}:warning` — pool utilization >90%
- `resource:agent-pool:{pool_name}:critical` — pool has 0 online agents

---

## 2. Diff Algorithm

```
previous_fps = cache_memory_load("health-check-fingerprints") ?? {}
current_fps  = {}

for each finding in all_collected_findings:
    fp = compute_fingerprint(finding)
    current_fps[fp] = finding

new_findings      = { fp: f for fp, f in current_fps  if fp NOT IN previous_fps }
existing_findings = { fp: f for fp, f in current_fps  if fp IN previous_fps }
resolved_findings = { fp: f for fp, f in previous_fps if fp NOT IN current_fps }

# Update occurrence tracking
for fp in existing_findings:
    existing_findings[fp].occurrences = previous_fps[fp].occurrences + 1
    existing_findings[fp].first_seen = previous_fps[fp].first_seen

for fp in new_findings:
    new_findings[fp].occurrences = 1
    new_findings[fp].first_seen = today

cache_memory_save("health-check-fingerprints", current_fps)
cache_memory_save("health-check-history", append(
    load("health-check-history"),
    { date: today, new_count, existing_count, resolved_count, by_severity }
))
```

### 2.1 Sorting Within Diff Categories

Within each category (NEW, EXISTING, RESOLVED):
1. **Primary**: Severity descending — 🔴 Critical → 🟡 Warning → 🔵 Info
2. **Secondary**: Category — pipeline → infra → resource
3. **Tertiary**: Alphabetical by title

---

## 3. Severity Rules Reference

### Pipeline

| Check | Condition | Severity |
|-------|-----------|----------|
| P1 | Official/internal pipeline failed on `main` | 🔴 Critical |
| P1 | Public CI pipeline failed on `main` | 🟡 Warning |
| P1 | Matches `known-noise` pattern | 🔵 Info (demoted) |
| P2 | Any timed-out build on `main` | 🟡 Warning |
| P2 | Timed-out build blocking release branch | 🔴 Critical |
| P3 | Build duration > 90% of timeout | 🔴 Critical |
| P3 | Build duration increased > 30% from 14d avg | 🟡 Warning |
| P4 | Rolling pass rate < 50% (7d) | 🔴 Critical |
| P4 | Rolling pass rate < 80% (7d) | 🟡 Warning |
| P5 | Codeflow PR failing > 48h with no intervention | 🔴 Critical |
| P5 | > 30% of recent codeflow PRs failing CI | 🟡 Warning |
| P6 | Latest official build failed on release branch | 🔴 Critical |
| P6 | Latest public CI failed on release branch | 🟡 Warning |
| P7 | GitHub Actions workflow failed | 🟡 Warning |
| P8 | Source-build failing > 24h on any active branch | 🔴 Critical |

### Infrastructure

| Check | Condition | Severity |
|-------|-----------|----------|
| I1 | Blocking operational issue open > 3 days | 🔴 Critical |
| I1 | > 10 open operational issues | 🟡 Warning |
| I2 | Codeflow PR > 3 days old (flow blockage) | 🔴 Critical |
| I2 | > 5 codeflow PRs open simultaneously | 🟡 Warning |
| I3 | Missing CODEOWNERS file | 🟡 Warning |
| I4 | Missing Dependabot config | 🟡 Warning |
| I5 | Unpinned third-party action | 🔵 Info |
| I6 | NuGet feed unreachable | 🔴 Critical |
| I7 | Source manifest repo > 7 days stale | 🟡 Warning |
| I8 | Branch without recent pipeline runs | 🔵 Info |

### Resource

| Check | Condition | Severity |
|-------|-----------|----------|
| U3 | Weekly compute up > 25% | 🟡 Warning |
| U4 | Agent pool has 0 online agents | 🔴 Critical |
| U4 | Agent pool utilization > 90% | 🟡 Warning |

---

## 4. Known Noise Patterns

The `cache-memory` key `known-noise` stores a list of fingerprint prefixes or patterns
that should be demoted to 🔵 Info severity. Example patterns:

- `pipeline:gha:backport` — backport workflow has known intermittent failures

When a finding's fingerprint matches any known-noise pattern (prefix match), demote
its severity to 🔵 Info. The finding is still reported in the output (in the EXISTING
section if recurring) — it is NOT hidden.

---

## 5. Investigation Dispatch Rules

Only 🆕 NEW findings that meet these criteria qualify for investigation dispatch:

| Condition | Action |
|-----------|--------|
| 🆕 + 🔴 Critical | **Always dispatch** |
| 🆕 + 🟡 Warning + `pipeline` category | **Dispatch** |
| 🆕 + 🟡 Warning + `infra` or `resource` category | **Skip** |
| 🆕 + 🔵 Info | **Never dispatch** |
| 📌 EXISTING or ✅ RESOLVED | **Never dispatch** |

**Budget cap:** Maximum 2 dispatches per run.
**Priority order when cap is hit:**
1. 🔴 Critical findings first
2. Pipeline findings before infrastructure
3. Other categories last

---

## 6. Output Templates

### 6.1 Issue Title

```
🏥 Repository Health Dashboard
```

### 6.2 Issue Label

```
devops-health
```
- Color: `#0E8A16`
- Description: `Daily automated health check report`

### 6.3 First Run Notice

If no previous fingerprints exist in `cache-memory`:

```markdown
> ⚠️ This is the first health check run. All findings appear as new.
> Starting from the next run, only changes will be highlighted.
```

### 6.4 Trends Arrow Legend

| Condition | Arrow | Meaning |
|-----------|-------|---------|
| Δ positive and good (e.g., pass rate up) | ✅ | Improving |
| Δ positive and bad (e.g., compute hours up) | ↗️ | Increasing (watch) |
| Δ negative and good (e.g., open issues down) | ✅ | Improving |
| Δ negative and bad (e.g., pass rate down) | ⚠️ | Degrading |
| Δ ≈ 0 | ➡️ | Stable |

### 6.5 Investigation Island Template

```markdown
<!-- investigation:{fingerprint} -->
⏳ Investigation dispatched — results arriving shortly...
<!-- /investigation:{fingerprint} -->
```

---

## 7. Operational Guardrails

### 7.1 API Rate Limits
- Use targeted, date-filtered queries to minimize API calls
- The `github` MCP toolset handles pagination automatically
- Space dispatches 5 seconds apart
- Azure DevOps data is pre-collected in Phase 1 (not called from the agent)

### 7.2 Issue Body Size
- GitHub issues have a ~65,535 character limit
- If body exceeds 60k: truncate EXISTING section (keep top 20 by severity)
- Footer: `> … N additional existing findings omitted`
- The daily comment always includes complete summary counts

### 7.3 Cache Memory Keys

| Key | Contents | Updated |
|-----|----------|---------|
| `health-check-fingerprints` | Map of fingerprint → finding (with occurrences, first_seen) | Every run |
| `health-check-history` | Array of daily summaries (date, counts by diff type and severity) | Appended each run |
| `known-noise` | Array of fingerprint patterns to demote to Info | Manual edit |

### 7.4 Graceful Degradation

If any data source is unavailable:
- Skip that check category entirely
- Note the skip in the output: `> ⚠️ Skipped {category} checks: {reason}`
- Do NOT fail the entire workflow
- Continue with available data

### 7.5 Cache Memory Loss

If `cache-memory` returns no previous state:
- Treat all findings as 🆕 NEW
- Display the first-run notice (§6.3)
- The diff will resume automatically on the next run

### 7.6 Azure DevOps Data

The agent does NOT have direct access to Azure DevOps APIs. All Azure DevOps data
is pre-collected by a standard GitHub Actions step (Phase 1) and passed into this
workflow as a single JSON blob via `needs.pre_activation.outputs.health_summary`.
The workflow saves that blob to `/tmp/health.json`, and the agent should read that
file with `cat` and `jq`.

The `/tmp/health.json` payload contains the collected health summary data needed
for analysis, including collection metadata, recent and historical build results,
timeline failure details, codeflow PR information, PR CI status, operational
issues, feed reachability, source manifest timestamps, and GitHub Actions failures.
