## Automated GitHub tagging for failed Pull requests from Maestro++ dependency flow

### Limitations:

- This functionality only makes sense when the subscriptions involved are non-batched, and documentation will call this out. We only want to tag a partner team if we're fairly sure that it's worth someone from the source repo to take a look.
- We won't be able to detangle whether the root cause of a failure is the target repo build's flakiness, an intentional breaking change from the source repo that was advertised via email, or a break from changes that lack test coverage from the source repository, so a list of these posssibilities will be part of the message where source repos get tagged.

See Epic: https://github.com/dotnet/core-eng/issues/12136

### Stakeholders

While we hope that the features added in this epic provide benefit to any "target" repo of dependency flow, the primary stakeholder currently is the .NET SDK team. They exist at the end of the metaphorical dependency-flow river, so they experience potential issues with dependency updates from every layer in the stack, possibly in a synergistic fashion (i.e. the regression requires two or more repos' changes).

### Risk

- What are the unknowns? 

  Unknowns are minimal for the work as described; the main being simply "will this improve how long it takes to look at a PR failure by the source repository of the build change?

- Are there any POCs (proof of concepts) required to be built for this epic? 

  No. This will be adding functionality to existing components that already do most of the related behaviors; after review it looks like every challenging thing done already has some component that could be augmented to support this.

- What dependencies will this epic have? Are the dependencies currently in a state that the functionality in the epic can consume them now, or will they need to be updated? 

  None.  All work will occur in the arcade-services repo (unless we decide to store the mappings in another)

- Will the new implementation of any existing functionality cause breaking changes for existing consumers? 

  No breaking changes; the idea is simply to add logging and tagging to an existing semi-mature process.

- Is there a goal to have this work completed by, and what is the risk of not hitting that date? (e.g. missed OKRs, increased pain-points for consumers, functionality is required for the next product release, et cetera)

  Goal is to finish by end of April 2021. Slipping the date has no associated risk, just means that the SDK team and other "end-of-the-line" repos will continue to operate as they are currently for some time.

### Serviceability

- How will the components that make up this epic be tested? 
  - Add tests to existing unit tests in the area (expanding on existing)
  - Arcade-services scenario test. (Tests using checks already exist, will be an exercise in code reuse for this somewhat similar scenario.)

- How will we have confidence in the deployments/shipping of the components of this epic? 

  Regularly run scenario and unit tests that exercise this code path always run before deployment to production.

- Identifying secrets (e.g. PATs, certificates, et cetera) that will be used (new ones to be created; existing ones to be used).
  
  No new secrets will be needed by the change.

- Does this change any existing SDL threat or data privacy models? (models can be found in [core-eng/SDL](https://github.com/dotnet/core-eng/SDL) folder)
- Does this require a new SDL threat or data privacy models?  

  No; the only PII used will be GitHub aliases / tags and this will be augmenting existing functionality.

- Steps for setting up repro/test/dev environments?

  Same as https://github.com/dotnet/arcade-services/blob/main/docs/DevGuide.md

#### Rollout and Deployment

This section left blank as this will be part of an arcade-services component.

### Usage Telemetry
- How are we tracking the “usefulness” to our customers of the goals? 

  Since there can be lots of reasons a PR is merged quickly or slowly, pure data metrics likely do not help here unless we somehow had many more data points.  However, we can monitor:
  - Feedback from SDK and other end-of-the-line repo team members
  - Whether we observe incidence of an upstream repo team member taking action before the downstream team raises the issue with them.

- How are we tracking the usage of the changes of the goals? 

  - We already use Application Insights for telemetry.  The idea here will be to start tracking the success rate % of PRs from a given repo to another.  By simply writing down source, destination, and success of the PR build we'll be able to identify which teams flow problems out of their repositories. It should be straightforward to make a "top N list" of repos whose changes break others, and dig in to the data from there.

### Monitoring 

  Already covered by the Dependency Flow error processor and Grafana alerts.

### FR Hand off
- What documentation/information needs to be provided to FR so the team as a whole is successful in maintaining these changes? 

  The location of mappings for Repository URL -> GitHub Tag, and instructions to update these.

### Description of the work:

#### Components changed:

- New function: TagSourceRepositoryGitHubContacts()
  - Reads config file from below, ensure a message is created in the PR to come check out the failure. Will reuse as much as possible from existing GitHub helper functions in Pull Request Actor.

- Changes to [PullRequestActor](https://github.com/dotnet/arcade-services/blob/main/src/Maestro/SubscriptionActorService/PullRequestActor.cs)
   - NonBatchedPullRequestActorImplementation changes:
       - Pass along a flag inside its overload of SynchronizeInProgressPullRequestAsync() to indicate that repo-tagging should occur (this can only happen in non-batched, since it's not feasible to detangle who broke what in a batched update)
       - This is needed to ensure that UpdatePullRequestAsync() gets called to check
   - PullRequestActorImplementation changes:
      - UpdatePullRequestAsync() would check first if it was being called inside a non-batched PR actor, and if so it would also check the existing field for the interesting state (`InProgressPullRequest.MergePolicyCheckResult == MergePolicyCheckResult.FailedPolicies`)
      - Before continuing to updating the PR with new commits it would fetch the `dependency-flow-failure-notifications.json` file described below (or just have it as a local asset in the Maestro++ service), check to see if any failure tagging is requested, and apply a comment to the issue with all unique tags which have been requested.
      
      - SynchronizePullRequestAsync already has an entry in [its switch statement](https://github.com/dotnet/arcade-services/blob/main/src/Maestro/SubscriptionActorService/PullRequestActor.cs#L424-L426), so we'd have a different actionresult added for this case.
   - For the `case MergePolicyCheckResult.FailedToMerge:` entry, we'd add a new enum value to SynchronizePullRequestResult (e.g. "InProgressCanUpdateNeedsNotification")
   - At this point SynchronizeInProgressPullRequestAsync calls the new function (TagSourceRepositoryGitHubContacts()) to ensure that a boilerplate message is pasted in with the tags from the config file.
   - Telemetry changes:  
      - Send telemetry with source and destianation repos whenever a non-batched PR is created.  
      - Whenever we tag an issue due to failed checks, send additional piece of telemetry with the same information indicating failure
      - Could just send once at the very end, but this might miss sending check failure telemetry on PRs that get manually fixed up to pass, or are manually merged.

- Changes to allow users to self-service dependency-flow failure notifications:
  - Introduce a new file, say `https://github.com/dotnet/arcade-services/blob/main/dependency-flow-failure-notifications.json` sample content:

``` json
  {
    [
      // Example showing options for disabling and specifying particular channels
      {
        // Notify @dotnet/runtime-infrastructure for any runtime content coming from the .NET 6 channel
        // targeted to the Sdk and ASP.NET Core teams where PRs fail.
        "SourceGitHubRepoUrl": "https://github.com/dotnet/runtime",
        "TargetGitHubRepoUrls": 
        [
          "https://github.com/dotnet/sdk",
          "https://github.com/dotnet/aspnetcore",
        ],
        "NotificationSettings": [{
            "GitHubTagsToNotify": [
              "@dotnet/runtime-infrastructure"
            ],
            "Channels": [
              ".NET 6",
            ]
          },
          // Notify a different tag for servicing branches
          {
            "GitHubTagsToNotify": [
              "@dotnet/runtime-infrastructure-servicing"
            ],
            "Channels": [
               ".NET 5",
               ".NET 3"
            ],
            // leave the ability to disable in case failures are expected
            "Enabled": false
          }
        ]
      },
      // Minimal example: always alert on all channels for PRs made from Nuget.Client -> Dotnet SDK
      {
        "SourceGitHubRepoUrl": "https://github.com/nuget/nuget.client",
        "TargetGitHubRepoUrls": 
        [
          "https://github.com/dotnet/sdk"
        ],
        "NotificationSettings": [{
            "GitHubTagsToNotify": [
              "@nuget/nuget-infrastructure",
            ]
          }
        ]
      }
    ]
  }
```

#### Notes
- All subscriptions to GitHub PR failure tagging must be OK with the "source" repo. That is, target repositories causing failure notifications for teams that do not want these notifications will have them removed.  A blurb to this effect will be part of the failure notification tag boilerplate message.
- See related feature request: https://github.com/dotnet/arcade/issues/7102


<!-- Begin Generated Content: Doc Feedback -->
<sub>Was this helpful? [![Yes](https://helix.dot.net/f/ip/5?p=Documentation%5CTeamProcess%5COne-Pagers%5Cpr-failure-tagging-one-pager-core-eng-12136.md)](https://helix.dot.net/f/p/5?p=Documentation%5CTeamProcess%5COne-Pagers%5Cpr-failure-tagging-one-pager-core-eng-12136.md) [![No](https://helix.dot.net/f/in)](https://helix.dot.net/f/n/5?p=Documentation%5CTeamProcess%5COne-Pagers%5Cpr-failure-tagging-one-pager-core-eng-12136.md)</sub>
<!-- End Generated Content-->
