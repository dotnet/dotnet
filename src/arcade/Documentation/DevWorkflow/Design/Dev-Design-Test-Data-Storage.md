# Test Result Storage
In order to facilitate several features in the reporting space, we need to store test results in a way we can query historically at fairly large scale.
The query API's in Azure DevOps work well enough for a single test or a single run, but when attempting to draw conclusions about all tests in all runs,
the API surface isn't sufficient or fast enough.

To that end, we should store tests in a large data store with broader query capabilities: Kusto

## Initial Data Colletion
We will create **a new service** in the Helix cluster that will monitor for build complete notifications, and translate the data into Kusto.
It's a new service so that
* It doesn't interfere with existing behaviors
* It can handle internal and public builds both (since test results from internal builds are important as well)

All "failed" tests for a build are easy enough to gather by calling
[Test Runs - Query](https://docs.microsoft.com/en-us/rest/api/azure/devops/test/runs/query?view=azure-devops-rest-6.0)
followed by
[Test Results - List](https://docs.microsoft.com/en-us/rest/api/azure/devops/test/results/list?view=azure-devops-rest-6.0)
to expand the lists into results with "outcome=Failed".

Unfortunately, "Passed on Rerun" tests are not queryable via any documented endpoint, but after talking with the Azure DevOps
devs, we were told about another endpoint,
`https://dev.azure.com/{organization}/{project}/_apis/test/ResultDetailsByBuild?buildId={buildId}&$filter=Outcome%20eq%20PassedOnRerun&api-version=6.1-preview`
that will list the testCaseResultIds of each test in this state.  We can follow that up with a call to
[Test Result - Get](https://docs.microsoft.com/en-us/rest/api/azure/devops/test/results/get?view=azure-devops-rest-6.0)
for each test in this state to get the details.  It is expected that for a given run, a small number of tests will be in this state,
so the number of calls necessary will be low.

All of this data can then be passed into Kusto, using our existing "TestResults" table, with a few minor tweaks
* Not all test results will necessarily correspond to a helix job/workitem (in build tests), so these columns will begin to have null
* We will add a "FailCount" and "RunCount" to each test, to account for flakiness.

## Historical Data Collection
On some timed cadence, we will create a list of tests that have failed in some time horizon, probably 28 days to start.  Then, using the
[Test History - Query](https://docs.microsoft.com/en-us/rest/api/azure/devops/test/test-history/test-history-query?view=azure-devops-rest-6.0)
api, we will gather statistics for that test since the last execution (probably nightly, so in the last day).

This data will be aggregated and put into Kusto as a raw count (100 executions, 4 failures, 5 pass in the previous 24 hours).
The same columns as in the TestResults table that are applicable for aggergations will be used (e.g. yes "Type" and "Method", but no "StackTrace")
Storing _every_ passing result is unecessary, so daily aggregations will be sufficient.
This, combined with the TestResultTable, will allow for deep querying about the reliability numbers for any specific test or group of tests.

## TestResults and TestAggregations table schema
Name | Type | In TestResults | In Aggergations
-- | - | - | -
JobId | I64 | X | 
WorkItemId | I64 | X | 
JobName | StringBuffer | X |
WorkItemName | StringBuffer | X |
WorkItemFriendlyName | StringBuffer | X |
Type | StringBuffer | X | X 
Method | StringBuffer | X | X 
ArgumentHash | StringBuffer | X | X 
Arguments | StringBuffer | X | X 
Result | StringBuffer | X | X 
Count | I32 | | X
Duration | R64 | X | 
Exception | StringBuffer | X |
Message | StringBuffer | X |
StackTrace | StringBuffer | X |
Traits | StringBuffer | X |
Reason | StringBuffer | X |
Attempt | StringBuffer | X |
AzurePipelinesTestRunId | I64 | | X |
AzurePipelinesTestResultId | I64 | | X |
ExecutionCount | I32 | X |
FailCount | I32 | X | 
Branch | StringBuffer | X | X
BuildId | I32 | X | X
DefinitionId | I32 | X | X
DefinitionName | StringBuffer | X | X
AggregationDate | DateTimeOffset | | X


<!-- Begin Generated Content: Doc Feedback -->
<sub>Was this helpful? [![Yes](https://helix.dot.net/f/ip/5?p=Documentation%5CDevWorkflow%5CDesign%5CDev-Design-Test-Data-Storage.md)](https://helix.dot.net/f/p/5?p=Documentation%5CDevWorkflow%5CDesign%5CDev-Design-Test-Data-Storage.md) [![No](https://helix.dot.net/f/in)](https://helix.dot.net/f/n/5?p=Documentation%5CDevWorkflow%5CDesign%5CDev-Design-Test-Data-Storage.md)</sub>
<!-- End Generated Content-->
