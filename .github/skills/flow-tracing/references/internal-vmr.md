# Internal VMR Branches

## Branch Routing

| Branch pattern | Location | How to access |
|----------------|----------|---------------|
| `main`, `release/*` | GitHub `dotnet/dotnet` | `gh api` / GitHub MCP tools |
| `internal/*` | AzDO (internal repo, requires auth) | AzDO Git Items REST API via `az` CLI auth |

> ⚠️ **If the target branch starts with `internal/`**, do NOT try GitHub — it will 404. Go directly to AzDO.

## Reading Files from AzDO

Authenticate with `az account get-access-token`, then use the AzDO Git Items REST API to download file content. Key points:

- `az devops invoke` for Git items returns metadata, not file content — use the REST API directly with `$format=text` to get raw content
- Without `$format=text`, the API returns JSON metadata about the blob instead of the file itself
- Use `versionDescriptor.version` and `versionDescriptor.versionType=branch` to target a specific branch

## Internal Release Branch Semantics

`internal/release/X.Y.NNN` branches (e.g., `internal/release/10.0.106`) are **point-in-time snapshots** cut from `release/X.Y.1xx` for a specific servicing release.

- They are **frozen** after the cut — no continuous forward flow
- Changes reach them only through explicit cherry-picks or merges
- If a change landed in `release/X.Y.1xx` after the branch was cut, it missed the release
- To determine if a change made the cut, compare the component SHA in the internal branch's `source-manifest.json` against the change's merge commit
