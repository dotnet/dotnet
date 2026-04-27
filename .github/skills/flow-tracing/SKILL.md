---
name: flow-tracing
description: >
  Trace dependency flow across .NET repos through the VMR pipeline.
  USE FOR: checking if a PR/commit from repo A has reached repo B,
  finding what runtime SHA is in an SDK build, tracing dependency
  versions through the VMR, checking if a commit is included in an
  SDK build, decoding SDK version strings, "has my fix reached runtime",
  "did roslyn#80873 flow to runtime", "what SHA is in SDK version X",
  cross-repo dependency tracing, mapping SDK versions to VMR commits.
  DO NOT USE FOR: codeflow PR health or staleness (use flow-analysis
  skill), CI build failures (use ci-analysis skill).
  INVOKES: maestro and GitHub MCP tools, Get-SdkVersionTrace.ps1 script.
---

# Flow Tracing

Trace dependency flow across .NET repositories through the VMR pipeline. Two workflows:

1. **Cross-repo flow trace**: Has a change from repo A reached repo B? → Use Steps 1-4 below (reads GitHub files + maestro MCP directly)
2. **SDK version trace**: What component SHA is in a specific SDK version? → **Run `Get-SdkVersionTrace.ps1`** (handles version decoding, build lookup, and servicing topology automatically)

## Cross-Repo Flow Trace

**Question**: "Has change X from repo A reached repo B?"

### Step 1: Resolve the Source Change

Identify the merge commit SHA in repo A from the PR number, issue number, commit SHA, or description the user provides. If the PR isn't merged yet, stop — the change hasn't entered the pipeline.

### Step 2: Check VMR Intake (source-manifest.json)
> ⚠️ **Internal branches** (`internal/release/*`) exist only on AzDO, not GitHub. Use the AzDO REST API to read files. See [references/internal-vmr.md](references/internal-vmr.md).

Read `src/source-manifest.json` from `dotnet/dotnet` on the target VMR branch (usually `main` or `release/*`). This file is the authoritative record of what the VMR has actually consumed — subscription status reflects Maestro's bookkeeping, but the manifest reflects reality. Find the entry for repo A — the `commitSha` field shows the latest commit the VMR has consumed.

**Determine if the change is included** (try in order):
1. **Date comparison** (fastest): If the VMR commit date is months after the PR merge date, it's included.
2. **Compare API**: Use GitHub compare endpoint if dates are close.
3. **Commit history walk**: List recent commits if compare is unavailable.

If repo A's SHA in source-manifest is not past the merge commit → the change hasn't reached the VMR yet. Check for an open forward flow PR from repo A into `dotnet/dotnet`.

> ⚠️ **2xx/3xx bands**: Only the **1xx branch** source-builds all components. If tracing to a 2xx/3xx branch, runtime/aspnetcore won't appear in source-manifest — they're consumed as prebuilts from 1xx. See [references/servicing-topology.md](references/servicing-topology.md).

### Step 3: Check Downstream Delivery (repo B)

If the VMR has the change, check if it has flowed to repo B:

> ⚠️ **`eng/Version.Details.xml`** is the file you want — it contains source dependency entries with `Sha` fields. Do NOT use `eng/Versions.props` (that has NuGet package versions, not source SHAs).

1. **Check subscription health** for repo B using maestro MCP.
2. **If current**: The change has reached repo B. Confirm by reading `eng/Version.Details.xml` in repo B.
3. **If stale**: Read `eng/Version.Details.xml` in repo B — the `dotnet/dotnet` entry's `Sha` is the VMR commit repo B last consumed. Check `src/source-manifest.json` at *that SHA* for repo A's commitSha. If it's past the merge commit, the change reached repo B despite the stale subscription.
4. **If stale AND change not in consumed VMR SHA**: Change is in the VMR but hasn't flowed to repo B. Suggest flow-analysis skill for diagnosis.

### Step 4: Report

Summarize the trace chain:
- "✅ roslyn#80873 merged at `abc123` → VMR consumed it → runtime backflow current"
- "⚠️ Change in VMR → but runtime backflow is 3 builds behind"
- "❌ PR hasn't merged yet — not in pipeline"

## SDK Version Trace

Run the script to trace from an SDK version string to a component commit SHA:

```powershell
# Trace runtime SHA in a specific SDK version
./scripts/Get-SdkVersionTrace.ps1 -SdkVersion "10.0.300-preview.26117.103"

# Trace a specific component
./scripts/Get-SdkVersionTrace.ps1 -SdkVersion "10.0.300-preview.26117.103" -Component "aspnetcore"

# Check if specific commits are included
./scripts/Get-SdkVersionTrace.ps1 -SdkVersion "10.0.300-preview.26117.103" -CheckCommit "b226ba1f77a4","f3bc0212e637"

# Just decode the version string
./scripts/Get-SdkVersionTrace.ps1 -SdkVersion "10.0.300-preview.26117.103" -DecodeOnly
```

The script decodes the SDK version, maps to a VMR branch, finds the build in AzDO, and walks the dependency chain through `source-manifest.json` (and `Version.Details.xml` for servicing branches that don't source-build all components).

> ⚠️ **SDK version dates use Arcade's SHORT_DATE formula**: `YY*1000 + MM*50 + DD` (NOT YYDDD day-of-year). `26117` = Feb 17, 2026, NOT April 27. See [references/sdk-version-format.md](references/sdk-version-format.md).

> ⚠️ **Servicing branches (2xx, 3xx)** do NOT source-build runtime. The script automatically follows the dependency chain through `Version.Details.xml` to the 1xx branch. See [references/servicing-topology.md](references/servicing-topology.md).

## References

- **Internal VMR branches**: See [references/internal-vmr.md](references/internal-vmr.md)
- **SDK version format**: See [references/sdk-version-format.md](references/sdk-version-format.md)
- **Servicing branch topology**: See [references/servicing-topology.md](references/servicing-topology.md)
- **AzDO pipeline IDs and queries**: See [references/azdo-pipelines.md](references/azdo-pipelines.md)
