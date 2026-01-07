# .NET Source Build Assets

This repository contains source, tools, and infrastructure for auxilary packages required to [build .NET from source](https://github.com/dotnet/source-build).
This repository is often referred to as `SBRP`.

This repo supports the following package types:

1. **External** - A package that resides outside of the [dotnet](https://github.com/dotnet) organization required to build .NET from source.
   Examples include Newtonsoft.Json and Application Insights for .NET.
   Git submodules are utilized to reference the external source.

1. **Reference** - A package that contains API signatures, no implementation.
   It enables developers to build, targeting a specific library version without having the full implementation assembly for that version.

1. **Targeting** - A bundle of reference packages that target a specific .NET TFM.

1. **Text only** - Packages that contain text-only assets.

## Supported Platforms

.NET source build currently only supports Linux but generating reference and text-only packages
is supported on both Windows and Unix based operating systems.

## Building

``` bash
./build.sh -sb
```

## Adding/Upgrading Packages

New packages are needed from time to time as
[existing dependency versions are upgraded](https://github.com/dotnet/source-build/blob/main/Documentation/sourcebuild-in-repos/update-dependencies.md)
and [new dependencies are added](https://github.com/dotnet/source-build/blob/main/Documentation/sourcebuild-in-repos/new-dependencies.md)
to .NET. The following sections describe how to add/upgrade the various types of packages.

* [External](#external)

* [Reference](#reference)

* [Targeting](#targeting)

* [Text only](#text-only)

### External

#### Adding a New External Component

1. Add the repo as a submodule to `./src/externalPackages/src`

    ```bash
    git submodule add <remote_url> ./src/externalPackages/src/<destination_dir>
    git commit -m "<commit_message>"
    ```

1. Define a [project](src/externalPackages/projects) for the new component.
   The project is responsible for building the submodule with the appropriate configuration for source build.
   See the [existing projects](src/externalPackages/projects) for examples.

1. [Build](#building) locally and resolve any build errors.
   Source changes must be applied via [patches](src/externalPackages/patches).
   See [below](#patches) for more info on patches.

1. Validate the version of the NuGet packages and binaries produced by the build.
   See the contents of `./artifacts/packages/<build_configuration>/Shipping`.

1. If the original binaries have strong name signatures, validate the source built ones have them as well.

1. Open a PR.

1. Trigger a full source build within the VMR from your PR by adding a `/azp run source-build-reference-packages-unified-build` comment.
   This will validate the new external will build without adding prebuilts.
   It will also ensure the external does not contain prohibited checked-in binaries.

#### Updating an External Component to a Newer Version

1. Update the `./src/externalPackages/src/<external_repo_dir>` to the desired sha

    ``` bash
    cd src/externalPackages/src/<external_repo_dir>
    git fetch
    git checkout <updated_sha>
    cd ..
    git add .
    git commit -m "<commit_message>"
    ```

1. [Build](#building) locally

    1. Update any [patches](src/externalPackages/patches) as needed.

    1. Review the [repo's project](src/externalPackages/projects) to ensure it is appropriate for the new version.
       There are a number of projects that utilize MSBuild properties to specify the version.
       These need to be manually updated with each upgrade.

    1. Resolve build errors.
       Source changes must be applied via [patches](src/externalPackages/patches).
       See [below](#patches) for more info on patches.

1. Validate the version of the NuGet packages and binaries produced by the build.
   See the contents of `./artifacts/packages/<build_configuration>/Shipping`.

1. Open a PR.

1. Trigger a full source build within the VMR from your PR by adding a `/azp run source-build-reference-packages-unified-build` comment.
   This will validate the new version will build without adding prebuilts.
   It will also ensure the new version does not contain prohibited checked-in binaries.

1. After the PR is merged to update a component, coordination is often needed in the darc dependency flows.
   The source-build-reference-packages source may need to flow in at the same time as the cooresponding changes in product repos which take a dependency on the new component version.
   Sometimes it can be easier to add the new upgraded version along side the older version and then delete the old version after all product repos have been upgraded to the new version.

#### Patches

1. When creating/updating patches, it is desirable to backport the changes whenever feasible as this reduces
the maintenance burden when [updating a component to a newer version](#updating-an-external-component-to-a-newer-version).

1. Steps to create new patches:

    1. Make changes in the submodule.

    1. Commit changes in the submodule.

    1. From the root directory of the submodule, run [extract-patches.sh](src/externalPackages/patches/extract-patches.sh)/[extract-patches.ps1](src/externalPackages/patches/extract-patches.ps1).
       The script will prepare a patch based on the base sha of the submodule and the latest committed changes.
       The patch will be added to `patches/<component>/*.patch`

1. To apply a patch, or multiple patches, use `git am` while inside the submodule directory.
   For example, to apply *all* `humanizer` patches:

   ```sh
   # cd src/externalPackages/src/humanizer
   # git am "../../patches/humanizer/*"
   ```

### Reference

The [generate script](https://github.com/dotnet/source-build-reference-packages/blob/main/generate.sh)
supports generating reference packages.
Run `generate.sh --help` for usage details.

When generating a reference package(s), the tooling will detect and generate all dependent packages.

> **Note:** Reference packages should be for released stable versions. Adding preview/release candidate
packages are for exceptional cases only and require approval from
[dotnet/source-build](https://github.com/orgs/dotnet/teams/source-build).

> **Note:** Reference packages should only be added to this repo if they are required during the product
source build (e.g. a [VMR](https://github.com/dotnet/dotnet) build). Reference packages that are only
required for building a repo level source build should not be added to this repo. In this case, it is
appropriate to add these types of package as allowed prebuilt via the `eng/SourceBuildPrebuiltBaseline.xml`
file. See the [Eliminating pre-builts documentation](https://github.com/dotnet/source-build/blob/main/Documentation/eliminating-pre-builts.md)
for detailed guidance.

``` bash
./generate.sh --package system.buffers,4.5.1
```

The tooling does not handle all situations and sometimes the generated code will need manual tweeks to get
it to compile. If this occurs when generating a newer version of an existing package, it can be helpful to
regenerate the older version to see what customizations to the generated code were made.

> **Suggestion:** Open an [issue](https://github.com/dotnet/source-build-reference-packages/issues/new) that describes the packages to add and assign it to Copilot to do the work (example [issue](https://github.com/dotnet/source-build-reference-packages/issues/1324) and [resulting PR](https://github.com/dotnet/source-build-reference-packages/pull/1325)).

#### Workflow

1. Generate reference package and its depencencies running the `./generate.sh --package <package>,<version>` script.

1. Inspect any changes to packages that already existed in the repository. There are two reasons why previously
generated packages show changes when being regenerated.

    1. The package contains intentional code modifications on top of the generated code.
       This may be code fixups because the generate tooling does not support a scenario.
       When this occurs, there should be code comments explaining why the code modification was made.
       If this is the case, the changes to the existing package should be reverted.
    1. The generate tooling has changed since the last time this package was generated.
       The new changes should be considered better/correct and should be committed.

1. Run build with the `./build.sh -sb` command.

1. If the compilation produces numerous compilation issue - run the `./build.sh --projects <path to .csproj file>`
   command for each generated reference package separately.
   It may be necessary to manually tweak the generated artifacts to address compilation issues.
   When this occurs, please ensure there is an [tracking issue](#filing-issues) to address the underlying problem with the generator.
   When making changes to the generated artifacts, it is recommended to utilize the following pre-defined constructs if possible.

    1. Customizations.props - Automatically imported by the generated project.
    Use it for additive changes such as NoWarns or additional source files.

    1. Customizations.cs - Automatically included by the generated project.
    Use it to add new types or members to partial classes.

   You can search the code base to see example usages.
   The benefit of using these files is that they will be preserved when the packages are regenerated.

1. Add comments calling out any modifications to the generated code that were necessary.

You can search for known issues in the [Known Generator Issues Markdown file](docs/known_generator_issues.md).

> **Note:** When porting new packages between branches, you must regenerate the packages when crossing the 10.0/9.0 boundary.
This is because in 10.0 the generated projects switched from using PackageReference to ProjectReference.
Porting new packages across 10.0/9.0 boundary will introduce prebuilts.
See the workflow documented in the servicing branch readmes for additional requirements when adding new packages pre 10.0.

#### Regenerating all Reference Packages

As bugs are fixed or enhancements are made to the generate tooling, it may be desirable or necessary to
regenerate the existing packages. The following command will regenerate all of the reference packages.

``` bash
./generate.sh -a
```

### Targeting

Generating new targeting packages is not supported.
If you feel a new targeting pack is needed, please [open a new issue](#filing-issues) to discuss.

### Text Only

The [generate script](https://github.com/dotnet/source-build-reference-packages/blob/main/generate.sh)
supports generating text-only packages.
Run `generate.sh --help` for usage details.

``` bash
./generate.sh --type text --package microsoft.build.traversal,3.1.6
```

> **Suggestion:** Open an [issue](https://github.com/dotnet/source-build-reference-packages/issues/new) that describes the packages to add and assign it to Copilot to do the work (example [issue](https://github.com/dotnet/source-build-reference-packages/issues/1324) and [resulting PR](https://github.com/dotnet/source-build-reference-packages/pull/1325)).

## Vulnerable Packages

CVEs may exist for reference packages included in this repo. Because the packages do not contain any
implementation, they do not pose a security risk. CG is configured in this repo to ignore the reference
packages. If product repos migrate off these vulnerable packages, they can be [removed](#cleanup).

## Filing Issues

This repo should contain [issues](https://github.com/dotnet/source-build-reference-packages/issues) that are tied to reference, text-only, external, and target packages used by source build.

Other source build related issues should be opened in [dotnet/source-build](https://github.com/dotnet/source-build/issues/new/choose)

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

> **Note:** [The package usage report does not currently support external packages](https://github.com/dotnet/source-build/issues/3405).

The [source-build-reference-packages-cleanup-unreferenced-packages](https://dev.azure.com/dnceng/internal/_build?definitionId=1426) pipeline can be utilized to remove unreferenced packages.

## License

This repo is licensed with [MIT](LICENSE.txt).
