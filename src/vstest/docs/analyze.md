# Monitor and analyze test run

This document will walk you through enabling data collection for a test run covering:

1. A brief overview of DataCollectors.
1. Configuring DataCollectors in TPv2.
1. Key differences for using DataCollectors in TPv2 v/s TPv1.
1. Instructions for using [code coverage][coverage].

> **Version note:**
>
> DataCollection support is added in test platform `15.3.0` onwards. It is part of
> VS 2017 15.3 and dotnet-cli 2.0.0 builds.

[coverage]: #coverage

## DataCollector

A DataCollector is a test platform extension to monitor test run. It can be extended to perform tasks on specific test execution events. Currently, four events are exposed to DataCollector:

1. Session Start event.
2. Test Case Start event
3. Test Case End event.
4. Session End event.

You can author a DataCollector to collect code coverage data for a test run, to collect logs when a test case or test run fails, etc. These additional files are called Attachments and they can be attached to test result report(trx).

Please refer [here](./extensions/datacollector.md) for instructions on creating a DataCollector and [here](./RFCs/0006-DataCollection-Protocol.md)
if you're interested in the architecture of data collection.

## Configure DataCollectors

DataCollectors can be configured for monitoring test execution through runSettings, testSettings or vstest.console args.

### Using RunSettings<a name="Using-RunSettings"></a>

Below is the sample runsettings for a custom DataCollector

```xml
<?xml version="1.0" encoding="utf-8"?>  
<RunSettings>
   <RunConfiguration>      
    <!-- Path to Test Adapters -->  
    <TestAdaptersPaths>PathToAdapters;PathToDataCollectors</TestAdaptersPaths>  
  </RunConfiguration>  

  <!-- Configurations for DataCollectors -->  
  <DataCollectionRunSettings>  
    <DataCollectors>  
      <DataCollector friendlyName="MyDataCollector" uri="datacollector://MyCompany/MyDataCollector/1.0">  
        <Configuration>
                    <LogFileName>DataCollectorLogs.txt</LogFileName>
        </Configuration>  
      </DataCollector>
    </DataCollectors>  
  </DataCollectionRunSettings>  
</RunSettings>
```

Below is the sample command for enabling DataCollectors using runsettings

```shell
> "%vsinstalldir%\Common7\IDE\Extensions\TestPlatform\vstest.console.exe" test_project.dll /settings:datacollection.runsettings
```

### Using vstest.console args<a name="Using-vstest.console-args"></a>

DataCollectors can be configured and used through first class command line arguments `/collect` and `/testadapterpath`. Hence, for common DataCollection scenarios, separate runsettings file may not be required.

Below is the sample command to configure and use DataCollectors through vstest.console command line.

```shell
> "%vsinstalldir%\Common7\IDE\Extensions\TestPlatform\vstest.console.exe" test_project.dll /collect:"Code Coverage"
> "%vsinstalldir%\Common7\IDE\Extensions\TestPlatform\vstest.console.exe" test_project.dll /collect:"MyDataCollector" /testadapterpath:<Path to MyDataCollector assembly>
```

Please note that `testadapterpath` is not required for DataCollectors shipped along with TPv2.

### Using TestSettings

While the recommended way is to use [runsettings](#Using-RunSettings) or [vstest.console args](#Using-vstest.console-args), there are few DataCollectors which only worked with testsettings.
E.g.: `System Information` DataCollector. Below is the sample testsettings for using `System Information` DataCollector.

```xml
<?xml version="1.0" encoding="UTF-8"?>
<TestSettings name="TestSettings1" id="2d572055-54c0-4cac-8a55-9d7ffb48ac17" xmlns="http://microsoft.com/schemas/VisualStudio/TeamTest/2010">
  <Description>These are default test settings for a local test run.</Description>
  <Deployment enabled="false" />
  <Execution>
    <AgentRule name="LocalMachineDefaultRole">
      <DataCollectors>
        <DataCollector uri="datacollector://microsoft/SystemInfo/1.0" assemblyQualifiedName="Microsoft.VisualStudio.TestTools.DataCollection.SystemInfo.SystemInfoDataCollector, Microsoft.VisualStudio.TestTools.DataCollection.SystemInfo, Version=15.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" friendlyName="System Information">
        </DataCollector>
      </DataCollectors>
    </AgentRule>
  </Execution>
  <Properties />
</TestSettings>
```

Below is the sample command for enabling DataCollectors using testsettings

```shell
> "%vsinstalldir%\Common7\IDE\Extensions\TestPlatform\vstest.console.exe" test_project.dll /settings:datacollection.testsettings
```

### Enable/Disable a DataCollector

All DataCollectors configured in the .runsettings files are loaded automatically and are enabled to participate for run, unless explicitly disabled using boolean valued attribute named `enabled`.

For example, only `MyDataCollector1` DataCollector will be enabled for a test run with
below runsettings:

```xml
<?xml version="1.0" encoding="utf-8"?>  
<RunSettings>
   <RunConfiguration>      
    <!-- Path to Test Adapters -->  
    <TestAdaptersPaths>PathToAdapters;PathToDataCollectors</TestAdaptersPaths>  
  </RunConfiguration>  

  <!-- Configurations for DataCollectors -->  
  <DataCollectionRunSettings> 
    <DataCollectors> 
      <DataCollector friendlyName="MyDataCollector1" uri="datacollector://MyCompany/MyDataCollector1/1.0">
    </DataCollector> 
  
    <DataCollector friendlyName="MyDataCollector2" uri="datacollector://MyCompany/MyDataCollector2/1.0" enabled="false">
      </DataCollector> 
    </DataCollectors> 
  </DataCollectionRunSettings>
</RunSettings>
```

A specific DataCollector can be explicitely enabled using the `/collect:<friendly name>` command line switch.

For example, below command line will enable a DataCollector named `MyDataCollector1`
(and disable other DataCollectors mentioned in .runsettings):

```shell
> "%vsinstalldir%\Common7\IDE\Extensions\TestPlatform\vstest.console.exe" test_project.dll /collect:MyDataCollector1 /settings:<Path to runSettings>
```

More than one DataCollectors can also be enabled using `/collect` command line switch

For example, below command will enable DataCollectors named `MyDataCollector1` and `MyDataCollector2`:

```shell
> "%vsinstalldir%\Common7\IDE\Extensions\TestPlatform\vstest.console.exe" test_project.dll /collect:MyDataCollector1 /collect:MyDataCollector2 /settings:<Path to runSettings>
```

## Key differences for using DataCollectors in TPv2 v/s TPv1

1. In TPv1, DataCollectors are loaded from `<<VisualStudio Installation Directory>>\Common7\IDE\PrivateAssemblies\DataCollectors`.
In TPv2, DataCollectors are loaded from `TestAdaptersPaths` specified in runSettings or `/testadapterpath` argument of `vstest.console.exe`. DataCollector assemblies must follow the naming convention *collector.dll.

2. Previous DataCollector settings will continue to work, but additional `TestAdaptersPaths` must be specified in runsettings if DataCollector is not shipped along with TPv2. `TestAdapterPath` can also be specified through [vstest.console args](#Using-vstest.console-args) from command line.

3. There are breaking changes in latest DataCollector interface. Hence, older DataCollectors need to be rebuilt against latest APIs to work with TPv2. For details, refer [here(todo)]();

## Working with Code Coverage<a name="coverage"></a>

> **Requirements:**
> Code Coverage requires the machine to have Visual Studio 2017 Enterprise ([15.3.0](https://www.visualstudio.com/vs) or later installed and a Windows operating system.

### Setup a project

Here's a sample project file, please note the xml entity marked as `Required`. Previously, the `Microsoft.VisualStudio.CodeCoverage` was required, but is now shipped with the SDK.

```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <TargetFramework>netcoreapp1.1</TargetFramework>
    
    <!-- Required in both test/product projects. This is a temporary workaround for https://github.com/Microsoft/vstest/issues/800 -->
    <DebugType>Full</DebugType>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Microsoft.NET.Test.Sdk" Version="15.3.0" />
    <PackageReference Include="MSTest.TestAdapter" Version="1.1.17" />
    <PackageReference Include="MSTest.TestFramework" Version="1.1.17" />
  </ItemGroup>

</Project>
```


### Analyze coverage with Visual Studio

> **Version note:**
>
> Try this feature with [Visual Studio 2017 15.3.0](https://www.visualstudio.com/vs) or later.

Use the `Analyze Code Coverage` context menu available in `Test Explorer` tool window to start a coverage run.

After the coverage run is complete, a detailed report will be available in the `Code Coverage Results` tool window.

Please refer the documentation for additional details: <https://docs.microsoft.com/en-us/visualstudio/test/using-code-coverage-to-determine-how-much-code-is-being-tested>

### Collect coverage with command line runner

Use the following command line to collect coverage data for tests:

```shell
> "%vsinstalldir%\Common7\IDE\Extensions\TestPlatform\vstest.console.exe" --collect:"Code Coverage" --framework:".NETCoreApp,Version=v1.1" d:\testproject\bin\Debug\netcoreapp1.1\testproject.dll
```

This will generate a `*.coverage` file in the `<Current working directory>\TestResults` directory.

### Event Log Data Collector

This document introduces Event Log DataCollector. We will start with a brief overview of Event Log DataCollector, use cases where it will be useful followed by steps to enable it.

#### Introduction

Event Log DataCollector is a Windows only DataCollector that is used to get event logs logged into Windows Event Viewer during test execution. Event logs are saved in a file `Event Log.xml` and this file is available as Attachment as part of test result report (trx).
When enabled, Event Log DataCollector generates one `Event Log.xml` file for entire test session. `Event Log.xml` files are also generated corresponding to all test cases as well, to provide a granular view of events logged while executing a test case.

More info on Event Viewer [here](https://technet.microsoft.com/en-us/library/cc938674.aspx)

#### Use cases for Event Log DataCollector

Event Log DataCollector is used to get event logs as Attachment and is particularly useful for remote scenarios where logging into the machine and viewing the Event Viewer is not possible.

#### Enabling Event Log DataCollector

There are two ways of enabling Event Log DataCollector for a test run:

##### 1. Using vstest.console argument

Use the following command to enable Event Log DataCollector with default configuration:

> "%vsinstalldir%\Common7\IDE\Extensions\TestPlatform\vstest.console.exe" test_project.dll /testadapterpath:<<Path to test adapter>> /collect:"Event Log"

##### 2. Using runsettings

Below runsettings can be used to enable Event Log DataCollector.

```xml
<?xml version="1.0" encoding="utf-8"?>  
<RunSettings>
  <!-- Configurations for DataCollectors -->  
  <DataCollectionRunSettings> 
    <DataCollectors> 
      <DataCollector friendlyName="Event Log" uri="datacollector://Microsoft/EventLog/2.0">
        <Configuration>
            <Setting name="EventLogs" value="System,Application" />
            <Setting name="EntryTypes" value="Error,Warning" />
            <Setting name="EventSources" value="CustomEventSource" />
            <Setting name="MaxEventLogEntriesToCollect" value="5" />
        </Configuration>
      </DataCollector>
    </DataCollectors>
  </DataCollectionRunSettings>
</RunSettings>
```

The above runsettings will collect event logs from `System` and `Application` event logs which are logged as `Error` or `Warning` and event source is specified as `CustomEventSource`. `MaxEventLogEntriesToCollect` specifies the upper limit on the events that are logged in `Event Log.xml` file corresponding to test cases. There is no upper limit on number of events logged in `Event Log.xml` file for test session.

In default configuration (through vstest.console.exe args or when <Configuration> section is empty in runsettings), `System`, `Application` and `Security` logs with entry types `Error`, `Warning` or `FailureAudit` and with any event source are collected. Default value of `MaxEventLogEntriesToCollect` is 50000. There is no upper limit on number of events logged in `Event Log.xml` file for test session.

> **A note on `Security` Event Log**
>
>Please note that `Security` event logs can only be collected if the account under with tests are run has admin privileges.
