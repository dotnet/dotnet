﻿# dotnet/dotnet - Home of the .NET VMR

This repository is a **Virtual Monolithic Repository (VMR)** which includes all the source code and infrastructure needed to build the .NET SDK.

What this means:
- **Monolithic** - a join of multiple repositories that make up the whole product, such as [dotnet/runtime](https://github.com/dotnet/runtime) or [dotnet/sdk](https://github.com/dotnet/sdk).
- **Virtual** - a mirror (not replacement) of product repos where sources from those repositories are synchronized into.
- **Experimental** - not to be depended on as we reserve the right to delete the current instance and create a new, different one in its stead. See [Limitations](#limitations).

In the VMR, you can find:
- source files of [each product repository](#list-of-components) which are mirrored inside of their respective directories under [`src/`](https://github.com/dotnet/dotnet/tree/main/src),
- tooling that enables [building the whole .NET product from source](https://github.com/dotnet/source-build) on Linux platforms,
- small customizations, in the form of [patches](https://github.com/dotnet/dotnet/tree/main/src/installer/src/SourceBuild/tarball/patches), applied on top of the original code to make the build possible,
- *[in future]* E2E tests for the whole .NET product.

Just like the development repositories, the VMR will have a release branch for every feature band (e.g. `release/8.0.1xx-preview1`).
Similarly, VMR's `main` branch will follow `main` branches of product repositories (see [Synchronization Based on Declared Dependencies](https://github.com/dotnet/arcade/blob/main/Documentation/UnifiedBuild/VMR-Design-And-Operation.md#synchronization-based-on-declared-dependencies)).

More in-depth documentation about the VMR can be found in [VMR Design And Operation](https://github.com/dotnet/arcade/blob/main/Documentation/UnifiedBuild/VMR-Design-And-Operation.md#layout).
See also [dotnet/source-build](https://github.com/dotnet/source-build) for more information about our whole-product source-build.

## Goals

- The main purpose of the [dotnet/dotnet](https://github.com/dotnet/dotnet) repository is to have all source code necessary to build the .NET product available in one repository and identified by a single commit.
- The VMR also aims to become the place from which we release and service future versions of .NET to reduce the complexity of the product construction process. This should allow our partners and and 3rd parties to easily build, test and modify .NET using their custom infrastructure as well as make the process available to the community.
- Lastly, we hope to solve other problems that the current multi-repo setup brings:
    - Enable the standard [down-/up-stream open-source model](https://github.com/dotnet/arcade/blob/main/Documentation/UnifiedBuild/VMR-Upstream-Downstream.md).
    - Fulfill requirements of .NET distro builders such as RedHat or Canonical to natively include .NET in their distribution repositories.
    - Simplify scenarios such as client-run testing of bug fixes and improvements. The build should work in an offline environment too for certain platforms.
    - Enable developers to make and test changes spanning multiple repositories.
    - More efficient pipeline for security fixes during the CVE pre-disclosure process.

## Limitations

**This is a work-in-progress and an experiment.**
There are considerable limitations to what is possible at the moment. For an extensive list of current limitations, please see [Temporary Mechanics](https://github.com/dotnet/arcade/blob/main/Documentation/UnifiedBuild/VMR-Design-And-Operation.md#temporary-mechanics).

The VMR is expected to become non-experimental by .NET 8 Preview 1 (Februrary, 2023).
This means it won't be short-lived anymore and we won't be reserving the right to delete and re-create it anymore.
Other limitations might apply until the .NET 9 timeframe.
See the [Unified Build roadmap](https://github.com/dotnet/arcade/blob/main/Documentation/UnifiedBuild/Roadmap.md) for more details.

### Supported platforms

The VMR only supports .NET 8.0 and higher. Additionally, source-build currently supports Linux only.  
It is expected that Mac and Window will be supported in the .NET 9.0.

For the latest information about Source-Build support for new .NET versions, please check our [GitHub Discussions page](https://github.com/dotnet/source-build/discussions) for announcements.

### Online build only

Building the product offline is not fully working at the moment. The `--online` switch is needed when building the VMR as not all dependencies are currently built from source.

### Code flow
For the time being, the source code only flows one way - from the development repos into the VMR.
More details on this process:

- [Source Synchronization Process](https://github.com/dotnet/arcade/blob/main/Documentation/UnifiedBuild/VMR-Design-And-Operation.md#source-synchronization-process)
- [Synchronization Based on Declared Dependencies](https://github.com/dotnet/arcade/blob/main/Documentation/UnifiedBuild/VMR-Design-And-Operation.md#synchronization-based-on-declared-dependencies)
- [Moving Code and Dependencies between the VMR and Development Repos](https://github.com/dotnet/arcade/blob/main/Documentation/UnifiedBuild/VMR-Design-And-Operation.md#moving-code-and-dependencies-between-the-vmr-and-development-repos)

We expect the code flow to start working both ways in the .NET 9 timeframe.
See the [Unified Build roadmap](https://github.com/dotnet/arcade/blob/main/Documentation/UnifiedBuild/Roadmap.md) for more details.

### Contribution

At this time, the VMR will not accept any changes and is a read-only mirror of the development repositories only.
Please, make the changes in the respective development repositories (e.g., [dotnet/runtime](https://github.com/dotnet/runtime) or [dotnet/sdk](https://github.com/dotnet/sdk)) and they will get synchronized into the VMR automatically.

## Dev instructions

Please note that **this repository is an experiment and a work-in-progress so it is possible that the build is broken**.
For the latest information about Source-Build support, please watch for announcements posted on our [GitHub Discussions page](https://github.com/dotnet/source-build/discussions).

### Prerequisites

The dependencies for building .NET from source can be found [here](https://github.com/dotnet/runtime/blob/main/docs/workflow/requirements/linux-requirements.md).

### Building

1. **Clone the VMR**

   ```bash
   git clone https://github.com/dotnet/dotnet dotnet-dotnet
   ```

2. **Prep the source to build on your distro**  
   This downloads a .NET SDK and a number of .NET packages needed to build .NET from source.

    ```bash
    cd dotnet-dotnet
    ./prep.sh
    ```

3. **Build the .NET SDK**

    ```bash
    ./build.sh --clean-while-building --online
    ```

    This builds the entire .NET SDK from source.
    The resulting SDK is placed at `artifacts/x64/Release/dotnet-sdk-8.0.100-your-RID.tar.gz`.

    Currently, the `--online` flag is required to allow NuGet restore from online sources during the build.
    This is useful for testing unsupported releases that don't yet build without downloading pre-built binaries from the internet.

    Run `./build.sh --help` to see more information about supported build options.

4. *(Optional)* **Unpack and install the .NET SDK**

    ```bash
    mkdir -p $HOME/dotnet
    tar zxf artifacts/x64/Release/dotnet-sdk-8.0.100-your-RID.tar.gz -C $HOME/dotnet
    ln -s $HOME/dotnet/dotnet /usr/bin/dotnet
    ```
    
    To test your source-built SDK, run the following:

    ```bash
    dotnet --info
    ```

## List of components

To enable full offline source-building of the VMR, we have no other choice than to synchronize all the necessary code into the VMR. This also includes any code referenced via git submodules. More details on why and how this is done can be found here:
- [Strategy for managing external source dependencies](https://github.com/dotnet/arcade/blob/main/Documentation/UnifiedBuild/VMR-Strategy-For-External-Source.md)
- [Source Synchronization Process](https://github.com/dotnet/arcade/blob/main/Documentation/UnifiedBuild/VMR-Design-And-Operation.md#source-synchronization-process)

### Detailed list

<!-- component list beginning -->
- `src/arcade`  
*[dotnet/arcade@80b6be4](https://github.com/dotnet/arcade/commit/80b6be47e1425ea90c5febffac119250043a0c92)*
- `src/aspnetcore`  
*[dotnet/aspnetcore@1bee0af](https://github.com/dotnet/aspnetcore/commit/1bee0afeedab9d6d9d1cf23e65daa7ea5fcc6d47)*
    - `src/aspnetcore/src/submodules/googletest`  
    *[google/googletest@a4f02ef](https://github.com/google/googletest/commit/a4f02ef38981350c9d673b9909559c7a86420d7a)*
    - `src/aspnetcore/src/submodules/MessagePack-CSharp`  
    *[aspnet/MessagePack-CSharp.git@fe9fa08](https://github.com/aspnet/MessagePack-CSharp.git/commit/fe9fa0834d18492eb229ff2923024af2c87553f8)*
    - `src/aspnetcore/src/submodules/spa-templates`  
    *[dotnet/spa-templates.git@57bf5a0](https://github.com/dotnet/spa-templates.git/commit/57bf5a0a1bf5d55eb9efefa92dcff6f067d4bfb3)*
- `src/command-line-api`  
*[dotnet/command-line-api@c776cd4](https://github.com/dotnet/command-line-api/commit/c776cd4e906b669b9cce1017fee7d0ba9845d163)*
- `src/deployment-tools`  
*[dotnet/deployment-tools@c3ad00a](https://github.com/dotnet/deployment-tools/commit/c3ad00ae84489071080a606f6a8e43c9a91a5cc2)*
- `src/diagnostics`  
*[dotnet/diagnostics@e3e1490](https://github.com/dotnet/diagnostics/commit/e3e1490a23f27a6e0ed30d323035660c3ffc52cd)*
- `src/format`  
*[dotnet/format@bee17d2](https://github.com/dotnet/format/commit/bee17d238280246c6a8e6decbd86a91fda6d00c6)*
- `src/fsharp`  
*[dotnet/fsharp@a6a6470](https://github.com/dotnet/fsharp/commit/a6a64706c78963cb033fd230f36518d3e773b725)*
- `src/installer`  
*[dotnet/installer@2066311](https://github.com/dotnet/installer/commit/20663112b5c5c18e4fcf22c7b1421d9cd5ccb480)*
- `src/linker`  
*[dotnet/linker@dc5e60f](https://github.com/dotnet/linker/commit/dc5e60f5f2becf0b462d37ad918443126e80b49b)*
    - `src/linker/external/cecil`  
    *[mono/cecil.git@1840b74](https://github.com/mono/cecil.git/commit/1840b7410d37a613e684b6f9650e39e2d4950bbb)*
- `src/msbuild`  
*[dotnet/msbuild@3bcada9](https://github.com/dotnet/msbuild/commit/3bcada934edfa09e5f68222411e3efeda407985c)*
- `src/nuget-client`  
*[NuGet/NuGet.Client@6038f76](https://github.com/NuGet/NuGet.Client/commit/6038f769411b98f4efdb10a0924986eae2583618)*
    - `src/nuget-client/submodules/Common`  
    *[aspnet/Common.git@e6fac80](https://github.com/aspnet/Common.git/commit/e6fac8061686c18531e2621ccef97dd5e0687a65)*
    - `src/nuget-client/submodules/FileSystem`  
    *[NuGet/FileSystem.git@f1f3f08](https://github.com/NuGet/FileSystem.git/commit/f1f3f0820a573b96b2faaf5b7e6be9a036e4c7aa)*
    - `src/nuget-client/submodules/NuGet.Build.Localization`  
    *[NuGet/NuGet.Build.Localization.git@f15db7b](https://github.com/NuGet/NuGet.Build.Localization.git/commit/f15db7b7c6f5affbea268632ef8333d2687c8031)*
- `src/razor-compiler`  
*[dotnet/razor-compiler@a415146](https://github.com/dotnet/razor-compiler/commit/a41514681a4db83c7cae7e17debf668d12efc1bb)*
- `src/roslyn`  
*[dotnet/roslyn@c9a6d5c](https://github.com/dotnet/roslyn/commit/c9a6d5cf04904ebd2b1aaab0adb33df16c8e76a6)*
- `src/roslyn-analyzers`  
*[dotnet/roslyn-analyzers@83c80bf](https://github.com/dotnet/roslyn-analyzers/commit/83c80bfbd2d283405d94af5e4bb496bf7d185b01)*
- `src/runtime`  
*[dotnet/runtime@af841c8](https://github.com/dotnet/runtime/commit/af841c8b33cecc92d74222298f1e45bf7bf3d90a)*
- `src/sdk`  
*[dotnet/sdk@4bd9b67](https://github.com/dotnet/sdk/commit/4bd9b67d81fbf6d65012ef08c9d8cf2a27953057)*
- `src/source-build-externals`  
*[dotnet/source-build-externals@4d37f75](https://github.com/dotnet/source-build-externals/commit/4d37f75b11b8613594fcb38bd9b965a75877c49f)*
    - `src/source-build-externals/src/application-insights`  
    *[Microsoft/ApplicationInsights-dotnet@51c3ed8](https://github.com/Microsoft/ApplicationInsights-dotnet/commit/51c3ed8aa3f32209edf01168f9136a3ac8486c5d)*
    - `src/source-build-externals/src/azure-activedirectory-identitymodel-extensions-for-dotnet`  
    *[AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet.git@a9de8ff](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet.git/commit/a9de8ff14648770a3caa33a68d8061d0fa84d105)*
    - `src/source-build-externals/src/cssparser`  
    *[dotnet/cssparser@d6d86bc](https://github.com/dotnet/cssparser/commit/d6d86bcd8c162b1ae22ef00955ff748d028dd0ee)*
    - `src/source-build-externals/src/humanizer`  
    *[Humanizr/Humanizer@3ebc38d](https://github.com/Humanizr/Humanizer/commit/3ebc38de585fc641a04b0e78ed69468453b0f8a1)*
    - `src/source-build-externals/src/MSBuildLocator`  
    *[microsoft/MSBuildLocator@47281c3](https://github.com/microsoft/MSBuildLocator/commit/47281c3de1c87a43ab946725d011b9dca4b6434a)*
    - `src/source-build-externals/src/newtonsoft-json`  
    *[JamesNK/Newtonsoft.Json.git@ae9fe44](https://github.com/JamesNK/Newtonsoft.Json.git/commit/ae9fe44e1323e91bcbd185ca1a14099fba7c021f)*
- `src/source-build-reference-packages`  
*[dotnet/source-build-reference-packages@4957077](https://github.com/dotnet/source-build-reference-packages/commit/4957077bf92bf720b0619fdf706be132e3b42a07)*
- `src/sourcelink`  
*[dotnet/sourcelink@d047202](https://github.com/dotnet/sourcelink/commit/d047202874ad79d72c75b6354c0f8a9a12d1b054)*
- `src/symreader`  
*[dotnet/symreader@7b9791d](https://github.com/dotnet/symreader/commit/7b9791daa3a3477eb22ec805946c9fff8b42d8ca)*
- `src/templating`  
*[dotnet/templating@fedc320](https://github.com/dotnet/templating/commit/fedc32080e326a6ede705a7c1c64f8c3178cb84c)*
- `src/test-templates`  
*[dotnet/test-templates@bb36956](https://github.com/dotnet/test-templates/commit/bb3695688177f5f80eeb3c0498168612e31549d5)*
- `src/vstest`  
*[microsoft/vstest@02296a2](https://github.com/microsoft/vstest/commit/02296a2873652bc5911ae4b58d74f9faae03917d)*
- `src/xdt`  
*[dotnet/xdt@9a1c3e1](https://github.com/dotnet/xdt/commit/9a1c3e1b7f0c8763d4c96e593961a61a72679a7b)*
- `src/xliff-tasks`  
*[dotnet/xliff-tasks@366ad9b](https://github.com/dotnet/xliff-tasks/commit/366ad9b9f7af7d0eddbd36d1e13d8fcff0ac99db)*
<!-- component list end -->

The repository also contains a [JSON manifest](https://github.com/dotnet/dotnet/blob/main/src/source-manifest.json) listing all components in a machine-readable format.

## Filing Issues

This repo does not accept issues as of now. Please file issues to the appropriate development repos.
For issues with the VMR itself, please use the [source-build repository](https://github.com/dotnet/source-build).

## Useful Links

- Design documentation for the VMR - a set of documents describing the high-level design and the why's and how's
  - [Design and Operation](https://github.com/dotnet/arcade/blob/main/Documentation/UnifiedBuild/VMR-Design-And-Operation.md)
  - [Upstream/Downstream Relationships](https://github.com/dotnet/arcade/blob/main/Documentation/UnifiedBuild/VMR-Upstream-Downstream.md)
  - [Code and Build Workflow](https://github.com/dotnet/arcade/blob/main/Documentation/UnifiedBuild/VMR-Code-And-Build-Workflow.md)
  - [Strategy for Managing External Source Dependencies](https://github.com/dotnet/arcade/blob/main/Documentation/UnifiedBuild/VMR-Strategy-For-External-Source.md)
- [.NET Source-Build](https://github.com/dotnet/source-build)
- [What is .NET](https://dotnet.microsoft.com)

## .NET Foundation

.NET Runtime is a [.NET Foundation](https://www.dotnetfoundation.org/projects) project.

## License

.NET is licensed under the [MIT](LICENSE.TXT) license.
