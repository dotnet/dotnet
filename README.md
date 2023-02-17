# dotnet/dotnet - Home of the .NET VMR

This repository is a **Virtual Monolithic Repository (VMR)** which includes all the source code and infrastructure needed to build the .NET SDK.

What this means:
- **Monolithic** - a join of multiple repositories that make up the whole product, such as [dotnet/runtime](https://github.com/dotnet/runtime) or [dotnet/sdk](https://github.com/dotnet/sdk).
- **Virtual** - a mirror (not replacement) of product repos where sources from those repositories are synchronized into.
- **Experimental** - not to be depended on as we reserve the right to delete the current instance and create a new, different one in its stead. See [Limitations](#limitations).

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

## Limitations

**This is a work-in-progress and an experiment.**
There are considerable limitations to what is possible at the moment. For an extensive list of current limitations, please see [Temporary Mechanics](src/arcade/Documentation/UnifiedBuild/VMR-Design-And-Operation.md#temporary-mechanics).

The VMR is expected to become non-experimental by .NET 8 Preview 1 (Februrary, 2023).
This means it won't be short-lived anymore and we won't be reserving the right to delete and re-create it anymore.
Other limitations might apply until the .NET 9 timeframe.
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

Please note that **this repository is an experiment and a work-in-progress so it is possible that the build is broken**.
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
    The resulting SDK is placed at `artifacts/x64/Release/dotnet-sdk-8.0.100-your-RID.tar.gz`.

    Currently, the `--online` flag is required to allow NuGet restore from online sources during the build.
    This is useful for testing unsupported releases that don't yet build without downloading pre-built binaries from the internet.

    Run `./build.sh --help` to see more information about supported build options.

4. *(Optional)* **Unpack and install the .NET SDK**

    ```bash
    mkdir -p $HOME/dotnet
    tar zxf artifacts/[your-arch]/Release/dotnet-sdk-8.0.100-[your-RID].tar.gz -C $HOME/dotnet
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
tar -zxf artifacts/x64/Release/dotnet-sdk-8.0.100-centos.8-x64.tar.gz -C $HOME/.dotnet
ln -s $HOME/.dotnet/dotnet /usr/bin/dotnet
```

### Codespaces

You can also utilize [GitHub Codespaces](https://github.com/features/codespaces) where you can find preset containers in this repository.

### Exporting a source archive

In case you'd like to export a more lightweight archive of sources that can be built outside of this git repository, a simple copy of the working tree won't do.
The build is using some git metadata (information from the `.git` directory) that are needed to be kept with the sources.
To export a `tar.gz` archive of the sources, you need to use the `eng/pack-sources.sh` script from within a clone of the VMR checked out at the revision that you're interested in.

## List of components

To enable full offline source-building of the VMR, we have no other choice than to synchronize all the necessary code into the VMR. This also includes any code referenced via git submodules. More details on why and how this is done can be found here:
- [Strategy for managing external source dependencies](src/arcade/Documentation/UnifiedBuild/VMR-Strategy-For-External-Source.md)
- [Source Synchronization Process](src/arcade/Documentation/UnifiedBuild/VMR-Design-And-Operation.md#source-synchronization-process)

### Detailed list

<!-- component list beginning -->
- `src/arcade`  
*[dotnet/arcade@02980a6](https://github.com/dotnet/arcade/commit/02980a654d20995c38eca6ceabf729c4c13412c4)*
- `src/aspnetcore`  
*[dotnet/aspnetcore@bb3f0d6](https://github.com/dotnet/aspnetcore/commit/bb3f0d626627d399695a9fa0741e4803c0c23eb8)*
    - `src/aspnetcore/src/submodules/googletest`  
    *[google/googletest@b73f27f](https://github.com/google/googletest/commit/b73f27fd164456fea9aba56163f5511355a03272)*
    - `src/aspnetcore/src/submodules/MessagePack-CSharp`  
    *[aspnet/MessagePack-CSharp@fe9fa08](https://github.com/aspnet/MessagePack-CSharp/commit/fe9fa0834d18492eb229ff2923024af2c87553f8)*
    - `src/aspnetcore/src/submodules/spa-templates`  
    *[dotnet/spa-templates@376a7f3](https://github.com/dotnet/spa-templates/commit/376a7f396d50d12f1d82818c8f03de1cb51198d5)*
- `src/cecil`  
*[dotnet/cecil@68e0c35](https://github.com/dotnet/cecil/commit/68e0c35d0b4b6651b9a062a52e7dd694d7a43927)*
- `src/command-line-api`  
*[dotnet/command-line-api@8374d5f](https://github.com/dotnet/command-line-api/commit/8374d5fca634a93458c84414b1604c12f765d1ab)*
- `src/deployment-tools`  
*[dotnet/deployment-tools@c3ad00a](https://github.com/dotnet/deployment-tools/commit/c3ad00ae84489071080a606f6a8e43c9a91a5cc2)*
- `src/diagnostics`  
*[dotnet/diagnostics@e3e1490](https://github.com/dotnet/diagnostics/commit/e3e1490a23f27a6e0ed30d323035660c3ffc52cd)*
- `src/emsdk`  
*[dotnet/emsdk@d7ff0aa](https://github.com/dotnet/emsdk/commit/d7ff0aa47c680be543905cc1410e2f62b54dfefe)*
- `src/format`  
*[dotnet/format@2cb3e68](https://github.com/dotnet/format/commit/2cb3e68c6b9a966114572fd63f2a20d2cb54a288)*
- `src/fsharp`  
*[dotnet/fsharp@75647e8](https://github.com/dotnet/fsharp/commit/75647e8f098a1a2b50d4d755394598ae6f38b164)*
- `src/installer`  
*[dotnet/installer@ccb3d41](https://github.com/dotnet/installer/commit/ccb3d41d0c4e35d942ad74bea8e68a9b05da1e54)*
- `src/msbuild`  
*[dotnet/msbuild@dfd8f41](https://github.com/dotnet/msbuild/commit/dfd8f413a80cd0865f968b2c0ad9b09c0df8c430)*
- `src/nuget-client`  
*[nuget/nuget.client@00375a7](https://github.com/nuget/nuget.client/commit/00375a7dfc2e2687c95ada0a0d24b570dc807d5d)*
    - `src/nuget-client/submodules/NuGet.Build.Localization`  
    *[NuGet/NuGet.Build.Localization@f15db7b](https://github.com/NuGet/NuGet.Build.Localization/commit/f15db7b7c6f5affbea268632ef8333d2687c8031)*
- `src/razor`  
*[dotnet/razor@23245e6](https://github.com/dotnet/razor/commit/23245e6bf9f65ef800009909a5de3e3ebb3f075e)*
- `src/roslyn`  
*[dotnet/roslyn@73338d9](https://github.com/dotnet/roslyn/commit/73338d92270b9f26982eca2e8872037a0214b912)*
- `src/roslyn-analyzers`  
*[dotnet/roslyn-analyzers@15abfb9](https://github.com/dotnet/roslyn-analyzers/commit/15abfb9a6ea6e28ec3ef7110e6efffa95bbf2cad)*
- `src/runtime`  
*[dotnet/runtime@f2f734a](https://github.com/dotnet/runtime/commit/f2f734a7e4f95d22a56994c127fe63d6aa97c780)*
- `src/sdk`  
*[dotnet/sdk@53fcc31](https://github.com/dotnet/sdk/commit/53fcc31d5ca2cc2cc57190eb06aa60e94d47d571)*
- `src/source-build-externals`  
*[dotnet/source-build-externals@46f5e8e](https://github.com/dotnet/source-build-externals/commit/46f5e8e5419df6689022dccf5ed5a7fd153cb601)*
    - `src/source-build-externals/src/application-insights`  
    *[Microsoft/ApplicationInsights-dotnet@51c3ed8](https://github.com/Microsoft/ApplicationInsights-dotnet/commit/51c3ed8aa3f32209edf01168f9136a3ac8486c5d)*
    - `src/source-build-externals/src/azure-activedirectory-identitymodel-extensions-for-dotnet`  
    *[AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet@a9de8ff](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/commit/a9de8ff14648770a3caa33a68d8061d0fa84d105)*
    - `src/source-build-externals/src/cssparser`  
    *[dotnet/cssparser@d6d86bc](https://github.com/dotnet/cssparser/commit/d6d86bcd8c162b1ae22ef00955ff748d028dd0ee)*
    - `src/source-build-externals/src/humanizer`  
    *[Humanizr/Humanizer@3ebc38d](https://github.com/Humanizr/Humanizer/commit/3ebc38de585fc641a04b0e78ed69468453b0f8a1)*
    - `src/source-build-externals/src/MSBuildLocator`  
    *[microsoft/MSBuildLocator@5484ae4](https://github.com/microsoft/MSBuildLocator/commit/5484ae4729a60a01724987a4e6658ea506ef279d)*
    - `src/source-build-externals/src/newtonsoft-json`  
    *[JamesNK/Newtonsoft.Json@ae9fe44](https://github.com/JamesNK/Newtonsoft.Json/commit/ae9fe44e1323e91bcbd185ca1a14099fba7c021f)*
- `src/source-build-reference-packages`  
*[dotnet/source-build-reference-packages@d842772](https://github.com/dotnet/source-build-reference-packages/commit/d8427722f537c3fbdc753dce52af1f104a8048d6)*
- `src/sourcelink`  
*[dotnet/sourcelink@763c618](https://github.com/dotnet/sourcelink/commit/763c618bbc4cc6481d37d122dc0dd3c2cafbcd49)*
- `src/symreader`  
*[dotnet/symreader@7b9791d](https://github.com/dotnet/symreader/commit/7b9791daa3a3477eb22ec805946c9fff8b42d8ca)*
- `src/templating`  
*[dotnet/templating@3b1069b](https://github.com/dotnet/templating/commit/3b1069b865d675579e7e6cfd1cf7509051e004e5)*
- `src/test-templates`  
*[dotnet/test-templates@bb36956](https://github.com/dotnet/test-templates/commit/bb3695688177f5f80eeb3c0498168612e31549d5)*
- `src/vstest`  
*[microsoft/vstest@9da4e7f](https://github.com/microsoft/vstest/commit/9da4e7f7505af42b34e1a2d7ba6857f98782025f)*
- `src/xdt`  
*[dotnet/xdt@9a1c3e1](https://github.com/dotnet/xdt/commit/9a1c3e1b7f0c8763d4c96e593961a61a72679a7b)*
- `src/xliff-tasks`  
*[dotnet/xliff-tasks@3cb4fa0](https://github.com/dotnet/xliff-tasks/commit/3cb4fa02ba389a7f070a16c50227cba2e610b48d)*
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
