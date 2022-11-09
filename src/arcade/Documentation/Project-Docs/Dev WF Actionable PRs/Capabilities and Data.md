# Capabilities and Data

Taking as inputs the discussions had so far, the Checks Tab mock-ups Chad completed and the scoping done during our latest Gap-minding meeting I did some investigations and experiments to better understand the AzDO APIs, especially around tests. I'm happy to say it looks like it will be a good solution for augmenting data we already store in Kusto, solving problems of timeliness (e.g. Kusto ingestion delay) and scale (e.g. test results).  

Going through the meeting notes and the Checks Tab mocks, I organized the questions this system is planning to answer and where that information will be retrieved.  

Generally, the Checks Tab frames information in terms of

- Build failures
- Test failures

Each type of failure showing information about 

- If unique
- If seen previously
- If happening now or recently on target (master) branch 

## Concerning current build failure information 

Run error information determined from aggregate of 

- AzDO Get Build API 
- AzDO Build Timeline API 
- AzDO Get Test Run API 

Answering questions or displaying information:  

- "\<failed testname> [Test] [History] [Artifacts]" 
- "Exception message \<message>" 
- "Callstack \<callstack>" 

## Concerning Build Retry information 

Retry information is aggregated from 

- Current build failure information 
- Auto-retry driving telemetry in SQL "Known Failure" table 

Answering questions 

- "This test \<likely, does not likely> pass on retry" 
- Concerning Branch Status 
- "The target branch (master) \<is/is not> failing" (AzDO "Get Build" API) 

## Concerning Historical build failure information 

- "This step first failed in master on \<date>" (Kusto "Timeline*" tables) 
- "This step has failed \<count> out of \<count> runs in master, most recently on \<date>" (Kusto "Timeline*" tables) 

## Concerning test history by branch, test pass/failure rate,  

- "There are test failures in this build for pipelines that are also failing in master" (AzDO "Query Test History" API) 
- "The test \<testname> has failed <count> out of <count> runs" (AzDO "Query Test History" API) 
- "The test \<testname> first failed on master at \<buildid> on \<date>" (AzDO "Query Test History" API) 
- "The test \<testname> latest failed on master at \<buildid> on \<date>" (AzDO "Query Test History" API) 
- "The test \<testname> was introduced on \<date>" (AzDO "Query Test History" API) 

<!-- Begin Generated Content: Doc Feedback -->
<sub>Was this helpful? [![Yes](https://helix.dot.net/f/ip/5?p=Documentation%5CProject-Docs%5CDev%20WF%20Actionable%20PRs%5CCapabilities%20and%20Data.md)](https://helix.dot.net/f/p/5?p=Documentation%5CProject-Docs%5CDev%20WF%20Actionable%20PRs%5CCapabilities%20and%20Data.md) [![No](https://helix.dot.net/f/in)](https://helix.dot.net/f/n/5?p=Documentation%5CProject-Docs%5CDev%20WF%20Actionable%20PRs%5CCapabilities%20and%20Data.md)</sub>
<!-- End Generated Content-->
