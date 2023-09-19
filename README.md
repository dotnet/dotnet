# dotnet/dotnet - Home of the .NET VMR

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

### Building from released sources

You can also build from sources (and not from a context of a git repository), such as the ones you can acquire from a [dotnet/dotnet release](https://github.com/dotnet/dotnet/releases).
In this case, you need to provide additional information which includes the original repository and commit hash the code was built from so that the SDK can provide a better debugging experience (think the `Step into..` functionality).
Usually, this means the [dotnet/dotnet repository](https://github.com/dotnet/dotnet) together with the commit the release tag is connected to.

In practice, this means that when calling the main build script, you need to provide additional arguments when building outside of a context of a git repository.  
Alternatively, you can also provide a manifest file where this information can be read from. This file (`release.json`) can be found attached with the [dotnet/dotnet release](https://github.com/dotnet/dotnet/releases).

## List of components

To enable full offline source-building of the VMR, we have no other choice than to synchronize all the necessary code into the VMR. This also includes any code referenced via git submodules. More details on why and how this is done can be found here:
- [Strategy for managing external source dependencies](src/arcade/Documentation/UnifiedBuild/VMR-Strategy-For-External-Source.md)
- [Source Synchronization Process](src/arcade/Documentation/UnifiedBuild/VMR-Design-And-Operation.md#source-synchronization-process)

### Detailed list

<!-- component list beginning -->
- `src/arcade`  
*[dotnet/arcade@1d451c3](https://github.com/dotnet/arcade/commit/1d451c32dda2314c721adbf8829e1c0cd4e681ff)*
- `src/aspnetcore`  
*[_git/dotnet-aspnetcore@3529714](https://dev.azure.com/dnceng/internal/_git/dotnet-aspnetcore/commit/3529714ed90eea0cdb60b347e0deeca02771c5b5)*
    - `src/aspnetcore/src/submodules/googletest`  
    *[google/googletest@7e33b6a](https://github.com/google/googletest/commit/7e33b6a1c497ced1e98fc60175aeb4678419281c)*
    - `src/aspnetcore/src/submodules/MessagePack-CSharp`  
    *[aspnet/MessagePack-CSharp@ecc4e18](https://github.com/aspnet/MessagePack-CSharp/commit/ecc4e18ad7a0c7db51cd7e3d2997a291ed01444d)*
- `src/cecil`  
*[dotnet/cecil@89be445](https://github.com/dotnet/cecil/commit/89be445dd4936157533ad96bafb95f701430653a)*
- `src/command-line-api`  
*[dotnet/command-line-api@02fe27c](https://github.com/dotnet/command-line-api/commit/02fe27cd6a9b001c8feb7938e6ef4b3799745759)*
- `src/deployment-tools`  
*[dotnet/deployment-tools@5957c5c](https://github.com/dotnet/deployment-tools/commit/5957c5c5f85f17c145e7fab4ece37ad6aafcded9)*
- `src/diagnostics`  
*[dotnet/diagnostics@5ce78f6](https://github.com/dotnet/diagnostics/commit/5ce78f66d89ea529e459abddb129ab36cb5bd936)*
- `src/emsdk`  
*[dotnet/emsdk@8e857c9](https://github.com/dotnet/emsdk/commit/8e857c92bff556d919f5f904bd9c777aade4afba)*
- `src/format`  
*[dotnet/format@0cf0f51](https://github.com/dotnet/format/commit/0cf0f51b02d759e89f2acc09663493ab422548b0)*
- `src/fsharp`  
*[dotnet/fsharp@10f956e](https://github.com/dotnet/fsharp/commit/10f956e631a1efc0f7f5e49c626c494cd32b1f50)*
- `src/installer`  
*[dotnet/installer@7def16a](https://github.com/dotnet/installer/commit/7def16ad0e2f31dc47a362d818d2b809a93d6185)*
- `src/msbuild`  
*[dotnet/msbuild@6cdef42](https://github.com/dotnet/msbuild/commit/6cdef424154c976f04802b101e6be6292f8a8897)*
- `src/nuget-client`  
*[nuget/nuget.client@7fb5ed8](https://github.com/nuget/nuget.client/commit/7fb5ed887352d2892797a365cfdd7bb8df029941)*
    - `src/nuget-client/submodules/NuGet.Build.Localization`  
    *[NuGet/NuGet.Build.Localization@f15db7b](https://github.com/NuGet/NuGet.Build.Localization/commit/f15db7b7c6f5affbea268632ef8333d2687c8031)*
- `src/razor`  
*[dotnet/razor@2daeaaa](https://github.com/dotnet/razor/commit/2daeaaaed440a9c59b063b1578616850a0ccddd1)*
- `src/roslyn`  
*[dotnet/roslyn@bdd9c5b](https://github.com/dotnet/roslyn/commit/bdd9c5ba66b00beebdc3516acc5e29b83efd89af)*
- `src/roslyn-analyzers`  
*[dotnet/roslyn-analyzers@4a7701f](https://github.com/dotnet/roslyn-analyzers/commit/4a7701fd72094614897b33e4cb1d9640c221d862)*
- `src/runtime`  
*[_git/dotnet-runtime@0b25e38](https://dev.azure.com/dnceng/internal/_git/dotnet-runtime/commit/0b25e38ad32a69cd83ae246104b32449203cc71c)*
- `src/sdk`  
*[_git/dotnet-sdk@fd120c5](https://dev.azure.com/dnceng/internal/_git/dotnet-sdk/commit/fd120c5b9ebf88506f63a095bd1cbe34cc1406ff)*
- `src/source-build-externals`  
*[dotnet/source-build-externals@6dbf3aa](https://github.com/dotnet/source-build-externals/commit/6dbf3aaa0fc9664df86462f5c70b99800934fccd)*
    - `src/source-build-externals/src/abstractions-xunit`  
    *[xunit/abstractions.xunit@b75d54d](https://github.com/xunit/abstractions.xunit/commit/b75d54d73b141709f805c2001b16f3dd4d71539d)*
    - `src/source-build-externals/src/application-insights`  
    *[Microsoft/ApplicationInsights-dotnet@5e2e7dd](https://github.com/Microsoft/ApplicationInsights-dotnet/commit/5e2e7ddda961ec0e16a75b1ae0a37f6a13c777f5)*
    - `src/source-build-externals/src/azure-activedirectory-identitymodel-extensions-for-dotnet`  
    *[AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet@bf4cb25](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/commit/bf4cb251a85f1b27bbb208c703f6f3105bdb24ca)*
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
*[dotnet/source-build-reference-packages@d825c66](https://github.com/dotnet/source-build-reference-packages/commit/d825c6693d4e26f63aaa93c3c1d057faa098e347)*
- `src/sourcelink`  
*[dotnet/sourcelink@ea3bd92](https://github.com/dotnet/sourcelink/commit/ea3bd92278af83bae656ad9747c11f5a345e5b4a)*
- `src/symreader`  
*[dotnet/symreader@2c8079e](https://github.com/dotnet/symreader/commit/2c8079e2e8e78c0cd11ac75a32014756136ecdb9)*
- `src/templating`  
*[_git/dotnet-templating@bd1b65d](https://dev.azure.com/dnceng/internal/_git/dotnet-templating/commit/bd1b65d553c65266182264b7aa72fc91943afa48)*
- `src/test-templates`  
*[dotnet/test-templates@1e5f360](https://github.com/dotnet/test-templates/commit/1e5f3603af2277910aad946736ee23283e7f3e16)*
- `src/vstest`  
*[microsoft/vstest@cf7d549](https://github.com/microsoft/vstest/commit/cf7d549fc0197abaabec19d61d2c20d7a7b089f8)*
- `src/xdt`  
*[dotnet/xdt@9a1c3e1](https://github.com/dotnet/xdt/commit/9a1c3e1b7f0c8763d4c96e593961a61a72679a7b)*
- `src/xliff-tasks`  
*[dotnet/xliff-tasks@194f328](https://github.com/dotnet/xliff-tasks/commit/194f32828726c3f1f63f79f3dc09b9e99c157b11)*
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
