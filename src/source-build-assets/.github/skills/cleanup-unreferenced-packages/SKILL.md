---
name: cleanup-unreferenced-packages
description: |
  Intelligently removes unreferenced packages from source-build-assets (SBA).
  Accounts for pending-uptake packages, dependency graphs, and flow timing between SBA and the VMR.
---

# Cleanup Unreferenced Packages

## Prerequisites

- The `gh` CLI must be authenticated with access to `dotnet/dotnet` (the VMR) and
  `dotnet/source-build-assets` repos.
- The working directory must be the root of the `source-build-assets` repository.
- The local repo **must** be synced with upstream (`dotnet/source-build-assets`) main,
  not a personal fork's main. Before starting, ensure the working tree reflects upstream:
  ```bash
  git remote add upstream https://github.com/dotnet/source-build-assets.git 2>/dev/null || true
  git fetch upstream
  git checkout upstream/main
  ```
  All package existence checks and deletions must be performed against **upstream main**,
  not a fork's potentially stale main branch.

## Workflow

### Step 1: Obtain the SBA Usage Report

Download `sbaPackageUsage.json` from the latest successful VMR unified build
([dotnet-unified-build, definition 1330](https://dev.azure.com/dnceng/internal/_build?definitionId=1330))
on the **main branch**. This is an Azure DevOps pipeline.

```bash
# Find the latest successful build on main
az pipelines runs list \
  --organization https://dev.azure.com/dnceng \
  --project internal \
  --pipeline-ids 1330 \
  --branch main \
  --status completed \
  --result succeeded \
  --top 1 \
  --query "[0].id" -o tsv
```

Next, find the artifact containing the usage report. The artifact name follows the pattern
`SB_<distro>_Online_MsftSdk_x64_BuildLogs_Attempt1` where `<distro>` is the current
CentOS Stream version (e.g., `CentOSStream10`). List the build's artifacts to find the
matching one:

```bash
# List artifacts to find the CentOS Stream source-build leg
az pipelines runs artifact list \
  --organization https://dev.azure.com/dnceng \
  --project internal \
  --run-id <build_id> \
  --query "[?contains(name, 'SB_CentOSStream')].name" -o tsv
```

Download the report from the identified artifact:

```bash
az pipelines runs artifact download \
  --organization https://dev.azure.com/dnceng \
  --project internal \
  --run-id <build_id> \
  --artifact-name "<artifact_name>" \
  --path /tmp/vmr-report
```

If the `az` CLI is not available, the artifact can also be downloaded via the AzDO REST API
using `curl` with a PAT, or the user can provide a local path to the report directly.

Parse the JSON file and extract the `UnreferencedSbaPackages` array. Each entry is a path
indicating the package type and identity:

- `/__w/1/s/src/source-build-assets/src/referencePackages/src/<name>/<version>`
- `/__w/1/s/src/source-build-assets/src/textOnlyPackages/src/<name>/<version>`
- `/__w/1/s/src/source-build-assets/src/externalPackages/...`
- `/__w/1/s/src/source-build-assets/src/targetPacks/...`

Normalize these to `<type>/<name>/<version>` tuples (e.g., `referencePackages/system.buffers/4.5.1`).

### Step 2: Load the Cleanup Config

Read `.github/skills/cleanup-unreferenced-packages/cleanup-packages-config.json` from the repo.

```json
{
  "knownFalsePositives": ["textOnlyPackages/microsoft.build.notargets/3.7.0", ...]
}
```

These packages are **protected** — they must not be deleted.

### Step 3: Determine Packages Not Yet Flowed to VMR

SBA tip-of-main may contain package additions or deletions that haven't flowed into the
VMR yet. Additions would appear unreferenced because the VMR hasn't seen them. Deletions
would appear unreferenced because the VMR still has the old state — this is expected and
those should still be candidates for deletion (they're already gone from SBA).

**How to detect:**

1. Fetch the VMR's `src/source-manifest.json` to find which SBA commit is incorporated:
   ```bash
   gh api repos/dotnet/dotnet/contents/src/source-manifest.json --jq '.content' | base64 -d | jq -r '.repositories[] | select(.path == "src/source-build-assets") | .commitSha'
   ```

2. Compare that commit to the current HEAD:
   ```bash
   git log --oneline <vmr-sba-sha>..HEAD -- src/
   ```

3. Identify package directories that were **added** in that range (additions that haven't
   flowed are the ones we need to protect):
   ```bash
   git diff --name-status --diff-filter=A <vmr-sba-sha>..HEAD -- src/referencePackages/src/ src/textOnlyPackages/src/ src/externalPackages/ src/targetPacks/
   ```

4. Extract the package identifiers (`<name>/<version>`) from added paths. These are
   **not-yet-flowed** packages — add them to the protected set.

Note: Packages that were **deleted** in SBA since the VMR commit may still appear in the
unreferenced report (because the VMR still has them). This is fine — leave them in the
deletion candidates since they've already been intentionally removed from SBA.

### Step 4: Classify Candidates by Age

Apply a tiered grace-period strategy based on when each package was added to the repository.

Compute the initial deletion candidates:

```
initialProtected = knownFalsePositives + notYetFlowedPackages (combined set)
candidatesForDeletion = unreferencedPackages - initialProtected
```

**Determine the age of each candidate** by finding the commit that first introduced its
directory:

```bash
# Get the date when a package was first added
git log --diff-filter=A --follow --format=%aI -- "src/<type>/src/<name>/<version>/" | tail -1
```

If no commit is found (e.g., the package predates the repo's git history), treat the
package as having an age exceeding the mature threshold.

Apply the following classification:

| Age | Classification | Action |
|-----|---------------|--------|
| < 14 days | Recent | Protected — skip entirely |
| 14–60 days | Middle window | Delete, but @-mention author in PR |
| > 60 days | Mature | Delete |

### Step 5: Identify Authors for Middle-Window Packages

For packages in the middle window (added between 14 and 60 days ago), identify the
original author so they can be @-mentioned in the PR.

**Identify the original author:**

```bash
# Get the commit that added the package
git log --diff-filter=A --follow --format="%H %an <%ae>" -- "src/<type>/src/<name>/<version>/" | tail -1
```

Then find the associated PR to get the GitHub username:

```bash
# Find PR associated with the commit
gh pr list --repo dotnet/source-build-assets --search "<commit_sha>" --state merged --json number,author --jq '.[0].author.login'
```

Record the author and addition date for each middle-window package — these will be
included in the PR body (see Step 7).

### Step 5a: Resolve Dependencies of Middle-Window Packages

Middle-window packages may have transitive dependencies that are also in the deletion
candidate list. Those dependencies should also be treated as middle-window (and their
authors notified in the PR).

For each middle-window package, resolve its dependencies:

1. Build the package to produce a `.nupkg`:
   ```bash
   ./build.sh -sb --projects /full/path/to/<package>.csproj
   ```

2. Extract dependencies from the nupkg:
   ```bash
   unzip -p artifacts/packages/Release/Shipping/<PackageId>.<Version>.nupkg "*.nuspec" | grep -A1 "<dependency "
   ```

3. Any dependency that appears in the mature deletion list should be reclassified as
   middle-window (attributed to the same author). These will appear in the Action
   Required table in the PR alongside the package that depends on them.
   Repeat transitively.

Display the final classification:

```
=== Cleanup Report ===

Protected (will NOT be deleted):
  - textOnlyPackages/microsoft.build.notargets/3.7.0 (known false positive)
  - referencePackages/some.new.package/1.0.0 (recent: added 5 days ago)

Deleting (<N> packages):
  Mature:
    - <type>/<name>/<version> (age: 95 days)
  Middle window (authors will be notified in PR):
    - <type>/<name>/<version> (age: 25 days, author: @username)
  Dependencies of middle-window packages:
    - <type>/<name>/<version> (dependency of <parent-package>, <parent-package-2>)
```

Proceed to Step 6 with both **mature** and **middle-window** packages.

### Step 6: Delete Packages

For each package in the confirmed deletion list:

1. Determine the package type from its path:
   - `src/referencePackages/src/<name>/<version>/` → reference package
   - `src/textOnlyPackages/src/<name>/<version>/` → text-only package
   - `src/targetPacks/src/<name>/<version>/` → targeting pack
   - `src/externalPackages/` → external package (includes submodule, project, patches)

2. Delete the package directory:
   ```bash
   rm -rf src/referencePackages/src/<name>/<version>
   # or
   rm -rf src/textOnlyPackages/src/<name>/<version>
   # or
   rm -rf src/targetPacks/src/<name>/<version>
   ```

3. For external packages, also:
   - Remove the submodule: `git rm src/externalPackages/src/<component>`
   - Remove the project file: `rm src/externalPackages/projects/<component>.proj`
   - Remove patches: `rm -rf src/externalPackages/patches/<component>/`

4. Check if deleting a versioned package leaves an empty parent directory (e.g., the
   `<name>/` dir has no more versions) and clean it up if so.

### Step 7: Commit and Create PR

```bash
git add -A
git commit -m "Remove unreferenced packages

Removed <N> packages that are no longer referenced by the VMR build.

Co-authored-by: Copilot <223556219+Copilot@users.noreply.github.com>"
```

Create a PR **against the upstream repo** (`dotnet/source-build-assets`), not a personal fork.

If there are middle-window packages, include an "Action Required" section in the PR body
that @-mentions the original authors. This gives them visibility and a chance to revert
specific deletions before the PR merges.

```bash
gh pr create \
  --repo dotnet/source-build-assets \
  --title "[main] Remove unreferenced packages" \
  --body "Removed the following packages which were detected as unreferenced:

<list each deleted package as type/name/version, one per line>

## Action Required

The following packages were added recently but are not yet referenced in the VMR.
If uptake is still in progress, ask Copilot to revert these package deletions from
this PR.

| Package | Added | Author |
|---------|-------|--------|
| \`<type>/<name>/<version>\` | <N> days ago | @<author> |

Dependencies of the above packages (also being removed):
- \`<type>/<name>/<version>\` (dependency of \`<parent-package>\`, \`<parent-package-2>\`)

---
Generated by the [cleanup-unreferenced-packages](.github/skills/cleanup-unreferenced-packages/SKILL.md) skill."
```

Omit the "Action Required" section entirely if there are no middle-window packages.
Omit the "Dependencies" list if there are no reclassified transitive dependencies.

After the PR is created, trigger a full VMR source build to validate the removals:

```bash
gh pr comment <pr_number> --repo dotnet/source-build-assets --body "/azp run source-build-assets-unified-build"
```

## Important Notes

- The `knownFalsePositives` in the config exist because `microsoft.build.notargets` and
  `microsoft.build.traversal` are MSBuild SDKs referenced in `global.json` — the usage
  reporter doesn't detect SDK-style usage.
- When in doubt, err on the side of keeping a package. It's much easier to delete later
  than to re-add and coordinate flows.
- Middle-window author notifications happen directly in the PR body via @-mentions.
  If a package's uptake is still in progress, the author can ask Copilot to revert
  those specific deletions from the PR.

## Config File Schema

**Location:** `.github/skills/cleanup-unreferenced-packages/cleanup-packages-config.json`

| Field | Type | Description |
|-------|------|-------------|
| `knownFalsePositives` | `string[]` | Packages that are always reported unreferenced due to detection limitations (e.g., MSBuild SDKs loaded implicitly by `global.json`). Format: `<type>/<name>/<version>` (e.g., `textOnlyPackages/microsoft.build.notargets/3.7.0`). These are permanent until the underlying detection issue is resolved. |
