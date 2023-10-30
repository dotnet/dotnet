﻿# dotnet/dotnet - Home of the .NET VMR

This repository is a **Virtual Monolithic Repository (VMR)** which includes all the source code and infrastructure needed to build the .NET SDK.

What this means:
- **Monolithic** - a join of multiple repositories that make up the whole product, such as [dotnet/runtime](https://github.com/dotnet/runtime) or [dotnet/sdk](https://github.com/dotnet/sdk).
- **Virtual** - a mirror (not replacement) of product repos where sources from those repositories are synchronized into.

In the VMR, you can find:
- source files of [each product repository](#list-of-components) which are mirrored inside of their respective directories under [`src/`](https://github.com/dotnet/dotnet/tree/main/src),
- tooling that enables [building the whole .NET product from source](https://github.com/dotnet/source-build) on Linux platforms,
- small customizations, in the form of [patches](https://github.com/dotnet/dotnet/tree/main/src/installer/src/SourceBuild/patches), applied on top of the original code to make the build possible,
- *[in future]* E2E tests for the whole .NET product.

Just like the development repositories, the VMR will have a release branch for every feature band (e.g. `release/8.0.1xx-preview1`).
Similarly, VMR's `main` branch will follow `main` branches of product repositories (see [Synchronization Based on Declared Dependencies](src/arcade/Documentation/UnifiedBuild/VMR-Design-And-Operation.md#synchronization-based-on-declared-dependencies)).

More in-depth documentation about the VMR can be found in [VMR Design And Operation](src/arcade/Documentation/UnifiedBuild/VMR-Design-And-Operation.md#layout).
See also [dotnet/source-build](https://github.com/dotnet/source-build) for more information about our whole-product source-build.

## Goals

- The main purpose of the [dotnet/dotnet](https://github.com/dotnet/dotnet) repository is to have all source code necessary to build the .NET product available in one repository and identified by a single commit.
- The VMR also aims to become the place from which we release and service future versions of .NET to reduce the complexity of the product construction process. This should allow our partners and and 3rd parties to easily build, test and modify .NET using their custom infrastructure as well as make the process available to the community.
- Lastly, we hope to solve other problems that the current multi-repo setup brings:
    - Enable the standard [down-/up-stream open-source model](src/arcade/Documentation/UnifiedBuild/VMR-Upstream-Downstream.md).
    - Fulfill requirements of .NET distro builders such as RedHat or Canonical to natively include .NET in their distribution repositories.
    - Simplify scenarios such as client-run testing of bug fixes and improvements. The build should work in an offline environment too for certain platforms.
    - Enable developers to make and test changes spanning multiple repositories.
    - More efficient pipeline for security fixes during the CVE pre-disclosure process.

We will achieve these goals while keeping active coding work in the separate repos where it happens today. For example: ASP.NET features will continue to be developed in `dotnet/aspnetcore` and CLR features will be continue to be developed in `dotnet/runtime`. Each of these repos have their own distinct communities and processes, and aggregating development into a true mono-repo would work against that. Hence, the "virtual" monolithic repo: the VMR gives us the simplicity of a mono-repo for building and servicing the product, while active development of components of that product stays in its various existing repos. The day to day experience for typical contributors will not change.

## Limitations

**This is a work-in-progress.**
There are considerable limitations to what is possible at the moment. For an extensive list of current limitations, please see [Temporary Mechanics](src/arcade/Documentation/UnifiedBuild/VMR-Design-And-Operation.md#temporary-mechanics).  
See the [Unified Build roadmap](src/arcade/Documentation/UnifiedBuild/Roadmap.md) for more details.

### Supported platforms

The VMR only supports .NET 8.0 and higher. Additionally, source-build currently supports Linux only.  
It is expected that Mac and Windows will be supported in the .NET 9.0.

For the latest information about Source-Build support for new .NET versions, please check our [GitHub Discussions page](https://github.com/dotnet/source-build/discussions) for announcements.

### Online build only

Building the product offline is not fully working at the moment. The `--online` switch is needed when building the VMR as not all dependencies are currently built from source.

### Code flow
For the time being, the source code only flows one way - from the development repos into the VMR.
More details on this process:

- [Source Synchronization Process](src/arcade/Documentation/UnifiedBuild/VMR-Design-And-Operation.md#source-synchronization-process)
- [Synchronization Based on Declared Dependencies](src/arcade/Documentation/UnifiedBuild/VMR-Design-And-Operation.md#synchronization-based-on-declared-dependencies)
- [Moving Code and Dependencies between the VMR and Development Repos](src/arcade/Documentation/UnifiedBuild/VMR-Design-And-Operation.md#moving-code-and-dependencies-between-the-vmr-and-development-repos)

We expect the code flow to start working both ways in the .NET 9 timeframe.
See the [Unified Build roadmap](src/arcade/Documentation/UnifiedBuild/Roadmap.md) for more details.

### Contribution

At this time, the VMR will not accept any changes and is a read-only mirror of the development repositories only.
Please, make the changes in the respective development repositories (e.g., [dotnet/runtime](https://github.com/dotnet/runtime) or [dotnet/sdk](https://github.com/dotnet/sdk)) and they will get synchronized into the VMR automatically.

## Dev instructions

Please note that **this repository is a work-in-progress** and there are some usability issues connected to this.
These can be nuisances such as some checked-in files getting modified by the build itself and similar.
For the latest information about Source-Build support, please watch for announcements posted on our [GitHub Discussions page](https://github.com/dotnet/source-build/discussions).

### Prerequisites

The dependencies for building .NET from source can be found [here](https://github.com/dotnet/runtime/blob/main/docs/workflow/requirements/linux-requirements.md).
In case you don't want to / cannot prepare your environment per the requirements, consider [using Docker](#building-using-docker).

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
    The resulting SDK is placed at `artifacts/x64/Release/dotnet-sdk-9.0.100-your-RID.tar.gz`.

    Currently, the `--online` flag is required to allow NuGet restore from online sources during the build.
    This is useful for testing unsupported releases that don't yet build without downloading pre-built binaries from the internet.

    Run `./build.sh --help` to see more information about supported build options.

4. *(Optional)* **Unpack and install the .NET SDK**

    ```bash
    mkdir -p $HOME/dotnet
    tar zxf artifacts/[your-arch]/Release/dotnet-sdk-9.0.100-[your-RID].tar.gz -C $HOME/dotnet
    ln -s $HOME/dotnet/dotnet /usr/bin/dotnet
    ```
    
    To test your source-built SDK, run the following:

    ```bash
    dotnet --info
    ```

### Building using Docker

You can also build the repository using a Docker image which has the required prerequisites inside.
The example below creates a Docker volume named `vmr` and clones and builds the VMR there.

```sh
docker run --rm -it -v vmr:/vmr -w /vmr mcr.microsoft.com/dotnet-buildtools/prereqs:centos-stream8
git clone https://github.com/dotnet/dotnet .
./prep.sh && ./build.sh --online
mkdir -p $HOME/.dotnet
tar -zxf artifacts/x64/Release/dotnet-sdk-9.0.100-centos.8-x64.tar.gz -C $HOME/.dotnet
ln -s $HOME/.dotnet/dotnet /usr/bin/dotnet
```

### Codespaces

You can also utilize [GitHub Codespaces](https://github.com/features/codespaces) where you can find preset containers in this repository.

### Building from released sources

You can also build from sources (and not from a context of a git repository), such as the ones you can acquire from a [dotnet/dotnet release](https://github.com/dotnet/dotnet/releases).
In this case, you need to provide additional information which includes the original repository and commit hash the code was built from so that the SDK can provide a better debugging experience (think the `Step into..` functionality).
Usually, this means the [dotnet/dotnet repository](https://github.com/dotnet/dotnet) together with the commit the release tag is connected to.

In practice, this means that when calling the main build script, you need to provide additional arguments when building outside of a context of a git repository.  
Alternatively, you can also provide a manifest file where this information can be read from. This file (`release.json`) can be found attached with the [dotnet/dotnet release](https://github.com/dotnet/dotnet/releases).

### Synchronizing code into the VMR

Sometimes you want to make a change in a repository and test that change in the VMR. You could of course make the change in the VMR directly (locally, as the VMR is read-only for now) but in case it's already available in your repository, you can synchronize it into the VMR (again locally).

To do this, you can start a [dotnet/dotnet](https://github.com/dotnet/dotnet) Codespace. You will see instructions right when the Codespace starts. Alternatively, you can clone the repository locally and use the `[eng/vmr-sync.sh](../../eng/vmr-sync.sh)` script to do that. Please refer to the documentation in the script for more details.

## List of components

To enable full offline source-building of the VMR, we have no other choice than to synchronize all the necessary code into the VMR. This also includes any code referenced via git submodules. More details on why and how this is done can be found here:
- [Strategy for managing external source dependencies](src/arcade/Documentation/UnifiedBuild/VMR-Strategy-For-External-Source.md)
- [Source Synchronization Process](src/arcade/Documentation/UnifiedBuild/VMR-Design-And-Operation.md#source-synchronization-process)

### Detailed list

<!-- component list beginning -->
- `src/arcade`  
*[dotnet/arcade@36d11a8](https://github.com/dotnet/arcade/commit/36d11a863c7ffc41e3730c806b0e98d32d1795ae)*
- `src/aspnetcore`  
*[dotnet/aspnetcore@ffa0a02](https://github.com/dotnet/aspnetcore/commit/ffa0a028464e13d46aaec0c5ad8de0725a4d5aa5)*
    - `src/aspnetcore/src/submodules/googletest`  
    *[google/googletest@116b7e5](https://github.com/google/googletest/commit/116b7e55281c4200151524b093ecc03757a4ffda)*
    - `src/aspnetcore/src/submodules/MessagePack-CSharp`  
    *[aspnet/MessagePack-CSharp@ecc4e18](https://github.com/aspnet/MessagePack-CSharp/commit/ecc4e18ad7a0c7db51cd7e3d2997a291ed01444d)*
- `src/cecil`  
*[dotnet/cecil@45dd3a7](https://github.com/dotnet/cecil/commit/45dd3a73dd5b64b010c4251303b3664bb30df029)*
- `src/command-line-api`  
*[dotnet/command-line-api@02fe27c](https://github.com/dotnet/command-line-api/commit/02fe27cd6a9b001c8feb7938e6ef4b3799745759)*
- `src/deployment-tools`  
*[dotnet/deployment-tools@1d17426](https://github.com/dotnet/deployment-tools/commit/1d174267bf45dabbadb12602b1170329611fd219)*
- `src/diagnostics`  
*[dotnet/diagnostics@5ce78f6](https://github.com/dotnet/diagnostics/commit/5ce78f66d89ea529e459abddb129ab36cb5bd936)*
- `src/emsdk`  
*[dotnet/emsdk@cd13d57](https://github.com/dotnet/emsdk/commit/cd13d57eccb71580227d612efc2fa27c5512aef4)*
- `src/format`  
*[dotnet/format@f5f24f3](https://github.com/dotnet/format/commit/f5f24f38cd00b9aa341569834f3d44cd33031482)*
- `src/fsharp`  
*[dotnet/fsharp@c040d25](https://github.com/dotnet/fsharp/commit/c040d25693a071efb0df0deb24fca347802d0f22)*
- `src/installer`  
*[dotnet/installer@f3c3909](https://github.com/dotnet/installer/commit/f3c39098a298693206acf00c470530407c8b2ba8)*
- `src/msbuild`  
*[dotnet/msbuild@d3fa669](https://github.com/dotnet/msbuild/commit/d3fa6693db77b6aad33ac934ca0a87f852f4cdf2)*
- `src/nuget-client`  
*[nuget/nuget.client@ec3e254](https://github.com/nuget/nuget.client/commit/ec3e25481c88488630cabe436d3e53907a3b7b7b)*
    - `src/nuget-client/submodules/NuGet.Build.Localization`  
    *[NuGet/NuGet.Build.Localization@f15db7b](https://github.com/NuGet/NuGet.Build.Localization/commit/f15db7b7c6f5affbea268632ef8333d2687c8031)*
- `src/razor`  
*[dotnet/razor@aeb01df](https://github.com/dotnet/razor/commit/aeb01dfb4f3c9aaa9aa799f59369ba55f3272a85)*
- `src/roslyn`  
*[dotnet/roslyn@18c0519](https://github.com/dotnet/roslyn/commit/18c05195e6973330cf465404c65f90626fc0bbea)*
- `src/roslyn-analyzers`  
*[dotnet/roslyn-analyzers@b4ed6a3](https://github.com/dotnet/roslyn-analyzers/commit/b4ed6a3093cfd3c8d353214ce97aaa7d24cf2df1)*
- `src/runtime`  
*[dotnet/runtime@2e48789](https://github.com/dotnet/runtime/commit/2e4878953076f0b0ffc4d8e7bf27ff5de7ef30d3)*
- `src/scenario-tests`  
*[dotnet/scenario-tests@8af694a](https://github.com/dotnet/scenario-tests/commit/8af694a5e3986a27ccfee1a638ba311c7e9bc55d)*
- `src/sdk`  
*[dotnet/sdk@6c69f67](https://github.com/dotnet/sdk/commit/6c69f67ac825da15ce05607d6a87b63693cac2a2)*
- `src/source-build-externals`  
*[dotnet/source-build-externals@54ad220](https://github.com/dotnet/source-build-externals/commit/54ad220ef4f236325dec31f2c0c66fa48a6fce33)*
    - `src/source-build-externals/src/abstractions-xunit`  
    *[xunit/abstractions.xunit@b75d54d](https://github.com/xunit/abstractions.xunit/commit/b75d54d73b141709f805c2001b16f3dd4d71539d)*
    - `src/source-build-externals/src/application-insights`  
    *[Microsoft/ApplicationInsights-dotnet@5e2e7dd](https://github.com/Microsoft/ApplicationInsights-dotnet/commit/5e2e7ddda961ec0e16a75b1ae0a37f6a13c777f5)*
    - `src/source-build-externals/src/azure-activedirectory-identitymodel-extensions-for-dotnet`  
    *[AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet@bb354ce](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/commit/bb354ceabed19189245e075abb864f327b6c14ad)*
    - `src/source-build-externals/src/cssparser`  
    *[dotnet/cssparser@0d59611](https://github.com/dotnet/cssparser/commit/0d59611784841735a7778a67aa6e9d8d000c861f)*
    - `src/source-build-externals/src/docker-creds-provider`  
    *[mthalman/docker-creds-provider@5701f66](https://github.com/mthalman/docker-creds-provider/commit/5701f6667c1fbd805684857baaa860383bbdfed7)*
    - `src/source-build-externals/src/humanizer`  
    *[Humanizr/Humanizer@3ebc38d](https://github.com/Humanizr/Humanizer/commit/3ebc38de585fc641a04b0e78ed69468453b0f8a1)*
    - `src/source-build-externals/src/MSBuildLocator`  
    *[microsoft/MSBuildLocator@e0281df](https://github.com/microsoft/MSBuildLocator/commit/e0281df33274ac3c3e22acc9b07dcb4b31d57dc0)*
    - `src/source-build-externals/src/newtonsoft-json`  
    *[JamesNK/Newtonsoft.Json@0a2e291](https://github.com/JamesNK/Newtonsoft.Json/commit/0a2e291c0d9c0c7675d445703e51750363a549ef)*
    - `src/source-build-externals/src/xunit`  
    *[xunit/xunit@f110e5b](https://github.com/xunit/xunit/commit/f110e5bee5dfd4c08339587c9c3df9292fcb597c)*
    - `src/source-build-externals/src/xunit/src/xunit.assert/Asserts`  
    *[xunit/assert.xunit@5c8c10e](https://github.com/xunit/assert.xunit/commit/5c8c10e085eb42f39f2fe0b40c94bf56649eb0a4)*
    - `src/source-build-externals/src/xunit/tools/build`  
    *[xunit/build-tools@8e186b0](https://github.com/xunit/build-tools/commit/8e186b0f8e398796e75453f3f18952b06d29fdfd)*
    - `src/source-build-externals/src/xunit/tools/media`  
    *[xunit/media@5738b6e](https://github.com/xunit/media/commit/5738b6e86f08e0389c4392b939c20e3eca2d9822)*
- `src/source-build-reference-packages`  
*[dotnet/source-build-reference-packages@947ef94](https://github.com/dotnet/source-build-reference-packages/commit/947ef94c52440c781aeb6ee13e95a9ec9992e444)*
- `src/sourcelink`  
*[dotnet/sourcelink@e2f4720](https://github.com/dotnet/sourcelink/commit/e2f4720f9e7411122675568b984606c405b3bb53)*
- `src/symreader`  
*[dotnet/symreader@2c8079e](https://github.com/dotnet/symreader/commit/2c8079e2e8e78c0cd11ac75a32014756136ecdb9)*
- `src/templating`  
*[dotnet/templating@6144148](https://github.com/dotnet/templating/commit/61441481b54ae070d73b8056118aeb79691f59a1)*
- `src/test-templates`  
*[dotnet/test-templates@c2c7795](https://github.com/dotnet/test-templates/commit/c2c77959527a597caf3d0351ea0d25c085fbb32c)*
- `src/vstest`  
*[microsoft/vstest@f82bfa7](https://github.com/microsoft/vstest/commit/f82bfa7cad1d5a6beaf08a789e4534623a78f53f)*
- `src/xdt`  
*[dotnet/xdt@9a1c3e1](https://github.com/dotnet/xdt/commit/9a1c3e1b7f0c8763d4c96e593961a61a72679a7b)*
- `src/xliff-tasks`  
*[dotnet/xliff-tasks@83531ff](https://github.com/dotnet/xliff-tasks/commit/83531fff1a39abe38de53e2e6b702bc1bb9c3bb4)*
<!-- component list end -->

The repository also contains a [JSON manifest](https://github.com/dotnet/dotnet/blob/main/src/source-manifest.json) listing all components in a machine-readable format.

## Filing Issues

This repo does not accept issues as of now. Please file issues to the appropriate development repos.
For issues with the VMR itself, please use the [source-build repository](https://github.com/dotnet/source-build).

## Useful Links

- Design documentation for the VMR - a set of documents describing the high-level design and the why's and how's
  - [Design and Operation](src/arcade/Documentation/UnifiedBuild/VMR-Design-And-Operation.md)
  - [Upstream/Downstream Relationships](src/arcade/Documentation/UnifiedBuild/VMR-Upstream-Downstream.md)
  - [Code and Build Workflow](src/arcade/Documentation/UnifiedBuild/VMR-Code-And-Build-Workflow.md)
  - [Strategy for Managing External Source Dependencies](src/arcade/Documentation/UnifiedBuild/VMR-Strategy-For-External-Source.md)
- [.NET Source-Build](https://github.com/dotnet/source-build)
- [What is .NET](https://dotnet.microsoft.com)

## .NET Foundation

.NET Runtime is a [.NET Foundation](https://www.dotnetfoundation.org/projects) project.

## License

.NET is licensed under the [MIT](LICENSE.TXT) license.
