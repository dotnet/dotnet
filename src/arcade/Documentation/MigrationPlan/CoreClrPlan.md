# .NET Core 3 Engineering transition plan (dotnet/coreclr) 
## Overall status
Official builds and Dependency Flow are complete.  There are just a couple of issues around symbol
publishing affecting ingestion by the upstream repos.

CI is close, but we are tracking a number of issues preventing the Default-trigger jobs from running
green against PRs.  External issues are labeled below.

## Primary Deliverable Breakdown of remaining work

### Internal builds from dnceng (overlaps dependency flow)
Outstanding issues
- [Enable transport packages for core-setup](https://github.com/dotnet/coreclr/issues/21897)
- [Symbol publishing issues in coreclr official build](https://github.com/dotnet/coreclr/issues/21898)

|Task         |Estimate remaining (days)|Estimate confidence (%)  |Priority 1 |Priority 2| Done |
|-------------|-------------------|---------------------|---|---|---|
| Decide what to do about native dependencies in official build|0|-|X| |X|
| Add signing to official product builds|0.5|90|X| |(Working through the last set of issues on this)|
| Add red hat product build job|0|-|X| |X|
| Add linux-musl product build job|0|-|X| |X|
| Publish packages to blob feed (this is part of dependency flow requirement)|1|50|X| |X|
| Publish symbols|1|50|X| | |
| Update Build Assets Registry (this is part of dependency flow requirement)|0.5|60|X| |X|
| IBC and PGO data|3|30| |X||
| Validate output against buildpipeline official build|1|80| |X| |
| Turn off buildpipeline official build and remove old scripts|1|80| |X| |
|**Total**    |**8**                 | | | | |

### Engineering dependency flow (beyond what required for official build)
These Dependency Flow tasks are done, though they have only been lightly tested.  We won't be able
to tell whether everything comes together until the Official Builds are published and we try to 
enable Dependency Flow end to end.

|Task         |Estimate remaining (days)|Estimate confidence (%)  |Priority 1 |Priority 2| Done |
|-------------|-------------------|---------------------|---|---|---|
| Set up branches/channels/subscriptions|0|-|X| |X|
| Express package dependencies in new format|0|-|X| |X|
|**Total**    |**0**                 | | | | |

### Using Azure DevOps for CI
- **[EXTERNAL]** Alpine x64 don't have open queues 
 - I will need to conditionalize the submission step in yaml and skip if there is no queue to submit.
 - No issue tracking this in core-eng
 - Will be disable in open until we have an open queue

- **[EXTERNAL]** RHEL 6 are missing ICU dependencies
 - https://github.com/dotnet/core-eng/issues/4100 
 - MattGal https://github.com/dotnet/core-eng/issues/4100#issuecomment-447912274 said @echesakovMSFT this being still broken is expected, we hope to have this rolled out for usage today. (today is 22 days ago)
 - jonfortescue is just assigned himself on this
 - If it's not resolved I will disable the queue temporary in open

- **[EXTERNAL]** Windows/arm32 and Windows/arm64 are missing VC component for building arm32 and arm64
 - https://github.com/dotnet/core-eng/issues/4764#issuecomment-451274215 
 - Last update from 5 days ago: We're blocked right now because of a problem with one of the queues (specifically, the Wasm queue), so we can't do the rollout we need. We're trying to figure out what workarounds we have available right now.
 - Can't get build/test jobs running without this
 - I tested Helix Windows arm32 arm64 manually though and there was no issue

- **[EXTERNAL]** We recently starting hitting this *Operation did not complete successfully because the file contains a virus or potentially unwanted software*
 - https://github.com/dotnet/core-eng/issues/4827 -- my issue was closed as dup
 - https://github.com/dotnet/coreclr/issues/21843  -- coreclr issue
 - https://github.com/dotnet/core-eng/issues/4555 -- the issue core-eng are tracking

- The issue with building coreclr on OSX.1013 which fails to run on OSX.1012 
 - https://github.com/dotnet/core-eng/issues/4856 
 - Testing solution in https://github.com/dotnet/coreclr/pull/21816 (passing on OSX.1012 OSX.1013 - left to confirm OSX.1014)

- Remove Windows.71.Amd64, Windows.10.Nano.Amd64 from testing in open since they are failing due to the following coreclr issues
 - https://github.com/dotnet/coreclr/issues/21693 -- Nano hits an assertion in Checked build (no issue with Release though)
 - https://github.com/dotnet/coreclr/issues/21796 -- IJW tests fail due to missing api-ms-core-win-* dll and even with these files in place they continue failing with some assertion

- Failing on Debian.8.Amd64.Open
 - https://github.com/dotnet/core-eng/issues/4807
 - Maybe we don't need this queue at all since the minimum supported version of Debian is 9. Need to confirm this and also need to add Debian.9 queue and test on it.

- JIT.superpmi is broken by design
 - https://github.com/dotnet/coreclr/issues/21698
 - Will disable

- I am working on removing Python dependency in Helix running time right now
 - __TestEnv file is created on build agents (either via MSBuild or Python)
 - The command that is submitted to Helix will look like `%CORE_ROOT%\CoreRun.exe *`
 - The idea is to make the test running as transparent as possible
 - And allow in the future using *stable* drop of net core to run tests (instead of CoreRun)

The above issues are required to get innerloop jobs running green and enable the AzDO testing against PRs.  Result visualization will be an ongoing task.
We expect to tune things based on feedback as people start to use the new system.

- Still need to finish the visualization part of testing to get the new CI usable
 - Made a prototype 
 - Need to discuss with core-eng and submit PR (or encourage them implementing this)
 - This needed to be able to attach metadata to a test run to make it identifiable in AzDO Tests explorer 
 - There is also an issue opened by safern with his suggestions regarding AzDO Tests explorer improvements 
 - https://github.com/dotnet/core-eng/issues/4929 

|Task         |Estimate remaining (days)|Estimate confidence (%)  |Priority 1 |Priority 2| Done |
|-------------|-------------------|---------------------|---|---|---|
| Express test matrix with commented jobs for those not implemented|0|-|X| |X|
| Group test jobs into useful subsets--each with its own pipeline|0|-|X| |X|
| Add Formatting jobs|1|80| |X| |
| Add IBC collection and consumption|4|30| |X| |
| Add R2R jobs|1|80| |X| |
| Add Perf Jobs|4|30| |X| |
| Add SuperPMI jobs|3|50| |X| |
| Run various test legs at job queue time|1|80|X| |X|
| Validate ~~mission control~~AzDO reporting for xunit results with different test legs|0|-|X| |X|
| Set up a way to trigger certain test legs from github PRs|0|-|X| |X|
|**Total**    |**14**                 | | | | |

### Using shared toolset (Arcade SDK)
CoreCLR made a conscious decision to deprioritize the Arcade transition in favor of getting the Official
Build and CI working in the new system ASAP.  Keeping the Product Construction effort unblocked is our
top priority.  We have accrued a lot of debt because of this and will need to clean it up at the same
time we are bringing the rest of the test jobs over.

|Task         |Estimate remaining (days)|Estimate confidence (%)  |Priority 1 |Priority 2| Done |
|-------------|-------------------|---------------------|---|---|---|
| Conform to their directory structure for build output (not sure we should do this)|8|20| |X| |
| Factor our build to go through the arcade entry point project (not sure we should do this) |8|20| |X| |
| Use arcade conventions for package directories and MSBuild imports|2|40| |X| |
| Migrate our tests to SDK-style projects|6|30| |X| |
| Remove config.json and run.exe from our build scripts (easiest once buildpipeline no longer uses them)|4|40| |X| |
|**Total**    |**28**                 | | | | |

## Delivery dates
|Priority |Expected Delivery|
|---|---|
|1| 12/14/2018 |
|2| 03/15/2019|


<!-- Begin Generated Content: Doc Feedback -->
<sub>Was this helpful? [![Yes](https://helix.dot.net/f/ip/5?p=Documentation%5CMigrationPlan%5CCoreClrPlan.md)](https://helix.dot.net/f/p/5?p=Documentation%5CMigrationPlan%5CCoreClrPlan.md) [![No](https://helix.dot.net/f/in)](https://helix.dot.net/f/n/5?p=Documentation%5CMigrationPlan%5CCoreClrPlan.md)</sub>
<!-- End Generated Content-->
