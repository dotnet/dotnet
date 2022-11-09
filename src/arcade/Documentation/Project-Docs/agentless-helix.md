# Allow for waiting for helix to use agentless tasks

## Overview
We want to reduce wasted compute caused by build jobs waiting for helix tests to complete. To do this we can use the "agentless job" feature in azure pipelines to remove the machine that waits for the helix job to be finished.

## Stakeholders
- Our Budget
- Customers who want to retry test jobs without having to rebuild

## Pros
- Build machine time is no longer wasted on waiting for helix jobs
- Test job retry doesn't require rebuilding the tests

## Cons
- Msbuild no longer waits for the helix jobs, so code written in msbuild to process job results needs to either go away or be rewritten server side.

## Current Workflow
The current build jobs that run helix work do the following:
1. Acquire build agent from pool
1. Compile code and create helix payloads and job list
1. Start azure pipelines test runs
1. Send Job to Helix via rest api
1. Poll rest api periodically to check status
1. Once job reports finished, verify results and finalize test runs
1. Return build agent to pool

This workflow holds on to a build agent for a long period where it is doing nothing but polling a rest api for status, this period can be removed.

## New Workflow
Using agentless jobs, we can change this flow to the following:
1. Acquire build agent from pool
1. Compile code and create helix payloads and job list
1. Save job information to output variable
1. Return build agent to pool
1. In agentless job:
    1. Read output variable from previous job/stage and send request to helix api using [Invoke Rest Api](https://learn.microsoft.com/en-us/azure/devops/pipelines/tasks/utility/http-rest-api?view=azure-devops)
    1. Using the "Callback" completion mode agentless job waits for notifications from helix service
1. In Helix Service
    1. Receive request from agentless job and store information
    1. Start test runs
    1. Start helix job execution
    1. Wait for job completion - send progress logs back to azure pipelines
    1. Report test results for workitem statuses
    1. Finish test runs and check for job pass/fail status
    1. Report task completion to azure pipelines
1. Agentless job finishes after receiving notification from helix service

This workflow gives back the build agent to the pool shortly after finishing the build, and allows the helix jobs to run without requiring a build agent to be listening.
The agentless job can also be retried to re-run the helix job(s) without requiring a rebuild.

## Proof of Concept
I have a proof of concept in this PR https://github.com/dotnet/arcade/pull/10342. The agentless job can be seen running here https://dev.azure.com/dnceng-public/public/_build/results?buildId=55561&view=logs&j=830c6850-7aa7-5384-6c90-1a1a71217f4b.

## Implementation details
This solution requires a web api endpoint on our server that handles the request from the agentless job, then a service running inside our cluster that starts and monitors jobs based on these requests. This service will need to fixup the job payloads with test run information, then monitor execution and report status back to azdo.
