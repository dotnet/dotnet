# The Unified Build Almanac (TUBA) - TFM Trimming and Targeting

A Target Framework Moniker (TFM) is the name of the API surface area that a project builds for (https://learn.microsoft.com/en-us/dotnet/standard/frameworks). A project may target multiple TFMs, resulting in multiple output binary for each surface area target. This document describes the a method by which .NET will allow components to transparently target at least a desired set of TFMs, and by which additional TFMs they choose to target can be filtered out in build environments that require it.

## Problem

.NET's product is made up of a wide variety of repositories. Each of these repositories has a number of projects that specify a desired set of target frameworks. This set is largely determined by the *union* of consumers of the component. Where does the component need to run? For instance:
- A library like `System.Text.Json` may multi-target to `netstandard2.0`, `net472`, and `net8.0` because it ships on nuget.org and is intended for consumption by .NET Framework customers in addition to .NET Core customers, as well as downstream components which may be targeting older .NET Core TFMs.
- SDK components may only target `net8.0` because they ship in-box with the .NET 8 runtime.
- .NET tooling components (roslyn, fsharp, etc.) may multi-target to `net7.0` and `net4*` because they will run within Visual Studio (which runs on Framework) as well as different .NET SDK bands that may cross major version boundaries of .NET (`7.0.2xx` and `8.0.1xx`). `net7` represents a common surface area that *should* work well if rolled forward onto .NET 8.

While this flexiblity is useful, it does present a significant challenge for .NET distro maintainers. Targeting frameworks other the one currently being built ultimately requires the reference assemblies for that framework. Most Linux distributions disallow internet access while building, so those targeting packs cannot come from the internet. Source-build provides a mechanism for creating these references assemblies during the build, via the [source-build-reference-packages](https://github.com/dotnet/source-build-reference-packages) repository. These are assembled early in the build. There are major downsides to these reference packages:
- **Size:** The netframework targeting packs (18 of them) are 2.3GB of IL on-disk. This represents ~50% of the total size of the VMR.
- **Build Time:** Most costumer scenarios do not require all of target frameworks. RedHat, for instance, has no need for `net4*` targeted arcade build tooling binaries to be produced. Those binaries cannot even execute on Linux. The extra binaries produced wastes some amount of build time.
- **Build environment compliance:** Targeting packs/reference assemblies do not generally have functionality. However for various reasons, the analyzer implementations have been integrated *into* the targeting packs in .NET 7 and 8, update with servicing releases, and are executed during the build (see https://learn.microsoft.com/en-us/dotnet/fundamentals/code-analysis/overview?tabs=net-7#code-quality-analysis) for info on some analyzers. This causes circular dependency (the analyzers themselves execute on the runtime that is currently being built) that results in unresolveable pre-builts. To work around this issue in .NET 7 releases, the source-build team has stripped away the functional elements of the targeting packs. However, this approach is fragile at best, and problematic in the long term.

## Producers and Consumers

We can significantly reduce the dependence on reference packages, especially the large targeting packs by recognizing two key points:
- The set of frameworks targeted by a project is currently driven by **all** possible consumers not built within the same repository.
- When building a specific product (NuGet package, SDK layout, etc.), the set of required input frameworks is usually a subset of the available input frameworks.

Because .NET uses a distributed, many-repository based development model, producing repositories lack information about any specific consumer, and so must produce assets that target any possible use case. In plainer terms, let's say that we have a single repository with 2 projects. One is a non-packable library project that targets `net462`, `net472`, and `net8.0`. The other is a console exe targeting just `net8.0`, which references the library project. When building and publishing the console exe, there is no need to build the `net462` and `net472` assets. Now, let's say we split those projects into two repositories. The library project now must become packable to be referenced in the downstream console project. It also has no way to know that the `net4*` assets are useless. It must build them all.

Unified Build/source-build builds all input repositories required to produce the assets shipped by .NET distro maintainers, the consumer side of the build **is** known. RedHat ships a RedHat-targeted SDK and packages to its consumers. Microsoft ships packages to nuget.org, SDKs to VS, etc. Roslyn ships packages to nuget.org, VS, and the SDK. When building for a specific consumer in the VMR, a producer should be able to avoid building (trim away) away TFMs that are not used. Practically, this means that an organization should be able to only target TFMs that meet their end-customer's needs.

## Solution Requirements

Any solution must meet the following requirements:
- It cannot force all projects into targeting a certain TFM or set of TFMs (no big hammer) - Repositories and projects often have real reasons to target the TFMs they do. Roslyn must be able to run in VS. Some global tools want to support multiple runtimes within the same shipping unit.
- A project or repo should be able to opt-out if necessary with reasonable msbuild logic - There is wide variance in project needs. There may be situations where a reference pack is simply the only option.
- It should avoid "messy" changes in project files - Changes required in project files should avoid excessive conditionals and other high-maintenance constructs. Where possible, provide functionality via Arcade.
- It should be compatible with the VS project system (ideally ignored)
- It should be compatible with NuGet static graph restore.

## Proposed Solution

.NET can reduce its dependence on reference packages with a two-pronged, layered approach:
- **Targeting** Reduce undesired downlevel targeting - Repositories are often slow to update to the latest target frameworks. This is for a variety of reasons:
  - **Cost** - It is often not cost-free to upgrade.
  - **Forgetfulness** - Because the roll-forward behavior of the runtime generally "just works", it's easy for projects to simply be left behind on older frameworks.
  To improve things, Arcade can provide a set of properties for use in project files which denote desired target frameworks. When .NET starts working on a new major release and its TFM becomes available, Arcade can update those properties. Repositories who have opted-in will see their projects upgraded with normal infrastructre update PRs.
- **Trimming** Remove unecessary TFMs based on build-environment and consumer requirements - Provide a way to specify which TFMs should be kept and which should not be built at a build invocation level. This is accomplished via a set of Arcade functionality that utilizes a new MSBuild intrinsic to set the TargetFrameworks/TargetFramework property based on a desired input set.

## Targeting

To enable latest-targeting, Arcade will introduce a new property file called `TargetFrameworkDefaults.props`. This approach takes direct inspiration from current approaches in runtime and other repositories.

```
<Project>
  <PropertyGroup>
    <NetCurrent>net8.0</NetCurrent>
    <NetSupported>$(NetCurrent);net7.0;net6.0</NetSupported>
  </PropertyGroup>
</Project>
```

Initially, this file will contain only one property, the currrent major version of .NET. If additional properties are needed (minimum version, newest framework versions, etc.), they can be added. This file is imported in `Settings.props` within the Arcade SDK. These properties are then used as desired within repositories' project, property files, etc. For example, a project might do the following:

```
Microsoft.FileProviders.Composite.csproj

<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFrameworks>$(NetCurrent);netstandard2.0</TargetFrameworks>
    <RootNamespace>Microsoft.Extensions.FileProviders</RootNamespace>
  </PropertyGroup>

  <ItemGroup>
    <Compile Include="Microsoft.Extensions.FileProviders.Composite.cs" />
  </ItemGroup>

  <ItemGroup>
    <ProjectReference Include="$(LibrariesProjectRoot)Microsoft.Extensions.FileProviders.Abstractions\ref\Microsoft.Extensions.FileProviders.Abstractions.csproj" />
    <ProjectReference Include="$(LibrariesProjectRoot)Microsoft.Extensions.Primitives\ref\Microsoft.Extensions.Primitives.csproj" />
  </ItemGroup>
</Project>
```

### Opt-in

This feature is opt-in. Repositories choose to use the properties to reduce the chance of falling behind and targeting older TFMs, especially .NET Core TFMs.

## Trimming

Opt-ins to the targeting feature should reduce the amount of downlevel targeting and help .NET move forward as a coherent stack. It does not, however, remove TFMs that are not required when building for a specific consumer. To do so, we need a filter. We need to be able to remove unwanted TFMs.

To accomplish this, we will implement an MSBuild intrinsic which removes target frameworks that do not match an input set based on name and version. Arcade will then use a targets file which will set the `TargetFrameworks`/`TargetFramework` property based on the output of this intrinsic, if a filter is to be applied. It's important to highlight the MSBuild intrinsic-based as *separate* from the "what should we target in this invocation". It is a method of exclusion, implemented in a way that does not require knowing the full set of possible TFMs to exclude. Instead, acts as a way of filtering out targets that are not required for the consumers of the project.

### MSBuild Intrinsic

**IntersectTargetFrameworks (string original, string filter)**

Given two sets of input target frameworks in the form: tfm1[;tfm2][;tfm3], compute the intersection of `original` with `filter`, based on Framework and Version. Platform elements of the TFM are ignored. Return the matching elements from the original set. Examples:

```
IntersectTargetFrameworks("net7.0;netstandard2.0", "net7.0") returns "net7.0"
IntersectTargetFrameworks("net7.0;netstandard2.0", "net7;netstandard2.0") returns "net7.0;netstandard2.0"
IntersectTargetFrameworks("net7.0-windows;net472;netstandard2.0", "netstandard2.0") returns "netstandard2.0"
IntersectTargetFrameworks("net7.0-windows;netstandard2.0", "net472;net7.0") returns "net7.0-windows"
IntersectTargetFrameworks("net7.0-windows;net7.0-linux;netstandard2.0;net472", "net472;net7.0") returns "net7.0-windows;net7.0-linux;net472"
```

### Arcade support for filtering

In `Imports.targets`, a new file `TargetFrameworkDefaults.targets` will be imported. 

```xml
TargetFrameworkDefaults.targets

<!-- Licensed to the .NET Foundation under one or more agreements. The .NET Foundation licenses this file to you under the MIT license. -->
<Project>
  <PropertyGroup>
    <!-- Obtain the original set based on whether TargetFramework or TargetFrameworks was used -->
    <_OriginalTargetFrameworks Condition="'$(TargetFrameworks)' != ''">$(TargetFrameworks)</_OriginalTargetFrameworks>
    <_OriginalTargetFrameworks Condition="'$(TargetFramework)' != ''">$(TargetFramework)</_OriginalTargetFrameworks>
    <_FilteredTargetFrameworks Condition="'$(NoTargetFrameworkFiltering)' != 'true' and '$(DotNetTargetFrameworkFilter)' != ''">$([MSBuild]::Unescape($([MSBuild]::IntersectTargetFrameworks('$(_OriginalTargetFrameworks)', '$(DotNetTargetFrameworkFilter)'))))</_FilteredTargetFrameworks>
    <TargetFrameworks Condition="'$(NoTargetFrameworkFiltering)' != 'true' and '$(_FilteredTargetFrameworks.Contains(';'))'">$(_FilteredTargetFrameworks)</TargetFrameworks>
    <TargetFramework Condition="'$(NoTargetFrameworkFiltering)' != 'true' and '!$(_FilteredTargetFrameworks.Contains(';'))'">$(_FilteredTargetFrameworks)</TargetFramework>
  </PropertyGroup>
</Project>
```

### Opt-out

If a repository sets property `NoTargetFrameworkFiltering` to `true`, then filtering will not be applied.

### Validation during repo-level source build

It is entirely possible that TFM filtering will break source-build for a repository. For instance, if a project targets no TFMs after filtering is applied, it will fail to build. The repository owner will then need to decide on a course of action. Perhaps they need to exclude that project during when doing source build (probably based on build platform), or target an included TFM. To avoid unexpected breaks, repo level source-build validation will enable filtering in certain cases. Which TFMs are kept will be dependent on the platform being validated and the build environment requirements of those who usually execute that build. For instance: 
- A Windows source build leg would not filter any TFMs. Ref packs can be supplied from the internet and many components built on Windows will require targeting a number of TFMs, including net4*
- An OSX leg might use a filter like `net7;net8;netstandard2.0`, which excludes `net4*` TFMs, but allows for all .NET Core TFMs to be kept. OSX builds have no restriction on pulling targeting packs from the internet, but `net4*` TFMs wouldn't generally be useful on OSX.
- A Linux source-build leg would use a filter like `net8;netstandard2.0` to remove all usage of targeting packs that would have to come from the internet or would be undesirable to check-in as source build reference packages, since that is what Linux source build partners require.

## Source-build Usage Example

```
# ./build.sh --clean-while-building --online --tfm-filter net7;netstandard2.0
```

With this invocation, projects will only produce assets that target `net7*` and `netstandard2.0`. For instance, `arcade`, which builds early on in source-build, has a project `src/Microsoft.DotNet.SignTool/Microsoft.DotNet.SignTool.csproj`.

```xml
<!-- Licensed to the .NET Foundation under one or more agreements. The .NET Foundation licenses this file to you under the MIT license. -->
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFrameworks>$(NetCurrent);net472</TargetFrameworks>
    <AllowUnsafeBlocks>true</AllowUnsafeBlocks>
    <IsPackable>true</IsPackable>
    <Description>Build artifact signing tool</Description>
    <PackageTags>Arcade Build Tool Signing</PackageTags>
    <DevelopmentDependency>false</DevelopmentDependency>
    <NoWarn>$(NoWarn);NU5128</NoWarn>
  </PropertyGroup>
  ...
</Project>
```

SignTool.csproj will not produce a `net472` targeted binary.