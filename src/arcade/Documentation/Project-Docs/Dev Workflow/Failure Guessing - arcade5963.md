# Failure Guessing for Pull Requests

We want to implement a mechanism that will trigger when a build fails and provides information (much like the [runfo](https://github.com/jaredpar/runfo/tree/master/runfo) tool operates) to the developer on the pull request that initiated the build, via a GitHub Check (if possible).

The information relayed back to the developer should note the following: 
- link to an open, public GitHub issue if the failure appears to be known
- that the failure encountered was novel

Examples of failure notes: 
- `This test is also failing in the main branch` (link to open issue, if available)
- `This is a known Mac disconnect issue` (link to open issue, if available)

This functionality may provide comparable data that is already provided by the runfo tool. In other words, if there is functionality that already exists in runfo that does what we need to do, we should look at implementing similar functionality in our own project. 

## Stakeholders
- Product teams' developers
- Product teams' management
- Engineering Services team's developers
- Engineering Services team's management

This feature is primarily for the product teams, however, the Engineering Services team should dogfood their own functionality. 

## Risks

- Data could be misleading and provide incorrect assumptions on cause of failures. (Because this functionality is new, we may not be able to create algorithms strong enough to sufficiently raise the signal-to-noise ratio experienced by devs.)
- Information relayed to the user may not contain actionable information (e.g. should they wait for other issues to be resolved? Do they need to fix a test? Is this something they need to report to Engineering Services team? et cetera)
- Information relayed to the user may be noisy. 
- Ensure we are not hiding data that used to be visible

### Proof of Concepts

- Through a proof of concept, verify that we are able to build a service that will be able to accurately analyze the failure and provide relevant data (e.g. links to open GitHub issues regarding the failure) to the user regarding the failure. 

  We will build a service that can take in an Azure DevOps build ID. This service will contain functionality that will detect failure patterns. Likely, this information will need to be provided by humans, so we will also need the ability to take in and store this information, such as in a JSON file. This should connect to a GitHub Checks API in order to report the failure analysis back to the user in the pull request. 

  Additionally, we may want to build our POC as a command line tool initially, to ensure that the functionality works as expected before we integrate it with the infrastructure. 

- Through a proof of concept, verify that we are able to provide a mechanism for the repo owner to annotate their error messages so they can ignore non-useful error messages. 

### Dependencies

- Helix Services
- Azure DevOps
- GitHub, GitHub Checks API
- Helix

## Serviceability

- Unit and functional (where appropriate, for both) tests for the individual components of the service
- Post-deployment scenario tests for the service to ensure that it is functioning as intended with all it's integrated parts. 
- Algorithms developed for this feature should all be unit tested in order to detect errors if the algorithm changes.
- Ensure additional resources needed for this feature, such as additional data stores are well-documented for serviceability. 

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
<sub>Was this helpful? [![Yes](https://helix.dot.net/f/ip/5?p=Documentation%5CProject-Docs%5CDev%20Workflow%5CFailure%20Guessing%20-%20arcade5963.md)](https://helix.dot.net/f/p/5?p=Documentation%5CProject-Docs%5CDev%20Workflow%5CFailure%20Guessing%20-%20arcade5963.md) [![No](https://helix.dot.net/f/in)](https://helix.dot.net/f/n/5?p=Documentation%5CProject-Docs%5CDev%20Workflow%5CFailure%20Guessing%20-%20arcade5963.md)</sub>
<!-- End Generated Content-->
