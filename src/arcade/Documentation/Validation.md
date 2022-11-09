# Validating Product Builds

Validating builds is an important part of producing a releasable product. There are several levels of validation that are performed on product builds at various phases of the process:

* CI/PR build time
  * Repository-level functional testing. Owned by the product teams and defined in the repositories, and used to identify bugs
* Official build time (optionally)
  * Source code validation: this includes SDL testing and localization testing. Used to confirm that source code meets Microsoft's standards.
  * Package validation: includes sourcelink validation, symbols validation, nupkg metadata validation. Used to confirm that customers will be able to install and debug packages from .NET.
* Nightly validation pipeline (optionally)
  * Source code validation: this includes SDL testing and localization testing. Used to confirm that source code meets Microsoft's standards.
  * Package validation: includes sourcelink validation, symbols validation, nupkg metadata validation. Used to confirm that customers will be able to install and debug packages from .NET.
  * Signing validation: Used to validate that all bits that we ship have been signed properly.

While many of these validation steps can be performed in official builds, the supported model is to onboard to the nightly validation pipeline. All of these steps will be performed as part of shipping the product, and all product teams have the option of running these exact validation steps at a nightly cadence.

## How do we validate .NET?

Pre-.NET 5, validation for the core .NET repositories were done during the official builds. For .NET 5, all post-build validation and source code validation was moved out of the official build and into a separate nightly pipeline, [Validate-DotNet](https://dev.azure.com/dnceng/internal/_build?definitionId=827). This pipeline runs all the same validation that is performed on the full product when we go to release, and is a smoke test for product teams so that they can address any issues well before release day.

Validate-DotNet is controlled by a separate scheduling pipeline, which, once a day, looks for the newest build for each repository that has been onboarded on every channel that those repositories publish to. If no build is found from the previous 24 hours, no validation run will be started for that channel/repository combination. For any repository that has published a new build to the BAR database for a given channel, a new Validate-DotNet run will be created.

To gather all of the assets to be validated, Validate-DotNet uses information found in the BAR database. Specifically, it uses the `darc gather-drop` command to pull down all of the assets that were created by a particular build (but no downstream assets). The pipeline will then sign the build before running the various validation checks on that build, including:

* Signing Validation
* SDL Validation (which will open TSA issues for any failures found)
* Localization Validation
* NuGet Metadata Validation
* Sourcelink Validation
* Symbols Validation
* NuGet Package Icon Validation
* Checksums Validation

Please note, we are always adding new validation steps to the pipeline, so this is not an exhaustive list.

## Why move validation out of official builds?

As a part of .NET 5, we had a goal of two hour build turn-around. In order to close in on that goal, we removed many things from official builds, including post-build validation.

## How do I onboard to Validate-DotNet?

[Validate-DotNet](https://dev.azure.com/dnceng/internal/_build?definitionId=827) is a pipeline that automatically runs nightly validation for all repositories that have been onboarded. Onboarding to Validate-DotNet is quite simple:

1. Update the [list of repositories](https://dev.azure.com/dnceng/internal/_git/dotnet-release?path=%2Feng%2Fpipeline%2Ftools%2Frepos-to-validate.txt) in [dotnet-release](https://dev.azure.com/dnceng/internal/_git/dotnet-release) with your repository's URL. Please reach out to dnceng for PR approval.
2. To enable nightly SDL runs, add the [sdl-tsa-vars.config](https://github.com/dotnet/runtime/blob/main/eng/sdl-tsa-vars.config) file to your repository. This file should include all of the necessary information specific to your repository for creating SDL issues.

Once the first step is complete, your repository will start validating on a nightly basis. Once the second step is complete, you will also get SDL validation.

## How do I know if there are failures in my validation runs?

There are two main ways of checking your validation results: manually, and using automatic notifications.

### Manually looking at the pipeline

Validate-DotNet runs each day at 4 PM Pacific Time. Each run is tagged with the repository name and the channel of the given run (to distinguish between the various versions). To look at the runs for only your repository, you can use the following URl, with your repository name in place of `<repo>` `https://dev.azure.com/dnceng/internal/_build?definitionId=827&tagFilter=<repo>`. For example, if you wanted to look at results for runtime, you would navigate to `https://dev.azure.com/dnceng/internal/_build?definitionId=827&tagFilter=runtime`.

### Automatic Notifications

[Build Monitor](https://dev.azure.com/dnceng/internal/_wiki/wikis/DNCEng%20Services%20Wiki/185/BuildFailureManagement) automatically monitors Validate-DotNet builds for a particular repository and opens issues when failures occur. If an issue is already open for Validate-DotNet failures in your repository for builds on a given channel, it will append a new comment.

To onboard, you will need to update both the `Builds` array and the `Issues` array in Build Monitor's [settings.json](https://github.com/dotnet/arcade-services/blob/main/src/DotNet.Status.Web/.config/settings.json#L23).

The `Builds` array controls which Azure DevOps pipeline builds will be monitored. To monitor Validate-DotNet runs for your repository, you will need to add a new item with the following information:

```json
{
    "Project": "internal",
    "DefinitionPath": "\\dotnet-release\\Validate-DotNet",
    "Branches": [ "main" ],
    "IssuesId": <id that matches an item in the Issues array>,
    "Tags": [ <repository name> ]
}
```

For example, for runtime, we would add

```json
{
    "Project": "internal",
    "DefinitionPath": "\\dotnet-release\\Validate-DotNet",
    "Branches": [ "main" ],
    "IssuesId": "dotnet-runtime-infra",
    "Tags": [ "runtime" ]
}
```

The `Issues` array controls where issues will be opened for builds that fail. You will need to update the `Issues` array in the same settings.json file, with the following information:

```json
{
    "Id": <id that matches the IssuesId in your Builds entry>,
    "Owner": <project>, 
    "Name": <repository>, 
    "Labels": [ <infrastructure label> ],
    "UpdateExisting": true // True if you want issues to be updated. False if you want new issues for every failure
}
```

For example, for runtime, we would do:

```json
{
    "Id": "dotnet-runtime-infra",
    "Owner": "dotnet",
    "Name": "runtime",
    "Labels": [ "area-Infrastructure" ],
    "UpdateExisting": true
}
```

### What do I do if an issue is opened in my repository?

Validation failures come in many forms. Most will be actual problems found with the assets in a drop for your repository. These are the responsibility of the product teams to fix. Any failures in the `Required Validation` stage should be fixed as soon as possible, as they are possible release blockers.

Some failures may be infrastructure issues. If you believe this is a case, please reach out to [DNCEng First Responders](https://dev.azure.com/dnceng/internal/_wiki/wikis/DNCEng%20Services%20Wiki/107/How-to-get-a-hold-of-Engineering-Servicing), and someone will help diagnose and fix the issue found.

Errors we commonly see in validation jobs include:

* Signing Validation
  * Files that are not intended to be signed are not listed in the `eng/SignCheckExclusionsFile.txt` for that repository, so validation complains that the files are not signed. Mitigation: add that file type to the `eng/SignCheckExclusionsFile.txt` in your repository.
* SDL Validation (which will open TSA issues for any failures found)
  * Any pipeline failures in these legs should be reported to [DNCEng First Responders](https://dev.azure.com/dnceng/internal/_wiki/wikis/DNCEng%20Services%20Wiki/107/How-to-get-a-hold-of-Engineering-Servicing), as it suggests an infrastructure failure. SDL failures should automatically create TSA issues, which you should address as appropriate.
* Localization Validation
  * Localization is done closer to release time. Localization failures suggest that either the localization team hasn't finished translations, or the translation PR hasn't been checked into your repository and should be. The closer to release we get, the more important fixing these failures becomes.
* Nuget Metadata Validation
  * Metadata is missing. These need to be fixed in the repository, and are shipping blockers.


<!-- Begin Generated Content: Doc Feedback -->
<sub>Was this helpful? [![Yes](https://helix.dot.net/f/ip/5?p=Documentation%5CValidation.md)](https://helix.dot.net/f/p/5?p=Documentation%5CValidation.md) [![No](https://helix.dot.net/f/in)](https://helix.dot.net/f/n/5?p=Documentation%5CValidation.md)</sub>
<!-- End Generated Content-->
