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

1. Add the repo as a submodule to `./src`

    ```bash
    git submodule add <remote_url> ./src/<destination_dir>
    git commit -m "<commit_message>"
    ```

1. Define a [project](repo-projects) for the new component. The project
is responsible for building the submodule with the appropriate configuration for
source build. See the [existing projects](repo-projects) for examples.

1. [Build](#how-to-build) locally and resolve any build errors. Source changes
must be applied via [patches](patches). See [below](#patches) for more info on patches.

1. Validate the version of the NuGet packages and binaries produced by the build. See the contents of
`./artifacts/packages/<build_configuration>/NonShipping/Microsoft.SourceBuild.Intermediate.source-build-externals.x.y.z-dev.nupkg`.

1. If the original binaries have strong name signatures, validate the source built ones have them as well.

## Updating an External Component to a Newer Version

1. Update the `src/<external_repo_dir>` to the desired sha

    ``` bash
    cd src/<external_repo_dir>
    git fetch
    git checkout <updated_sha>
    cd ..
    git add .
    git commit -m "<commit_message>"
    ```

1. [Build](#how-to-build) locally

    1. Update any [patches](patches) as needed.

    1. Review the [repo's project](repo-projects) to ensure it is appropriate for the new version.
    There are a number of projects that utilize MSBuild properties to specify the version.
    These need to be manually updated with each upgrade.

    1. Resolve build errors. Source changes must be applied via [patches](patches).  See [below](#patches) for more info on patches.

1. Validate the version of the NuGet packages and binaries produced by the build. See the contents of
`./artifacts/packages/<build_configuration>/NonShipping/Microsoft.SourceBuild.Intermediate.source-build-externals.x.y.z-dev.nupkg`

1. After the PR is merged to update a component, coordination is often needed in the darc dependency flows. The source-build-external update
may need to flow in at the same time as the cooresponding changes in product repos which take a dependency on the new component version.

### Updating an External Component Used in a Pre-SBE Repo

A _Pre-SBE_ repo is a repo that is built before source-build-externals during the product build.

> [!NOTE]
>
> You can view the current pre-SBE repos by running `dotnet msbuild repo-projects/source-build-externals.proj -target:ShowDependencyGraph /p:DotNetBuildSourceOnly=true` from the root of the VMR.

When updating a component that is used in a Pre-SBE repo, please adhere to the following steps:

1. **Add the updated component**: Include the component with the updated version as a new submodule in source-build-externals named `<component>-<new-version>` (this new submodule should be separate from any previously existing version's submodule). Rename the component's old submodule to `<component>-<old-version>`. Also rename the old component's patches directory in `patches/` (if it exists) and the project file in `repo-projects/` to match the submodule name of `<component>-<old-version>`.
    - See [#276](https://github.com/dotnet/source-build-externals/pull/276) for an example.
2. **Update post-SBE repositories (if applicable)**: If your dependency is also in a repo that is built after SBE, allow the updated version to flow to the post-SBE repository and update the post-SBE repository to use this new version.
3. **Build and release**: Perform a build and release the artifacts to update the n-1 (previous) artifacts for the pre-SBE repository.
4. **Update the pre-SBE repository**: Update the pre-SBE repository to use the new version of the component.
5. **Clean up**: Remove the outdated component from source-build-externals, the outdated component's patches directory in `patches/`, if applicable, and the project file in `repo-projects`. Rename the updated component's submodule to `<component>`. Also rename any existing patches directory in `patches/` and the respective project file in `repo-projects/` to match the submodule name, `<component>`.

## Patches

When creating/updating patches, it is desirable to backport the changes whenever feasible as this reduces
the maintenance burden when [updating a component to a newer version](#updating-an-external-component-to-a-newer-version).

## Found an Issue?

Source build related issues are tracked in the [source build repo](https://github.com/dotnet/source-build/).

## License

This repo is licensed under the [MIT](LICENSE.txt) license.
