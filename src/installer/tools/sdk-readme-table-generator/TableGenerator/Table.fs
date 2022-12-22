module TableGenerator.Table

open System
open TableGenerator.Shared

let notAvailable = "**N/A**"

let windowsDesktopArchTableTemplate =
    """[![][{0}-badge-{1}]][{0}-version-{1}]<br>[Installer][{0}-installer-{1}] - [Checksum][{0}-installer-checksum-{1}]<br>[zip][{0}-zip-{1}] - [Checksum][{0}-zip-checksum-{1}]"""

let linuxArmTableTemplate =
    "[![][{0}-badge-{1}]][{0}-version-{1}]<br>[tar.gz][{0}-targz-{1}] - [Checksum][{0}-targz-checksum-{1}]"

let osxDesktopArchTableTemplate =
    """[![][{0}-badge-{1}]][{0}-version-{1}]<br>[Installer][{0}-installer-{1}] - [Checksum][{0}-installer-checksum-{1}]<br>[tar.gz][{0}-targz-{1}] - [Checksum][{0}-targz-checksum-{1}]"""

let joinListOfArchs (listOfArchs: List<string>): string = "| " + String.Join(" | ", listOfArchs) + " |"

let formRow rowTitle tableTemplateForThisArch branches =
    let inList =
        List.concat
            [ [ rowTitle ]
              (branches |> List.map tableTemplateForThisArch) ]
    joinListOfArchs inList

let windowsX64Row branches =
    let tableTemplateForThisArch branch =
        String.Format(windowsDesktopArchTableTemplate, "win-x64", branchNameShorten branch)
    formRow "**Windows x64**" tableTemplateForThisArch branches

let windowsX86Row branches =
    let tableTemplateForThisArch branch =
        String.Format(windowsDesktopArchTableTemplate, "win-x86", branchNameShorten branch)
    formRow "**Windows x86**" tableTemplateForThisArch branches

let osxX64Row branches =
    let tableTemplateForThisArch branch = String.Format(osxDesktopArchTableTemplate, "osx-x64", branchNameShorten branch)
    formRow "**macOS x64**" tableTemplateForThisArch branches

let osxArm64Row branches =
    let format branch = String.Format(osxDesktopArchTableTemplate, "osx-arm64", branchNameShorten branch)
    let tableTemplateForThisArch branch =
        match getMajorMinor branch with
        | Main -> format branch
        | MajorMinor { Major = major; Minor = _minor } when major >= 6 -> format branch
        | _ -> notAvailable
    formRow "**macOS arm64**" tableTemplateForThisArch branches

let linuxDesktopArchRow branches =
    let tableTemplate =
        """[![][linux-badge-{0}]][linux-version-{0}]<br>[DEB Installer][linux-DEB-installer-{0}] - [Checksum][linux-DEB-installer-checksum-{0}]<br>[RPM Installer][linux-RPM-installer-{0}] - [Checksum][linux-RPM-installer-checksum-{0}]<br>_see installer note below_<sup>1</sup><br>[tar.gz][linux-targz-{0}] - [Checksum][linux-targz-checksum-{0}]"""
    let tableTemplateForThisArch branch = String.Format(tableTemplate, branchNameShorten branch)
    formRow "**Linux x64**" tableTemplateForThisArch branches

let linuxArmRow branches =
    let tableTemplateForThisArch branch = String.Format(linuxArmTableTemplate, "linux-arm", branchNameShorten branch)
    formRow "**Linux arm**" tableTemplateForThisArch branches

let linuxArm64Row branches =
    let tableTemplateForThisArch branch = String.Format(linuxArmTableTemplate, "linux-arm64", branchNameShorten branch)
    formRow "**Linux arm64**" tableTemplateForThisArch branches

let rhel6Row branches =
    let tableTemplateForThisArch branch =
        match getMajorMinor branch with
        | NoVersion -> notAvailable
        | Main -> notAvailable
        | MajorMinor { Major = major; Minor = minor } when major >= 5 -> notAvailable
        | _ -> String.Format(linuxArmTableTemplate, "rhel-6", branchNameShorten branch)
    formRow "**RHEL 6**" tableTemplateForThisArch branches

let linuxMuslRowX64 branches =
    let format branch = String.Format(linuxArmTableTemplate, "linux-musl-x64", branchNameShorten branch)
    let tableTemplateForThisArch branch = format branch
    formRow "**Linux-musl-x64**" tableTemplateForThisArch branches

let linuxMuslRowArm branches =
    let format branch = String.Format(linuxArmTableTemplate, "linux-musl-arm", branchNameShorten branch)
    let tableTemplateForThisArch branch =
        match getMajorMinor branch with
        | Main -> format branch
        | MajorMinor { Major = major; Minor = _minor; Patch = patch } when major >= 6 || (major >= 5 && patch >= 299) -> format branch
        | _ -> notAvailable
    formRow "**Linux-musl-arm**" tableTemplateForThisArch branches

let linuxMuslRowArm64 branches =
    let format branch = String.Format(linuxArmTableTemplate, "linux-musl-arm64", branchNameShorten branch)
    let tableTemplateForThisArch branch =
        match getMajorMinor branch with
        | Main -> format branch
        | MajorMinor { Major = major; Minor = _minor; Patch = patch } when major >= 6 || (major >= 5 && patch >= 299) -> format branch
        | _ -> notAvailable
    formRow "**Linux-musl-arm64**" tableTemplateForThisArch branches

let windowsArmRow branches =
    let tableTemplate =
        "[![][win-arm-badge-{0}]][win-arm-version-{0}]<br>[zip][win-arm-zip-{0}] - [Checksum][win-arm-zip-checksum-{0}]"

    let tableTemplateForThisArch branch =
        match getMajorMinor branch with
        | NoVersion -> notAvailable
        | Main -> notAvailable
        | MajorMinor { Major = major; Minor = minor } when  ( major >= 5) -> notAvailable
        | _ -> String.Format(tableTemplate, branchNameShorten branch)
    formRow "**Windows arm**" tableTemplateForThisArch branches

let windowsArm64Row branches =
    let tableTemplate =
        """[![][win-arm64-badge-{0}]][win-arm64-version-{0}]<br>[Installer][win-arm64-installer-{0}] - [Checksum][win-arm64-installer-checksum-{0}]<br>[zip][win-arm64-zip-{0}]"""

    let tableTemplateForThisArch branch =
        match getMajorMinor branch with
        | NoVersion -> notAvailable
        | MajorMinor { Major = major; Minor = minor; Release = release; } when major <= 3 -> notAvailable
        | MajorMinor { Major = major; Minor = minor; Release = release; } when major = 5 -> String.Format(tableTemplate, branchNameShorten branch)
        | _ -> String.Format(tableTemplate, branchNameShorten branch)
    formRow "**Windows arm64**" tableTemplateForThisArch branches

let titleRow = formRow "Platform" (fun (b: Branch) -> b.DisplayName)

let separator = formRow ":---------" (fun _ -> ":----------:")

let rows =
    [ titleRow
      separator
      windowsX64Row
      windowsX86Row
      windowsArmRow
      windowsArm64Row
      osxX64Row
      osxArm64Row
      linuxDesktopArchRow
      linuxArmRow
      linuxArm64Row
      linuxMuslRowX64
      linuxMuslRowArm
      linuxMuslRowArm64
      rhel6Row
    ]

let table branches = String.Join(Environment.NewLine, rows |> List.map (fun row -> row branches))
