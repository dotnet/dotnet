# Microsoft.DotNet.Helix.Sdk

This Package provides Helix Job-sending functionality from an MSBuild project file.

## Examples
Each of the following examples require dotnet-cli >= 3.1.x, and need the following files in a directory at or above the example project's directory.
#### NuGet.config
```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <packageSources>
    <clear />
    <add key="dotnet-eng" value="https://pkgs.dev.azure.com/dnceng/public/_packaging/dotnet-eng/nuget/v3/index.json" />
  </packageSources>
</configuration>
```
#### global.json
```json
{
  "msbuild-sdks": {
    "Microsoft.DotNet.Helix.Sdk": "<version of helix sdk package from package feed>"
  }
}
```

Versions of the package can be found by browsing the feed at https://dev.azure.com/dnceng/public/_packaging?_a=feed&feed=dotnet-eng

### Developing Helix SDK

The examples can all be run with `dotnet msbuild` and will require an environment variable or MSBuildProperty `HelixAccessToken` set if a queue with a value of IsInternalOnly=true (usually any not ending in '.Open') is selected for `HelixTargetQueues`. You will also need to set the following environment variables before building:

```
BUILD_SOURCEBRANCH
BUILD_REPOSITORY_NAME
SYSTEM_TEAMPROJECT
BUILD_REASON
```

Also, make sure your helix project doesn't have `EnableAzurePipelinesReporter` set, or sets it to false, or building locally will fail with an error that looks like `SYSTEM_ACCESSTOKEN is not set`.

When running Helix tests in Azure DevOps pipelines where results need to be published back to Azure DevOps, you'll need to set the environment variable:
```yaml
env:
  SYSTEM_ACCESSTOKEN: $(System.AccessToken) # We need to set this env var to publish helix results to Azure DevOps
```

### Helix Job Monitor for Azure DevOps

If you want to decouple Helix test execution from the build agents that submit the work, use the Helix Job Monitor.

The job monitor is a lightweight dedicated pipeline job that:

- polls Azure DevOps for pipeline state,
- polls Helix for jobs associated with the current build,
- downloads test result artifacts from completed Helix jobs,
- publishes results to Azure DevOps incrementally,
- returns a final green or red status once all non-monitor jobs and Helix jobs have completed.

This allows the original build jobs to stop waiting on Helix execution while still preserving test visibility and pass/fail behavior in the pipeline.

![Helix Job Monitor](../../../Documentation/HelixJobMonitor.png)

The job is added with the template at [/eng/common/core-templates/job/helix-job-monitor.yml](/eng/common/core-templates/job/helix-job-monitor.yml).

Example:

```yaml
jobs:
- template: /eng/common/core-templates/job/helix-job-monitor.yml@self
  parameters:
    pollingIntervalSeconds: 30
    timeoutInMinutes: 360
```

Useful parameters:

- `helixBaseUri`: base URI for the Helix service. Defaults to `https://helix.dot.net/`.
- `helixAccessToken`: optional token for authenticated Helix access on internal builds.
- `pollingIntervalSeconds`: how often the job monitor checks for new completed jobs.
- `timeoutInMinutes`: overall timeout for the job monitor.

Behavior notes:

- The reporter uses its own `SYSTEM_ACCESSTOKEN`, so it does not depend on the shorter-lived token from the job that originally submitted the Helix work.
- If parseable xUnit, JUnit, or TRX result files are available, those are uploaded.
- If no result files are found for a work item, no test results are uploaded for that work item; Helix work-item failures still affect the monitor job's final pass/fail status.
- The reporter is safe to rerun because it checks for already-completed test runs and only processes new results.

#### What changes for pipeline users when the monitor is on

When `EnableHelixJobMonitor` is set, the Helix-submitting jobs and the Helix Job Monitor job play
different roles than in the legacy ("inline") flow. The user-visible UX differs in a few important
ways:

- **The submitter job no longer waits for Helix.** The job that runs `SendHelixJob` now exits as
  soon as the Helix jobs have been queued. It will go green in Azure DevOps long before the tests
  finish running. **Don't interpret a green submitter job as "tests passed"** — it only means the
  work was successfully queued.
- **The monitor job owns pass/fail for tests.** The pipeline's overall test pass/fail status comes
  from the Helix Job Monitor job. If the monitor is red, the tests (or the Helix work that runs
  them) failed. If the monitor is green, all Helix work items passed (after any retries — see
  below).
- **Test results appear incrementally.** Results are uploaded to Azure DevOps as each Helix job
  completes, not in a single batch at the end. The "Tests" tab will start populating while the
  monitor is still running.
- **Build agents are freed up sooner.** Because the submitter job exits early, build pool capacity
  is no longer held hostage by long-running Helix queues. The monitor job runs on a lightweight
  agent and is the only thing waiting on Helix.
- **One monitor job per stage.** The monitor is scoped to a single Azure DevOps stage by default.
  In a multi-stage pipeline you add the `helix-job-monitor.yml` template once per stage that
  submits Helix work.

#### How re-runs work with the monitor

The monitor performs **automatic, in-pipeline retries of failed Helix work items** at the start of
each monitor invocation. This is in addition to (and operates very differently from) the existing
[test retry](#test-retry) feature, which retries individual tests within a single work item.

What this means in practice:

- **One-shot retry on entry.** When the monitor starts, it takes a snapshot of the Helix jobs for
  the build and resubmits the failed work items from each completed job's latest incarnation.
  Passing work items are not resubmitted; only the failed ones are.
- **Re-running the monitor job re-runs failed Helix work.** If you re-run the **monitor job** in
  Azure DevOps (e.g. via "Rerun failed jobs" on the build), it picks up where it left off:
  - Passing work items from previous attempts are preserved.
  - Failed work items are resubmitted to Helix. These are not built again; the same payloads are re-queued for execution.
  - The monitor then waits for the new work items to complete, uploads their results, and folds
    them into the pipeline test pass/fail status.
  - A newer passing incarnation of a work item supersedes an older failed one, so retries
    naturally converge — each rerun should resubmit fewer work items than the previous one.
- **Test result uploads are deduplicated.** Each Helix job's test results are uploaded at most
  once per build, even across monitor reruns. The monitor identifies already-uploaded jobs from
  Azure DevOps test-run tags, so it's always safe to re-run the monitor job.

💡 The recommended workflow when something fails is therefore:

1. Look at the monitor job's log to see which Helix work items failed and follow the linked
   Helix console output.
2. If the failure looks transient (flaky infrastructure, network blip, etc.), **re-run the monitor job**.
   The monitor will resubmit only the failed work items.
3. If the failure is a real product/test bug, fix it and push a new commit — that triggers a new
   build with a fresh submitter + monitor pair.

#### Adding the `microsoft.dotnet.helix.jobmonitor` package

The Helix Job Monitor ships as a .NET tool in the `microsoft.dotnet.helix.jobmonitor` package, which
must be added as a dependency to the repo and registered as a local tool.

1. Look up the latest version of the package on the `.NET Eng - Latest` channel:

    ```sh
    darc get-asset --name microsoft.dotnet.helix.jobmonitor --channel '.NET Eng - Latest' --latest
    ```

2. Use the version, commit, and repo URI returned above to add the dependency via `darc`:

    ```sh
    darc add-dependency \
      --name microsoft.dotnet.helix.jobmonitor \
      --type toolset \
      --version <version-from-get-asset> \
      --commit <commit-from-get-asset> \
      --repo <repo-uri-from-get-asset>
    ```

3. Add a matching entry under `tools` in `.config/dotnet-tools.json` so the tool is restored locally:

    ```json
    "microsoft.dotnet.helix.jobmonitor": {
      "version": "11.0.0-beta.26255.6",
      "commands": [
        "dotnet-helix-job-monitor"
      ]
    }
    ```

    Use the same version that was added via `darc add-dependency`.

#### Opting in from a Helix project

Pair the monitor job with the `EnableHelixJobMonitor` MSBuild property in the Helix `.proj` that
calls `SendHelixJob`:

```xml
<PropertyGroup>
  <EnableHelixJobMonitor>true</EnableHelixJobMonitor>
</PropertyGroup>
```

When set, the Helix SDK will submit Helix jobs and exit immediately without waiting for completion.
The Helix Job Monitor will be responsible for tracking the jobs to completion and publishing results to Azure DevOps, so no other changes are needed to the Helix project file itself.
You must however add the `helix-job-monitor.yml` template to your pipeline (see the example above) so the
results are still published to Azure DevOps.

Furthermore, when you need to make changes to Helix SDK, there's a way to run it locally with ease to test your changes in a tighter dev loop than having to have to wait for the full PR build.

The repository contains E2E tests that utilize the Helix SDK to send test Helix jobs.
In order to run them, one has to publish the SDK locally so that the unit tests can grab the re-built DLLs.

#### Detailed steps:
1. Make your changes
2. Build the product
    ```sh
    # Linux/MacOS
    ./build.sh
    # Windows
    .\Build.cmd
    ```
3. Publish Arcade SDK and Helix SDK
    ```sh
    dotnet publish -f <tfm> src/Microsoft.DotNet.Arcade.Sdk/Microsoft.DotNet.Arcade.Sdk.csproj
    dotnet publish -f <tfm> src/Microsoft.DotNet.Helix/Sdk/Microsoft.DotNet.Helix.Sdk.csproj
    ```
4. Pick one of the test `.proj` files, set some env variables and build it  
    Bash
    ```sh
    export BUILD_REASON=pr
    export BUILD_REPOSITORY_NAME=arcade
    export BUILD_SOURCEBRANCH=master
    export SYSTEM_TEAMPROJECT=dnceng
    export SYSTEM_ACCESSTOKEN=''

    eng/common/build.sh -test -projects tests/XHarness.Apple.DeviceTests.proj /v:n /bl:Arcade.binlog
    ```

    PowerShell
    ```ps1
    $Env:BUILD_REASON = "pr"
    $Env:BUILD_REPOSITORY_NAME = "arcade"
    $Env:BUILD_SOURCEBRANCH = "master"
    $Env:SYSTEM_TEAMPROJECT = "dnceng"
    $Env:SYSTEM_ACCESSTOKEN = ""

    .\eng\common\build.ps1 -configuration Debug -restore -test -projects tests\XHarness.Apple.DeviceTests.proj /p:RestoreUsingNugetTargets=false /bl:Arcade.binlog
    ```
5. An MSBuild log file called `Arcade.binlog` will be produced which you can inspect using the [MSBuild Structured Log Viewer](https://msbuildlog.com/). There you can see which props were set with which values, in what order the targets were executed under which conditions and so on.

### Docker Support
Helix machines now have (where available on the machine) the ability to run work items directly inside Docker containers.  This allows work items to use operating systems that only work for Docker scenarios, as well as custom configurations of already-supported operating systems.  
#### Specifying a docker tag:
Supported docker tags include anything publicly available on dockerhub.io, as well as azurecr.io and mcr container registries which have had the appropriate service principal users added or are public. In all cases, use the format:
```
({Optional Queue Alias}){Helix Queue Id}@{DockerTag}
```
As an example, to run a typical Helix work item targeting an Alpine 3.9 docker image on a Ubuntu 16.04 host, the queue Id used would be `(Alpine.39.Amd64)ubuntu.1604.amd64.open@mcr.microsoft.com/dotnet-buildtools/prereqs:alpine-3.9-helix-bfcd90a-20200123191053`.
Anywhere a container registry is left out, dockerhub.io is assumed; generally execution is done only on MCR images controlled by the .NET Team, as this allows fine control over the user inside the container and its permissions.
#### Limitations:
- Windows Docker machines will always be set in Windows container mode, as it is non-trivial to reliably switch between these formats.  In general you should use OSX or Linux machines for your non-Windows Docker needs, and a matching RS* level of Windows for the Server used (i.e. to run Nano RS5, you need to run on Server RS5 currently)
- While work items will execute as usual, many Helix work items assume the existence of python 3.7 on the machine and will fail to do certain parts (such as uploading logs and other artifacts) when run.  The ['python' repo on dockerhub](https://hub.docker.com/_/python) provides Python 3 forms of Alpine, Debian, and Windows Server Core images.  Others will need to install python as part of image generation.
- Not all Helix Queues will have Docker installed (or in some cases Docker may be broken there).  Contact the dnceng team if you feel a particular Helix queue should have Docker installed, but does not; the https://helix.dot.net/api/2019-06-17/info/queues API will always serve as the "source of truth" for which machines have the Docker EE installed at any given time.
- To update or add new Helix Docker images, make a pull request to https://github.com/dotnet/dotnet-buildtools-prereqs-docker; updated images are published to dotnet/versions repo.  Image names checked into your sources are not automatically updated, and there is intentionally no "latest" tag.

### Hello World
This will print out 'Hai Wurld!' in the job console log.
```xml
<Project Sdk="Microsoft.DotNet.Helix.Sdk" DefaultTargets="Test">
  <PropertyGroup>
    <HelixSource>pr/testing/</HelixSource>
    <HelixType>test/stuff</HelixType>
    <HelixBuild>23456.01</HelixBuild>
    <HelixTargetQueues>Windows.10.Amd64.Open</HelixTargetQueues>
  </PropertyGroup>

  <ItemGroup>
    <HelixWorkItem Include="Hello World!">
      <Command>echo 'Hai Wurld!'</Command>
    </HelixWorkItem>
  </ItemGroup>
</Project>
```

### Using a Payload folder
Given a local folder `$(TestFolder)` containing `runtests.cmd`, this will run `runtests.cmd`.
```xml
<Project Sdk="Microsoft.DotNet.Helix.Sdk" DefaultTargets="Test">
  <PropertyGroup>
    <HelixSource>pr/testing</HelixSource>
    <HelixType>test/stuff</HelixType>
    <HelixBuild>23456.01</HelixBuild>
    <HelixTargetQueues>Windows.10.Amd64.Open</HelixTargetQueues>
  </PropertyGroup>

  <ItemGroup>
    <HelixWorkItem Include="Using a Payload">
      <Command>runtests.cmd</Command>
      <PayloadDirectory>$(TestFolder)</PayloadDirectory>
    </HelixWorkItem>
  </ItemGroup>
</Project>
```

### All Possible Options
```xml
<Project Sdk="Microsoft.DotNet.Helix.Sdk" DefaultTargets="Test">
  <PropertyGroup>
    <!-- The 'source' value reported to helix  -->
    <HelixSource>pr/testing/</HelixSource>
    <!-- The 'type' value reported to helix  -->
    <HelixType>test/stuff/</HelixType>
    <!-- The 'build' value reported to helix  -->
    <HelixBuild>23456.01</HelixBuild>

    <!-- The helix queue this job should run on. -->
    <HelixTargetQueue>Windows.10.Amd64.Open</HelixTargetQueue>

    <!-- Whether to fail the build if any Helix queues supplied don't exist.
         If set to false, sending to non-existent Helix Queues will only print a warning. Defaults to true. 
         Only set this to false if losing this coverage when the target queue is deprecated is acceptable.
         For any job waiting on runs, this will still cause failure if all queues do not exist as there must be
         one or more runs started for waiting to not log errors.  Only set if you need it.
    -->
    <FailOnMissingTargetQueue>false</FailOnMissingTargetQueue>

    <!--
      The set of helix queues to send jobs to.
      This property is multiplexed over just like <TargetFrameworks> for C# projects.
      The project is built once per entry in this list with <HelixTargetQueue> set to the current list element value.
      Note that all payloads sent need to be able to run on all variations included.
    -->
    <HelixTargetQueues>Ubuntu.1804.Amd64.Open;Ubuntu.1604.Amd64.Open;(Alpine.39.Amd64)Ubuntu.1804.Amd64.Open@mcr.microsoft.com/dotnet-buildtools/prereqs:alpine-3.9-helix-bfcd90a-20200123191053</HelixTargetQueues>

    <!-- 'true' to download dotnet cli and add it to the path for every workitem. Default 'false' -->
    <IncludeDotNetCli>true</IncludeDotNetCli>
    <!-- 'sdk', 'runtime' or 'aspnetcore-runtime' -->
    <DotNetCliPackageType>sdk</DotNetCliPackageType>
    <!-- 'latest' or a specific version of dotnet cli -->
    <DotNetCliVersion>2.1.403</DotNetCliVersion>
    <!-- 'Current' or 'LTS', determines what channel 'latest' version pulls from -->
    <DotNetCliChannel>Current</DotNetCliChannel>

    <!-- Enable reporting of test results to azure dev ops -->
    <EnableAzurePipelinesReporter>false</EnableAzurePipelinesReporter>
    <!-- 'true' to produce a build error when tests fail. Default 'true' -->
    <FailOnTestFailure>true</FailOnTestFailure>

    <!--
      Commands that are run before each workitem's command
      semicolon-separated; use ';;' to escape a single semicolon
    -->
    <HelixPreCommands>$(HelixPreCommands);echo 'pizza'</HelixPreCommands>

    <!--
      Commands that are run after each workitem's command
      semicolon separated; use ';;' to escape a single semicolon
    -->
    <HelixPostCommands>$(HelixPostCommands);echo 'One Pepperoni Pizza'</HelixPostCommands>
  </PropertyGroup>

  <!--
    Optional additional dotnet runtimes or SDKs for correlation payloads
    PackageType (defaults to runtime)
    Channel (defaults to Current)
  -->
  <ItemGroup>
    <!-- Includes the 6.0.0-preview.4.21178.6 dotnet runtime package version from the Current channel, using the DotNetCliRuntime -->
    <AdditionalDotNetPackage Include="6.0.0-preview.4.21178.6">
      <!-- 'sdk', 'runtime' or 'aspnetcore-runtime' -->
      <PackageType>runtime</PackageType>
      <!-- 'Current' or 'LTS', determines what channel 'latest' version pulls from -->
      <Channel>Current</Channel>
    </AdditionalDotNetPackage>
    <!-- Includes the 6.0.0-preview.4.21175.1 version, using the default runtime packageType, DotNetCliRuntime, and Current channel  -->
    <AdditionalDotNetPackage Include="6.0.0-preview.4.21175.1" />

    <!-- if the above package was not available from a public feed, you can specify a private feed like this -->
    <AdditionalDotNetPackageFeed Include="https://someprivatefeed.blob.azure.com/internal">
      <SasToken>$(SasTokenValueForSomePrivateFeed)</SasToken>
    </AdditionalDotNetPackageFeed>
  </ItemGroup>
  
  <!--
    XUnit Runner
      Enabling this will create one work item for each xunit test project specified.
      This is enabled by specifying one or more XUnitProject items
  -->
  <ItemGroup>
    <XUnitProject Include="..\tests\foo.Tests.csproj"/>
  </ItemGroup>
  <PropertyGroup>
    <!-- TargetFramework to publish the xunit test projects for -->
    <XUnitPublishTargetFramework>netcoreapp3.1</XUnitPublishTargetFramework>
    <!-- TargetFramework of the xunit.runner.dll to use when running the tests -->
    <XUnitRuntimeTargetFramework>netcoreapp2.0</XUnitRuntimeTargetFramework>
    <!-- PackageVersion of xunit.runner.console to use -->
    <XUnitRunnerVersion>2.9.3</XUnitRunnerVersion>
    <!-- Additional command line arguments to pass to xunit.console.exe -->
    <XUnitArguments></XUnitArguments>
  </PropertyGroup>

  <!--
    Microsoft.Testing.Platform (MTP) Runner
      Enabling this will create one Helix work item per MTP-based test project. This
      covers MSTest 4.x with the MTP runner, xUnit v3 with MTP (the default for v3),
      NUnit with the MTP runner, TUnit, and any custom MTP-based framework.

      Each test project must reference Microsoft.Testing.Extensions.TrxReport so that
      results can be reported as a TRX file (which arcade's reporter consumes natively).
      Projects built with MSTest.Sdk, or with Microsoft.DotNet.Arcade.Sdk's XUnitV3
      targets, get this reference implicitly.

      With MTP's --auto-reporters on by default the TRX reporter activates automatically.
      The generated work item command passes only the built-in '--results-directory .'
      so artifacts land in the work item working directory. Pass any reporter flags
      (e.g. '--report-trx --report-trx-filename testResults.trx' if you need a
      deterministic file name) via MTPAdditionalArguments below.
  -->
  <ItemGroup>
    <MTPProject Include="..\tests\bar.Tests.csproj"/>
  </ItemGroup>
  <PropertyGroup>
    <!-- Optional: per-work-item timeout (TimeSpan format). Defaults to 5 minutes. -->
    <MTPWorkItemTimeout>00:05:00</MTPWorkItemTimeout>
    <!--
      Optional: extra command-line arguments appended to every MTP work item command,
      between the built-in '--results-directory .' flag and any per-project Arguments
      metadata. Use this for reporter or framework-specific MTP switches that should
      not be forced on every framework. Examples:
        '--report-trx --report-trx-filename testResults.trx' to control the TRX name
        (requires Microsoft.Testing.Extensions.TrxReport).
        '--auto-reporters off' to silence xUnit v3's auto-activated reporters - this
        flag is registered by xUnit v3 only; MSTest / NUnit / TUnit reject it as an
        unknown option, so it is not injected by default.
    -->
    <MTPAdditionalArguments></MTPAdditionalArguments>
  </PropertyGroup>

  <ItemGroup>
    <!--
      Another way to specify target queues
      This can be used to specify more properties to use for each queue.
    -->
    <HelixTargetQueue Include="Windows.10.Amd64.Open">
      <AdditionalProperties>Platform=x64;Configuration=Debug</AdditionalProperties>
    </HelixTargetQueue>

    <!-- Directory that is zipped up and sent as a correlation payload -->
    <HelixCorrelationPayload Include="some\directory\that\exists" />

    <!-- Workitem that is run on a machine from the $(HelixTargetQueue) queue -->
    <HelixWorkItem Include="some work item name">
      <!-- Command that runs the work item -->
      <Command>echo 'sauce'</Command>

      <!-- A directory that is zipped up and sent as the work item payload -->
      <PayloadDirectory>$(TestFolder)</PayloadDirectory>

      <!-- A TimeSpan that specifies the work item execution timeout -->
      <Timeout>00:30:00</Timeout>

      <!-- Commands that will run before the work item command -->
      <PreCommands>echo 'pepperoni';echo 'cheese'</PreCommands>

      <!-- Commands that will run after the work item command -->
      <PostCommands>echo 'crust';echo 'oven'</PostCommands>
    </HelixWorkItem>
  </ItemGroup>
</Project>
```

### iOS/Android/WASM workload support (XHarness)
The Helix SDK also supports execution of Android/iOS/WASM workloads where you only need to point it to an Android .apk or an iOS/tvOS/WatchOS .app bundle and it will execute these using a tool called XHarness on a specified emulator/device/JS engine. The workloads have to run on Helix queues that are ready for these types of jobs, meaning they have emulators installed, devices connected or JS engine installed. You can read more about this [here](https://github.com/dotnet/arcade/blob/master/src/Microsoft.DotNet.Helix/Sdk/tools/xharness-runner/Readme.md).

### Custom Helix WorkItem functionality
There are times when a work item may detect that the machine being executed on is in a (possibly transient) undesirable state.  Additionally there can be times when a work item would like to request its machine be rebooted after execution (for instance, when a file handle is mysteriously open from another process).  The following functionality has been added to request both of these and can be used either from within a python script or any command line.

#### Request Infrastructure Retry
An "infrastructure retry" is pre-existing functionality Helix Clients use in cases such as when communication to the telemetry service or Azure Service Bus fails; this allows the work item be run again in entirety, generally (but not guaranteedly) on a different machine, with the hope that the next machine will be in a better state.  Note that requesting this prevents any job using it from finishing, and as a FIFO queue the work items that get retried go to the back of the queue, so calling this API can significantly increase job execution time based off how many jobs are being handled by a given queue.      

##### Sample usage in Python:


```py
from helix.public import request_infra_retry

request_infra_retry('Optional reason string')
```

#### Request post-workitem reboot
Helix work items explicitly rebooting the helix client machine themself will never "finish", since this will in most cases preclude sending the final event telemetry for these work items.  However, a work item may know that the machine is in a bad state where a reboot would be desirable (for instance, if the Helix agent is acting as a build machine and some leaked build process is preventing workspace cleanup). After calling this API, the work item runs to completion as normal, then after sending the usual telemetry and uploading results will perform a reboot before taking the next work item. 

##### Sample usage in Python:

```py
from helix.public import request_reboot

request_reboot('Optional reason string')
```

#### Send workitem metric / metrics
Send custom metric(s) for the current workitem. The API accepts metric name(s), value(s) (float) and metric dimensions. These metrics are stored in the Kusto Metrics table.

##### Sample usage in Python:

```py
from helix.public import send_metric, send_metrics

send_metric('MetricName', <value>, {'Dimension1': 'value1', 'Dimension2' : 'value2', ...})

send_metrics({'Metric1': <value1>, 'Metric2': <value2>,...}, {'Dimension1': 'value1', 'Dimension2' : 'value2', ...})
```

#### Sample usage from outside python:

Linux / OSX: `$HELIX_PYTHONPATH -c "from helix.public import <function>; <function>(...)"`

Windows: `%HELIX_PYTHONPATH% -c "from helix.public import <function>; <function>(...)"`

### Common Helix client environment variables

When possible, constructing paths for scripts / commands executed within Helix work items should be done using the provided environment variables, allowing for the engineering team to move and optimize placement of these folders without breaking execution.

You may assume that all the following variables are set on any given Helix client. (Use appropriate-for-OS means to access, i.e. %WINDOWS% or $Linux, $OSX).  The list is not exhaustive but most other variables are simply uninteresting from the perspective of the work item.

- **HELIX_CORRELATION_ID** : GUID identifier for a helix run (include this if sending mail to or tagging dnceng)
- **HELIX_CORRELATION_PAYLOAD** : Correlation payload folder;  root of where all correlation payloads are unzipped.
- **HELIX_PYTHONPATH** : Path to a python 3.x executable (Due to OS constraints, this is only guaranteed to be >= 3.4)
- **HELIX_WORKITEM_FRIENDLYNAME** - "Friendly" name of work item as provided at queue time (include this if relevant when sending mail to or tagging dnceng)
- **HELIX_WORKITEM_ID** : GUID identifier for a helix work item 
- **HELIX_WORKITEM_PAYLOAD** : "Unzip" folder of helix workitem, where its payload was unpacked
- **HELIX_WORKITEM_ROOT** : "Execution" folder of helix workitem, where its payload is copied to and run
- **HELIX_WORKITEM_UPLOAD_ROOT** : Any file in this folder at the end of the work item will be uploaded to result storage and made available via Helix API / backing database.
- **HELIX_DUMP_FOLDER** : Process dumps created here will get uploaded and automatically cleaned up
- **HELIX_CURRENT_LOG** : Path to the current work item's console log (note: will typically have file handles open)


## Test Retry
Helix supports retrying and reporting on partial successes for tests based on repository specific configuration.
When the configuration matches a test failure, the test assembly is reexecuted, and the results compared.
Tests that failed partially (only in some executions) will be reported to Azure DevOps as "Passed on Rerun",
which will include each iteration as a sub-result of the failing test.
If a test passes or fails in all attempts, only a single report is made representing the first execution.

To opt-in and configure test retries when using helix, create file in the reporitory at "eng/test-configuration.json"

### test-configuration.json format
```json
{
  "version" : 1,
  "defaultOnFailure": "fail",
  "localRerunCount" : 2,
  "retryOnRules": [
    {"testName": {"regex": "^System\\.Networking\\..*"}},
    {"testAssembly": {"wildcard": "System.SomethingElse.*" }},
    {"failureMessage": "network disconnected" },
  ],
  "failOnRules": [
  ],
  "quarantineRules": [
  ]
}
```

## Description
### version
Schema version for compatibility (curent version is 1)

### defaultOnFailure
- default: "fail"

One of "fail" or "rerun"
<dl>
<dt>fail</dt><dd>If a test fails, the default behavior if no rules match is to fail the test immediate</dd>
<dt>rerun</dt><dd>If a test fails, the default behavior is no rules match is to rerun the test according to the localRerun/remoteRerun counts</dd>
</dl>

## localRerunCount
- default: 1

This number indicates the number of times a test that needs to be "rerun" should be rerun on the local computer immediately.
This is the fastest rerun option, because the payloads don't need to be redownloaded, so it always the first attempted re-execution method.

In the example, with a value of "2", that means that the test will need to fail 3 times before being marks as failed (1 intial failure, and 2 rerun failures).

## rules
The three "rules" entries are lists of rules that will be used to match test to determine desired behavior. In the case of multiple rule matches:
- if a quarantine rule matches, the test is quarantined
- if the default behavior is "rerun" and a "fail" rule matches, the test is failed
- if the default behavior is "fail" and a "rerun" rule matches, the test is rerun
- default behavior is used

A "rule" consists on at least one condition. A condition should have a [property](#properties) and a [rule object](#rule-object), but it could have more than one condition.

#### Rule with one condition
In this case any test with a testName of "Pizza" is going to be retried

```json
{
 "retryOnRules":[{"testName": "Pizza"}]
}
```

#### Rule with multiple conditions
In this case the  `testName` needs to be "Pizza" and the `failureMessage` needs to be "Message" in order to meet the rule to be retried.

```json
{
  "retryOnRules": [{"testName":"Pizza",   "failureMessage":"Message"}]
}
```

#### Multiple rules
In this example we see two rules on `retryOnRules` section, only one rule needs to be met to retry the build. 

In this case if a test fails and its `testName` is "Pizza" or its `testName` is "Taco", the test is going to be retried.

```json
{
  "retryOnRules": [
    {"testName":"Pizza"},
    {"testName":"Taco"}
  ]
}
```

### Properties
<dl>
<dt>testName</dt><dd>The name of the test, including namespace, class name, and method name</dd>
<dt>testAssembly</dt><dd>The name of the assembly containing the test</dd>
<dt>failureMessage</dt><dd>The failure message logged by the test</dd>
<dt>callstack (multiline)</dt><dd>The callstack reported by the test execution</dd>
</dl>

### Rule object
For all rules, if a property is designated "multiline", then the rule must match a line, otherwise the entire value is used.

All comparisons are case-insensitive
#### Raw string (e.g. "rule string")
True if the property value exactly matches the string
#### {"contains": "value"}
True if the property contains (case-insensitive) the value string
#### {"wildcard": "value with * wildcard"}
The same as a raw string, but "*" can match any number of characters, and "?" can match one character
#### {"regex": "value with .* regex"}
true if the property matches the regular expression
