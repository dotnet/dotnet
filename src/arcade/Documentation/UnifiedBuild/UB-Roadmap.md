# Unified Build Roadmap

```mermaid

gantt
    title Unified Build Roadmap
    axisFormat %b-%Y
    .NET8 RC2: milestone, net8-rc2, 2023-10-10, 0
    .NET8 GA: milestone, net8-ga, 2023-11-14, 0
    Holiday Break - VB PoCs done: milestone, holiday, 2023-12-25, 0
    %% delta between Holiday and P1 is 7w
    %% the release dates are all 14th so that the task lengths can be expressed in whole weeks and still match the preview points
    P1 - Confidence Point in PoC: milestone, net9-p1, 2024-02-14, 0
    P2: milestone, net9-p2, 2024-03-14, 0
    P3: milestone, net9-p3, 2024-04-14, 0
    P4: milestone, net9-p4, 2024-05-14, 0
    P5: milestone, net9-p5, 2024-06-14, 0
    P6: milestone, net9-p6, 2024-07-14, 0
    P7: milestone, net9-p7, 2024-08-14, 0
    RC1 - Productize PoC: milestone, net9-rc1, 2024-09-14, 0
    RC2 - VMR Test Release: milestone, net9-rc2, 2024-10-14, 0
    GA: milestone, net9-ga, 2024-11-14, 0

    section Common
        UB Week: ub-week, 2023-10-02, 1w

    section Vertical Build (SteveP / TomasK / MattM)
        Win VB PoC (ViktorH): after net8-ga, 6w
        Linux VB PoC (JoS, JacksonS): after net8-rc2, 11w
        MacOS VB PoC (JoS, JacksonS): after net8-rc2, 11w
        Workloads PoC (AlexK, AnkitJ): after net8-ga, 6w
        Vertical Builds Design: after holiday, 7w
        Enable Vertical Builds: after net9-p1, 30w
        Crossbuilds Design (JoS / SteveP): after net8-ga, 13w

    section Source-Build (MichaelS)
        Eliminate Src Edits During Build: after net8-ga, 6w
        Remove Inner Clone: after holiday, 3w
        Parallel Build Support: after net9-p1, 2w
        Eliminate Src Edits During Build: after net9-p2, 4w
        Incremental Build Support: after net9-p3, 6w
        Multi-band SDKs: after net9-p4, 4w
        Distro Partners Support (sparse): after holidays, 44w

    section Product Validation (RichaV)
        Scenario tests in VMR: after net8-rc2, 10w
        PR Validation: after net9-p2, 4w
        Product Validation Tooling (sparse): after net9-p1, 16w

    section Product Construction (TomasK / MattM / PremekV)
        Backflow design: after net8-rc2, 5w
        Backflow tooling: after net8-ga, 6w
        Dependency Flow Service: after holiday, 7w
        Maestro Integration: after net9-p1, 10w
        Multi-band SDKs: after net9-p4, 4w
        Dependency Flow Switch Preparation: after net9-p4, 2w
        Depenendency Flow Switch: after net9-rc2, 4w

    section Release Infra (TomasK)
        Release infra investigation & design: after net8-ga, 6w
        Signing Design: after holiday, 7w
        Identify Repo Dependencies: after net9-p1, 4w
        Staging / Release Pipeline: after net9-p1, 12w
```

(not displayed on the roadmap) Switch .NET 9 to UB at 9.0.2 or 9.0.3, based on risk calculation and .NET 10 results.

# Milestones

The Unified Build milestones are aligned with the .NET9 lifecycle, specifically with the preview releases.

**Holiday Break (end of Dec)**

* Vertical Build (VB) Proof of Concepts (PoCs) for each of the major platforms completed.
* The main join points are identified and most of the unforseen problems have surfaced as result of the PoC work.
* The VMR scenario tests focused on the overall product functionality are running on the VMR for every PR.

**.NET9 Preview 1**

* High confidence in the PoCs. At this point, we expect to have uncovered and understood all problematic aspects of the Unified build designs, including the Vertical Buidls, their Join points, cross-compiled builds, and the full flow (forward flow + backflow) between the individual produt repos and the VMR. Differences between the current source-build and Linux vertical build are identified.

**.NET9 Preview 3**

* PoC Productization. At this point, we expect to have the Vertical Builds work completed, including setting up minimal set of join points.
* The VMR tooling will support automated backflow and be fully integrated with the current dependency flow tooling (Maestro++).

**.NET9 Preview 4**

* Test release of VMR in parallel with existing build methodology.

**.NET9 Preview 5**

* Switch to release from VMR.
* Start turning off the existing build methodology (for .NET9).

**.NET9 Preview 7**

* The latest possible time to move to the VMR based dependency flow with confidence to not introduce any risk for .NET9 release.

# Product Areas Owners

List of product area owners from the Vertical Builds perspective.

* Runtime - Jeff Schwartz
* Libraries - Art Leonard
* ASP.NET - Dan Moseley
* Mono - Marek Safar
* SDK, Winforms, MSBuild - Donald Drake
* Roslyn - Jared Parsons
* WPF - TBD

# Individual Tasks

## Vertical Build area

**Windows, Linux, MacOS Vertical Build (VB) PoC**

Initial vertical build efforts for each of the main platforms (Windows, Linux, MacOS). The goal of these efforts is to uncover hidden problems with building the product for each platform in a single build without requiring cross-platform build assets.
        
**Workloads PoC**

SDK Workloads vertical build Proof of Concept. Similar to the platform VB PoCs, this effort is focused to identify and uncover all issues related to building the SDK workloads.

**Identify Join Points**

Identification and mapping of the current cross-platform build orchestration.

**Vertical Builds Design**

Leverage the finding of the vertical build PoC works to design vertical builds for each platform properly, with the minimal set of join points.

**Enable Vertical Builds**

Implementation of the vertical builds design with the new se of join points.

**Crossbuilds Design**

Design for cross-arch or cross-platform builds and determine how to define cross-build behavior.

## Source-Build area

* [Eliminate Src Edits During Build](https://github.com/dotnet/source-build/issues/3664)
* [Parallel Build Support](https://github.com/dotnet/source-build/issues/3072)
* [Remove Inner Clone](https://github.com/dotnet/source-build/issues/3666)
* [Incremental Build Support](https://github.com/dotnet/source-build/issues/3608)
* [Multi-band SDKs](https://github.com/dotnet/source-build/issues/3667)

## Product Validation area

**Scenario tests in VMR**

End-to-end scenario tests (such as `dotnet new console` or a running a Stage 2 Linux source-build of the SDK) of the .NET product.

**PR Validation**

Definition and implementation of the set of tests that would be executed as part of the VMR PR validation.

## Product Construction area

**Backflow design**

Design for the backflow from the VMR to the individual product repositories.

**Backflow tooling**

Implementation of the core functionality and CLI tooling for the VMR backflow in to the product repos.

**Dependency Flow Service**

Implementation of the new dependency flow service, that will be extending the current BAR design.

**Maestro Integration**

Integration of the new dependency flow service with Maestro++.

**Dependency Flow Switch Preparation**

Preparation for switching from the existing multi-leayered product dependency flow to the new flat dependency flow between VMR and product repos.

**Depenendency Flow Switch**

Switch to the new flat dependency flow between VMR and product repos.

## Release Infra area

**Release infra investigation & design**

Design for changes necessary to enable releases off of the VMR.

**Signing Design**

Design for signing releases based off of the VMR.

**Identify Repo Dependencies**

Identification of the dependencies between product repos and the layout used to stage the product assets for release.

**Staging / Release Pipeline**

Updates to the current release infrastructure, namely the staging and release pipelines to be able to base releases both off the current dependency flow for .NET8 and the VMR for .NET9.

