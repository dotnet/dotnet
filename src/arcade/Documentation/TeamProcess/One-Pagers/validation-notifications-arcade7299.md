# Notifications For Validate-DotNet

## Summary

For the results of the post-build/nightly validation pipeline to be actionable, repo owners or their representatives need to be notified of failures when they occur. We propose to use [BuildMonitor](https://dev.azure.com/dnceng/internal/_wiki/wikis/DNCEng%20Services%20Wiki/185/BuildFailureManagement) to monitor the results of the validation pipeline, and notify repo owners of the failures in their runs based on the tags associated with each build by opening issues in the repositories and adding the repo's 'Area-Infrastructure' label. With these notifications, we will be able to evangelize Validate-DotNet as the supported post-build validation platform, moving away from running validation in official builds completely. For Stage-DotNet, we can also open issues in core-eng, that we can tag with the Release team stakeholders who would then pass the issue on to the product team responsible for any failures.

## Stakeholders

- .NET Core Engineering Services team (contact: @dnceng)
- .NET Core Release team (contact: @leecow)
- All product teams that currently use, or will in the future use, Validate-DotNet for their validation needs

## Risk

- What are the unknowns?

  The main unknown is how to determine who, for each repository, should be notified on each issue that BuildMonitor will open. We will start by simply adding the repos' infrastructure label to the issues that are created, and we can update that to include an assignee if it becomes desired.

- Are there any POCs (proof of concepts) required to be built for this work?

  No, as this work will simply extend BuildMonitor to be able to differentiate builds based on tags, as well as open issues in repositories other than core-eng.

- What dependencies will this epic have? Are the dependencies currently in a state that the functionality in the epic can consume them now, or will they need to be updated?

  This work depends on the existing BuildMonitor project. This project will need to be updated, but the work is minimal and known.

- Will the new implementation of any existing functionality cause breaking changes for existing consumers?

  Build Monitor will have to be extended so that we can open issues in multiple repositories. Today, BuildMonitor takes a single Issues definition, when we will need to specify multiple. Additionally, we will need a way to map monitors to the repos that they will open issues in. We should be able to do this in a way that does not break current functionality, but it may be a breaking change.

- Is there a goal to have this work completed by, and what is the risk of not hitting that date? (e.g. missed OKRs, increased pain-points for consumers, functionality is required for the next product release, et cetera)

  The goal is to have this work completed by June 2021. There is no risk associated with slipping the date.

## Serviceability

- How will the components that make up this epic be tested?

  - Existing BuildMonitor tests will be extended to include the components added by this work

- Identifying secrets (e.g. PATs, certificates, et cetera) that will be used (new ones to be created; existing ones to be used).
  
  No new secrets will be needed by the change.

- Does this change any existing SDL threat or data privacy models? (models can be found in [core-eng/SDL](https://github.com/dotnet/core-eng/SDL) folder)
- Does this require a new SDL threat or data privacy models?  

  No; the only PII used will be GitHub aliases / labels and this will be augmenting existing functionality.

### Rollout and Deployment

This section left blank as this will be part of an arcade-services component.

## FR Hand off

- What documentation/information needs to be provided to FR so the team as a whole is successful in maintaining these changes? 

  As the changes for this should be minimal, no additional documentation should be needed.

## Description of the work

### Components changed

#### Component: BuildMonitor

- Changes to [BuildMonitorOptions](https://github.com/dotnet/arcade-services/blob/main/src/DotNet.Status.Web/Options/BuildMonitorOptions.cs)
  - Restructure BuildMonitorOptions, AzurePipelineOptions, and/or IssuesOptions so that we can open issues in multiple repos, not just core-eng. This means either having multiple monitors/issues or linking monitor definitions to issue definitions.
- Changes to [AzurePipelineOptions](https://github.com/dotnet/arcade-services/blob/main/src/DotNet.Status.Web/Options/BuildMonitorOptions.cs#L20)
  - Add an "tags" field to BuildDescription. Tags on a pipeline allow you to mark the build with a piece of metadata that you can filter on in the AzDO UI, and is also exposed via the AzDO api for pipelines. We use this data so that product teams can filter the pipeline to only see their builds.
- [ProcessBuildNotificationsAsync()](https://github.com/dotnet/arcade-services/blob/main/src/DotNet.Status.Web/Controllers/AzurePipelinesController.cs#L143)
  - If Tags are set in BuildDescription, compare to the tags for the given build, to determine if this is a build of interest
  - If custom text available, append it to the body of the issue description

#### Component: Repository access

- Build monitor needs to be given access to create issues in each repository that will require notifications. It is currently only enabled in core-eng

#### Component: Schedule-Validation-Pipeline

- Update tags for builds to tag with the repo, the channel, and repo-channel. Right now, we only tag as repo-channel

<!-- Begin Generated Content: Doc Feedback -->
<sub>Was this helpful? [![Yes](https://helix.dot.net/f/ip/5?p=Documentation%5CTeamProcess%5COne-Pagers%5Cvalidation-notifications-arcade7299.md)](https://helix.dot.net/f/p/5?p=Documentation%5CTeamProcess%5COne-Pagers%5Cvalidation-notifications-arcade7299.md) [![No](https://helix.dot.net/f/in)](https://helix.dot.net/f/n/5?p=Documentation%5CTeamProcess%5COne-Pagers%5Cvalidation-notifications-arcade7299.md)</sub>
<!-- End Generated Content-->
