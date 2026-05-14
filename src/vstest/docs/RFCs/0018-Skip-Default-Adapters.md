# 0018 Skip Default Adapters

## Summary
This note outlines the proposed changes for skipping default adapters so that they don't take part in test discovery and execution.

## Motivation
In addition to test adapters provided via TestAdaptersPaths, test platform loads default adapters present in Extensions folder. This folder is present alongside vstest.console.exe. Even for test sources which can't be discovered/executed by default adapters, we pass those test sources to default adapters. This causes following problems:
1. Adapters initialization cost increases.
2. Discovery/Execution time taken by adapters increases as test source is passed to default adapters as well for discovery/execution.

Example: Default adapters can't run/discover XUnit tests, still testplatform initializes default adapters and pass XUnit test assemblies to them to discover/execute.

Proposed changes are focused on improving performance of test discovery/execution by allowing option to skip initialization of default adapters.

## Proposed Changes
1. Translation layer clients can skip default adapters by setting TestPlatformOptions.SkipDefaultAdapters as true.
2. TestRequestManager passes SkipDefaultAdapters flag to TestPlatform via CreateDiscoveryRequest/CreateTestRunRequest.
3. TestPlatform passes SkipDefaultAdapters flag to ProxyDiscoveryManager/ProxyExecutionManager via initialize method.
4. ProxyDiscoveryManager/ProxyExecutionManager stores this value and uses it to skip default adapters to be passed to test host whenever test host extensions are initialized.

## API Changes
1. Adding of field SkipDefaultAdapters in TestPlatformOptions.
2. TestPlatform's CreateDiscoveryRequest/CreateTestRunRequest will accept TestPlatformOptions as additional argument.
3. **Making TestPlatform internal as it is not meant to be exposed. TestPlatform supports entry point only via command line and translation layer.**
4. IProxyDiscoveryManager and IProxyExecutionManager's Initialize method will accept SkipDefaultAdapters as additional argument.

## Performance Improvement Analysis
Following are the performance improvements which are achieved when default adapters are skipped.

**Sample**: XUnit test project

| Discovery Request      | Before Changes | After Changes | Improvement |
|------------------------|----------------|---------------|-------------|
| Adapter Initialization | 620 ms         | 131 ms        | 489 ms      |
| Test Discovery         | 892 ms         | 678 ms        | 214 ms      |

| Execution Request      | Before Changes | After Changes | Improvement |
|------------------------|----------------|---------------|-------------|
| Adapter Initialization | 608 ms         | 126 ms        | 482 ms      |

Test Execution is not affected by these changes as while test execution, test source is passed to the matching adapter executor URI only.

## Alternatives
**Alternative 1**: Passing SkipDefaultAdapters via runsettings
Issues:
SkipDefaultAdapters is something which is neither understood by test user nor by test adapter. So, it should not be part of runsettings.

**Alternative 2**: Instead of passing TestPlatformOptions to CreateDiscoveryRequest/CreateTestRunRequest, pass only SkipDefaultAdapters as we don't need TestPlatformOptions in TestPlatform.cs for anything other than SkipDefaultAdapters.
Issue:
If in future similar field needs to be added, we need to change API of TestPlatform again to add the field.
