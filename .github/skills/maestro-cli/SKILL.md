---
name: maestro-cli
description: >
  Query Maestro/BAR dependency flow data using the mstro CLI tool via bash.
  USE FOR: subscription health checks, build flow tracing, codeflow status,
  channel discovery, triggering subscription updates — when MCP tools aren't
  loaded or when scripting with JSON output and jq. Also use when investigating
  "is this subscription stale", "what's the latest build", "check backflow status".
  DO NOT USE FOR: tasks where maestro MCP tools are already available in context
  (prefer flow-analysis or flow-tracing skills when MCP server is loaded).
  INVOKES: bash (mstro CLI commands with --json output).
---

# Maestro CLI

Query Maestro/BAR data via the `mstro` CLI tool instead of loading MCP tools into context. Same data, same caching, zero context tax — agents discover capabilities progressively through `--help` and `mstro guide`.

## When to Use This Skill

Use this skill when:
- You need Maestro/BAR data but the maestro MCP server isn't configured
- You're scripting or chaining commands with `jq`, `grep`, or other CLI tools
- You want structured JSON output for downstream processing
- The task is one-shot (not a multi-turn conversational investigation)

**Prefer MCP-based skills (flow-analysis, flow-tracing) when:**
- The maestro MCP server is already loaded in your tool list
- You're doing multi-turn conversational investigation
- You need markdown-formatted output with visual indicators

## Installation

```bash
dotnet tool install -g lewing.maestro.mcp
```

After installation, `mstro` is available globally. Verify: `mstro --help`

> Note: The same package (`lewing.maestro.mcp`) serves as both a .NET global tool (providing the `mstro` CLI) and an MCP server (via `dotnet dnx`). The CLI is what this skill uses.

## Authentication

Three-tier cascade (automatic fallback):
1. **`MAESTRO_BAR_TOKEN`** env var — explicit PAT
2. **Cached Entra ID** — reuses `darc authenticate` credentials from `~/.darc/.auth-record-*`
3. **Anonymous** — read-only fallback (may be rate-limited)

Run `darc authenticate` once for persistent credentials.

## Progressive Discovery

Don't memorize commands. Discover them:

```bash
mstro --help              # All commands, one line each
mstro <command> --help    # Parameters for a specific command
mstro <command> --schema  # JSON response field names (for jq pipelines)
mstro guide               # Workflow-organized guide (~3KB)
```

Before writing jq queries, run `--schema` to see the response shape:
```bash
mstro subscription-health --schema   # → shows StaleSubs[].Id, .BuildsBehind, etc.
mstro latest-build --schema          # → shows Id, Repository, Commit, etc.
```

## Common Patterns

All query commands support `--json` (structured output) and `--no-cache` (fresh data).

### Check subscription health
```bash
mstro subscription-health --target-repository https://github.com/dotnet/dotnet --json
```

### Find latest build
```bash
mstro latest-build https://github.com/dotnet/runtime --channel-name ".NET 10.0.1xx SDK" --json
```

### Check codeflow status
```bash
mstro codeflow-statuses --json
```

### Trace a build graph
```bash
mstro build-graph <build-id> --json
```

### Trigger a stale subscription
```bash
# Provide --source-repository + --channel-name (auto-resolves latest build)
mstro trigger-subscription <guid> --source-repository https://github.com/dotnet/runtime --channel-name ".NET 10.0.1xx SDK"
# Or provide --build-id directly. Add --force to overwrite stale PR branch.
```

## Chaining with jq

Use `mstro <command> --schema` to see all response fields before writing jq queries.

```bash
# Count stale subscriptions (StaleSubs[].Id, .BuildsBehind, .SourceRepository, .ChannelName)
mstro subscription-health --target-repository https://github.com/dotnet/dotnet --json | jq '.StaleSubs | length'

# Filter channels by name
mstro channels --json | jq '.[] | select(.Name | contains("10.0"))'

# Get build ID then trace graph
BUILD_ID=$(mstro latest-build https://github.com/dotnet/runtime --json | jq -r '.Id')
mstro build-graph $BUILD_ID --json
```

## Cache

Shared SQLite cache at `~/.mstro/cache.db` (WAL mode). Cache is shared between CLI and MCP server instances — using the CLI warms the cache for MCP and vice versa.

- `mstro cache status` — show cache stats
- `mstro cache clear` — clear all cached data
- `--no-cache` on any command bypasses cache
