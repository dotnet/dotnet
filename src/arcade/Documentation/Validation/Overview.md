# Arcade Validation

We need to make sure changes done in the Arcade SDK as well as in the [core packages](https://github.com/dotnet/arcade/tree/master/Documentation/CorePackages) don't break any of the consuming repos or Arcade itself. 

## Arcade Validation Policy

- Contributors who are changing existing code should use their best judgement to decide if additional validation against the runtime, aspnetcore and installer repos is necessary. The following is a list of situations in which the contributor may want to run their changes against these repos: 
  - Changes made to Signing, Publishing, or other stages outside of the build stage that would not show up in a PR build. 
  - Changes that affect a fundamental piece of Arcade (e.g. build scripts, install scripts)
  - Changes that affect many files (e.g. refactoring MSBuild Tasks to use a new abstract class for dependency injection support)
  - Changes to packages that are only exercised by a specific set of repos, such as the Shared Framework SDK. 
- If there are any known breaking changes or any breaking changes surface during the validation, those changes should be communicated per the [Breaking Change Policy](../Policy/ChangesPolicy.md).
- Official Arcade builds from main will now be promoted automatically to `.NET Eng - Latest` channel once it has passed the official Arcade Validation pipeline. 

## The process

### Self-build

1. Build the latest changes locally using the specified settings
2. Using darc, update the dependencies based on the packages built in #1
3. Execute the "official build" (this will restore the packages built in #1)

To validate against the Arcade Validation for Promotion pipeline (that includes the ability to build Arcade with the bellwether repos), follow these steps (which are similar to the steps outlined here for [How to Validate a Private Build](https://github.com/dotnet/arcade/blob/master/Documentation/Policy/TestingMSBuildGuidance.md#how-to-validate-a-private-build)):

1. Run a build of your Arcade branch on the [arcade-official-ci](https://dnceng.visualstudio.com/internal/_build?definitionId=6) Azure DevOps Pipeline
2. [Promote your build](../Darc.md#add-build-to-channel) to the "General Testing" Maestro channel. 
3. Create a branch of [arcade-validation](https://github.com/dotnet/arcade-validation)
4. Using darc, run `update-dependencies` ([update-dependencies documentation](../Darc.md#updating-dependencies-in-your-local-repository)) on your Arcade Validation branch to use the build of Arcade you just created in the previous steps. 
5. Push your branch up to Azure DevOps Arcade Validation repository and run a build of your branch on the [dotnet-arcade-validation-for-promotion](https://dev.azure.com/dnceng/internal/_build?definitionId=838&_a=summary) to verify your changes. 
6. It's not necessary to merge your Arcade Validation branch into the repo's main branch, so feel free to delete it when you're done validating your changes.

### '.NET Eng - Validation' channel

Arcade's official builds go to the ".NET Eng - Validation" channel.

### [Arcade-Validation Repository](https://github.com/dotnet/arcade-validation)

This repository contains the scenarios where we validate the last produced version of the SDK as a consumer repository.

#### Validation Process

1. On every Arcade build dependencies will be updated and auto-merged when all the checks pass
2. Arcade validation [official build](https://dnceng.visualstudio.com/internal/_build?definitionId=282) 
is triggered. This will validate the version which was just “pushed” by Arcade

#### Validation Scenarios

The following scenarios are a part of [Arcade Validation](https://github.com/dotnet/arcade-validation)'s build process.

**Build**

The code in the repo is built using the pushed in version of Arcade SDK and core packages. During this phase, packaging, signing and publishing are validated automatically.

**[Signing](https://github.com/dotnet/arcade-validation/tree/master/eng/validation/templates/signing)**

Test and real signing are validated by attempting to sign files [here](https://github.com/dotnet/arcade-validation/tree/master/src/Validation/Resources). 
Since we download all the files in the folder we can easily modify the amount and type of files we sign.

Details on how the Signing package works [here](https://github.com/dotnet/arcade/blob/master/Documentation/CorePackages/Signing.md).

**[Testing](https://github.com/dotnet/arcade-validation/tree/master/eng/validation/templates/testing) (Send jobs to Helix)**

We validate external (anonymous) and internal scenarios of sending jobs to Helix.

Details on how sending jobs to Helix works [here](https://github.com/dotnet/arcade/blob/master/Documentation/AzureDevOps/SendingJobsToHelix.md).

**Publishing to BAR**

When the Build portion of the validation build completes we publish the produced package to BAR. This package won't be consumed by any repo but we want to make sure changes in the SDK did not affect it.

While this is good enough to validate basic scenarios we still have to make sure we validate changes in tier one repos. This will be done post preview 2 as specified in [this](https://github.com/dotnet/arcade/issues/111) epic.


<!-- Begin Generated Content: Doc Feedback -->
<sub>Was this helpful? [![Yes](https://helix.dot.net/f/ip/5?p=Documentation%5CValidation%5COverview.md)](https://helix.dot.net/f/p/5?p=Documentation%5CValidation%5COverview.md) [![No](https://helix.dot.net/f/in)](https://helix.dot.net/f/n/5?p=Documentation%5CValidation%5COverview.md)</sub>
<!-- End Generated Content-->
