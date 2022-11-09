# Asynchronous Triage Bot

We want to implement a mechanism that will allow newly-created issues on GitHub that still need to be triaged to automatically have their own dedicated conversation started on Teams. It would also be useful to denote Teams conversations as [Triaged] or [Needs Triage] in Teams for clarity.

See Epic for more context: https://github.com/dotnet/core-eng/issues/13288

## Stakeholders
  - .NET Engineering Services team

## Risk

### Unknowns and Open Questions
- How can we distinguish if an issue needs triage, or if it was just created and will immediately have an Epic assigned to it? ([link to issue](https://github.com/dotnet/core-eng/issues/13457))
  - Suggested - a background job that occasionally scans for unassigned issues and adds a conversation on Teams if necessary, rather than generating a conversation every time an issue is created. This also addresses an interesting case in which an issue was assigned to one Epic, then unassigned.
- Some issues may not need to be assigned to an Epic (ex. customers that open issues for themselves).
  - Suggested - marking these issues with a label for the bot to ignore, or triage assigns them to the "work tracking for other teams" epic.

### Proof of Concepts
- Verify that we are able to open a new conversation in a channel on Teams when a new issue is opened on GitHub. Try to utilize some similar functionality to the FR Mention Bot.
- Verify that additional comments added to an issue update their dedicated Teams conversation.
- Verify that existing Teams conversations can be updated when triaged, such as denoting [Triaged] in the conversation title.
- Verify that we can grab all the existing unassigned issues from GitHub (this will require the use of the ZenHub API to check if it is in an Epic).

### Dependencies
- GitHub API
- ZenHub API
- Teams

Additionally, the solution should mostly be a new addition to the Arcade Services repo. Note that most of the GitHub webhook functionality is already implemented in Arcade Services. The goal to have the work completed by is September 10th at the latest.

## Serviceability

### Testing
To avoid sending actual requests to Teams and cluttering up the channel, we can test some functionality of the code by creating mock HTTP calls and checking if they send requests in the right cases. We can also try creating a hidden channel on Teams for integration testing.

### Security
Identifying secrets that will be used include the Teams channel connector URI and authentication for the GitHub app. Note that all PII is owned by GitHub and Teams.
- The "bot" account that will be posting to Teams will also require authentication and must have permission to access channel messages, add replies to a discussion, etc.
- Setting up repro/test/dev environments: this will vary based on implementation details; the environment may simply be able to be set up and opened as a VS project, or might require a more complicated process.

### Rollout and Deployment
- The solution will be most likely be deployed on Arcade Services, and thus on the Arcade Services cadence. Once deployed, check the Async Triage channel on Teams for updates.
- While nothing is being deprecated, we will probably remove the "Needs Triage" and "I Think This is Triaged" Fabric bot tags once the solution is deployed.
- What are the risks during deployment?
  - If the solution stops working, new untriaged issues may go unnoticed without a conversation being created on Teams. It may require some monitoring to make sure that untriaged issues are consistently being added on Teams.

## Usage Telemetry
Suggestions for tracking "usefulness" of the solution include measuring the average amount of time it takes for an issue to be triaged, how many people are involved, the average length of conversations, etc. However, more discussion is needed to figure out how the necessary information will be collected.

## Monitoring
- Is there existing monitoring that will be used by this epic?
  - On the Teams end, we can monitor if there are an abnormal amount of non-successful requests made to Teams.
  - Suggested - using Azure Application Insights to monitor the functionality of the GitHub controller.
- If new monitoring is needed, it should be defined and alerting thresholds should be set up.

## FR Hand off
- We should create some documentation on what functionalities the solution currently possesses and link to the code for more information. The code should also be clearly documented.


<!-- Begin Generated Content: Doc Feedback -->
<sub>Was this helpful? [![Yes](https://helix.dot.net/f/ip/5?p=Documentation%5CTeamProcess%5COne-Pagers%5Casync-triage-core-eng13288.md)](https://helix.dot.net/f/p/5?p=Documentation%5CTeamProcess%5COne-Pagers%5Casync-triage-core-eng13288.md) [![No](https://helix.dot.net/f/in)](https://helix.dot.net/f/n/5?p=Documentation%5CTeamProcess%5COne-Pagers%5Casync-triage-core-eng13288.md)</sub>
<!-- End Generated Content-->
