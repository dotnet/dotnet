# .NET Core SDK

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


This repo contains the source code for the cross-platform [.NET Core](http://github.com/dotnet/core) SDK. It aggregates the .NET Toolchain, the .NET Core runtime, the templates, and the .NET Core Windows Desktop runtime. It produces zip, tarballs, and native packages for various supported platforms.

Looking for released versions of the .NET Core tooling?
----------------------------------------

Download released versions of the .NET Core tools (CLI, MSBuild and the new csproj) at https://dot.net/core.

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

.NET Source-Build is supported on the oldest available .NET SDK feature update, and on Linux only.
For example, if both .NET 6.0.1XX and 6.0.2XX feature updates are available from [dotnet.microsoft.com](https://dotnet.microsoft.com/en-us/download/dotnet/6.0), Source-Build will only support 6.0.1XX.
For the latest information about Source-Build support for new .NET versions, please check our [GitHub Discussions page](https://github.com/dotnet/source-build/discussions) for announcements.

## Prerequisites

The dependencies for building .NET from source can be found [here](https://github.com/dotnet/runtime/blob/main/docs/workflow/requirements/linux-requirements.md).

## Building

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
    
    On arm64, please use `./prep.sh --bootstrap` instead.

3. Build the .NET SDK

    ```bash
    ./build.sh --clean-while-building
    ```

    This builds the entire .NET SDK from source.
    The resulting SDK is placed at `artifacts/x64/Release/dotnet-sdk-6.0.100-fedora.33-x64.tar.gz`.

    Optionally add the `--online` flag to add online NuGet restore sources to the build.
    This is useful for testing unsupported releases that don't yet build without downloading pre-built binaries from the internet.

    Run `./build.sh --help` to see more information about supported build options.

4. (Optional) Unpack and install the .NET SDK

    ```bash
    mkdir -p $HOME/dotnet
    tar zxf artifacts/x64/Release/dotnet-sdk-6.0.100-fedora.33-x64.tar.gz -C $HOME/dotnet
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

You can download the .NET Core SDK as either an installer (MSI, PKG) or a zip (zip, tar.gz). The .NET Core SDK contains both the .NET Core runtime and CLI tools.

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

Please do not directly edit the table below. Use https://github.com/dotnet/installer/tree/main/tools/sdk-readme-table-generator to help you generate it. Make sure to run the table generator test and make any changes to the generator along with your changes to the table. Daily servicing builds have been removed as all servicing is done in private repos to avoid disclosure of critical security fixes. All public servicing builds can be downloaded at http://aka.ms/dotnet-download.

### Table
*Note* the 7.0.100 build will be finished internally. Below is the last public version available from that branch but is not fully updated with the final runtime.

| Platform | main<br>(8.0.x&nbsp;Runtime) | Release/7.0.2xx<br>(7.0.x&nbsp;Runtime) | Release/7.0.1xx<br>(7.0.x&nbsp;Runtime) |
| :--------- | :----------: | :----------: | :----------: |
| **Windows x64** | [![][win-x64-badge-main]][win-x64-version-main]<br>[Installer][win-x64-installer-main] - [Checksum][win-x64-installer-checksum-main]<br>[zip][win-x64-zip-main] - [Checksum][win-x64-zip-checksum-main] | [![][win-x64-badge-7.0.2XX]][win-x64-version-7.0.2XX]<br>[Installer][win-x64-installer-7.0.2XX] - [Checksum][win-x64-installer-checksum-7.0.2XX]<br>[zip][win-x64-zip-7.0.2XX] - [Checksum][win-x64-zip-checksum-7.0.2XX] | [![][win-x64-badge-7.0.1XX]][win-x64-version-7.0.1XX]<br>[Installer][win-x64-installer-7.0.1XX] - [Checksum][win-x64-installer-checksum-7.0.1XX]<br>[zip][win-x64-zip-7.0.1XX] - [Checksum][win-x64-zip-checksum-7.0.1XX] |
| **Windows x86** | [![][win-x86-badge-main]][win-x86-version-main]<br>[Installer][win-x86-installer-main] - [Checksum][win-x86-installer-checksum-main]<br>[zip][win-x86-zip-main] - [Checksum][win-x86-zip-checksum-main] | [![][win-x86-badge-7.0.2XX]][win-x86-version-7.0.2XX]<br>[Installer][win-x86-installer-7.0.2XX] - [Checksum][win-x86-installer-checksum-7.0.2XX]<br>[zip][win-x86-zip-7.0.2XX] - [Checksum][win-x86-zip-checksum-7.0.2XX] | [![][win-x86-badge-7.0.1XX]][win-x86-version-7.0.1XX]<br>[Installer][win-x86-installer-7.0.1XX] - [Checksum][win-x86-installer-checksum-7.0.1XX]<br>[zip][win-x86-zip-7.0.1XX] - [Checksum][win-x86-zip-checksum-7.0.1XX] |
| **Windows arm** | **N/A** | **N/A** | **N/A** |
| **Windows arm64** | [![][win-arm64-badge-main]][win-arm64-version-main]<br>[Installer][win-arm64-installer-main] - [Checksum][win-arm64-installer-checksum-main]<br>[zip][win-arm64-zip-main] | [![][win-arm64-badge-7.0.2XX]][win-arm64-version-7.0.2XX]<br>[Installer][win-arm64-installer-7.0.2XX] - [Checksum][win-arm64-installer-checksum-7.0.2XX]<br>[zip][win-arm64-zip-7.0.2XX] | [![][win-arm64-badge-7.0.1XX]][win-arm64-version-7.0.1XX]<br>[Installer][win-arm64-installer-7.0.1XX] - [Checksum][win-arm64-installer-checksum-7.0.1XX]<br>[zip][win-arm64-zip-7.0.1XX] |
| **macOS x64** | [![][osx-x64-badge-main]][osx-x64-version-main]<br>[Installer][osx-x64-installer-main] - [Checksum][osx-x64-installer-checksum-main]<br>[tar.gz][osx-x64-targz-main] - [Checksum][osx-x64-targz-checksum-main] | [![][osx-x64-badge-7.0.2XX]][osx-x64-version-7.0.2XX]<br>[Installer][osx-x64-installer-7.0.2XX] - [Checksum][osx-x64-installer-checksum-7.0.2XX]<br>[tar.gz][osx-x64-targz-7.0.2XX] - [Checksum][osx-x64-targz-checksum-7.0.2XX] | [![][osx-x64-badge-7.0.1XX]][osx-x64-version-7.0.1XX]<br>[Installer][osx-x64-installer-7.0.1XX] - [Checksum][osx-x64-installer-checksum-7.0.1XX]<br>[tar.gz][osx-x64-targz-7.0.1XX] - [Checksum][osx-x64-targz-checksum-7.0.1XX] |
| **macOS arm64** | [![][osx-arm64-badge-main]][osx-arm64-version-main]<br>[Installer][osx-arm64-installer-main] - [Checksum][osx-arm64-installer-checksum-main]<br>[tar.gz][osx-arm64-targz-main] - [Checksum][osx-arm64-targz-checksum-main] | [![][osx-arm64-badge-7.0.2XX]][osx-arm64-version-7.0.2XX]<br>[Installer][osx-arm64-installer-7.0.2XX] - [Checksum][osx-arm64-installer-checksum-7.0.2XX]<br>[tar.gz][osx-arm64-targz-7.0.2XX] - [Checksum][osx-arm64-targz-checksum-7.0.2XX] | [![][osx-arm64-badge-7.0.1XX]][osx-arm64-version-7.0.1XX]<br>[Installer][osx-arm64-installer-7.0.1XX] - [Checksum][osx-arm64-installer-checksum-7.0.1XX]<br>[tar.gz][osx-arm64-targz-7.0.1XX] - [Checksum][osx-arm64-targz-checksum-7.0.1XX] |
| **Linux x64** | [![][linux-badge-main]][linux-version-main]<br>[DEB Installer][linux-DEB-installer-main] - [Checksum][linux-DEB-installer-checksum-main]<br>[RPM Installer][linux-RPM-installer-main] - [Checksum][linux-RPM-installer-checksum-main]<br>_see installer note below_<sup>1</sup><br>[tar.gz][linux-targz-main] - [Checksum][linux-targz-checksum-main] | [![][linux-badge-7.0.2XX]][linux-version-7.0.2XX]<br>[DEB Installer][linux-DEB-installer-7.0.2XX] - [Checksum][linux-DEB-installer-checksum-7.0.2XX]<br>[RPM Installer][linux-RPM-installer-7.0.2XX] - [Checksum][linux-RPM-installer-checksum-7.0.2XX]<br>_see installer note below_<sup>1</sup><br>[tar.gz][linux-targz-7.0.2XX] - [Checksum][linux-targz-checksum-7.0.2XX] | [![][linux-badge-7.0.1XX]][linux-version-7.0.1XX]<br>[DEB Installer][linux-DEB-installer-7.0.1XX] - [Checksum][linux-DEB-installer-checksum-7.0.1XX]<br>[RPM Installer][linux-RPM-installer-7.0.1XX] - [Checksum][linux-RPM-installer-checksum-7.0.1XX]<br>_see installer note below_<sup>1</sup><br>[tar.gz][linux-targz-7.0.1XX] - [Checksum][linux-targz-checksum-7.0.1XX] |
| **Linux arm** | [![][linux-arm-badge-main]][linux-arm-version-main]<br>[tar.gz][linux-arm-targz-main] - [Checksum][linux-arm-targz-checksum-main] | [![][linux-arm-badge-7.0.2XX]][linux-arm-version-7.0.2XX]<br>[tar.gz][linux-arm-targz-7.0.2XX] - [Checksum][linux-arm-targz-checksum-7.0.2XX] | [![][linux-arm-badge-7.0.1XX]][linux-arm-version-7.0.1XX]<br>[tar.gz][linux-arm-targz-7.0.1XX] - [Checksum][linux-arm-targz-checksum-7.0.1XX] |
| **Linux arm64** | [![][linux-arm64-badge-main]][linux-arm64-version-main]<br>[tar.gz][linux-arm64-targz-main] - [Checksum][linux-arm64-targz-checksum-main] | [![][linux-arm64-badge-7.0.2XX]][linux-arm64-version-7.0.2XX]<br>[tar.gz][linux-arm64-targz-7.0.2XX] - [Checksum][linux-arm64-targz-checksum-7.0.2XX] | [![][linux-arm64-badge-7.0.1XX]][linux-arm64-version-7.0.1XX]<br>[tar.gz][linux-arm64-targz-7.0.1XX] - [Checksum][linux-arm64-targz-checksum-7.0.1XX] |
| **Linux-musl-x64** | [![][linux-musl-x64-badge-main]][linux-musl-x64-version-main]<br>[tar.gz][linux-musl-x64-targz-main] - [Checksum][linux-musl-x64-targz-checksum-main] | [![][linux-musl-x64-badge-7.0.2XX]][linux-musl-x64-version-7.0.2XX]<br>[tar.gz][linux-musl-x64-targz-7.0.2XX] - [Checksum][linux-musl-x64-targz-checksum-7.0.2XX] | [![][linux-musl-x64-badge-7.0.1XX]][linux-musl-x64-version-7.0.1XX]<br>[tar.gz][linux-musl-x64-targz-7.0.1XX] - [Checksum][linux-musl-x64-targz-checksum-7.0.1XX] |
| **Linux-musl-arm** | [![][linux-musl-arm-badge-main]][linux-musl-arm-version-main]<br>[tar.gz][linux-musl-arm-targz-main] - [Checksum][linux-musl-arm-targz-checksum-main] | [![][linux-musl-arm-badge-7.0.2XX]][linux-musl-arm-version-7.0.2XX]<br>[tar.gz][linux-musl-arm-targz-7.0.2XX] - [Checksum][linux-musl-arm-targz-checksum-7.0.2XX] | [![][linux-musl-arm-badge-7.0.1XX]][linux-musl-arm-version-7.0.1XX]<br>[tar.gz][linux-musl-arm-targz-7.0.1XX] - [Checksum][linux-musl-arm-targz-checksum-7.0.1XX] |
| **Linux-musl-arm64** | [![][linux-musl-arm64-badge-main]][linux-musl-arm64-version-main]<br>[tar.gz][linux-musl-arm64-targz-main] - [Checksum][linux-musl-arm64-targz-checksum-main] | [![][linux-musl-arm64-badge-7.0.2XX]][linux-musl-arm64-version-7.0.2XX]<br>[tar.gz][linux-musl-arm64-targz-7.0.2XX] - [Checksum][linux-musl-arm64-targz-checksum-7.0.2XX] | [![][linux-musl-arm64-badge-7.0.1XX]][linux-musl-arm64-version-7.0.1XX]<br>[tar.gz][linux-musl-arm64-targz-7.0.1XX] - [Checksum][linux-musl-arm64-targz-checksum-7.0.1XX] |
| **RHEL 6** | **N/A** | **N/A** | **N/A** |

Reference notes:
> **1**: Our Debian packages are put together slightly differently than the other OS specific installers. Instead of combining everything, we have separate component packages that depend on each other. If you're installing the SDK from the .deb file (via dpkg or similar), then you'll need to install the corresponding dependencies first:
> * [Host, Host FX Resolver, and Shared Framework](https://github.com/dotnet/runtime/blob/main/docs/project/dogfooding.md#nightly-builds-table)
> * [ASP.NET Core Shared Framework](https://github.com/aspnet/AspNetCore/blob/main/docs/DailyBuilds.md)

.NET Core SDK 2.x downloads can be found here: [.NET Core SDK 2.x Installers and Binaries](Downloads2.x.md)

[win-x64-badge-main]: https://aka.ms/dotnet/8.0.1xx/daily/win_x64_Release_version_badge.svg
[win-x64-version-main]: https://aka.ms/dotnet/8.0.1xx/daily/productCommit-win-x64.txt
[win-x64-installer-main]: https://aka.ms/dotnet/8.0.1xx/daily/dotnet-sdk-win-x64.exe
[win-x64-installer-checksum-main]: https://aka.ms/dotnet/8.0.1xx/daily/dotnet-sdk-win-x64.exe.sha
[win-x64-zip-main]: https://aka.ms/dotnet/8.0.1xx/daily/dotnet-sdk-win-x64.zip
[win-x64-zip-checksum-main]: https://aka.ms/dotnet/8.0.1xx/daily/dotnet-sdk-win-x64.zip.sha

[win-x64-badge-7.0.2XX]: https://aka.ms/dotnet/7.0.2xx/daily/win_x64_Release_version_badge.svg
[win-x64-version-7.0.2XX]: https://aka.ms/dotnet/7.0.2xx/daily/productCommit-win-x64.txt
[win-x64-installer-7.0.2XX]: https://aka.ms/dotnet/7.0.2xx/daily/dotnet-sdk-win-x64.exe
[win-x64-installer-checksum-7.0.2XX]: https://aka.ms/dotnet/7.0.2xx/daily/dotnet-sdk-win-x64.exe.sha
[win-x64-zip-7.0.2XX]: https://aka.ms/dotnet/7.0.2xx/daily/dotnet-sdk-win-x64.zip
[win-x64-zip-checksum-7.0.2XX]: https://aka.ms/dotnet/7.0.2xx/daily/dotnet-sdk-win-x64.zip.sha

[win-x64-badge-7.0.1XX]: https://aka.ms/dotnet/7.0.1xx/daily/win_x64_Release_version_badge.svg
[win-x64-version-7.0.1XX]: https://aka.ms/dotnet/7.0.1xx/daily/productCommit-win-x64.txt
[win-x64-installer-7.0.1XX]: https://aka.ms/dotnet/7.0.1xx/daily/dotnet-sdk-win-x64.exe
[win-x64-installer-checksum-7.0.1XX]: https://aka.ms/dotnet/7.0.1xx/daily/dotnet-sdk-win-x64.exe.sha
[win-x64-zip-7.0.1XX]: https://aka.ms/dotnet/7.0.1xx/daily/dotnet-sdk-win-x64.zip
[win-x64-zip-checksum-7.0.1XX]: https://aka.ms/dotnet/7.0.1xx/daily/dotnet-sdk-win-x64.zip.sha

[win-x86-badge-main]: https://aka.ms/dotnet/8.0.1xx/daily/win_x86_Release_version_badge.svg
[win-x86-version-main]: https://aka.ms/dotnet/8.0.1xx/daily/productCommit-win-x86.txt
[win-x86-installer-main]: https://aka.ms/dotnet/8.0.1xx/daily/dotnet-sdk-win-x86.exe
[win-x86-installer-checksum-main]: https://aka.ms/dotnet/8.0.1xx/daily/dotnet-sdk-win-x86.exe.sha
[win-x86-zip-main]: https://aka.ms/dotnet/8.0.1xx/daily/dotnet-sdk-win-x86.zip
[win-x86-zip-checksum-main]: https://aka.ms/dotnet/8.0.1xx/daily/dotnet-sdk-win-x86.zip.sha

[win-x86-badge-7.0.2XX]: https://aka.ms/dotnet/7.0.2xx/daily/win_x86_Release_version_badge.svg
[win-x86-version-7.0.2XX]: https://aka.ms/dotnet/7.0.2xx/daily/productCommit-win-x86.txt
[win-x86-installer-7.0.2XX]: https://aka.ms/dotnet/7.0.2xx/daily/dotnet-sdk-win-x86.exe
[win-x86-installer-checksum-7.0.2XX]: https://aka.ms/dotnet/7.0.2xx/daily/dotnet-sdk-win-x86.exe.sha
[win-x86-zip-7.0.2XX]: https://aka.ms/dotnet/7.0.2xx/daily/dotnet-sdk-win-x86.zip
[win-x86-zip-checksum-7.0.2XX]: https://aka.ms/dotnet/7.0.2xx/daily/dotnet-sdk-win-x86.zip.sha

[win-x86-badge-7.0.1XX]: https://aka.ms/dotnet/7.0.1xx/daily/win_x86_Release_version_badge.svg
[win-x86-version-7.0.1XX]: https://aka.ms/dotnet/7.0.1xx/daily/productCommit-win-x86.txt
[win-x86-installer-7.0.1XX]: https://aka.ms/dotnet/7.0.1xx/daily/dotnet-sdk-win-x86.exe
[win-x86-installer-checksum-7.0.1XX]: https://aka.ms/dotnet/7.0.1xx/daily/dotnet-sdk-win-x86.exe.sha
[win-x86-zip-7.0.1XX]: https://aka.ms/dotnet/7.0.1xx/daily/dotnet-sdk-win-x86.zip
[win-x86-zip-checksum-7.0.1XX]: https://aka.ms/dotnet/7.0.1xx/daily/dotnet-sdk-win-x86.zip.sha

[osx-x64-badge-main]: https://aka.ms/dotnet/8.0.1xx/daily/osx_x64_Release_version_badge.svg
[osx-x64-version-main]: https://aka.ms/dotnet/8.0.1xx/daily/productCommit-osx-x64.txt
[osx-x64-installer-main]: https://aka.ms/dotnet/8.0.1xx/daily/dotnet-sdk-osx-x64.pkg
[osx-x64-installer-checksum-main]: https://aka.ms/dotnet/8.0.1xx/daily/dotnet-sdk-osx-x64.pkg.sha
[osx-x64-targz-main]: https://aka.ms/dotnet/8.0.1xx/daily/dotnet-sdk-osx-x64.tar.gz
[osx-x64-targz-checksum-main]: https://aka.ms/dotnet/8.0.1xx/daily/dotnet-sdk-osx-x64.pkg.tar.gz.sha

[osx-x64-badge-7.0.2XX]: https://aka.ms/dotnet/7.0.2xx/daily/osx_x64_Release_version_badge.svg
[osx-x64-version-7.0.2XX]: https://aka.ms/dotnet/7.0.2xx/daily/productCommit-osx-x64.txt
[osx-x64-installer-7.0.2XX]: https://aka.ms/dotnet/7.0.2xx/daily/dotnet-sdk-osx-x64.pkg
[osx-x64-installer-checksum-7.0.2XX]: https://aka.ms/dotnet/7.0.2xx/daily/dotnet-sdk-osx-x64.pkg.sha
[osx-x64-targz-7.0.2XX]: https://aka.ms/dotnet/7.0.2xx/daily/dotnet-sdk-osx-x64.tar.gz
[osx-x64-targz-checksum-7.0.2XX]: https://aka.ms/dotnet/7.0.2xx/daily/dotnet-sdk-osx-x64.pkg.tar.gz.sha

[osx-x64-badge-7.0.1XX]: https://aka.ms/dotnet/7.0.1xx/daily/osx_x64_Release_version_badge.svg
[osx-x64-version-7.0.1XX]: https://aka.ms/dotnet/7.0.1xx/daily/productCommit-osx-x64.txt
[osx-x64-installer-7.0.1XX]: https://aka.ms/dotnet/7.0.1xx/daily/dotnet-sdk-osx-x64.pkg
[osx-x64-installer-checksum-7.0.1XX]: https://aka.ms/dotnet/7.0.1xx/daily/dotnet-sdk-osx-x64.pkg.sha
[osx-x64-targz-7.0.1XX]: https://aka.ms/dotnet/7.0.1xx/daily/dotnet-sdk-osx-x64.tar.gz
[osx-x64-targz-checksum-7.0.1XX]: https://aka.ms/dotnet/7.0.1xx/daily/dotnet-sdk-osx-x64.pkg.tar.gz.sha

[osx-arm64-badge-main]: https://aka.ms/dotnet/8.0.1xx/daily/osx_arm64_Release_version_badge.svg
[osx-arm64-version-main]: https://aka.ms/dotnet/8.0.1xx/daily/productCommit-osx-arm64.txt
[osx-arm64-installer-main]: https://aka.ms/dotnet/8.0.1xx/daily/dotnet-sdk-osx-arm64.pkg
[osx-arm64-installer-checksum-main]: https://aka.ms/dotnet/8.0.1xx/daily/dotnet-sdk-osx-arm64.pkg.sha
[osx-arm64-targz-main]: https://aka.ms/dotnet/8.0.1xx/daily/dotnet-sdk-osx-arm64.tar.gz
[osx-arm64-targz-checksum-main]: https://aka.ms/dotnet/8.0.1xx/daily/dotnet-sdk-osx-arm64.pkg.tar.gz.sha

[osx-arm64-badge-7.0.2XX]: https://aka.ms/dotnet/7.0.2xx/daily/osx_arm64_Release_version_badge.svg
[osx-arm64-version-7.0.2XX]: https://aka.ms/dotnet/7.0.2xx/daily/productCommit-osx-arm64.txt
[osx-arm64-installer-7.0.2XX]: https://aka.ms/dotnet/7.0.2xx/daily/dotnet-sdk-osx-arm64.pkg
[osx-arm64-installer-checksum-7.0.2XX]: https://aka.ms/dotnet/7.0.2xx/daily/dotnet-sdk-osx-arm64.pkg.sha
[osx-arm64-targz-7.0.2XX]: https://aka.ms/dotnet/7.0.2xx/daily/dotnet-sdk-osx-arm64.tar.gz
[osx-arm64-targz-checksum-7.0.2XX]: https://aka.ms/dotnet/7.0.2xx/daily/dotnet-sdk-osx-arm64.pkg.tar.gz.sha

[osx-arm64-badge-7.0.1XX]: https://aka.ms/dotnet/7.0.1xx/daily/osx_arm64_Release_version_badge.svg
[osx-arm64-version-7.0.1XX]: https://aka.ms/dotnet/7.0.1xx/daily/productCommit-osx-arm64.txt
[osx-arm64-installer-7.0.1XX]: https://aka.ms/dotnet/7.0.1xx/daily/dotnet-sdk-osx-arm64.pkg
[osx-arm64-installer-checksum-7.0.1XX]: https://aka.ms/dotnet/7.0.1xx/daily/dotnet-sdk-osx-arm64.pkg.sha
[osx-arm64-targz-7.0.1XX]: https://aka.ms/dotnet/7.0.1xx/daily/dotnet-sdk-osx-arm64.tar.gz
[osx-arm64-targz-checksum-7.0.1XX]: https://aka.ms/dotnet/7.0.1xx/daily/dotnet-sdk-osx-arm64.pkg.tar.gz.sha

[linux-badge-main]: https://aka.ms/dotnet/8.0.1xx/daily/linux_x64_Release_version_badge.svg
[linux-version-main]: https://aka.ms/dotnet/8.0.1xx/daily/productCommit-linux-x64.txt
[linux-DEB-installer-main]: https://aka.ms/dotnet/8.0.1xx/daily/dotnet-sdk-x64.deb
[linux-DEB-installer-checksum-main]: https://aka.ms/dotnet/8.0.1xx/daily/dotnet-sdk-x64.deb.sha
[linux-RPM-installer-main]: https://aka.ms/dotnet/8.0.1xx/daily/dotnet-sdk-x64.rpm
[linux-RPM-installer-checksum-main]: https://aka.ms/dotnet/8.0.1xx/daily/dotnet-sdk-x64.rpm.sha
[linux-targz-main]: https://aka.ms/dotnet/8.0.1xx/daily/dotnet-sdk-linux-x64.tar.gz
[linux-targz-checksum-main]: https://aka.ms/dotnet/8.0.1xx/daily/dotnet-sdk-linux-x64.tar.gz.sha

[linux-badge-7.0.2XX]: https://aka.ms/dotnet/7.0.2xx/daily/linux_x64_Release_version_badge.svg
[linux-version-7.0.2XX]: https://aka.ms/dotnet/7.0.2xx/daily/productCommit-linux-x64.txt
[linux-DEB-installer-7.0.2XX]: https://aka.ms/dotnet/7.0.2xx/daily/dotnet-sdk-x64.deb
[linux-DEB-installer-checksum-7.0.2XX]: https://aka.ms/dotnet/7.0.2xx/daily/dotnet-sdk-x64.deb.sha
[linux-RPM-installer-7.0.2XX]: https://aka.ms/dotnet/7.0.2xx/daily/dotnet-sdk-x64.rpm
[linux-RPM-installer-checksum-7.0.2XX]: https://aka.ms/dotnet/7.0.2xx/daily/dotnet-sdk-x64.rpm.sha
[linux-targz-7.0.2XX]: https://aka.ms/dotnet/7.0.2xx/daily/dotnet-sdk-linux-x64.tar.gz
[linux-targz-checksum-7.0.2XX]: https://aka.ms/dotnet/7.0.2xx/daily/dotnet-sdk-linux-x64.tar.gz.sha

[linux-badge-7.0.1XX]: https://aka.ms/dotnet/7.0.1xx/daily/linux_x64_Release_version_badge.svg
[linux-version-7.0.1XX]: https://aka.ms/dotnet/7.0.1xx/daily/productCommit-linux-x64.txt
[linux-DEB-installer-7.0.1XX]: https://aka.ms/dotnet/7.0.1xx/daily/dotnet-sdk-x64.deb
[linux-DEB-installer-checksum-7.0.1XX]: https://aka.ms/dotnet/7.0.1xx/daily/dotnet-sdk-x64.deb.sha
[linux-RPM-installer-7.0.1XX]: https://aka.ms/dotnet/7.0.1xx/daily/dotnet-sdk-x64.rpm
[linux-RPM-installer-checksum-7.0.1XX]: https://aka.ms/dotnet/7.0.1xx/daily/dotnet-sdk-x64.rpm.sha
[linux-targz-7.0.1XX]: https://aka.ms/dotnet/7.0.1xx/daily/dotnet-sdk-linux-x64.tar.gz
[linux-targz-checksum-7.0.1XX]: https://aka.ms/dotnet/7.0.1xx/daily/dotnet-sdk-linux-x64.tar.gz.sha

[linux-arm-badge-main]: https://aka.ms/dotnet/8.0.1xx/daily/linux_arm_Release_version_badge.svg
[linux-arm-version-main]: https://aka.ms/dotnet/8.0.1xx/daily/productCommit-linux-arm.txt
[linux-arm-targz-main]: https://aka.ms/dotnet/8.0.1xx/daily/dotnet-sdk-linux-arm.tar.gz
[linux-arm-targz-checksum-main]: https://aka.ms/dotnet/8.0.1xx/daily/dotnet-sdk-linux-arm.tar.gz.sha

[linux-arm-badge-7.0.2XX]: https://aka.ms/dotnet/7.0.2xx/daily/linux_arm_Release_version_badge.svg
[linux-arm-version-7.0.2XX]: https://aka.ms/dotnet/7.0.2xx/daily/productCommit-linux-arm.txt
[linux-arm-targz-7.0.2XX]: https://aka.ms/dotnet/7.0.2xx/daily/dotnet-sdk-linux-arm.tar.gz
[linux-arm-targz-checksum-7.0.2XX]: https://aka.ms/dotnet/7.0.2xx/daily/dotnet-sdk-linux-arm.tar.gz.sha

[linux-arm-badge-7.0.1XX]: https://aka.ms/dotnet/7.0.1xx/daily/linux_arm_Release_version_badge.svg
[linux-arm-version-7.0.1XX]: https://aka.ms/dotnet/7.0.1xx/daily/productCommit-linux-arm.txt
[linux-arm-targz-7.0.1XX]: https://aka.ms/dotnet/7.0.1xx/daily/dotnet-sdk-linux-arm.tar.gz
[linux-arm-targz-checksum-7.0.1XX]: https://aka.ms/dotnet/7.0.1xx/daily/dotnet-sdk-linux-arm.tar.gz.sha

[linux-arm64-badge-main]: https://aka.ms/dotnet/8.0.1xx/daily/linux_arm64_Release_version_badge.svg
[linux-arm64-version-main]: https://aka.ms/dotnet/8.0.1xx/daily/productCommit-linux-arm64.txt
[linux-arm64-targz-main]: https://aka.ms/dotnet/8.0.1xx/daily/dotnet-sdk-linux-arm64.tar.gz
[linux-arm64-targz-checksum-main]: https://aka.ms/dotnet/8.0.1xx/daily/dotnet-sdk-linux-arm64.tar.gz.sha

[linux-arm64-badge-7.0.2XX]: https://aka.ms/dotnet/7.0.2xx/daily/linux_arm64_Release_version_badge.svg
[linux-arm64-version-7.0.2XX]: https://aka.ms/dotnet/7.0.2xx/daily/productCommit-linux-arm64.txt
[linux-arm64-targz-7.0.2XX]: https://aka.ms/dotnet/7.0.2xx/daily/dotnet-sdk-linux-arm64.tar.gz
[linux-arm64-targz-checksum-7.0.2XX]: https://aka.ms/dotnet/7.0.2xx/daily/dotnet-sdk-linux-arm64.tar.gz.sha

[linux-arm64-badge-7.0.1XX]: https://aka.ms/dotnet/7.0.1xx/daily/linux_arm64_Release_version_badge.svg
[linux-arm64-version-7.0.1XX]: https://aka.ms/dotnet/7.0.1xx/daily/productCommit-linux-arm64.txt
[linux-arm64-targz-7.0.1XX]: https://aka.ms/dotnet/7.0.1xx/daily/dotnet-sdk-linux-arm64.tar.gz
[linux-arm64-targz-checksum-7.0.1XX]: https://aka.ms/dotnet/7.0.1xx/daily/dotnet-sdk-linux-arm64.tar.gz.sha

[rhel-6-badge-main]: https://aka.ms/dotnet/8.0.1xx/daily/rhel.6_x64_Release_version_badge.svg
[rhel-6-version-main]: https://aka.ms/dotnet/8.0.1xx/daily/productCommit-rhel.6-x64.txt
[rhel-6-targz-main]: https://aka.ms/dotnet/8.0.1xx/daily/dotnet-sdk-rhel.6-x64.tar.gz
[rhel-6-targz-checksum-main]: https://aka.ms/dotnet/8.0.1xx/daily/dotnet-sdk-rhel.6-x64.tar.gz.sha

[rhel-6-badge-7.0.2XX]: https://aka.ms/dotnet/7.0.2xx/daily/rhel.6_x64_Release_version_badge.svg
[rhel-6-version-7.0.2XX]: https://aka.ms/dotnet/7.0.2xx/daily/productCommit-rhel.6-x64.txt
[rhel-6-targz-7.0.2XX]: https://aka.ms/dotnet/7.0.2xx/daily/dotnet-sdk-rhel.6-x64.tar.gz
[rhel-6-targz-checksum-7.0.2XX]: https://aka.ms/dotnet/7.0.2xx/daily/dotnet-sdk-rhel.6-x64.tar.gz.sha

[rhel-6-badge-7.0.1XX]: https://aka.ms/dotnet/7.0.1xx/daily/rhel.6_x64_Release_version_badge.svg
[rhel-6-version-7.0.1XX]: https://aka.ms/dotnet/7.0.1xx/daily/productCommit-rhel.6-x64.txt
[rhel-6-targz-7.0.1XX]: https://aka.ms/dotnet/7.0.1xx/daily/dotnet-sdk-rhel.6-x64.tar.gz
[rhel-6-targz-checksum-7.0.1XX]: https://aka.ms/dotnet/7.0.1xx/daily/dotnet-sdk-rhel.6-x64.tar.gz.sha

[linux-musl-x64-badge-main]: https://aka.ms/dotnet/8.0.1xx/daily/linux_musl_x64_Release_version_badge.svg
[linux-musl-x64-version-main]: https://aka.ms/dotnet/8.0.1xx/daily/productCommit-linux-musl-x64.txt
[linux-musl-x64-targz-main]: https://aka.ms/dotnet/8.0.1xx/daily/dotnet-sdk-linux-musl-x64.tar.gz
[linux-musl-x64-targz-checksum-main]: https://aka.ms/dotnet/8.0.1xx/daily/dotnet-sdk-linux-musl-x64.tar.gz.sha

[linux-musl-x64-badge-7.0.2XX]: https://aka.ms/dotnet/7.0.2xx/daily/linux_musl_x64_Release_version_badge.svg
[linux-musl-x64-version-7.0.2XX]: https://aka.ms/dotnet/7.0.2xx/daily/productCommit-linux-musl-x64.txt
[linux-musl-x64-targz-7.0.2XX]: https://aka.ms/dotnet/7.0.2xx/daily/dotnet-sdk-linux-musl-x64.tar.gz
[linux-musl-x64-targz-checksum-7.0.2XX]: https://aka.ms/dotnet/7.0.2xx/daily/dotnet-sdk-linux-musl-x64.tar.gz.sha

[linux-musl-x64-badge-7.0.1XX]: https://aka.ms/dotnet/7.0.1xx/daily/linux_musl_x64_Release_version_badge.svg
[linux-musl-x64-version-7.0.1XX]: https://aka.ms/dotnet/7.0.1xx/daily/productCommit-linux-musl-x64.txt
[linux-musl-x64-targz-7.0.1XX]: https://aka.ms/dotnet/7.0.1xx/daily/dotnet-sdk-linux-musl-x64.tar.gz
[linux-musl-x64-targz-checksum-7.0.1XX]: https://aka.ms/dotnet/7.0.1xx/daily/dotnet-sdk-linux-musl-x64.tar.gz.sha

[linux-musl-arm-badge-main]: https://aka.ms/dotnet/8.0.1xx/daily/linux_musl_arm_Release_version_badge.svg
[linux-musl-arm-version-main]: https://aka.ms/dotnet/8.0.1xx/daily/productCommit-linux-musl-arm.txt
[linux-musl-arm-targz-main]: https://aka.ms/dotnet/8.0.1xx/daily/dotnet-sdk-linux-musl-arm.tar.gz
[linux-musl-arm-targz-checksum-main]: https://aka.ms/dotnet/8.0.1xx/daily/dotnet-sdk-linux-musl-arm.tar.gz.sha

[linux-musl-arm-badge-7.0.2XX]: https://aka.ms/dotnet/7.0.2xx/daily/linux_musl_arm_Release_version_badge.svg
[linux-musl-arm-version-7.0.2XX]: https://aka.ms/dotnet/7.0.2xx/daily/productCommit-linux-musl-arm.txt
[linux-musl-arm-targz-7.0.2XX]: https://aka.ms/dotnet/7.0.2xx/daily/dotnet-sdk-linux-musl-arm.tar.gz
[linux-musl-arm-targz-checksum-7.0.2XX]: https://aka.ms/dotnet/7.0.2xx/daily/dotnet-sdk-linux-musl-arm.tar.gz.sha

[linux-musl-arm-badge-7.0.1XX]: https://aka.ms/dotnet/7.0.1xx/daily/linux_musl_arm_Release_version_badge.svg
[linux-musl-arm-version-7.0.1XX]: https://aka.ms/dotnet/7.0.1xx/daily/productCommit-linux-musl-arm.txt
[linux-musl-arm-targz-7.0.1XX]: https://aka.ms/dotnet/7.0.1xx/daily/dotnet-sdk-linux-musl-arm.tar.gz
[linux-musl-arm-targz-checksum-7.0.1XX]: https://aka.ms/dotnet/7.0.1xx/daily/dotnet-sdk-linux-musl-arm.tar.gz.sha

[linux-musl-arm64-badge-main]: https://aka.ms/dotnet/8.0.1xx/daily/linux_musl_arm64_Release_version_badge.svg
[linux-musl-arm64-version-main]: https://aka.ms/dotnet/8.0.1xx/daily/productCommit-linux-musl-arm64.txt
[linux-musl-arm64-targz-main]: https://aka.ms/dotnet/8.0.1xx/daily/dotnet-sdk-linux-musl-arm64.tar.gz
[linux-musl-arm64-targz-checksum-main]: https://aka.ms/dotnet/8.0.1xx/daily/dotnet-sdk-linux-musl-arm64.tar.gz.sha

[linux-musl-arm64-badge-7.0.2XX]: https://aka.ms/dotnet/7.0.2xx/daily/linux_musl_arm64_Release_version_badge.svg
[linux-musl-arm64-version-7.0.2XX]: https://aka.ms/dotnet/7.0.2xx/daily/productCommit-linux-musl-arm64.txt
[linux-musl-arm64-targz-7.0.2XX]: https://aka.ms/dotnet/7.0.2xx/daily/dotnet-sdk-linux-musl-arm64.tar.gz
[linux-musl-arm64-targz-checksum-7.0.2XX]: https://aka.ms/dotnet/7.0.2xx/daily/dotnet-sdk-linux-musl-arm64.tar.gz.sha

[linux-musl-arm64-badge-7.0.1XX]: https://aka.ms/dotnet/7.0.1xx/daily/linux_musl_arm64_Release_version_badge.svg
[linux-musl-arm64-version-7.0.1XX]: https://aka.ms/dotnet/7.0.1xx/daily/productCommit-linux-musl-arm64.txt
[linux-musl-arm64-targz-7.0.1XX]: https://aka.ms/dotnet/7.0.1xx/daily/dotnet-sdk-linux-musl-arm64.tar.gz
[linux-musl-arm64-targz-checksum-7.0.1XX]: https://aka.ms/dotnet/7.0.1xx/daily/dotnet-sdk-linux-musl-arm64.tar.gz.sha

[win-arm-badge-main]: https://aka.ms/dotnet/8.0.1xx/daily/win_arm_Release_version_badge.svg
[win-arm-version-main]: https://aka.ms/dotnet/8.0.1xx/daily/productCommit-win-arm.txt
[win-arm-zip-main]: https://aka.ms/dotnet/8.0.1xx/daily/dotnet-sdk-win-arm.zip
[win-arm-zip-checksum-main]: https://aka.ms/dotnet/8.0.1xx/daily/dotnet-sdk-win-arm.zip.sha

[win-arm-badge-7.0.2XX]: https://aka.ms/dotnet/7.0.2xx/daily/win_arm_Release_version_badge.svg
[win-arm-version-7.0.2XX]: https://aka.ms/dotnet/7.0.2xx/daily/productCommit-win-arm.txt
[win-arm-zip-7.0.2XX]: https://aka.ms/dotnet/7.0.2xx/daily/dotnet-sdk-win-arm.zip
[win-arm-zip-checksum-7.0.2XX]: https://aka.ms/dotnet/7.0.2xx/daily/dotnet-sdk-win-arm.zip.sha

[win-arm-badge-7.0.1XX]: https://aka.ms/dotnet/7.0.1xx/daily/win_arm_Release_version_badge.svg
[win-arm-version-7.0.1XX]: https://aka.ms/dotnet/7.0.1xx/daily/productCommit-win-arm.txt
[win-arm-zip-7.0.1XX]: https://aka.ms/dotnet/7.0.1xx/daily/dotnet-sdk-win-arm.zip
[win-arm-zip-checksum-7.0.1XX]: https://aka.ms/dotnet/7.0.1xx/daily/dotnet-sdk-win-arm.zip.sha

[win-arm64-badge-main]: https://aka.ms/dotnet/8.0.1xx/daily/win_arm64_Release_version_badge.svg
[win-arm64-version-main]: https://aka.ms/dotnet/8.0.1xx/daily/productCommit-win-arm64.txt
[win-arm64-installer-main]: https://aka.ms/dotnet/8.0.1xx/daily/dotnet-sdk-win-arm64.exe
[win-arm64-installer-checksum-main]: https://aka.ms/dotnet/8.0.1xx/daily/dotnet-sdk-win-arm64.exe.sha
[win-arm64-zip-main]: https://aka.ms/dotnet/8.0.1xx/daily/dotnet-sdk-win-arm64.zip
[win-arm64-zip-checksum-main]: https://aka.ms/dotnet/8.0.1xx/daily/dotnet-sdk-win-arm64.zip.sha

[win-arm64-badge-7.0.2XX]: https://aka.ms/dotnet/7.0.2xx/daily/win_arm64_Release_version_badge.svg
[win-arm64-version-7.0.2XX]: https://aka.ms/dotnet/7.0.2xx/daily/productCommit-win-arm64.txt
[win-arm64-installer-7.0.2XX]: https://aka.ms/dotnet/7.0.2xx/daily/dotnet-sdk-win-arm64.exe
[win-arm64-installer-checksum-7.0.2XX]: https://aka.ms/dotnet/7.0.2xx/daily/dotnet-sdk-win-arm64.exe.sha
[win-arm64-zip-7.0.2XX]: https://aka.ms/dotnet/7.0.2xx/daily/dotnet-sdk-win-arm64.zip
[win-arm64-zip-checksum-7.0.2XX]: https://aka.ms/dotnet/7.0.2xx/daily/dotnet-sdk-win-arm64.zip.sha

[win-arm64-badge-7.0.1XX]: https://aka.ms/dotnet/7.0.1xx/daily/win_arm64_Release_version_badge.svg
[win-arm64-version-7.0.1XX]: https://aka.ms/dotnet/7.0.1xx/daily/productCommit-win-arm64.txt
[win-arm64-installer-7.0.1XX]: https://aka.ms/dotnet/7.0.1xx/daily/dotnet-sdk-win-arm64.exe
[win-arm64-installer-checksum-7.0.1XX]: https://aka.ms/dotnet/7.0.1xx/daily/dotnet-sdk-win-arm64.exe.sha
[win-arm64-zip-7.0.1XX]: https://aka.ms/dotnet/7.0.1xx/daily/dotnet-sdk-win-arm64.zip
[win-arm64-zip-checksum-7.0.1XX]: https://aka.ms/dotnet/7.0.1xx/daily/dotnet-sdk-win-arm64.zip.sha

[sdk-shas-2.2.1XX]: https://github.com/dotnet/versions/tree/master/build-info/dotnet/product/cli/release/2.2#built-repositories


Looking for dotnet-install sources?
-----------------------------------

Sources for dotnet-install.sh and dotnet-install.ps1 are in the [install-scripts repo](https://github.com/dotnet/install-scripts).

Questions & Comments
--------------------

For all feedback, use the Issues on the [.NET SDK](https://github.com/dotnet/sdk) repository.

License
-------

By downloading the .zip you are agreeing to the terms in the project [EULA](https://aka.ms/dotnet-core-eula).

