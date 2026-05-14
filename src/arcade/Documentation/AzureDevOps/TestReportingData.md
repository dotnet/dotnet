# Working with Test Reporting Data

## What is the data? 

The data that we collect is only a subset of tests that run and not every test that runs. **We've chosen to collect results of tests that have failed in the last 90 days**. We assume that tests that are always passing or have continued to pass after 90 days are no longer interesting to look at, and thus, their results are no longer collected. 

## How do we collect the data? 

The test results data we collect come from tests that run through Helix. The results are then captured in the Kusto tables noted below. 

Example scenarios for captured test result data: 
- Tests that run during a pull request build.
- Tests that run during a rolling CI build. 

As long as our PAT has access to the Azure DevOps project the test runs in, we can collect results from tests that run in that Azure DevOps project. 

## Where is the data? 

Kusto's `Engsrvprod/engineeringdata` cluster contains the following tables: 

- AzureDevOpsTests
- AzureDevOpsTestsHourly
- AzureDevOpsTestsSummary
- AzureDevOpsTestAnalysis

### AzureDevOpsTests

This table contains the raw data that we collect for test results. Keep in mind that due to the large amount of data we are collecting, we are not including initial passing results in the raw table. However, you can see aggregates of passing results in other tables. 

Notable fields in this table include: 

**QueueName**: The OS queue the test ran on. Useful when needing to track down a configuration a test ran on. 

**Branch**: Name of the code branch the test ran from.

**Outcome**: Contains values of `Failed`, `PassedOnRerun`, and `NotExecuted`.

**RunCompleted**: The datetime value of when this test run completed. 

### AzureDevOpsTestsHourly

Due to the amount of data that we need to process, we start with hourly aggregations of test results. The data collected on this table is for all failed tests in the last 90 days that we are tracking, so it may occur that we have a row for a test that has not ran in the last hour, but has failed in the last 90 days. 

Because this table is an aggregate, you'll notice that some of the fields, such as `QueueName` and `BuildReason` are not available, as they are aggregated. (This means the aggregates will contain all pass/fail results of the test regardless of which configuration it ran against or if it occurred during a CI build or Pull Request)

Notable fields in this table include:

**Branch**: Name of the code branch the test ran from. 

**PassCount**: How many times the test passed on the first try in the last hour.

**FailCount**: How many times the test failed in the last hour. 

**PassOnRetryCount**: How many times the test passed after a retry in the last hour. 

**ReportDate**: The latest datetime value of data that was aggregated in this row. 

### AzureDevOpsTestsSummary

Daily, we aggregate the hourly values into this table. It contains nearly the same fields as the `AzureDevOpsTestsHourly` table does, with the exception of fields we track for debugging purposes. 

### AzureDevOpsTestAnalysis

This table contains the results of running the AzureDevOpsTestSummary results through [Chi-Squared Distribution](https://en.wikipedia.org/wiki/Chi-squared_distribution) in order to get a statistical analysis of the failures. 

Categories used when performing the chi-squared distribution: 
- Status
    - Passing = Passed test + Passed on Rerun (since these also "passed" once) 
    - Failed = Failing test + Passed on Rerun (since these also "failed" once)
- Time
    - Historical = Test results between 30 and 7 days ago
    - Current = Test results between 7 and 1 days ago. 

Notable fields in this table include:

**ReportDate**: The latest datetime value of data that was used in the statistical analysis of the data in this row. 

**Significance**: The closer this value is to 1, the more likely something changed recently to cause this test to start failing. This value also indicates the difference between "Current" and "Historical"

**SplitDate**: The date of the first day of the week of data aggregated. This date also represents the difference between the "Current" and "Historical" columns. 


<!-- Begin Generated Content: Doc Feedback -->
<sub>Was this helpful? [![Yes](https://helix.dot.net/f/ip/5?p=Documentation%5CAzureDevOps%5CTestReportingData.md)](https://helix.dot.net/f/p/5?p=Documentation%5CAzureDevOps%5CTestReportingData.md) [![No](https://helix.dot.net/f/in)](https://helix.dot.net/f/n/5?p=Documentation%5CAzureDevOps%5CTestReportingData.md)</sub>
<!-- End Generated Content-->
