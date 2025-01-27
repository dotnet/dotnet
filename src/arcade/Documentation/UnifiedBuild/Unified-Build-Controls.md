# The Unified Build Almanac (TUBA) - Unified Build Controls

This document serves a design for a revamp of the current set of source-build controls to support Unified Build.

Note: This document generally focuses on MSBuild properties, as most of our build infrastructure is based in MSBuild. In general, many controls can conceptually be applied to other tooling and build infrastructure, though name adjustments may be necessary.

## Background

Unified Build seeks to bring the way Microsoft builds .NET for its releases closer to the way that our Linux distro partners build. One of the ways that it will achieve this is to use much of the same infrastructure that source build uses, with some tweaks. We can think of Unified Build as:

*The expansion of source build with different rules about allowable inputs based on the requirements of the organization that is building .NET.*

Some organizations will allow pre-built binaries, some will not. These differences in requirements may generate some differences in the produced product and will also influence how the build needs to be run, what can be included, etc. The underlying principles and approach, however, will be the same.

Utilizing the source build infrastructure as it exists today is not desirable. That infrastructure has grown over many years and become tailored to one specific purpose. The switches that drive source build have been used in incorrect contexts and have been used as proxies for other things (e.g. using `DotNetBuildFromSource`` when "Building on Linux" was what was meant). Moreover, the existing meanings of the switches are so intertwined with "Linux source build" that use in other contexts would be confusing.

## Goals

- Unify Linux distro partner builds and Microsoft builds under a common switch infrastructure.
- Reduce baggage of existing Linux distro partner build control switches.
- Increase clarity around which switches may be used in which contexts.
- Provide an opportunity to re-evaluate current uses of control switches
- Cover core VMR and repo scenarios.
- Provide a straightforward way to migrate from the existing control structure to the new one.

## Current State (Jan 2024)

The following is an attempt to document how the current switches work, their current uses, misuses. A quick primer on the build phases is useful:
- **Orchestrator (Full Build only)** – The infrastructure responsible for setting up input sources, laying out previously source-built artifacts, building repos in sequence based on dependencies and managing intermediate feeds.
- **Outer Repo Build** – For arcade-ified repos, an initial invocation of a repo’s build is performed, in a special mode. It prepares the inner clone of the repo, monitors and reports prebuilts, creates intermediate packages, and invokes the inner build.
- **Inner Repo Build** – Build of the repo with additional switches passed to transfer to "source-build" mode. This is invoked by the outer build.   The inner build is a sandboxed build that enables prebuilt detection.

### Existing Control Set

- **ArcadeBuildFromSource**  (true/false/empty)
  - **Phases** – Outer Repo Build, Inner Repo Build
  - **Infrastructure or Product Use** – Infrastructure
  - **Values and their intended purposes**
    - **True** – The outer or inner repo build is executing. This can generally mean "executing source build via arcade infra". When set to true in the outer repo, this begins to active source build specific targets like the inner clone, prebuilt detection, intermediate publishing, etc.
    - **False/Empty** – The outer or inner repo build is not executing. A source build of a repo could still be running if DotNetBuildFromSource was passed, in a non-arcade repo.   
- **ArcadeInnerBuildFromSource** (true/false/empty)
  - **Phases** – Inner Repo Build
  - **Infrastructure or Product Use?** – Infrastructure
  - **Values and their intended purposes**
    - **True** – The inner repo build is executing. This switch is not intended for product repos to use to control anything but their build behavior. Typically, this is used to activate or deactivate behavior that should only be executed on an inner build and nowhere else. This switch is also only present when building repos that use arcade.
    - **False/Empty** – Not executing in the inner build, or not a repo that uses arcade based infra.
- **DotNetBuildFromSource** (true/false/empty)
  - **Phases** - Orchestrator, Inner Repo Build
  - **Infrastructure or Product Use?** - Both
  - **Values and their intended purposes.**
    - **True** – Indicates that that the build is being run in "Linux distro partner" mode. This may mean that certain feature sets should be disabled because they are not buildable from source alone. In some infrastructural cases, this may mean that certain build operations (e.g. Publish targets) should not be run in repo builds as well as the outer repo build. This switch does not require the Arcade* switches to activate. In theory, one can pass just the DotNetBuildFromSource switch on the command line to a repo build. This should ideally activate online Linux distro partner source build behavior but without the infrastructure around it doing prebuilt checking, inner clone, etc. It’s unlikely that this works in many cases, though.
    - **False/Empty** – The build is not being run in Linux distro partner mode. This generally indicates MSFT centric official build scenarios, dev scenarios etc.
  - **Notes** – This is the messiest switch. It is used correctly in many places, but incorrectly in others. It has a mix of infrastructural and product uses. I do not believe it was originally intended to be used for anything but the inner repo build. However, the orchestrator passes this switch to the outer repo builds, causing its own publish targets to be substituted for arcade’s standard targets. There are a variety of very confusing scenarios where certain build targets are executed both in the inner build and the outer repo build, this switch is used to conditionalize those targets. This means that when writing build logic, it’s necessary to know where the targets may be executed in the orchestrator or outer repo context.
- **BuildWithOnlineSources**
  - **Phases** - Orchestrator
  - **Infrastructure or Product Use?** – Infrastructure
  - Values and their intended purposes 
    - False/Empty – Removes online sources from the repo’s NuGet.config files.
- **DotNetBuildFromSourceFlavor**
  - **Phases** – Outer Repo Build, Inner Repo Build
  - **Infrastructure or Product Use?** – Product and Infrastructure
  - Values and their intended purposes
    - **Product** – Executing within the fully orchestrated product build. This switch activates and deactivates some infra behaviors (like prebuilt reporting on individual repos), changes TFMs in some cases, and enables repos to build in "repo" source build mode by looking for some resources in online locations (e.g. targeting packs) instead of locally on disk.
    - **Empty** – Executing in a repo source build context. This might change what TFMs are targeted or pull targeting packs from online locations.
- **ExcludeFromSourceBuild** 
  - **Phases** - Inner Repo Build
  - **Infrastructure or Product Use?** - Product
  - **Values and their intended purposes.**
    - **True** – If true and DotNetBuildFromSource is true, no targets should be executed for the project. Causes the standard Arcade empty targets to be imported. No restore, no build. Standard test projects set this implicitly.
    - **False/Empty** – Default behavior is used.
- **OfficialBuilder**
  - **Phases** – Inner Repo Build
  - **Infrastructure or Product Use?** - Product
  - **Values and their intended purposes**
    - **"Microsoft"** – Enables SDK telemetry.
    - **False/Empty** – No SDK telemetry

## Scenarios

What do we mean when we say scenarios? We mean given the source and a set of control switches, a user/automation system/whatever will get a desired outcome.

Let’s enumerate this in terms of the desired results:
- Microsoft builds a full .NET product for shipping to customers. This product is built using external services and prebuilt binaries.
- A non-MSFT organization builds a .NET product for shipping to their customers. This product is compatible with the equivalent Microsoft .NET product distribution (e.g. RedHat’s Preview 5 is compatible with Microsoft’s Preview 5). This build does not use any external resources.
- A developer or CI system builds a non-official build of the full .NET product for validation of Linux distro partner scenarios. This build does not include a build of repo test projects. This build reports any prebuilt binaries after completion of the full product build.
- A developer or CI system builds an isolated repo in a mode that closely mimics how the repo will build when the full product build is executed. Depending on inputs, this build may mimic Linux distro partner organizational requirements (no prebuilts) or standard Microsoft requirements (allow prebuilts).
- A developer or CI system builds the full .NET product, including test projects, for validation.

## Controls

Based on these scenarios, we can imagine groups of controls that will be required to satisfy them:
- **Context controls** – These controls identify what phase of the build is currently executing. These controls are *infrastructure* controls that are useful for differentiating the context in which build infrastructure happens to be executing. For instance, arcade targets might be used in the orchestration layer as well as the outer and inner build, and it will be necessary to differentiate these cases. In addition, a general context control differentiates Unified Build scenarios from non-UB scenarios. This is useful for repos that keep their official builds. These controls can be subdivided into three categories:
  - **General context controls** – Identifies whether we are in context of Unified Build in general.
  - **Exclusive context controls** – An exclusive context control is active for code executing in the current phase. This is noII inclusive. For example, a switch marking that code is executing in context of the orchestration phase would not be active within the inner repo build.
   - **Inclusive contexts controls** – An inclusive context control is active if executed within that phase or any subphases. For instance, the outer repo build inclusive phase switch is active within the inner repo build. These controls are useful for conditionalizing infrastructure that should be active within a subsection of the build. For instance, a code path might only execute when running under an orchestrated full product build.
- **Resource controls** – Controls what set of resources are available to the build. This may influence what product is produced. The biggest one here would be source alone vs. any binary/external resources. 
- **Output controls** - Set of switches controlling what outputs are produced from the VMR build. Architecture, configuration, IBC data sources, if tests are built, etc.
- **Organizational controls** – Switches that identify who (organization) is executing a build. They could control both product and resource availability. For instance, RH may not want telemetry in the output product, but presumably this could also be used to activate an alternative signing infra.

In addition, some ‘derivative’ controls are added for common scenarios.

### Tool Applicability

The following controls apply to msbuild infrastructure. These controls may be introduced into other tooling (e.g. cmake, node, powershell, bash) if necessary. Control names should be adjusted based on the conventions of those ecosystems.

### Context Controls

The following context controls will be implemented. These controls should be used for **infrastructure purposes (exceptions may be made on a case-by-case basis).**

#### General Context Controls

| **Name** | **Values** | **Default** | **Description** |
| -------- | -------- | -------- | -------- |
| DotNetBuild | "true", "false", "" | "" | This is a general identification control that essentially identifies whether the infrastructure is building in any kind of Unified Build mode. This serves as a way to conditionalize non-phase specific infrastructure in a general manner.<br/>Generally, this is `DotNetBuildPhase != ‘’`<br/>In general, uses of this switch should be limited to infrastructure, though it is possible that those infrastructure uses may affect the build output, especially in cases where a repo maintains a separate official build. |

#### Exclusive Context Controls

| **Name** | **Values** | **Default** | **Description** |
| -------- | -------- | -------- | -------- |
| DotNetBuildPhase | "Orchestrator", "Repo", "InnerRepo", "" | "" | Exclusive phase control identifying the phase currently executing.<br/>Generally, this replaces uses of `ArcadeInnerBuildFromSource` (exclusive inner build) as well as common conditionals like `ArcadeBuildFromSource && !ArcadeInnerBuildFromSource`. |

 #### Inclusive Context Controls

| **Name** | **Values** | **Default** | **Description** |
| -------- | -------- | -------- | -------- |
| DotNetBuildInnerRepo | "true", "false", "" | "" | When "true", indicates that the infrastructure is executing within the inner repo build. This is equivalent to `ArcadeInnerBuildFromSource``. |
| DotNetBuildOrchestrator | "true", "false", "" | "" | When "true", indicates that the infrastructure is executing within the orchestrator, outer repo build, and inner repo build.<br/>This is roughly equivalent to `DotNetBuildFromSourceFlavor` as `Product`` in the current control set. |
| DotNetBuildRepo | "true", "false", "" | "" | When "true", indicates that the infrastructure is executing within outer repo build or inner repo build phases.<br/>This is essentially the same as the legacy `ArcadeBuildFromSource`. |

### Resource Controls

These controls may be used for **infrastructure or product purposes**.

| **Name** | **Values** | **Default** | **Description** |
| -------- | -------- | -------- | -------- |
| DotNetBuildWithOnlineSources | "true", "false", "" | "false" by default when `SourceOnly` switch is active. | When "true", do not remove non-local input sources. Infrastructure switch only. This switch is only exposed at the orchestrator level.</br>This replaces the existing `DotNetBuildOffline` switch. |
| DotNetBuildSourceOnly | "true", "false", "" | "" | When "true", build only from source. Online sources may remain unless `DotNetBuildOffline` is set to true. This is both an infrastructure and a product switch.<br/>This is roughly equivalent to `DotNetBuildFromSource` in the current infrastructure, though other controls may be better suited. |
| DotNetBuildTargetRidOnly | "true", "false", "" | "" | When not set, defaults to "true" if the repository build transitively depends on dotnet/runtime and `DotNetBuildOrchestrator` == "true"; otherwise "false". When "true", builds projects for the current `TargetRid` instead of using the current runtime identifier. |

### Output Controls

These controls may be used for **infrastructure or product purposes**. It is expected that they will be mostly product.

In addition to these default high level controls, there may be additional component/repo-specific controls that can influence the product output.

| **Name** | **Values** | **Default** | **Description** |
| -------- | -------- | -------- | -------- |
| BuildOS | "linux", "osx", "freebsd", "netbsd", "illumos", "solaris", "haiku", "windows", ... | OS of the build environment | The operating system of the machine that is built on. Lower-case string. |
| TargetOS | Same as `BuildOS` | `BuildOS` | The operating system of the machine that will run the binary -> the end user’s machine. |
| HostOS | Same as `BuildOS` | `TargetOS` | The operating system of the machine that will run the produced tool (i.e. compiler) to generate the binary for the target operating system. |
| BuildRid | Valid RIDs | RID of the the currently executing runtime | The RID of the runtime that is running the build |
| TargetRid | Valid RIDs | When building non-portable, the OS of build Rid + TargetArchitecture. When building portable, `TargetOS-TargetArchitecture`. | The RID of the runtime that will run the binary -> the end user’s machine. |
| HostRid | Valid RIDs | `TargetRid` | The RID of the runtime that will run the produced tool (i.e. compiler) to generate the binary for the target operating system. |
| BuildArchitecture | "x64", "x86", "arm", "arm64", ... | The architecture of the build environment | The architecture of the machine that is built on. Lower-case string. |
| TargetArchitecture | Same as `BuildArchitecture` | `BuildArchitecture` | The architecture of the machine that will run the binary -> the end user's machine. |
| HostArchitecture | Same as `BuildArchitecture` | `TargetArchitecture` | The architecture of the machine that will run the produced tool (i.e. compiler) to generate the binary for the target architecture |
| Configuration | Debug, Release | Release | Defaults produces a shipping product. |
| DotNetBuildTests | "true", "false", "" | "" is the default. | When "true", the build should include test projects.<br/>Not "true" is essentially the default behavior for source build today. This is essentially equivalent to ExcludeFromBuild being set to true when `DotNetBuildTests` == false and Arcade’s `IsTestProject` or `IsTestUtilityProject`` is true. |
| ShortStack | "true", "false", "" | "" | If true, the build is a 'short stack' (runtime and its dependencies only). Other repo builds are skipped. |
| ExcludeFromDotNetBuild | "true", "false", "" | "" | When "true" and `DotNetBuild` == "true", the project is not built.<br/>This is equivalent to `ExcludeFromBuild` being set to true when `DotNetBuild` == "true".<br/>This control applies to project properties. |
| ExcludeFromSourceOnlyBuild | "true", "false", "" | "" | When "true" and `DotNetBuild` == "true" and `DotNetBuildSourceOnly` == "true" the project is not built.<br/>This is equivalent to `ExcludeFromBuild` being set to true when `DotNetBuild` == "true". Same as `ExcludeFromSourceBuild` today.<br/>This control applies to project properties. |
| PortableBuild | "true", "false", "" | "" | When "false", the build is non-portable. |

### Organizational Controls

These controls may be used for **infrastructure or product purposes**.

| **Name** | **Values** | **Default** | **Description** |
| -------- | -------- | -------- | -------- |
| OfficialBuilder | "Microsoft", "<org name>", ""  | "" | May be used to differentiate product or infrastructure behavior between organizations. This is equivalent to the `OfficialBuilder` switch currently in place. See use in `dotnet/sdk` |

## Rollout Plan

The rollout of this plan should happen in three stages: Add new controls, transition existing usages to new controls, then remove old controls. The goal is to have a seamless transition that does not break repo or VMR builds at any point. Because changes cannot yet be made solely in the VMR, these changes must flow from the individual repos.

### Add new controls

In this stage, the new control sets are implemented, and the switches are made available within the infrastructure. No old switches are removed.

### Re-evaluation and transition to new controls

Once new controls are available, we walk through the existing control set and replace usages one by one. Some transitions will be relatively straightforward (e.g. `DotNetBuildFromSourceFlavor` -> `DotNetBuildOrchestrated`), while others may be more involved. Each control usage location should be evaluated to determine the correct control to switch to (if any). The biggest shift here will be a re-evaluation of the existing `DotNetBuildFromSource` switch. There are many cases where this should transition to `DotNetBuildFromSourceOnly`, as well as plenty of cases where this should transition to `DotNetBuild`. Similar cases exist for `ExcludeFromSourceBuild`.
In general, usages should be removed first from the inner repo builds, then the outer, then the orchestrated build.

### Removal of old controls

Once all usages of existing controls are transitioned, the old controls are removed. Again, we start with the inner repo build, then the outer, then the orchestrator. Note, since some repos may not be able to get on newer version of arcade immediately, removal will happen late in .NET 9.
