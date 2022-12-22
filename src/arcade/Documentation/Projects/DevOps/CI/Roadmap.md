# .NET Core CI Telemetry Roadmap

## Phase 1 - Build telemetry reporting

**Estimated Completion Date:** 6/14

### Summary

After this phase completes, we will be surfacing reasonable errors from our builds that help us differentiate categories of failures. We will surface data to the Azure DevOps Timeline API.  We will also provide the tools such that if surfaced failures do not provide enough differentiable value, we can tweak the telemetry / guidance.

### Phase 1a - .NET Core Engineering Telemetry Reporting

Phase 1a and 1b (below) are parallel work streams.

#### Work Items

- In progress - provide [guidance](https://github.com/dotnet/core-eng/issues/6390) on what “good” error telemetry is.  Guidance can also include general rules for different types of failures (timeouts, cancelled jobs, bad yaml parsing, other azdo failures, etc...)

  [Guidance document](./Telemetry-Guidance.md)

- In progress - [report powershell pipeline errors](https://github.com/dotnet/arcade/issues/2038)

- Done - report bash pipeline errors

- Done - report Linux MSBuild errors

- Done - report Windows MSBuild errors

### Phase 1b - Continue Gathering Telemetry

In parallel with phase 1a (above).

Jeff Schwartz (w/ Jared Parsons' help) is going to update Jared Parsons' telemetry gathering tool to push to a Kusto (staging) database or at least set up the process such that repos can capture telemetry and self-report it to a central location.  This will give us real data that we can analyze during CI council and we can later converge with the workstreams in Phases 2/3.

Jeff is also going to look at what data is useful and the kind of reports that are valuable, working towards Phase 3 where applicable.

## Phase 2 – Capture telemetry

**Estimated Completion Date:** 7/19/2019

### Summary

After this phase completes, data from the Azure DevOps Timeline API will be stored in a database we own where it can be used to drive phase 3.

### Work Items

- Setup Kusto database for storing telemetry results

- Determine the format for Kusto tables

- Create an Azure DevOps task that can be used to gather telemetry the Timeline API and move into our database / format

  - Modify and package https://github.com/jaredpar/AzureUtil
  
  - Task acquires AzureUtil package and [LightIngest](https://kusto.azurewebsites.net/docs/tools/lightingest.html) package
  
  - Task authenticates with Azure Key Vault to acquire secrets for lightingest
  
  - Task ingests into Kusto database

- Create an dnceng build definition that runs on a scheduled trigger which will be used to gather telemetry on a defined cadence (daily? hourly? weekly?) or create a service that triggers automatically based on REST API calls or schedule.

## Phase 3 – Provide reports

**Estimated Completion Date:** TBD

### Summary

After this phase completes, teams will be able to look at telemetry driven reports that can be used to help them understand the health of their CI system and make decisions about what tasks require attention.

### Work Items

- Determine what information is useful to make decisions about CI “health”

- Provide a place to view reports.

  - Current thinking is Azure DevOps dashboard and / or each builds "Build Analytics" tab

### Open Issues

- Chcosta - can we extend the Build Analytics tab.  

- Chcosta - Can Azure DevOps automatically surface information we put in the Timeline API if we structure it correctly?

- Chcosta - The Build Analytics tab's "task failure trends" section doesn't appear to be working

- Chcosta - To work with Jeff Schwartz for what he found valuable / problematic in his investigations

## Phase 4 – Reassess

**Estimated Completion Date:** TBD

### Summary

Assess the state of our system and prioritize additional goals

- Work with Jared / Nate to determine how best to manage / report on test flakiness

- Are we tackling the right issues / surfacing the right data? Do we have coverage in the right areas (CI, PR, official)?


<!-- Begin Generated Content: Doc Feedback -->
<sub>Was this helpful? [![Yes](https://helix.dot.net/f/ip/5?p=Documentation%5CProjects%5CDevOps%5CCI%5CRoadmap.md)](https://helix.dot.net/f/p/5?p=Documentation%5CProjects%5CDevOps%5CCI%5CRoadmap.md) [![No](https://helix.dot.net/f/in)](https://helix.dot.net/f/n/5?p=Documentation%5CProjects%5CDevOps%5CCI%5CRoadmap.md)</sub>
<!-- End Generated Content-->
