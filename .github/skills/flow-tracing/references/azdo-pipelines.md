# AzDO Pipeline Reference for VMR Builds

## Pipeline Overview

The VMR (`dotnet/dotnet`) is built by two pipeline tiers:

### Official Builds (produces SDK artifacts)

| Property | Value |
|----------|-------|
| **AzDO org** | `dnceng` (internal, requires auth) |
| **Project** | `internal` |
| **Pipeline** | `dotnet-unified-build` (definition 1330) |
| **Branches** | `main`, `release/X.Y.Nxx`, `release/X.Y.Nxx-previewN` |
| **Publishes to** | Maestro channels + blob storage (`ci.dot.net`) |

### Public CI (validation only)

| Property | Value |
|----------|-------|
| **AzDO org** | `dnceng-public` |
| **Project** | `public` (ID: `cbb18261-c48f-4abb-8651-8cdcb5474649`) |
| **Pipeline** | `dotnet-unified-build` (definition 278) |
| **Purpose** | PR validation + scheduled CI |
| **Does NOT publish** | to Maestro channels |

## Querying Official Builds

### Via AzDO MCP Tools

Use the `azure-devops-pipelines_get_builds` tool:
- `project`: `internal`
- `definitions`: `[1330]`
- `branchName`: `refs/heads/release/X.Y.Nxx`

> ⚠️ The AzDO MCP tools connect to the `dnceng` org by default. If builds return empty, verify the org configuration.

### Filtering by Date

To find a build for a specific SDK version:
1. Decode the SDK version date (see [sdk-version-format.md](sdk-version-format.md))
2. Use `minTime`/`maxTime` to narrow the search window to ±1 day of the decoded date
3. Match on `result: succeeded` or `result: partiallySucceeded` (some SDK builds succeed partially)

### Build Properties

Key fields in build results:

| Field | Meaning |
|-------|---------|
| `sourceVersion` | The VMR commit SHA that was built |
| `buildNumber` | Format `YYYYMMDD.N` (e.g., `20260217.3`) |
| `result` | `succeeded`, `partiallySucceeded`, `failed` |
| `sourceBranch` | Full ref (e.g., `refs/heads/release/10.0.3xx`) |

### Via aka.ms SDK Blob URLs

When you only need the latest SDK for a channel (not a specific version):

```powershell
# Resolve aka.ms → ci.dot.net blob URL (301 redirect)
# Channel examples: 11.0.1xx, 10.0.3xx, 10.0.1xx
$url = "https://aka.ms/dotnet/{channel}/daily/dotnet-sdk-win-x64.zip"
```

The blob URL contains the SDK version string, and `HEAD` gives the `Last-Modified` date.

## Channel-to-Branch Mapping

| Channel | VMR Branch | SDK Band |
|---------|-----------|----------|
| `.NET 11.0.1xx SDK` | `main` | 11.0.1xx |
| `.NET 11.0.1xx-preview1 SDK` | `release/11.0.1xx-preview1` | 11.0.1xx-preview1 |
| `.NET 10.0.3xx SDK` | `release/10.0.3xx` | 10.0.3xx |
| `.NET 10.0.2xx SDK` | `release/10.0.2xx` | 10.0.2xx |
| `.NET 10.0.1xx SDK` | `release/10.0.1xx` | 10.0.1xx |

## Common Pitfalls

1. **Wrong org**: SDK official builds are in `dnceng` (internal), NOT `dnceng-public`
2. **Pipeline name collision**: Both orgs have a `dotnet-unified-build` pipeline but with different definition IDs (278 public, 1330 internal)
3. **Date mismatch**: The aka.ms blob `Last-Modified` can differ from the AzDO build finish time by hours (signing/publishing delay). Use ±1 day search windows.
4. **Partially succeeded builds**: Some SDK builds publish artifacts even when individual platform legs fail. Don't filter to only `succeeded`.
