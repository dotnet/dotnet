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

### Building outside of git

.NET uses git metadata so that it can link assemblies to their original source code when debugging (think "Step into.." functionality) and for that it needs information about the original place the code comes from.
When you're building source code only, taken outside of context of a git repository (e.g. you download it from the release page), you will need to specify the source repository to the `prep.sh` script via the `--source-repository` and `--source-version` arguments. This can be your fork of the repository and should match the origin where your SDK was built from.

## List of components

To enable full offline source-building of the VMR, we have no other choice than to synchronize all the necessary code into the VMR. This also includes any code referenced via git submodules. More details on why and how this is done can be found here:
- [Strategy for managing external source dependencies](src/arcade/Documentation/UnifiedBuild/VMR-Strategy-For-External-Source.md)
- [Source Synchronization Process](src/arcade/Documentation/UnifiedBuild/VMR-Design-And-Operation.md#source-synchronization-process)

### Detailed list

<!-- component list beginning -->
- `src/arcade`  
*[dotnet/arcade@dca04a5](https://github.com/dotnet/arcade/commit/dca04a5ec44fe1e75535f51a4e0b9a7aa2318de7)*
- `src/aspnetcore`  
*[dotnet/aspnetcore@03e670f](https://github.com/dotnet/aspnetcore/commit/03e670f9854651b9995d57f8e87cef16022e739a)*
    - `src/aspnetcore/src/submodules/googletest`  
    *[google/googletest@12a5852](https://github.com/google/googletest/commit/12a5852e451baabc79c63a86c634912c563d57bc)*
    - `src/aspnetcore/src/submodules/MessagePack-CSharp`  
    *[aspnet/MessagePack-CSharp@ecc4e18](https://github.com/aspnet/MessagePack-CSharp/commit/ecc4e18ad7a0c7db51cd7e3d2997a291ed01444d)*
    - `src/aspnetcore/src/submodules/spa-templates`  
    *[dotnet/spa-templates@3c41fe2](https://github.com/dotnet/spa-templates/commit/3c41fe26f7c51fc85fbc97b92871765467dd533c)*
- `src/cecil`  
*[dotnet/cecil@9a7551f](https://github.com/dotnet/cecil/commit/9a7551fa7ba8218affbc1148eabc4b5c9f4e1151)*
- `src/command-line-api`  
*[dotnet/command-line-api@8374d5f](https://github.com/dotnet/command-line-api/commit/8374d5fca634a93458c84414b1604c12f765d1ab)*
- `src/deployment-tools`  
*[dotnet/deployment-tools@9e87099](https://github.com/dotnet/deployment-tools/commit/9e870996b8bf0b91a791edd1039bfd23bdd01af8)*
- `src/diagnostics`  
*[dotnet/diagnostics@5ce78f6](https://github.com/dotnet/diagnostics/commit/5ce78f66d89ea529e459abddb129ab36cb5bd936)*
- `src/emsdk`  
*[dotnet/emsdk@2327f6b](https://github.com/dotnet/emsdk/commit/2327f6b63d887979222f5bcf15f3322f800b9695)*
- `src/format`  
*[dotnet/format@0b96805](https://github.com/dotnet/format/commit/0b968051beac5d7c1a62a52aee7fbbbb47dc1f47)*
- `src/fsharp`  
*[dotnet/fsharp@b9c0f04](https://github.com/dotnet/fsharp/commit/b9c0f04d0ec3ec292bde52acccbc41c208931f07)*
- `src/installer`  
*[dotnet/installer@7b80cf4](https://github.com/dotnet/installer/commit/7b80cf4e8aca5ee118b234c8b685d1e529cc7306)*
- `src/msbuild`  
*[dotnet/msbuild@cc55017](https://github.com/dotnet/msbuild/commit/cc55017f88688cbe3f9aa810cdf44273adea76ea)*
- `src/nuget-client`  
*[nuget/nuget.client@7eaba9f](https://github.com/nuget/nuget.client/commit/7eaba9f3f68b8df63d5dcf10e2a8ef3591164ddf)*
    - `src/nuget-client/submodules/NuGet.Build.Localization`  
    *[NuGet/NuGet.Build.Localization@f15db7b](https://github.com/NuGet/NuGet.Build.Localization/commit/f15db7b7c6f5affbea268632ef8333d2687c8031)*
- `src/razor`  
*[dotnet/razor@f182ffb](https://github.com/dotnet/razor/commit/f182ffbd63af02071c127b27d2478fd51e69e983)*
- `src/roslyn`  
*[dotnet/roslyn@a6a78dd](https://github.com/dotnet/roslyn/commit/a6a78dd64fc1eb466113ece272747a72d497aad7)*
- `src/roslyn-analyzers`  
*[dotnet/roslyn-analyzers@8011226](https://github.com/dotnet/roslyn-analyzers/commit/801122692fc2953759f41330f5d42e9144a034ea)*
- `src/runtime`  
*[dotnet/runtime@6887fc4](https://github.com/dotnet/runtime/commit/6887fc439b2d211608ed7665f795069555ae6df4)*
- `src/sdk`  
*[dotnet/sdk@ddd3409](https://github.com/dotnet/sdk/commit/ddd34099073c212b5a8148737d93de8e0a458936)*
- `src/source-build-externals`  
*[dotnet/source-build-externals@de4dda4](https://github.com/dotnet/source-build-externals/commit/de4dda48d0cf31e13182bc24107b2246c61ed483)*
    - `src/source-build-externals/src/application-insights`  
    *[Microsoft/ApplicationInsights-dotnet@51c3ed8](https://github.com/Microsoft/ApplicationInsights-dotnet/commit/51c3ed8aa3f32209edf01168f9136a3ac8486c5d)*
    - `src/source-build-externals/src/azure-activedirectory-identitymodel-extensions-for-dotnet`  
    *[AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet@a9de8ff](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/commit/a9de8ff14648770a3caa33a68d8061d0fa84d105)*
    - `src/source-build-externals/src/cssparser`  
    *[dotnet/cssparser@0d59611](https://github.com/dotnet/cssparser/commit/0d59611784841735a7778a67aa6e9d8d000c861f)*
    - `src/source-build-externals/src/docker-creds-provider`  
    *[mthalman/docker-creds-provider@e92e354](https://github.com/mthalman/docker-creds-provider/commit/e92e354c78d6d5c60e9d0f679712c871bef97ebe)*
    - `src/source-build-externals/src/humanizer`  
    *[Humanizr/Humanizer@3ebc38d](https://github.com/Humanizr/Humanizer/commit/3ebc38de585fc641a04b0e78ed69468453b0f8a1)*
    - `src/source-build-externals/src/MSBuildLocator`  
    *[microsoft/MSBuildLocator@5484ae4](https://github.com/microsoft/MSBuildLocator/commit/5484ae4729a60a01724987a4e6658ea506ef279d)*
    - `src/source-build-externals/src/newtonsoft-json`  
    *[JamesNK/Newtonsoft.Json@ae9fe44](https://github.com/JamesNK/Newtonsoft.Json/commit/ae9fe44e1323e91bcbd185ca1a14099fba7c021f)*
- `src/source-build-reference-packages`  
*[dotnet/source-build-reference-packages@c73065b](https://github.com/dotnet/source-build-reference-packages/commit/c73065ba502e1982f4ae743ebd0fc6bb8cf06690)*
- `src/sourcelink`  
*[dotnet/sourcelink@d4e7ffb](https://github.com/dotnet/sourcelink/commit/d4e7ffb47785c4ff21fcffa0a29aa20fec3a5980)*
- `src/symreader`  
*[dotnet/symreader@0c29b71](https://github.com/dotnet/symreader/commit/0c29b7109c054bdc578e917515ae7e8635b9cb9d)*
- `src/templating`  
*[dotnet/templating@6536e8d](https://github.com/dotnet/templating/commit/6536e8d9b4ae1e70047cf4c254c352db75b99903)*
- `src/test-templates`  
*[dotnet/test-templates@45a41b3](https://github.com/dotnet/test-templates/commit/45a41b353d078e4d1e5918246fddbacb452eb210)*
- `src/vstest`  
*[microsoft/vstest@1541fc3](https://github.com/microsoft/vstest/commit/1541fc31ead99a358c649e7e9fa203e0dd4d20a5)*
- `src/xdt`  
*[dotnet/xdt@9a1c3e1](https://github.com/dotnet/xdt/commit/9a1c3e1b7f0c8763d4c96e593961a61a72679a7b)*
- `src/xliff-tasks`  
*[dotnet/xliff-tasks@2d18d93](https://github.com/dotnet/xliff-tasks/commit/2d18d93582529a08ec2ff6da2d2f5c8e9a6b5e26)*
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
