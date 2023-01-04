module TableGenerator.Reference

open System
open TableGenerator.Shared

let referenceTemplate: ReferenceTemplate = {
    LegacyTemplate = """[{0}-badge-{1}]: https://dotnetcli.blob.core.windows.net/dotnet/Sdk/{2}/{3}_Release_version_badge.svg?no-cache
[{0}-version-{1}]: https://dotnetcli.blob.core.windows.net/dotnet/Sdk/{2}/latest.version
[{0}-installer-{1}]: https://dotnetcli.blob.core.windows.net/dotnet/Sdk/{2}/dotnet-sdk-latest-{0}.exe
[{0}-installer-checksum-{1}]: https://dotnetclichecksums.blob.core.windows.net/dotnet/Sdk/{2}/dotnet-sdk-latest-{0}.exe.sha
[{0}-zip-{1}]: https://dotnetcli.blob.core.windows.net/dotnet/Sdk/{2}/dotnet-sdk-latest-{0}.zip
[{0}-zip-checksum-{1}]: https://dotnetclichecksums.blob.core.windows.net/dotnet/Sdk/{2}/dotnet-sdk-latest-{0}.zip.sha"""

    AkaMSLegacyTemplate = """[{0}-badge-{1}]: https://aka.ms/dotnet/{4}/Sdk/{3}_Release_version_badge.svg?no-cache
[{0}-version-{1}]: https://aka.ms/dotnet/{4}/Sdk/productCommit-{0}.txt
[{0}-installer-{1}]: https://aka.ms/dotnet/{4}/Sdk/dotnet-sdk-{0}.exe
[{0}-installer-checksum-{1}]: https://aka.ms/dotnet/{4}/Sdk/dotnet-sdk-{0}.exe.sha
[{0}-zip-{1}]: https://aka.ms/dotnet/{4}/Sdk/dotnet-sdk-{0}.zip
[{0}-zip-checksum-{1}]: https://aka.ms/dotnet/{4}/Sdk/dotnet-sdk-{0}.zip.sha"""

    AkaMSTemplate = """[{0}-badge-{1}]: https://aka.ms/dotnet/{4}/{3}_Release_version_badge.svg?no-cache
[{0}-version-{1}]: https://aka.ms/dotnet/{4}/productCommit-{0}.txt
[{0}-installer-{1}]: https://aka.ms/dotnet/{4}/dotnet-sdk-{0}.exe
[{0}-installer-checksum-{1}]: https://aka.ms/dotnet/{4}/dotnet-sdk-{0}.exe.sha
[{0}-zip-{1}]: https://aka.ms/dotnet/{4}/dotnet-sdk-{0}.zip
[{0}-zip-checksum-{1}]: https://aka.ms/dotnet/{4}/dotnet-sdk-{0}.zip.sha"""
}

let targzReferenceTemplate: ReferenceTemplate = {
    LegacyTemplate = """[{0}-badge-{1}]: https://dotnetcli.blob.core.windows.net/dotnet/Sdk/{2}/{3}_Release_version_badge.svg?no-cache
[{0}-version-{1}]: https://dotnetcli.blob.core.windows.net/dotnet/Sdk/{2}/latest.version
[{0}-targz-{1}]: https://dotnetcli.blob.core.windows.net/dotnet/Sdk/{2}/dotnet-sdk-latest-{0}.tar.gz
[{0}-targz-checksum-{1}]: https://dotnetclichecksums.blob.core.windows.net/dotnet/Sdk/{2}/dotnet-sdk-latest-{0}.tar.gz.sha"""

    AkaMSLegacyTemplate = """[{0}-badge-{1}]: https://aka.ms/dotnet/{4}/Sdk/{3}_Release_version_badge.svg?no-cache
[{0}-version-{1}]: https://aka.ms/dotnet/{4}/Sdk/productCommit-{0}.txt
[{0}-targz-{1}]: https://aka.ms/dotnet/{4}/Sdk/dotnet-sdk-{0}.tar.gz
[{0}-targz-checksum-{1}]: https://aka.ms/dotnet/{4}/Sdk/dotnet-sdk-{0}.tar.gz.sha"""

    AkaMSTemplate = """[{0}-badge-{1}]: https://aka.ms/dotnet/{4}/{3}_Release_version_badge.svg?no-cache
[{0}-version-{1}]: https://aka.ms/dotnet/{4}/productCommit-{0}.txt
[{0}-targz-{1}]: https://aka.ms/dotnet/{4}/dotnet-sdk-{0}.tar.gz
[{0}-targz-checksum-{1}]: https://aka.ms/dotnet/{4}/dotnet-sdk-{0}.tar.gz.sha""" }

let linuxArmNoArchitectureReferenceTemplate: ReferenceTemplate = {
    LegacyTemplate = """[{0}-badge-{1}]: https://dotnetcli.blob.core.windows.net/dotnet/Sdk/{2}/{3}_Release_version_badge.svg?no-cache
[{0}-version-{1}]: https://dotnetcli.blob.core.windows.net/dotnet/Sdk/{2}/latest.version
[{0}-targz-{1}]: https://dotnetcli.blob.core.windows.net/dotnet/Sdk/{2}/dotnet-sdk-latest-{0}.tar.gz
[{0}-targz-checksum-{1}]: https://dotnetclichecksums.blob.core.windows.net/dotnet/Sdk/{2}/dotnet-sdk-latest-{0}.tar.gz.sha"""

    AkaMSLegacyTemplate = """[{0}-badge-{1}]: https://aka.ms/dotnet/{4}/Sdk/{3}_Release_version_badge.svg?no-cache
[{0}-version-{1}]: https://aka.ms/dotnet/{4}/Sdk/productCommit-{0}.txt
[{0}-targz-{1}]: https://aka.ms/dotnet/{4}/Sdk/dotnet-sdk-{0}.tar.gz
[{0}-targz-checksum-{1}]: https://aka.ms/dotnet/{4}/Sdk/dotnet-sdk-{0}.tar.gz.sha"""

    AkaMSTemplate = """[{0}-badge-{1}]: https://aka.ms/dotnet/{4}/{3}_Release_version_badge.svg?no-cache
[{0}-version-{1}]: https://aka.ms/dotnet/{4}/productCommit-{0}.txt
[{0}-targz-{1}]: https://aka.ms/dotnet/{4}/dotnet-sdk-{0}.tar.gz
[{0}-targz-checksum-{1}]: https://aka.ms/dotnet/{4}/dotnet-sdk-{0}.tar.gz.sha""" }

let osxReferenceTemplate: ReferenceTemplate = {
    LegacyTemplate = """[{0}-badge-{1}]: https://dotnetcli.blob.core.windows.net/dotnet/Sdk/{2}/{3}_Release_version_badge.svg?no-cache
[{0}-version-{1}]: https://dotnetcli.blob.core.windows.net/dotnet/Sdk/{2}/latest.version
[{0}-installer-{1}]: https://dotnetcli.blob.core.windows.net/dotnet/Sdk/{2}/dotnet-sdk-latest-{0}.pkg
[{0}-installer-checksum-{1}]: https://dotnetclichecksums.blob.core.windows.net/dotnet/Sdk/{2}/dotnet-sdk-latest-{0}.pkg.sha
[{0}-targz-{1}]: https://dotnetcli.blob.core.windows.net/dotnet/Sdk/{2}/dotnet-sdk-latest-{0}.tar.gz
[{0}-targz-checksum-{1}]: https://dotnetclichecksums.blob.core.windows.net/dotnet/Sdk/{2}/dotnet-sdk-latest-{0}.tar.gz.sha"""

    AkaMSLegacyTemplate = """[{0}-badge-{1}]: https://aka.ms/dotnet/{4}/Sdk/{3}_Release_version_badge.svg?no-cache
[{0}-version-{1}]: https://aka.ms/dotnet/{4}/Sdk/productCommit-{0}.txt
[{0}-installer-{1}]: https://aka.ms/dotnet/{4}/Sdk/dotnet-sdk-{0}.pkg
[{0}-installer-checksum-{1}]: https://aka.ms/dotnet/{4}/Sdk/dotnet-sdk-{0}.pkg.sha
[{0}-targz-{1}]: https://aka.ms/dotnet/{4}/Sdk/dotnet-sdk-{0}.tar.gz
[{0}-targz-checksum-{1}]: https://aka.ms/dotnet/{4}/Sdk/dotnet-sdk-{0}.pkg.tar.gz.sha"""

    AkaMSTemplate = """[{0}-badge-{1}]: https://aka.ms/dotnet/{4}/{3}_Release_version_badge.svg?no-cache
[{0}-version-{1}]: https://aka.ms/dotnet/{4}/productCommit-{0}.txt
[{0}-installer-{1}]: https://aka.ms/dotnet/{4}/dotnet-sdk-{0}.pkg
[{0}-installer-checksum-{1}]: https://aka.ms/dotnet/{4}/dotnet-sdk-{0}.pkg.sha
[{0}-targz-{1}]: https://aka.ms/dotnet/{4}/dotnet-sdk-{0}.tar.gz
[{0}-targz-checksum-{1}]: https://aka.ms/dotnet/{4}/dotnet-sdk-{0}.pkg.tar.gz.sha"""
}

let linuxReferenceTemplate: ReferenceTemplate = {
    LegacyTemplate = """[linux-badge-{1}]: https://dotnetcli.blob.core.windows.net/dotnet/Sdk/{2}/{3}_Release_version_badge.svg?no-cache
[linux-version-{1}]: https://dotnetcli.blob.core.windows.net/dotnet/Sdk/{2}/latest.version
[linux-DEB-installer-{1}]: https://dotnetcli.blob.core.windows.net/dotnet/Sdk/{2}/dotnet-sdk-latest-x64.deb
[linux-DEB-installer-checksum-{1}]: https://dotnetclichecksums.blob.core.windows.net/dotnet/Sdk/{2}/dotnet-sdk-latest-x64.deb.sha
[linux-RPM-installer-{1}]: https://dotnetcli.blob.core.windows.net/dotnet/Sdk/{2}/dotnet-sdk-latest-x64.rpm
[linux-RPM-installer-checksum-{1}]: https://dotnetclichecksums.blob.core.windows.net/dotnet/Sdk/{2}/dotnet-sdk-latest-x64.rpm.sha
[linux-targz-{1}]: https://dotnetcli.blob.core.windows.net/dotnet/Sdk/{2}/dotnet-sdk-latest-{0}.tar.gz
[linux-targz-checksum-{1}]: https://dotnetclichecksums.blob.core.windows.net/dotnet/Sdk/{2}/dotnet-sdk-latest-{0}.tar.gz.sha"""

    AkaMSLegacyTemplate = """[linux-badge-{1}]: https://aka.ms/dotnet/{4}/Sdk/{3}_Release_version_badge.svg?no-cache
[linux-version-{1}]: https://aka.ms/dotnet/{4}/Sdk/productCommit-{0}.txt
[linux-DEB-installer-{1}]: https://aka.ms/dotnet/{4}/Sdk/dotnet-sdk-x64.deb
[linux-DEB-installer-checksum-{1}]: https://aka.ms/dotnet/{4}/Sdk/dotnet-sdk-x64.deb.sha
[linux-RPM-installer-{1}]: https://aka.ms/dotnet/{4}/Sdk/dotnet-sdk-x64.rpm
[linux-RPM-installer-checksum-{1}]: https://aka.ms/dotnet/{4}/Sdk/dotnet-sdk-x64.rpm.sha
[linux-targz-{1}]: https://aka.ms/dotnet/{4}/Sdk/dotnet-sdk-{0}.tar.gz
[linux-targz-checksum-{1}]: https://aka.ms/dotnet/{4}/Sdk/dotnet-sdk-{0}.tar.gz.sha"""

    AkaMSTemplate = """[linux-badge-{1}]: https://aka.ms/dotnet/{4}/{3}_Release_version_badge.svg?no-cache
[linux-version-{1}]: https://aka.ms/dotnet/{4}/productCommit-{0}.txt
[linux-DEB-installer-{1}]: https://aka.ms/dotnet/{4}/dotnet-sdk-x64.deb
[linux-DEB-installer-checksum-{1}]: https://aka.ms/dotnet/{4}/dotnet-sdk-x64.deb.sha
[linux-RPM-installer-{1}]: https://aka.ms/dotnet/{4}/dotnet-sdk-x64.rpm
[linux-RPM-installer-checksum-{1}]: https://aka.ms/dotnet/{4}/dotnet-sdk-x64.rpm.sha
[linux-targz-{1}]: https://aka.ms/dotnet/{4}/dotnet-sdk-{0}.tar.gz
[linux-targz-checksum-{1}]: https://aka.ms/dotnet/{4}/dotnet-sdk-{0}.tar.gz.sha"""
}

let rhel6ReferenceTemplate: ReferenceTemplate = {
    LegacyTemplate = """[rhel-6-badge-{1}]: https://dotnetcli.blob.core.windows.net/dotnet/Sdk/{2}/{3}_Release_version_badge.svg?no-cache
[rhel-6-version-{1}]: https://dotnetcli.blob.core.windows.net/dotnet/Sdk/{2}/latest.version
[rhel-6-targz-{1}]: https://dotnetcli.blob.core.windows.net/dotnet/Sdk/{2}/dotnet-sdk-latest-{0}.tar.gz
[rhel-6-targz-checksum-{1}]: https://dotnetclichecksums.blob.core.windows.net/dotnet/Sdk/{2}/dotnet-sdk-latest-{0}.tar.gz.sha"""
    
    AkaMSLegacyTemplate = """[rhel-6-badge-{1}]: https://aka.ms/dotnet/{4}/Sdk/{3}_Release_version_badge.svg?no-cache
[rhel-6-version-{1}]: https://aka.ms/dotnet/{4}/Sdk/productCommit-{0}.txt
[rhel-6-targz-{1}]: https://aka.ms/dotnet/{4}/Sdk/dotnet-sdk-{0}.tar.gz
[rhel-6-targz-checksum-{1}]: https://aka.ms/dotnet/{4}/Sdk/dotnet-sdk-{0}.tar.gz.sha"""

    AkaMSTemplate = """[rhel-6-badge-{1}]: https://aka.ms/dotnet/{4}/{3}_Release_version_badge.svg?no-cache
[rhel-6-version-{1}]: https://aka.ms/dotnet/{4}/productCommit-{0}.txt
[rhel-6-targz-{1}]: https://aka.ms/dotnet/{4}/dotnet-sdk-{0}.tar.gz
[rhel-6-targz-checksum-{1}]: https://aka.ms/dotnet/{4}/dotnet-sdk-{0}.tar.gz.sha"""
}

let linuxMuslReferenceTemplate: ReferenceTemplate = {
    LegacyTemplate = """[{0}-badge-{1}]: https://dotnetcli.blob.core.windows.net/dotnet/Sdk/{2}/{3}_Release_version_badge.svg?no-cache
[{0}-version-{1}]: https://dotnetcli.blob.core.windows.net/dotnet/Sdk/{2}/latest.version
[{0}-targz-{1}]: https://dotnetcli.blob.core.windows.net/dotnet/Sdk/{2}/dotnet-sdk-latest-{0}.tar.gz
[{0}-targz-checksum-{1}]: https://dotnetclichecksums.blob.core.windows.net/dotnet/Sdk/{2}/dotnet-sdk-latest-{0}.tar.gz.sha"""

    AkaMSLegacyTemplate = """[{0}-badge-{1}]: https://aka.ms/dotnet/{4}/Sdk/{3}_Release_version_badge.svg?no-cache
[{0}-version-{1}]: https://aka.ms/dotnet/{4}/Sdk/productCommit-{0}.txt
[{0}-targz-{1}]: https://aka.ms/dotnet/{4}/Sdk/dotnet-sdk-{0}.tar.gz
[{0}-targz-checksum-{1}]: https://aka.ms/dotnet/{4}/Sdk/dotnet-sdk-{0}.tar.gz.sha"""

    AkaMSTemplate = """[{0}-badge-{1}]: https://aka.ms/dotnet/{4}/{3}_Release_version_badge.svg?no-cache
[{0}-version-{1}]: https://aka.ms/dotnet/{4}/productCommit-{0}.txt
[{0}-targz-{1}]: https://aka.ms/dotnet/{4}/dotnet-sdk-{0}.tar.gz
[{0}-targz-checksum-{1}]: https://aka.ms/dotnet/{4}/dotnet-sdk-{0}.tar.gz.sha"""
}

let winMuslReferenceTemplate: ReferenceTemplate = {
    LegacyTemplate = """[{0}-badge-{1}]: https://dotnetcli.blob.core.windows.net/dotnet/Sdk/{2}/{3}_Release_version_badge.svg?no-cache
[{0}-version-{1}]: https://dotnetcli.blob.core.windows.net/dotnet/Sdk/{2}/latest.version
[{0}-zip-{1}]: https://dotnetcli.blob.core.windows.net/dotnet/Sdk/{2}/dotnet-sdk-latest-{0}.zip
[{0}-zip-checksum-{1}]: https://dotnetclichecksums.blob.core.windows.net/dotnet/Sdk/{2}/dotnet-sdk-latest-{0}.zip.sha"""

    AkaMSLegacyTemplate = """[{0}-badge-{1}]: https://aka.ms/dotnet/{4}/Sdk/{3}_Release_version_badge.svg?no-cache
[{0}-version-{1}]: https://aka.ms/dotnet/{4}/Sdk/productCommit-{0}.txt
[{0}-zip-{1}]: https://aka.ms/dotnet/{4}/Sdk/dotnet-sdk-{0}.zip
[{0}-zip-checksum-{1}]: https://aka.ms/dotnet/{4}/Sdk/dotnet-sdk-{0}.zip.sha"""

    AkaMSTemplate = """[{0}-badge-{1}]: https://aka.ms/dotnet/{4}/{3}_Release_version_badge.svg?no-cache
[{0}-version-{1}]: https://aka.ms/dotnet/{4}/productCommit-{0}.txt
[{0}-zip-{1}]: https://aka.ms/dotnet/{4}/dotnet-sdk-{0}.zip
[{0}-zip-checksum-{1}]: https://aka.ms/dotnet/{4}/dotnet-sdk-{0}.zip.sha"""
}

let formatTemplate (platform: String) (template: ReferenceTemplate) (branch: Branch): Option<string> =
    if branch.AkaMsChannel <> None then
        let useLegacyAkaMsTemplate = 
            match getMajorMinor branch with
            | Main -> false
            | MajorMinor { Major = major; Minor = _minor; Patch = patch } when major = 5 && patch < 300 -> true
            | _ -> false
        if useLegacyAkaMsTemplate then
            Some (String.Format(template.AkaMSLegacyTemplate, 
                                platform,                       // 0 - win-x64
                                (branchNameShorten branch),     // 1 - 5.0.1xx-preview2
                                branch.GitBranchName,           // 2 - 5.0.100-preview.2.20169.1
                                (platform.Replace('-', '_')),   // 3 - win_64
                                branch.AkaMsChannel.Value))     // 4 - 5.0/preview2
        else
            Some (String.Format(template.AkaMSTemplate, 
                                platform,                       // 0 - win-x64
                                (branchNameShorten branch),     // 1 - 5.0.1xx-preview2
                                branch.GitBranchName,           // 2 - 5.0.100-preview.2.20169.1
                                (platform.Replace('-', '_')),   // 3 - win_64
                                branch.AkaMsChannel.Value))     // 4 - 5.0/preview2
    else
        Some (String.Format(template.LegacyTemplate,
                            platform,                       // 0 - win-64
                            (branchNameShorten branch),     // 1 - 5.0.1xx-preview2
                            branch.GitBranchName,           // 2 - 5.0.100-preview.2.20169.1
                            (platform.Replace('-', '_'))))  // 3 - win_64

let winX64ReferenceTemplate = formatTemplate "win-x64" referenceTemplate

let winX86ReferenceTemplate = formatTemplate "win-x86" referenceTemplate

let osxX64ReferenceTemplate = formatTemplate "osx-x64" osxReferenceTemplate

let osxArm64ReferenceTemplate branch =
    let format branch = formatTemplate "osx-arm64" osxReferenceTemplate branch
    match getMajorMinor branch with
        | Main -> format branch
        | MajorMinor { Major = major; Minor = _minor } when major >= 6 -> format branch
        | _ -> None

let linuxX64ReferenceTemplate = formatTemplate "linux-x64" linuxReferenceTemplate

let linuxArmReferenceTemplate = formatTemplate "linux-arm" linuxArmNoArchitectureReferenceTemplate

let linuxArm64ReferenceTemplate = formatTemplate "linux-arm64" linuxArmNoArchitectureReferenceTemplate

let rhel6x64ReferenceTemplate = formatTemplate "rhel.6-x64" rhel6ReferenceTemplate

let linuxMuslx64ReferenceTemplate = formatTemplate "linux-musl-x64" linuxMuslReferenceTemplate

let linuxMuslArmReferenceTemplate branch =
    let format branch = formatTemplate "linux-musl-arm" linuxMuslReferenceTemplate branch
    match getMajorMinor branch with
        | Main -> format branch
        | MajorMinor { Major = major; Minor = _minor; Patch = patch } when major >= 6 || (major >= 5 && patch >= 299) -> format branch
        | _ -> None

let linuxMuslArm64ReferenceTemplate branch =
    let format branch = formatTemplate "linux-musl-arm64" linuxMuslReferenceTemplate branch
    match getMajorMinor branch with
        | Main -> format branch
        | MajorMinor { Major = major; Minor = _minor; Patch = patch } when major >= 6 || (major >= 5 && patch >= 299) -> format branch
        | _ -> None

let winArmMuslReferenceTemplate branch =
    match getMajorMinor branch with
    | NoVersion -> None
    | MajorMinor { Major = major; Minor = minor } when major <= 2 && minor <= 1 -> None
    | _ -> formatTemplate "win-arm" winMuslReferenceTemplate branch

let winArm64MuslReferenceTemplate branch =
    match getMajorMinor branch with
    | NoVersion -> None
    | MajorMinor { Major = major; Minor = minor } when major < 5 -> None
    | _ -> formatTemplate "win-arm64" referenceTemplate branch

let templates =
    [ winX64ReferenceTemplate
      winX86ReferenceTemplate
      osxX64ReferenceTemplate
      osxArm64ReferenceTemplate
      linuxX64ReferenceTemplate
      linuxArmReferenceTemplate
      linuxArm64ReferenceTemplate
      rhel6x64ReferenceTemplate
      linuxMuslx64ReferenceTemplate
      linuxMuslArmReferenceTemplate
      linuxMuslArm64ReferenceTemplate
      winArmMuslReferenceTemplate
      winArm64MuslReferenceTemplate ]

let referenceList branches =
    String.Join
        (Environment.NewLine + Environment.NewLine,
         templates
         |> List.collect (fun template ->
             branches
             |> List.map template
             |> List.choose id))
