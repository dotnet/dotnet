# Developer Check-in Decision Guide

This guide helps you answer “What branch can I check into for my change?” using detailed condition tables for both **vNext/vNext Preview** and **Servicing/Patch** scenarios.

**Tip: When in doubt, ask.**

See also:
- [Servicing Workflow Details](VMR-Servicing-Workflows.md)

---

## Table 1: vNext and vNext Preview Changes

### Servicing branches have not been cut for vNext

| Condition(s)                                                                                                           | Where Can I Check In?                      | Do I need to port the change to avoid regression? |
|------------------------------------------------------------------------------------------------------------------------|--------------------------------------------|-----------------------------------|
| - Change is non-security **AND** <br/> - Preview branch has not been cut **OR** change is not targeted at next preview to ship | **main** in VMR or component repository | No |
| - Change is non-security **AND** <br/> - Preview branch has been cut **AND** <br/> - Change is targeted at next preview to ship | **Preview branch** (e.g. `release/9.0.0-preview.3`) in VMR or component repository. | Yes. Port to main |
| - Change is security **AND** <br/> - Preview branch has not been cut **OR** change is not targeted at next preview to ship | **Wait** for creation of internal preview branches (e.g. internal/release/11.0.0-rc.N branch) | **main**, **AFTER** public release. |
| - Change is security **AND** <br/> - Preview branch has been cut **AND** <br/> - Change is targeted at next preview to ship | **internal/release/preview.N** (e.g. `internal/release/11.0.0-rc.1`) in VMR or component repository. | **main**, **AFTER** public release. |

### Servicing branches have been cut for vNext

| Condition(s)                                                                                                           | Where Can I Check In?                      | Do I need to port the change to avoid regression? |
|------------------------------------------------------------------------------------------------------------------------|--------------------------------------------|-----------------------------------|
| - Change is non-security **AND** <br/> - Preview branch has not been cut **OR** change is not targeted at next preview to ship | **release/vNext** in VMR or component repository | No |
| - Change is non-security **AND** <br/> - Preview branch has been cut **AND** <br/> - Change is targeted at next preview to ship | **Preview branch** (e.g. `release/9.0.0-preview.3`) in VMR or component repository. | Yes. Port to release/vNext |
| - Change is security **AND** <br/> - Preview branch has not been cut **OR** change is not targeted at next preview to ship | **Wait** for creation of internal preview branches (e.g. internal/release/11.0.0-rc.N branch) | **release/vNext**, **AFTER** public release. |
| - Change is security **AND** <br/> - Preview branch has been cut **AND** <br/> - Change is targeted at next preview to ship | **internal/release/preview.N** (e.g. `internal/release/11.0.0-rc.1`) in VMR or component repository. | **release/vNext**, **AFTER** public release. |

---

## Table 2: Servicing (Patch/Hotfix) Changes

| Condition(s)                                                                                                                        | Where Can I Check In?                                 | Do I need to port the change to avoid regression? |
|-------------------------------------------------------------------------------------------------------------------------------------|------------------------------------------------------|-------------------------------|
| Change is non-security **AND** <br/> - Change is approved for next calendar release **AND** <br/> - Release specific branches have not been cut (e.g. release/10.0.105)  |  **General Servicing branch** in VMR or component repository (e.g. release/10.0) | Port to main as necessary on disclosure day |
| Change is not approved for release corresponding to any branch (usually approved for future month's release) | Hold change for branch opening  |  | N/A |
| Change is non-security **AND** <br/> - Change is approved for next calendar release **AND** <br/> - Release specific branches have been cut (e.g. release/10.0.105) | **Release specific branch in VMR** (e.g. release/10.0.105) | Will merge to General Servicing branch on release day. Port to main as necessary on disclosure day. |
| Change is security **AND** <br/> - Change is approved for next calendar release **AND** <br/> - Release specific branches have been cut (e.g. release/10.0.105) | **Internal specific branch in VMR** (e.g. internal/release/10.0.105) | Will merge to General Servicing branch on release day. Port to main as necessary on disclosure day.|
| Change is security **AND** <br/> - Change is approved for next calendar release **AND** <br/> - Release specific branches have not been cut (e.g. release/10.0.105) | Internal release branch (e.g. `internal/release/10.0.1xx`) | Will merge to General Servicing branch on release day. Port to main as necessary on disclosure day. |

---

## Quick Reference Decision Diagrams

**vNext/vNext Preview (servicing branches not cut):**

```mermaid
flowchart TD
    classDef decision fill:#fff4e5,stroke:#d35400,stroke-width:1px;
    classDef action fill:#e3f2fd,stroke:#1565c0,stroke-width:1px;
    classDef wait fill:#f5f5f5,stroke:#8d6e63,stroke-dasharray:5 3;

    start([Start])
    security{Is the change<br/>security-related?}
    nonSecPreviewCut{Has the preview branch<br/>been cut?}
    nonSecTargetNext{Is it targeted at the<br/>next preview to ship?}
    secPreviewCut{Has the preview branch<br/>been cut?}
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

**vNext/vNext Preview (servicing branches cut):**

```mermaid
flowchart TD
    classDef decision fill:#fff4e5,stroke:#d35400,stroke-width:1px;
    classDef action fill:#e3f2fd,stroke:#1565c0,stroke-width:1px;
    classDef wait fill:#f5f5f5,stroke:#8d6e63,stroke-dasharray:5 3;

    start([Start])
    security{Is the change<br/>security-related?}
    nonSecPreviewCut{Has the preview branch<br/>been cut?}
    nonSecTargetNext{Is it targeted at the<br/>next preview to ship?}
    secPreviewCut{Has the preview branch<br/>been cut?}
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
    approved{Is the change<br/>approved for the next<br/>calendar release?}
    security{Is the change<br/>security-related?}
    releaseCutNonSec{Have release-specific<br/>branches been cut?}
    releaseCutSec{Have release-specific<br/>branches been cut?}

    generalServicing["General servicing<br/>branch<br/>(e.g. release/10.0)<br/><br/>Port to main as<br/>necessary on disclosure day"]
    releaseSpecific["Release-specific<br/>branch<br/>(e.g. release/10.0.105)<br/>Merges to general<br/>servicing on release day<br/><br/>Port to main as<br/>necessary on disclosure day"]
    internalSpecific["Internal release<br/>branch<br/>(e.g. internal/release/10.0.105)<br/>Merges to general<br/>servicing on release day<br/><br/>Port to main as<br/>necessary on disclosure day"]
    internalGeneral["Internal servicing<br/>branch<br/>(e.g. internal/release/10.0.1xx)<br/>Merges to general<br/>servicing on release day<br/><br/>Port to main as<br/>necessary on disclosure day"]
    holdChange["Hold change until<br/>branch opens<br/><br/>(Re-evaluate later)"]

    start --> approved
    approved -- "No" --> holdChange
    approved -- "Yes" --> security

    security -- "No" --> releaseCutNonSec
    security -- "Yes" --> releaseCutSec

    releaseCutNonSec -- "No" --> generalServicing
    releaseCutNonSec -- "Yes" --> releaseSpecific

    releaseCutSec -- "No" --> internalGeneral
    releaseCutSec -- "Yes" --> internalSpecific

    class approved,security,releaseCutNonSec,releaseCutSec decision;
    class generalServicing,releaseSpecific,internalSpecific,internalGeneral action;
    class holdChange wait;
```
