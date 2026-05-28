# Dependency Description Format

This document describes the format by which dependencies are specified in a .NET Core Repository

## Requirements
- Support tool-able version query and alteration without the need to execute any code in the repository
- Support versioning of various assets referenced through MSBuild, including SDKs, packages, etc.
- Provide additional data for Darc and associated tools to query (SHA, repo)
- Separate out toolset and runtime dependencies.

## Existing Expressions of Dependencies
Historically, there have been a few places where versioning shows up that we want to be tool-able.
- **NuGet (or asset) package versions ingested by the build** - Most repositories handle these the same way, with a props file in or near the root of the repository. This props file is imported by projects and its properties are utilized by PackageReference elements. For example, this is from the CLI repo:

    ```
    DependencyVersion.props

    <PropertyGroup>
        <MicrosoftExtensionsDependencyModelPackageVersion>2.1.0-preview2-26314-02</MicrosoftExtensionsDependencyModelPackageVersion>
    </PropertyGroup>
    ```
    ```
    Microsoft.DotNet.Cli.Utils.csproj

    <PackageReference Include="Microsoft.Extensions.DependencyModel" Version="$(MicrosoftExtensionsDependencyModelPackageVersion)" />
    ```

- **MSBuild SDKs** - MSBuild SDKs are special packages that MSBuild will download, unpack and inject imports in project files that contain references to the SDK.  Version numbers for these SDKs can either live in global.json at the root of the repo, or alongside the SDK attribute on the Project element.  For purposes of .NET Core, we intend to support version dependency tracking/update of versions in global.json, but not in individual SDK directives in project files.

    ```
    {
        "sdk": {
            "version": "2.200.0"
        },
        "msbuild-sdks": {
            "Arcade.Sdk": "1.0.0"
        }
    }
    ```

- **Bootstrapping files (e.g. BuildToolsVersion.txt, DotNetCLIVersion.txt, etc.)** – We often need to pull in tools before we can invoke msbuild, run the dotnet command line tool, etc.  These files are typically ingested by scripts which pull an asset from a specific location based on the version.  For example, corefx has:

    ```
    DotnetCLIVersion.txt

    2.1.2
    ```

- **Bootstrapping/Base Repository Scripts** – Under Arcade (formerly RepoToolset), a set of scripts are committed to the repo which wrap basic functionality (e.g. build) as well as do initial tool acquisition (e.g. msbuild).  These scripts are wholesale copies from the Arcade repo.  These scripts have historically not been strictly versioned with the RepoToolset/Arcade SDK in global.json.  Going forward, we will push versions of the scripts when we push new versions of the repotoolset, and the scripts will come from the arcade repo at the point the RepoToolset/Arcade SDK was produced:

    ```
    {
        "sdk": {
            "version": "2.1.100-preview-007366"
        },
        "msbuild-sdks": {
            // If build produced at SHA 12345, scripts in repo are obtained from SHA
            // 12345
            "RoslynTools.RepoToolset": "1.0.0-beta2-62628-01"
        }
    }
    ```
## Dependency Description Overview
The dependency description is comprised of two types of assets:
- **1 Dependency Details File** - XML file containing a set of information for each input dependency:
  - Dependency Name – Unique, typically the package name, but it is not required that individual packages be listed.  For example, a repo which ingests 10 assets with the same version could group all under a single dependency.
  - Version – Version of dependency
  - URL – URL of repository (typically) that produced the asset.
  - SHA – Git SHA at which the dependency was produced
  - Dependency Type – Either 'Toolset' or 'Product'.  'Toolset' dependencies are those that are used to produce the product, 'Product' dependencies are effectively everything else.  It is useful to differentiate between these so that we know how we are building (e.g. what CLI SDKs are in use across all repositories).  Defined another way, two successive builds of a Toolset dependency on the same SHA could produce two different tools (version-wise) with the same functionality.  Using either of the different toolset dependencies would produce no bit difference in the output product. **Note that this set could be extended in the future if needed.  E.g. a set of test-only dependencies might be added**
  - Pinned - if set to true, the dependency won't be automatically updated but will still be part of the graph. Is false or not defined it will be updated.
- **N Dependency Expression Files** - Places where dependencies are expressed.  These are well known locations and formats.  The version expressions are listed below.
  - Version Props File – The props file, typically for dependencies acquired via NuGet and msbuild
  - Global Json – Global json file (e.g. CLI SDK version and native toolsets acquired outside of msbuild)
  - Arcade Version – MSBuild SDK in the global.json file and associated checked in scripting.

    Two additional values may be used in the interim when moving the core repositories off of buildtools
  - Build Tools Version – Build tools version text file and associated version in the props file
  - Dot Net CLI Version – CLI version text file and other associated versions.

  The expression file corresponding to a dependency is implied by the dependency name.  Toolset or Product dependencies can have well known names, implying an expression in a specific form (e.g. global.json + repository scripting), or any other generic name, implying the version is expressed in the version.props file in the repository.  Examples:
    - AspNetCoreAllVersion - Product input of aspnet, expressed in version.props
    - DotNetSDKVersion – CLI SDK, expressed in global.json
    - ArcadeSDKVersion – Arcade SDK, expressed global.json and repository scripting
    - BuildToolsVersion – Legacy BuildToolsVersion.txt
    - DotNetCLIVersion – Legacy DotNetCLIVersion.txt

## Dependency Description Details
### Details File

**Version.Details.xml(eng\Version.Details.xml)**
```
<?xml version="1.0" encoding="utf-8"?>
<Dependencies>
    <!-- Elements contains all product dependencies -->
    <ProductDependencies>
        <-- All product dependencies are contained in Version.Props -->
        <Dependency Name="DependencyA" Version="1.2.3-45" Pinned="true">
            <Uri>https://github.com/dotnet/arepo</Uri>
            <Sha>23498123740982349182340981234</Sha>
        </Dependency>
        <Dependency Name="DependencyB" Version="1.2.3-45">
            <Uri>https://github.com/dotnet/arepo</Uri>
            <Sha>13242134123412341465</Sha>
        </Dependency>
        <Dependency Name="DependencyC" Version="1.2.3-45" Pinned="false">
            <Uri>https://github.com/dotnet/arepo</Uri>
            <Sha>789789789789789789789789</Sha>
        </Dependency>
    </ProductDependencies>

    <!-- Elements contains all toolset dependencies -->
    <ToolsetDependencies>
        <-- Non well-known dependency.  Expressed in Version.props -->
        <Dependency Name="DependencyD" Version="2.100.3-1234">
            <Uri>https://github.com/dotnet/atoolsrepo</Uri>
            <Sha>203409823586523490823498234</Sha>
            <Expression>VersionProps</Expression>
        </Dependency>
        <-- Well-known dependency.  Expressed in global.json -->
        <Dependency Name="DotNetSdkVersion" Version="2.200.0" Pinned="False">
            <Uri>https://github.com/dotnet/cli</Uri>
            <Sha>1234123412341234</Sha>
        </Dependency>
        <-- Well-known dependency.  Expressed in global.json -->
        <Dependency Name="Arcade.Sdk" Version="1.0.0">
            <Uri>https://github.com/dotnet/arcade</Uri>
            <Sha>132412342341234234</Sha>
        </Dependency>
    </ToolsetDependencies>
</Dependencies>
```

### Expression Files
**Versions.props (eng\Versions.props)**
```
<Project>
  <PropertyGroup>
    <!-- DependencyA, DependencyB, DependencyC substrings correspond to
         DependencyName elements in Version.Details.xml file -->
    <DependencyAPackageVersion>1.2.3-45</DependencyAPackageVersion>
    <DependencyBPackageVersion>1.2.3-45</DependencyBPackageVersion>
    <DependencyCPackageVersion>1.2.3-45</DependencyCPackageVersion>
    <DependencyDPackageVersion>2.100.3-1234</DependencyCPackageVersion>
    ...
  </PropertyGroup>
</Project>
```

**Global.Json (global.json)**
```
{
  "sdk": {
    "version": "2.200.0"
  },
  "msbuild-sdks": {
    "Arcade.Sdk": "1.0.0"
  },
  "native-tools": {
    "cmake": "3.11.1"
  }
}
```
