# Test Reporting Queries

The following is a list of default queries to use to look up information about failed tests. Feel free to change them for your own usages. 

Click [here](TestReportingData.md) to learn more about the data we're collecting. 

Caveats (updated July 14, 2022): 
- Because this data is stored in an internal data source, it is unfortunately currently only available to Microsoft employees. (If you are an internal Microsoft employee, you can request access from the [.NET Engineering Services team](https://dev.azure.com/dnceng/internal/_wiki/wikis/DNCEng%20Services%20Wiki/107/How-to-get-a-hold-of-Engineering-Servicing).)

## Index
  - [Tests That Have Changed Failure Rate in the Last Week](#tests-that-have-changed-failure-rate-in-the-last-week)
  - [Tests That Have Failed X% of the Time in the Recent Timespan](#tests-that-have-failed-x-of-the-time-in-the-recent-timespan)
  - [Mean Value for the Expected Pass Rate for Tests](#mean-value-for-the-expected-pass-rate-for-tests)
  - [Mean Value for the Expected Pass on Retry Rate for Tests](#mean-value-for-the-expected-pass-on-retry-rate-for-tests)
  - [Search Failed Test Results as with Runfo](#search-failed-test-results-as-with-runfo)
  - [Search Timeline as with Runfo](#search-timeline-as-with-runfo)
  - [Build Analysis Reporting](#build-analysis-reporting)
  - [Sentiment Tracker Feedback](#sentiment-tracker-feedback)

## Tests That Have Changed Failure Rate in the Last Week

<details>
  <summary>Expand for query</summary>

Variables: 
- `targetSignificance`: Target statistical likelihood that the failure change is due to a change in the last week. (The closer to 1 this value is, the more likely the test changed.)
- `repo`: Repository to filter on. Set to empty string to inclue all repositories. Default is `dotnet/runtime`.
- `minimumHistoricalData`: Minimum number of historical data points (e.g. how many times the test has run) to include to avoid new tests, (`0` includes all tests)
```
let targetSignificance = 0.95;
let repo = "dotnet/runtime";
let minimumHistoricalData = 0;
let dt = toscalar(AzureDevOpsTestAnalysis | summarize max(ReportDate));
AzureDevOpsTestAnalysis
| where ReportDate == dt and Repository == repo
| where Significance >= targetSignificance and CurrentFailCount != 0
| extend HistoricalTotal = HistoricalFailCount + HistoricalPassCount + HistoricalPassOnRerunCount
| where HistoricalTotal >= minimumHistoricalData
| order by Significance desc
```
</details>

:part_alternation_mark: [Link](https://dataexplorer.azure.com/clusters/engsrvprod/databases/engineeringdata?query=H4sIAAAAAAAAA3WQy04CYQyF9/MUlRXERN24MGRMCMS4wyAvUGcqNPkvpO0oQ3h4O5AwYsZdc05P26+BDAxlQ/bOm8SfXGGqCEp4uHt6nBbBbaFddmFUZ0tk99Ik40ijsxk5cWziK6tl8XBYoGEXP9u1eW1Z3UAZzw6N0IK+ljtdk9osYWiVFY6gTYwofCCIuB+vfKOYT6LJZFr8kyqO8L0lIei7oSy7jZjqk6jsN7Wd2CFc+q9An8sh/G7CvBGhZC/IYZ6dGW6cyofQ3sjtnnidDYNj9kqfuf2lvqHqsLpMK/K3nszLmX8X+KWDz/ZAlpoEPtprtJq0+gHUohbl3wEAAA==) to query editor

## Tests That Have Failed X% of the Time in the Recent Timespan

This query will return a list of tests that have failed a certain percentage in the recent provided timespan. The default example in this query provides a list of tests in the dotnet/runtime repo that have failed 10% of the time in the last 7 days. 

<details>
  <summary>Expand for query</summary>

Variables: 
- `ts`: [Kusto timespan format](https://docs.microsoft.com/en-us/azure/data-explorer/kusto/query/scalar-data-types/timespan). Default is `7d`.
- `repo`: Repository to filter on. Set to empty string to inclue all repositories. Default is `dotnet/runtime`.
- `failureThreshold`: Double value denoting failure rate percentage. Default is `0.1` or 10%. 
- `excludeAlwaysFailing`: Set to true to filter out tests that are always failing to get a list of tests that are "flakey". Default is `true`.

```
let ts = 7d;                      // Timespan value
let repo = "dotnet/runtime";      // Optional: set to empty string if you want results from all repositories
let failureThreshold = 0.1;       // Double value denoting the lowest test fail rate to return
let excludeAlwaysFailing = true;  // Boolean. Set to true to exclude test results that are always failing
let subtable = AzureDevOpsTestsSummary
| where ReportDate >= ago(ts) and iff(repo == "", Repository == Repository, Repository == repo)
| summarize numerator=sum(FailCount), denom=sum(PassCount) + sum(FailCount) + sum(PassOnRetryCount), asOfDate=max(ReportDate) by BuildDefinitionName, TestName, ArgumentHash;
let argumentHashMap = AzureDevOpsTestsSummary
| where ReportDate >= ago(ts)
| summarize by ArgumentHash, Arguments;
subtable
| where denom > 0 and (todouble(numerator)/todouble(denom)) >= failureThreshold and iff(excludeAlwaysFailing, (todouble(numerator)/todouble(denom)) < 1, true)
| lookup (argumentHashMap) on ArgumentHash
| project BuildDefinitionName, TestName, Arguments, FailRate=(todouble(numerator) / todouble(denom)), FailCount=numerator, TotalRunCount=denom;
```
</details>

:part_alternation_mark: [Link](https://dataexplorer.azure.com/clusters/engsrvprod/databases/engineeringdata?query=H4sIAAAAAAAAA51SzW7bMAy+F+g7EDnZmNG0pwFzXSBdMOyyZUjzAkpNx25lyZCopi728COlJWmyHIrlECQUvx9+pEYC8lDB57qEs5/pFFZdj35QBl6UDnh5oRnkcLAMm9SWDNLUBUPcNSn3oMVAnTVKfwEvGhawH2gET64zG+gaGG2ArTJC5YNmE42zPSitI7fvyLoOfVJrVKeDw1XLva3VNStfX92UB4tzG9Yak0Go0VgSFWoRtN2iZwPyJTTgFKH4cUjBmcSPr4861DjTWzX6b9wl6ArIBSwj/721GpW5goc0jLzEoRIw0e8GoVYRKIc8jPBFWSZMUj6sSYnXCmZvPNMcXxaDXzHeP4S+V268vPgN2xYZv+QgHM3F8F0FamMz8jkoU3N+TZZWwDuYFLEzRjZK5fDv9EUwuQj4qNW9IZjQI2diXcW1TIb/anmbeRFz7GP1l/I+VeETHLf9LUjHwiyR3LiDK79oxHvVq9fsMEoO6xHuQ6frOTad6eRMfqoeC5AQ0q+Z27ArQ9+Vb8uUm3pX+qGG/47veHi28l7roOxZdreqA2NMBO7gOi4hI1vHu8v2GebTfS325rlI/3O+uxWeu7vig7y3cFPEO4wjaWufwwDZSUw5WHM0ofQOzj7hI310Cb4AsbaUVZ6zBlM4NZcQ8RCqfSNzW1J6GUx6iL3lH0YDDByCBAAA) to query editor

## Mean Value for the Expected Pass Rate for Tests

This query will return a list of tests and the mean value for the expected pass rate based on historical data. (Comparably, an inverse to [Tests That Have Failed X% of the Time in the Recent Timespan](#tests-that-have-failed-x-of-the-time-in-the-recent-timespan))

Calculate the expected value (E[_Y_]) for how often a test is likely to pass on its initial run (retries not included) based on its historical data (e.g. monthly, weekly, daily aggregates). Ex: A test is known to fail once out of every seven runs. Its expected value (E[_Y_]) is determined to be 85.7%, meaning, we expect this test to succeed 85.7% of the time.

<details>
  <summary>Expand for query</summary>

Variables: 
- `ts`: [Kusto timespan format](https://docs.microsoft.com/en-us/azure/data-explorer/kusto/query/scalar-data-types/timespan). Default is `7d`.
- `repo`: Repository to filter on. Set to empty string to inclue all repositories. Default is `dotnet/runtime`.
- `excludeAlwaysPassing`: Set to true to filter out tests that are always passing. Default is `true`.

```
let ts = 7d;                      // Timespan value
let repo = "dotnet/runtime";      // Optional: set to empty string if you want results from all repositories
let excludeAlwaysPassing = true;  // Boolean. Set to true to exclude test results that are always passing
let subtable = AzureDevOpsTestsSummary
| where ReportDate >= ago(ts) and iff(repo == "", Repository == Repository, Repository == repo)
| summarize numerator=sum(PassCount), denom=sum(PassCount+FailCount+PassOnRetryCount), asOfDate=max(ReportDate) by BuildDefinitionName, TestName, ArgumentHash;
let argumentHashMap = AzureDevOpsTestsSummary
| where ReportDate >= ago(ts)
| summarize by ArgumentHash, Arguments;
subtable
| where denom > 0 and iff(excludeAlwaysPassing, (todouble(numerator)/todouble(denom)) < 1, true)
| lookup (argumentHashMap) on ArgumentHash
| project BuildDefinitionName, TestName, Arguments, MeanPassRate=(todouble(numerator) / todouble(denom)), PassCount=numerator, TotalRunCount=denom
| order by MeanPassRate;
```
</details>

:part_alternation_mark: [Link](https://dataexplorer.azure.com/clusters/engsrvprod/databases/engineeringdata?query=H4sIAAAAAAAAA51U3WvbMBB/L/R/OAoDm5lkexgdzVJIP8b20GWkZTDGHi7xOdEiS0Y6NXXpH7+T3DhpycNYXqxI9/u4n84eDuH6oaEFUwk/UAeCyjr4jt7DDJnO4BL1ImhZAq8IaFt7n2qz618/f+cJsrIbsBWTAQQmz6A8aLUm3QJbaCKhNaDYgzKKFWpwwUDmiJ0iD8YKwix0KKnM4fhoOIQ5ehF6Bq2UZ+vUQnAlMkJGg+UAamt4pdsCNkTr+CxRiSAul46W4tnnA2nvDCa9pbWxGxMdVVIp5AsCG1icA92Ta8HLw0RrfgBfRTc5Ody1sJXE5Gpl5Ego5wQfPwxO3xRQExplltHXM1jSk/rkQip9WCxIQKk8isdsWdU0EEFNUuNhDKflCA7+xNKdFPsGTWepAzlqrMBOSsuGeChNRMqTUQ+aNqysQX0mbSYfVDcsPcsVmCWoClobYIMmUvmgxUTlbA2odeL2Kl4B+U6NHtJtTfQGWx8HJnKMgV2gUVK7sFZLDgO47cTiSRLtgF0YWyFeIQM6ErHIlwZGCDspH+aMc01CP3kMjq7oftr4O8H721DX6NrjoyfYrEjwMzHq+CoO7PlYJsFm7HNAU0p/VdZFJBmdFKkytdTGnd2/1ycRk0cBn7TUI4EJNTmUgrHsZbH5Sytp5zKAZGz9cvftZxm1bhX3pmYmM99uAeinVXQ7rvEh25nPYd7CRVC6vKIqvTHWfMOaCohtd6uJW4oPw1/Qr0ZdUri3dYPNfwf2sl2xsq+1U/Yiu72cHWPKAM7hXR/7oVkpIGNb2iDQrM8zH/Z7iSXP4RO8L9LsJFPa2nVoIHvVaB6/E/seY23j7J/46v1jjL6AG5nXaC9++saH7MEQXhssoL/pcV8o/JZRz4LpDlJt9GRdSS4Gui81+gtMm5LlhgUAAA==) to query editor

## Mean Value for the Expected Pass on Retry Rate for Tests

Retries are meant to unblock and prevent a build from failing due to failing test, but they are still indicative of unwanted behavior, therefore, we need to track how often a test passes when retries are introduced. Ex: A test has a 100% pass rate, but only when the test re-runs after a failure every six runs, so it’s expected value for re-runs is 83.3%.

<details>
  <summary>Expand for query</summary>

Variables: 
- `ts`: [Kusto timespan format](https://docs.microsoft.com/en-us/azure/data-explorer/kusto/query/scalar-data-types/timespan). Default is `14d`.
- `repo`: Repository to filter on. Set to empty string to inclue all repositories. Default is `dotnet/arcade`.

```
let ts = 14d;                      // Timespan value
let repo = "dotnet/arcade";        // Optional: set to empty string if you want results from all repositories
let subtable = AzureDevOpsTestsSummary
| where ReportDate >= ago(ts) and iff(repo == "", Repository == Repository, Repository == repo) and PassOnRetryCount > 0
| summarize numerator=sum(PassOnRetryCount), denom=sum(FailCount+PassOnRetryCount), asOfDate=max(ReportDate) by BuildDefinitionName, TestName, ArgumentHash;
let argumentHashMap = AzureDevOpsTestsSummary
| where ReportDate >= ago(ts)
| summarize by ArgumentHash, Arguments;
subtable
| where denom > 0
| lookup (argumentHashMap) on ArgumentHash
| project BuildDefinitionName, TestName, Arguments, MeanPassOnRetryRate=(todouble(numerator) / todouble(denom)), PassOnRetryCount=numerator, TotalRunCount=denom
| order by MeanPassOnRetryRate;
```
</details>

:part_alternation_mark: [Link](https://dataexplorer.azure.com/clusters/engsrvprod/databases/engineeringdata?query=H4sIAAAAAAAAA51Uy24TQRC8+ytakSKthYkThQOK5UiBgLgEI5MfaHt64yGzM6t52N6IA7/B7/ElqZkljmPlgPDJ24+q6ureHY9pLtFrCcReqBG2kaKjZBfGLe+JraLWy1oQZlokbRTV3jVUszba3pFKkuufHqOEOEIdQFbSFcwQtTGkrdJLjnot5GrAb0Akihay4rV2fkSD8Tj3eKmdlxFthKygANjRM5Ss3AadUSx0ZBZqOQTI3qwQ8nszaBu9U2kp6oQ+bS/oqi9fMdJ0dnp6XDrJc5ReqbOm62HA3+vIDV7e+mTRBFKP1jxiAj7M8B0FvaWcHlFwpOOfX78DybaVZZ5qzQa2YJAdiA70/vzk/PiEBkZgTqApnb1TE3r1Bwm3upHQsu2xSpOX1qHtSLloJY7ZL1nJ0WSvadZG7SybCwpS1ihNG6EV5mA3uqbOJcrOAyskAxVll4z9ZPCgo8s2FraQFpEXRsB49YC5r2U9a8MtnAnfU9Ow7wY/s2twZI5eH69hKF1Oie9cFcOwnI6u66qXDd1Ho1JZWLoceX46zOSeHuEbdjWz+Ua7jy5B+SWdgjgUCfoBV5IawS6dnyJWHZYPR6TEuqYkP2ODJfrmlTIOszqPMG14Wz1PNKRFRx/y3V9Lra3OBn/lBqeTrej/Xfk7iLDxC4fVpJjHe5Ebbv/XwxeDQsc+0TNtmAyelrWDK0P/9co4d59aqg5EDXH4LxBR2nr3Ayf8rwPj+m/wxdhzc54drKJTLkFOtVvOkPBWPUWLuCE8P1zDdFcPNhfZzJPtE6UFAp1XeBlhxSu8k0dFwXcxzQQAAA==) to query editor

## Overall Average Pass Rate for a Test (Including Retries)

Query suggested by @dougbu for getting the overall average pass rate for tests, including retries. 

<details>
  <summary>Expand for query</summary>

Variables: 
- `ts`: [Kusto timespan format](https://docs.microsoft.com/en-us/azure/data-explorer/kusto/query/scalar-data-types/timespan). How many days ago to query.
- `definition`: Build definition name. 
- `repo`: Repository to filter on. Set to empty string to inclue all repositories. 
- `branch`: Name of the target branch. 
- `testName`: Test name. 
- `excludeAlwaysPassing`: Set to true to filter out tests that are always passing. 

```
let ts = 30d;                                                  // Timespan value to find test results after. 
let definition = "";                                           // Optional: The name of the build definition, leave as empty string for all build definitions.
let repo = "dotnet/aspnetcore";                                // Optional: set to empty string if you want results from all repositories
let branch = "";                                               // Optional: The name of the target branch the test ran against.
let testName="Templates.Test.GrpcTemplateTest.GrpcTemplate";   // Optional: The name of the test
let excludeAlwaysPassing = "";                                 // Boolean. Set to true to exclude test results that are always passing
let subtable = AzureDevOpsTestsSummary
    | where ReportDate >= ago(ts)
    | where isempty(definition) or BuildDefinitionName has definition
    | where isempty(repo) or Repository has repo
    | where isempty(branch) or Branch has branch
    | where isempty(testName) or TestName has testName
    | summarize
        numerator=sum(PassCount),
        denomerator=sum(PassCount) + sum(FailCount) + sum(PassOnRetryCount),
        passOnRetryCount=sum(PassOnRetryCount)
        by TestName, ArgumentHash;
let argumentHashMap = AzureDevOpsTestsSummary
    | where ReportDate >= ago(ts)
    | summarize by ArgumentHash, Arguments;
subtable
| where denomerator > 0 and (isempty(excludeAlwaysPassing) or (todouble(numerator) / todouble(denomerator)) < 1)
| lookup (argumentHashMap) on ArgumentHash
| project
    TestName,
    Arguments,
    MeanPassRate=(todouble(numerator) / todouble(denomerator)),
    PassCount=numerator,
    PassOnRetryCount=passOnRetryCount,
    TotalRunCount=denomerator;
```
</details>

:part_alternation_mark: [Link](https://dataexplorer.azure.com/clusters/engsrvprod/databases/engineeringdata?query=H4sIAAAAAAAAA61UwW7bMAy95yuInmysSDrstswF2hXbLl2HND/AxEziTZYMiUrmYh8/SqqdOA3aBZgviSiS7/E92ooY2EEBH67KKZz9TCYwr2pyDWrYovIEbGBV6RKYHIMl55W0xxWTHcNICVpJcl9xZbSgXlycAypoD02oRPUR5hsCjTWBWQHL/4WvVHnQ/RIU4ZYAHVDdcAuObaXXsDIWUKkX+W4c6VlqTCBWGtbEE3SN/CyNpbeZDui5IKwZQlcraI2HHeq9NCtr6sgnALuKja3IRSYLi3q5OVukN4VitOt9+xiJXomFuMZKO05KhOh3qSsu5jKFQjmP5xIbf7XNsgu9CESur+NLSQSg30vlS7pRO2zdD3QuaPRv4wrArTFisB7DY1Kabdq+567DBeQNMqCVbYhg0CS0SMP5BeNCkUDfPHlLd7R9aFwYzD36ukbbjgLkH9htSDrMxCfLdzIqXBeimMnY5YOMykXXs/1u5SBLdxsW7q6PBWlhI9u5TzvZJexFrJ91C9LGshA/WZCMTZDJ45CeoicLOqNjyfz5EIu6m+cyF+WontI5PNrXZFE4FXKXBQ8/G685v+wzStLmdA68Cw2zL1ipQSBkPOgZsW2PmzVHV8XJ/D590fbjXMKNXQtZzd/QbabReDyI3GPzH/zvBQrQh4B7eDcddQs36loeaATXcAUo38+sc+fUWxKdytiUxkufrHchhwn00YOueQ6f4H0ugMqYX76B7Gh2aagHhCW1seYnLTmO1ssYT/0s6Xgvr2EgNhNNirNYpfp+JYq+ZH8xsPvY/5Q2N4xq5nXKOQCY/gUanUNL3gYAAA==) to query editor

## Search Failed Test Results as with Runfo

The `AzureDevOpsTests` table collects the details of failed test results, making it searchable as one would with [searching for tests with Runfo](https://runfo.azurewebsites.net/search/tests/#test). Here's a query to get you started: 

<details>
  <summary>Expand for query</summary>

Variables: 
- `started`: [Kusto timespan format](https://docs.microsoft.com/en-us/azure/data-explorer/kusto/query/scalar-data-types/timespan). How many days ago to query.
- `defintion`: Build definition name
- `reason`: The Azure DevOps build reason: `Schedule`, `IndividualCI`, `PullRequest`, `Manual`, and `BatchedCI`
- `targetBranch`: Name of the target branch
- `name`: Test name
- `jobName`: Job name
- `message`: Error message
- `workItemName`: Work item name

```
let started = 7d;                    // Timespan value to find test results after. 
let definition = "aspnetcore-ci";    // Optional: The name of the build definition, leave as empty string for all build definitions.
let reason = "BatchedCI";            // Optional: The Azure DevOps build reason value (e.g. PullRequest, BatchedCI, et cetera)
let targetBranch = "main";           // Optional: The name of the target branch the test ran against.
let name = "";                       // Optional: The name of the test
let jobName = "";                    // Optional: The name of the job
let message = "AggregateException";  // Optional: Error message to search for
let workItemName = "";               // Optional: Work item name
AzureDevOpsTests
| where RunCompleted >= ago(started)
| where isempty(definition) or BuildDefinitionName has definition
| where isempty(reason) or BuildReason has reason
| where isempty(targetBranch) or Branch has targetBranch
| where isempty(name) or TestName has name
| where isempty(message) or Message has message
| where isempty(workItemName) or WorkItemName has workItemName
```
</details>

:part_alternation_mark: [Link](https://dataexplorer.azure.com/clusters/engsrvprod/databases/engineeringdata?query=H4sIAAAAAAAAA4VTTU/rMBC8V+p/WPXUSqUckah4EgUOHPhQVYnzNtkmBscO9qZ86P34t7bz2pRA6Slaz8zO7k41MXhGx5TDBZzlc/jmd3oKK1WRr9HAFnVDwBY2yuTA5Bkc+UazB9wwuRkMB1pEcxKAYmWN6I7Q14Y4s45OMjWat6IPdXhHfQ6rksBgRWA3wPK9bpTOOxpT0IRbAvRAVc0f4tkpU8DGOkCte3g/Sy4coU8OFshZSfnV7Wj+ZbRDF5efjSO4pu1D7VvZViRNPqZZMYPHRuslvTYy/hR20lOQlhnJFnCS+stmC+KFQ5OVwUWFyhwYOLqFxIZ1osdK2LfyIBXAQsQ8t5NGnnQYfXvBXzuJbtJ5tuv7o1JHdYSdZCQtHosoc1kUjgpkunnPKDKD8IHMjXNyyP8cCZcndDKynDfJvVn3cstU/WjtQO5J0KAEHr0NB/Gm6aQrGdQPB3/hrSS587IxV7aqpYXk/8+F7NSO27/DZI9SPoZuvM/XBMTvIqTjeleL3kpJ6B7Wl0hZ2tOXKVuBlp76lG6GEjHlIXC6b31mmD4ywtQ7e2knX7Ht8iP8rj1EQLf1PqF7ksh66t4oULuIf4oms49oBAAA) to query editor

## Search Timeline as with Runfo

The `TimelineIssues` and `TimelineBuilds` tables can be used to [search timeline issues as you would with Runfo](https://runfo.azurewebsites.net/search/tests/#timeline). Here's a query to get you started. 

<details>
  <summary>Expand for query</summary>

Variables: 
- `started`: [Kusto timespan format](https://docs.microsoft.com/en-us/azure/data-explorer/kusto/query/scalar-data-types/timespan). How many days ago to query.
- `definition`: Build definition name
- `reason`: The Azure DevOps build reason: `manual`, `schedule`, `individualCI`, `batchedCI`, and `pullRequest`
- `result`: State of the build
- `targetBranch`: Name of the target branch
- `message`: Error message
- `type`: Warning or Error?
- `jobName`: Job name
- `taskName`: Task name

```
let started = 7d;                                  // Timespan value to find test results after. 
let definition = "\\dotnet\\roslyn\\roslyn-CI";    // Optional: The name of the build definition, leave as empty string for all build definitions.
let reason = "";                                   // Optional: The Azure DevOps build reason value (e.g. pullRequest, batchedCI, manual, individualCI)
let result = "";                                   // Optional: The state of the build (e.g. succeeded, failed, canceled, and partiallysucceeded)
let targetBranch = "main";                         // Optional: The name of the target branch the test is ran against.
let message = "timeout";                           // Optional: Error message to search for
let type = "";                                     // Optional: warning or error
let jobName = "Build_Windows_Release";             // Optional: Issues associated with jobs with this name
let taskName = "";                                 // Optional: Issues associated with tasks with this task name
TimelineIssues
| where isempty(message) or Message has message
| where isempty(type) or Type has type
| join kind=inner TimelineBuilds on BuildId
| where StartTime >= ago(started)
| where isempty(definition) or Definition has definition
| where isempty(reason) or Reason has reason
| where isempty(result) or Result == result
| where isempty(targetBranch) or TargetBranch has targetBranch
| join kind=inner TimelineRecords on BuildId
| where isempty(taskName) or TaskName has taskName
| where isempty(jobName) or Name has jobName
| order by StartTime
```
</details>

:part_alternation_mark: [Link](https://dataexplorer.azure.com/clusters/engsrvprod/databases/engineeringdata?query=H4sIAAAAAAAAA51UwW7aQBC9R8o/jDiBRMmxUiMqJaQHDm0kitQLUrTYA95k2XV31iCqfnxnvOvYwSlF5eL1MPPe7Js3NhiAgvIBc5jCx/wW/vm7uYGl3iGVysJemQohONhom0NACuCRKhMI1Cagn8D1lWGKHDlBB+0sswxWq9wFi2G18o7M0TbPD7P54DZRPJaSrcwnWBYIVu0Q3AYCn9eVNnkHcQwG1R5BEeCuDEe+j9d2CxvnQRnTy6dJ7MmjotjP4IJb93u6+1V5hAfcP5aUSBJkVGWIk+0EysqYBf6sWJoxrFXICsxn8zHslK2UGQPrpvc65/NsPmoaEwX/vzGeZzhRK/ZCVZYh5piPYaO0kWembIb1SfEASzaCZs2Or5mpJXbIFsO95+xCGtspbc80d3Z+EQvWEayOiG80AUdAbRmaQpoR24zUFoUysOdcFc5K8ob3i/fsgAaBPUqoPDOyL9KljiVerPIJ+EF5KyZjBhSeiPjs1t/kpgx6L7o//eDxugM9LVhjRXjC9AZxTsQmYReTy7SSfTzoUAgkxVMoWCIRshkJvTRkF9zgEi6B7JLJe2KUjTfaYqy8vvoNhwLZ/prqnRsmmUciyNckecEbmeL9AhG/zl7KFCRVIpL37LSFFxZuqq1FDw11rSgB71d9muct6Hf5gkkefJ6yg9wwfdJGfd72O1CzP7QfJumh/bdfGZe7rlrEPZeKGH0vW5Y4Zcd9nqbNfkeMznpFUbr7VovTCZwTaYGZ839RqWWLzklMyUaRJb70a5Kz65LX9BSUbCblLtbHdhR/AH9S5UBaBgAA) to query editor

## Build Analysis Reporting

This Power BI page contains the following reports: 
- Details of PRs outcomes when merged (e.g. when a PR was merged on red)
- Build outcomes and retry metrics
- Tests that pass on rerun

:part_alternation_mark: [Link](https://msit.powerbi.com/groups/me/reports/8bdd4339-ea39-4e67-963a-fc44a450605b/ReportSection?ctid=72f988bf-86f1-41af-91ab-2d7cd011db47) to Power BI Report

## Sentiment Tracker Feedback

This report tracks the usage and trends of the feedback we receive via the sentiment tracker in the Build Analysis check on the PRs. 

:part_alternation_mark: [Link](https://msit.powerbi.com/groups/de8c4cb8-b06d-4af8-8609-3182bb4bdc7c/reports/e6deb422-46fc-4f80-8892-ba7036081986/ReportSection) to Power BI Report


<!-- Begin Generated Content: Doc Feedback -->
<sub>Was this helpful? [![Yes](https://helix.dot.net/f/ip/5?p=Documentation%5CAzureDevOps%5CTestReportingQueries.md)](https://helix.dot.net/f/p/5?p=Documentation%5CAzureDevOps%5CTestReportingQueries.md) [![No](https://helix.dot.net/f/in)](https://helix.dot.net/f/n/5?p=Documentation%5CAzureDevOps%5CTestReportingQueries.md)</sub>
<!-- End Generated Content-->
