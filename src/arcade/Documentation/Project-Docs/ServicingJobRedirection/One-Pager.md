# Execute servicing Helix jobs in a COGS Azure subscription
## Stakeholders
The main stakeholder of the project is Chris Bohm as the .NET Azure Champion

## Risk
More risk comes from the lack of experience the team working on Helix API projects, there will be a ramp-up on how development and debugging works in these projects.

No proof of concept will be needed as we already redirect jobs based on the repository that sends them. We will expand this adding to build and now redirect jobs based on the target branch.

We have all the dependencies in place to start working on redirecting work.

This will not require any change from our customers, servicing jobs will be redirected once a servicing branch exists for them.

Completing this work will open the possibility to increase the load in our vNext Helix queues if needed so the longer it takes to complete the project the higher the risk of not having enough R&D budget for future workload.

## Serviceability
Unit tests will be added to ensure servicing builds and tests jobs are identified by our services and then redirected appropriately.
Daily validation will be added to our staging environment where the code paths added are executed and alerts will be triggered if unexpected behavior is identified.

No new secrets will be added as part of this effort

This project has no SDL implications.

This epic will not modify the current steps for setting up repro/test/dev environments for Helix API. Any missing information in the current documentation will be added
## Rollout and Deployment
The epic doesn't consider any breaking changes on how helix services are deployed to production but feature flags will be added during the development to reduce the need of rollback in case of unexpected results in production.
The epic isn't deprecating any service.

The impacted services will follow the current deployment schedule which considers one deployment to production every Wednesday.

New queues will need to be created when the servicing OS matrix changes after a release. 
Changes for redirecting work will be added to Helix API during the development of the project but won't be required once the epic is completed.

The risk of running production deployments for these services is low as they are mature services and have been executing successful builds for a long time. In case of having bugs in the payload that prevents the work from being redirected, the customers most likely won't be impacted as all the work will executed in the queue originally used in the job.

## Usage Telemetry
We will add a new property to servicing jobs to mark them so they can query in Kusto for monitoring and alerting.

## Monitoring
S360 report will show us how our R&D bill goes down while the COGS bill increases.
New Grafana charts will be created to show the subscription where the servicing runs are being executed and an alert, that gets triggered when a servicing job is executed in a R&D queue, will be added.


<!-- Begin Generated Content: Doc Feedback -->
<sub>Was this helpful? [![Yes](https://helix.dot.net/f/ip/5?p=Documentation%5CProject-Docs%5CServicingJobRedirection%5COne-Pager.md)](https://helix.dot.net/f/p/5?p=Documentation%5CProject-Docs%5CServicingJobRedirection%5COne-Pager.md) [![No](https://helix.dot.net/f/in)](https://helix.dot.net/f/n/5?p=Documentation%5CProject-Docs%5CServicingJobRedirection%5COne-Pager.md)</sub>
<!-- End Generated Content-->
