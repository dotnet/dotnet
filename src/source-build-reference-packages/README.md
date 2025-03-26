# .NET Source Build Reference Packages

This repository contains tools, source and build scripts for source buildable reference
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

.NET source build currently only supports Linux but generating a source build reference or text-only package
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

**Note:** All new packages should be for released stable versions. Adding preview/release candidate
packages are for exceptional cases only and require approval from
[dotnet/source-build](https://github.com/orgs/dotnet/teams/source-build).

**Note:** Reference packages should only be added to this repo if they are required during the product
source build (e.g. a [VMR](https://github.com/dotnet/dotnet) build). Reference packages that are only
required for building a repo level source build should not be added to this repo. In this case, it is
appropriate to add these types of package as allowed prebuilt via the `eng/SourceBuildPrebuiltBaseline.xml`
file. See the [Eliminating pre-builts documentation](https://github.com/dotnet/source-build/blob/main/Documentation/eliminating-pre-builts.md)
for detailed guidance.

### Reference

``` bash
./generate.sh --package system.buffers,4.5.1
```

The tooling does not handle all situations and sometimes the generated code will need manual tweeks to get
it to compile. If this occurs when generating a newer version of an existing package, it can be helpful to
regenerate the older version to see what customizations to the generated code were made.

#### Workflow

* Generate reference package and its depencencies running the `./generate.sh --package <package>,<version>` script.
* Inspect any changes to packages that already existed in the repository. There are two reasons why previously
generated packages show changes when being regenerated.
    1. The package contains intentional code modifications on top of the generated code.
    This may be code fixups because the generate tooling does not support a scenario.
    When this occurs, there should be code comments explaining why the code modification was made. If this is
    the case, the changes to the existing package should be reverted.
    2. The generate tooling has changed since the last time this package was generated. The new changes should
    be considered better/correct and should be committed.
* Run build with the `./build.sh -sb` command.
* If the compilation produces numerous compilation issue - run the `./build.sh --projects <path to .csproj file>`
  command for each generated reference package separately.
  It may be necessary to manually tweak the generated artifacts to address compilation issues.
  When this occurs, please ensure there is an [tracking issue](#filing-issues) to address the underlying problem with the generator.
  When making changes to the generated artifacts, it is recommended to utilize the following pre-defined constructs if possible.

  * Customizations.props - Automatically imported by the generated project. Use it for additive changes such as NoWarns or additional source files.
  * Customizations.cs - Automatically included by the generated project. Use it to add new types or members to partial classes.

  You can search the code base to see example usages.
  The benefit of using these files is that they will be preserved when the packages are regenerated.
* Add comments calling out any modifications to the generated code that were necessary.

You can search for known issues in the [Known Generator Issues Markdown file](docs/known_generator_issues.md).

**Note:** When porting new packages between branches, you must regenerate the packages when crossing the 10.0/9.0 boundary.
This is because in 10.0 the generated projects switched from using PackageReference to ProjectReference.
Porting new packages across 10.0/9.0 boundary will introduce prebuilts.
See the workflow documented in the servicing branch readmes for additional requirements when adding new packages pre 10.0.

### Targeting

Generating new targeting packages is not supported. No new targeting packs should be needed/added. If you feel
a new targeting pack is needed, please [open a new issue](#filing-issues) to discuss.

### Text Only

``` bash
./generate.sh --type text --package microsoft.build.traversal,3.1.6
```

## Regenerating all Packages

As bugs are fixed or enhancements are made to the generate tooling, it may be desirable or necessary to
regenerate the existing packages. The following commands can be used to generate all of the reference packages.

``` bash
find src/referencePackages/src -mindepth 2 -maxdepth 2 -type d | awk -F'/' '{print $(NF-1)","$NF}' > packages.csv
./generate.sh -x -c packages.csv
```

## Vulnerable Packages

CVEs may exist for reference packages included in this repo. Because the packages do not contain any
implementation, they do not pose a security risk. CG is configured in this repo to ignore the reference
packages. If product repos migrate off these vulnerable packages, they can be [removed](#cleanup).

## Filing Issues

This repo does not accept issues. Please file issues in
[dotnet/source-build](https://github.com/dotnet/source-build/issues/new/choose).

## Cleanup

Periodically, packages that are unreferenced by the product source build should be deleted. The number of
unreferenced packages build up over time as the product repositories upgrade their dependencies to newer
versions. Ideally this cleanup would be performed around RC1 timeframe as the product locks down in preparation
for the GA release. To find which packages are unreferenced, you can run a VMR build with the `ReportSbrpUsage`
option to generate an SBRP package usage report. The resulting report will be written to
`artifacts/log/<configuration>/sbrpPackageUsage.json`.

``` bash
./build.sh -sb /p:ReportSbrpUsage=true
```

The VMR CI runs with the `ReportSbrpUsage` option set therefore you can grab the usage report from any build's
artifacts.

## License

This repo is licensed with [MIT](LICENSE.txt).
