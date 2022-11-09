# Automatically Retry Tests For Pull Requests

We want to implement a mechanism in which work items that fail due to a reason that is defined in a configuration by the repository owners are automatically retried during pull request builds and tests.

The work item can either be retried in the current environment, or, optionally, pushed to another machine to be retried. 

The retry will be communicated to the developer via GitHub on the pull request that initiated the test. We will ensure that failed attempts are not hidden from the user, and also communicated, as well. Full details of any failure shall be recorded. 

It is important to note that we can only retry based on a Helix Work Item since we are constrained by Azure DevOps and cannot retry on a per test basis. 

## Stakeholders
- Product teams' developers
- Product teams' management
- Engineering Services team's developers
- Engineering Services team's management

This feature is primarily for the product teams, however, the Engineering Services team should dogfood their own functionality. 

## Risks

- High implementation cost, however, the increase of green PRs due to automatically retrying failed tests would be a huge win. 
- Increase of resource costs because of retries, however, retries per work item is a lower cost than a retry of an entire build and test suite that users do today. 
- Increase in time to investigate failures due to lag introduced by the retries. 
- Rule management may become cumbersome. 
- Low adoption-rate due to configuration requirements. 

### Proof of Concepts

- Through a proof of concept, verify that we are able to implement a retry mechanism in Arcade by enhancing the [Helix SDK scripts](https://github.com/dotnet/arcade/tree/main/src/Microsoft.DotNet.Helix/Sdk/tools/azure-pipelines/reporter) written in Python that will allow work items that fail and meet a certain criteria (defined in JSON) to be retried. 
  
  Implementation details for this proof of concept are as follows: 
    - Customer configures their desired retry rules in a pre-defined json file that will be provided in the /eng/ folder within Arcade. 
    - When the build runs during the PR, if a work item fails that matches the rules defined in the json file, we will automatically retry that work item. 
    - When the retry is initiated, a comment will be posted on the pull request that initiated the build/test suite remarking the failure and subsequent retry event. This information should also be communicated to the Failure Summary page through GitHub Checks. 
    - The test will be retried the specified number of times as noted in the configuration, depending on subsequent failures. 

- Through a proof of concept, verify that we are able to incorporate a mechanism for customers to quickly and easily provide feedback via some kind of tracking image. This mechanism must be able to work with markdown, and will provide: 1) a like or dislike of the feature; and 2) capture usage tracking information. This data should be captured in Application Insights so that we can query upon the data to see how it is being used and if customers are approve or disapproving of the feature. 

### Dependencies

- Helix
- Arcade
- Azure DevOps
- GitHub, GitHub Checks API

## Serviceability

- Tests (unit, functional, E2E) for Python components in Arcade that will interact with Helix. (Provided the aforementioned proof of concept is acceptable, or for whatever mechanism is eventually written to support this functionality)
  - Tests for retry functionality on same machine
  - Tests for retry functionality sent to another machine
- Tests/Validation for functionality we write to support the GitHub Check that will report on the retry status of a work item. 
- SDL considerations will need to be made for sentiment tracking.

### Rollout and Deployment

- If all the functionality for this exists in Arcade, then the rollout for this functionality should only be impacted by Arcade promotions. We will need to ensure that there's sufficient testing (E2E) in Arcade Validation for this feature. 
- Initial configuration and set up in the repositories will likely need assistance from the Engineering Services team to get the customers up and running. 

## Usage Telemetry

- Create a feedback mechanism (e.g. tracking image with a thumbs-up and thumbs-down) to include next to any retry communication so that the users can provide feedback quickly. 
- Links created in new areas (e.g. GitHub comments, Failure Summary in GitHub Checks) could pass-through an aka.ms redirect link for us to capture usage from. 
- Usage telemetry: 
  - Did the retry result in a passing test?
  - What triggered the retry?
  - Which work item did it trigger on?
  - How many retries were triggered for this work item?

## Monitoring

- The sentiment feature will need monitoring (e.g. sudden and/or excessive negative feedback may indicate something is wrong with a feature and is getting negative feedback from customers).
- The retry functionality will need monitoring.
  - Does this increase CPU or resource usage? 
  - Get a baseline of resource usage today before feature is implemented. 
  - Other cost-related monitoring
  - Detect if the functionality is down/broken (e.g. a retry should've been triggered, but it was not)

## FR Hand off

- We will create user documentation for how to configure the retries and expectations for viewing results. 
- We will create documentation for the Engineering Services team to investigate issues with the feature itself. 

<!-- Begin Generated Content: Doc Feedback -->
<sub>Was this helpful? [![Yes](https://helix.dot.net/f/ip/5?p=Documentation%5CProject-Docs%5CDev%20Workflow%5CRetry%20Tests%20-%20arcade5963.md)](https://helix.dot.net/f/p/5?p=Documentation%5CProject-Docs%5CDev%20Workflow%5CRetry%20Tests%20-%20arcade5963.md) [![No](https://helix.dot.net/f/in)](https://helix.dot.net/f/n/5?p=Documentation%5CProject-Docs%5CDev%20Workflow%5CRetry%20Tests%20-%20arcade5963.md)</sub>
<!-- End Generated Content-->
