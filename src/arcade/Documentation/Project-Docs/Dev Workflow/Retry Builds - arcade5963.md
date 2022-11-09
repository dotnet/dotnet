# Automatically Retry Builds For Pull Requests

We want to implement a mechanism in which builds that fail due to a reason that is defined in a configuration by the repository owners are automatically retried during pull request builds. 

The retry event will be communicated to the developer via GitHub on the pull request that initiated the build. 

## Stakeholders
- Product teams' developers
- Product teams' management
- Engineering Services team's developers
- Engineering Services team's management

This feature is primarily for the product teams, however, the Engineering Services team should dogfood their own functionality. 

## Risks

- Increase of resource costs because of retries, however, this should not be much more than the current cost it takes to re-run a build when a failure happens. 
- Increase in time to investigate failures due to lag introduced by the retries. 
- Rule management may become cumbersome. 
- Low adoption-rate due to configuration requirements. 

### Proof of Concepts

- Through a proof of concept, verify that we are able to retry a build when a retryable scenario (customer configured) is detected. See if we can leverage similar functionality that exists in Runfo today that scans the build logs when a failure occurs, and if the reason for failure matches any of the retryable scenarios, a retry is attempted on the build. This mechanism should be built as a separate service, and connected to the build processes in Azure DevOps via a webhook. 

- Through a proof of concept, verify that we are able to incorporate a mechanism for customers to quickly and easily provide feedback via some kind of tracking image. This mechanism must be able to work with markdown, and will provide: 1) a like or dislike of the feature; and 2) capture usage tracking information. This data should be captured in Application Insights so that we can query upon the data to see how it is being used and if customers are approve or disapproving of the feature. 

### Dependencies

- Helix
- Arcade
- Azure DevOps
- GitHub, GitHub Checks API

## Serviceability

- Tests (unit, functional, E2E) for retry functionality
  - A failed build for a retryable reason should be retried. 
  - A failed build for a reason that was not configured to be retried should not be retried. 
- Tests/Validation for functionality we write to support the GitHub Check that will report on the retry status of a work item. 

### Rollout and Deployment

- If all the functionality for this exists in Arcade, then the rollout for this functionality should only be impacted by Arcade promotions. We will need to ensure that there's sufficient testing (E2E) in Arcade Validation for this feature. 
- Initial configuration and set up in the repositories will likely need assistance from the Engineering Services team to get the customers up and running. 

## Usage Telemetry

- Create a feedback mechanism (e.g. tracking image with a thumbs-up and thumbs-down) to include next to any retry communication so that the users can provide feedback quickly. 
- Links created in new areas (e.g. GitHub comments, Failure Summary in GitHub Checks) could pass-through an aka.ms redirect link for us to capture usage from. 

## FR Hand off

- We will create user documentation for how to configure the retries and expectations for viewing results. 
- We will create documentation for the Engineering Services team to investigate issues with the feature itself. 

<!-- Begin Generated Content: Doc Feedback -->
<sub>Was this helpful? [![Yes](https://helix.dot.net/f/ip/5?p=Documentation%5CProject-Docs%5CDev%20Workflow%5CRetry%20Builds%20-%20arcade5963.md)](https://helix.dot.net/f/p/5?p=Documentation%5CProject-Docs%5CDev%20Workflow%5CRetry%20Builds%20-%20arcade5963.md) [![No](https://helix.dot.net/f/in)](https://helix.dot.net/f/n/5?p=Documentation%5CProject-Docs%5CDev%20Workflow%5CRetry%20Builds%20-%20arcade5963.md)</sub>
<!-- End Generated Content-->
