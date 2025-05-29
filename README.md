# dotnet/dotnet - Home of the .NET VMR

This repository is a **Virtual Monolithic Repository (VMR)** which includes all the source code and infrastructure needed to build the .NET SDK.

What this means:

- **Monolithic** - a join of multiple repositories that make up the whole product, such as [dotnet/runtime](https://github.com/dotnet/runtime) or [dotnet/sdk](https://github.com/dotnet/sdk).
- **Virtual** - a mirror (not replacement) of product repos where sources from those repositories are synchronized with.

In the VMR, you can find:

- source files of each product repository which are mirrored inside of their respective directories under [`src/`](https://github.com/dotnet/dotnet/tree/main/src),
- tooling that enables [building the whole .NET product from source](https://github.com/dotnet/source-build) on Linux platforms,
- small customizations, in the form of [patches](https://github.com/dotnet/dotnet/tree/main/src/sdk/src/SourceBuild/patches), applied on top of the original code to make the build possible,
- *[in future]* E2E tests for the whole .NET product.

Just like the development repositories, the VMR will have a release branch for every feature band (e.g. `release/10.0.1xx`).
Similarly, VMR's `main` branch will follow default branches of product repositories (see [Synchronization Based on Declared Dependencies](docs/VMR-Design-And-Operation.md#synchronization-based-on-declared-dependencies)).

More in-depth documentation about the VMR can be found in [VMR Design And Operation](docs/VMR-Design-And-Operation.md#layout).
See also [dotnet/source-build](https://github.com/dotnet/source-build) for more information about our whole-product source-build.

## Installing the SDK

You can download the .NET SDK either as an installer (MSI, PKG) or as an archive (zip, tar.gz). The .NET SDK contains both the .NET runtimes and CLI tools.

- [**Latest builds table**](docs/builds-table.md)

## Goals

- The main purpose of the [dotnet/dotnet](https://github.com/dotnet/dotnet) repository is to have all source code necessary to build the .NET product available in one repository and identified by a single commit.
- The VMR also aims to become the place from which we release and service future versions of .NET to reduce the complexity of the product construction process. This should allow our partners and and 3rd parties to easily build, test and modify .NET using their custom infrastructure as well as make the process available to the community.
- Lastly, we hope to solve other problems that the current multi-repo setup brings:
  - Enable the standard [down-/up-stream open-source model](docs/VMR-Upstream-Downstream.md).
  - Fulfill requirements of .NET distro builders such as RedHat or Canonical to natively include .NET in their distribution repositories.
  - Simplify scenarios such as client-run testing of bug fixes and improvements. The build should work in an offline environment too for certain platforms.
  - Enable developers to make and test changes spanning multiple repositories.
  - More efficient pipeline for security fixes during the CVE pre-disclosure process.

We will achieve these goals while keeping active coding work in the separate repos where it happens today. For example: ASP.NET features will continue to be developed in `dotnet/aspnetcore` and CLR features will be continue to be developed in `dotnet/runtime`. Each of these repos have their own distinct communities and processes, and aggregating development into a true mono-repo would work against that. Hence, the "virtual" monolithic repo: the VMR gives us the simplicity of a mono-repo for building and servicing the product, while active development of components of that product stays in its various existing repos. The day to day experience for typical contributors will not change.

## Supported platforms

- 8.0 and 9.0
  - source-build configuration on Linux
- 10.0+ (WIP)
  - source-build configuration on Linux
  - non-source-build configuration on Linux, Mac, and Windows

For the latest information about Source-Build support for new .NET versions, please check our [GitHub Discussions page](https://github.com/dotnet/source-build/discussions) for announcements.

## Code flow

The VMR's code flow operates in two directions. Individual repositories flow source changes into the VMR upon promotion of their local official builds (forward flow). The VMR changes are checked in, an official build happens, and then source changes + packages flows backward into the constituent repositories (back flow). For more details on code flow and code flow pull requests, please see this information on [Code Flow PRs](src/arcade/Documentation/UnifiedBuild/Codeflow-PRs.md).

## Contribution

Contribution to the .NET product should currently be done mostly in the constituent repositories. The reasons for this are two-fold:
- We want to slowly ramp up direct VMR changes to avoid surprises.
- The individual repositories still have the best validation for most changes.

If you would like to make a cross-cutting change in the VMR, please ask the Unified Build team (please tag @dotnet/product-construction in an issue/discussion in your repository). However, some changes **should** be made directly in the VMR. For a breakdown of where changes should be made, please see below.

#### Where to make changes:

- `src/*` - Constituent repositories, except VMR pipeline changes.
- Non `src/*` directories - Directly in VMR
- Arcade `eng/common` changes - There are many copies of eng/common in the VMR:
  - The VMR uses its root eng/common/* to bootstrap the VMR build. These should not be updated manually. They should only be updated via a re-bootrap of the VMR.
  - A VMR build uses `src/arcade/eng/common/*` for arcade and any repository that builds after arcade. Changes may be made to these files, and they will flow back into arcade as well as to any repository that gets its arcade flow from the VMR. However, due to varying scenarios in which `eng/common/` can be used, it is generally recommended that the VMR only be used to test `eng/common` changes, while actual changes should still be made in the dotnet/arcade repository.
- VMR pipeline changes - The root pipeline logic lives in eng/* and should be changed in the VMR.

For any questions, please ask the Unified Build team.

## Dev instructions

Please note that **this repository is a work-in-progress** and there are some usability issues connected to this.
These can be nuisances such as some checked-in files getting modified by the build itself and similar.
For the latest information about Source-Build support, please watch for announcements posted on our [GitHub Discussions page](https://github.com/dotnet/source-build/discussions).

### Prerequisites

The dependencies for building can be found [here](https://github.com/dotnet/runtime/blob/main/docs/workflow/requirements/).
In case you don't want to / cannot prepare your environment per the requirements, consider [using Docker](#building-using-docker).

For building the VMR with Source-Build, the following additional dependencies are required for your Linux environment:
* `brotli-dev`

For building the VMR on Windows, it is recommended to put the repo under a short path, i.e. `C:\dotnet`. Also, [long path support must be enabled](https://learn.microsoft.com/en-us/windows/win32/fileio/maximum-file-path-limitation?tabs=registry#enable-long-paths-in-windows-10-version-1607-and-later). This is necessary as some of the tools used don't support long paths (WiX Toolset v3 and cl.exe).

For some `git` commands and when synchronizing changes via the `darc` CLI, long path support should be enabled in the `git` config as well:
```bash
git config --system core.longpaths true # needs elevated prompt
git config --global core.longpaths true
```

### Building

1. **Clone the repository**

   ```bash
   git clone https://github.com/dotnet/dotnet dotnet-dotnet
   cd dotnet-dotnet
   ```

1. **Build the .NET SDK**

    Choose one of the following build modes:

    - **Microsoft based build**

        For Unix:

        ```bash
        ./build.sh --clean-while-building
        ```

        For Windows:

        ```cmd
        .\build.cmd -cleanWhileBuilding
        ```

    - **Building from source**

        ```bash
        # Prep the source to build on your distro.
        # This downloads a .NET SDK and a number of .NET packages needed to build .NET from source.
        ./prep-source-build.sh

        # Build the .NET SDK
        ./build.sh -sb --clean-while-building
        ```

    The resulting SDK is placed at `artifacts/assets/Release/dotnet-sdk-9.0.100-[your-RID].tar.gz` (for Unix) or `artifacts/assets/Release/dotnet-sdk-9.0.100-[your-RID].zip` (for Windows).

1. *(Optional)* **Unpack and install the .NET SDK**

    For Unix:

    ```bash
    mkdir -p $HOME/dotnet
    tar zxf artifacts/assets/Release/dotnet-sdk-10.0.100-[your-RID].tar.gz -C $HOME/dotnet
    ln -s $HOME/dotnet/dotnet /usr/bin/dotnet
    ```

    For Windows:

    ```cmd
    mkdir %userprofile%\dotnet
    tar -xf artifacts/assets/Release/dotnet-sdk-10.0.100-[your RID].zip -C %userprofile%\dotnet
    set "PATH=%userprofile%\dotnet;%PATH%"
    ```

    To test your built SDK, run the following:

    ```bash
    dotnet --info
    ```

> [!NOTE]
> Run `./build.sh --help` (for Unix) or `.\build.cmd -help` (for Windows) to see more information about supported build options.

### Building using Docker

You can also build the repository using a Docker image which has the required prerequisites inside.
The example below creates a Docker volume named `vmr` and clones and builds the VMR there.

```sh
docker run --rm -it -v vmr:/vmr -w /vmr mcr.microsoft.com/dotnet-buildtools/prereqs:centos-stream9
git clone https://github.com/dotnet/dotnet .

# - Microsoft based build
./build.sh --clean-while-building

# - Building from source
./prep-source-build.sh && ./build.sh -sb --clean-while-building

mkdir -p $HOME/.dotnet
tar -zxf artifacts/assets/Release/dotnet-sdk-9.0.100-centos.9-x64.tar.gz -C $HOME/.dotnet
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

Sometimes you want to make a change in a repository and test that change in the VMR. You could of course make the change in the VMR directly, but in case it's already available in your repository, you can synchronize it locally into your clone of the VMR, commit, and then open a PR.

To do this, you need to use the [`darc vmr forwardflow` command](https://github.com/dotnet/arcade-services/blob/main/docs/Darc.md#forwardflow) which can move your changes from your repository's dev branch into a local VMR one. Please refer to command's documentation (`--help`) for more details.

## Filing Issues

This repo should contain issues that are tied to the VMR infrastructure and documentation.

For other issues, please open them in the appropriate product repos. We have links to many of them on [our new issue page](https://github.com/dotnet/dotnet/issues/new/choose).

## Useful Links

- Design documentation for the VMR - a set of documents describing the high-level design and the why's and how's
  - [Design and Operation](docs/VMR-Design-And-Operation.md)
  - [Upstream/Downstream Relationships](docs/VMR-Upstream-Downstream.md)
  - [Code and Build Workflow](docs/VMR-Code-And-Build-Workflow.md)
  - [Strategy for Managing External Source Dependencies](docs/VMR-Strategy-For-External-Source.md)
- [.NET Source-Build](https://github.com/dotnet/source-build)
- [What is .NET](https://dotnet.microsoft.com)

## .NET Foundation

.NET Runtime is a [.NET Foundation](https://www.dotnetfoundation.org/projects) project.

## License

.NET is licensed under the [MIT](LICENSE.TXT) license.
