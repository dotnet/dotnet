# Configure a test run

This document covers configuration of a test run in the test platform.

## Overview

There are three different ways to configure various aspects of a test run.

1. **Using command line arguments**
Various configuration options can be provided to the `vstest.console` or `dotnet
test` command line. For example, `--framework` can specify the runtime framework
version, or `--platform` can specify the architecture of test run (`x86` or
`x64`).

1. **Using a runsettings file**
User can specify a `runsettings` file to configure test run. For example:

* `> vstest.console.exe --settings:test.runsettings test.dll`
* `> dotnet test -s test.runsettings`

1. **Using command line runsettings parameters**
Various elements of a `runsettings` file can also specified as command line
parameters directly. For example, consider following `runsettings`:

```xml
<RunSettings>
    <RunConfiguration>
        <DisableAppDomain>False</DisableAppDomain>
    </RunConfiguration>
</RunSettings>
```

The `DisableAppDomain` settings can be changed in the following way:
`> vstest.console --settings:test.runsettings -- RunConfiguration.DisableAppDomain=true`

Or, `> dotnet test -s test.runsettings -- RunConfiguration.DisableAppDomain=true` if
you're using dotnet test to execute tests.

Order of priority (in case of conflicting settings) is as follows:

1. If a command line switch is available, it takes priority. E.g. `/platform`
   wins over `<TargetPlatform>` specified in the `runsettings` file.
1. If a command line runsettings is available, it takes priority over the
   content of `runsettings` file. E.g. in case of `dotnet test -s
   test.runsettings -- RunConfiguration.TargetPlatform=x86`, platform is set to
   `x86` even if `test.runsettings` specifies `x64` as platform.
1. If a `runsettings` file is provided, it is used for the test run.

## Run settings

A `*.runsettings` file is used to configure various aspects of a test discovery
and execution with `vstest.console.exe` or the `Test Explorer` in VS. An editor
using the test platform, can specify a `runsettings` as an `xml` in the `Test
Discovery` or `Test Run` requests (details are in Editor API Specification).

The `runsettings` file is a xml file with following sections:

1. Run Configuration
1. Data Collection
1. Runtime Parameters
1. Adapter Configuration
1. Legacy Settings

We will cover these sections in detail later in the document. Let's discuss few
core principles for runsettings.

### Principles

1. Any runner (CLI, IDE, Editor) can use run settings to configure the test run
1. These runners need to take care of merging user provided runsettings with
   their own configuration parameters. E.g. an IDE may provide `TargetPlatform`
   as an UI configuration, at the same time an user can also provide the setting
   in a `runsettings` file. It is the IDE's responsibility to disambiguate.
1. Run settings are always created at the start of a test run. They are
   immutable for rest of the run.
1. Every test platform extension should get the _effective_ runsettings for a
   test run via the test platform APIs. It may use it to read its own settings
   or take decisions (e.g. an adapter needs to disable appdomains if
   `DisableAppDomain` setting is provided via run settings).

### Sample

Given below is a complete `runsettings` file with all available options. Each
option is briefly annotated, details are available in a later section of this
document.

```xml
<?xml version="1.0" encoding="utf-8"?>  
<RunSettings>  
  <!-- Configurations that affect the Test Framework -->  
  <RunConfiguration>  
    <!-- 1. Test related settings -->
    <!-- [x86] | x64: architecture of test host -->  
    <TargetPlatform>x86</TargetPlatform>
  
    <!-- Framework35 | [Framework40] | Framework45 -->  
    <TargetFrameworkVersion>Framework40</TargetFrameworkVersion>
  
    <!-- Path to Test Adapters -->  
    <TestAdaptersPaths>%SystemDrive%\Temp\foo;%SystemDrive%\Temp\bar</TestAdaptersPaths>

    <!-- Path relative to solution directory -->  
    <ResultsDirectory>.\TestResults</ResultsDirectory>
    <SolutionDirectory>.\TestResults</SolutionDirectory>
    
    <!-- CPU cores to use for parallel runs -->
    <MaxCpuCount>2</MaxCpuCount>
    
    <!-- Specify timeout in milliseconds. A valid value should be >= 0. If 0, timeout will be infinity-->
    <TestSessionTimeout>10000</TestSessionTimeout>

    <!-- STA | MTA  default is STA for .NET Full and MTA for .NET Core-->
    <ExecutionThreadApartmentState>STA</ExecutionThreadApartmentState>

    <!-- 2. Hints to adapters to behave in a specific way -->
    <DesignMode>false</DesignMode>
    <DisableParallelization>false</DisableParallelization>
    <DisableAppDomain>false</DisableAppDomain>
    <CollectSourceInformation>true</CollectSourceInformation>

    <!-- 3. Runner related configuration -->
    <BatchSize>10</BatchSize>
    
  </RunConfiguration>  
  
  <!-- Configurations for data collectors -->  
  <DataCollectionRunSettings>  
    <DataCollectors>  
      <DataCollector friendlyName="Code Coverage" uri="datacollector://Microsoft/CodeCoverage/2.0">
        <Configuration>  
          <CodeCoverage>  
            <ModulePaths>  
              <Exclude>  
                <ModulePath>.*CPPUnitTestFramework.*</ModulePath>  
              </Exclude>  
            </ModulePaths>  
  
            <!-- We recommend you do not change the following values: -->  
            <UseVerifiableInstrumentation>True</UseVerifiableInstrumentation>  
            <AllowLowIntegrityProcesses>True</AllowLowIntegrityProcesses>  
            <CollectFromChildProcesses>True</CollectFromChildProcesses>  
            <CollectAspDotNet>False</CollectAspDotNet>  
          </CodeCoverage>  
        </Configuration>  
      </DataCollector>  
  
    </DataCollectors>  
  </DataCollectionRunSettings>  
  
  <!-- Configurations for in-proc data collectors -->  
  <InProcDataCollectionRunSettings>  
    <InProcDataCollectors>
      <InProcDataCollector friendlyName="InProcDataCollectionExample" uri="InProcDataCollector://Vstest.Datacollectors/InProcDataCollectionExample/1.0" codebase="C:\Users\samadala\src\vstest.datacollectors\Examples\bin\Debug\net46\ExamplesDataCollector.dll" assemblyQualifiedName="Vstest.Datacollectors.Examples.InProcDataCollectionExample, ExamplesDataCollector, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null" >
      <Configuration>
          <Port>4312</Port>
        </Configuration>
      </InProcDataCollector>
    </InProcDataCollectors>
  </InProcDataCollectionRunSettings>

  <!-- Adapter Configuration -->
  <MSTest>
    <!-- Enable legacy mode, legacy setting are honored only when this flag is set to true -->
    <ForcedLegacyMode>true</ForcedLegacyMode>
  </MSTest>

  <!-- Settings used for Ordered tests and MSTest v1 based tests when running in legacy mode -->
  <!-- Note: These settings do not apply to MSTest v2 tests -->
  <LegacySettings>
    <Deployment>
      <DeploymentItem filename=""C:\DeploymentDir\DeploymentFile"" />
    </Deployment>

    <!-- Set up scripts for setup and clean up-->
    <Scripts setupScript=""C:\SetupScript.bat"" cleanupScript=""C:\CleanupScript.bat"" />
  </LegacySettings>

  <!-- Parameters used by tests at runtime -->  
  <TestRunParameters>  
    <Parameter name="webAppUrl" value="http://localhost" /> 
    <Parameter name="webAppUserName" value="Admin" />
    <Parameter name="webAppPassword" value="Password" />
  </TestRunParameters>  
</RunSettings>  
```

### Section 1: Run Configuration

```xml
<RunSettings>  
  <RunConfiguration>  
    <!-- Run Configuration section -->
  </RunConfiguration>  
</RunSettings>
```

This is the core part of a `runsettings` file. It must be present in every
runsettings. This section is filled in by the test runner (CLI, IDE or Editor)
with various parameters of the test projects and user provided settings.

Available elements are:

1. **Test settings**

_Example_

```xml
<?xml version="1.0" encoding="utf-8"?>  
<RunSettings>  
  <RunConfiguration>  
    <TargetPlatform>x86</TargetPlatform>  
    <TargetFrameworkVersion>.NET Framework, Version=v4.6</TargetFrameworkVersion>  
    <TestAdaptersPaths>%SystemDrive%\Temp\foo;%SystemDrive%\Temp\bar</TestAdaptersPaths>  
    <ResultsDirectory>.\TestResults</ResultsDirectory>  
    <SolutionDirectory>.\TestResults</SolutionDirectory>  
    <MaxCpuCount>2</MaxCpuCount>
    <TestSessionTimeout>10000</TestSessionTimeout>
    <ExecutionThreadApartmentState>STA</ExecutionThreadApartmentState>
  </RunConfiguration>  
</RunSettings>
```

_Description_

| Settings Element  | Type   | Function                                                                                        |
|-------------------|--------|-------------------------------------------------------------------------------------------------|
| TargetPlatform    | string | Configures the architecture of test host. Possible values: (x86,  x64)                          |
| TargetFrameworkVersion   | string | Configures the target runtime for a test run. Possible values: any valid [FrameworkName][]      |
| TestAdaptersPaths | string | Semi-colon separated directories which contain test adapters                                    |
| ResultsDirectory  | string | Directory for test run reports. E.g. trx, coverage etc.                                         |
| SolutionDirectory | string | Working directory for test invocation. Results directory can be relative to this. Used by IDEs. |
| MaxCpuCount       | int    | Degree of parallelization, spawns `n` test hosts to run tests. Default: 1. Max: Number of cpu cores. |
| TestSessionTimeout | int   | Testplatform will cancel the test run after it exceeded given TestSessionTimeout in milliseconds and will show the results of tests which ran till that point. **Required Version: 15.5+.** |
| ExecutionThreadApartmentState       | string    | Apartment state of thread which calls adapter's RunTests and Cancel APIs. Possible values: (MTA, STA). default is STA for .NET Full and MTA for .NET Core.  STA supported only for .NET Full **Required Version: 15.5+.** [More details.](#execution-thread-apartment-state) |

Examples of valid `TargetFrameworkVersion`:

* .NETCoreApp, Version=v1.0
* .NETCoreApp, Version=v1.1
* .NETFramework, Version=v4.5

[FrameworkName]: https://msdn.microsoft.com/en-us/library/dd414023(v=vs.110).aspx

2. **Adapter settings**
These settings are a hint to adapters to behave in a particular way. These are
determined by runners based on user configuration. It is also possible for an
user to provide these settings, if they want to tweak a run.

_Example_

```xml
<?xml version="1.0" encoding="utf-8"?>  
<RunSettings>  
  <RunConfiguration>  
    <DesignMode>false</DesignMode>
    <DisableParallelization>false</DisableParallelization>
    <DisableAppDomain>false</DisableAppDomain>
    <CollectSourceInformation>true</CollectSourceInformation>
  </RunConfiguration>  
</RunSettings>
```

_Description_

| Settings Element         | Type | Function                                                                           |
|--------------------------|------|------------------------------------------------------------------------------------|
| DesignMode               | bool | True if test run is triggered in an IDE/Editor context.                            |
| DisableParallelization   | bool | If true, an adapter should disable any test case parallelization                   |
| DisableAppDomain         | bool | If true, an adapter shouldn't create appdomains to run tests                       |
| CollectSourceInformation | bool | If false, an adapter need not parse symbols to provide test case file, line number |

3. **Runner settings**
An IDE/Editor can set these settings to change the behavior of test platform.
They are not actionable for an adapter.

_Example_

```xml
<?xml version="1.0" encoding="utf-8"?>  
<RunSettings>  
  <RunConfiguration>  
    <BatchSize>10</BatchSize>
  </RunConfiguration>  
</RunSettings>
```

_Description_

| Settings Element | Type | Function                                                                                                    |
|------------------|------|-------------------------------------------------------------------------------------------------------------|
| BatchSize        | int  | Configures the frequency of run statistics. Discovered/Test results are send once `n` tests are accumulated |

### Section 2: Data Collection

```xml
<RunSettings>  
  <!-- Configurations for out-of-process data collectors -->  
  <DataCollectionRunSettings>  
    <DataCollectors>  
      <!-- Data collectors configuration goes here -->
    </DataCollectors>  
  </DataCollectionRunSettings>  
  
  <!-- Configurations for in-process data collectors -->  
  <InProcDataCollectionRunSettings>  
    <DataCollectors>  
      <!-- Data collectors configuration goes here -->
    </DataCollectors>
  </InProcDataCollectionRunSettings>
</RunSettings>
```

This section lists all [data collectors][] configured for a test run. The test
runner creates this section based on arguments provided in the command line.
E.g. if `--collect:"Code Coverage"` is provided, a `Code Coverage` entry is
created out of process data collectors.

[data collectors]: ./analyze.md

In-process data collectors should be configured by the user.

Data collector extension authors may use the content within the specific
`<DataCollector friendlyName=mycollector>` node to provide configuration options
for their data collector. For example, the code coverage data collector uses
following section.

_Example_

```xml
<RunSettings>  
  <!-- Configurations for data collectors -->  
  <DataCollectionRunSettings>  
    <DataCollectors>  
      <DataCollector friendlyName="Code Coverage" uri="datacollector://Microsoft/CodeCoverage/2.0">
        <!-- Code Coverage Settings Start -->
        <Configuration>  
          <CodeCoverage>  
            <ModulePaths>  
              <Exclude>  
                <ModulePath>.*CPPUnitTestFramework.*</ModulePath>  
              </Exclude>  
            </ModulePaths>  
  
            <!-- We recommend you do not change the following values: -->  
            <UseVerifiableInstrumentation>True</UseVerifiableInstrumentation>  
            <AllowLowIntegrityProcesses>True</AllowLowIntegrityProcesses>  
            <CollectFromChildProcesses>True</CollectFromChildProcesses>  
            <CollectAspDotNet>False</CollectAspDotNet>  
          </CodeCoverage>  
        </Configuration>  
        <!-- Code Coverage Settings End -->
      </DataCollector>  
    </DataCollectors>  
  </DataCollectionRunSettings>  
</RunSettings>  
```

_Description_

| Settings Element | Type   | Function                                                                  |
|------------------|--------|---------------------------------------------------------------------------|
| DataCollector    | string | Provides a data collector information. See below for required attributes. |

Required attributes:

1. `friendlyName` provides a common name for the data collector. It is declared
   by a data collector implementation.
2. `enabled` is used to enable/disable a data collector. If `true` the data
   collector participates in test run.
3. `uri` is the identity of a data collector. It is declared by the data
   collector implementation.

Test platform uses above attributes to instantiate the appropriate datacollector for
a test run.

> Note: it is possible for users to provide the runsettings with a
> `DataCollectionRunSettings` node with any configuration for the data
> collectors.

### Other Sections

#### Legacy Settings

This section allows users to configure settings that were earlier required to be
set using *testsettings file.
These settings can be used for Ordered tests and MSTest v1 based tests when running in legacy mode.

Users can now migrate testsettings to runsettings using [SettingsMigrator](./RFCs/0023-TestSettings-Deprecation.md#migration)

Here is an example:

```xml
<Runsettings>
  <MSTest>
    <!-- Enable legacy mode, legacy setting are honored only when this flag is set to true -->
    <ForcedLegacyMode>true</ForcedLegacyMode>
  </MSTest>

  <!-- Settings used for Ordered tests and MSTest v1 based tests when running in legacy mode -->
  <!-- Note: These settings do not apply to MSTest v2 tests -->
  <LegacySettings>
    <Deployment>
      <DeploymentItem filename=""C:\DeploymentDir\DeploymentFile"" />
    </Deployment>

    <!-- Set up scripts for setup and clean up-->
    <Scripts setupScript=""C:\SetupScript.bat"" cleanupScript=""C:\CleanupScript.bat"" />

    <Execution parallelTestCount=""5"" hostProcessPlatform=""MSIL"">
      <!-- Configure test timeout, use TestSessionTimeout in Runconfiguration to configure session timeout -->
      <Timeouts testTimeout=""6000"" />

      <!-- Configure the Hosts-->
      <Hosts skipUnhostableTests=""false"">
        <AspNet name=""ASP.NET"" executionType=""Iis"" urlToTest=""http://localhost"" />
      </Hosts>

      <!-- Add assembly resolution -->
      <TestTypeSpecific>
        <UnitTestRunConfig testTypeId=""13cdc9d9-ddb5-4fa4-a97d-d965ccfc6d4b"">
          <AssemblyResolution applicationBaseDirectory=""E:\AppBaseDir"">
            <TestDirectory useLoadContext=""false"" />
            <RuntimeResolution>
              <Directory path=""E:\RuntimeResolutionDir"" includeSubDirectories=""false"" />
            </RuntimeResolution>
            <DiscoveryResolution>
              <Directory path=""E:\DiscoveryResolutionDir"" includeSubDirectories=""true"" />
            </DiscoveryResolution>
          </AssemblyResolution>
        </UnitTestRunConfig>
      </TestTypeSpecific>
    </Execution>
  </LegacySettings>
</Runsettings>
  ```

#### Run Parameters

```xml
  <!-- Parameters used by tests at runtime -->  
  <TestRunParameters>  
    <Parameter name="webAppUrl" value="http://localhost" />  
    <Parameter name="webAppUserName" value="Admin" />  
    <Parameter name="webAppPassword" value="Password" />  
  </TestRunParameters>  
```

This section provides ability for user to specify a `key-value` pair dictionary
available to tests during runtime.

> Note that it is the responsibility of an adapter to ensure this section is
> available to tests at runtime.
> E.g. the mstest framework makes these settings available as part of the
> `TestContext` API.

#### Adapter Configuration

An adapter may provide a section in runsettings for users. See [mstest config][]
and [nunit config][] for more details.

[mstest config]: TODO
[nunit config]: TODO

# Execution thread apartment state

This section explains usage of ExecutionThreadApartmentState element in runsettings and testplatform behavior for same.

### Usage

#### using runsettings file

```xml
<?xml version="1.0" encoding="utf-8"?>
<RunSettings>
  <RunConfiguration>
     <ExecutionThreadApartmentState>STA</ExecutionThreadApartmentState>
  </RunConfiguration>
</RunSettings>
```

#### using command line

vstest.console.exe a.dll -- RunConfiguration.ExecutionThreadApartmentState=STA

dotnet test -f net46 -- RunConfiguration.ExecutionThreadApartmentState=STA

### History

In Test Platform V1 ExecutionThreadApartmentState property can be set from vstest.executionengine*.exe.config file. Default value is `STA`.

### Behavior

In Test platform V2 ExecutionThreadApartmentState property default value is `MTA` for .NET Core and `STA` for .NET Full. `STA` value is only supported for .NET Framework.
Warning should be shown on trying to set value `STA` for .NET Core and UAP10.0 frameworks tests.

* To support adapters which depends on thread test platform creates may need STA apartment state to run UI tests. `ExecutionThreadApartmentState` option can be used to set apartment state. Example: MSTest v1, MSTest v2 and MSCPPTest adapters.

* The recommended way to make the tests run in STA thread is using custom attributes that adapter provides.

| Adapter | Attribute|
|-----------|-----------|
| MSTest v2 | STATestMethod/STATestClass
| NUnit | Apartment |
| Xunit | STAFact
