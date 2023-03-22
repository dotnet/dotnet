# .NET SDK Installers

[![Join the chat at https://gitter.im/dotnet/cli](https://badges.gitter.im/Join%20Chat.svg)](https://gitter.im/dotnet/cli?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)
[![GitHub release](https://img.shields.io/github/release/dotnet/installer.svg)](https://GitHub.com/dotnet/installer/releases/)
[![GitHub repo size](https://img.shields.io/github/repo-size/dotnet/installer)](https://github.com/dotnet/installer)
[![GitHub issues-opened](https://img.shields.io/github/issues/dotnet/installer.svg)](https://GitHub.com/dotnet/installer/issues?q=is%3Aissue+is%3Aopened)
[![GitHub issues-closed](https://img.shields.io/github/issues-closed/dotnet/installer.svg)](https://GitHub.com/dotnet/installer/issues?q=is%3Aissue+is%3Aclosed)
[![GitHub pulls-opened](https://img.shields.io/github/issues-pr/dotnet/installer.svg)](https://GitHub.com/dotnet/installer/pulls?q=is%3Aissue+is%3Aopened)
[![GitHub pulls-merged](https://img.shields.io/github/issues-search/dotnet/installer?label=merged%20pull%20requests&query=is%3Apr%20is%3Aclosed%20is%3Amerged&color=darkviolet)](https://github.com/dotnet/installer/pulls?q=is%3Apr+is%3Aclosed+is%3Amerged)
[![GitHub pulls-unmerged](https://img.shields.io/github/issues-search/dotnet/installer?label=unmerged%20pull%20requests&query=is%3Apr%20is%3Aclosed%20is%3Aunmerged&color=red)](https://github.com/dotnet/installer/pulls?q=is%3Apr+is%3Aclosed+is%3Aunmerged)
[![GitHub contributors](https://img.shields.io/github/contributors/dotnet/installer.svg)](https://GitHub.com/dotnet/installer/graphs/contributors/)
[![Commit Activity](https://img.shields.io/github/commit-activity/m/dotnet/installer)]()

This repo contains the source code for the cross-platform [.NET](http://github.com/dotnet/core) SDK. It aggregates the .NET toolchain, the .NET runtime, the templates, and the .NET  Windows Desktop runtime. It produces zip, tarballs, and native packages for various supported platforms.

Looking for released versions of the .NET tooling?
----------------------------------------

The links below are for preview versions of .NET tooling. Prefer to use released versions of the .NET tools? Go to https://dot.net/download.

Looking for .NET Framework downloads?
----------------------------------------

.NET Framework is the product from which the .NET Core project originated. .NET Core (mostly just called ".NET" here) adds many features and improvements and supports many more platforms than .NET Framework. .NET Framework remains fully supported and you can find the downloads on the [.NET website](https://dotnet.microsoft.com/download/dotnet-framework). For new projects, we recommend you use .NET Core.

Want to contribute or find out more about the .NET project?
----------------------------------------

This repo is for the installers. Most of the implementation is in other repos, such as the [dotnet/runtime repo](https://github.com/dotnet/runtime) or the [dotnet/aspnetcore repo](https://github.com/dotnet/aspnetcore) and [many others](https://github.com/dotnet/core/blob/main/Documentation/core-repos.md). We welcome you to join us there!

Found an issue?
---------------
You can consult the [Documents Index for the SDK repo](https://github.com/dotnet/sdk/blob/main/documentation/README.md) to find out current issues, see workarounds, and to see how to file new issues.

This project has adopted the code of conduct defined by the [Contributor Covenant](http://contributor-covenant.org/) to clarify expected behavior in our community. For more information, see the [.NET Foundation Code of Conduct](http://www.dotnetfoundation.org/code-of-conduct).

# Build .NET installer

The repository contains native code project required for the Windows installer. If you intend to build it locally on Windows, you will need to ensure that you have the following items installed.
- Install CMAKE 3.21.0 is required if you're building VS 17.0. Make sure to add CMAKE to your PATH (the installer will prompt you).
- Install MSVC Build tools for x86/x64/arm64, v14.28-16.9

- `build` for basic build
- `build -pack` to build the installer
- To build in VS, run a command line build first, then run `artifacts\core-sdk-build-env.bat` from a VS command prompt and then `devenv Microsoft.DotNet.Cli.sln`
- To test different languages of the installer, run `artifacts\packages\Debug\Shipping>dotnet-sdk-3.1.412-win-x64.exe /lang 1046` using the LCID of the language you want to test

# Build .NET from source (source-build)

This repo also contains code to help you build the entire .NET product end-to-end from source (often referred to as source-build), even in disconnected/offline mode.
Please see the [dotnet/source-build](https://github.com/dotnet/source-build) repo for more information.

## Support

.NET Source-Build is supported on the oldest available .NET SDK feature update for each major release, and on Linux only.
For example, if .NET 6.0.1xx, 6.0.2xx, and 7.0.1xx feature updates are available from [dotnet.microsoft.com](https://dotnet.microsoft.com/en-us/download/dotnet/6.0), Source-Build will support 6.0.1xx and 7.0.1xx.
For the latest information about Source-Build support for new .NET versions, please check our [GitHub Discussions page](https://github.com/dotnet/source-build/discussions) for announcements.

## Prerequisites

The dependencies for building .NET from source can be found [here](https://github.com/dotnet/runtime/blob/main/docs/workflow/requirements/linux-requirements.md).

## Building .NET 8.0

.NET 8.0 (currently in prerelease) and newer will be built from the [dotnet/dotnet](https://github.com/dotnet/dotnet) repo.
Clone the dotnet/dotnet repo and check out the tag for the desired release.
Then, follow the instructions in [dotnet/dotnet's README](https://github.com/dotnet/dotnet/blob/main/README.md#dev-instructions) to build .NET from source.

### Codespaces

It is also possible to utilize [GitHub Codespaces](https://github.com/features/codespaces) and build .NET from the `dotnet/dotnet` repository from source that way.
You can either create a Codespace in `dotnet/dotnet` directly or you can also make one from a PR branch in `dotnet/installer`. This will give you an environment with the VMR checked out and containing all of new changes from the PR.
This can be especially valuable for investigations of source-build failures during PRs.

To create a Codespace for a `dotnet/installer` PR, use the `vmr-source-build` devcontainer configuration (select this when "newing the Codespace with options" under the three-dots-menu).
Further instructions on how to build inside of the Codespace will be available upon launch.

## Building .NET 7.0 and .NET 6.0

1. Create a .NET source tarball.

   ```bash
   ./build.sh /p:ArcadeBuildTarball=true /p:TarballDir=/path/to/place/complete/dotnet/sources
   ```

   This fetches the complete .NET source code and creates a tarball at `artifacts/packages/<Release|Debug>/Shipping/`.
   The extracted source code is also placed at `/path/to/place/complete/dotnet/sources`.
   The source directory should be outside (and not somewhere under) the installer directory.

2. Prep the source to build on your distro. This downloads a .NET SDK and a number of .NET packages needed to build .NET from source.

    ```bash
    cd /path/to/complete/dotnet/sources
    ./prep.sh
    ```

3. Build the .NET SDK

    ```bash
    ./build.sh --clean-while-building
    ```

    This builds the entire .NET SDK from source.
    The resulting SDK is placed at `artifacts/x64/Release/dotnet-sdk-7.0.100-your-RID.tar.gz`.

    Optionally add the `--online` flag to add online NuGet restore sources to the build.
    This is useful for testing unsupported releases that don't yet build without downloading pre-built binaries from the internet.

    Run `./build.sh --help` to see more information about supported build options.

4. (Optional) Unpack and install the .NET SDK

    ```bash
    mkdir -p $HOME/dotnet
    tar zxf artifacts/x64/Release/dotnet-sdk-7.0.100-your-RID.tar.gz -C $HOME/dotnet
    ln -s $HOME/dotnet/dotnet /usr/bin/dotnet
    ```
    
    To test your source-built SDK, run the following:

    ```bash
    dotnet --info
    ```

# Build status

Visibility|All legs|
|:------|:------|
|Public|[![Status](https://dev.azure.com/dnceng-public/public/_apis/build/status/20)](https://dev.azure.com/dnceng/public/_build?definitionId=20)|
|Microsoft Internal|[![Status](https://dev.azure.com/dnceng/internal/_apis/build/status/286)](https://dev.azure.com/dnceng/internal/_build?definitionId=286)|

## Installers and Binaries

You can download the .NET SDK as either an installer (MSI, PKG) or a zip (zip, tar.gz). The .NET SDK contains both the .NET runtime and CLI tools.

**Note:** Be aware that the following installers are the **latest bits**. If you
want to install the latest released versions, check out the [preceding section](#looking-for-released-versions-of-the-net-core-tooling).
With development builds, internal NuGet feeds are necessary for some scenarios (for example, to acquire the runtime pack for self-contained apps). You can use the following NuGet.config to configure these feeds. See the following document [Configuring NuGet behavior](https://docs.microsoft.com/en-us/nuget/consume-packages/configuring-nuget-behavior) for more information on where to modify your NuGet.config to apply the changes.

**For .NET 8 builds**

```xml
<configuration>
  <packageSources>
    <add key="dotnet8" value="https://pkgs.dev.azure.com/dnceng/public/_packaging/dotnet8/nuget/v3/index.json" />
  </packageSources>
</configuration>
```

**For .NET 7 builds**

```xml
<configuration>
  <packageSources>
    <add key="dotnet7" value="https://pkgs.dev.azure.com/dnceng/public/_packaging/dotnet7/nuget/v3/index.json" />
  </packageSources>
</configuration>
```

Do not directly edit the table below. Use https://github.com/dotnet/installer/tree/main/tools/sdk-readme-table-generator to help you generate it. Make sure to run the table generator test and make any changes to the generator along with your changes to the table. Daily servicing builds aren't shown here because they may contain upcoming security fixes. All public servicing builds can be downloaded at http://aka.ms/dotnet-download.

### Table
*Note* the 7.0.100 build will be finished internally. Below is the last public version available from that branch but is not fully updated with the final runtime.

| Platform | main<br>(8.0.x&nbsp;Runtime) | 8.0.1xx-preview3<br>(8.0-preview3&nbsp;Runtime) | Release/7.0.3xx<br>(7.0.x&nbsp;Runtime) |
| :--------- | :----------: | :----------: | :----------: |
| **Windows x64** | [![][win-x64-badge-main]][win-x64-version-main]<br>[Installer][win-x64-installer-main] - [Checksum][win-x64-installer-checksum-main]<br>[zip][win-x64-zip-main] - [Checksum][win-x64-zip-checksum-main] | [![][win-x64-badge-8.0.1XX-preview3]][win-x64-version-8.0.1XX-preview3]<br>[Installer][win-x64-installer-8.0.1XX-preview3] - [Checksum][win-x64-installer-checksum-8.0.1XX-preview3]<br>[zip][win-x64-zip-8.0.1XX-preview3] - [Checksum][win-x64-zip-checksum-8.0.1XX-preview3] | [![][win-x64-badge-7.0.3XX]][win-x64-version-7.0.3XX]<br>[Installer][win-x64-installer-7.0.3XX] - [Checksum][win-x64-installer-checksum-7.0.3XX]<br>[zip][win-x64-zip-7.0.3XX] - [Checksum][win-x64-zip-checksum-7.0.3XX] |
| **Windows x86** | [![][win-x86-badge-main]][win-x86-version-main]<br>[Installer][win-x86-installer-main] - [Checksum][win-x86-installer-checksum-main]<br>[zip][win-x86-zip-main] - [Checksum][win-x86-zip-checksum-main] | [![][win-x86-badge-8.0.1XX-preview3]][win-x86-version-8.0.1XX-preview3]<br>[Installer][win-x86-installer-8.0.1XX-preview3] - [Checksum][win-x86-installer-checksum-8.0.1XX-preview3]<br>[zip][win-x86-zip-8.0.1XX-preview3] - [Checksum][win-x86-zip-checksum-8.0.1XX-preview3] | [![][win-x86-badge-7.0.3XX]][win-x86-version-7.0.3XX]<br>[Installer][win-x86-installer-7.0.3XX] - [Checksum][win-x86-installer-checksum-7.0.3XX]<br>[zip][win-x86-zip-7.0.3XX] - [Checksum][win-x86-zip-checksum-7.0.3XX] |
| **Windows arm** | **N/A** | **N/A** | **N/A** |
| **Windows arm64** | [![][win-arm64-badge-main]][win-arm64-version-main]<br>[Installer][win-arm64-installer-main] - [Checksum][win-arm64-installer-checksum-main]<br>[zip][win-arm64-zip-main] | [![][win-arm64-badge-8.0.1XX-preview3]][win-arm64-version-8.0.1XX-preview3]<br>[Installer][win-arm64-installer-8.0.1XX-preview3] - [Checksum][win-arm64-installer-checksum-8.0.1XX-preview3]<br>[zip][win-arm64-zip-8.0.1XX-preview3] | [![][win-arm64-badge-7.0.3XX]][win-arm64-version-7.0.3XX]<br>[Installer][win-arm64-installer-7.0.3XX] - [Checksum][win-arm64-installer-checksum-7.0.3XX]<br>[zip][win-arm64-zip-7.0.3XX] |
| **macOS x64** | [![][osx-x64-badge-main]][osx-x64-version-main]<br>[Installer][osx-x64-installer-main] - [Checksum][osx-x64-installer-checksum-main]<br>[tar.gz][osx-x64-targz-main] - [Checksum][osx-x64-targz-checksum-main] | [![][osx-x64-badge-8.0.1XX-preview3]][osx-x64-version-8.0.1XX-preview3]<br>[Installer][osx-x64-installer-8.0.1XX-preview3] - [Checksum][osx-x64-installer-checksum-8.0.1XX-preview3]<br>[tar.gz][osx-x64-targz-8.0.1XX-preview3] - [Checksum][osx-x64-targz-checksum-8.0.1XX-preview3] | [![][osx-x64-badge-7.0.3XX]][osx-x64-version-7.0.3XX]<br>[Installer][osx-x64-installer-7.0.3XX] - [Checksum][osx-x64-installer-checksum-7.0.3XX]<br>[tar.gz][osx-x64-targz-7.0.3XX] - [Checksum][osx-x64-targz-checksum-7.0.3XX] |
| **macOS arm64** | [![][osx-arm64-badge-main]][osx-arm64-version-main]<br>[Installer][osx-arm64-installer-main] - [Checksum][osx-arm64-installer-checksum-main]<br>[tar.gz][osx-arm64-targz-main] - [Checksum][osx-arm64-targz-checksum-main] | [![][osx-arm64-badge-8.0.1XX-preview3]][osx-arm64-version-8.0.1XX-preview3]<br>[Installer][osx-arm64-installer-8.0.1XX-preview3] - [Checksum][osx-arm64-installer-checksum-8.0.1XX-preview3]<br>[tar.gz][osx-arm64-targz-8.0.1XX-preview3] - [Checksum][osx-arm64-targz-checksum-8.0.1XX-preview3] | [![][osx-arm64-badge-7.0.3XX]][osx-arm64-version-7.0.3XX]<br>[Installer][osx-arm64-installer-7.0.3XX] - [Checksum][osx-arm64-installer-checksum-7.0.3XX]<br>[tar.gz][osx-arm64-targz-7.0.3XX] - [Checksum][osx-arm64-targz-checksum-7.0.3XX] |
| **Linux x64** | [![][linux-badge-main]][linux-version-main]<br>[DEB Installer][linux-DEB-installer-main] - [Checksum][linux-DEB-installer-checksum-main]<br>[RPM Installer][linux-RPM-installer-main] - [Checksum][linux-RPM-installer-checksum-main]<br>_see installer note below_<sup>1</sup><br>[tar.gz][linux-targz-main] - [Checksum][linux-targz-checksum-main] | [![][linux-badge-8.0.1XX-preview3]][linux-version-8.0.1XX-preview3]<br>[DEB Installer][linux-DEB-installer-8.0.1XX-preview3] - [Checksum][linux-DEB-installer-checksum-8.0.1XX-preview3]<br>[RPM Installer][linux-RPM-installer-8.0.1XX-preview3] - [Checksum][linux-RPM-installer-checksum-8.0.1XX-preview3]<br>_see installer note below_<sup>1</sup><br>[tar.gz][linux-targz-8.0.1XX-preview3] - [Checksum][linux-targz-checksum-8.0.1XX-preview3] | [![][linux-badge-7.0.3XX]][linux-version-7.0.3XX]<br>[DEB Installer][linux-DEB-installer-7.0.3XX] - [Checksum][linux-DEB-installer-checksum-7.0.3XX]<br>[RPM Installer][linux-RPM-installer-7.0.3XX] - [Checksum][linux-RPM-installer-checksum-7.0.3XX]<br>_see installer note below_<sup>1</sup><br>[tar.gz][linux-targz-7.0.3XX] - [Checksum][linux-targz-checksum-7.0.3XX] |
| **Linux arm** | [![][linux-arm-badge-main]][linux-arm-version-main]<br>[tar.gz][linux-arm-targz-main] - [Checksum][linux-arm-targz-checksum-main] | [![][linux-arm-badge-8.0.1XX-preview3]][linux-arm-version-8.0.1XX-preview3]<br>[tar.gz][linux-arm-targz-8.0.1XX-preview3] - [Checksum][linux-arm-targz-checksum-8.0.1XX-preview3] | [![][linux-arm-badge-7.0.3XX]][linux-arm-version-7.0.3XX]<br>[tar.gz][linux-arm-targz-7.0.3XX] - [Checksum][linux-arm-targz-checksum-7.0.3XX] |
| **Linux arm64** | [![][linux-arm64-badge-main]][linux-arm64-version-main]<br>[tar.gz][linux-arm64-targz-main] - [Checksum][linux-arm64-targz-checksum-main] | [![][linux-arm64-badge-8.0.1XX-preview3]][linux-arm64-version-8.0.1XX-preview3]<br>[tar.gz][linux-arm64-targz-8.0.1XX-preview3] - [Checksum][linux-arm64-targz-checksum-8.0.1XX-preview3] | [![][linux-arm64-badge-7.0.3XX]][linux-arm64-version-7.0.3XX]<br>[tar.gz][linux-arm64-targz-7.0.3XX] - [Checksum][linux-arm64-targz-checksum-7.0.3XX] |
| **Linux-musl-x64** | [![][linux-musl-x64-badge-main]][linux-musl-x64-version-main]<br>[tar.gz][linux-musl-x64-targz-main] - [Checksum][linux-musl-x64-targz-checksum-main] | [![][linux-musl-x64-badge-8.0.1XX-preview3]][linux-musl-x64-version-8.0.1XX-preview3]<br>[tar.gz][linux-musl-x64-targz-8.0.1XX-preview3] - [Checksum][linux-musl-x64-targz-checksum-8.0.1XX-preview3] | [![][linux-musl-x64-badge-7.0.3XX]][linux-musl-x64-version-7.0.3XX]<br>[tar.gz][linux-musl-x64-targz-7.0.3XX] - [Checksum][linux-musl-x64-targz-checksum-7.0.3XX] |
| **Linux-musl-arm** | [![][linux-musl-arm-badge-main]][linux-musl-arm-version-main]<br>[tar.gz][linux-musl-arm-targz-main] - [Checksum][linux-musl-arm-targz-checksum-main] | [![][linux-musl-arm-badge-8.0.1XX-preview3]][linux-musl-arm-version-8.0.1XX-preview3]<br>[tar.gz][linux-musl-arm-targz-8.0.1XX-preview3] - [Checksum][linux-musl-arm-targz-checksum-8.0.1XX-preview3] | [![][linux-musl-arm-badge-7.0.3XX]][linux-musl-arm-version-7.0.3XX]<br>[tar.gz][linux-musl-arm-targz-7.0.3XX] - [Checksum][linux-musl-arm-targz-checksum-7.0.3XX] |
| **Linux-musl-arm64** | [![][linux-musl-arm64-badge-main]][linux-musl-arm64-version-main]<br>[tar.gz][linux-musl-arm64-targz-main] - [Checksum][linux-musl-arm64-targz-checksum-main] | [![][linux-musl-arm64-badge-8.0.1XX-preview3]][linux-musl-arm64-version-8.0.1XX-preview3]<br>[tar.gz][linux-musl-arm64-targz-8.0.1XX-preview3] - [Checksum][linux-musl-arm64-targz-checksum-8.0.1XX-preview3] | [![][linux-musl-arm64-badge-7.0.3XX]][linux-musl-arm64-version-7.0.3XX]<br>[tar.gz][linux-musl-arm64-targz-7.0.3XX] - [Checksum][linux-musl-arm64-targz-checksum-7.0.3XX] |
| **RHEL 6** | **N/A** | **N/A** | **N/A** |

Reference notes:
> **1**: Our Debian packages are put together slightly differently than the other OS specific installers. Instead of combining everything, we have separate component packages that depend on each other. If you're installing the SDK from the .deb file (via dpkg or similar), then you'll need to install the corresponding dependencies first:
> * [Host, Host FX Resolver, and Shared Framework](https://github.com/dotnet/runtime/blob/main/docs/project/dogfooding.md#nightly-builds-table)
> * [ASP.NET Core Shared Framework](https://github.com/aspnet/AspNetCore/blob/main/docs/DailyBuilds.md)

.NET Core SDK 2.x downloads can be found at [.NET Core SDK 2.x Installers and Binaries](Downloads2.x.md) but they are [out of support](https://dotnet.microsoft.com/platform/support/policy/dotnet-core).

[win-x64-badge-main]: https://aka.ms/dotnet/8.0.1xx/daily/win_x64_Release_version_badge.svg?no-cache
[win-x64-version-main]: https://aka.ms/dotnet/8.0.1xx/daily/productCommit-win-x64.txt
[win-x64-installer-main]: https://aka.ms/dotnet/8.0.1xx/daily/dotnet-sdk-win-x64.exe
[win-x64-installer-checksum-main]: https://aka.ms/dotnet/8.0.1xx/daily/dotnet-sdk-win-x64.exe.sha
[win-x64-zip-main]: https://aka.ms/dotnet/8.0.1xx/daily/dotnet-sdk-win-x64.zip
[win-x64-zip-checksum-main]: https://aka.ms/dotnet/8.0.1xx/daily/dotnet-sdk-win-x64.zip.sha

[win-x64-badge-8.0.1XX-preview3]: https://aka.ms/dotnet/8.0.1xx-preview3/daily/win_x64_Release_version_badge.svg?no-cache
[win-x64-version-8.0.1XX-preview3]: https://aka.ms/dotnet/8.0.1xx-preview3/daily/productCommit-win-x64.txt
[win-x64-installer-8.0.1XX-preview3]: https://aka.ms/dotnet/8.0.1xx-preview3/daily/dotnet-sdk-win-x64.exe
[win-x64-installer-checksum-8.0.1XX-preview3]: https://aka.ms/dotnet/8.0.1xx-preview3/daily/dotnet-sdk-win-x64.exe.sha
[win-x64-zip-8.0.1XX-preview3]: https://aka.ms/dotnet/8.0.1xx-preview3/daily/dotnet-sdk-win-x64.zip
[win-x64-zip-checksum-8.0.1XX-preview3]: https://aka.ms/dotnet/8.0.1xx-preview3/daily/dotnet-sdk-win-x64.zip.sha

[win-x64-badge-7.0.3XX]: https://aka.ms/dotnet/7.0.3xx/daily/win_x64_Release_version_badge.svg?no-cache
[win-x64-version-7.0.3XX]: https://aka.ms/dotnet/7.0.3xx/daily/productCommit-win-x64.txt
[win-x64-installer-7.0.3XX]: https://aka.ms/dotnet/7.0.3xx/daily/dotnet-sdk-win-x64.exe
[win-x64-installer-checksum-7.0.3XX]: https://aka.ms/dotnet/7.0.3xx/daily/dotnet-sdk-win-x64.exe.sha
[win-x64-zip-7.0.3XX]: https://aka.ms/dotnet/7.0.3xx/daily/dotnet-sdk-win-x64.zip
[win-x64-zip-checksum-7.0.3XX]: https://aka.ms/dotnet/7.0.3xx/daily/dotnet-sdk-win-x64.zip.sha

[win-x86-badge-main]: https://aka.ms/dotnet/8.0.1xx/daily/win_x86_Release_version_badge.svg?no-cache
[win-x86-version-main]: https://aka.ms/dotnet/8.0.1xx/daily/productCommit-win-x86.txt
[win-x86-installer-main]: https://aka.ms/dotnet/8.0.1xx/daily/dotnet-sdk-win-x86.exe
[win-x86-installer-checksum-main]: https://aka.ms/dotnet/8.0.1xx/daily/dotnet-sdk-win-x86.exe.sha
[win-x86-zip-main]: https://aka.ms/dotnet/8.0.1xx/daily/dotnet-sdk-win-x86.zip
[win-x86-zip-checksum-main]: https://aka.ms/dotnet/8.0.1xx/daily/dotnet-sdk-win-x86.zip.sha

[win-x86-badge-8.0.1XX-preview3]: https://aka.ms/dotnet/8.0.1xx-preview3/daily/win_x86_Release_version_badge.svg?no-cache
[win-x86-version-8.0.1XX-preview3]: https://aka.ms/dotnet/8.0.1xx-preview3/daily/productCommit-win-x86.txt
[win-x86-installer-8.0.1XX-preview3]: https://aka.ms/dotnet/8.0.1xx-preview3/daily/dotnet-sdk-win-x86.exe
[win-x86-installer-checksum-8.0.1XX-preview3]: https://aka.ms/dotnet/8.0.1xx-preview3/daily/dotnet-sdk-win-x86.exe.sha
[win-x86-zip-8.0.1XX-preview3]: https://aka.ms/dotnet/8.0.1xx-preview3/daily/dotnet-sdk-win-x86.zip
[win-x86-zip-checksum-8.0.1XX-preview3]: https://aka.ms/dotnet/8.0.1xx-preview3/daily/dotnet-sdk-win-x86.zip.sha

[win-x86-badge-7.0.3XX]: https://aka.ms/dotnet/7.0.3xx/daily/win_x86_Release_version_badge.svg?no-cache
[win-x86-version-7.0.3XX]: https://aka.ms/dotnet/7.0.3xx/daily/productCommit-win-x86.txt
[win-x86-installer-7.0.3XX]: https://aka.ms/dotnet/7.0.3xx/daily/dotnet-sdk-win-x86.exe
[win-x86-installer-checksum-7.0.3XX]: https://aka.ms/dotnet/7.0.3xx/daily/dotnet-sdk-win-x86.exe.sha
[win-x86-zip-7.0.3XX]: https://aka.ms/dotnet/7.0.3xx/daily/dotnet-sdk-win-x86.zip
[win-x86-zip-checksum-7.0.3XX]: https://aka.ms/dotnet/7.0.3xx/daily/dotnet-sdk-win-x86.zip.sha

[osx-x64-badge-main]: https://aka.ms/dotnet/8.0.1xx/daily/osx_x64_Release_version_badge.svg?no-cache
[osx-x64-version-main]: https://aka.ms/dotnet/8.0.1xx/daily/productCommit-osx-x64.txt
[osx-x64-installer-main]: https://aka.ms/dotnet/8.0.1xx/daily/dotnet-sdk-osx-x64.pkg
[osx-x64-installer-checksum-main]: https://aka.ms/dotnet/8.0.1xx/daily/dotnet-sdk-osx-x64.pkg.sha
[osx-x64-targz-main]: https://aka.ms/dotnet/8.0.1xx/daily/dotnet-sdk-osx-x64.tar.gz
[osx-x64-targz-checksum-main]: https://aka.ms/dotnet/8.0.1xx/daily/dotnet-sdk-osx-x64.pkg.tar.gz.sha

[osx-x64-badge-8.0.1XX-preview3]: https://aka.ms/dotnet/8.0.1xx-preview3/daily/osx_x64_Release_version_badge.svg?no-cache
[osx-x64-version-8.0.1XX-preview3]: https://aka.ms/dotnet/8.0.1xx-preview3/daily/productCommit-osx-x64.txt
[osx-x64-installer-8.0.1XX-preview3]: https://aka.ms/dotnet/8.0.1xx-preview3/daily/dotnet-sdk-osx-x64.pkg
[osx-x64-installer-checksum-8.0.1XX-preview3]: https://aka.ms/dotnet/8.0.1xx-preview3/daily/dotnet-sdk-osx-x64.pkg.sha
[osx-x64-targz-8.0.1XX-preview3]: https://aka.ms/dotnet/8.0.1xx-preview3/daily/dotnet-sdk-osx-x64.tar.gz
[osx-x64-targz-checksum-8.0.1XX-preview3]: https://aka.ms/dotnet/8.0.1xx-preview3/daily/dotnet-sdk-osx-x64.pkg.tar.gz.sha

[osx-x64-badge-7.0.3XX]: https://aka.ms/dotnet/7.0.3xx/daily/osx_x64_Release_version_badge.svg?no-cache
[osx-x64-version-7.0.3XX]: https://aka.ms/dotnet/7.0.3xx/daily/productCommit-osx-x64.txt
[osx-x64-installer-7.0.3XX]: https://aka.ms/dotnet/7.0.3xx/daily/dotnet-sdk-osx-x64.pkg
[osx-x64-installer-checksum-7.0.3XX]: https://aka.ms/dotnet/7.0.3xx/daily/dotnet-sdk-osx-x64.pkg.sha
[osx-x64-targz-7.0.3XX]: https://aka.ms/dotnet/7.0.3xx/daily/dotnet-sdk-osx-x64.tar.gz
[osx-x64-targz-checksum-7.0.3XX]: https://aka.ms/dotnet/7.0.3xx/daily/dotnet-sdk-osx-x64.pkg.tar.gz.sha

[osx-arm64-badge-main]: https://aka.ms/dotnet/8.0.1xx/daily/osx_arm64_Release_version_badge.svg?no-cache
[osx-arm64-version-main]: https://aka.ms/dotnet/8.0.1xx/daily/productCommit-osx-arm64.txt
[osx-arm64-installer-main]: https://aka.ms/dotnet/8.0.1xx/daily/dotnet-sdk-osx-arm64.pkg
[osx-arm64-installer-checksum-main]: https://aka.ms/dotnet/8.0.1xx/daily/dotnet-sdk-osx-arm64.pkg.sha
[osx-arm64-targz-main]: https://aka.ms/dotnet/8.0.1xx/daily/dotnet-sdk-osx-arm64.tar.gz
[osx-arm64-targz-checksum-main]: https://aka.ms/dotnet/8.0.1xx/daily/dotnet-sdk-osx-arm64.pkg.tar.gz.sha

[osx-arm64-badge-8.0.1XX-preview3]: https://aka.ms/dotnet/8.0.1xx-preview3/daily/osx_arm64_Release_version_badge.svg?no-cache
[osx-arm64-version-8.0.1XX-preview3]: https://aka.ms/dotnet/8.0.1xx-preview3/daily/productCommit-osx-arm64.txt
[osx-arm64-installer-8.0.1XX-preview3]: https://aka.ms/dotnet/8.0.1xx-preview3/daily/dotnet-sdk-osx-arm64.pkg
[osx-arm64-installer-checksum-8.0.1XX-preview3]: https://aka.ms/dotnet/8.0.1xx-preview3/daily/dotnet-sdk-osx-arm64.pkg.sha
[osx-arm64-targz-8.0.1XX-preview3]: https://aka.ms/dotnet/8.0.1xx-preview3/daily/dotnet-sdk-osx-arm64.tar.gz
[osx-arm64-targz-checksum-8.0.1XX-preview3]: https://aka.ms/dotnet/8.0.1xx-preview3/daily/dotnet-sdk-osx-arm64.pkg.tar.gz.sha

[osx-arm64-badge-7.0.3XX]: https://aka.ms/dotnet/7.0.3xx/daily/osx_arm64_Release_version_badge.svg?no-cache
[osx-arm64-version-7.0.3XX]: https://aka.ms/dotnet/7.0.3xx/daily/productCommit-osx-arm64.txt
[osx-arm64-installer-7.0.3XX]: https://aka.ms/dotnet/7.0.3xx/daily/dotnet-sdk-osx-arm64.pkg
[osx-arm64-installer-checksum-7.0.3XX]: https://aka.ms/dotnet/7.0.3xx/daily/dotnet-sdk-osx-arm64.pkg.sha
[osx-arm64-targz-7.0.3XX]: https://aka.ms/dotnet/7.0.3xx/daily/dotnet-sdk-osx-arm64.tar.gz
[osx-arm64-targz-checksum-7.0.3XX]: https://aka.ms/dotnet/7.0.3xx/daily/dotnet-sdk-osx-arm64.pkg.tar.gz.sha

[linux-badge-main]: https://aka.ms/dotnet/8.0.1xx/daily/linux_x64_Release_version_badge.svg?no-cache
[linux-version-main]: https://aka.ms/dotnet/8.0.1xx/daily/productCommit-linux-x64.txt
[linux-DEB-installer-main]: https://aka.ms/dotnet/8.0.1xx/daily/dotnet-sdk-x64.deb
[linux-DEB-installer-checksum-main]: https://aka.ms/dotnet/8.0.1xx/daily/dotnet-sdk-x64.deb.sha
[linux-RPM-installer-main]: https://aka.ms/dotnet/8.0.1xx/daily/dotnet-sdk-x64.rpm
[linux-RPM-installer-checksum-main]: https://aka.ms/dotnet/8.0.1xx/daily/dotnet-sdk-x64.rpm.sha
[linux-targz-main]: https://aka.ms/dotnet/8.0.1xx/daily/dotnet-sdk-linux-x64.tar.gz
[linux-targz-checksum-main]: https://aka.ms/dotnet/8.0.1xx/daily/dotnet-sdk-linux-x64.tar.gz.sha

[linux-badge-8.0.1XX-preview3]: https://aka.ms/dotnet/8.0.1xx-preview3/daily/linux_x64_Release_version_badge.svg?no-cache
[linux-version-8.0.1XX-preview3]: https://aka.ms/dotnet/8.0.1xx-preview3/daily/productCommit-linux-x64.txt
[linux-DEB-installer-8.0.1XX-preview3]: https://aka.ms/dotnet/8.0.1xx-preview3/daily/dotnet-sdk-x64.deb
[linux-DEB-installer-checksum-8.0.1XX-preview3]: https://aka.ms/dotnet/8.0.1xx-preview3/daily/dotnet-sdk-x64.deb.sha
[linux-RPM-installer-8.0.1XX-preview3]: https://aka.ms/dotnet/8.0.1xx-preview3/daily/dotnet-sdk-x64.rpm
[linux-RPM-installer-checksum-8.0.1XX-preview3]: https://aka.ms/dotnet/8.0.1xx-preview3/daily/dotnet-sdk-x64.rpm.sha
[linux-targz-8.0.1XX-preview3]: https://aka.ms/dotnet/8.0.1xx-preview3/daily/dotnet-sdk-linux-x64.tar.gz
[linux-targz-checksum-8.0.1XX-preview3]: https://aka.ms/dotnet/8.0.1xx-preview3/daily/dotnet-sdk-linux-x64.tar.gz.sha

[linux-badge-7.0.3XX]: https://aka.ms/dotnet/7.0.3xx/daily/linux_x64_Release_version_badge.svg?no-cache
[linux-version-7.0.3XX]: https://aka.ms/dotnet/7.0.3xx/daily/productCommit-linux-x64.txt
[linux-DEB-installer-7.0.3XX]: https://aka.ms/dotnet/7.0.3xx/daily/dotnet-sdk-x64.deb
[linux-DEB-installer-checksum-7.0.3XX]: https://aka.ms/dotnet/7.0.3xx/daily/dotnet-sdk-x64.deb.sha
[linux-RPM-installer-7.0.3XX]: https://aka.ms/dotnet/7.0.3xx/daily/dotnet-sdk-x64.rpm
[linux-RPM-installer-checksum-7.0.3XX]: https://aka.ms/dotnet/7.0.3xx/daily/dotnet-sdk-x64.rpm.sha
[linux-targz-7.0.3XX]: https://aka.ms/dotnet/7.0.3xx/daily/dotnet-sdk-linux-x64.tar.gz
[linux-targz-checksum-7.0.3XX]: https://aka.ms/dotnet/7.0.3xx/daily/dotnet-sdk-linux-x64.tar.gz.sha

[linux-arm-badge-main]: https://aka.ms/dotnet/8.0.1xx/daily/linux_arm_Release_version_badge.svg?no-cache
[linux-arm-version-main]: https://aka.ms/dotnet/8.0.1xx/daily/productCommit-linux-arm.txt
[linux-arm-targz-main]: https://aka.ms/dotnet/8.0.1xx/daily/dotnet-sdk-linux-arm.tar.gz
[linux-arm-targz-checksum-main]: https://aka.ms/dotnet/8.0.1xx/daily/dotnet-sdk-linux-arm.tar.gz.sha

[linux-arm-badge-8.0.1XX-preview3]: https://aka.ms/dotnet/8.0.1xx-preview3/daily/linux_arm_Release_version_badge.svg?no-cache
[linux-arm-version-8.0.1XX-preview3]: https://aka.ms/dotnet/8.0.1xx-preview3/daily/productCommit-linux-arm.txt
[linux-arm-targz-8.0.1XX-preview3]: https://aka.ms/dotnet/8.0.1xx-preview3/daily/dotnet-sdk-linux-arm.tar.gz
[linux-arm-targz-checksum-8.0.1XX-preview3]: https://aka.ms/dotnet/8.0.1xx-preview3/daily/dotnet-sdk-linux-arm.tar.gz.sha

[linux-arm-badge-7.0.3XX]: https://aka.ms/dotnet/7.0.3xx/daily/linux_arm_Release_version_badge.svg?no-cache
[linux-arm-version-7.0.3XX]: https://aka.ms/dotnet/7.0.3xx/daily/productCommit-linux-arm.txt
[linux-arm-targz-7.0.3XX]: https://aka.ms/dotnet/7.0.3xx/daily/dotnet-sdk-linux-arm.tar.gz
[linux-arm-targz-checksum-7.0.3XX]: https://aka.ms/dotnet/7.0.3xx/daily/dotnet-sdk-linux-arm.tar.gz.sha

[linux-arm64-badge-main]: https://aka.ms/dotnet/8.0.1xx/daily/linux_arm64_Release_version_badge.svg?no-cache
[linux-arm64-version-main]: https://aka.ms/dotnet/8.0.1xx/daily/productCommit-linux-arm64.txt
[linux-arm64-targz-main]: https://aka.ms/dotnet/8.0.1xx/daily/dotnet-sdk-linux-arm64.tar.gz
[linux-arm64-targz-checksum-main]: https://aka.ms/dotnet/8.0.1xx/daily/dotnet-sdk-linux-arm64.tar.gz.sha

[linux-arm64-badge-8.0.1XX-preview3]: https://aka.ms/dotnet/8.0.1xx-preview3/daily/linux_arm64_Release_version_badge.svg?no-cache
[linux-arm64-version-8.0.1XX-preview3]: https://aka.ms/dotnet/8.0.1xx-preview3/daily/productCommit-linux-arm64.txt
[linux-arm64-targz-8.0.1XX-preview3]: https://aka.ms/dotnet/8.0.1xx-preview3/daily/dotnet-sdk-linux-arm64.tar.gz
[linux-arm64-targz-checksum-8.0.1XX-preview3]: https://aka.ms/dotnet/8.0.1xx-preview3/daily/dotnet-sdk-linux-arm64.tar.gz.sha

[linux-arm64-badge-7.0.3XX]: https://aka.ms/dotnet/7.0.3xx/daily/linux_arm64_Release_version_badge.svg?no-cache
[linux-arm64-version-7.0.3XX]: https://aka.ms/dotnet/7.0.3xx/daily/productCommit-linux-arm64.txt
[linux-arm64-targz-7.0.3XX]: https://aka.ms/dotnet/7.0.3xx/daily/dotnet-sdk-linux-arm64.tar.gz
[linux-arm64-targz-checksum-7.0.3XX]: https://aka.ms/dotnet/7.0.3xx/daily/dotnet-sdk-linux-arm64.tar.gz.sha

[rhel-6-badge-main]: https://aka.ms/dotnet/8.0.1xx/daily/rhel.6_x64_Release_version_badge.svg?no-cache
[rhel-6-version-main]: https://aka.ms/dotnet/8.0.1xx/daily/productCommit-rhel.6-x64.txt
[rhel-6-targz-main]: https://aka.ms/dotnet/8.0.1xx/daily/dotnet-sdk-rhel.6-x64.tar.gz
[rhel-6-targz-checksum-main]: https://aka.ms/dotnet/8.0.1xx/daily/dotnet-sdk-rhel.6-x64.tar.gz.sha

[rhel-6-badge-8.0.1XX-preview3]: https://aka.ms/dotnet/8.0.1xx-preview3/daily/rhel.6_x64_Release_version_badge.svg?no-cache
[rhel-6-version-8.0.1XX-preview3]: https://aka.ms/dotnet/8.0.1xx-preview3/daily/productCommit-rhel.6-x64.txt
[rhel-6-targz-8.0.1XX-preview3]: https://aka.ms/dotnet/8.0.1xx-preview3/daily/dotnet-sdk-rhel.6-x64.tar.gz
[rhel-6-targz-checksum-8.0.1XX-preview3]: https://aka.ms/dotnet/8.0.1xx-preview3/daily/dotnet-sdk-rhel.6-x64.tar.gz.sha

[rhel-6-badge-7.0.3XX]: https://aka.ms/dotnet/7.0.3xx/daily/rhel.6_x64_Release_version_badge.svg?no-cache
[rhel-6-version-7.0.3XX]: https://aka.ms/dotnet/7.0.3xx/daily/productCommit-rhel.6-x64.txt
[rhel-6-targz-7.0.3XX]: https://aka.ms/dotnet/7.0.3xx/daily/dotnet-sdk-rhel.6-x64.tar.gz
[rhel-6-targz-checksum-7.0.3XX]: https://aka.ms/dotnet/7.0.3xx/daily/dotnet-sdk-rhel.6-x64.tar.gz.sha

[linux-musl-x64-badge-main]: https://aka.ms/dotnet/8.0.1xx/daily/linux_musl_x64_Release_version_badge.svg?no-cache
[linux-musl-x64-version-main]: https://aka.ms/dotnet/8.0.1xx/daily/productCommit-linux-musl-x64.txt
[linux-musl-x64-targz-main]: https://aka.ms/dotnet/8.0.1xx/daily/dotnet-sdk-linux-musl-x64.tar.gz
[linux-musl-x64-targz-checksum-main]: https://aka.ms/dotnet/8.0.1xx/daily/dotnet-sdk-linux-musl-x64.tar.gz.sha

[linux-musl-x64-badge-8.0.1XX-preview3]: https://aka.ms/dotnet/8.0.1xx-preview3/daily/linux_musl_x64_Release_version_badge.svg?no-cache
[linux-musl-x64-version-8.0.1XX-preview3]: https://aka.ms/dotnet/8.0.1xx-preview3/daily/productCommit-linux-musl-x64.txt
[linux-musl-x64-targz-8.0.1XX-preview3]: https://aka.ms/dotnet/8.0.1xx-preview3/daily/dotnet-sdk-linux-musl-x64.tar.gz
[linux-musl-x64-targz-checksum-8.0.1XX-preview3]: https://aka.ms/dotnet/8.0.1xx-preview3/daily/dotnet-sdk-linux-musl-x64.tar.gz.sha

[linux-musl-x64-badge-7.0.3XX]: https://aka.ms/dotnet/7.0.3xx/daily/linux_musl_x64_Release_version_badge.svg?no-cache
[linux-musl-x64-version-7.0.3XX]: https://aka.ms/dotnet/7.0.3xx/daily/productCommit-linux-musl-x64.txt
[linux-musl-x64-targz-7.0.3XX]: https://aka.ms/dotnet/7.0.3xx/daily/dotnet-sdk-linux-musl-x64.tar.gz
[linux-musl-x64-targz-checksum-7.0.3XX]: https://aka.ms/dotnet/7.0.3xx/daily/dotnet-sdk-linux-musl-x64.tar.gz.sha

[linux-musl-arm-badge-main]: https://aka.ms/dotnet/8.0.1xx/daily/linux_musl_arm_Release_version_badge.svg?no-cache
[linux-musl-arm-version-main]: https://aka.ms/dotnet/8.0.1xx/daily/productCommit-linux-musl-arm.txt
[linux-musl-arm-targz-main]: https://aka.ms/dotnet/8.0.1xx/daily/dotnet-sdk-linux-musl-arm.tar.gz
[linux-musl-arm-targz-checksum-main]: https://aka.ms/dotnet/8.0.1xx/daily/dotnet-sdk-linux-musl-arm.tar.gz.sha

[linux-musl-arm-badge-8.0.1XX-preview3]: https://aka.ms/dotnet/8.0.1xx-preview3/daily/linux_musl_arm_Release_version_badge.svg?no-cache
[linux-musl-arm-version-8.0.1XX-preview3]: https://aka.ms/dotnet/8.0.1xx-preview3/daily/productCommit-linux-musl-arm.txt
[linux-musl-arm-targz-8.0.1XX-preview3]: https://aka.ms/dotnet/8.0.1xx-preview3/daily/dotnet-sdk-linux-musl-arm.tar.gz
[linux-musl-arm-targz-checksum-8.0.1XX-preview3]: https://aka.ms/dotnet/8.0.1xx-preview3/daily/dotnet-sdk-linux-musl-arm.tar.gz.sha

[linux-musl-arm-badge-7.0.3XX]: https://aka.ms/dotnet/7.0.3xx/daily/linux_musl_arm_Release_version_badge.svg?no-cache
[linux-musl-arm-version-7.0.3XX]: https://aka.ms/dotnet/7.0.3xx/daily/productCommit-linux-musl-arm.txt
[linux-musl-arm-targz-7.0.3XX]: https://aka.ms/dotnet/7.0.3xx/daily/dotnet-sdk-linux-musl-arm.tar.gz
[linux-musl-arm-targz-checksum-7.0.3XX]: https://aka.ms/dotnet/7.0.3xx/daily/dotnet-sdk-linux-musl-arm.tar.gz.sha

[linux-musl-arm64-badge-main]: https://aka.ms/dotnet/8.0.1xx/daily/linux_musl_arm64_Release_version_badge.svg?no-cache
[linux-musl-arm64-version-main]: https://aka.ms/dotnet/8.0.1xx/daily/productCommit-linux-musl-arm64.txt
[linux-musl-arm64-targz-main]: https://aka.ms/dotnet/8.0.1xx/daily/dotnet-sdk-linux-musl-arm64.tar.gz
[linux-musl-arm64-targz-checksum-main]: https://aka.ms/dotnet/8.0.1xx/daily/dotnet-sdk-linux-musl-arm64.tar.gz.sha

[linux-musl-arm64-badge-8.0.1XX-preview3]: https://aka.ms/dotnet/8.0.1xx-preview3/daily/linux_musl_arm64_Release_version_badge.svg?no-cache
[linux-musl-arm64-version-8.0.1XX-preview3]: https://aka.ms/dotnet/8.0.1xx-preview3/daily/productCommit-linux-musl-arm64.txt
[linux-musl-arm64-targz-8.0.1XX-preview3]: https://aka.ms/dotnet/8.0.1xx-preview3/daily/dotnet-sdk-linux-musl-arm64.tar.gz
[linux-musl-arm64-targz-checksum-8.0.1XX-preview3]: https://aka.ms/dotnet/8.0.1xx-preview3/daily/dotnet-sdk-linux-musl-arm64.tar.gz.sha

[linux-musl-arm64-badge-7.0.3XX]: https://aka.ms/dotnet/7.0.3xx/daily/linux_musl_arm64_Release_version_badge.svg?no-cache
[linux-musl-arm64-version-7.0.3XX]: https://aka.ms/dotnet/7.0.3xx/daily/productCommit-linux-musl-arm64.txt
[linux-musl-arm64-targz-7.0.3XX]: https://aka.ms/dotnet/7.0.3xx/daily/dotnet-sdk-linux-musl-arm64.tar.gz
[linux-musl-arm64-targz-checksum-7.0.3XX]: https://aka.ms/dotnet/7.0.3xx/daily/dotnet-sdk-linux-musl-arm64.tar.gz.sha

[win-arm-badge-main]: https://aka.ms/dotnet/8.0.1xx/daily/win_arm_Release_version_badge.svg?no-cache
[win-arm-version-main]: https://aka.ms/dotnet/8.0.1xx/daily/productCommit-win-arm.txt
[win-arm-zip-main]: https://aka.ms/dotnet/8.0.1xx/daily/dotnet-sdk-win-arm.zip
[win-arm-zip-checksum-main]: https://aka.ms/dotnet/8.0.1xx/daily/dotnet-sdk-win-arm.zip.sha

[win-arm-badge-8.0.1XX-preview3]: https://aka.ms/dotnet/8.0.1xx-preview3/daily/win_arm_Release_version_badge.svg?no-cache
[win-arm-version-8.0.1XX-preview3]: https://aka.ms/dotnet/8.0.1xx-preview3/daily/productCommit-win-arm.txt
[win-arm-zip-8.0.1XX-preview3]: https://aka.ms/dotnet/8.0.1xx-preview3/daily/dotnet-sdk-win-arm.zip
[win-arm-zip-checksum-8.0.1XX-preview3]: https://aka.ms/dotnet/8.0.1xx-preview3/daily/dotnet-sdk-win-arm.zip.sha

[win-arm-badge-7.0.3XX]: https://aka.ms/dotnet/7.0.3xx/daily/win_arm_Release_version_badge.svg?no-cache
[win-arm-version-7.0.3XX]: https://aka.ms/dotnet/7.0.3xx/daily/productCommit-win-arm.txt
[win-arm-zip-7.0.3XX]: https://aka.ms/dotnet/7.0.3xx/daily/dotnet-sdk-win-arm.zip
[win-arm-zip-checksum-7.0.3XX]: https://aka.ms/dotnet/7.0.3xx/daily/dotnet-sdk-win-arm.zip.sha

[win-arm64-badge-main]: https://aka.ms/dotnet/8.0.1xx/daily/win_arm64_Release_version_badge.svg?no-cache
[win-arm64-version-main]: https://aka.ms/dotnet/8.0.1xx/daily/productCommit-win-arm64.txt
[win-arm64-installer-main]: https://aka.ms/dotnet/8.0.1xx/daily/dotnet-sdk-win-arm64.exe
[win-arm64-installer-checksum-main]: https://aka.ms/dotnet/8.0.1xx/daily/dotnet-sdk-win-arm64.exe.sha
[win-arm64-zip-main]: https://aka.ms/dotnet/8.0.1xx/daily/dotnet-sdk-win-arm64.zip
[win-arm64-zip-checksum-main]: https://aka.ms/dotnet/8.0.1xx/daily/dotnet-sdk-win-arm64.zip.sha

[win-arm64-badge-8.0.1XX-preview3]: https://aka.ms/dotnet/8.0.1xx-preview3/daily/win_arm64_Release_version_badge.svg?no-cache
[win-arm64-version-8.0.1XX-preview3]: https://aka.ms/dotnet/8.0.1xx-preview3/daily/productCommit-win-arm64.txt
[win-arm64-installer-8.0.1XX-preview3]: https://aka.ms/dotnet/8.0.1xx-preview3/daily/dotnet-sdk-win-arm64.exe
[win-arm64-installer-checksum-8.0.1XX-preview3]: https://aka.ms/dotnet/8.0.1xx-preview3/daily/dotnet-sdk-win-arm64.exe.sha
[win-arm64-zip-8.0.1XX-preview3]: https://aka.ms/dotnet/8.0.1xx-preview3/daily/dotnet-sdk-win-arm64.zip
[win-arm64-zip-checksum-8.0.1XX-preview3]: https://aka.ms/dotnet/8.0.1xx-preview3/daily/dotnet-sdk-win-arm64.zip.sha

[win-arm64-badge-7.0.3XX]: https://aka.ms/dotnet/7.0.3xx/daily/win_arm64_Release_version_badge.svg?no-cache
[win-arm64-version-7.0.3XX]: https://aka.ms/dotnet/7.0.3xx/daily/productCommit-win-arm64.txt
[win-arm64-installer-7.0.3XX]: https://aka.ms/dotnet/7.0.3xx/daily/dotnet-sdk-win-arm64.exe
[win-arm64-installer-checksum-7.0.3XX]: https://aka.ms/dotnet/7.0.3xx/daily/dotnet-sdk-win-arm64.exe.sha
[win-arm64-zip-7.0.3XX]: https://aka.ms/dotnet/7.0.3xx/daily/dotnet-sdk-win-arm64.zip
[win-arm64-zip-checksum-7.0.3XX]: https://aka.ms/dotnet/7.0.3xx/daily/dotnet-sdk-win-arm64.zip.sha

[sdk-shas-2.2.1XX]: https://github.com/dotnet/versions/tree/master/build-info/dotnet/product/cli/release/2.2#built-repositories

Looking for dotnet-install sources?
-----------------------------------

Sources for dotnet-install.sh and dotnet-install.ps1 are in the [install-scripts repo](https://github.com/dotnet/install-scripts).

Questions & Comments
--------------------

For all feedback, use the Issues on the [.NET SDK](https://github.com/dotnet/sdk) repository.

License
-------

The .NET project uses the [MIT license](LICENSE).

The LICENSE and ThirdPartyNotices in any downloaded archives are authoritative.
