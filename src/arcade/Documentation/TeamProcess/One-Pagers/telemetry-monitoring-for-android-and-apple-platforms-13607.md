# Monitoring for Android and Apple platforms

As part of [#10420](https://github.com/dotnet/core-eng/issues/10420), we started supporting Android and Apple mobile platforms in Helix.
These new on-prem devices are different in nature to the hardware we were managing so far.
The devices (and simulators) can "break" in ways we are unable to detect at the moment and thus new approach to monitoring these devices is needed

**The goal of this design document** is to capture our plan for
- why, how, when and what telemetry to collect,
- what alerting to create based on this telemetry.

## Motivation

The main goal is to learn fast about mobile devices in some failed state such as inable to install/run an application.
Currently, we are unable to achieve this because we don't have enough data around the singular operations (XHarness commands) we perform with the devices.
Furthermore, we don't have data about the big picture.

To mark a mobile device as faulty, we need more than a single failing operation as it is hard to decide based on one data point.
The reason is that for some groups of problems we need a margin for cases where an operation fails but it's a user error that we just couldn't distinguish from an infra issue.
For example, if we spot a single installation failure the cause can be an app that was badly built and fails to install which is an user error.
However, if we see a high percentage of failures over time, probably the device is the faulty part (memory is full, emulator is hanging..).
In the large amount of data we will have (tens of thousands of operations per day), we will be able to account for these and alert more reliably.
This is the reason we we need to collect and store data about a series of operations and alert based on the whole.

## Stakeholders

- **.NET Engineering Services** - team servicing the Helix devices
- **DDFUN** - team managing the hardware - in future, they might be involved in the alert response process

## Scope

The mobile platforms that are in scope of this work are:
- iOS devices (iPhones)
- AppleTV devices
- Android devices
- Ubuntu Android queues with Android emulators
- OSX queues with Xcode and Simulators

The overview of the queues and Mobile devices in Helix can be found [here (Mobile devices for .NET testing)](https://dev.azure.com/dnceng/internal/_wiki/wikis/DNCEng%20Services%20Wiki/107/Mobile-devices-for-.NET-testing).

## Alerts

The main goal is to learn about device failures such as inability find the device or to install/run an application there.
This will be achieved by creating Grafana alerts.
There are several alerts we want to fire that we know about already.
These events are usually signaled by XHarness CLI exiting with a specific return code.
Most of these events are already captured in the [wrapper scripts in Helix SDK](https://github.com/dotnet/arcade/blob/e6abc186425e675e0f201acc1c33e183a0e51c86/src/Microsoft.DotNet.Helix/Sdk/tools/xharness-runner/xharness-runner.apple.sh#L154-L181) and currently we only perform work item retry and reboot the machine, hoping the work item will be dealt with on a healthy machine.
The problem with this approach is that broken machines stay broken silently.
The monitoring will help us learn about these cases.

Examples of these event are:
- HW device is not found on a queue with HW devices – this usually means the device is stuck after a reboot or similar.
- Apple Simulator is not found while we are not requesting a specific runtime version (we target `ios-simulator-64` for example, which should always be available, whereas `ios-simulator-64_14.2` might not).
- Android emulator is not available on a queue where we expect it to be running.
- Long taking operations - we see this for iOS simulators which leak memory/CPU which manifests as the installation of applications slowing down.
- Low SD card / internal storage disk space - root cause of installation failures in Android.

### Implementation plan for alerts

**Stage 1**
At first, we will focus on detecting fatal errors when they happen such as "device is not visible".
These are usually easy to detect since we will get almost a 100% failure rate once a device goes into this state.
We can detect this over a short period of time and alert.
The customer won’t be impacted as much thanks to the retry/reboot we already do so this is transparently dealt with.
It is important for us to know as these things now happen silently and we never learn about them.
We will be able to take the device offline and investigate it.

**Stage 2**
As we gain more experience and investigate devices in broken states, we will learn to collect new metrics that can be used to predict breaks before they happen and avoid customer impact.
Examples can be:
- Long taking operations - we see this for iOS simulators which leak memory/CPU which manifests as the installation of applications slowing down.
- Low SD card / internal storage disk space - root cause of installation failures in Android.

**Stage 3**
For some events, we already self-heal by doing a retry/reboot and thus we might not need an alert.
We can, however, be notified about this, so that we know when and how often these things happen.
This stage can introduce additional events as described above.
These are rather "nice to have" so we can consider the business need later and potentially omit this stage.

## Challenges

Every mobile device is connected via a USB cable to a host machine which acts as a Helix agent - Helix client runs on the host machine only, never the device.
In contrary to the other platforms, the host machines are not interesting to us very much, we care about the devices.
Since the Helix client always runs on host machines and the devices are not visible from the Helix perspective, the heartbeat mechanism won't detect bad states of these devices.

Furthermore, it is quite hard to determine the health of these devices for several reasons:
- We need specialized tooling to find the devices (XHarness, ADB, mlaunch).
- This tooling needs to be updated frequently (to react to new versions of iOS, Android...).
- The operations that gather device information can take even longer time than the over all runtime of a large portion of work items we run today that finish in several seconds (e.g., iOS Simulator BCL tests).
- It is difficult to tell a bad state as some of the problems do not manifest until we try to perform certain actions with the device, e.g., a device is locked but installation succeeds, only trying to run the app will fail.

We explored several approaches but they had several drawbacks which would make them hard to implement/maintain:

- Adding a monitoring command to the XHarness CLI and calling it at the beginning and at the end of the job.
    - It would add sizable time to all the work items.
    - We don't know which device/emulator will be used until we read the application metadata and only after then we look for the appropriate device.
    - We can only detect some problems when we try to install/run applications on the devices which wouldn't work for this command neither. We care about results of some of the operations more than the state of the device because we just don't know the state is corrupted until some operations fail.
    - The user can call many XHarness commands as part of a single Helix work item - they can install the app once and then run it several times over with different parameters. We wouldn't know when the state got corrupted.

- Adding a monitoring module to the machines that would scan the environment periodically and report back.
    - Shares some of the drawbacks from above.
    - Would be hard to maintain/update together with all of the dependencies.
    - Might interfere with currently running Helix work item.

## Architecture & design

> **TLDR:** XHarness will collect diagnostics data during execution and Helix SDK will send them to an Application Insights account as custom metrics so that Grafana can alert based on it.

### Affected components:
- **XHarness CLI** - tool used to run the Android/Apple tests ([`dotnet/xharness`](https://github.com/dotnet/xharness))
- **Helix SDK** - MSBuild targets that enable creation of XHarness Helix workloads ([`dotnet/arcade`](https://github.com/dotnet/arcade/blob/main/src/Microsoft.DotNet.Helix/Sdk/tools/xharness-runner/Readme.md))
- **XHarness workload scripts** - wrapper scripts that execute the commands supplied by the user ([`dotnet/arcade`](https://github.com/dotnet/arcade/blob/main/src/Microsoft.DotNet.Helix/Sdk/tools/xharness-runner/))
- **Grafana dashboards** - definitions of monitoring charts and alerts ([`dotnet/arcade-services`](https://github.com/dotnet/arcade-services/tree/main/src/Monitoring))

### Data collection process

Based on the requirements and the challenges we face we can collect the data in the following way:

1. We will add a diagnostics mode for every command of the XHarness CLI. There will be a new option and XHarness will also accept this an environment variable.
2. As the command runs in this mode, it will collect diagnostics data (i.e., create a diagnostic file) with information about which operation was running and how it ended. It will note down which device was used and other useful statistics.
3. Since we control the execution of user's commands via the Helix SDK wrapper scripts, we can set the environmental variable at the beginning to execute always in the diagnostic mode. We can then collect the results at the end.
4. We will send the diagnostics to Application Insights (later to Kusto) at the end of the Helix work item. We only need to make sure we have enough time (few seconds at most) in the Helix work item to send the data after user's commands are finished. However, since we control the execution of those, we can time-constrain them and leave a buffer for ourselves.

### Data storage

To store the data, we will use an already existing Application Insights account [helix-client](https://ms.portal.azure.com/#@microsoft.onmicrosoft.com/resource/subscriptions/68672ab8-de0c-40f1-8d1b-ffb20bd62c0f/resourceGroups/helixinfrarg/providers/microsoft.insights/components/helix-client-prod/overview) and upload the diagnostics data there as a [custom metric](https://docs.microsoft.com/en-us/azure/azure-monitor/essentials/metrics-custom-overview).
To send the data, we will need the API key and then send a request to Azure.
We have code and infrastructure for this in place already [in the Helix client (appinsights.py)](https://dev.azure.com/dnceng/internal/_git/dotnet-helix-machines?path=%2Fresources%2Fhelix-scripts%2Fhelix%2Fappinsights.py).
No SDL / threat model changes would be necessary for this.

The metric will have a static name common for all events generated for these purposes such as `XHarnessOperation`.
The value will be the exit code of the operation and the rest of the properties will go to **customDimensions** (an assorted dictionary).
We can then query the data points like in the following example:

```sql
customMetric
| where name == "XHarnessOperation" and value != 0
| where customDimensions.platform == "android"
| where customDimensions.command == "install"
```

In a later stage, we will migrate to Kusto and send the telemetry together with Helix job events that we are sending already.
Currently, we are storing the events in the `Metrics` table which also has its own `CustomDimensions` column.
We will decide at the time of implementation whether we will create our own table or re-use this existing one, it is an implementation detail.
The queries for AI and Kusto are very similar, using the same language so it will be quite effortless to migrate the charts and alerts.

### Moving to Kusto

In the design stage, we have come to a conclusion that a follow-up effort will be to generate some more rigorous reports about the mobile devices and their reliability.
For this, Kusto is more convenient place to store the data because of reliability and linkability with other job data.
The reports are a new requirement that wasn't originally part of the epic and since we need to still support .NET 6, we are on an aggressive schedule.
Because almost no additional work is introduced when switching to Kusto and also for reasons stated below, we we will use Application Insights to store the data at first and then move to Kusto in a second iteration:

- Kusto changes are more complicated as the data travels to Kusto through EventHub and an SQL table (handled by the Helix service). This means they require Helix service rollout and then Helix machines rollout (for Helix client) so it will take time to get them out.
- Application Insights only require a PR in Arcade and thus are a better tool for iterating in case we don't get everything right the first time.
- With AI, we can have the data available immediately and this iterate faster with graphs/alerts to see if we missed something, no release cycle needed.
- We can have data while we work on changes needed for Kusto already and service the platforms to better accomodate the .NET 6 schedule.
- No extra work needs to be done when choosing AI, everything is in place already.
- No additional work is introduced when switching to Kusto, only Kusto specific work that needs to be done anyway, we will just use different Python class to send the same data to a different place (from Helix perspective).

### Collected data

It is important that XHarness CLI stays Helix agnostic as it is used by other teams in scenarios not related to Helix at all.
This means that XHarness CLI will only collect data related to the operations it performs on the device.
We will then enrich the diagnostics XHarness data with Helix environment specifics that will help us identify problematic machines.

**XHarness CLI data:**
- Platform (android/apple)
- Executed command (install/test/run...)
- Exit code
- Duration of the command
- Emulator/Simulator ID (can also be architecture + OS/API version string)

As we continue to identify new properties of the devices that can help prevent/detect problems, we can extend these data points or add new metrics.
We can also only choose to report some in case of unsuccessful operations only.

**Helix SDK data:**
- Machine name
- Queue name

There are other data points we might choose to collect (XHarness CLI version, Helix work item friendly name…) but we are constrained by some limits set for custom metrics.

**Example data**

Example data for 1 run that uses the granual XHarness operations (we also have `test` command that performs all `install`, `just-test` and `uninstall`) can look something like this:

| Metric                    | Value            | Cloud role                 | Cloud instance | CD.platform | CD.command | CD.target       |
|---------------------------|------------------|----------------------------|----------------|-------------|------------|-----------------|
| XHarnessOperation         | 0 (SUCCESS)      | osx.1015.amd64.iphone.open | DNCENGMAC049   | apple       | install    | ios-device 14.4 |
| XHarnessOperationDuration | 40 (sec)         | osx.1015.amd64.iphone.open | DNCENGMAC049   | apple       | install    | ios-device 14.4 |
| XHarnessOperation         | 1 (TESTS FAILED) | osx.1015.amd64.iphone.open | DNCENGMAC049   | apple       | just-test  | ios-device 14.4 |
| XHarnessOperationDuration | 150 (sec)        | osx.1015.amd64.iphone.open | DNCENGMAC049   | apple       | just-test  | ios-device 14.4 |
| XHarnessOperation         | 0 (SUCCESS)      | osx.1015.amd64.iphone.open | DNCENGMAC049   | apple       | uninstall  | ios-device 14.4 |
| XHarnessOperationDuration | 4 (sec)          | osx.1015.amd64.iphone.open | DNCENGMAC049   | apple       | uninstall  | ios-device 14.4 |

*\* CD means Custom Dimensions, just the object is expanded into separate columns*

The `Value` column for `XHarnessOperation` is the exit code of XHarness and number of seconds for `XHarnessOperationDuration`.
From these data points, we will be able to gather the success rate of each operation per every machine.
We can be strict for alerting based on some exit codes and more benevolent in other:
- `DEVICE_NOT_FOUND` results on machines in a queue with devices (not emulators) is probably an error we want to know about fast as it means the device is turned off.
- `INSTALLATION_FAILED` for Android devices can be the applicaiton's fault (bad app) but can also mean low storage space, so a we can have lower expectations but still want to be alerted about some levels of dropped SLA on a device.

### Metric limits and quotas

There are some [limits set for Application Insights custom metrics](https://docs.microsoft.com/en-us/azure/azure-monitor/essentials/metrics-custom-overview#quotas-and-limits):

- Maximum of 10 custom dimensions per metric
- Limit of 50,000 total variations of values stored in the custom dimensions

The second limit means that we need to consider the cardinality of each custom dimension and multiply them. This number must not be larger than 50,000.
The cardinalities of proposed dimensions are:

| Dimension        | Values                                                   | # of values |
|------------------|----------------------------------------------------------|:-----------:|
| Platform         | android/apple                                            |      2      |
| Executed command | test, run, just-test, just-run, install, uninstall       |      6      |
| Test target      | Android – 10 API versions, 32/64 bit – roughly 20 values |             |
|                  | Apple – iOS/tvOS, OS version – roughly 30 values         |     50      |

We don’t need any other dimensions because:
- Exit code will be the value of the metric
- Machine name and queue name are stored in the metric’s `cloud_RoleName` and `cloud_RoleInstance` fields
- Simulator vs device can be deduced from queue name
- Device ID can be deduced from machine name

Altogether, we’re looking at 600 variations which gives us space to grow still.
There are also other fields in the metrics we could probably use such as `client_OS`.
Nonetheless, if we choose to collect stats such as free RAM, we should consider adding it as a new metric and keeping its custom dimensions same.

### Data volume & estimated costs

The [metrics pricing](https://azure.microsoft.com/en-us/pricing/details/monitor/) doesn't include custom dimensions yet since they are in preview still.
The metrics themselves cost $0.258/MB with first 150 MB per month free.
Each metric is considered 8B so we have 18.75 million metric data points for free.

Currently, the AI account receives 1,500,000 data points per day which amounts to 2-3 GB with 100 GB being the daily volume limit.
The current monthly cost of the account is 170 USD.

The new metrics we want to collect will be generated by around 70,000 XHarness operations per day which amounts to 4-5 USD per month based on these calculations (70000 data points * 30 days * 8 bytes / 0.258 USD per MB), possibly less if we hit the higher volume band which has lower rates.

## Feature rollout plan

This feature will be on-demand and users of Helix SDK will be able to turn it for their Helix jobs on by setting an MSBuild property.
We can experiment this way by spawning single tests jobs from the local environment.
After the feature is code-complete:

- We will start by turning the feature on in **dotnet/xharness** where we have few runs per day (lower tens of metric data points).
- We will continue by rolling it out gradually for the BCL tests in **dotnet/runtime**. We can do it test suite by test suite, queue by queue, platform by platform. The property can be conditioned in MSBuild easily as part of a **dotnet/runtime** pull request. In case of problems (we start hitting some limits), we can dial it back by another **dotnet/runtime** PR.
- We will finalize the rollout by adding it to the runtime tests in **dotnet/runtime** (which requires them to start using the Helix SDK properly – pending work on their part).
- We will make the feature be opt-out and on by default in the Helix SDK.

To make a change to the system, we need to change XHarness / Helix SDK and let the dependency updates flow to **dotnet/runtime**.

## Risk

- What are the unknowns?  
    *We only estimated the volume and variety of the data stored in Application Insights based on already existing data. We might need to create a separate account in case we need more custom dimensions. We however have control over this because we can roll out platform by platform.*
- Are there any PoCs required to be built for this epic?  
    *No, this work will be easily testable before put into production.*
- What dependencies will this epic have? Are the dependencies currently in a state that the functionality in the epic can consume them now, or will they need to be updated?  
    *The dependencies (XHarness, Helix SDK, Grafana) are in place and will only be extended.*
- Will the new implementation of any existing functionality cause breaking changes for existing consumers?  
    *No changes, just new functionality which is internal to our team only so no consumer impact.*
- Is there a goal to have this work completed by, and what is the risk of not hitting that date?  
    *There is no hard deadline. Having this delivered will have impact on the smoothness of work on .NET 6 for the mono teams.*

## Serviceability

- How will the components that make up this epic be tested?  
    *We have unit tests and E2E tests for both places with code changes (XHarness CLI, Helix SDK). We can verify that the data is flowing to Grafana before merging each change as we have E2E tests for XHarness and we can query by machine. We are unfortunately unable to have some sort of E2E test that makes sure Helix client changes don't break the Helix SDK integration. We would need to have the newest Arcade and Helix SDK in Helix Machines. This seems very complicated to do and my be a bad ROI.*
- How will we have confidence in the deployments/shipping of the components of this epic?  
    *We can test everything in PRs and roll out gradually before merging.*
- Identifying secrets (e.g. PATs, certificates, et cetera) that will be used (new ones to be created; existing ones to be used).  
    *Application Insights API key is already in place, also used on public queues so considered not a secret*
- Does this change any existing SDL threat or data privacy models?  
    *No, we already send data from the Helix client to the same AI account*
- Steps for setting up repro/test/dev environments?  
    *XHarness CLI can be run locally as is. Helix SDK integration tests can be triggered manually from `dotnet/xharness` and `dotnet/arcade` by devs.*
    *We will have a staging Application Insights account.*

## Rollout and Deployment
- How will we roll this out safely into production?  
    *We will feature switch this via an MSBuild property in client repos (dotnet/xharness with low traffic to begin with).*
- How often and with what means we will deploy this?  
    *Rollout process for all artifacts is already in place.*
- What needs to be deployed and where?  
    *XHarness CLI diagnostics functionality and Helix SDK features will be delivered via Maestro updates.*
    *Alerts and charts will be deployed to Grafana using the regular `arcade-services` rollout.*
- What are the risks when doing it?  
    *No risks as we will feature switch this inside of a PR.*
- What are the dependencies when rolling out?  
    *None*

## FR Hand off

Documentation and FR hand off are subsequent goals of the parent epic and will be handled separately:
- All new alerting should be actionable with links to documentation.
- Team will be educated on how to service the new platforms.


<!-- Begin Generated Content: Doc Feedback -->
<sub>Was this helpful? [![Yes](https://helix.dot.net/f/ip/5?p=Documentation%5CTeamProcess%5COne-Pagers%5Ctelemetry-monitoring-for-android-and-apple-platforms-13607.md)](https://helix.dot.net/f/p/5?p=Documentation%5CTeamProcess%5COne-Pagers%5Ctelemetry-monitoring-for-android-and-apple-platforms-13607.md) [![No](https://helix.dot.net/f/in)](https://helix.dot.net/f/n/5?p=Documentation%5CTeamProcess%5COne-Pagers%5Ctelemetry-monitoring-for-android-and-apple-platforms-13607.md)</sub>
<!-- End Generated Content-->
