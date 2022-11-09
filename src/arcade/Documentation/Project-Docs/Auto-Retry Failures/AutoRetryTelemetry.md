# Overview
This document provides Kusto Queries to pull telemetry data from Kusto DB for Auto-Retry of Test Failures.

## WorkItems that belong to Jobs that have total pass and has items / tests that passed on retry:

```
let jobs = Jobs
| where Source == [source_name]
| where TestsFail == 0 and ItemsPassedOnRetry > 0 and TestsPassedOnRetry > 0 
| project JobId;
WorkItems
| where Status == 'PassOnRetry'        
| where JobId in (jobs)
| summarize count(WorkItemId) by FriendlyName
| project FriendlyName, count_WorkItemId 
```
## Tests Passed on Retry summarized by type and date:

```
Jobs
| where Source == [source_name]
| summarize count(TestsPassedOnRetry) by Type, format_datetime(Finished,"yyyy-MM-dd")
```
## Tests Passed on Retry summarized by type and Month:

```
Jobs
| where Source == [source_name]
| summarize count(TestsPassedOnRetry) by Type, format_datetime(Finished,"yyyy-MM")
```
## Test Results for a specific WorkItemFriendlyName, Type, Method, Arguments, if the test had failed and passed on retry. 
Test Results table has huge amount of data so getting pass on retry data for entire set of tests/workitems will break the DB’s back, hence the need to filter by a specific test. 

```
TestResults
| where WorkItemFriendlyName == [WorkItemFriendlyName]
    and Type == [Type]
    and Method == [Method]
    and Arguments == [Arguments]
    and Result != 'Pass'
| summarize arg_max(toint(Attempt), *) by WorkItemId, Type, Method, Arguments, ArgumentHash
| join kind = leftouter(
          TestResults
          | where WorkItemFriendlyName == [WorkItemFriendlyName]
   	    and Type == [Type]
    	    and Method == [Method]
            and Arguments == [Arguments]
            and Result == 'Pass'
          ) on WorkItemId, Type, Method, Arguments, ArgumentHash
| extend PassOnRetry = isnotnull(WorkItemId1)
| summarize count() by Type, Method, Arguments, Result = iif(PassOnRetry, 'PassOnRetry', Result)   
```
### Sample Value for Parameters:

[source_name] : 'official/corefx/master/'

[WorkItemFriendlyName] : 'System.Net.Http.WinHttpHandler.Functional.Tests'

[Type] : 'System.Net.Http.WinHttpHandlerFunctional.Tests.WinHttpHandlerTest'

[Method] : 'SendAsync_SlowServerAndCancel_ThrowsTaskCanceledException'

[Arguments] : ''






<!-- Begin Generated Content: Doc Feedback -->
<sub>Was this helpful? [![Yes](https://helix.dot.net/f/ip/5?p=Documentation%5CProject-Docs%5CAuto-Retry%20Failures%5CAutoRetryTelemetry.md)](https://helix.dot.net/f/p/5?p=Documentation%5CProject-Docs%5CAuto-Retry%20Failures%5CAutoRetryTelemetry.md) [![No](https://helix.dot.net/f/in)](https://helix.dot.net/f/n/5?p=Documentation%5CProject-Docs%5CAuto-Retry%20Failures%5CAutoRetryTelemetry.md)</sub>
<!-- End Generated Content-->
