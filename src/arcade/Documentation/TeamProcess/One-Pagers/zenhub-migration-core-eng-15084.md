# Migration from ZenHub to GitHub Projects (beta)

Going forward, we need to move all of our issue tracking out of ZenHub. The replacement we have chosen is GitHub Projects (beta). While Projects (beta) is not a one-to-one mapping of ZenHub and its concepts, it will fulfill our needs as an issue tracking management system.

## Stakeholders

* .NET Core Engineering Services team (contact: @dnceng)
* .NET Partners who use ZenHub

## Risks

* What are the unknowns?
    * The unknowns of this epic center around the Projects (beta) board as a whole: can it support the number of issues that we have open? What sort of compromises will we have to make with the new structure? Not everything is directly mappable, so we need to keep this in mind. The GitHub roadmap is [here](https://github.com/github/roadmap/projects/1).
* Are there any POCs (proofs of concept) that need to be built for this work?
    * We will want a POC for the migration from the ZenHub board to the Projects (beta) board that takes an issue and adds it to the board, and also that takes an epic issue and converts it into a Projects (beta) field on the board
    * We also will likely want a POC board, though several other teams have their own boards that we can use as examples for how they are being used (see the [.NET Docker board](https://github.com/orgs/dotnet/projects/58/views/1) as an example that uses the board, list, triage and epic views).
* What dependencies will this work have? Are the dependencies currently in a state that the functionality in the work can consume them now, or will they need to be updated?
    * GitHub REST API
    * GitHub GraphQL API - There is no C# API for the Projects (beta) APIs (and the C# API that there is for GraphQL is also in beta), so we are stuck with HTTP requests until we have a C# api to work off of. This could mean changes that happen underneath us could lead to issues that we don't see until we notice that we are missing issues being added to the board.
    * ZenHub API
* Will the new implementation of any existing functionality cause breaking changes for existing consumers?
    * It should not, other than the fact that the ZenHub board will no longer be our source of truth.
* Is there a goal to have this work completed by, and what is the risk of not hitting that date? (e.g. missed OKRs, increased pain-points for consumers, functionality is required for the next product release, et cetera)
    * We want to complete the initial migration by the end of February, when our ZenHub subscription runs out.
    * If at all possible, we should also have the automatic adding of issues/epics to the board done by then as well, so the board doesn't need to be manually managed.

## Open Questions

* Single board vs board-per-epic?
    * Both have pros and cons, but a single board with all of the issues will be required for stand up, and is the most directly comparable to what we have now.

* Where should the board live?
    * Because we may have issues in multiple repositories (as we do now with arcade and core-eng), we need to create the board at the organization level, rather than the repository level. That means to get to the board, we will need to go through https://github.com/orgs/dotnet/projects?type=beta. This will enable folks who are working on issues in non-dnceng repositories to add their issues to the dnceng board (for example, Android/Apple issues in dotnet/runtime).

* What are all the use cases for the ZenHub board, and how will those translate to the new Projects (beta) board?
    * Epic Reviews
        * Reviewing all of the epics to determine business priorities
        * Reviewing a single epic
    * Stand up
        * Reviewing all of the work currently being worked on
        * Either by epic or by status
    * FR stand up
        * Reviewing the FR issues currently being worked on
        * Reviewing the FR backlog
    * Triage
        * Reviewing all issues that have currently not been assigned to an epic
    * Individual user view
        * Reviewing all issues assigned to a particular person

    All of these scenarios will be manageable in Projects (beta). The only things that don't seem translatable are:
    
    * Convert this issue into an epic
        * We will need to manually add the epic label to the issue, and then have our own app or webhook that monitors for this label and adds the Epic issue to the board as an epic
        * Additionally, the issues won't have all of the extra epic stuff (table with all of the issues and their statuses) that ZenHub gives us
    * Sort by assignee on board view
        * Currently, you can group by status and sort by assignee on the table view in Projects (beta), but not in the board view
        * This may cause us to rethink how we run stand-up. Do we even need to be going over issues on an individual basis? Should our stand ups be adjusted?

* How will we translate the concept of an "epic" to Projects (beta)?

We have used Epics in ZenHub as a way to track the concept of business priorities. While GitHub does not have the exact concepts of epics, they are currently implementing features in issues to track issues in other issues: essentially, what we do with ZenHub epics now. This works by adding a task list to each tracking issue containing each issue that is tracked by that issue/epic. After discussions with GitHub, this is the recommended path forward. Additionally, there is a coming feature that will allow us to display the "Tracked in" issue on the Projects (beta) board, which is comparable to what we have with the ZenHub board. The major challenge with this approach is adding issues to an epic/tracking issue. Today, the only way to do so is to update the markdown description of the tracking issue with either the link, org/repo#issueNumber, or issue number (if it's in the same repository) of the tracked issue. This cannot be done from the tracked issue. Issues can be tracked in multiple tracking issues, and GitHub, behind the scenes, creates a tree structure that can be queried for async triage (though that is still in beta as well). Whether or not this query capability will be in REST or only graphql is still a question.

This will be a major change to how we work when it comes to epics. We will need to be cognizant of updating the epic issue when we create a new issue (or, you can just add a checkbox to the task list and use the "Create issue from task" button that will appear after saving). Unlike before, where we went from issue to epic, will need to start thinking in an epic-first way, where we start at the epic and end at the issue.

The nice thing about this is that each issue will have a "tracked by" link under the title, which will allow us to go back to the epic issue easily.

* How will issues be added to the board?
    * Projects (beta) does not have the ability to automatically add issues to the board when they are created
    * Users can manually add issues at creation time
    * We will need to use a webhook that watches for issue creation and adds those issues to the board automatically if the user didn't do so

## Components to change

This work consists of three parts: the new Projects (beta) board, a command line tool to do the initial port of the ZenHub board to the Projects (beta) board, and a service that adds issues and epics to the board as they are created and/or updated.

### The Projects (beta) board

We need to create a new board and add all of the required columns and fields so that it maintains parity with the ZenHub board. As we do this, we may decide to add new columns and/or remove columns that are rarely used or no longer fit our needs.

### Command line tool

This tool will do the following:

* For every issue in arcade, core-eng, and xliff-tasks
    * Add it to the new Projects (beta) board
* For every epic issue
    * Add a section to the issue description for a task list
    * Use the ZenHub API to get the list of issues in the epic and add it to the task list markdown if it isn't already there

If all goes well, we will only run this command line tool once to get the initial port done, and then we will be able to switch over to the new board and rely on the service and users correctly adding new issues to epics/the project using the new methodology.

### Service

The service will be a webhook that monitors new and updated issues. 

* New Issue:
    * If the issue is not already on the board, add it to the board

Note: GitHub has an issue open to automatically add issues in a repository to a project. When that feature comes out, the service can be decomissioned.

### Async Triage tool

As part of this work, we will be removing the ZenHub board, and therefore must update the async triage tool to monitor the new Projects (beta) board rather than using the ZenHub APIs.

The triage tool will need to walk through all the issues that are still open in each of our repositories, and discover the tracked in information. We can do this using graphql, though the api for it is under a feature flag. However, we should be able to easily get the information for every issue and use linq to identify the issues with no tracking information. My understanding is that when this is released, there will also be a REST API for it that we might be able to switch to.

## Serviceability

* How will the components that make up this epic be tested?
    * With unit tests
    * Potentially with the staging environment to update a staging copy of the board
* How will we have confidence in the deployments/shipping of the components of this work?
    * We will monitor the board to as issues are added to arcade to make sure that they are also added to the new board.

## Rollout and Deployment

* How will we roll this out safely into production?
    * We will have unit tests for the service
    * We may have a staging board that uses the service in staging to update it, so that we can catch when changes cause the process to fail
* How often and with what means will we deploy this?
    * This will be rolled out alongside Helix, and will follow the normal helix rollout process

## FR Handoff

* What documentation/information needs to be provided to FR so the team as a whole is successful in maintaining these changes?
    * We will provide documentation for adding and debugging GraphQL queries
    * We will provide documentation on how to run the migration tool, though it should only need to be run once
    * We will provide documentation on new procedures for creating new epics, and how the github webhook works to update the board so that new issues can be added to epics
    * We will provide documentation on how new issues are automatically added to the board, and how to debug the service if they are not being added

## Monitoring

We will use AppInsights for monitoring of the service and add grafana alerts for when there are errors adding issues to the board.

## Decomissioning the CLI tool

Once the migration is complete, we will no longer need the cli tool. After we have successfully helped the rest of the org migrate off of ZenHub, either using the CLI tool or not, we will remove the CLI tool so as to not clutter up helix-services.

<!-- Begin Generated Content: Doc Feedback -->
<sub>Was this helpful? [![Yes](https://helix.dot.net/f/ip/5?p=Documentation%5CTeamProcess%5COne-Pagers%5Czenhub-migration-core-eng-15084.md)](https://helix.dot.net/f/p/5?p=Documentation%5CTeamProcess%5COne-Pagers%5Czenhub-migration-core-eng-15084.md) [![No](https://helix.dot.net/f/in)](https://helix.dot.net/f/n/5?p=Documentation%5CTeamProcess%5COne-Pagers%5Czenhub-migration-core-eng-15084.md)</sub>
<!-- End Generated Content-->
