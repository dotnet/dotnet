# Overview
The requirement is to have an ability to retry Helix Test Failures automatically instead of a developer hunting the offensive test and having to manually queue a rerun or spam the PR's with "[at] dotnet-bot test this please".
In order to promote always green before merging, a mechanism to manage inherently flaky tests in a highly visible fashion is needed. Some tests (like networking) perhaps are flaky by design, but others require additional dev attention. Regardless, the infrastructure should provide the ability to not fail the entire run - but rather provide visibility to the fact that certain tests required multiple tries to pass.

The following are the Steps to achieve this at a high-level: 
1. Ability to configure at a job level:
   - If a work item can be retried on failure (defaults to false)
   - Max retries allowed for a workitem (defaults to 0)
1. Helix Client detects a failed workitem and requeues the work item at the end of the queue. A work item is not a test.
1. Helix API will retry the work item until it passes or max retry count is hit.
1. Test results for each rerun is stored in DB for further retrieval. 
1. On success or when the max retries allowed count is reached the work item is marked as WorkItemFinished.
1. MC displays the retried workitem with a specific color/format (TBD). The Tests that passed on retried displayed with a specific icon (TBD)
1. MC displays logs for each retry.
1. MC displays number of retries

# Out of scope
1. There will be no attempt to retry a work item on timeouts.

Detailed requirements list below.

# Helix Client

1. Support a separate parameter / reuse delivery count in QueueInfo to identify if a particular work item is a retry work item.
1. Requeue a work item if one or many tests fail in the work item.
1. Identify if Min(Max Delivery Count for Requested Queue - 1, Requested Value for retries)  has crossed.
1. Retry work item requeued with a special event other than work item started.
1. WorkItemFinished should not be sent when requeuing a workitem until the workitem succeeds or max retires for the workitem has reached.
 
# Helix Controller

In order to support automated configurable retry, we'll need a few changes in the controller:

1. In Job Started V2+ messages, have a new optional property, call it MaxRetryCount or MaxDeliveryCount. (the difference between the two is 1)
1. When the property is not supplied, set it to 0 (for MaxRetry option) or 1 (for MaxDelivery)
1. When the property is supplied, set it to Min(Max Delivery Count for Requested Queue - 1, Requested Value)
1. Attach the property of your choice to the work items being sent to the clients.

### Nice to Have
1. Optional: Send App Insights Trace every time you have to pick a value < than requested (helps investigations for folks curious about delivery counts.)
 
# Helix API
 
 1. Helix API needs to be aware of partial failures.
 1. Helix API needs to be able to process multiple test results for a work item ie change the 1 to 1 mapping from name to result blob to potentially a list.
 1. Helix API notifies WorkItemFinished to CI only after the retries are finished. No change to CI is needed. 
 
# DB
 
1. Currently we check to pick the latest set of results for a workitem, this check needs to be removed.
 
# Mission Control UI
 
1. Add a new icon to show that the test passed on a retry / test failed despite reruns. TBD what icon needs to be used. 
1. Show multiple entries of logs for retried workitems. 
1. "Show Failures Only" checkbox should display the retried tests as well.
1. Show number of retries, we are already displaying number of failures and skips
 
### Nice to Have 
1. Separate Checkbox - Show retried workitems (TBD the name) should display only the retried work items
 
 
 


<!-- Begin Generated Content: Doc Feedback -->
<sub>Was this helpful? [![Yes](https://helix.dot.net/f/ip/5?p=Documentation%5CProject-Docs%5CAuto-Retry%20Failures%5COverview-Requirements.md)](https://helix.dot.net/f/p/5?p=Documentation%5CProject-Docs%5CAuto-Retry%20Failures%5COverview-Requirements.md) [![No](https://helix.dot.net/f/in)](https://helix.dot.net/f/n/5?p=Documentation%5CProject-Docs%5CAuto-Retry%20Failures%5COverview-Requirements.md)</sub>
<!-- End Generated Content-->
