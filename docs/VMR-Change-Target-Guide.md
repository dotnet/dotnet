# Developer Check-in Decision Guide

This guide helps you answer “What branch can I check into for my change?” using decision matrices for both **vNext/vNext Preview** and **Servicing/Patch** scenarios.

**Tip: When in doubt, ask.**

For help choosing a target, use the [pr-target skill](https://github.com/dotnet/arcade-skills/tree/main/plugins/dotnet-dnceng/skills/pr-target)
in the `dotnet-dnceng` plugin. It examines the change and current branch state,
asks for any missing information, and returns links to the branches to create PRs against.

See also:
- [Servicing Workflow Details](VMR-Servicing-Workflows.md)
- [Feature Band Constraints for Tooling Repo Devs](VMR-Feature-Band-Tooling-Constraints.md) — additional constraints (SDK version, source availability, etc.) when working in a non-1xx band branch.

---

## Table 1: vNext and vNext Preview Changes

Choose the table based on whether vNext servicing branches exist, then determine whether the next preview to ship has been branched and the change targets it.

### Servicing branches have not been created for vNext

| Branched for the next preview and change targeted at it? | Non-security change | Security change |
|---------------------------------------------------------|---------------------|-----------------|
| **No:** the preview branch does not exist, or the change does not target that preview | Check into public **main** in the VMR or component repository. | **Wait** for an internal preview branch to be created (e.g. `internal/release/11.0.0-rc.N`). |
| **Yes:** the preview branch exists and the change targets it | Check into the public **preview branch** in the VMR or component repository (e.g. `release/9.0.0-preview.3`). | Check into the **internal preview branch** in the VMR or component repository (e.g. `internal/release/11.0.0-rc.1`). |

### Servicing branches have been created for vNext

| Branched for the next preview and change targeted at it? | Non-security change | Security change |
|---------------------------------------------------------|---------------------|-----------------|
| **No:** the preview branch does not exist, or the change does not target that preview | Check into public **release/vNext** (e.g. `release/11.0.1xx`) in the VMR or component repository. | **Wait** for an internal preview branch to be created (e.g. `internal/release/11.0.1xx-rcN`). |
| **Yes:** the preview branch exists and the change targets it | Check into the public **preview branch** in the VMR or component repository (e.g. `release/11.0.1xx-preview3`). | Check into the **internal preview branch** in the VMR or component repository (e.g. `internal/release/11.0.1xx-rc1`). |

### Port to avoid regression

- **Non-security:** If you checked into a preview branch, port to **main** when servicing branches have not been created, or **release/vNext** (e.g. `release/11.0.1xx`) when they have. No port is needed when checking into those branches directly.
- **Security:** Port to **main** or **release/vNext** (e.g. `release/11.0.1xx`), as appropriate, **after** the public release.

---

## Table 2: Servicing (Patch/Hotfix) Changes

### 1. Confirm approval and select bands

Obtain Tactics approval for a specific servicing window before checking in. For 10.0+ releases, select the branches for the affected bands:

- **Runtime / shared-component fixes:** Target the **1xx band only**. Updated packages flow automatically to 2xx and higher bands.
- **SDK / band-specific fixes:** Target **each affected band** separately.

Runtime source lives in the 1xx band branch and release-specific branches derived from it. See [Managing SDK Bands – Servicing fixes across bands](./VMR-Managing-SDK-Bands.md#servicing-fixes-across-bands) for details.

### 2. Choose the destination

Compare the **approved release window** with the state of the servicing branches, then choose the column for the type of change. Apply the result to the bands selected above. A Release Specific branch takes precedence if it exists for the approved release, even if General Servicing still represents that release.

| Approved release and branch state | Non-security change | Security change |
|-----------------------------------|---------------------|-----------------|
| **Release Specific branch exists for the approved release** | Merge into the [internal VMR Release Specific branch](#internal-vmr-release-specific-branch). | Merge into the [internal VMR Release Specific branch](#internal-vmr-release-specific-branch). |
| **General Servicing represents the approved release**, and no Release Specific branch exists for it | Merge into [public General Servicing](#public-general-servicing). | Merge into [internal VMR General Servicing](#internal-vmr-general-servicing). |
| **General Servicing represents an earlier release**, and no Release Specific branch exists for the approved release | Open a [public General Servicing PR](#public-general-servicing); wait to merge until General Servicing represents the approved release. | Merge into the approved window's [internal VMR parking branch](#internal-vmr-parking-branch). |

During parking integration, follow the QB's published destination and do not submit to a frozen parking branch. OOBs use the explicitly approved OOB destination and consume no parking branch.

### 3. Port to main

- **Non-security:** Port to main as necessary.
- **Security:** Port to main as necessary on disclosure day.

### Destination details

#### Public General Servicing

Target the public General Servicing branch in the VMR or component repository, such as `release/10.0.1xx` or `release/10.0.2xx`. If the branch represents an earlier release, leave the PR unmerged until it represents the approved release; do not merge into servicing early. If a Release Specific branch exists for the approved release, use it instead.

#### Internal VMR parking branch

Target the internal VMR parking branch for the approved year, window, and band, such as `internal/parking/10.0.1xx/2027/1B` or `internal/parking/10.0.2xx/2027/1B`. Ask the QB to prepare the window if it is missing.

After the preceding release is cut, NETMechanic merges each parking branch into its matching internal General Servicing branch when the window opens and deletes the parking branch. Developers do not resubmit fixes to General Servicing or later parking windows. See [Release-Window Parking Branches](VMR-Servicing-Workflows.md#release-window-parking-branches).

#### Internal VMR General Servicing

Target the internal VMR General Servicing branch, such as `internal/release/10.0.1xx` or `internal/release/10.0.2xx`, and merge when it represents the approved window. On release day, the release tag merges shipped fixes into public General Servicing.

#### Internal VMR Release Specific branch

Target the internal VMR Release Specific branch for the approved release, such as `internal/release/10.0.105` or `internal/release/10.0.200`, and merge the fix there. On release day, the release tag merges shipped fixes into General Servicing.
