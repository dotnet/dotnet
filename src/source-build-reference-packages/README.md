# .NET Source-build Reference Packages

This repository contains tools, source and build scripts for source-buildable reference
versions of historical .NET Core packages that are referenced by
[.NET source build](https://github.com/dotnet/source-build). These are used only
when source building .NET.

This repo supports three package types:

1. Reference - A package that contains API signatures, no implementation. It enables developers to build,
targeting a specific library version without having the full implementation assembly for that version.
1. Targeting - A bundle of reference packages that target a specific .NET TFM.
1. Text only - Packages that contain text-only assets.

This repo and it's reference packages are often referred to as SBRPs. This acronym is used from time
to time in issues and some documentation.

## Supported Platforms

.NET source build currently only supports Linux but generating a source-build reference or text-only package
is supported on both Windows and Unix based operating systems.

## Building

``` bash
./build.sh -sb
```

## Adding new Packages

New packages are needed from time to time as
[existing dependency versions are upgraded](https://github.com/dotnet/source-build/blob/main/Documentation/sourcebuild-in-repos/update-dependencies.md)
and [new dependencies are added](https://github.com/dotnet/source-build/blob/main/Documentation/sourcebuild-in-repos/new-dependencies.md)
to .NET. The [generate script](https://github.com/dotnet/source-build-reference-packages/blob/main/generate.sh)
supports generating new packages. Run `generate.sh --help` for usage details.

When generating a package(s), the tooling will detect and generate all dependent packages.

### Reference

``` bash
./generate.sh --package system.buffers,4.5.1
```

After generating new reference packages, all new projects must be referenced as a
[DependencyPackageProjects](https://github.com/dotnet/source-build-reference-packages/blob/main/eng/Build.props#L9).
These must be defined in dependency order. There is a
[tracking issue](https://github.com/dotnet/source-build/issues/1690) to address this manual step.

The tooling does not handle all situations and sometimes the generated code will need manual tweeks to get
it to compile. If this occurs when generating a newer version of an existing package, it can be helpful to
regenerate the older version to see what customizations to the generated code were made.

#### Workflow

* Generate reference package and its depencencies running the `./generate.sh --package <package>,<version>` script.
* Inspect any changes to packages that already existed in the repository. There are two reasons why previously
generated packages show changes when being regenerated.
    1. The package contains intentional code modifications on top of the generated code. This may be upgrading a
    project reference to address a CVE or code fixups because the generate tooling does not support a scenario.
    When this occurs, there should be code comments explaining why the code modification was made. If this is
    the case, the changes to the existing package should be reverted.
    2. The generate tooling has changed since the last time this package was generated. The new changes should
    be considered better/correct and should be committed.
* Add `DependencyPackageProjects` for all new projects in the
[eng/Build.props](https://github.com/dotnet/source-build-reference-packages/blob/main/eng/Build.props#L9)
in the correct dependency order.
* Run build with the `./build.sh -sb` command.
* If the compilation produces numerous compilation issue - run the `./build.sh --projects <path to .csproj file>`
command for each generated reference package separately. It may be necessary to manually tweak the code to
address compilation issues. When this occurs, please ensure there is an [tracking issue](#filing-issues) to
address the underlying problem with the generator.
* Add comments calling out any modifications to the generated code that were necessary.

You can search for known issues in the [Known Generator Issues Markdown file](docs/known_generator_issues.md).

### Targeting

Generating new targeting packages is not supported. No new targeting packs should be needed/added. If you feel
a new targeting pack is needed, please [open a new issue](#filing-issues) to discuss.

### Text Only

``` bash
./generate.sh --type text --package microsoft.build.traversal,3.1.6
```

## Vulnerable Packages

CVEs may exist for reference packages included in this repo. If they are mitigated by a newer version, the
newer version should be added, the vulnerable version should be removed, and references to the vulnerable
package within other reference packages should be upgraded. A comment should be added to indicate when
packages were manually upgraded.

``` xml
    <!-- Manually updated version from 4.3.0 to address CVE-2017-0247 -->
    <PackageReference Include="System.Net.Security" Version="4.3.1" />
```

## Filing Issues

This repo does not accept issues. Please file issues in the
[source-build repository](https://github.com/dotnet/source-build/issues/new/choose).

## Cleanup

Periodically, a query is ran in a source-built environment to detect unused reference packages. These
unreferenced packages will be deleted.

## License

This repo is licensed with [MIT](LICENSE.txt).
