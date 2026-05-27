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

### Step 4: Present Initial Deletion Candidates

Compute the initial deletion candidates:

```
initialProtected = knownFalsePositives + notYetFlowedPackages (combined set)
candidatesForDeletion = unreferencedPackages - initialProtected
```

Display the results:

```
=== Unreferenced Packages Report ===

Protected (will NOT be deleted):
  - textOnlyPackages/microsoft.build.notargets/3.7.0 (known false positive)
  - textOnlyPackages/microsoft.build.traversal/3.4.0 (known false positive)
  - externalPackages/opentelemetry.api/1.x.x (not yet flowed to VMR)

Candidates for deletion (<N> packages):
  - <type>/<name>/<version>
  - <type>/<name>/<version>
  ...
```

**Ask the user:**
> "Are there any packages in the deletion list that should be kept because their
> uptake is still in progress? (i.e., the package was added to SBA but the consuming
> repo hasn't yet merged the PR that depends on it)"

If the user does not identify any such packages, proceed directly to Step 6.

### Step 5: Resolve Dependencies of Pending-Uptake Packages

If the user identifies packages whose uptake is still in progress, we need to find their
dependencies so those aren't deleted either. Rather than building a full dependency graph
of the entire repo upfront, only resolve dependencies for the specific packages that need
protection.

**For each pending-uptake package the user identifies:**

1. Build the repo (or the specific package) to produce NuGet packages:
   ```bash
   ./build.sh -sb --projects /full/path/to/<package>.csproj
   ```

2. Inspect the resulting `.nupkg` in `artifacts/packages/` to extract its dependencies:
   ```bash
   # List dependencies from the nuspec inside the nupkg
   unzip -p artifacts/packages/Release/Shipping/<PackageId>.<Version>.nupkg "*.nuspec" | grep -A1 "<dependency "
   ```

3. Cross-reference those dependencies against the deletion candidates. Any dependency that
   appears in the deletion list should also be protected.

4. Repeat transitively: if a newly-protected dependency itself has dependencies in the
   deletion list, protect those too.

After resolving all transitive dependencies, recompute and **re-present** the final state:

```
=== Updated Deletion Plan ===

Additionally protected (pending uptake + dependencies):
  - <type>/<package> (uptake in progress)
  - <type>/<dep-package> (dependency of <package>)

Final candidates for deletion (<N> packages):
  - <type>/<name>/<version>
  - ...
```

**Ask the user to confirm** the final deletion list before proceeding.

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
The PR body should only list the deleted packages — do not include protected/excluded packages
as that creates noise and raises questions for reviewers.

```bash
gh pr create \
  --repo dotnet/source-build-assets \
  --title "[main] Remove unreferenced packages" \
  --body "Removed the following packages which were detected as unreferenced:

<list each deleted package as type/name/version, one per line>

---
Generated by the [cleanup-unreferenced-packages](.github/skills/cleanup-unreferenced-packages/SKILL.md) skill."
```

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
- Packages with pending uptake should **not** be stored in the config file. They should be
  identified fresh by the user each time the skill runs. The expectation is that pending
  uptake is short-lived — either the consuming repo merges within the release cycle or the
  packages should be cleaned up.

## Config File Schema

**Location:** `.github/skills/cleanup-unreferenced-packages/cleanup-packages-config.json`

| Field | Type | Description |
|-------|------|-------------|
| `knownFalsePositives` | `string[]` | Packages that are always reported unreferenced due to detection limitations (e.g., MSBuild SDKs loaded implicitly by `global.json`). Format: `<type>/<name>/<version>` (e.g., `textOnlyPackages/microsoft.build.notargets/3.7.0`). These are permanent until the underlying detection issue is resolved. |
