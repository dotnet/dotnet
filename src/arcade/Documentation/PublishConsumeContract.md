# Toolset Packages

- [Toolset Feed](#toolset-feed)
- [Core Tools SDK](#core-tools-sdk)
- [Bootstrapping](#bootstrapping)
- [Using tools in non-bootstrapping scenarios](#using-tools-in-non-bootstrapping-scenarios)
- [Onboarding](#onboarding)
- [Package versioning](#toolset-package-versions)
- [Package contents](#package-contents)
- [Package symbols](#package-symbols)
- [Maestro and the Versions repo](#maestro-and-the-versions-repo)
- [Gallery](#gallery)
- [Package validation](#package-validation)
- [Sdk validation](#sdk-validation)
- [Provenance](#provenance)
- [Usage](#usage)

## Toolset Feed

Toolset packages should be published to a consistent location for consumption.

There are currently a couple of different sources for various repo toolsets.

- https://dotnet.myget.org/F/aspnetcore-tools/api/v3/index.json
- https://dotnet.myget.org/F/roslyn-tools/api/v3/index.json
- https://dotnet.myget.org/F/dotnet-buildtools/api/v3/index.json

Shared toolset packages will be published to a single location so that consumption / [discoverability](#gallery) is simplified.

Toolset package feed: https://dotnetfeed.blob.core.windows.net/dotnet-core/index.json

## Core Tools SDK

The core tools SDK is the entry point for toolset functionality.  We will provide a core SDK which repo's will consume as an SDK (or package reference) that provides functionality for tasks that are common across repo's.  The core tools SDK may contain one or more tools packages which have been determined to be beneficial to a common set of repos (most?) across DotNet.  As packages prove valuable to more than one repo, they will be considered for inclusion in the core tools SDK.  However, we want to be considerate of package bloat and seek alternative (but common) means of consumption for tool packages which do not meet the criteria for inclusion in the core tools SDK.  In other words, the packages will need to provide clear benefit to the majority (or all) of repos in order to be considered for inclusion in the core tools SDK.

## Tools packages

Tools packages provide functionality (MSBuild or other) which are useful to one ore more repo's.  Tools packages (specifically MSBuild task packages) are currently being [discussed](https://github.com/dotnet/core-eng/pull/2541/files) and will be considered for inclusion in the core tools SDK (if they provide clear functionality to the majority of DotNet repos).  Additional tools / task packages will be available for direct consumption or via the core tools SDK.

## Bootstrapping

Bootstrapping a repo will consist of using the CLI (obtainable via a script from a well-known / secure location) to restore the [core tools SDK](#core-tools-sdk) project.

## Using tools in non-bootstrapping scenarios

There are some scenarios where bootstrapping is not ideal for acquiring tools.  These are scenarios which are not project based, or not tied to a specific repo.  A primary example of this is telemetry, where you want to be able to send information about a build, before a repo has even bootstrapped.  Another may be orchestration (depending on implementation), the orchestration may schedule and report on multiple repo's, but itself is not tied to a repo.  For these scenarios, we would like to be able to provide common tooling.  At this point, there are a couple of ideas being thrown around.

- "DotNet CLI install tools" is one option for local toolset installs, but not available until .NET Core 2.1 Preview 2 (at the earliest).
- "Shared Library" model (like Jenkins), where tools are provided via another common tools repo.
- [CBT](https://cbt-userguide/Introduction.html) is a new offering from 1ES.  Not enough investigation has occurred to determine if this is a viable option.

We will evaluate guidance for these scenarios when they arise.

## Onboarding

Onboarding a repo to the toolset will be a [simple process](https://github.com/dotnet/dnceng/tree/main/Documentation/ProjectDocs/buildtools-bootstrap.md).

We will provide links to zips / tarballs to acquire the basic pieces necessary for bootstrapping the core tools SDK on a supported platform.

Note: Further guidance on onboarding a repo and customizing for a particular repo's needs will be provided in a separate documentation.  Some general [usage](#usage) is provided below.

## Toolset package versions

Package versioning should follow precedent set by other repo's rather than trying to produce new versioning scheme / tooling.  Most of the "core" DotNet repositories (CoreFx, CoreClr, Core-Setup, etc...) are using [versioning](https://github.com/dotnet/corefx/blob/master/Documentation/building/versioning.md) tools which are a part of [BuildTools](https://github.com/dotnet/buildtools/blob/master/src/Microsoft.DotNet.Build.Tasks/PackageFiles/versioning.targets).  The versioning logic will be available from a [task package](https://github.com/dotnet/core-eng/pull/2541/files) where it will be generally available for all participating repositories.

### Versioning constraints

- Version needed to be higher than the versions previously shipped.
- There needs to be an ability to have multiple versions per day.
- Versions need to be always increasing.
- Version needs to be lower than 65535 (unsigned short int max) since the version is used as assembly file version which has that constraint.
- Version needs to be reproducible.
- We shouldn't have the need to check in a file containing the buildnumber.  Checked in files containing major/minor/patch will be permitted.
- We will support SemVer [1.0](https://semver.org/spec/v1.0.0.html) and [2.0](https://blog.nuget.org/20140924/supporting-semver-2.0.0.html) semantics.  If there are issues related to SemVer 2.0 support on older clients, then we'll consider adjusting to support those scenarios.

Package version example:

```Text
SemVer 1.0: mylibrary.1.0.0-prerelease-00001-01.nupkg
SemVer 2.0: mylibrary.1.0.0-prerelease.1.1.nupkg
```

## Package contents

Standard package layout

``` Text
(root)
  - sdk/
    + Sdk.props (optional)
    + Sdk.targets
  - build/
    + $packageId.props (optional)
    + $packageId.targets
    - netstandard1.5/
      + $taskAssembly.dll
    - net46/
      + $taskAssembly.dll
```

The standard package layout *supports* (not required) consuming packages as [MSBuild Project SDKs](https://docs.microsoft.com/en-us/visualstudio/msbuild/how-to-use-project-sdk).  In general, we believe that there will be one project SDK which is referenced and that the toolset packages will be consumed as package references, not as SDK's.  At this time, however, we are not enforcing a strict model which prevents or requires toolset consumption as individual SDK's.

`Sdk.props` and `Sdk.targets` should not contain any functional code, only imports for the respective build props / targets.

### Requirements

- Utilities, exe's, scripts, etc which are part of the package functionality must be usable via MSBuild properties / targets.  You should not have a collection of executable files in your package which do not include MSBuild entry points for using them.

- Additional package guidelines are outlined [here](https://github.com/dotnet/dnceng/tree/main/Documentation/ProjectDocs/Toolshed/TaskPackages.md#implementation-details)

- Packages need to include accountability information in the nuspec.  At a minimum, source repository link and commit SHA.

### Package dependencies

The tools provided via NuGet packages for MSBuild tasks will be self-contained (include all of their dependencies).  It is important to be deliberate about what dependency versions are included in a package because otherwise the mix-match model of the tools will be broken.  As a starting place, dependency versions should align with what is provided by the core tools SDK.  If you have additional dependencies outside of those in the core tools SDK (or need to change dependency versions), then we should be deliberate (have a conversation with core tools stakeholders) about what those dependencies are and what versions are required.

### Best practices

- Choose non-generic build property / target names.  Packages should be very considerate when defining property / target names.  For example, if each package defines a property called `TaskDir` which is defined as `$(MSBuildThisFileDirectory)build/blah`, then the last package imported will be the one to define `TaskDir`, and all of your other packages will be broken.  So packages should prefer to choose target / property names which are unlikely to conflict with other packages or which include the package name in the property / targetname, ie `MyPackageNameTaskDir`

- Ensure build props file is imported.  In the props file, you should define some property such as `<_MyPackageNameImported>true</_MyPackageNameImported>` and the targets file then includes `<Import Project="$(MSBuildThisFileDirectory)$(MSBuildThisFileName).props" Condition="'$(_MyPackageNameImported)' == ''" />`.  This would permit consumers to just directly import the targets file if desired instead of importing two files.

## Package symbols

Task package symbols should be embedded in the binaries.

## Maestro and the Versions repo

Toolset packages will assume the use of Maestro for automatic version uptake.

Toolset packages should be publishing version information to the versions repo so that repositories using automatic version updating can consume them.  When publishing, there should be package versions entries both for the repo producing the package, and for a tools location which aggregates the various toolset packages. [Details are TBD]

## Gallery

A traditional gallery (ie myget.org) is not provided for the toolset.  Instead toolset packages may be browsed using a [package source](https://docs.microsoft.com/en-us/nuget/tools/package-manager-ui#package-sources) in Visual Studio.  Additionally, toolset packages will be listed on the versions repo [link TBD].

## Package validation

Currently, there are no unit tests for package validation / conformance.

## Sdk validation

Currently, there are no unit tests for Sdk validation / conformance

## Provenance

Security is continuing to tighten, and we require provenance for any bits that we own / control directly.  Provenance guidance / requirements are provided [here](https://securityguidance.cloudapp.net/).  It is important to keep these rules in mind for all tools package providing repos.

## Usage

### Core Tools SDK Usage

The core tools SDK will be typically consumed as a [project SDK](https://docs.microsoft.com/en-us/visualstudio/msbuild/how-to-use-project-sdk).

### Tools packages Usage

Tools packages will typically be consumed as package references in an individual repo.  The toolset SDK should provide extensibility points to add package references for the toolset which are specific to a repo.   If functionality proves to be beneficial to additional repo's, it will go under consideration for becoming part of the core toolset SDK.

[Note: Extensibility points may not yet be present]

Example of common `Toolset.proj`

```XML
<Project Sdk="DotNet.Tools.Internal.Sdk">
  <PropertyGroup>
    <TargetFramework>net462</TargetFramework>
    <RestoreSources>https://dotnetfeed.blob.core.windows.net/dotnet-core/index.json</RestoreSources>
  </PropertyGroup>
</PropertyGroup>
```

[Note: This example should include how to add project specific PackageReferences to the toolset]

Example of tools as SDK's usage (less common usage)

```XML
<Project Sdk="DotNet.Tools.Internal.Sdk;SignTool;Microsoft.DotNet.Build.Tasks.Feed">
  <PropertyGroup>
    <TargetFramework>net462</TargetFramework>
    <RestoreSources>https://dotnetfeed.blob.core.windows.net/dotnet-core/index.json</RestoreSources>
  </PropertyGroup>
</PropertyGroup>
```


<!-- Begin Generated Content: Doc Feedback -->
<sub>Was this helpful? [![Yes](https://helix.dot.net/f/ip/5?p=Documentation%5CPublishConsumeContract.md)](https://helix.dot.net/f/p/5?p=Documentation%5CPublishConsumeContract.md) [![No](https://helix.dot.net/f/in)](https://helix.dot.net/f/n/5?p=Documentation%5CPublishConsumeContract.md)</sub>
<!-- End Generated Content-->
