# 0022 User Specified Test Adapter Lookup

## Summary
This note outlines the proposed changes for lookup and initialization of the test adapters, based on User input.

### Current Adapter Lookup

In the current model, when ever a Run/Discovery call is made from IDE to TestPlatform, the test adapter initialization happens in multiple phases, listed below

* InitializeExtensions(): In this call, the IDE passes list of all adapters it finds in current session, these include adapters referenced via nuget in entire solution, & adpaters acquired via Vsix(e.g. GoogleTestAdapter, BoostAdapters, etc..)
* TestAdapterPath: After InitializeExtensions call, TestPlatform then looks into adapters present in TestAdapterPath mentioned in RunSettings, & adds adapters to existing list.
* TestPlatform then adds default Adapters from **Extensions** directory to current list.
* Finally platform looks in adapters present in test source directory, & adds them to adapter list.

Once adapters are gathered from all the possible locations, this list is passed to testhost process, which then loads all these adapters, & passes test source(s) to all the adapters.

Following the above adapter look up logic, we pass down test sources to adapters which have no business running them, for e.g. passing a managed test source to a native adapter(GoogleTestAdapter, BoostAdapters, etc.), & vice-versa.

## Motivation

Select the correct adapter for the run. Most managed adapters are available in the test source directory, hence can be picked up from later for the test run.

In .NetCore scenario, we only pick adapters from test Source location, this helps
* Testplatform, as it doesn't need to probe various locations for adapters.
* Test Framework/Adapter writers, they know where to drop adapters so they are consumed by platform.
* Users as they don't have to specify any additional location for adapters, also they know exactly what adapter is used for the run.

We want to move to the same model for FullCLR projects as well.


## Principles
1. The adapter referenced in the project should be used for test execution
2. Performance should not degrade.
3. Adapter lookup logic is consistent across runs.
4. Adapter lookup logic should be clear and consistent for both IDE/Editor and CLI runs.


## RoadMap

1. Introduce an option(**UseSpecifedAdapterLocations**) to pick adapter from test source location & TestAdapterPath, this option would be present under Tool->Option->Test.
2. Deprecate **Extensions** directory: From VS 15.8 platform will show a warning message to users in both CLI/IDE, for whom adapters from Extensions directory were used to discovery/run tests. We will stop picking adapters from Extensions directory from next Major VS release.

### Details for lookup

* Option "UseSpecifedAdapterLocations" is checked: Adapters only from source, & TestAdapterPaths would be picked.
* Option "UseSpecifedAdapterLocations" is unchecked: Adapters will be picked from Vsix & Extensions directory as well.

### Perf Improvements
Restricting adapters via above logic also results in perf improvements, as it filters out unnecessary adapters which should be not used for a given test source, primarily for a managed test source, it filters our native adapters, as they are acquired only via Vsix, or from default **Extensions** folder.

The improvement is however seen when a few tests are run, for e.g. when running two tests via RunSelected options from IDE, we see the time reduced from **0:00:01.4782809** to **0:00:01.1312048** in IDE Test Output pane

## Implementation Details:
Option "UseSpecifedAdapterLocations" is enabled:

1. IDE would send “InitializeExtensions” call with empty list: This would allow platform to clear the existing cache it would have created if the option was not enabled.
2. While sending a discovery/run request, IDE should set [SkipDefaultAdapters](https://github.com/Microsoft/vstest/blob/e48fde2ccd5c029ffe346fcf20533a556d6f2583/src/Microsoft.TestPlatform.ObjectModel/Client/TestPlatformOptions.cs#L42) to true while sending this request, this specifies to platform to not use adapters from **Extensions** folder.

Option "UseSpecifedAdapterLocations" is disabled:
1. IDE will send “InitializeExtensions” call with list of adapters acquired from Vsix locations.
2. IDE will send discovery/run request with [SkipDefaultAdapters](https://github.com/Microsoft/vstest/blob/e48fde2ccd5c029ffe346fcf20533a556d6f2583/src/Microsoft.TestPlatform.ObjectModel/Client/TestPlatformOptions.cs#L42) as false.
