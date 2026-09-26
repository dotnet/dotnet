<!-- Source: devops-health-investigate.md VMR-specific knowledge -->
<!-- General investigation methodology is provided by dotnet-dnceng plugin skills -->

# DevOps Investigation — VMR-Specific Knowledge (dotnet/dotnet)

This document contains VMR-specific investigation context that complements the
`dotnet-dnceng` plugin skills (pipeline-investigation, helix-investigation,
ci-analysis, flow-analysis). Use those skills as the primary investigation
methodology. This file provides VMR-specific root cause patterns and report
formatting required by the downstream grooming workflow.

---

## 1. VMR Pipeline Root Causes

These patterns are specific to the dotnet/dotnet VMR build and complement the
general failure categorization in the `pipeline-investigation` skill.

| Pattern | Typical Cause | Remediation |
|---------|---------------|-------------|
| tvOS/iOS build timeout | Cross-compilation target OOM or hang | Check runtime repo for tvOS changes; increase timeout |
| Signing failure | Certificate or key vault issue | Check with `@dotnet/product-construction` team |
| NuGet feed race | Package not yet available on feed | Retry build; check feed propagation |
| OOM during build | Memory-intensive compilation step | Check for memory regression in constituent repo |
| Cross-compilation crash | DAC build issue (Windows x86 pass 2) | Check runtime cross-DAC changes |
| Codeflow merge conflict | Conflicting changes across repos | Manual resolution needed on codeflow PR |
| Source-build failure | Pre-built package detection or poisoning | Check source-build patches in `src/<repo>/` |
| Installer test failure | deb/rpm packaging regression | Check installer changes in `src/installer/` |
| `exit code 57005` (0xDEAD) in `Windows_Pgo_*` legs | crossgen2 fatal crash | Check dotnet/runtime area-crossgen2-coreclr |
| `Binary Analysis Scan` + `exit code null` | 1ES infra tooling crash | 1ES PT team issue, not code bug |
| `SB_*_Validation_*` timeout epidemic | Run Tests timeout across source-build legs | Check test timeout config in dotnet/dotnet |
| Container OOM (`exit code null` on cross-compilation) | Container memory limit hit | Infrastructure — pool/container config |

### VMR Build Topology

- The VMR builds ~20 constituent repos in dependency order (see `repo-projects/`)
- Some repos build in multiple passes (e.g., runtime pass 2 for cross-OS DACs)
- Shared components (aspnetcore, runtime, winforms, wpf, efcore) are only built on 1xx feature bands
- Artifact cascade failures are common: if one repo fails, downstream repos fail with "missing artifacts" — always trace back to the root failure

### Key ADO Pipeline Definitions

| Pipeline | Org | Type | Notes |
|----------|-----|------|-------|
| dotnet-dotnet (ID: 278) | dnceng-public | Rolling CI | Public, no auth required |
| dotnet-dotnet-official (ID: 1330) | dnceng | Official | Internal, requires auth |

### AzDO Public API Access

For `dnceng-public/public` pipelines, query directly without authentication:
```bash
# Build timeline for failure analysis
curl -sf "https://dev.azure.com/dnceng-public/public/_apis/build/builds/{buildId}/timeline?api-version=7.1"

# Recent builds for frequency analysis (use reasonFilter=schedule for rolling builds)
curl -sf "https://dev.azure.com/dnceng-public/public/_apis/build/builds?definitions=278&branchName=refs/heads/main&resultFilter=failed&minTime={iso8601}&\$top=20&api-version=7.1"
```

---

## 2. Infrastructure Investigation (VMR-Specific)

When `finding_type == "infra"`:

- **Stale operational issues**: Review the `area-unified-build-OperationalIssue` issue list for patterns
- **Codeflow backlog**: Use `flow-analysis` skill patterns — check subscription health, forward flow blockers, and whether widespread staleness indicates VMR build failures (not Maestro issues)
- **Source manifest staleness**: Check `src/source-manifest.json` — if upstream repo has recent commits but manifest is stale, it's a codeflow issue. If upstream has no recent commits, it's normal (low-activity repo)
- **NuGet feed issues**: Check if the feed URL is correct and reachable

---

## 3. Resource Investigation

When `finding_type == "resource"`:

- Compare pipeline run counts against historical baselines
- Check if a new release branch was added (expected compute increase)
- Check if new constituent repos were added to the VMR
- Identify top compute consumers by pipeline/branch

---

## 4. Report Format

All investigation results MUST follow this exact template (the groomer depends on it):

```markdown
## 🔍 Investigation: {finding_title}

**Finding ID:** `{finding_id}`
**Severity:** {severity_emoji} {severity}
**Correlation:** {correlation_id}
**Executive Summary:** {one-sentence summary of root cause and recommended action}

### Root Cause
{one-paragraph description with evidence}

**Confidence:** {High|Medium|Low} — {justification}

### Blast Radius
{what else is affected}

### Suggested Fix
1. {step 1}
2. {step 2}
3. {step 3} (if applicable)

### Evidence
{key log excerpts, API responses, or code references in code blocks}

### Related
- {links to related issues, PRs, or commits — or "None found"}

---
<sub>🔍 [Investigation Run #{run_number}]({run_url}) · Dispatched by health check · {correlation_id}</sub>
```

**Critical parsing anchors** (groomer depends on these):
- Comment starts with `## 🔍 Investigation:`
- `**Finding ID:** \`{id}\`` appears on its own line
- `**Executive Summary:**` appears on its own line
- `**Correlation:**` appears on its own line

---

## 5. Cross-Category Patterns

These patterns span multiple check categories and help identify systemic issues:

| Pattern | Indicates |
|---------|-----------|
| Multiple pipeline failures on same branch | Common infrastructure root cause |
| Codeflow backlog + pipeline failures | Upstream repo change breaking VMR build |
| NuGet feed down + build failures | Infrastructure dependency issue |
| Source manifest stale + codeflow backlog | Codeflow system may be down |
| Build duration spike + new release branch | Expected growth from additional build matrix |
| Multiple operational issues created recently | Systemic build infrastructure problem |
