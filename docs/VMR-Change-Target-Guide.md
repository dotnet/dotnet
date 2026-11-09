# Developer Check-in Decision Guide

This guide helps you answer “What branch can I check into for my change?” using detailed condition tables for both **vNext/vNext Preview** and **Servicing/Patch** scenarios.

**Tip: When in doubt, ask.**

See also:
- [Servicing Workflow Details](VMR-Servicing-Workflows.md)
- [Feature Band Constraints for Tooling Repo Devs](VMR-Feature-Band-Tooling-Constraints.md) — additional constraints (SDK version, source availability, etc.) when working in a non-1xx band branch.

---

## Table 1: vNext and vNext Preview Changes

### Servicing branches have not been created for vNext

| Condition(s) | Where Can I Check In? | Target branch visibility | Do I need to port the change to avoid regression? |
|--------------|----------------------|--------------------------|--------------------------------------------------|
| - Change is non-security **AND** <br/> - Preview branch has not been created **OR** change is not targeted at next preview to ship | **main** in VMR or component repository | Public | No |
| - Change is non-security **AND** <br/> - Preview branch has been created **AND** <br/> - Change is targeted at next preview to ship | **Preview branch** (e.g. `release/9.0.0-preview.3`) in VMR or component repository. | Public | Yes. Port to main |
| - Change is security **AND** <br/> - Preview branch has not been created **OR** change is not targeted at next preview to ship | **Wait** for creation of internal preview branches (e.g. internal/release/11.0.0-rc.N branch) | Internal (wait for branch creation) | **main**, **AFTER** public release. |
| - Change is security **AND** <br/> - Preview branch has been created **AND** <br/> - Change is targeted at next preview to ship | **internal/release/preview.N** (e.g. `internal/release/11.0.0-rc.1`) in VMR or component repository. | Internal | **main**, **AFTER** public release. |

### Servicing branches have been created for vNext

| Condition(s) | Where Can I Check In? | Target branch visibility | Do I need to port the change to avoid regression? |
|--------------|----------------------|--------------------------|--------------------------------------------------|
| - Change is non-security **AND** <br/> - Preview branch has not been created **OR** change is not targeted at next preview to ship | **release/vNext** in VMR or component repository | Public | No |
| - Change is non-security **AND** <br/> - Preview branch has been created **AND** <br/> - Change is targeted at next preview to ship | **Preview branch** (e.g. `release/9.0.1xx-preview3`) in VMR or component repository. | Public | Yes. Port to release/vNext |
| - Change is security **AND** <br/> - Preview branch has not been created **OR** change is not targeted at next preview to ship | **Wait** for creation of internal preview branches (e.g. internal/release/11.0.1xx-rcN branch) | Internal (wait for branch creation) | **release/vNext**, **AFTER** public release. |
| - Change is security **AND** <br/> - Preview branch has been created **AND** <br/> - Change is targeted at next preview to ship | **internal/release/preview.N** (e.g. `internal/release/11.0.1xx-rc1`) in VMR or component repository. | Internal | **release/vNext**, **AFTER** public release. |

---

## Table 2: Servicing (Patch/Hotfix) Changes

> **Multi-band note (10.0+):** For 10.0+ releases, the runtime source code only lives in the **1xx band branch** (e.g., `release/10.0.1xx`) and in release-specific branches derived from it (e.g., `internal/release/10.0.105`). Non-1xx band branches consume the runtime as build output packages that automatically flow from the 1xx build.
>
> - **Runtime / shared-component fixes** – Check the fix into the **1xx band branch only** (e.g., `release/10.0.1xx` or `internal/release/10.0.1xx`). The updated packages flow automatically to 2xx and higher band branches.
> - **SDK / band-specific fixes** – Check the fix into **each band branch** that requires it (e.g., both `release/10.0.1xx` and `release/10.0.2xx`).
>
> See [Managing SDK Bands – Servicing fixes across bands](./VMR-Managing-SDK-Bands.md#servicing-fixes-across-bands) for full details.

Choose the branch representing the **approved release window**, not necessarily the nearest calendar release. If that window has not reached General Servicing, security fixes use its internal VMR parking branch. Non-security fixes wait in unmerged PRs targeting public General Servicing until their approved window opens; do not merge them early. For example, `internal/parking/10.0.1xx/2027/1B` merges into `internal/release/10.0.1xx`, while `internal/parking/10.0.2xx/2027/1B` merges into `internal/release/10.0.2xx`. Runtime/shared fixes target only 1xx; SDK/band-specific fixes target each affected band.

NETMechanic integrates and deletes parking branches when their General Servicing window opens, after the preceding release cut. Developers do not resubmit fixes to General Servicing or later parking windows. Once the approved window is cut, new fixes go directly to its Release Specific branch. During parking integration, follow the QB's published destination and do not submit to a frozen parking branch. OOBs use the explicitly approved OOB destination and consume no parking branch. See [Release-Window Parking Branches](VMR-Servicing-Workflows.md#release-window-parking-branches).

| Condition(s) | Where Can I Check In? | Target branch visibility | Do I need to port the change to avoid regression? |
|--------------|----------------------|--------------------------|--------------------------------------------------|
| - Change is non-security **AND** <br/> - Approved window is represented by General Servicing <br/> - **Runtime/shared-component fix (10.0+)** | **General Servicing 1xx band branch** in VMR or component repository (e.g. `release/10.0.1xx`) | Public | Port to main as necessary on disclosure day |
| - Change is non-security **AND** <br/> - Approved window is represented by General Servicing <br/> - **SDK/band-specific fix (10.0+)** | **Each affected General Servicing band branch** in VMR or component repository (e.g. `release/10.0.1xx` and `release/10.0.2xx`) | Public | Port to main as necessary on disclosure day |
| - Change is non-security **AND** <br/> - Approved window is beyond General Servicing | **Unmerged PR against public General Servicing** in VMR or component repository. Wait to merge until the approved window opens. Runtime/shared fixes target only 1xx; SDK/band-specific fixes target each affected band. | Public | Port to main as necessary; do not merge into servicing before the approved window. |
| - Change is security **AND** <br/> - Approved window is beyond General Servicing | **Internal VMR parking branch for the approved year/window and band** (e.g. `internal/parking/10.0.1xx/2027/1B`). Runtime/shared fixes target only 1xx; SDK/band-specific fixes target each affected band. Ask the QB to prepare the window if missing. | Internal | Mechanic merges into the matching internal General Servicing branch when the window opens; no developer resubmission. Port to main as necessary on disclosure day. |
| - Change has no approved servicing window | Obtain Tactics approval before checking into servicing | N/A (no check-in yet) | N/A |
| - Change is non-security **AND** <br/> - Release specific branches exist for the approved window (e.g. internal/release/10.0.105) <br/> - **Runtime/shared-component fix (10.0+)** | **Internal 1xx release specific branch in VMR** (e.g. `internal/release/10.0.105`) | Internal | Will merge to General Servicing branch on release day. Port to main as necessary on disclosure day. |
| - Change is non-security **AND** <br/> - Release specific branches exist for the approved window (e.g. internal/release/10.0.105) <br/> - **SDK/band-specific fix (10.0+)** | **Each affected internal release specific branch in VMR** (e.g. `internal/release/10.0.105` and `internal/release/10.0.200`) | Internal | Will merge to General Servicing branches on release day. Port to main as necessary on disclosure day. |
| - Change is security **AND** <br/> - Release specific branches exist for the approved window (e.g. internal/release/10.0.105) | **Internal release specific branch in VMR** (e.g. internal/release/10.0.105). For SDK/band-specific fixes, target each affected band's release specific branch. | Internal | Will merge to General Servicing branch on release day. Port to main as necessary on disclosure day.|
| - Change is security **AND** <br/> - Approved window is represented by General Servicing | **Internal 1xx band branch** (e.g. `internal/release/10.0.1xx`) for runtime/shared fixes. For SDK/band-specific fixes, target each affected `internal/release/10.0.Nxx` branch. | Internal | Will merge to General Servicing branch on release day. Port to main as necessary on disclosure day. |

---

## Quick Reference Decision Diagrams

**vNext/vNext Preview (servicing branches not created):**

```mermaid
flowchart TD
    classDef decision fill:#fff4e5,stroke:#d35400,stroke-width:1px;
    classDef action fill:#e3f2fd,stroke:#1565c0,stroke-width:1px;
    classDef wait fill:#f5f5f5,stroke:#8d6e63,stroke-dasharray:5 3;

    start([Start])
    security{Is the change<br/>security-related?}
    nonSecPreviewCut{Has the preview branch<br/>been created?}
    nonSecTargetNext{Is it targeted at the<br/>next preview to ship?}
    secPreviewCut{Has the preview branch<br/>been created?}
    secTargetNext{Is it targeted at the<br/>next preview to ship?}

    mainDev["main<br/>(VMR or component)"]
    previewBranchMain["Preview branch<br/>(e.g. release/9.0.0-preview.3)<br/><br/>Port to main"]
    waitInternalMain["Wait for internal<br/>preview branch<br/><br/>Port to main after<br/>public release"]
    internalPreviewMain["internal/release/preview.N<br/><br/>Port to main after<br/>public release"]

    start --> security
    security -- "No" --> nonSecPreviewCut
    security -- "Yes" --> secPreviewCut

    nonSecPreviewCut -- "No" --> mainDev
    nonSecPreviewCut -- "Yes" --> nonSecTargetNext
    nonSecTargetNext -- "Yes" --> previewBranchMain
    nonSecTargetNext -- "No" --> mainDev

    secPreviewCut -- "No" --> waitInternalMain
    secPreviewCut -- "Yes" --> secTargetNext
    secTargetNext -- "Yes" --> internalPreviewMain
    secTargetNext -- "No" --> waitInternalMain

    class security,nonSecPreviewCut,nonSecTargetNext,secPreviewCut,secTargetNext decision;
    class mainDev,previewBranchMain,internalPreviewMain action;
    class waitInternalMain wait;
```

**vNext/vNext Preview (servicing branches created):**

```mermaid
flowchart TD
    classDef decision fill:#fff4e5,stroke:#d35400,stroke-width:1px;
    classDef action fill:#e3f2fd,stroke:#1565c0,stroke-width:1px;
    classDef wait fill:#f5f5f5,stroke:#8d6e63,stroke-dasharray:5 3;

    start([Start])
    security{Is the change<br/>security-related?}
    nonSecPreviewCut{Has the preview branch<br/>been created?}
    nonSecTargetNext{Is it targeted at the<br/>next preview to ship?}
    secPreviewCut{Has the preview branch<br/>been created?}
    secTargetNext{Is it targeted at the<br/>next preview to ship?}

    releaseVNext["release/vNext<br/>branch"]
    previewBranchRelease["Preview branch<br/>(e.g. release/9.0.0-preview.3)<br/><br/>Port to release/vNext"]
    waitInternalRelease["Wait for internal<br/>preview branch<br/><br/>Port to release/vNext<br/>after public release"]
    internalPreviewRelease["internal/release/preview.N<br/><br/>Port to release/vNext<br/>after public release"]

    start --> security
    security -- "No" --> nonSecPreviewCut
    security -- "Yes" --> secPreviewCut

    nonSecPreviewCut -- "No" --> releaseVNext
    nonSecPreviewCut -- "Yes" --> nonSecTargetNext
    nonSecTargetNext -- "Yes" --> previewBranchRelease
    nonSecTargetNext -- "No" --> releaseVNext

    secPreviewCut -- "No" --> waitInternalRelease
    secPreviewCut -- "Yes" --> secTargetNext
    secTargetNext -- "Yes" --> internalPreviewRelease
    secTargetNext -- "No" --> waitInternalRelease

    class security,nonSecPreviewCut,nonSecTargetNext,secPreviewCut,secTargetNext decision;
    class releaseVNext,previewBranchRelease,internalPreviewRelease action;
    class waitInternalRelease wait;
```

**Servicing:**

```mermaid
flowchart TD
    classDef decision fill:#fff4e5,stroke:#d35400,stroke-width:1px;
    classDef action fill:#e3f2fd,stroke:#1565c0,stroke-width:1px;
    classDef wait fill:#f5f5f5,stroke:#8d6e63,stroke-dasharray:5 3;

    start([Start])
    approved{Does the change have an<br/>approved servicing window?}
    releaseCut{Does the approved window<br/>have a release-specific branch?}
    generalWindow{Is the approved window<br/>represented by General Servicing?}
    futureSecurity{Is the future-window fix<br/>security-related?}
    parkingType{Runtime or shared<br/>component fix? 10.0+}
    security{Is the change<br/>security-related?}
    componentTypeNonSecNoCut{Runtime or shared<br/>component fix? 10.0+}
    componentTypeNonSecCut{Runtime or shared<br/>component fix? 10.0+}
    componentTypeSecNoCut{Runtime or shared<br/>component fix? 10.0+}

    generalServicing1xx["1xx General servicing<br/>branch only<br/>e.g. release/10.0.1xx<br/><br/>Port to main as<br/>necessary on disclosure day"]
    generalServicingAllBands["Each affected band's<br/>General servicing branch<br/>e.g. release/10.0.1xx<br/>AND release/10.0.2xx<br/><br/>Port to main as<br/>necessary on disclosure day"]
    internalSpecific1xx["Internal 1xx release-specific<br/>branch only<br/>e.g. internal/release/10.0.105<br/>Merges to general<br/>servicing on release day<br/><br/>Port to main as<br/>necessary on disclosure day"]
    internalSpecificAllBands["Each affected band's internal<br/>release-specific branch<br/>e.g. internal/release/10.0.105<br/>AND internal/release/10.0.200<br/>Merges to general<br/>servicing on release day<br/><br/>Port to main as<br/>necessary on disclosure day"]
    internalGeneral1xx["Internal 1xx band branch<br/>e.g. internal/release/10.0.1xx<br/>Merges to general<br/>servicing on release day<br/><br/>Port to main as<br/>necessary on disclosure day"]
    internalGeneralAllBands["Each affected band's<br/>internal band branch<br/>e.g. internal/release/10.0.1xx<br/>AND internal/release/10.0.2xx<br/>Merges to general<br/>servicing on release day<br/><br/>Port to main as<br/>necessary on disclosure day"]
    parking1xx["Internal 1xx parking branch<br/>for the approved year/window<br/>e.g. internal/parking/10.0.1xx/2027/1B"]
    parkingAllBands["Each affected band's internal<br/>parking branch for the approved year/window"]
    holdChange["Obtain Tactics approval"]
    waitPublic["Public General Servicing PR<br/>Wait unmerged until the approved window<br/>1xx for shared fixes; each affected band for SDK fixes"]

    start --> approved
    approved -- "No" --> holdChange
    approved -- "Yes" --> releaseCut
    releaseCut -- "Yes" --> componentTypeNonSecCut
    releaseCut -- "No" --> generalWindow
    generalWindow -- "Yes" --> security
    generalWindow -- "No" --> futureSecurity
    futureSecurity -- "Yes" --> parkingType
    futureSecurity -- "No" --> waitPublic
    parkingType -- "Yes" --> parking1xx
    parkingType -- "No" --> parkingAllBands

    security -- "No" --> componentTypeNonSecNoCut
    security -- "Yes" --> componentTypeSecNoCut

    componentTypeNonSecNoCut -- "Yes" --> generalServicing1xx
    componentTypeNonSecNoCut -- "No" --> generalServicingAllBands

    componentTypeNonSecCut -- "Yes" --> internalSpecific1xx
    componentTypeNonSecCut -- "No" --> internalSpecificAllBands

    componentTypeSecNoCut -- "Yes" --> internalGeneral1xx
    componentTypeSecNoCut -- "No" --> internalGeneralAllBands

    class approved,releaseCut,generalWindow,futureSecurity,parkingType,security,componentTypeNonSecNoCut,componentTypeNonSecCut,componentTypeSecNoCut decision;
    class generalServicing1xx,generalServicingAllBands,internalSpecific1xx,internalSpecificAllBands,internalGeneral1xx,internalGeneralAllBands,parking1xx,parkingAllBands action;
    class holdChange,waitPublic wait;
```
