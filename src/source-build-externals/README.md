# .NET Source Build Externals

This repo contains the source for components that resides outside of the [dotnet](https://github.com/dotnet)
organization required to build .NET from source. Examples include Newtonsoft.Json
and Application Insights for .NET. Git submodules are utilized to reference the
external source. This repo contains the infrastructure to build these external
repos within the .NET source build. See
[dotnet/source-build](https://github.com/dotnet/source-build) for more details on
.NET source build.

## How to Build

This repo utilizes the .NET [Arcade](https://github.com/dotnet/arcade) build
infrastructure. Since this repo is intended solely for source build, it usually
 makes sense to build with the -sb (source build) flag.

``` bash
./build.sh -sb
```

## Adding a New External Component

1. Add the repo as a submodule to `./src/repos/src`

    ```bash
    git submodule add <remote_url> ./src/repos/src/<destination_dir>
    git commit -m "<commit_message>"
    ```

1. Define a [project](src/repos/projects) for the new component. The project
is responsible for building the submodule with the appropriate configuration for
source build. See the [existing projects](src/repos/projects) for examples.

1. [Build](#how-to-build) locally and resolve any build errors. Source changes
must be applied via [patches](src/repos/patches). See [below](#patches) for more info on patches.

1. Validate the version of the NuGet packages and binaries produced by the build. See the contents of
`./artifacts/packages/<build_configuration>/NonShipping/Microsoft.SourceBuild.Intermediate.source-build-externals.x.y.z-dev.nupkg`.

1. If the original binaries have strong name signatures, validate the source built ones have them as well.

## Updating an External Component to a Newer Version

1. Update the `./src/repos/src/<external_repo_dir>` to the desired sha

    ``` bash
    cd src/repos/src/<external_repo_dir>
    git fetch
    git checkout <updated_sha>
    cd ..
    git add .
    git commit -m "<commit_message>"
    ```

1. [Build](#how-to-build) locally

    1. Update any [patches](src/repos/patches) as needed.

    1. Review the [repo's project](src/repos/projects) to ensure it is appropriate for the new version.
    There are a number of projects that utilize MSBuild properties to specify the version.
    These need to be manually updated with each upgrade.

    1. Resolve build errors. Source changes must be applied via [patches](src/repos/patches).  See [below](#patches) for more info on patches.

1. Validate the version of the NuGet packages and binaries produced by the build. See the contents of
`./artifacts/packages/<build_configuration>/NonShipping/Microsoft.SourceBuild.Intermediate.source-build-externals.x.y.z-dev.nupkg`

1. After the PR is merged to update a component, coordination is often needed in the darc dependency flows. The source-build-external update
may need to flow in at the same time as the cooresponding changes in product repos which take a dependency on the new component version.

### Updating an External Component Used in a Pre-SBE Repo

A _Pre-SBE_ repo is a repo that is built before source-build-externals during the product build.

> [!NOTE]
>
> You can view the current pre-SBE repos by running `dotnet msbuild src/repos/projects/source-build-externals.proj -target:ShowDependencyGraph /p:DotNetBuildSourceOnly=true` from the root of the VMR.

The steps outlined below will enable source-build to adjust the package version to match the N-1 artifacts in the product build. If you prefer to maintain a fixed version of the dependency and prevent source-build from making any changes, please follow the instructions provided [here](https://github.com/dotnet/source-build-externals/blob/83566118e44922c30d146654d42c7c3745cc119d/README.md?plain=1#L81). However, if you are comfortable with source-build infrastructure adjusting your package version, please proceed with the following steps:

1. **Include the component in Versions.props and Version.Details.xml**:

    1. **Locate the Pre-SBE repository**: This is where you'll be making changes.

    1. **Check `eng/Versions.props`**: Look for a version property for your component. If it doesn't exist, you'll need to add it.

    1. **Check `eng/Version.Details.xml`**: Similarly, look for an entry for your component. If it doesn't exist, add it.

    1.  **Add a descriptive comment**: When adding an entry to `Version.Details.xml`, include a comment above the entry that describes what it is.

    1. **Examples**: For reference, you can check these examples of [`eng/Versions.props`](https://github.com/dotnet/arcade/pull/14698/files#diff-1ea18ff65faa2ae6fed570b83747086d0317f5e4bc325064f6c14319a9c4ff67R81) and [`eng/Version.Details.xml`](https://github.com/dotnet/arcade/pull/14698/files#diff-fb62e94a1d6f29f863e3d0a22aa38269f6cd1d7f03b109dc06e2cbf2548b86d3R8).

1. **Update the component**: 

    1. **Wait for changes to propagate**: If you added a Version.Details.xml dependency and corresponding property in the Versions.Props for the component that you are updating, then you need to wait for these changes to flow to the [VMR](https://github.com/dotnet/dotnet) before you can update the component. If there is already a Version.Details.xml dependency and corresponding property in the Versions.Props, then there is no need to wait and you can move on to the next step immediately.

    1. **Update the component**: Once the changes have propagated, you can update the component as usual. For guidance, follow the steps in [`Updating an External Component to a Newer Version`](#updating-an-external-component-to-a-newer-version).

## Patches

1. When creating/updating patches, it is desirable to backport the changes whenever feasible as this reduces
the maintenance burden when [updating a component to a newer version](#updating-an-external-component-to-a-newer-version).

1. Steps to create new patches:

    1. Make changes in the submodule.

    1. Commit changes in the submodule.

    1. From the root directory of the submodule, run [extract-patches.sh](src/repos/patches/extract-patches.sh)/[extract-patches.ps1](src/repos/patches/extract-patches.ps1).
       The script will prepare a patch based on the base sha of the submodule and the latest committed changes. The patch
       will be added to patches/<component>/*.patch

1. To apply a patch, or multiple patches, use `git am` while inside the submodule directory
For example, to apply *all* `humanizer` patches:

```sh
# cd src/humanizer
# git am "../../patches/humanizer/*"
```
## Found an Issue?

Source build related issues are tracked in the [source build repo](https://github.com/dotnet/source-build/).

## License

This repo is licensed under the [MIT](LICENSE.txt) license.
