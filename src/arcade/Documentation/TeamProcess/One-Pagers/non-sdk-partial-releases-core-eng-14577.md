# Non-SDK Partial Releases for 6.0

## Summary

In general, we only have two types of releases: full releases (which consist of sdk, runtime, aspnetcore, and windows desktop artifacts) and sdk-only releases (which only consist of the sdk, and rely on previously released versions of runtime, aspnetcore and windows desktop). However, in the 6.0 timeframe, we now find ourselves requiring a non-sdk partial release (i.e. a runtime-only or runtime and aspnetcore-only release), due to requirements for MAUI. Specifically, we will need to publish all of the workload packs and packages for this process.

## Stakeholders

* .NET Core Engineering Services team (contact: @dnceng)
* .NET SDK team
* .NET MAUI team
* .NET Release team

## Risks

* What are the unknowns?
  * Today, it is unclear how to identify which files we will want to publish and how. Many are in the "workloads" directory for a given repo (runtime, emsdk), but the nupkgs are all in a flat directory structure. While we could publish all of the new nupkgs, it's unclear if we would actually want to. The safest option is to just publish all of the artifacts that are part of the release, rather than trying to pick and choose. This is what we should do, as it is simpler, safer, and gives us more flexibility for using this work in the future.
  * We do not know how much of the infrastructure around releases relies on there being an actual sdk version associated with a release. Depending on how much we can work around, we may be able to work within the current infrastructure, but it also might require a full rewrite of some of the initialize release work, including that around linux signing, building the release layout, generating the release metadata (if we want to do that, which is also an unknown), etc.
  * We do not know how much of the full release process we will require in a runtime-only release, and how that will interact with our scheduled releases. We want to make sure that we reuse and share as much of the current infrastructure and processes as we can; however, it may require some one off work that would only work for these non-sdk partial releases.

* Are there any POCs (proofs of concept) that need to be built for this work?

  There is nothing particularly new that we should need to build for this work that doesn't already exist as part of the Stage-DotNet and Release-DotNet-5.x pipelines. While we will require some modifications to the current system that may necessitate a new pipeline on the release side, the staging pipeline should be able to be used mostly, if not completely, unchanged.

* What dependencies will this work have? Are the dependencies currently in a state that the functionality in the work can consume them now, or will they need to be updated?

  This work depends on the work that the runtime, emsdk and other repositories have done to allow for releases of workloads (namely, the workloads directory that these repositories now produce). The work may also rely on a new naming schema for the workload nupkgs, if we decide we need to do that. Finally, the work relies on the Stage-DotNet and Release-DotNet-5.x pipelines that currently perform our releases, and the processes that those pipelines do today. These pipelines and/or processes will likely need to be updated in order to support this new type of release.

* Will the new implementation of any existing functionality cause breaking changes for existing consumers?

  It should not. Wherever possible, we should add existing functionality that is exclusive to non-sdk partial releases under option flags that do not affect existing infrastructure.

* Is there a goal to have this work completed by, and what is the risk of not hitting that date? (e.g. missed OKRs, increased pain-points for consumers, functionality is required for the next product release, et cetera)

  This work is required for MAUI to release pre-.NET 7. What will trigger the need for this work is a big runtime change that is too risky to immediately go into .NET 6 GA or servicing, but is required for MAUI. We will likely need this by early 2022. While this work is currently specifically for MAUI bring up today, it may likely be needed for shipping preview versions of servicing releases of .NET 6 with potentially risky changes.

## Open Questions

* What epic should this be part of?

  It is strongly recommended that this work go into its own epic

* What needs to be updated?

  It's unclear, as the staging pipeline fails early (in linux signing), and everything later in the pipeline is gated on the signing stage. Once we have a clear picture of the staging pipeline, we can get a handle on what changes need to be done in the release pipeline.

* Will this be treated differently than a full release or an sdk-only release?

  Probably, as we aren't going to be publishing everything from the drop, unlike what we do for full/sdk-only releases.

## Components to change, with order/estimates of work to do

### Component: Staging pipeline

The Staging pipeline does the work of preparing the release, including gathering the drop, signing the assets, validating the assets, and creating the final release layout. Most of the pipeline is pretty robust and seems to handle well builds that don't contain sdk assets, however some of the pipeline relies on a fully formed release config.json, which we will not have in these releases.

#### High-level activities:

* Update the config file generation to allow us to get runtime version info from the runtime assets (and aspnetcore version from the aspnetcore assets), not just the sdk dependencies. We would like to, as much as possible, automatically detect what kind of release the pipeline has been asked to perform.
* Update initialize-release to allow for config files that are missing versions for sdk, runtime, aspnetcore, etc.
* Determine if we actually need the release metadata, or if that will be unnecessary for this sort of release.
* Modify the yaml to skip anything that is unnecessary for these releases
* Find all the places where we use the sdk version in the yaml, and for these releases, use something else (runtime version, for example). Using the sdk version is currently breaking some publishing code, in addition to the release initialization infrastructure.
* Confirm that VS insertion continues to work properly for this scenario

### Component: Release pipeline

The release pipeline does the actual work of publishing the release, including creating additional metadata files, publishing nuget packages, publishing signed files to dotnetcli, updating aka.ms links, publishing symbols, publishing transport files to public locations, etc.

#### High-level activities:
* Walk through the entire pipeline to see what uses the config file, and how.
* Confirm that anything that uses the release layout (i.e., what we drop to NET_CORE on vsufile), can handle files being missing.
* Update whatever metadata file creation there is to accept a config file that is missing data that is not part of the release.
* Confirm that repo-propagation will handle missing versions gracefully.
* Determine how we will release only the workloads directory, rather than the entire drop.
* Determine how we will identify and publish only those nupkgs that we are interested in releasing for this process.

### Component: Product repositories

The product repositories need to make sure that any workloads that need to be released as part of this process are published as part of the workloads directory. Many already do this today.

#### High-level activities:
* Make sure any repositories we need to be part of this work publish required assets to their workloads directories.
* Where possible, have assets put in a subdirectory that identifies the sdk release version the assets should be associated with
* Potentially rename required nupkgs to identify them as Workload packages, like we do the VS.* packages for other releases.

## Serviceability

* How will the components that make up this epic be tested?

Like the staging pipeline, we will have a test pipeline and BAR Build ID that we can run through the full pipeline for testing. This should be run prior to any main testing and will run on production rollouts. Additionally, all new functionality will have unit tests added to test that they're doing what they are supposed to and don't break prior behavior.

* How will we have confidence in the deployments/shipping of the components of this work?

By using the test pipelines prior to deployment.

## Rollout and Deployment

* How will we roll this out safely into production?

By using the test pipelines with known good builds.

* How often and with what means will we deploy this?

Weekly, along with the rest of dotnet-release, or as needed, if weekly is too frequent.

* What needs to be deployed and where?

Any code related to the staging and release pipelines. We will deploy to the production branch (and potentially later, the release/6.0 branch) from main.

* What are the risks when deploying?

We may break the main pipeline for full releases if we do not do enough testing. With two/three different scenarios that require testing, we may need to run the full test pipeline for each of these types of releases. Instructions for validating any changes made to the pipeline can be found [here](https://github.com/dotnet/arcade/blob/main/Documentation/Staging-Pipeline/making-and-validating-changes.md).

## FR Handoff

* What documantion/information needs to be provided to FR so the team as a whole is successful in maintaining these changes?

Changes to the staging pipeline are already documented in the Staging-Pipeline docs, however, we will likely want to add additional information about the new scenarios.

## Useful Release-Related Documentation and Links

* [Stage-DotNet](https://dev.azure.com/dnceng/internal/_build?definitionId=792)
* [Release-DotNet-5.x](https://dev.azure.com/dnceng/internal/_build?definitionId=984)
* [Running the Staging Pipeline](https://github.com/dotnet/arcade/blob/main/Documentation/Staging-Pipeline/running-the-pipeline.md)
* [Original Release Rings Plan](https://github.com/dotnet/arcade/blob/main/Documentation/ReleaseRingsPlan.md)
* [releases.json](https://github.com/dotnet/core/blob/main/release-notes/6.0/releases.json)

<!-- Begin Generated Content: Doc Feedback -->
<sub>Was this helpful? [![Yes](https://helix.dot.net/f/ip/5?p=Documentation%5CTeamProcess%5COne-Pagers%5Cnon-sdk-partial-releases-core-eng-14577.md)](https://helix.dot.net/f/p/5?p=Documentation%5CTeamProcess%5COne-Pagers%5Cnon-sdk-partial-releases-core-eng-14577.md) [![No](https://helix.dot.net/f/in)](https://helix.dot.net/f/n/5?p=Documentation%5CTeamProcess%5COne-Pagers%5Cnon-sdk-partial-releases-core-eng-14577.md)</sub>
<!-- End Generated Content-->
