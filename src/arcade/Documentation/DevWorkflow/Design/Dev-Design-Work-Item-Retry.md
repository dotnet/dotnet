# Dev Design for Work Item Retry and Result Recording
Today in Helix, the Helix SDK (code that lives inside arcade) handles all of the running and uploading of helix results.
Unfortunately, this means that the Helix systems and services have no visibility into this behavior, and cannot modify
or depend on it.

In order to more strongly integrate this into Helix, allowing us stronger controls and reporting, we need to change this
to be more embedded into Helix itself.

The changes should require no sweeping changes to repositories other than arcade and helix, product repositories will
get the correct behavior by virtue of having the eng/test-configuration.json file.

## Current flow
1. Arcade code generates a workitem payload, embedding the Azure Pipelines tokens necessary to report back to the test run
1. Arcade send this workitem to helix
1. Helix runs this workitem, ignorant of what is happening inside
  1. The python code inside the workitem directly parses the results and uploads it to Azure using
     https://docs.microsoft.com/en-us/rest/api/azure/devops/test/results/add?view=azure-devops-rest-5.0
  1. If this workitem get's retried for any reason (like infrastructure failures), the reporting is nonsense
1. The workitem completes, and helix uploads any completed artifacts to helix storage
1. The Azure Helix Logs plugin combines data in Azure with the data in Helix to produce a report

#### Issues
* This behavior is the only thing that causes test workitem to not be idempotent, which is an important quality of a queue based system.
  Because helix sometimes needs to retry workitems (for example, if the "Finished" even fails to upload), this can cause
  duplicate reporting to Azure, leading to confusing results.  It means we can't safely run a sample workitem to validate a fix / repro a problem.
  In general, if idempotency can be acheived, the queue based nature of our system can shine to help identify and correct
  systematic errors.
* Code inside the workitem cannot get to a different machine if that is desired to resolve test issues
* Helix is unaware that "retries" happen inside the workitem, so cannot differentiate them in reporting
* If there is a need to change this reporting, it involves pushing python code changes through arcade to every branch of every repository
* Version of this code gets very difficult, since we need to support arbitrary backwards compat because of release branches,
  and there is no clear boundary where this compatibility must be preserved.

## Proposed new flow
1. Arcade similar to today, creates a workitem that _runs_ the tests, and puts a file in a well defined format in a provided folder
  * This will include the results for the execution
  * Any necessary authentication necessary to report back
  * Any rerun instructions
1. Helix's client python will use this information to upload the results to Azure, similar to how the arcade code does this today
1. If the folder contains rerun instructions, Helix will use these to determin appropriate behavior.
  1. If a rerun is required, before uploading to Azure, it will run the workitem a second time, creating a second set of artifacts.
  1. These artifacts are combined to produce a final upload to Azure, using "SubResults" type of "rerun".
    * If all executions pass, report only the first result (to save upload time, which is significant), report test as passing
    * If some, but not all, executions fail, upload all runs, and mark the overall test as "passing with rerun"
    * If all executions fail, upload all runs and mark the overall test as failed.
  1. Each artifact will be related to the "attempt" that triggered it, so the correct console logs can be viewed for each result
  1. (Version 2) Helix will also provide a mechanism to run the workitem on a different machine in case machine configuration issues
    are at fault, if data shows this to be necessary, and local retries are not functioning

### Contract file formats

#### Result file
This will likely be a JSON file that is an intermediate step between the raw execution (XUnit XML or VSTest TRX files). The type of runner
isn't useful information for Helix, so it's fine for that to remain inside the workitem (which also allows teams to use other runners without
having to make modifications to the Helix server code).

Whatever is necessary to report back to Azure DevOps

Something like
```json
{
  "results": [
    {
      "testName": "System.Example.TestClass.TestMethod(\"Test argument\")",
      "duration": 1345,
      "result": "fail",
      "output" : "Full test output\nFrom runner",
      "failureMessage": "Expected: 5\nActual: 4",
      "callstack": "A callstack\nat a place",
  ]
```

The code that handles producing this will live in arcade, since that's where it lives now, and there is no compelling reason to move it.

#### Rerun instructions
Rerun instructions will just be a copy of the eng\test-configuration.json file from the repository
* Includes number of rerun attempts
* Any qualifiers for which tests to rerun or not

If this file is missing, no reruns are performed, and the single result is uploaded just as current

#### Parameters
Since uploading to an Azure pipeline requires authentication, a file will need to be generated including a token capable of uploading to that
specific test run. This is already in place today, and is passed

## Testing and deployment
We current have no infrastructure for testing the python code that drives the bulk of the helix system. This will need to be written to make sure
the logic in this code, including the strong file and location contracts are respected. This will be a large expense, as the python code
is not well structured for testing.

Most of the code logic will be made in the helix-machines repository, because that is where the python code resides today.  It could theoretically be
moved, but the value isn't there for that.


<!-- Begin Generated Content: Doc Feedback -->
<sub>Was this helpful? [![Yes](https://helix.dot.net/f/ip/5?p=Documentation%5CDevWorkflow%5CDesign%5CDev-Design-Work-Item-Retry.md)](https://helix.dot.net/f/p/5?p=Documentation%5CDevWorkflow%5CDesign%5CDev-Design-Work-Item-Retry.md) [![No](https://helix.dot.net/f/in)](https://helix.dot.net/f/n/5?p=Documentation%5CDevWorkflow%5CDesign%5CDev-Design-Work-Item-Retry.md)</sub>
<!-- End Generated Content-->
