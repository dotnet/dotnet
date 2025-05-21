# The Unified Build Almanac (TUBA) - Unified Build Controls

Note: This document generally focuses on MSBuild properties, as most of our build infrastructure is based in MSBuild. In general, many controls can conceptually be applied to other tooling and build infrastructure, though name adjustments may be necessary.

## Goals

- Unify Linux distro partner builds and Microsoft builds under a common switch infrastructure.
- Increase clarity around which switches may be used in which contexts.
- Cover core VMR and repo scenarios.

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
- **Context controls** – These controls identify what phase of the build is currently executing. These controls are *infrastructure* controls that are useful for differentiating the context in which build infrastructure happens to be executing. For instance, arcade targets might be used in the orchestration layer as well as the outer and inner build, and it will be necessary to differentiate these cases. In addition, a general context control differentiates Unified Build scenarios from non-UB scenarios. This is useful for repos that keep their official builds.
- **Resource controls** – Controls what set of resources are available to the build. This may influence what product is produced. The biggest one here would be source alone vs. any binary/external resources. 
- **Output controls** - Set of switches controlling what outputs are produced from the VMR build. Architecture, configuration, IBC data sources, if tests are built, etc.
- **Organizational controls** – Switches that identify who (organization) is executing a build. They could control both product and resource availability. For instance, RH may not want telemetry in the output product, but presumably this could also be used to activate an alternative signing infra.

In addition, some ‘derivative’ controls are added for common scenarios.

### Tool Applicability

The following controls apply to msbuild infrastructure. These controls may be introduced into other tooling (e.g. cmake, node, powershell, bash) if necessary. Control names should be adjusted based on the conventions of those ecosystems.

### Context Controls

The following context controls will be implemented. These controls should be used for **infrastructure purposes (exceptions may be made on a case-by-case basis).**

| **Name** | **Values** | **Default** | **Description** |
| -------- | -------- | -------- | -------- |
| DotNetBuildFromVMR | "true", "false", "" | "" | When "true", indicates that the infrastructure is executing within the orchestrator and repo build. "True" inside the VMR orchestrator and inside a VMR repo build. |
| DotNetBuild | "true", "false", "" | "" | When "true", indicates that the infrastructure is executing in product build mode. Not "true" for repo builds without the `--source-build` or `--product-build` switch. |

### Resource Controls

These controls may be used for **infrastructure or product purposes**.

| **Name** | **Values** | **Default** | **Description** |
| -------- | -------- | -------- | -------- |
| DotNetBuildWithOnlineSources | "true", "false", "" | "false" by default when `SourceOnly` switch is active. | When "true", do not remove non-local input sources. Infrastructure switch only. This switch is only exposed at the orchestrator level.</br>This replaces the existing `DotNetBuildOffline` switch. |
| DotNetBuildSourceOnly | "true", "false", "" | "" | When "true", build only from source. Online sources may remain unless `DotNetBuildOffline` is set to true. This is both an infrastructure and a product switch. |
| DotNetBuildTargetRidOnly | "true", "false", "" | "" | When not set, defaults to "true" if the repository build transitively depends on dotnet/runtime and `DotNetBuildFromVMR` == "true"; otherwise "false". When "true", builds projects for the current `TargetRid` instead of using the current runtime identifier. |

### Output Controls

These controls may be used for **infrastructure or product purposes**. It is expected that they will be mostly product.

In addition to these default high level controls, there may be additional component/repo-specific controls that can influence the product output.

| **Name** | **Values** | **Default** | **Description** |
| -------- | -------- | -------- | -------- |
| BuildOS | "linux", "osx", "freebsd", "netbsd", "illumos", "solaris", "haiku", "windows", ... | OS of the build environment | The operating system of the machine that is built on. Lower-case string. |
| TargetOS | Same as `BuildOS` | `BuildOS` | The operating system of the machine that will run the binary -> the end user’s machine. |
| HostOS | Same as `BuildOS` | `TargetOS` | The operating system of the machine that will run the produced tool (i.e. compiler) to generate the binary for the target operating system. |
| BuildRid | Valid RIDs | RID of the the currently executing runtime | The RID of the runtime that is running the build |
| TargetRid | Valid RIDs or custom RID | When building non-portable, the OS of build Rid + TargetArchitecture. When building portable, `TargetOS-TargetArchitecture`. | The RID of the runtime that will run the binary -> the end user’s machine. |
| HostRid | Valid RIDs or `TargetRid` | `TargetRid` | The RID of the runtime that will run the produced tool (i.e. compiler) to generate the binary for the target operating system. |
| BaseRid | Valid RIDs | OS portion of `NETCoreSdkPortableRuntimeIdentifier` appended with `-TargetArchitecture` | A known RID to use as a parent of a custom RID specified in `TargetRid` if `TargetRid` is unknown. |
| BuildArchitecture | "x64", "x86", "arm", "arm64", ... | The architecture of the build environment | The architecture of the machine that is built on. Lower-case string. |
| TargetArchitecture | Same as `BuildArchitecture` | `BuildArchitecture` | The architecture of the machine that will run the binary -> the end user's machine. |
| HostArchitecture | Same as `BuildArchitecture` | `TargetArchitecture` | The architecture of the machine that will run the produced tool (i.e. compiler) to generate the binary for the target architecture |
| Configuration | Debug, Release | Release | Defaults produces a shipping product. |
| DotNetBuildTests | "true", "false", "" | "" is the default. | When "true", the build should include test projects.<br/>Not "true" is essentially the default behavior for source build today. This is essentially equivalent to ExcludeFromBuild being set to true when `DotNetBuildTests` == false and Arcade’s `IsTestProject` or `IsTestUtilityProject`` is true. |
| ShortStack | "true", "false", "" | "" | If true, the build is a 'short stack' (runtime and its dependencies only). Other repo builds are skipped. |
| ExcludeFromDotNetBuild | "true", "false", "" | "" | When "true" and `DotNetBuildFromVMR` == "true", the project is not built.<br/>This is equivalent to `ExcludeFromBuild` being set to true when `DotNetBuildFromVMR` == "true".<br/>This control applies to project properties. |
| ExcludeFromSourceOnlyBuild | "true", "false", "" | "" | When "true" and `DotNetBuildSourceOnly` == "true" the project is not built.<br/>This is equivalent to `ExcludeFromBuild` being set to true when `DotNetBuildSourceONly` == "true". Same as `ExcludeFromSourceBuild` today.<br/>This control applies to project properties. |
| PortableBuild | "true", "false", "" | "" | When "false", the build is non-portable. |

### Organizational Controls

These controls may be used for **infrastructure or product purposes**.

| **Name** | **Values** | **Default** | **Description** |
| -------- | -------- | -------- | -------- |
| OfficialBuilder | "Microsoft", "<org name>", ""  | "" | May be used to differentiate product or infrastructure behavior between organizations. This is equivalent to the `OfficialBuilder` switch currently in place. See use in `dotnet/sdk` |
