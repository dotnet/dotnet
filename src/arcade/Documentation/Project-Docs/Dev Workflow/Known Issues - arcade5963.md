# Known Issues and Outage Reporting for Pull Requests

We want to implement a notification that will be communicated to the developer on the pull request that initiated the build that notes any known outages that may cause or is causing failures. This will be similar to the Failure Guessing feature. 

Some of this data may already be available via [runfo](https://github.com/jaredpar/runfo/tree/master/runfo), so we should try to leverage the functionality that it has there, if it's useful for this feature. (e.g. recreating the functionality in our own code)

Also, if possible, we'd like to automatically retry builds that have failed due to outages when the outage has been resolved. 

## Stakeholders
- Product teams' developers
- Product teams' management
- Engineering Services team's developers
- Engineering Services team's management

This feature is primarily for the product teams, however, the Engineering Services team should dogfood their own functionality. 

## Risks

- Data could be misleading and provide incorrect assumptions on cause of failures. 
- Information relayed to the user may not contain actionable information (e.g. should they wait for other issues to be resolved? Do they need to fix a test? Is this something they need to report to Engineering Services team? et cetera)
- Information relayed to the user may be noisy. 
- It's possible that the amount of work required for this future will overlap greatly with the greater Outage epic that is on the backlog. 
- A manual process for tracking outages may mean issues that are tracking outages may be missed when the outage is resolved, or resolved prematurely. 

### Proof of Concepts

- Through a proof of concept, verify that we are able to retry a build that had previously failed due to an outage. This will require us to track the builds that are affected by outages and then know when an outage has been resolved so that it can retry the failed builds. 

  Additional things to consider: 
  - Is a retry for the build already in progress? 
  - Are there other things in the build that caused a failure that does not warrant a retry? 
  - Should only builds of a certain "age" be retried? (e.g. what if a build is a week old before a reported outage is marked as resolved?)
  - Can a customer opt out of an automatic retry? (Would this make sense to have?)

- Through a proof of concept, verify that we have a way of tracking outages that can be posted back to the pull request in GitHub via the GitHub Checks API to report this information back to the user. This may be a specific way of naming GitHub issues to track outages, or a service that will track the outages. 

### Dependencies

- Helix Services
- Azure DevOps
- GitHub, GitHub Checks API

## Serviceability

- Unit and functional (where appropriate, for both) tests for the individual components of the service
- Post-deployment scenario tests for the service to ensure that it is functioning as intended with all it's integrated parts. 

### Rollout and Deployment

- If the service is built in an existing project, such as Arcade Services or Helix Services, the rollout should be consistent with the existing rollouts for the project. 

## Usage Telemetry

- Create a feedback mechanism (e.g. tracking image with a thumbs-up and thumbs-down) to include next to any communication so that the users can provide feedback quickly. 
- Links created in new areas (e.g. GitHub comments, Failure Summary in GitHub Checks) could pass-through an aka.ms redirect link for us to capture usage from. 

## Monitoring

- This new service will require usage and health monitoring. 

## FR Hand off

- We will create user documentation for how to configure the retries and expectations for viewing results. 
- We will create documentation for the Engineering Services team to investigate issues with the feature itself. 

<!-- Begin Generated Content: Doc Feedback -->
<sub>Was this helpful? [![Yes](https://helix.dot.net/f/ip/5?p=Documentation%5CProject-Docs%5CDev%20Workflow%5CKnown%20Issues%20-%20arcade5963.md)](https://helix.dot.net/f/p/5?p=Documentation%5CProject-Docs%5CDev%20Workflow%5CKnown%20Issues%20-%20arcade5963.md) [![No](https://helix.dot.net/f/in)](https://helix.dot.net/f/n/5?p=Documentation%5CProject-Docs%5CDev%20Workflow%5CKnown%20Issues%20-%20arcade5963.md)</sub>
<!-- End Generated Content-->
