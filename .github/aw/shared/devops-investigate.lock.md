<!-- AUTO-GENERATED — DO NOT EDIT -->
<!-- Source: devops-health-investigate.md knowledge compilation -->

# DevOps Investigation — Compiled Knowledge (dotnet/dotnet VMR)

This document contains category-specific investigation playbooks, root-cause patterns,
and remediation templates for the DevOps Health Investigation worker agent.
Adapted for the dotnet/dotnet VMR which uses Azure DevOps pipelines.

---

## 1. Pipeline Investigation Playbook (Azure DevOps)

When `finding_type == "pipeline"`:

### Step-by-Step Protocol

1. **Parse pre-collected build data** from `context_json` input:
   - The orchestrator passes error messages, failed step names, build URLs, and
     timeline excerpts as dispatch input strings
   - The worker does NOT have Azure DevOps API access — all ADO data is in the inputs

2. **Extract error details** from the context:
   - Failed task/step names
   - Error messages and exit codes
   - Build duration vs expected duration
   - Source branch and triggering commit

3. **Check recent commits** via GitHub API:
   - Correlate with codeflow PRs merged in the last 24h:
     ```
     GET /repos/{owner}/{repo}/pulls?state=closed&sort=updated&direction=desc&per_page=20
     ```
   - Filter to codeflow PRs (title contains "Source code updates from")
   - Check if the failing build's branch matches a recent merge

4. **Check other branches** from context:
   - Review whether the same pipeline is failing on other branches
   - Failing on multiple branches → systemic issue
   - Failing on one branch → branch-specific regression

5. **Cross-reference operational issues**:
   ```
   GET /repos/{owner}/{repo}/issues?state=open&labels=area-unified-build-OperationalIssue&per_page=50
   ```
   - Search for existing issues that match the same error pattern
   - Check if this is a known tracked problem

6. **Check upstream repo** via GitHub API:
   - If failure is in a constituent repo's build step (e.g., "Build runtime"),
     check that repo's recent activity:
     ```
     GET /repos/dotnet/{repo}/commits?per_page=10
     ```
   - Look for recent changes that could cause the failure

7. **Determine root cause** with confidence level:
   - **High**: Error message explicitly identifies the cause
   - **Medium**: Strong timing correlation with a specific commit/PR
   - **Low**: Multiple possibilities, no direct evidence

8. **Generate 1–3 specific remediation steps**:
   - Include Azure DevOps build links for manual follow-up
   - Reference specific repos, branches, or config files
   - Order by recommended priority

9. **Provide Azure DevOps links**:
   - Include direct links to the failed build from `resource_url`
   - The worker cannot browse Azure DevOps but can reference URLs for humans

10. **Check for existing tracking**:
    - Search for operational issues with matching error patterns
    - Reference any related GitHub issues or PRs

### Common VMR Pipeline Root Causes

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

---

## 2. Infrastructure Investigation Playbook

When `finding_type == "infra"`:

### Step-by-Step Protocol

1. **Audit the configuration**:
   - For stale operational issues: review the issue list for patterns
   - For codeflow backlog: identify which repos are blocked
   - For NuGet feed issues: check if the feed URL is correct

2. **Check if intentional**:
   - Search for issues or PRs that discuss the configuration choice
   - Check commit history of relevant config files

3. **Compare with best practices**:
   - Reference .NET repo standards for CODEOWNERS, Dependabot
   - Note any security implications

4. **For source manifest staleness**:
   - Check if the stale repo has recent commits in its upstream
   - If upstream has commits but manifest is stale → codeflow issue
   - If upstream has no recent commits → normal (low-activity repo)

---

## 3. Resource Investigation Playbook

When `finding_type == "resource"`:

### Step-by-Step Protocol

1. **Review pre-collected metrics** from `context_json`:
   - Pipeline run counts and durations from the data artifact
   - Compare against historical baselines

2. **Identify top consumers**:
   - Which pipelines/branches use the most compute time?
   - Has a new branch been added recently (release prep)?

3. **Compare to baseline**:
   - Is the increase justified (new release branch, preview builds)?
   - Check if new constituent repos were added

4. **Recommend optimization**:
   - Can any builds be consolidated?
   - Are there unnecessary rebuilds?
   - Can caching reduce build times?

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

### Confidence Level Guidelines

| Level | Criteria | Example |
|-------|----------|---------|
| **High** | Direct evidence links cause to effect | Error log says "OOM killed"; build step used 32GB |
| **Medium** | Strong circumstantial correlation | Pipeline failed right after codeflow merge from runtime |
| **Low** | Possible but speculative | Multiple recent changes could explain the issue |

---

## 5. Common Cross-Category Patterns

These patterns span multiple check categories and may help identify systemic issues:

| Pattern | Indicates |
|---------|-----------|
| Multiple pipeline failures on same branch | Common infrastructure root cause |
| Codeflow backlog + pipeline failures | Upstream repo change breaking VMR build |
| NuGet feed down + build failures | Infrastructure dependency issue |
| Source manifest stale + codeflow backlog | Codeflow system may be down |
| Build duration spike + new release branch | Expected growth from additional build matrix |
| Multiple operational issues created recently | Systemic build infrastructure problem |
