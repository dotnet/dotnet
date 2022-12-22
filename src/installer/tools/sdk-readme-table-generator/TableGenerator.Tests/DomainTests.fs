module TableGenerator.Tests.DomainTests

open TableGenerator.Shared
open TableGenerator.Reference
open TableGenerator.Table
open TableGenerator.App

open FsUnit
open Xunit
open System

[<Fact>]
let ``it can Shorten main branch Name``() =
    let branch =
        { GitBranchName = "main"
          DisplayName = "main<br>(5.0.x&nbsp;Runtime)"
          AkaMsChannel = Some("net5/dev") }
    branchNameShorten branch |> should equal "main"

[<Fact>]
let ``it can Shorten release branch Name``() =
    let branch =
        { GitBranchName = "release/3.1.1xx"
          DisplayName = "Release/3.1.1XX<br>(3.1.x Runtime)"
          AkaMsChannel = None}
    branchNameShorten branch |> should equal "3.1.1XX"

[<Fact>]
let ``it can get major and minor version of a branch``() =
    let branch =
        { GitBranchName = "release/3.1.1xx"
          DisplayName = "Release/3.1.1XX<br>(3.1.x Runtime)"
          AkaMsChannel = None}
    getMajorMinor branch
    |> should equal
           (MajorMinor
               ({ Major = 3
                  Minor = 1
                  Patch = 199
                  Release = ""}))
           
[<Fact>]
let ``it can get major and minor version of a preview branch``() =
    let branch =
      { GitBranchName = "release/5.0.1xx-preview2"
        DisplayName = "5.0.100 Preview 2<br>(5.0 Runtime)"
        AkaMsChannel = Some("net5/preview2") }
    getMajorMinor branch
    |> should equal
           (MajorMinor
               ({ Major = 5
                  Minor = 0
                  Patch = 199
                  Release = "preview2"}))

[<Fact>]
let ``it can get main version of a main branch``() =
    let branch =
        { GitBranchName = "main"
          DisplayName = "main<br>(5.0.x&nbsp;Runtime)"
          AkaMsChannel = None}
    getMajorMinor branch |> should equal Main

[<Fact>]
let ``it can get bad branch version``() =
    let branch =
        { GitBranchName = "badbranch"
          DisplayName = ""
          AkaMsChannel = None}
    getMajorMinor branch |> should equal NoVersion

[<Fact>]
let ``it can format winx64Reference``() =
    let branch =
        { GitBranchName = "release/3.1.1xx"
          DisplayName = "Release/3.1.1XX<br>(3.1.x Runtime)"
          AkaMsChannel = None}
    (winX64ReferenceTemplate branch).Value
    |> should equal
           """[win-x64-badge-3.1.1XX]: https://dotnetcli.blob.core.windows.net/dotnet/Sdk/release/3.1.1xx/win_x64_Release_version_badge.svg
[win-x64-version-3.1.1XX]: https://dotnetcli.blob.core.windows.net/dotnet/Sdk/release/3.1.1xx/latest.version
[win-x64-installer-3.1.1XX]: https://dotnetcli.blob.core.windows.net/dotnet/Sdk/release/3.1.1xx/dotnet-sdk-latest-win-x64.exe
[win-x64-installer-checksum-3.1.1XX]: https://dotnetclichecksums.blob.core.windows.net/dotnet/Sdk/release/3.1.1xx/dotnet-sdk-latest-win-x64.exe.sha
[win-x64-zip-3.1.1XX]: https://dotnetcli.blob.core.windows.net/dotnet/Sdk/release/3.1.1xx/dotnet-sdk-latest-win-x64.zip
[win-x64-zip-checksum-3.1.1XX]: https://dotnetclichecksums.blob.core.windows.net/dotnet/Sdk/release/3.1.1xx/dotnet-sdk-latest-win-x64.zip.sha"""

[<Fact>]
let ``it can format ReferenceWithAkaMslink``() =
    let branch =
        { GitBranchName = "release/5.0.1xx-preview2"
          DisplayName = "5.0.100 Preview 2<br>(5.0 Runtime)"
          AkaMsChannel = Some("net5/preview2") }

    (formatTemplate "win-x64" referenceTemplate branch).Value
    |> should equal
           """[win-x64-badge-5.0.1XX-preview2]: https://aka.ms/dotnet/net5/preview2/Sdk/win_x64_Release_version_badge.svg
[win-x64-version-5.0.1XX-preview2]: https://aka.ms/dotnet/net5/preview2/Sdk/productCommit-win-x64.txt
[win-x64-installer-5.0.1XX-preview2]: https://aka.ms/dotnet/net5/preview2/Sdk/dotnet-sdk-win-x64.exe
[win-x64-installer-checksum-5.0.1XX-preview2]: https://aka.ms/dotnet/net5/preview2/Sdk/dotnet-sdk-win-x64.exe.sha
[win-x64-zip-5.0.1XX-preview2]: https://aka.ms/dotnet/net5/preview2/Sdk/dotnet-sdk-win-x64.zip
[win-x64-zip-checksum-5.0.1XX-preview2]: https://aka.ms/dotnet/net5/preview2/Sdk/dotnet-sdk-win-x64.zip.sha"""


let branches =
    [ { GitBranchName = "main"
        DisplayName = "main<br>(5.0.x&nbsp;Runtime)"
        AkaMsChannel = None}
      { GitBranchName = "release/3.1.1xx"
        DisplayName = "Release/3.1.1XX<br>(3.1.x Runtime)"
        AkaMsChannel = None}
      { GitBranchName = "release/3.0.1xx"
        DisplayName = "Release/3.0.1xx<br>(3.0.x Runtime)"
        AkaMsChannel = None}
      { GitBranchName = "release/2.2.2xx"
        DisplayName = "Release/2.2.2XX<br>(2.2.x Runtime)"
        AkaMsChannel = None}
      { GitBranchName = "release/2.2.1xx"
        DisplayName = "Release/2.2.1XX<br>(2.2.x Runtime)"
        AkaMsChannel = None}
      { GitBranchName = "release/2.1.6xx"
        DisplayName = "Release/2.1.6XX<br>(2.1.x Runtime)"
        AkaMsChannel = None}
      { GitBranchName = "release/2.1.5xx"
        DisplayName = "Release/2.1.5XX<br>(2.1.x Runtime)"
        AkaMsChannel = None} ]

[<Fact>]
let ``it can generate WindowsX64Row``() =
    windowsX64Row branches
    |> should equal
           "| **Windows x64** | [![][win-x64-badge-main]][win-x64-version-main]<br>[Installer][win-x64-installer-main] - [Checksum][win-x64-installer-checksum-main]<br>[zip][win-x64-zip-main] - [Checksum][win-x64-zip-checksum-main] | [![][win-x64-badge-3.1.1XX]][win-x64-version-3.1.1XX]<br>[Installer][win-x64-installer-3.1.1XX] - [Checksum][win-x64-installer-checksum-3.1.1XX]<br>[zip][win-x64-zip-3.1.1XX] - [Checksum][win-x64-zip-checksum-3.1.1XX] | [![][win-x64-badge-3.0.1XX]][win-x64-version-3.0.1XX]<br>[Installer][win-x64-installer-3.0.1XX] - [Checksum][win-x64-installer-checksum-3.0.1XX]<br>[zip][win-x64-zip-3.0.1XX] - [Checksum][win-x64-zip-checksum-3.0.1XX] | [![][win-x64-badge-2.2.2XX]][win-x64-version-2.2.2XX]<br>[Installer][win-x64-installer-2.2.2XX] - [Checksum][win-x64-installer-checksum-2.2.2XX]<br>[zip][win-x64-zip-2.2.2XX] - [Checksum][win-x64-zip-checksum-2.2.2XX] | [![][win-x64-badge-2.2.1XX]][win-x64-version-2.2.1XX]<br>[Installer][win-x64-installer-2.2.1XX] - [Checksum][win-x64-installer-checksum-2.2.1XX]<br>[zip][win-x64-zip-2.2.1XX] - [Checksum][win-x64-zip-checksum-2.2.1XX] | [![][win-x64-badge-2.1.6XX]][win-x64-version-2.1.6XX]<br>[Installer][win-x64-installer-2.1.6XX] - [Checksum][win-x64-installer-checksum-2.1.6XX]<br>[zip][win-x64-zip-2.1.6XX] - [Checksum][win-x64-zip-checksum-2.1.6XX] | [![][win-x64-badge-2.1.5XX]][win-x64-version-2.1.5XX]<br>[Installer][win-x64-installer-2.1.5XX] - [Checksum][win-x64-installer-checksum-2.1.5XX]<br>[zip][win-x64-zip-2.1.5XX] - [Checksum][win-x64-zip-checksum-2.1.5XX] |"

[<Fact>]
let ``it can generate OsxX64Row``() =
    osxX64Row branches
    |> should equal
           "| **macOS x64** | [![][osx-x64-badge-main]][osx-x64-version-main]<br>[Installer][osx-x64-installer-main] - [Checksum][osx-x64-installer-checksum-main]<br>[tar.gz][osx-x64-targz-main] - [Checksum][osx-x64-targz-checksum-main] | [![][osx-x64-badge-3.1.1XX]][osx-x64-version-3.1.1XX]<br>[Installer][osx-x64-installer-3.1.1XX] - [Checksum][osx-x64-installer-checksum-3.1.1XX]<br>[tar.gz][osx-x64-targz-3.1.1XX] - [Checksum][osx-x64-targz-checksum-3.1.1XX] | [![][osx-x64-badge-3.0.1XX]][osx-x64-version-3.0.1XX]<br>[Installer][osx-x64-installer-3.0.1XX] - [Checksum][osx-x64-installer-checksum-3.0.1XX]<br>[tar.gz][osx-x64-targz-3.0.1XX] - [Checksum][osx-x64-targz-checksum-3.0.1XX] | [![][osx-x64-badge-2.2.2XX]][osx-x64-version-2.2.2XX]<br>[Installer][osx-x64-installer-2.2.2XX] - [Checksum][osx-x64-installer-checksum-2.2.2XX]<br>[tar.gz][osx-x64-targz-2.2.2XX] - [Checksum][osx-x64-targz-checksum-2.2.2XX] | [![][osx-x64-badge-2.2.1XX]][osx-x64-version-2.2.1XX]<br>[Installer][osx-x64-installer-2.2.1XX] - [Checksum][osx-x64-installer-checksum-2.2.1XX]<br>[tar.gz][osx-x64-targz-2.2.1XX] - [Checksum][osx-x64-targz-checksum-2.2.1XX] | [![][osx-x64-badge-2.1.6XX]][osx-x64-version-2.1.6XX]<br>[Installer][osx-x64-installer-2.1.6XX] - [Checksum][osx-x64-installer-checksum-2.1.6XX]<br>[tar.gz][osx-x64-targz-2.1.6XX] - [Checksum][osx-x64-targz-checksum-2.1.6XX] | [![][osx-x64-badge-2.1.5XX]][osx-x64-version-2.1.5XX]<br>[Installer][osx-x64-installer-2.1.5XX] - [Checksum][osx-x64-installer-checksum-2.1.5XX]<br>[tar.gz][osx-x64-targz-2.1.5XX] - [Checksum][osx-x64-targz-checksum-2.1.5XX] |"

[<Fact>]
let ``it can generate linuxArmRow``() =
    linuxArmRow branches
    |> should equal
           "| **Linux arm** | [![][linux-arm-badge-main]][linux-arm-version-main]<br>[tar.gz][linux-arm-targz-main] - [Checksum][linux-arm-targz-checksum-main] | [![][linux-arm-badge-3.1.1XX]][linux-arm-version-3.1.1XX]<br>[tar.gz][linux-arm-targz-3.1.1XX] - [Checksum][linux-arm-targz-checksum-3.1.1XX] | [![][linux-arm-badge-3.0.1XX]][linux-arm-version-3.0.1XX]<br>[tar.gz][linux-arm-targz-3.0.1XX] - [Checksum][linux-arm-targz-checksum-3.0.1XX] | [![][linux-arm-badge-2.2.2XX]][linux-arm-version-2.2.2XX]<br>[tar.gz][linux-arm-targz-2.2.2XX] - [Checksum][linux-arm-targz-checksum-2.2.2XX] | [![][linux-arm-badge-2.2.1XX]][linux-arm-version-2.2.1XX]<br>[tar.gz][linux-arm-targz-2.2.1XX] - [Checksum][linux-arm-targz-checksum-2.2.1XX] | [![][linux-arm-badge-2.1.6XX]][linux-arm-version-2.1.6XX]<br>[tar.gz][linux-arm-targz-2.1.6XX] - [Checksum][linux-arm-targz-checksum-2.1.6XX] | [![][linux-arm-badge-2.1.5XX]][linux-arm-version-2.1.5XX]<br>[tar.gz][linux-arm-targz-2.1.5XX] - [Checksum][linux-arm-targz-checksum-2.1.5XX] |"

[<Fact>]
let ``it can generate windowsArm``() =
    windowsArmRow branches
    |> should equal
          "| **Windows arm** | **N/A** | [![][win-arm-badge-3.1.1XX]][win-arm-version-3.1.1XX]<br>[zip][win-arm-zip-3.1.1XX] - [Checksum][win-arm-zip-checksum-3.1.1XX] | [![][win-arm-badge-3.0.1XX]][win-arm-version-3.0.1XX]<br>[zip][win-arm-zip-3.0.1XX] - [Checksum][win-arm-zip-checksum-3.0.1XX] | [![][win-arm-badge-2.2.2XX]][win-arm-version-2.2.2XX]<br>[zip][win-arm-zip-2.2.2XX] - [Checksum][win-arm-zip-checksum-2.2.2XX] | [![][win-arm-badge-2.2.1XX]][win-arm-version-2.2.1XX]<br>[zip][win-arm-zip-2.2.1XX] - [Checksum][win-arm-zip-checksum-2.2.1XX] | [![][win-arm-badge-2.1.6XX]][win-arm-version-2.1.6XX]<br>[zip][win-arm-zip-2.1.6XX] - [Checksum][win-arm-zip-checksum-2.1.6XX] | [![][win-arm-badge-2.1.5XX]][win-arm-version-2.1.5XX]<br>[zip][win-arm-zip-2.1.5XX] - [Checksum][win-arm-zip-checksum-2.1.5XX] |"


[<Fact>]
let ``it can generate windowsArm with preview branch``() =
    let inputBranches =
        [ { GitBranchName = "main"
            DisplayName = "main<br>(5.0.x&nbsp;Runtime)"
            AkaMsChannel = Some("net5/dev") }
          { GitBranchName = "release/5.0.1xx-preview4"
            DisplayName = "5.0.100 Preview 4<br>(5.0 Runtime)"
            AkaMsChannel = Some("net5/preview4") }]
    windowsArmRow inputBranches
    |> should equal
           "| **Windows arm** | **N/A** | **N/A** |"

[<Fact>]
let ``it can generate windowsArm64``() =
    windowsArm64Row branches
    |> should equal
           "| **Windows arm64** | [![][win-arm64-badge-main]][win-arm64-version-main]<br>[Installer][win-arm64-installer-main] - [Checksum][win-arm64-installer-checksum-main]<br>[zip][win-arm64-zip-main] | **N/A** | **N/A** | **N/A** | **N/A** | **N/A** | **N/A** |"

let branches2 =
        [ { GitBranchName = "main"
            DisplayName = "main<br>(6.0.x&nbsp;Runtime)"
            AkaMsChannel = Some("net6/dev") }
          { GitBranchName = "release/5.0.1xx-rtm"
            DisplayName = "5.0.100 RTM<br>(5.0 Runtime)"
            AkaMsChannel = Some("net5/5.0.1xx/daily") }
          { GitBranchName = "release/3.1.4xx"
            DisplayName = "Release/3.1.4XX<br>(3.1.x Runtime)"
            AkaMsChannel = None }]

[<Fact>]
let ``it can generate linuxMuslRowArm``() =
    linuxMuslRowArm branches2
    |> should equal
           "| **Linux-musl-arm** | [![][linux-musl-arm-badge-main]][linux-musl-arm-version-main]<br>[tar.gz][linux-musl-arm-targz-main] - [Checksum][linux-musl-arm-targz-checksum-main] | **N/A** | **N/A** |"

[<Fact>]
let ``it can generate linuxMuslRowArm64``() =
    linuxMuslRowArm64 branches2
    |> should equal
           "| **Linux-musl-arm64** | [![][linux-musl-arm64-badge-main]][linux-musl-arm64-version-main]<br>[tar.gz][linux-musl-arm64-targz-main] - [Checksum][linux-musl-arm64-targz-checksum-main] | **N/A** | **N/A** |"


[<Fact>]
let ``it can generate titleRow``() =
    titleRow branches
    |> should equal
           "| Platform | main<br>(5.0.x&nbsp;Runtime) | Release/3.1.1XX<br>(3.1.x Runtime) | Release/3.0.1xx<br>(3.0.x Runtime) | Release/2.2.2XX<br>(2.2.x Runtime) | Release/2.2.1XX<br>(2.2.x Runtime) | Release/2.1.6XX<br>(2.1.x Runtime) | Release/2.1.5XX<br>(2.1.x Runtime) |"

[<Fact>]
let ``it can generate separator``() =
    separator branches
    |> should equal
           "| :--------- | :----------: | :----------: | :----------: | :----------: | :----------: | :----------: | :----------: |"
// pinning tests https://wiki.c2.com/?PinningTests

[<Fact>]
let ``pinning tests for readme in main 11/02/2020``() =
    let inputBranches =
        [ { GitBranchName = "main"
            DisplayName = "main<br>(6.0.x&nbsp;Runtime)"
            AkaMsChannel = Some("6.0/daily") }
          { GitBranchName = "release/5.0.1xx-rtm"
            DisplayName = "5.0.100 RTM<br>(5.0 Runtime)"
            AkaMsChannel = Some("net5/5.0.1xx/daily") }
          { GitBranchName = "release/5.0.1xx-rc2"
            DisplayName = "5.0.100 RC2<br>(5.0 Runtime)"
            AkaMsChannel = Some("net5/rc2") }
          { GitBranchName = "release/3.1.4xx"
            DisplayName = "Release/3.1.4XX<br>(3.1.x Runtime)"
            AkaMsChannel = None }
          { GitBranchName = "release/3.1.1xx"
            DisplayName = "Release/3.1.1XX<br>(3.1.x Runtime)"
            AkaMsChannel = None }]

    wholeTable inputBranches
    |> should equal
       """| Platform | main<br>(6.0.x&nbsp;Runtime) | 5.0.100 RTM<br>(5.0 Runtime) | 5.0.100 RC2<br>(5.0 Runtime) | Release/3.1.4XX<br>(3.1.x Runtime) | Release/3.1.1XX<br>(3.1.x Runtime) |
| :--------- | :----------: | :----------: | :----------: | :----------: | :----------: |
| **Windows x64** | [![][win-x64-badge-main]][win-x64-version-main]<br>[Installer][win-x64-installer-main] - [Checksum][win-x64-installer-checksum-main]<br>[zip][win-x64-zip-main] - [Checksum][win-x64-zip-checksum-main] | [![][win-x64-badge-5.0.1XX-rtm]][win-x64-version-5.0.1XX-rtm]<br>[Installer][win-x64-installer-5.0.1XX-rtm] - [Checksum][win-x64-installer-checksum-5.0.1XX-rtm]<br>[zip][win-x64-zip-5.0.1XX-rtm] - [Checksum][win-x64-zip-checksum-5.0.1XX-rtm] | [![][win-x64-badge-5.0.1XX-rc2]][win-x64-version-5.0.1XX-rc2]<br>[Installer][win-x64-installer-5.0.1XX-rc2] - [Checksum][win-x64-installer-checksum-5.0.1XX-rc2]<br>[zip][win-x64-zip-5.0.1XX-rc2] - [Checksum][win-x64-zip-checksum-5.0.1XX-rc2] | [![][win-x64-badge-3.1.4XX]][win-x64-version-3.1.4XX]<br>[Installer][win-x64-installer-3.1.4XX] - [Checksum][win-x64-installer-checksum-3.1.4XX]<br>[zip][win-x64-zip-3.1.4XX] - [Checksum][win-x64-zip-checksum-3.1.4XX] | [![][win-x64-badge-3.1.1XX]][win-x64-version-3.1.1XX]<br>[Installer][win-x64-installer-3.1.1XX] - [Checksum][win-x64-installer-checksum-3.1.1XX]<br>[zip][win-x64-zip-3.1.1XX] - [Checksum][win-x64-zip-checksum-3.1.1XX] |
| **Windows x86** | [![][win-x86-badge-main]][win-x86-version-main]<br>[Installer][win-x86-installer-main] - [Checksum][win-x86-installer-checksum-main]<br>[zip][win-x86-zip-main] - [Checksum][win-x86-zip-checksum-main] | [![][win-x86-badge-5.0.1XX-rtm]][win-x86-version-5.0.1XX-rtm]<br>[Installer][win-x86-installer-5.0.1XX-rtm] - [Checksum][win-x86-installer-checksum-5.0.1XX-rtm]<br>[zip][win-x86-zip-5.0.1XX-rtm] - [Checksum][win-x86-zip-checksum-5.0.1XX-rtm] | [![][win-x86-badge-5.0.1XX-rc2]][win-x86-version-5.0.1XX-rc2]<br>[Installer][win-x86-installer-5.0.1XX-rc2] - [Checksum][win-x86-installer-checksum-5.0.1XX-rc2]<br>[zip][win-x86-zip-5.0.1XX-rc2] - [Checksum][win-x86-zip-checksum-5.0.1XX-rc2] | [![][win-x86-badge-3.1.4XX]][win-x86-version-3.1.4XX]<br>[Installer][win-x86-installer-3.1.4XX] - [Checksum][win-x86-installer-checksum-3.1.4XX]<br>[zip][win-x86-zip-3.1.4XX] - [Checksum][win-x86-zip-checksum-3.1.4XX] | [![][win-x86-badge-3.1.1XX]][win-x86-version-3.1.1XX]<br>[Installer][win-x86-installer-3.1.1XX] - [Checksum][win-x86-installer-checksum-3.1.1XX]<br>[zip][win-x86-zip-3.1.1XX] - [Checksum][win-x86-zip-checksum-3.1.1XX] |
| **Windows arm** | **N/A** | **N/A** | **N/A** | [![][win-arm-badge-3.1.4XX]][win-arm-version-3.1.4XX]<br>[zip][win-arm-zip-3.1.4XX] - [Checksum][win-arm-zip-checksum-3.1.4XX] | [![][win-arm-badge-3.1.1XX]][win-arm-version-3.1.1XX]<br>[zip][win-arm-zip-3.1.1XX] - [Checksum][win-arm-zip-checksum-3.1.1XX] |
| **Windows arm64** | [![][win-arm64-badge-main]][win-arm64-version-main]<br>[Installer][win-arm64-installer-main] - [Checksum][win-arm64-installer-checksum-main]<br>[zip][win-arm64-zip-main] | [![][win-arm64-badge-5.0.1XX-rtm]][win-arm64-version-5.0.1XX-rtm]<br>[Installer][win-arm64-installer-5.0.1XX-rtm] - [Checksum][win-arm64-installer-checksum-5.0.1XX-rtm]<br>[zip][win-arm64-zip-5.0.1XX-rtm] | [![][win-arm64-badge-5.0.1XX-rc2]][win-arm64-version-5.0.1XX-rc2]<br>[Installer][win-arm64-installer-5.0.1XX-rc2] - [Checksum][win-arm64-installer-checksum-5.0.1XX-rc2]<br>[zip][win-arm64-zip-5.0.1XX-rc2] | **N/A** | **N/A** |
| **macOS x64** | [![][osx-x64-badge-main]][osx-x64-version-main]<br>[Installer][osx-x64-installer-main] - [Checksum][osx-x64-installer-checksum-main]<br>[tar.gz][osx-x64-targz-main] - [Checksum][osx-x64-targz-checksum-main] | [![][osx-x64-badge-5.0.1XX-rtm]][osx-x64-version-5.0.1XX-rtm]<br>[Installer][osx-x64-installer-5.0.1XX-rtm] - [Checksum][osx-x64-installer-checksum-5.0.1XX-rtm]<br>[tar.gz][osx-x64-targz-5.0.1XX-rtm] - [Checksum][osx-x64-targz-checksum-5.0.1XX-rtm] | [![][osx-x64-badge-5.0.1XX-rc2]][osx-x64-version-5.0.1XX-rc2]<br>[Installer][osx-x64-installer-5.0.1XX-rc2] - [Checksum][osx-x64-installer-checksum-5.0.1XX-rc2]<br>[tar.gz][osx-x64-targz-5.0.1XX-rc2] - [Checksum][osx-x64-targz-checksum-5.0.1XX-rc2] | [![][osx-x64-badge-3.1.4XX]][osx-x64-version-3.1.4XX]<br>[Installer][osx-x64-installer-3.1.4XX] - [Checksum][osx-x64-installer-checksum-3.1.4XX]<br>[tar.gz][osx-x64-targz-3.1.4XX] - [Checksum][osx-x64-targz-checksum-3.1.4XX] | [![][osx-x64-badge-3.1.1XX]][osx-x64-version-3.1.1XX]<br>[Installer][osx-x64-installer-3.1.1XX] - [Checksum][osx-x64-installer-checksum-3.1.1XX]<br>[tar.gz][osx-x64-targz-3.1.1XX] - [Checksum][osx-x64-targz-checksum-3.1.1XX] |
| **macOS arm64** | [![][osx-arm64-badge-main]][osx-arm64-version-main]<br>[Installer][osx-arm64-installer-main] - [Checksum][osx-arm64-installer-checksum-main]<br>[tar.gz][osx-arm64-targz-main] - [Checksum][osx-arm64-targz-checksum-main] | **N/A** | **N/A** | **N/A** | **N/A** |
| **Linux x64** | [![][linux-badge-main]][linux-version-main]<br>[DEB Installer][linux-DEB-installer-main] - [Checksum][linux-DEB-installer-checksum-main]<br>[RPM Installer][linux-RPM-installer-main] - [Checksum][linux-RPM-installer-checksum-main]<br>_see installer note below_<sup>1</sup><br>[tar.gz][linux-targz-main] - [Checksum][linux-targz-checksum-main] | [![][linux-badge-5.0.1XX-rtm]][linux-version-5.0.1XX-rtm]<br>[DEB Installer][linux-DEB-installer-5.0.1XX-rtm] - [Checksum][linux-DEB-installer-checksum-5.0.1XX-rtm]<br>[RPM Installer][linux-RPM-installer-5.0.1XX-rtm] - [Checksum][linux-RPM-installer-checksum-5.0.1XX-rtm]<br>_see installer note below_<sup>1</sup><br>[tar.gz][linux-targz-5.0.1XX-rtm] - [Checksum][linux-targz-checksum-5.0.1XX-rtm] | [![][linux-badge-5.0.1XX-rc2]][linux-version-5.0.1XX-rc2]<br>[DEB Installer][linux-DEB-installer-5.0.1XX-rc2] - [Checksum][linux-DEB-installer-checksum-5.0.1XX-rc2]<br>[RPM Installer][linux-RPM-installer-5.0.1XX-rc2] - [Checksum][linux-RPM-installer-checksum-5.0.1XX-rc2]<br>_see installer note below_<sup>1</sup><br>[tar.gz][linux-targz-5.0.1XX-rc2] - [Checksum][linux-targz-checksum-5.0.1XX-rc2] | [![][linux-badge-3.1.4XX]][linux-version-3.1.4XX]<br>[DEB Installer][linux-DEB-installer-3.1.4XX] - [Checksum][linux-DEB-installer-checksum-3.1.4XX]<br>[RPM Installer][linux-RPM-installer-3.1.4XX] - [Checksum][linux-RPM-installer-checksum-3.1.4XX]<br>_see installer note below_<sup>1</sup><br>[tar.gz][linux-targz-3.1.4XX] - [Checksum][linux-targz-checksum-3.1.4XX] | [![][linux-badge-3.1.1XX]][linux-version-3.1.1XX]<br>[DEB Installer][linux-DEB-installer-3.1.1XX] - [Checksum][linux-DEB-installer-checksum-3.1.1XX]<br>[RPM Installer][linux-RPM-installer-3.1.1XX] - [Checksum][linux-RPM-installer-checksum-3.1.1XX]<br>_see installer note below_<sup>1</sup><br>[tar.gz][linux-targz-3.1.1XX] - [Checksum][linux-targz-checksum-3.1.1XX] |
| **Linux arm** | [![][linux-arm-badge-main]][linux-arm-version-main]<br>[tar.gz][linux-arm-targz-main] - [Checksum][linux-arm-targz-checksum-main] | [![][linux-arm-badge-5.0.1XX-rtm]][linux-arm-version-5.0.1XX-rtm]<br>[tar.gz][linux-arm-targz-5.0.1XX-rtm] - [Checksum][linux-arm-targz-checksum-5.0.1XX-rtm] | [![][linux-arm-badge-5.0.1XX-rc2]][linux-arm-version-5.0.1XX-rc2]<br>[tar.gz][linux-arm-targz-5.0.1XX-rc2] - [Checksum][linux-arm-targz-checksum-5.0.1XX-rc2] | [![][linux-arm-badge-3.1.4XX]][linux-arm-version-3.1.4XX]<br>[tar.gz][linux-arm-targz-3.1.4XX] - [Checksum][linux-arm-targz-checksum-3.1.4XX] | [![][linux-arm-badge-3.1.1XX]][linux-arm-version-3.1.1XX]<br>[tar.gz][linux-arm-targz-3.1.1XX] - [Checksum][linux-arm-targz-checksum-3.1.1XX] |
| **Linux arm64** | [![][linux-arm64-badge-main]][linux-arm64-version-main]<br>[tar.gz][linux-arm64-targz-main] - [Checksum][linux-arm64-targz-checksum-main] | [![][linux-arm64-badge-5.0.1XX-rtm]][linux-arm64-version-5.0.1XX-rtm]<br>[tar.gz][linux-arm64-targz-5.0.1XX-rtm] - [Checksum][linux-arm64-targz-checksum-5.0.1XX-rtm] | [![][linux-arm64-badge-5.0.1XX-rc2]][linux-arm64-version-5.0.1XX-rc2]<br>[tar.gz][linux-arm64-targz-5.0.1XX-rc2] - [Checksum][linux-arm64-targz-checksum-5.0.1XX-rc2] | [![][linux-arm64-badge-3.1.4XX]][linux-arm64-version-3.1.4XX]<br>[tar.gz][linux-arm64-targz-3.1.4XX] - [Checksum][linux-arm64-targz-checksum-3.1.4XX] | [![][linux-arm64-badge-3.1.1XX]][linux-arm64-version-3.1.1XX]<br>[tar.gz][linux-arm64-targz-3.1.1XX] - [Checksum][linux-arm64-targz-checksum-3.1.1XX] |
| **Linux-musl-x64** | [![][linux-musl-x64-badge-main]][linux-musl-x64-version-main]<br>[tar.gz][linux-musl-x64-targz-main] - [Checksum][linux-musl-x64-targz-checksum-main] | [![][linux-musl-x64-badge-5.0.1XX-rtm]][linux-musl-x64-version-5.0.1XX-rtm]<br>[tar.gz][linux-musl-x64-targz-5.0.1XX-rtm] - [Checksum][linux-musl-x64-targz-checksum-5.0.1XX-rtm] | [![][linux-musl-x64-badge-5.0.1XX-rc2]][linux-musl-x64-version-5.0.1XX-rc2]<br>[tar.gz][linux-musl-x64-targz-5.0.1XX-rc2] - [Checksum][linux-musl-x64-targz-checksum-5.0.1XX-rc2] | [![][linux-musl-x64-badge-3.1.4XX]][linux-musl-x64-version-3.1.4XX]<br>[tar.gz][linux-musl-x64-targz-3.1.4XX] - [Checksum][linux-musl-x64-targz-checksum-3.1.4XX] | [![][linux-musl-x64-badge-3.1.1XX]][linux-musl-x64-version-3.1.1XX]<br>[tar.gz][linux-musl-x64-targz-3.1.1XX] - [Checksum][linux-musl-x64-targz-checksum-3.1.1XX] |
| **Linux-musl-arm** | [![][linux-musl-arm-badge-main]][linux-musl-arm-version-main]<br>[tar.gz][linux-musl-arm-targz-main] - [Checksum][linux-musl-arm-targz-checksum-main] | **N/A** | **N/A** | **N/A** | **N/A** |
| **Linux-musl-arm64** | [![][linux-musl-arm64-badge-main]][linux-musl-arm64-version-main]<br>[tar.gz][linux-musl-arm64-targz-main] - [Checksum][linux-musl-arm64-targz-checksum-main] | **N/A** | **N/A** | **N/A** | **N/A** |
| **RHEL 6** | **N/A** | **N/A** | **N/A** | [![][rhel-6-badge-3.1.4XX]][rhel-6-version-3.1.4XX]<br>[tar.gz][rhel-6-targz-3.1.4XX] - [Checksum][rhel-6-targz-checksum-3.1.4XX] | [![][rhel-6-badge-3.1.1XX]][rhel-6-version-3.1.1XX]<br>[tar.gz][rhel-6-targz-3.1.1XX] - [Checksum][rhel-6-targz-checksum-3.1.1XX] |

Reference notes:
> **1**: Our Debian packages are put together slightly differently than the other OS specific installers. Instead of combining everything, we have separate component packages that depend on each other. If you're installing the SDK from the .deb file (via dpkg or similar), then you'll need to install the corresponding dependencies first:
> * [Host, Host FX Resolver, and Shared Framework](https://github.com/dotnet/runtime/blob/main/docs/project/dogfooding.md#nightly-builds-table)
> * [ASP.NET Core Shared Framework](https://github.com/aspnet/AspNetCore/blob/main/docs/DailyBuilds.md)

.NET Core SDK 2.x downloads can be found here: [.NET Core SDK 2.x Installers and Binaries](Downloads2.x.md)

[win-x64-badge-main]: https://aka.ms/dotnet/6.0/daily/win_x64_Release_version_badge.svg
[win-x64-version-main]: https://aka.ms/dotnet/6.0/daily/productCommit-win-x64.txt
[win-x64-installer-main]: https://aka.ms/dotnet/6.0/daily/dotnet-sdk-win-x64.exe
[win-x64-installer-checksum-main]: https://aka.ms/dotnet/6.0/daily/dotnet-sdk-win-x64.exe.sha
[win-x64-zip-main]: https://aka.ms/dotnet/6.0/daily/dotnet-sdk-win-x64.zip
[win-x64-zip-checksum-main]: https://aka.ms/dotnet/6.0/daily/dotnet-sdk-win-x64.zip.sha

[win-x64-badge-5.0.1XX-rtm]: https://aka.ms/dotnet/net5/5.0.1xx/daily/Sdk/win_x64_Release_version_badge.svg
[win-x64-version-5.0.1XX-rtm]: https://aka.ms/dotnet/net5/5.0.1xx/daily/Sdk/productCommit-win-x64.txt
[win-x64-installer-5.0.1XX-rtm]: https://aka.ms/dotnet/net5/5.0.1xx/daily/Sdk/dotnet-sdk-win-x64.exe
[win-x64-installer-checksum-5.0.1XX-rtm]: https://aka.ms/dotnet/net5/5.0.1xx/daily/Sdk/dotnet-sdk-win-x64.exe.sha
[win-x64-zip-5.0.1XX-rtm]: https://aka.ms/dotnet/net5/5.0.1xx/daily/Sdk/dotnet-sdk-win-x64.zip
[win-x64-zip-checksum-5.0.1XX-rtm]: https://aka.ms/dotnet/net5/5.0.1xx/daily/Sdk/dotnet-sdk-win-x64.zip.sha

[win-x64-badge-5.0.1XX-rc2]: https://aka.ms/dotnet/net5/rc2/Sdk/win_x64_Release_version_badge.svg
[win-x64-version-5.0.1XX-rc2]: https://aka.ms/dotnet/net5/rc2/Sdk/productCommit-win-x64.txt
[win-x64-installer-5.0.1XX-rc2]: https://aka.ms/dotnet/net5/rc2/Sdk/dotnet-sdk-win-x64.exe
[win-x64-installer-checksum-5.0.1XX-rc2]: https://aka.ms/dotnet/net5/rc2/Sdk/dotnet-sdk-win-x64.exe.sha
[win-x64-zip-5.0.1XX-rc2]: https://aka.ms/dotnet/net5/rc2/Sdk/dotnet-sdk-win-x64.zip
[win-x64-zip-checksum-5.0.1XX-rc2]: https://aka.ms/dotnet/net5/rc2/Sdk/dotnet-sdk-win-x64.zip.sha

[win-x64-badge-3.1.4XX]: https://dotnetcli.blob.core.windows.net/dotnet/Sdk/release/3.1.4xx/win_x64_Release_version_badge.svg
[win-x64-version-3.1.4XX]: https://dotnetcli.blob.core.windows.net/dotnet/Sdk/release/3.1.4xx/latest.version
[win-x64-installer-3.1.4XX]: https://dotnetcli.blob.core.windows.net/dotnet/Sdk/release/3.1.4xx/dotnet-sdk-latest-win-x64.exe
[win-x64-installer-checksum-3.1.4XX]: https://dotnetclichecksums.blob.core.windows.net/dotnet/Sdk/release/3.1.4xx/dotnet-sdk-latest-win-x64.exe.sha
[win-x64-zip-3.1.4XX]: https://dotnetcli.blob.core.windows.net/dotnet/Sdk/release/3.1.4xx/dotnet-sdk-latest-win-x64.zip
[win-x64-zip-checksum-3.1.4XX]: https://dotnetclichecksums.blob.core.windows.net/dotnet/Sdk/release/3.1.4xx/dotnet-sdk-latest-win-x64.zip.sha

[win-x64-badge-3.1.1XX]: https://dotnetcli.blob.core.windows.net/dotnet/Sdk/release/3.1.1xx/win_x64_Release_version_badge.svg
[win-x64-version-3.1.1XX]: https://dotnetcli.blob.core.windows.net/dotnet/Sdk/release/3.1.1xx/latest.version
[win-x64-installer-3.1.1XX]: https://dotnetcli.blob.core.windows.net/dotnet/Sdk/release/3.1.1xx/dotnet-sdk-latest-win-x64.exe
[win-x64-installer-checksum-3.1.1XX]: https://dotnetclichecksums.blob.core.windows.net/dotnet/Sdk/release/3.1.1xx/dotnet-sdk-latest-win-x64.exe.sha
[win-x64-zip-3.1.1XX]: https://dotnetcli.blob.core.windows.net/dotnet/Sdk/release/3.1.1xx/dotnet-sdk-latest-win-x64.zip
[win-x64-zip-checksum-3.1.1XX]: https://dotnetclichecksums.blob.core.windows.net/dotnet/Sdk/release/3.1.1xx/dotnet-sdk-latest-win-x64.zip.sha

[win-x86-badge-main]: https://aka.ms/dotnet/6.0/daily/win_x86_Release_version_badge.svg
[win-x86-version-main]: https://aka.ms/dotnet/6.0/daily/productCommit-win-x86.txt
[win-x86-installer-main]: https://aka.ms/dotnet/6.0/daily/dotnet-sdk-win-x86.exe
[win-x86-installer-checksum-main]: https://aka.ms/dotnet/6.0/daily/dotnet-sdk-win-x86.exe.sha
[win-x86-zip-main]: https://aka.ms/dotnet/6.0/daily/dotnet-sdk-win-x86.zip
[win-x86-zip-checksum-main]: https://aka.ms/dotnet/6.0/daily/dotnet-sdk-win-x86.zip.sha

[win-x86-badge-5.0.1XX-rtm]: https://aka.ms/dotnet/net5/5.0.1xx/daily/Sdk/win_x86_Release_version_badge.svg
[win-x86-version-5.0.1XX-rtm]: https://aka.ms/dotnet/net5/5.0.1xx/daily/Sdk/productCommit-win-x86.txt
[win-x86-installer-5.0.1XX-rtm]: https://aka.ms/dotnet/net5/5.0.1xx/daily/Sdk/dotnet-sdk-win-x86.exe
[win-x86-installer-checksum-5.0.1XX-rtm]: https://aka.ms/dotnet/net5/5.0.1xx/daily/Sdk/dotnet-sdk-win-x86.exe.sha
[win-x86-zip-5.0.1XX-rtm]: https://aka.ms/dotnet/net5/5.0.1xx/daily/Sdk/dotnet-sdk-win-x86.zip
[win-x86-zip-checksum-5.0.1XX-rtm]: https://aka.ms/dotnet/net5/5.0.1xx/daily/Sdk/dotnet-sdk-win-x86.zip.sha

[win-x86-badge-5.0.1XX-rc2]: https://aka.ms/dotnet/net5/rc2/Sdk/win_x86_Release_version_badge.svg
[win-x86-version-5.0.1XX-rc2]: https://aka.ms/dotnet/net5/rc2/Sdk/productCommit-win-x86.txt
[win-x86-installer-5.0.1XX-rc2]: https://aka.ms/dotnet/net5/rc2/Sdk/dotnet-sdk-win-x86.exe
[win-x86-installer-checksum-5.0.1XX-rc2]: https://aka.ms/dotnet/net5/rc2/Sdk/dotnet-sdk-win-x86.exe.sha
[win-x86-zip-5.0.1XX-rc2]: https://aka.ms/dotnet/net5/rc2/Sdk/dotnet-sdk-win-x86.zip
[win-x86-zip-checksum-5.0.1XX-rc2]: https://aka.ms/dotnet/net5/rc2/Sdk/dotnet-sdk-win-x86.zip.sha

[win-x86-badge-3.1.4XX]: https://dotnetcli.blob.core.windows.net/dotnet/Sdk/release/3.1.4xx/win_x86_Release_version_badge.svg
[win-x86-version-3.1.4XX]: https://dotnetcli.blob.core.windows.net/dotnet/Sdk/release/3.1.4xx/latest.version
[win-x86-installer-3.1.4XX]: https://dotnetcli.blob.core.windows.net/dotnet/Sdk/release/3.1.4xx/dotnet-sdk-latest-win-x86.exe
[win-x86-installer-checksum-3.1.4XX]: https://dotnetclichecksums.blob.core.windows.net/dotnet/Sdk/release/3.1.4xx/dotnet-sdk-latest-win-x86.exe.sha
[win-x86-zip-3.1.4XX]: https://dotnetcli.blob.core.windows.net/dotnet/Sdk/release/3.1.4xx/dotnet-sdk-latest-win-x86.zip
[win-x86-zip-checksum-3.1.4XX]: https://dotnetclichecksums.blob.core.windows.net/dotnet/Sdk/release/3.1.4xx/dotnet-sdk-latest-win-x86.zip.sha

[win-x86-badge-3.1.1XX]: https://dotnetcli.blob.core.windows.net/dotnet/Sdk/release/3.1.1xx/win_x86_Release_version_badge.svg
[win-x86-version-3.1.1XX]: https://dotnetcli.blob.core.windows.net/dotnet/Sdk/release/3.1.1xx/latest.version
[win-x86-installer-3.1.1XX]: https://dotnetcli.blob.core.windows.net/dotnet/Sdk/release/3.1.1xx/dotnet-sdk-latest-win-x86.exe
[win-x86-installer-checksum-3.1.1XX]: https://dotnetclichecksums.blob.core.windows.net/dotnet/Sdk/release/3.1.1xx/dotnet-sdk-latest-win-x86.exe.sha
[win-x86-zip-3.1.1XX]: https://dotnetcli.blob.core.windows.net/dotnet/Sdk/release/3.1.1xx/dotnet-sdk-latest-win-x86.zip
[win-x86-zip-checksum-3.1.1XX]: https://dotnetclichecksums.blob.core.windows.net/dotnet/Sdk/release/3.1.1xx/dotnet-sdk-latest-win-x86.zip.sha

[osx-x64-badge-main]: https://aka.ms/dotnet/6.0/daily/osx_x64_Release_version_badge.svg
[osx-x64-version-main]: https://aka.ms/dotnet/6.0/daily/productCommit-osx-x64.txt
[osx-x64-installer-main]: https://aka.ms/dotnet/6.0/daily/dotnet-sdk-osx-x64.pkg
[osx-x64-installer-checksum-main]: https://aka.ms/dotnet/6.0/daily/dotnet-sdk-osx-x64.pkg.sha
[osx-x64-targz-main]: https://aka.ms/dotnet/6.0/daily/dotnet-sdk-osx-x64.tar.gz
[osx-x64-targz-checksum-main]: https://aka.ms/dotnet/6.0/daily/dotnet-sdk-osx-x64.pkg.tar.gz.sha

[osx-x64-badge-5.0.1XX-rtm]: https://aka.ms/dotnet/net5/5.0.1xx/daily/Sdk/osx_x64_Release_version_badge.svg
[osx-x64-version-5.0.1XX-rtm]: https://aka.ms/dotnet/net5/5.0.1xx/daily/Sdk/productCommit-osx-x64.txt
[osx-x64-installer-5.0.1XX-rtm]: https://aka.ms/dotnet/net5/5.0.1xx/daily/Sdk/dotnet-sdk-osx-x64.pkg
[osx-x64-installer-checksum-5.0.1XX-rtm]: https://aka.ms/dotnet/net5/5.0.1xx/daily/Sdk/dotnet-sdk-osx-x64.pkg.sha
[osx-x64-targz-5.0.1XX-rtm]: https://aka.ms/dotnet/net5/5.0.1xx/daily/Sdk/dotnet-sdk-osx-x64.tar.gz
[osx-x64-targz-checksum-5.0.1XX-rtm]: https://aka.ms/dotnet/net5/5.0.1xx/daily/Sdk/dotnet-sdk-osx-x64.pkg.tar.gz.sha

[osx-x64-badge-5.0.1XX-rc2]: https://aka.ms/dotnet/net5/rc2/Sdk/osx_x64_Release_version_badge.svg
[osx-x64-version-5.0.1XX-rc2]: https://aka.ms/dotnet/net5/rc2/Sdk/productCommit-osx-x64.txt
[osx-x64-installer-5.0.1XX-rc2]: https://aka.ms/dotnet/net5/rc2/Sdk/dotnet-sdk-osx-x64.pkg
[osx-x64-installer-checksum-5.0.1XX-rc2]: https://aka.ms/dotnet/net5/rc2/Sdk/dotnet-sdk-osx-x64.pkg.sha
[osx-x64-targz-5.0.1XX-rc2]: https://aka.ms/dotnet/net5/rc2/Sdk/dotnet-sdk-osx-x64.tar.gz
[osx-x64-targz-checksum-5.0.1XX-rc2]: https://aka.ms/dotnet/net5/rc2/Sdk/dotnet-sdk-osx-x64.pkg.tar.gz.sha

[osx-x64-badge-3.1.4XX]: https://dotnetcli.blob.core.windows.net/dotnet/Sdk/release/3.1.4xx/osx_x64_Release_version_badge.svg
[osx-x64-version-3.1.4XX]: https://dotnetcli.blob.core.windows.net/dotnet/Sdk/release/3.1.4xx/latest.version
[osx-x64-installer-3.1.4XX]: https://dotnetcli.blob.core.windows.net/dotnet/Sdk/release/3.1.4xx/dotnet-sdk-latest-osx-x64.pkg
[osx-x64-installer-checksum-3.1.4XX]: https://dotnetclichecksums.blob.core.windows.net/dotnet/Sdk/release/3.1.4xx/dotnet-sdk-latest-osx-x64.pkg.sha
[osx-x64-targz-3.1.4XX]: https://dotnetcli.blob.core.windows.net/dotnet/Sdk/release/3.1.4xx/dotnet-sdk-latest-osx-x64.tar.gz
[osx-x64-targz-checksum-3.1.4XX]: https://dotnetclichecksums.blob.core.windows.net/dotnet/Sdk/release/3.1.4xx/dotnet-sdk-latest-osx-x64.tar.gz.sha

[osx-x64-badge-3.1.1XX]: https://dotnetcli.blob.core.windows.net/dotnet/Sdk/release/3.1.1xx/osx_x64_Release_version_badge.svg
[osx-x64-version-3.1.1XX]: https://dotnetcli.blob.core.windows.net/dotnet/Sdk/release/3.1.1xx/latest.version
[osx-x64-installer-3.1.1XX]: https://dotnetcli.blob.core.windows.net/dotnet/Sdk/release/3.1.1xx/dotnet-sdk-latest-osx-x64.pkg
[osx-x64-installer-checksum-3.1.1XX]: https://dotnetclichecksums.blob.core.windows.net/dotnet/Sdk/release/3.1.1xx/dotnet-sdk-latest-osx-x64.pkg.sha
[osx-x64-targz-3.1.1XX]: https://dotnetcli.blob.core.windows.net/dotnet/Sdk/release/3.1.1xx/dotnet-sdk-latest-osx-x64.tar.gz
[osx-x64-targz-checksum-3.1.1XX]: https://dotnetclichecksums.blob.core.windows.net/dotnet/Sdk/release/3.1.1xx/dotnet-sdk-latest-osx-x64.tar.gz.sha

[osx-arm64-badge-main]: https://aka.ms/dotnet/6.0/daily/osx_arm64_Release_version_badge.svg
[osx-arm64-version-main]: https://aka.ms/dotnet/6.0/daily/productCommit-osx-arm64.txt
[osx-arm64-installer-main]: https://aka.ms/dotnet/6.0/daily/dotnet-sdk-osx-arm64.pkg
[osx-arm64-installer-checksum-main]: https://aka.ms/dotnet/6.0/daily/dotnet-sdk-osx-arm64.pkg.sha
[osx-arm64-targz-main]: https://aka.ms/dotnet/6.0/daily/dotnet-sdk-osx-arm64.tar.gz
[osx-arm64-targz-checksum-main]: https://aka.ms/dotnet/6.0/daily/dotnet-sdk-osx-arm64.pkg.tar.gz.sha

[linux-badge-main]: https://aka.ms/dotnet/6.0/daily/linux_x64_Release_version_badge.svg
[linux-version-main]: https://aka.ms/dotnet/6.0/daily/productCommit-linux-x64.txt
[linux-DEB-installer-main]: https://aka.ms/dotnet/6.0/daily/dotnet-sdk-x64.deb
[linux-DEB-installer-checksum-main]: https://aka.ms/dotnet/6.0/daily/dotnet-sdk-x64.deb.sha
[linux-RPM-installer-main]: https://aka.ms/dotnet/6.0/daily/dotnet-sdk-x64.rpm
[linux-RPM-installer-checksum-main]: https://aka.ms/dotnet/6.0/daily/dotnet-sdk-x64.rpm.sha
[linux-targz-main]: https://aka.ms/dotnet/6.0/daily/dotnet-sdk-linux-x64.tar.gz
[linux-targz-checksum-main]: https://aka.ms/dotnet/6.0/daily/dotnet-sdk-linux-x64.tar.gz.sha

[linux-badge-5.0.1XX-rtm]: https://aka.ms/dotnet/net5/5.0.1xx/daily/Sdk/linux_x64_Release_version_badge.svg
[linux-version-5.0.1XX-rtm]: https://aka.ms/dotnet/net5/5.0.1xx/daily/Sdk/productCommit-linux-x64.txt
[linux-DEB-installer-5.0.1XX-rtm]: https://aka.ms/dotnet/net5/5.0.1xx/daily/Sdk/dotnet-sdk-x64.deb
[linux-DEB-installer-checksum-5.0.1XX-rtm]: https://aka.ms/dotnet/net5/5.0.1xx/daily/Sdk/dotnet-sdk-x64.deb.sha
[linux-RPM-installer-5.0.1XX-rtm]: https://aka.ms/dotnet/net5/5.0.1xx/daily/Sdk/dotnet-sdk-x64.rpm
[linux-RPM-installer-checksum-5.0.1XX-rtm]: https://aka.ms/dotnet/net5/5.0.1xx/daily/Sdk/dotnet-sdk-x64.rpm.sha
[linux-targz-5.0.1XX-rtm]: https://aka.ms/dotnet/net5/5.0.1xx/daily/Sdk/dotnet-sdk-linux-x64.tar.gz
[linux-targz-checksum-5.0.1XX-rtm]: https://aka.ms/dotnet/net5/5.0.1xx/daily/Sdk/dotnet-sdk-linux-x64.tar.gz.sha

[linux-badge-5.0.1XX-rc2]: https://aka.ms/dotnet/net5/rc2/Sdk/linux_x64_Release_version_badge.svg
[linux-version-5.0.1XX-rc2]: https://aka.ms/dotnet/net5/rc2/Sdk/productCommit-linux-x64.txt
[linux-DEB-installer-5.0.1XX-rc2]: https://aka.ms/dotnet/net5/rc2/Sdk/dotnet-sdk-x64.deb
[linux-DEB-installer-checksum-5.0.1XX-rc2]: https://aka.ms/dotnet/net5/rc2/Sdk/dotnet-sdk-x64.deb.sha
[linux-RPM-installer-5.0.1XX-rc2]: https://aka.ms/dotnet/net5/rc2/Sdk/dotnet-sdk-x64.rpm
[linux-RPM-installer-checksum-5.0.1XX-rc2]: https://aka.ms/dotnet/net5/rc2/Sdk/dotnet-sdk-x64.rpm.sha
[linux-targz-5.0.1XX-rc2]: https://aka.ms/dotnet/net5/rc2/Sdk/dotnet-sdk-linux-x64.tar.gz
[linux-targz-checksum-5.0.1XX-rc2]: https://aka.ms/dotnet/net5/rc2/Sdk/dotnet-sdk-linux-x64.tar.gz.sha

[linux-badge-3.1.4XX]: https://dotnetcli.blob.core.windows.net/dotnet/Sdk/release/3.1.4xx/linux_x64_Release_version_badge.svg
[linux-version-3.1.4XX]: https://dotnetcli.blob.core.windows.net/dotnet/Sdk/release/3.1.4xx/latest.version
[linux-DEB-installer-3.1.4XX]: https://dotnetcli.blob.core.windows.net/dotnet/Sdk/release/3.1.4xx/dotnet-sdk-latest-x64.deb
[linux-DEB-installer-checksum-3.1.4XX]: https://dotnetclichecksums.blob.core.windows.net/dotnet/Sdk/release/3.1.4xx/dotnet-sdk-latest-x64.deb.sha
[linux-RPM-installer-3.1.4XX]: https://dotnetcli.blob.core.windows.net/dotnet/Sdk/release/3.1.4xx/dotnet-sdk-latest-x64.rpm
[linux-RPM-installer-checksum-3.1.4XX]: https://dotnetclichecksums.blob.core.windows.net/dotnet/Sdk/release/3.1.4xx/dotnet-sdk-latest-x64.rpm.sha
[linux-targz-3.1.4XX]: https://dotnetcli.blob.core.windows.net/dotnet/Sdk/release/3.1.4xx/dotnet-sdk-latest-linux-x64.tar.gz
[linux-targz-checksum-3.1.4XX]: https://dotnetclichecksums.blob.core.windows.net/dotnet/Sdk/release/3.1.4xx/dotnet-sdk-latest-linux-x64.tar.gz.sha

[linux-badge-3.1.1XX]: https://dotnetcli.blob.core.windows.net/dotnet/Sdk/release/3.1.1xx/linux_x64_Release_version_badge.svg
[linux-version-3.1.1XX]: https://dotnetcli.blob.core.windows.net/dotnet/Sdk/release/3.1.1xx/latest.version
[linux-DEB-installer-3.1.1XX]: https://dotnetcli.blob.core.windows.net/dotnet/Sdk/release/3.1.1xx/dotnet-sdk-latest-x64.deb
[linux-DEB-installer-checksum-3.1.1XX]: https://dotnetclichecksums.blob.core.windows.net/dotnet/Sdk/release/3.1.1xx/dotnet-sdk-latest-x64.deb.sha
[linux-RPM-installer-3.1.1XX]: https://dotnetcli.blob.core.windows.net/dotnet/Sdk/release/3.1.1xx/dotnet-sdk-latest-x64.rpm
[linux-RPM-installer-checksum-3.1.1XX]: https://dotnetclichecksums.blob.core.windows.net/dotnet/Sdk/release/3.1.1xx/dotnet-sdk-latest-x64.rpm.sha
[linux-targz-3.1.1XX]: https://dotnetcli.blob.core.windows.net/dotnet/Sdk/release/3.1.1xx/dotnet-sdk-latest-linux-x64.tar.gz
[linux-targz-checksum-3.1.1XX]: https://dotnetclichecksums.blob.core.windows.net/dotnet/Sdk/release/3.1.1xx/dotnet-sdk-latest-linux-x64.tar.gz.sha

[linux-arm-badge-main]: https://aka.ms/dotnet/6.0/daily/linux_arm_Release_version_badge.svg
[linux-arm-version-main]: https://aka.ms/dotnet/6.0/daily/productCommit-linux-arm.txt
[linux-arm-targz-main]: https://aka.ms/dotnet/6.0/daily/dotnet-sdk-linux-arm.tar.gz
[linux-arm-targz-checksum-main]: https://aka.ms/dotnet/6.0/daily/dotnet-sdk-linux-arm.tar.gz.sha

[linux-arm-badge-5.0.1XX-rtm]: https://aka.ms/dotnet/net5/5.0.1xx/daily/Sdk/linux_arm_Release_version_badge.svg
[linux-arm-version-5.0.1XX-rtm]: https://aka.ms/dotnet/net5/5.0.1xx/daily/Sdk/productCommit-linux-arm.txt
[linux-arm-targz-5.0.1XX-rtm]: https://aka.ms/dotnet/net5/5.0.1xx/daily/Sdk/dotnet-sdk-linux-arm.tar.gz
[linux-arm-targz-checksum-5.0.1XX-rtm]: https://aka.ms/dotnet/net5/5.0.1xx/daily/Sdk/dotnet-sdk-linux-arm.tar.gz.sha

[linux-arm-badge-5.0.1XX-rc2]: https://aka.ms/dotnet/net5/rc2/Sdk/linux_arm_Release_version_badge.svg
[linux-arm-version-5.0.1XX-rc2]: https://aka.ms/dotnet/net5/rc2/Sdk/productCommit-linux-arm.txt
[linux-arm-targz-5.0.1XX-rc2]: https://aka.ms/dotnet/net5/rc2/Sdk/dotnet-sdk-linux-arm.tar.gz
[linux-arm-targz-checksum-5.0.1XX-rc2]: https://aka.ms/dotnet/net5/rc2/Sdk/dotnet-sdk-linux-arm.tar.gz.sha

[linux-arm-badge-3.1.4XX]: https://dotnetcli.blob.core.windows.net/dotnet/Sdk/release/3.1.4xx/linux_arm_Release_version_badge.svg
[linux-arm-version-3.1.4XX]: https://dotnetcli.blob.core.windows.net/dotnet/Sdk/release/3.1.4xx/latest.version
[linux-arm-targz-3.1.4XX]: https://dotnetcli.blob.core.windows.net/dotnet/Sdk/release/3.1.4xx/dotnet-sdk-latest-linux-arm.tar.gz
[linux-arm-targz-checksum-3.1.4XX]: https://dotnetclichecksums.blob.core.windows.net/dotnet/Sdk/release/3.1.4xx/dotnet-sdk-latest-linux-arm.tar.gz.sha

[linux-arm-badge-3.1.1XX]: https://dotnetcli.blob.core.windows.net/dotnet/Sdk/release/3.1.1xx/linux_arm_Release_version_badge.svg
[linux-arm-version-3.1.1XX]: https://dotnetcli.blob.core.windows.net/dotnet/Sdk/release/3.1.1xx/latest.version
[linux-arm-targz-3.1.1XX]: https://dotnetcli.blob.core.windows.net/dotnet/Sdk/release/3.1.1xx/dotnet-sdk-latest-linux-arm.tar.gz
[linux-arm-targz-checksum-3.1.1XX]: https://dotnetclichecksums.blob.core.windows.net/dotnet/Sdk/release/3.1.1xx/dotnet-sdk-latest-linux-arm.tar.gz.sha

[linux-arm64-badge-main]: https://aka.ms/dotnet/6.0/daily/linux_arm64_Release_version_badge.svg
[linux-arm64-version-main]: https://aka.ms/dotnet/6.0/daily/productCommit-linux-arm64.txt
[linux-arm64-targz-main]: https://aka.ms/dotnet/6.0/daily/dotnet-sdk-linux-arm64.tar.gz
[linux-arm64-targz-checksum-main]: https://aka.ms/dotnet/6.0/daily/dotnet-sdk-linux-arm64.tar.gz.sha

[linux-arm64-badge-5.0.1XX-rtm]: https://aka.ms/dotnet/net5/5.0.1xx/daily/Sdk/linux_arm64_Release_version_badge.svg
[linux-arm64-version-5.0.1XX-rtm]: https://aka.ms/dotnet/net5/5.0.1xx/daily/Sdk/productCommit-linux-arm64.txt
[linux-arm64-targz-5.0.1XX-rtm]: https://aka.ms/dotnet/net5/5.0.1xx/daily/Sdk/dotnet-sdk-linux-arm64.tar.gz
[linux-arm64-targz-checksum-5.0.1XX-rtm]: https://aka.ms/dotnet/net5/5.0.1xx/daily/Sdk/dotnet-sdk-linux-arm64.tar.gz.sha

[linux-arm64-badge-5.0.1XX-rc2]: https://aka.ms/dotnet/net5/rc2/Sdk/linux_arm64_Release_version_badge.svg
[linux-arm64-version-5.0.1XX-rc2]: https://aka.ms/dotnet/net5/rc2/Sdk/productCommit-linux-arm64.txt
[linux-arm64-targz-5.0.1XX-rc2]: https://aka.ms/dotnet/net5/rc2/Sdk/dotnet-sdk-linux-arm64.tar.gz
[linux-arm64-targz-checksum-5.0.1XX-rc2]: https://aka.ms/dotnet/net5/rc2/Sdk/dotnet-sdk-linux-arm64.tar.gz.sha

[linux-arm64-badge-3.1.4XX]: https://dotnetcli.blob.core.windows.net/dotnet/Sdk/release/3.1.4xx/linux_arm64_Release_version_badge.svg
[linux-arm64-version-3.1.4XX]: https://dotnetcli.blob.core.windows.net/dotnet/Sdk/release/3.1.4xx/latest.version
[linux-arm64-targz-3.1.4XX]: https://dotnetcli.blob.core.windows.net/dotnet/Sdk/release/3.1.4xx/dotnet-sdk-latest-linux-arm64.tar.gz
[linux-arm64-targz-checksum-3.1.4XX]: https://dotnetclichecksums.blob.core.windows.net/dotnet/Sdk/release/3.1.4xx/dotnet-sdk-latest-linux-arm64.tar.gz.sha

[linux-arm64-badge-3.1.1XX]: https://dotnetcli.blob.core.windows.net/dotnet/Sdk/release/3.1.1xx/linux_arm64_Release_version_badge.svg
[linux-arm64-version-3.1.1XX]: https://dotnetcli.blob.core.windows.net/dotnet/Sdk/release/3.1.1xx/latest.version
[linux-arm64-targz-3.1.1XX]: https://dotnetcli.blob.core.windows.net/dotnet/Sdk/release/3.1.1xx/dotnet-sdk-latest-linux-arm64.tar.gz
[linux-arm64-targz-checksum-3.1.1XX]: https://dotnetclichecksums.blob.core.windows.net/dotnet/Sdk/release/3.1.1xx/dotnet-sdk-latest-linux-arm64.tar.gz.sha

[rhel-6-badge-main]: https://aka.ms/dotnet/6.0/daily/rhel.6_x64_Release_version_badge.svg
[rhel-6-version-main]: https://aka.ms/dotnet/6.0/daily/productCommit-rhel.6-x64.txt
[rhel-6-targz-main]: https://aka.ms/dotnet/6.0/daily/dotnet-sdk-rhel.6-x64.tar.gz
[rhel-6-targz-checksum-main]: https://aka.ms/dotnet/6.0/daily/dotnet-sdk-rhel.6-x64.tar.gz.sha

[rhel-6-badge-5.0.1XX-rtm]: https://aka.ms/dotnet/net5/5.0.1xx/daily/Sdk/rhel.6_x64_Release_version_badge.svg
[rhel-6-version-5.0.1XX-rtm]: https://aka.ms/dotnet/net5/5.0.1xx/daily/Sdk/productCommit-rhel.6-x64.txt
[rhel-6-targz-5.0.1XX-rtm]: https://aka.ms/dotnet/net5/5.0.1xx/daily/Sdk/dotnet-sdk-rhel.6-x64.tar.gz
[rhel-6-targz-checksum-5.0.1XX-rtm]: https://aka.ms/dotnet/net5/5.0.1xx/daily/Sdk/dotnet-sdk-rhel.6-x64.tar.gz.sha

[rhel-6-badge-5.0.1XX-rc2]: https://aka.ms/dotnet/net5/rc2/Sdk/rhel.6_x64_Release_version_badge.svg
[rhel-6-version-5.0.1XX-rc2]: https://aka.ms/dotnet/net5/rc2/Sdk/productCommit-rhel.6-x64.txt
[rhel-6-targz-5.0.1XX-rc2]: https://aka.ms/dotnet/net5/rc2/Sdk/dotnet-sdk-rhel.6-x64.tar.gz
[rhel-6-targz-checksum-5.0.1XX-rc2]: https://aka.ms/dotnet/net5/rc2/Sdk/dotnet-sdk-rhel.6-x64.tar.gz.sha

[rhel-6-badge-3.1.4XX]: https://dotnetcli.blob.core.windows.net/dotnet/Sdk/release/3.1.4xx/rhel.6_x64_Release_version_badge.svg
[rhel-6-version-3.1.4XX]: https://dotnetcli.blob.core.windows.net/dotnet/Sdk/release/3.1.4xx/latest.version
[rhel-6-targz-3.1.4XX]: https://dotnetcli.blob.core.windows.net/dotnet/Sdk/release/3.1.4xx/dotnet-sdk-latest-rhel.6-x64.tar.gz
[rhel-6-targz-checksum-3.1.4XX]: https://dotnetclichecksums.blob.core.windows.net/dotnet/Sdk/release/3.1.4xx/dotnet-sdk-latest-rhel.6-x64.tar.gz.sha

[rhel-6-badge-3.1.1XX]: https://dotnetcli.blob.core.windows.net/dotnet/Sdk/release/3.1.1xx/rhel.6_x64_Release_version_badge.svg
[rhel-6-version-3.1.1XX]: https://dotnetcli.blob.core.windows.net/dotnet/Sdk/release/3.1.1xx/latest.version
[rhel-6-targz-3.1.1XX]: https://dotnetcli.blob.core.windows.net/dotnet/Sdk/release/3.1.1xx/dotnet-sdk-latest-rhel.6-x64.tar.gz
[rhel-6-targz-checksum-3.1.1XX]: https://dotnetclichecksums.blob.core.windows.net/dotnet/Sdk/release/3.1.1xx/dotnet-sdk-latest-rhel.6-x64.tar.gz.sha

[linux-musl-x64-badge-main]: https://aka.ms/dotnet/6.0/daily/linux_musl_x64_Release_version_badge.svg
[linux-musl-x64-version-main]: https://aka.ms/dotnet/6.0/daily/productCommit-linux-musl-x64.txt
[linux-musl-x64-targz-main]: https://aka.ms/dotnet/6.0/daily/dotnet-sdk-linux-musl-x64.tar.gz
[linux-musl-x64-targz-checksum-main]: https://aka.ms/dotnet/6.0/daily/dotnet-sdk-linux-musl-x64.tar.gz.sha

[linux-musl-x64-badge-5.0.1XX-rtm]: https://aka.ms/dotnet/net5/5.0.1xx/daily/Sdk/linux_musl_x64_Release_version_badge.svg
[linux-musl-x64-version-5.0.1XX-rtm]: https://aka.ms/dotnet/net5/5.0.1xx/daily/Sdk/productCommit-linux-musl-x64.txt
[linux-musl-x64-targz-5.0.1XX-rtm]: https://aka.ms/dotnet/net5/5.0.1xx/daily/Sdk/dotnet-sdk-linux-musl-x64.tar.gz
[linux-musl-x64-targz-checksum-5.0.1XX-rtm]: https://aka.ms/dotnet/net5/5.0.1xx/daily/Sdk/dotnet-sdk-linux-musl-x64.tar.gz.sha

[linux-musl-x64-badge-5.0.1XX-rc2]: https://aka.ms/dotnet/net5/rc2/Sdk/linux_musl_x64_Release_version_badge.svg
[linux-musl-x64-version-5.0.1XX-rc2]: https://aka.ms/dotnet/net5/rc2/Sdk/productCommit-linux-musl-x64.txt
[linux-musl-x64-targz-5.0.1XX-rc2]: https://aka.ms/dotnet/net5/rc2/Sdk/dotnet-sdk-linux-musl-x64.tar.gz
[linux-musl-x64-targz-checksum-5.0.1XX-rc2]: https://aka.ms/dotnet/net5/rc2/Sdk/dotnet-sdk-linux-musl-x64.tar.gz.sha

[linux-musl-x64-badge-3.1.4XX]: https://dotnetcli.blob.core.windows.net/dotnet/Sdk/release/3.1.4xx/linux_musl_x64_Release_version_badge.svg
[linux-musl-x64-version-3.1.4XX]: https://dotnetcli.blob.core.windows.net/dotnet/Sdk/release/3.1.4xx/latest.version
[linux-musl-x64-targz-3.1.4XX]: https://dotnetcli.blob.core.windows.net/dotnet/Sdk/release/3.1.4xx/dotnet-sdk-latest-linux-musl-x64.tar.gz
[linux-musl-x64-targz-checksum-3.1.4XX]: https://dotnetclichecksums.blob.core.windows.net/dotnet/Sdk/release/3.1.4xx/dotnet-sdk-latest-linux-musl-x64.tar.gz.sha

[linux-musl-x64-badge-3.1.1XX]: https://dotnetcli.blob.core.windows.net/dotnet/Sdk/release/3.1.1xx/linux_musl_x64_Release_version_badge.svg
[linux-musl-x64-version-3.1.1XX]: https://dotnetcli.blob.core.windows.net/dotnet/Sdk/release/3.1.1xx/latest.version
[linux-musl-x64-targz-3.1.1XX]: https://dotnetcli.blob.core.windows.net/dotnet/Sdk/release/3.1.1xx/dotnet-sdk-latest-linux-musl-x64.tar.gz
[linux-musl-x64-targz-checksum-3.1.1XX]: https://dotnetclichecksums.blob.core.windows.net/dotnet/Sdk/release/3.1.1xx/dotnet-sdk-latest-linux-musl-x64.tar.gz.sha

[linux-musl-arm-badge-main]: https://aka.ms/dotnet/6.0/daily/linux_musl_arm_Release_version_badge.svg
[linux-musl-arm-version-main]: https://aka.ms/dotnet/6.0/daily/productCommit-linux-musl-arm.txt
[linux-musl-arm-targz-main]: https://aka.ms/dotnet/6.0/daily/dotnet-sdk-linux-musl-arm.tar.gz
[linux-musl-arm-targz-checksum-main]: https://aka.ms/dotnet/6.0/daily/dotnet-sdk-linux-musl-arm.tar.gz.sha

[linux-musl-arm64-badge-main]: https://aka.ms/dotnet/6.0/daily/linux_musl_arm64_Release_version_badge.svg
[linux-musl-arm64-version-main]: https://aka.ms/dotnet/6.0/daily/productCommit-linux-musl-arm64.txt
[linux-musl-arm64-targz-main]: https://aka.ms/dotnet/6.0/daily/dotnet-sdk-linux-musl-arm64.tar.gz
[linux-musl-arm64-targz-checksum-main]: https://aka.ms/dotnet/6.0/daily/dotnet-sdk-linux-musl-arm64.tar.gz.sha

[win-arm-badge-main]: https://aka.ms/dotnet/6.0/daily/win_arm_Release_version_badge.svg
[win-arm-version-main]: https://aka.ms/dotnet/6.0/daily/productCommit-win-arm.txt
[win-arm-zip-main]: https://aka.ms/dotnet/6.0/daily/dotnet-sdk-win-arm.zip
[win-arm-zip-checksum-main]: https://aka.ms/dotnet/6.0/daily/dotnet-sdk-win-arm.zip.sha

[win-arm-badge-5.0.1XX-rtm]: https://aka.ms/dotnet/net5/5.0.1xx/daily/Sdk/win_arm_Release_version_badge.svg
[win-arm-version-5.0.1XX-rtm]: https://aka.ms/dotnet/net5/5.0.1xx/daily/Sdk/productCommit-win-arm.txt
[win-arm-zip-5.0.1XX-rtm]: https://aka.ms/dotnet/net5/5.0.1xx/daily/Sdk/dotnet-sdk-win-arm.zip
[win-arm-zip-checksum-5.0.1XX-rtm]: https://aka.ms/dotnet/net5/5.0.1xx/daily/Sdk/dotnet-sdk-win-arm.zip.sha

[win-arm-badge-5.0.1XX-rc2]: https://aka.ms/dotnet/net5/rc2/Sdk/win_arm_Release_version_badge.svg
[win-arm-version-5.0.1XX-rc2]: https://aka.ms/dotnet/net5/rc2/Sdk/productCommit-win-arm.txt
[win-arm-zip-5.0.1XX-rc2]: https://aka.ms/dotnet/net5/rc2/Sdk/dotnet-sdk-win-arm.zip
[win-arm-zip-checksum-5.0.1XX-rc2]: https://aka.ms/dotnet/net5/rc2/Sdk/dotnet-sdk-win-arm.zip.sha

[win-arm-badge-3.1.4XX]: https://dotnetcli.blob.core.windows.net/dotnet/Sdk/release/3.1.4xx/win_arm_Release_version_badge.svg
[win-arm-version-3.1.4XX]: https://dotnetcli.blob.core.windows.net/dotnet/Sdk/release/3.1.4xx/latest.version
[win-arm-zip-3.1.4XX]: https://dotnetcli.blob.core.windows.net/dotnet/Sdk/release/3.1.4xx/dotnet-sdk-latest-win-arm.zip
[win-arm-zip-checksum-3.1.4XX]: https://dotnetclichecksums.blob.core.windows.net/dotnet/Sdk/release/3.1.4xx/dotnet-sdk-latest-win-arm.zip.sha

[win-arm-badge-3.1.1XX]: https://dotnetcli.blob.core.windows.net/dotnet/Sdk/release/3.1.1xx/win_arm_Release_version_badge.svg
[win-arm-version-3.1.1XX]: https://dotnetcli.blob.core.windows.net/dotnet/Sdk/release/3.1.1xx/latest.version
[win-arm-zip-3.1.1XX]: https://dotnetcli.blob.core.windows.net/dotnet/Sdk/release/3.1.1xx/dotnet-sdk-latest-win-arm.zip
[win-arm-zip-checksum-3.1.1XX]: https://dotnetclichecksums.blob.core.windows.net/dotnet/Sdk/release/3.1.1xx/dotnet-sdk-latest-win-arm.zip.sha

[win-arm64-badge-main]: https://aka.ms/dotnet/6.0/daily/win_arm64_Release_version_badge.svg
[win-arm64-version-main]: https://aka.ms/dotnet/6.0/daily/productCommit-win-arm64.txt
[win-arm64-installer-main]: https://aka.ms/dotnet/6.0/daily/dotnet-sdk-win-arm64.exe
[win-arm64-installer-checksum-main]: https://aka.ms/dotnet/6.0/daily/dotnet-sdk-win-arm64.exe.sha
[win-arm64-zip-main]: https://aka.ms/dotnet/6.0/daily/dotnet-sdk-win-arm64.zip
[win-arm64-zip-checksum-main]: https://aka.ms/dotnet/6.0/daily/dotnet-sdk-win-arm64.zip.sha

[win-arm64-badge-5.0.1XX-rtm]: https://aka.ms/dotnet/net5/5.0.1xx/daily/Sdk/win_arm64_Release_version_badge.svg
[win-arm64-version-5.0.1XX-rtm]: https://aka.ms/dotnet/net5/5.0.1xx/daily/Sdk/productCommit-win-arm64.txt
[win-arm64-installer-5.0.1XX-rtm]: https://aka.ms/dotnet/net5/5.0.1xx/daily/Sdk/dotnet-sdk-win-arm64.exe
[win-arm64-installer-checksum-5.0.1XX-rtm]: https://aka.ms/dotnet/net5/5.0.1xx/daily/Sdk/dotnet-sdk-win-arm64.exe.sha
[win-arm64-zip-5.0.1XX-rtm]: https://aka.ms/dotnet/net5/5.0.1xx/daily/Sdk/dotnet-sdk-win-arm64.zip
[win-arm64-zip-checksum-5.0.1XX-rtm]: https://aka.ms/dotnet/net5/5.0.1xx/daily/Sdk/dotnet-sdk-win-arm64.zip.sha

[win-arm64-badge-5.0.1XX-rc2]: https://aka.ms/dotnet/net5/rc2/Sdk/win_arm64_Release_version_badge.svg
[win-arm64-version-5.0.1XX-rc2]: https://aka.ms/dotnet/net5/rc2/Sdk/productCommit-win-arm64.txt
[win-arm64-installer-5.0.1XX-rc2]: https://aka.ms/dotnet/net5/rc2/Sdk/dotnet-sdk-win-arm64.exe
[win-arm64-installer-checksum-5.0.1XX-rc2]: https://aka.ms/dotnet/net5/rc2/Sdk/dotnet-sdk-win-arm64.exe.sha
[win-arm64-zip-5.0.1XX-rc2]: https://aka.ms/dotnet/net5/rc2/Sdk/dotnet-sdk-win-arm64.zip
[win-arm64-zip-checksum-5.0.1XX-rc2]: https://aka.ms/dotnet/net5/rc2/Sdk/dotnet-sdk-win-arm64.zip.sha

[sdk-shas-2.2.1XX]: https://github.com/dotnet/versions/tree/master/build-info/dotnet/product/cli/release/2.2#built-repositories"""
