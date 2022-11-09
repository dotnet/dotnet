# Fetch Optional (Internal) Tooling

This is an implementation plan for how to fetch sensitive internal tools during a .NET Core build.

## Uploading a new tool

The tool is put in a NuGet package and uploaded to a VSTS feed. VSTS feeds require authentication for any operation, and are secure.

## Fetching during the build

**To fetch internal tooling in your local dev build, see the [Running CoreFx tests on UAP (CoreCLR scenario) OneNote page
](https://microsoft.sharepoint.com/teams/netfx/corefx/_layouts/OneNote.aspx?id=%2Fteams%2Fnetfx%2Fcorefx%2FDocuments%2FCoreFx%20Notes&wd=target%28Engineering%2FNet%20Standard%202.0.one%7CD8792BD0-63D5-4D0F-8EF0-B0F8444F49CD%2FRunning%20CoreFx%20tests%20on%20UAP%20%28CoreCLR%20scenario%5C%29%7C48A101A6-5621-4131-A49C-DA95C155D126%2F%29)**

An `optional-tool-runtime/project.json` file in BuildTools specifies all required tooling that is only available from the internal VSTS feed. This is similar to [`tool-runtime/project.json`](https://github.com/dotnet/buildtools/blob/6a1400e631a097587246e973973e9fafe7ab6254/src/Microsoft.DotNet.Build.Tasks/PackageFiles/tool-runtime/project.json).

In the official build, three properties are set for the `sync` call:

```
OptionalToolSource=https://devdiv.pkgs.visualstudio.com/_packaging/dotnet-core-internal-tooling/nuget/v3/index.json
OptionalToolSourceUser=dn-bot
OptionalToolSourcePassword=******
```

A target in BuildTools runs before the main project package restore, detects that these properties are set, then restores `optional-tool-runtime/project.json` into the `packages` directory. Build steps that need an optional tool can find it using `PrereleaseResolveNuGetPackageAssets`.

The path to the project file can be overridden to specify repo-specific tooling, like in CoreFX: [dir.props#L303](https://github.com/dotnet/corefx/blob/30a0f7f753162b89ad110b4beba3fdeda434fe8c/dir.props#L303), [optional.json](https://github.com/dotnet/corefx/blob/30a0f7f753162b89ad110b4beba3fdeda434fe8c/external/test-runtime/optional.json).

Devs who have the optional tooling packages but don't have convenient access to the VSTS feed can set `OptionalToolSource` to a directory to use it as an optional tool package feed.

If `OptionalToolSource` isn't set, no optional tooling is restored.


<!-- Begin Generated Content: Doc Feedback -->
<sub>Was this helpful? [![Yes](https://helix.dot.net/f/ip/5?p=Documentation%5CProject-Docs%5Cfetch-internal-tooling.md)](https://helix.dot.net/f/p/5?p=Documentation%5CProject-Docs%5Cfetch-internal-tooling.md) [![No](https://helix.dot.net/f/in)](https://helix.dot.net/f/n/5?p=Documentation%5CProject-Docs%5Cfetch-internal-tooling.md)</sub>
<!-- End Generated Content-->
