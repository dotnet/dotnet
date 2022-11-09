# Microsoft.DotNet.Build.Tasks.Archives

Targets package for producing zip and tar archives.

This package generates an archive that can be extracted on top of an existing .NET SDK or Runtime layout. The name of this file is derived from the `ArchiveName` property and the RID. The `ArchiveName` property defaults to the project file name without the extension. This package calls the `PublishToDisk` target on the project to generate the project layout.

## Build Skip Support for Unsupported Platforms and Servicing

This SDK also supports automatically skipping builds on unsupported platforms or in servicing releases. If a project with a list of provided RIDs in `RuntimeIdentifiers` is built with the `RuntimeIdentifier` property set to a RID that is not in the `RuntimeIdentifiers` list, the build will be skipped. This enables cleanly skipping optional packs, installers, or bundles that only exist on specific platforms. 

Additionally, if a `ProjectServicingConfiguration` item is provided with the identity of the project name and the `PatchVersion` metadata on the item is not equal to the current `PatchVersion`, the build will be skipped. This support enables a repository to disable building targeting packs in servicing releases if that is desired.

# Creating tar.gz archives on Windows

There is an override that you can use to opt into generating tar.gz archives instead of zip archives on Windows to get an consistent experience as with linux and macos.
That opt-in is setting ``ArchiveFormat`` to ``tar.gz`` on a project that uses this package when building for Windows.
This can also be used on Linux and MacOS to force creating ``zip`` archives as well.
