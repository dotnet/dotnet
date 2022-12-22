# Blob Feed

## Why Blob Feed
More than 15% of our build failures are due to MyGet errors. These are both restore and publish errors and have ranged from network connectivity to inability to push/locate packages already on the feed due to service issues.
To address these issues, the idea is to remove our dependency on MyGet as a part of official builds.

## Requirements
 * Needs to support packages, symbols and other build assets
 * Needs to have a configurable retention policy plan
 * Needs to be based on the NuGet protocol

## Solution
Utilise Azure blob storage as the backing storage. A participent repo's official build will push it's build output to a single feed.

## How it works
```
   Typical Feed url - https://<STORAGEACCOUNT>.blob.core.windows.net/<CONTAINER>/<RELATIVEPATH>/index.json
```
Blob feeds have the same file layout that a NuGet v3 feed would typically use in blob storage. This consists of a root service index.json file, which contains the base url of the feed. Each package that lives in the feed has its own package name folders and each of these folders have versions inside them. Each package folder has an index.json, which contains all the versions that the package supports. These package index.json are generated during the push step.

We upload the nupkg, then generate packages index.jsons on disk then upload and overwrite the package index.json. The generation of the index.jsons is to support NuGet v3. These blob feeds currently don't support the NuGet.exe install operation.

## Structure of the feed inside the Storage Account

```
CONTAINER: dotnet-core
    |
    +-- packages
    |   +-- index.json
    |   +-- Package1
    |   |   +-- Version1
    |   |       +-- Package1.Version1.nupkg
    |   |   +-- Version2
    |   |       +-- Package1.Version2.nupkg
    |   |   +-- index.json
    |   +-- Package2
    |       +-- Version1
    |       |   +-- Package2.Version1.nupkg
    |       +-- index.json
    +-- symbols
    |   +-- Package1
    |   |   +-- Version1
    |   |   |   +-- Package1.Version1.symbols.nupkg
    |   |   +-- Version2
    |   |   |   +-- Package1.Version2.symbols.nupkg
    |   +-- Package2
    |       +-- Version1
    |           +-- Package2.Version1.symbols.nupkg
    +-- assets
        +-- Installer.msi
        +-- Installer.deb
        +-- Installer.rpm
        +-- BuildOutput.zip
        +-- BuildOutput.tar.gz
```

## Moving to Blob Feed

The current dotnet-core main blob feed is https://dotnetfeed.blob.core.windows.net/dotnet-core/packages/index.json. This is publically accessible, meaning anyone can restore the packages.

### Publishing:

To publish packages to this feed requires a dependency on the Microsoft.Dotnet.Build.Tasks.Feed package which lives in buildtools. The repos must create a step in their build specific to publishing and use the PushToBlobFeed task in the tool.

## To use PushToBlobFeed

An example consumption can be found in the standard repo in PR https://github.com/dotnet/standard/pull/548.

1. Add a reference to the latest Microsoft.DotNet.Build.Tasks.Feed which lives in the buildtools myget feed (https://dotnet.myget.org/F/dotnet-buildtools/api/v3/index.json) and restore to a known location.

Ex: https://github.com/dotnet/standard/pull/548/files#diff-c817b27c74ebbbcdac5a58a66668b503R11

2. Decide the feed to which you would like to push. (the storage account, container, and relativepath)

    ```
       Typical Public Feed Url - https://<STORAGEACCOUNT>.blob.core.windows.net/<CONTAINER>/<RELATIVEPATH>/index.json
    ```
This will genererally be passed in to you via a property from the build definition and needs to be passed as the ExpectedFeedUrl
property for the task.

Ex: See ExpectedFeedUrl property being pass from definition https://github.com/dotnet/standard/pull/548/files#diff-4caa8c56eee8a6602df1fde15da88400R109.

3. If you are consuming from a project that didn't do the retore be sure to import the targets file.

    ```
      <Import Project="$(PackagesDir)/$(FeedTasksPackage.ToLower())/$(FeedTasksPackageVersion)/build/$(FeedTasksPackage).targets" />
    ```
Ex: https://github.com/dotnet/standard/pull/548/files#diff-8ac86cae397225debd70633300ab6d49R4


4. Construct an ItemGroup of packages you want to push to your blob feed

    ```
    <ItemGroup>
        <ItemsToPush Include="$(MSBuildThisFileDirectory)/Release/**/*.nupkg" />
    </ItemGroup>
    ```
Ex: https://github.com/dotnet/standard/pull/548/files#diff-8ac86cae397225debd70633300ab6d49R4

5.  Add the call to PushToBlobFeed
    ```
    <PushToBlobFeed ExpectedFeedUrl="Your Feed Url"
                    AccountKey="Your Storage Account Key"
                    ItemsToPush="@(ItemsToPush)"
                    Overwrite="$(PublishOverwrite)" />
    ```
If you have assets to push without create a NuGet Feed in flat, set PublishFlatContainer=true.
If you would like to overwrite, set Overwrite=true.

Ex: https://github.com/dotnet/standard/pull/548/files#diff-8ac86cae397225debd70633300ab6d49R17

### Consumption:

Consuming a blob feed works the same as consuming any other NuGet feed. You add a reference to the index.json file to your
NuGet.config or your restore sources. It is also good practice to provide a way to have your build definition configure this
feed via a property so the feed can be controlled while queuing builds.

## FAQ

1. What happens when we need to update an external package dependency?

You'll need to manually push the package to the blob feed. The general process to follow is sync all your dependencies locally and then use PushToBlobFeed to push them to the feed.

2. Does the blob feed support overwriting?

Yes. You can overwrite any package or asset uploaded.

3. Does blob feed support private secure feeds?

Yes. This is currently supported and requires a proxy to be stood up. Please contact dnceng.

4. Does the blob feed support symbol packages?

Not yet so for publishing them put them in a flat container until support is added for the symbol package indexing.

<!-- Begin Generated Content: Doc Feedback -->
<sub>Was this helpful? [![Yes](https://helix.dot.net/f/ip/5?p=Documentation%5CProject-Docs%5CBlobFeeds%5CBlob-Feeds-Intro.md)](https://helix.dot.net/f/p/5?p=Documentation%5CProject-Docs%5CBlobFeeds%5CBlob-Feeds-Intro.md) [![No](https://helix.dot.net/f/in)](https://helix.dot.net/f/n/5?p=Documentation%5CProject-Docs%5CBlobFeeds%5CBlob-Feeds-Intro.md)</sub>
<!-- End Generated Content-->
