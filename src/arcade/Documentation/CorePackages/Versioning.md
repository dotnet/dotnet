# .NET Core Ecosystem v2 - Versioning

| Date       | Note                                                         |
| ---------- | ------------------------------------------------------------ |
| 10/03/2018 | The implementation doesn't include the SHA on the version strings anymore, because: 1) The chaining of targets to plug that into SemVer1 strings were causing generation of incorrect NuSpec files and no clear path to fix it was found and, 2) VS CoreXT build system doesn't support it. |

## Versioning Plan

.NET Core repositories should use [SemVer2](https://semver.org) for their asset versioning scheme. Package versions take the general form of:

```
MAJOR.MINOR.PATCH-PRERELEASE+BUILDMETADATA
```

MAJOR, MINOR, and PATCH versions are rigid in their requirements.  Please refer to the [SemVer2 documentation](https://semver.org) for details. PRERELEASE and BUILD are optional and leave a fair bit of room for organizations to implement what they want. The only caveat is that build metadata cannot be used to differentiate two different packages. Build metadata should not be used when determining version precedence. There are two primary questions:
- What goes in the PRERELEASE and BUILD fields and when are they used?
- Should we use date agnostic versioning vs. date varying versioning?

## Date Agnostic vs. Date-Varying Build Versioning

In the context of the .NET Core product, a build version number is date agnostic if successive builds of the same commit produce the same build version numbers. Historically, .NET Core has had repositories that implement date agnostic versioning through build numbers that increment based on commit depth, as well as repositories that base their build number off of the date and number of builds prior on the day (date based versioning).

Both types of versioning have advantages and disadvantages:

### Date Agnostic Versioning

**PROS:**
- Assets with identical version metadata can be reproduced without outside input (e.g. providing a build id)
- Parallel, independent legs of the same build do not require parent orchestration and will generate coherent versioning.  Separate parts of the build can potentially be respun as required.
- We already have 'date agnostic' versioning at the end of a product cycle.
- Build metadata typically includes SHA, which makes identification of the source of bits easy.

**CONS:**
- Some related infrastructure systems may not support the ingestion of multiple assets with the same name/version.  For example, MyGet, NuGet, or VSTS support overwriting to varying degrees. You're left to deal with these scenarios on a case-by-case basis.  For example:
  - Introduce a new meaningless commit to bump the version number
  - Successive builds of the same SHA, if they were to have different outputs (e.g. a checked vs. debug build) may require a clean of the cache.
  - Temporarily unlock a feed to enable re-publishing (may not be possible, and bad practice)
  - Add logic into all processes to gracefully understand and handle overwrite cases (e.g. are bits the same? then okay)
  - Telemetry systems must understand reruns of the same build.
- Most of the .NET Core builds aren't bit-for-bit identical at the same commit given the same inputs.  If they were, then a partial-respin can gracefully handle dealing with overwrites.
- Date agnostic version doesn't encode inputs (e.g. checked build vs. release build).  This can make it tricky to run 'non-standard' build scenarios if they interact with external systems.
- Generating the same package version in multiple builds with different outputs bits means that package caches must be cleared.

### Date-Varying Versioning

**PROS:**
- Respins are easier.  No overwriting except for release versions at the end of the product cycle.  Release versions at the end of the cycle typically avoid external overwrite-averse systems.
- Non-standard builds (e.g. different input parameters) do not collide with standard builds at the same SHA when dealing with external systems.
- File versions must be ever increasing to correctly layout new files for servicing (MSI requirement)
- Does not affect the determinism of the build. Input dates are just another build parameter (e.g. OfficialBuildId) and rerunning a build with the same input date should produce equivalent binaries.

**CONS:**
- Requires orchestration to produce a coherent set of versions across multiple build legs
- More difficult to reproduce version identical bits (need to know input parameters)
- Source SHA not easily identifiable based on asset version.

### Conclusion

Date agnostic versioning is more hassle than it's worth, though having the SHA in the output version number is also useful.  We should combine a SHA in the build metadata with the build date+revision (short data + number of builds so far today) to generate a date-varying, unique, identifiable build.

## Build Determinism

  There is often concern around build determinism when date-varying versioning is used. It is important to note that date-varying versioning does not affect the ability to have deterministic builds in either the local dev or official build lab scenarios.  The date is either a provided parameter or obtained from git information, meaning that setting it to a specific value at a specific commit can enable the production of the same outputs over and over again.  Date varying versioning only says that **by default** this input varies from build to build.

## Build kind

_Build kind_ is determined based on global build properties `ContinuousIntegrationBuild`, `OfficialBuildId` and `DotNetFinalVersionKind`.

| `ContinuousIntegrationBuild` | `OfficialBuildId` | `DotNetFinalVersionKind` | Build kind                      |
|------------------------------|-------------------|--------------------------|---------------------------------|
| _false_                      | ""                | ""                       | Local developer build           |
| _true_                       | ""                | ""                       | PR validation build             |
| _true_                       | `yyyymmdd.r`      | ""                       | Daily official build            |
| _true_                       | `yyyymmdd.r`      | "prerelease"             | Final pre-release official build |
| _true_                       | `yyyymmdd.r`      | "release"                | Release official build          |

## Version parts specified by repository

The repository specifies a 3-part version prefix in `VersionPrefix` build property (**MAJOR**.**MINOR**.**PATCH**), 
or 2 parts using `MajorVersion` and `MinorVersion` properties (the patch number defaults to 0 in such case). 
If neither of these properties are specified the default is 1.0.0.

The versioning scheme defined below imposes the following limits on these version parts:
- **MAJOR** version is in range [0-65535]
- **MINOR** version is in range [0-654]
- **PATCH** version is in range [0-9999]

## Package Version

_Package Version_ comprises of a three-part version number (**PACKAGE_MAJOR**, **PACKAGE_MINOR**, **PACKAGE_PATCH**) and optional pre-release labels.

The pre-release label is determined based on the following table:

| Build kind                       | Pre-release labels                                | Package Version example    |
|----------------------------------|---------------------------------------------------|----------------------------|
| Local developer build default    | "dev"                                             | "1.2.3-dev"                |
| PR validation build              | "ci"                                              | "1.2.3-ci"                 |
| Daily official build             | **PRERELEASE_LABELS**.**SHORT_DATE**.**REVISION** | "1.2.3-preview.1.12345.1"  |
| Final pre-release official build | **PRERELEASE_LABELS**."final"                     | "1.2.3-beta.1.final"       |
| Release official build           | ""                                                | "1.2.3"                    |   

In official builds, the value of **PRERELEASE_LABELS** is derived as in the following way:
- If `SemanticVersioningV1` is set to true, it will be `PreReleaseVersionLabel` with `PreReleaseVersionIteration` appended. `PreReleaseVersionIteration` may be empty
- If `SemanticVersioningV1` is not set to true, and `PreReleaseVersionIteration` is empty, it will be `PreReleaseVersionLabel`
- If `SemanticVersioningV1` is not set to true, and `PreReleaseVersionIteration` is non empty, it will be `PreReleaseVersionLabel`.`PreReleaseVersionIteration`

In official builds the values of **SHORT_DATE** and **REVISION** are derived from build parameter `OfficialBuildId` with format `20yymmdd.r` like so:
- **REVISION** is set to `r` component of `OfficialBuildId` build property.
- **SHORT_DATE** is set to `yy` * 1000 + 50 * `mm` + `dd`. In year 2018 the value is in range [18051, 18631].

In PR validation and local developer builds **SHORT_DATE** and **REVISION** are not included in the package version,
unless `DotNetUseShippingVersion` is _true_, in which case the values of `yy`, `mm` and `dd` are derived from the current date
and `r` = 1. Note that non-official builds with `DotNetUseShippingVersion` set to _true_ are non-deterministic.

If a package is designated to be a _release-only_ package (`PreReleaseVersionLabel` is empty) its package version does not include 
any pre-release labels when produced by an official build. Every official build of such package must produce a unique **PATCH_NUMBER**.
This versioning policy is not applied to developer and PR validation builds as it relies on the availability of a unique `OfficialBuildId`.

Some projects want to remain producing pre-release packages even if the repository is running a final stable build because
they don't ship or aren't ready to ship stable. Those projects can set `SuppressFinalPackageVersion` property to `true`.

**PATCH_NUMBER** is defined as (**SHORT_DATE** - `VersionBaseShortDate`) * 100 + `r`, where `VersionBaseShortDate` is `19000` unless 
set in `eng/Version.props`. 
Repository shall only change the value of `VersionBaseShortDate` at the same time as it increments **MAJOR** or **MINOR** version.

The three-part package version is based on the value of `VersionPrefix` property:

| Package version part | Value            | Condition                                |
|----------------------|------------------|------------------------------------------|
| **PACKAGE_MAJOR**    | **MAJOR**        |                                          |
| **PACKAGE_MINOR**    | **MINOR**        |                                          |
| **PACKAGE_PATCH**    | **PATCH**        | `PreReleaseVersionLabel` is non-empty    |
|                      | **PATCH_NUMBER** | otherwise                                |

## Assembly Version

_Assembly Version_ is a four-part version number (**ASSEMBLY_MAJOR**, **ASSEMBLY_MINOR**, **ASSEMBLY_PATCH**, **ASSEMBLY_REVISION**).

**PATCH_NUMBER_HI** is defined as **PATCH_NUMBER** / 50000.

**PATCH_NUMBER_LO** is defined as **PATCH_NUMBER** % 50000.

| Assembly version part | Value                                             | Condition                                |
|-----------------------|---------------------------------------------------|------------------------------------------|
| **ASSEMBLY_MAJOR**    | **MAJOR**                                         |                                          |
| **ASSEMBLY_MINOR**    | **MINOR**                                         |                                          |
| **ASSEMBLY_PATCH**    | **PATCH**                                         | `AutoGenerateAssemblyVersion` is _false_ |
|                       | **PATCH_NUMBER_HI**                               | otherwise                                |
| **ASSEMBLY_REVISION** | 0                                                 | `AutoGenerateAssemblyVersion` is _false_ |
|                       | **PATCH_NUMBER_LO**                               | otherwise                                |

## File Version

_File Version_ is a four-part version number (**FILE_MAJOR**.**FILE_MINOR**.**FILE_PATCH**.**FILE_REVISION**) and 
must increase every official build. This is especially important when building MSIs. 

If build property `AutoGenerateAssemblyVersion` is _true_ then _File Version_ is the same as _Assembly Version_, otherwise:

| File version part     | Value                                             |
|-----------------------|---------------------------------------------------|
| **FILE_MAJOR**        | **MAJOR**                                         |
| **FILE_MINOR**        | **MINOR** * 100 + **PATCH** / 100                 |
| **FILE_PATCH**        | (**PATCH** % 100) * 100 + `yy`                    |
| **FILE_REVISION**     | (50 * `mm` + `dd`) * 100 + `r`                    |

## Recommended Settings

It is recommended for **global tools** projects to build assemblies with auto-generated assembly version and pack as _release-only_ packages, 
i.e. set `AutoGenerateAssemblyVersion` to _true_ and clear `PreReleaseVersionLabel`.

It is recommended for **msbuild task** projects and projects building **analyzers** to build assemblies with auto-generated assembly version,
i.e. set `AutoGenerateAssemblyVersion` to _true_.

Library projects that target .NET Standard or .NET Framework shall keep `AutoGenerateAssemblyVersion` set to _false_ to avoid the need for updating binding redirects of the consuming .NET Framework apps every time a new build is consumed.

## SemVer1 Fallback

In cases where SemVer2 cannot be used (e.g. old versions of NuGet), we can fall back to [SemVer1](https://semver.org/spec/v1.0.0.html). 
In SemVer1, there is no built in build metadata, and the pre-release field may only contain `[0-9A-Za-z-]`. To comply, cases where `+` or `.` 
are used in SemVer2's prerelease field are replaced with `-`.

The repository opts into SemVer1 fallback by setting `SemanticVersioningV1` property to _true_.

Examples of SemVer1 package versions:

"1.2.3-dev", "1.2.3-ci", "1.2.3-beta-12345-01", "1.2.3-beta-final", "1.2.3".

Note that the number in `-01` suffix is zero-padded to two characters.

## Assembly Informational Version Generation

Assembly informational version is generated by .NET SDK. If the project references SourceLink package (added by Arcade SDK by default) the assembly informational version will include the full commit SHA. An example of a version string that the assembly will include:

```csharp
[AssemblyInformationalVersion("1.2.3-beta.12345.1+fe80f83075d723eddd6e26582c75f27f242c69c4")]
```

Specifying `/p:IncludeSourceRevisionInInformationalVersion=false` will remove the `+<commitId>` part from the AssemblyInformationalVersion (and ProductVersion in File properties). This is not recommended because it prevents identifying which commit the assembly was built from, but might be necessary if upstream consumers parse the version and can only handle SemVer1. This option is coming from [Microsoft.NET.Sdk](https://learn.microsoft.com/en-us/dotnet/core/project-sdk/msbuild-props#generateassemblyinfo).

## NuGet Package Repository Information

Packages produced by projects that reference SourceLink package and use standard NuGet Pack target for packaging will include repository URL and commit SHA metadata in their nuspec files:

```xml
<repository type="git" url="https://github.com/dotnet/roslyn" commit="d37ac834f69e1a771626813da3b820502309462d" />
```

## Implementation

The Arcade SDK implements [here](../../src/Microsoft.DotNet.Arcade.Sdk/tools/Version.targets) the Versioning schema described in this document. This section provides a brief outline of how to use the implementation and how it works. More documentation is available in the respective [.target/.props](../../src/Microsoft.DotNet.Arcade.Sdk/tools/Version.targets) file.

Arcade onboarded repos use this implementation automatically by using the Arcade SDK. *If you use the SDK you won't have to do anything else to use the format strings described in the proposal.* 

Below is a list of the main parameters that control the logic.

### Parameters

| Parameter                  | Scope  | Description                                                  |
| -------------------------- | ------ | ------------------------------------------------------------ |
| OfficialBuildId            | Arcade | ID of current build. The accepted format is `yyyyMMdd.r`. Should be passed to build in YAML official build definition. |
| SemanticVersioningV1       | Arcade | Set to `true` in `Versions.props` file to use versions compatible with SemVer 1.0. |
| DotNetUseShippingVersions  | Arcade | Set to `true` to produce shipping version strings in non-official builds. I.e., instead of fixed values like `42.42.42.42` for `AssemblyVersion`. |
| DotNetFinalVersionKind     | Arcade | Specify the kind of version being generated: `release`, `prerelease` or empty. |
| PreReleaseVersionLabel     | Arcade | Pre-release label to be used on the string. E.g., `beta`, `prerelease`, etc. `ci` and `dev` are reserved for non-official CI builds and dev builds, respectively. |
| PreReleaseVersionIteration | Arcade | Numeric pre-release iteration to be used on the pre-release suffix string. E.g., `1`, `2`, etc. If set, and SemVer2 is in use, appends to the prerelease version label, separated by a `.`. If SemVer1 is in use, then it is appended without a separator. |
| VersionPrefix              | .NET   | Specify the leading part of the version string. If empty and both `MajorVersion` and `MinorVersion` are set, initialized to `$(MajorVersion).$(MinorVersion).0`. |
| MajorVersion               | Arcade | Major version to use in `VersionPrefix`. |
| MinorVersion               | Arcade | Minor version to use in `VersionPrefix`. |
| ContinuousIntegrationBuild | .NET   | Specify whether the build is happening on a CI server (PR build or official build). |

## Output
 This is the list of properties set by Arcade SDK versioning implementation. The values are available only after `GetAssemblyVersion` target has been executed.

| Name            | Scope | Description                                                  |
| --------------- | ----- | ------------------------------------------------------------ |
| OfficialBuild   | Arcade| Boolean indicating that the current build is official. Set to `true` if `OfficialBuildId` is non-empty. |
| AssemblyVersion | .NET  | Set to `42.42.42.42` if not set in the project, the build is not official and `DotNetUseShippingVersions` is not `true`. |
| FileVersion     | .NET  | Set to `42.42.42.42424` if the build is not official and `DotNetUseShippingVersions` is not `true`. |
| VersionPrefix   | .NET  | Initialized to `$(MajorVersion).$(MinorVersion).0` if not set by the project.  |
| VersionSuffix   | .NET  | The suffix part of the version string, including the pre-release portion. |



<!-- Begin Generated Content: Doc Feedback -->
<sub>Was this helpful? [![Yes](https://helix.dot.net/f/ip/5?p=Documentation%5CCorePackages%5CVersioning.md)](https://helix.dot.net/f/p/5?p=Documentation%5CCorePackages%5CVersioning.md) [![No](https://helix.dot.net/f/in)](https://helix.dot.net/f/n/5?p=Documentation%5CCorePackages%5CVersioning.md)</sub>
<!-- End Generated Content-->
