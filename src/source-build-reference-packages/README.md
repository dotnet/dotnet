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

.NET source build currently only supports Linux.

## Building

``` bash
./build.sh -sb
```

## Adding new Packages

New packages are needed from time to time as
[existing dependency versions are upgraded](https://github.com/dotnet/source-build/blob/main/Documentation/sourcebuild-in-repos/update-dependencies.md)
and [new dependencies are added](https://github.com/dotnet/source-build/blob/main/Documentation/sourcebuild-in-repos/new-dependencies.md)
to .NET. The [generate script](https://github.com/dotnet/source-build-reference-packages/blob/main/generate.sh) supports
generating new packages. Run `generate.sh -h` for usage details.

When generating a package(s), the tooling will detect and generate all dependent packages.

### Reference

``` bash
./generate.sh --pkg system.buffers,4.5.1
```

After generating new reference packages, all new projects must be referenced as a
[DependencyPackageProjects](https://github.com/dotnet/source-build-reference-packages/blob/main/eng/Build.props#L10).
These must be defined in dependency order. There is a [tracking issue](https://github.com/dotnet/source-build/issues/1690)
to address this manual step.

The tooling will not pull in icons referenced by the nuspec so they will have to be manually removed. There is a
[tracking issue](https://github.com/dotnet/source-build/issues/1957) to address this manual step.

The tooling does not handle all situations and sometimes the generated code will need manual tweeks to get it to compile.
If this occurs when generating a newer version of an existing package, it can be helpful to regenerate the older version
to see what changes were made.

The tooling has evolved over time and therefore when generating new packages, you may see edits made to existing packages.
This is because the new package has a dependency on an existing package, it was regenerated and changes were detected from
when it was originally generated.

### Targeting

Generating new targeting packages is not supported. No new targeting packs should be needed/added. If you feel a new
targeting pack is needed, please [open a new issue](#filing-issues) to discuss.

### Text Only

``` bash
./generate.sh --type text --pkg microsoft.build.traversal,3.1.6
```

## Filing Issues

This repo does not accept issues. Please file issues in the
[source-build repository](https://github.com/dotnet/source-build/issues/new/choose).

## Cleanup

Periodically, a query is ran in a source-built environment to detect unused reference packages. These unreferenced packages
will be deleted.

## License

This repo is licensed with [MIT](LICENSE.txt).
