# xliff-tasks

A set of MSBuild tasks and targets to automatically update xliff (.xlf) files for localizable resources, and to build satellite assemblies from those xliff files.

## Build Status

|Windows x64 |
|:------:|
|[![Build Status][win-x64-build-badge]][win-x64-build]|

## Installing

If you're using the [Arcade Toolset][arcade-toolset] then the `Microsoft.DotNet.XliffTasks` package is already pulled in, and enabled by default.

Otherwise, you'll need to add the Azure DevOps feed `https://pkgs.dev.azure.com/dnceng/public/_packaging/dotnet-eng/nuget/v3/index.json` ([browse](https://dev.azure.com/dnceng/public/_packaging?_a=feed&feed=dotnet-eng)) to your NuGet.config file, and then add a `PackageReference` for the XliffTasks package, like so:

```
<PackageReference Include="Microsoft.DotNet.XliffTasks" Version="1.0.0-beta.19253.1" PrivateAssets="all" />
```

The `PrivateAssets` metadata is needed to prevent `dotnet pack` or `msbuild /t:pack` from listing `Microsoft.DotNet.XliffTasks` as one of your package's dependencies.

## Using Microsoft.DotNet.XliffTasks

### Updating .xlf files

Once `Microsoft.DotNet.XliffTasks` is installed building a project will automatically build satellite assemblies from .xlf files. To _update_ .xlf files to bring them in line with the source .resx/.vsct/.xaml files you need to run the `UpdateXlf` target, like so:

```
msbuild /t:UpdateXlf
```

This will only update the .xlf files. Alternatively, run a normal build with the `UpdateXlfOnBuild` property set:

```
msbuild /p:UpdateXlfOnBuild=true
```

By default, `XliffTasks` will produce an error during build if it detects that the .xlf files are out of data with the source .resx/.vsct/.xaml files.

Many teams using `XliffTasks` default `UpdateXlfOnBuild` to true for local developer builds, but leave it off for CI builds. This way the .xlf files are automatically updated as the developer works, and the CI build will fail if the developer forgets to include the changes to the .xlf files as part of their PR. This way the .xlf files are always in sync with the source files, and can be handed off to a localization team at any time.

Other workflows are possible by changing the `XliffTasks` properties (see below)

### Sorting .xlf files

`XliffTasks` attempts to keep .xlf files sorted when inserting new items. This doesn't matter for the generation of satellite assemblies, but can reduce merge conflicts when localizable resources are being added in multiple branches (as opposed to always adding new items at the end, which more or less guarantees merge conflicts).

Note `XliffTasks` does not force the items into a sorted order if they are not already sorted. You can do that manually by running `msbuild /t:SortXlf`.

## Project Properties

`EnableXlfLocalization` - The "master switch" for turning locallization with `XliffTasks` on or off completely. When set to false, .xlf files will not be updated and satellite assemblies will not be generated from the .xlf files, regardless of the other properties. Defaults to true, but it is useful to set it to false for any project that does not need to produce localized resources (unit test projects, packaging projects, etc.).

`UpdateXlfOnBuild` - When set to true, .xlf files will automatically be brought in sync with the source .resx/.vsct/.xaml files. This may involve adding or removing items from the .xlf files, or creating new .xlf files. Defaults to false.

`ErrorOnOutOfDateXlf` - When set to true the build will produce an error if the .xlf files are out-of-date with respect to the source files. Defaults to true.

`XlfLanguages` - The set of locales to which the project is localized. Defaults to the thirteen locales supported by Visual Studio: `cs;de;es;fr;it;ja;ko;pl;pt-BR;ru;tr;zh-Hans;zh-Hant`.

## Source File Properties

`XlfInput` - Set this to false to opt out of .xlf file generation for a specific source file that would otherwise be included by default.

## Contact

For more information, contact @dotnet/dnceng on GitHub, or file an issue.

[win-x64-build-badge]: https://dev.azure.com/dnceng/internal/_apis/build/status/dotnet/xliff-tasks/dotnet-xliff-tasks-official-ci?branchName=main
[win-x64-build]: https://dev.azure.com/dnceng/internal/_build?definitionId=485&branchName=main
[arcade-toolset]: https://github.com/dotnet/arcade
