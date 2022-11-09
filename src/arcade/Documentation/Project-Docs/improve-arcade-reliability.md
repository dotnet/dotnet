# Improve Arcade Reliability

The goal of this effort is to help us improve Arcade Validation by filling in its test gaps, while mitigating potential build breaks before the latest version of Arcade is pushed out to customers. 

## Step 1

We will reassess this step after two weeks to see if we're getting any value out of it. 

1. Create a scheduled pipeline similar to Arcade Validation today, with some alterations: 
   1. If Arcade-Validation passes, create a branch of the `runtime` and `roslyn` repos using the last known good build within the last three days. (If there are no last known good builds in the last three days, we will skip building the latest Arcade against that repository.)
   2. Update those branches with the latest version of Arcade being validated. 
   3. Build those branches and check for errors. 
      1. Should any errors occur, Arcade SMEs should help to determine if the errors are related to Arcade or not. 
         1. If the errors are related to Arcade, authors of commits included in the latest Arcade build since the last Arcade version release, must be contacted. 
         2. Authors will be expected to fix whatever in their commits broke the other repos. 
         3. Authors will be expected to add tests/validation to Arcade-Validation so that it will be caught in the future. 
      2. If the errors are unrelated to Arcade we will send the Arcade build to the Latest channel to proceed with the normal dependency flow. 
   4. The pipeline should run 3 times a week, scheduled during non-peak hours. 
   5. Clean up branches of non Arcade-Validation repos used. 
   6. Ensure that we can use existing telemetry to capture any data from the builds. Data from SME triage/investigation can be stored in an Excel spreadsheet (or another low cost document.) Data we want to capture: 
      1. Repo and SHA branched from (since we are using the last known good build in the last three days).
      2. Result of the investigation: 
         1. Passing (document that we ran Arcade through that branch and it was passing)
         2. Failure due to product (this will include the scenario if there are no last known good builds in the last three days, thus, we do not build Arcade against that repo)
         3. Failure due to Arcade/infra failure (this will result in any authors fixing the bug and contributing to the tests in Arcade Validation)
         4. Failure due to "Impedance Mismatch" (this is when there's too much churn)
2. After this process is set up, we will turn off automatic dependecy flow of Arcade. 

## Step 2

1. As test gaps are identified in Arcade Validation, we need to fill in those gaps with tests. This will be an ongoing process as test gaps are identified during the short term solution. 
2. Once we are satisified with the robustness of Arcade Validation, we can re-enable automatic dependency flow of Arcade to the Latest channel. 
3. Ensure that we have process/policy in place to promote adding validation to Arcade Validition when changes are made to Arcade. 

## Known Testing Gaps

| Issue | Description | Root Cause | Resolution |
| ----- | ----------- | ---------- | ---------- |
| https://github.com/dotnet/arcade/issues/4660 | Roslyn signed builds failed to publish on 1/20/2020 to the package feeds in Dnceng due to authorization issues after taking an Arcade update. | A refactoring of powershell scripts done in 11/21/2019 made it so the script that enables the authentication for publishing and restore across AzDO account boundaries stopped running. | Made sure the script always runs during AzDO builds in https://github.com/dotnet/arcade/pull/4661 |
| https://github.com/dotnet/arcade/issues/4759 | Roslyn signed builds failed to queue due to a missing variable group after taking an Arcade update. | We refactored a YAML template so that the set of common variables that are required by post-build validation and publishing were shared across the stage instead of being referenced in each individual job. This caused the Validation stage to try and load a variable group that didn’t exist in DevDiv.<br/>This break made it apparent that SDL validation was never set up to work for repos outside of dnceng as there was a variable group missing. | The variable group was created in DevDiv with the required variables and subsequent builds were queued successfully. |
| https://github.com/dotnet/arcade/issues/4748 | A Roslyn build was published to the “.NET Core SDK 3.1.2xx” channel in the Build Asset Registry, but no packages were actually published, so the Dependency flow PRs opened by this build were all failing to restore the packages. | The branch that produced the build was using a version of Arcade that didn’t have the publishing set up for that channel. | The branch was updated to use the latest arcade in the “.NET 3 Eng” channel, which brought in the correct publishing templates, and we’re working on adding a warning to builds that try to publish to channels and there’s no publishing implementation available in the YAML templates flowed to that branch. |
| https://github.com/dotnet/arcade/issues/4775<br/>https://github.com/dotnet/arcade/issues/4728 | The darc version that gets installed by default from the darc-init scripts fails during any operation that uses the paged APIs. | The auto rest client generator generated an invalid client that would fail with a 404 for any APIs that are paged, such as update-dependencies, get-builds, or get-asset. | The generator, and the generated client Darc have already been patched, but we require a production deployment so that the darc-init scripts install a fixed version by default. |
| https://github.com/dotnet/arcade/issues/4860 | | Roslyn Updated the SDK version they use to build their compilers to 3.1, this change flowed to Arcade, and caused every Repo that wasn’t using a 3.1 SDK to break when compiling certain types of code that we don’t build in Arcade itself | |


<!-- Begin Generated Content: Doc Feedback -->
<sub>Was this helpful? [![Yes](https://helix.dot.net/f/ip/5?p=Documentation%5CProject-Docs%5Cimprove-arcade-reliability.md)](https://helix.dot.net/f/p/5?p=Documentation%5CProject-Docs%5Cimprove-arcade-reliability.md) [![No](https://helix.dot.net/f/in)](https://helix.dot.net/f/n/5?p=Documentation%5CProject-Docs%5Cimprove-arcade-reliability.md)</sub>
<!-- End Generated Content-->
