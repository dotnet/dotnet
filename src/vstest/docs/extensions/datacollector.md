# Your first datacollector
DataCollectors are used to monitor test execution. Getting CPU or memory usage info, taking screenshot, recording screen activity, measuring code coverage, etc. while executing tests are a few common scenarios that can be realised through DataCollectors. You can write your own data collectors to meet your specific requirements and use it while executing tests.

In this walkthrough, you will learn how to create your first `DataCollector` and how to plug it in while executing test cases. 

## Extend DataCollector
The very first thing you will need to create is a Class Library project and add reference to `Microsoft.TestPlatform.ObjectModel` nuget package.
Class Library project can target .NET Framework (for example `net462`) or .NET (for example `net8.0`, `net9.0`, or `net10.0`), or multi-target both families when the collector needs to run in both environments.

> **DataCollector Assembly Naming Convention**
>
> When test platform is looking for a DataCollector it will likely need to examine many
> assemblies. As an optimization, test platform will only look at distinctly named
> assemblies, specifically, a DataCollector must follow the naming convention
> `*collector.dll`. By enforcing such a naming convention, test platform can speed up
> locating a DataCollector assembly. Once located, test platform will load the data
> collector for the entire run.

A new data collector can be implemented by extending the abstract `DataCollector` class. 

```csharp
using System;
using System.IO;
using System.Xml;

using Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;

[DataCollectorFriendlyName("NewDataCollector")]
[DataCollectorTypeUri("my://new/datacollector")]
public class NewDataCollector : DataCollector
{
    private string logFileName = "DataCollectorLogs.txt";
    private DataCollectionEnvironmentContext? context;
    private DataCollectionSink dataSink = null!;
    private DataCollectionLogger logger = null!;

    public override void Initialize(
            XmlElement? configurationElement,
            DataCollectionEvents events,
            DataCollectionSink dataSink,
            DataCollectionLogger logger,
            DataCollectionEnvironmentContext? environmentContext)
    {
        this.context = environmentContext;
        this.dataSink = dataSink;
        this.logger = logger;

        events.SessionStart += this.SessionStarted_Handler;
        events.TestCaseStart += this.Events_TestCaseStart;
        logFileName = configurationElement?["LogFileName"]?.InnerText ?? logFileName;
    }

    private void SessionStarted_Handler(object? sender, SessionStartEventArgs args)
    {
        var filename = Path.Combine(AppContext.BaseDirectory, logFileName);
        File.WriteAllText(filename, "SessionStarted");

        if (context is not null)
        {
            dataSink.SendFileAsync(context.SessionDataCollectionContext, filename, true);
            logger.LogWarning(context.SessionDataCollectionContext, "SessionStarted");
        }
    }

    private void Events_TestCaseStart(object? sender, TestCaseStartEventArgs e)
    {
        logger.LogWarning(e.Context, "TestCaseStarted " + e.TestCaseName);
    }
}
```
TestPlatform uniquely identifies each of the DataCollector by `DataCollectorFriendlyName` and `DataCollectorTypeUri`.

Here is a brief description of each of the argument that is passed in constructor.

### Configuration Element
At times, there is a need to pass configuration settings for initializing data collectors that could vary between test runs. 
E.g. module names to exclude from code coverage.
For supporting those scenarios, configuration xml can be passed to DataCollectors using runsettings.
```xml
<RunSettings>
    <DataCollectionRunSettings>
        <DataCollectors>
            <DataCollector friendlyName="NewDataCollector">
                <Configuration>
                    <LogFileName>DataCollectorLogs.txt</LogFileName>
                </Configuration>
            </DataCollector>
        </DataCollectors>
    </DataCollectionRunSettings>
</RunSettings>
```
```csharp
XmlElement? logFileElement = configurationElement?["LogFileName"];
string logFile = logFileElement?.InnerText ?? "DataCollectorLogs.txt";
string path = Path.Combine(AppContext.BaseDirectory, logFile);

if (!File.Exists(path))
{
    // Create a file to write to.
    string createText = "Hello and Welcome" + Environment.NewLine;
    File.WriteAllText(path, createText);
}
```

### DataCollectionEvents
DataCollectors can choose to subscribe to the following events exposed by `DataCollectionEvents` 
1. TestSessionStart : Raised when test execution session starts.
2. TestSessionEnd : Raised when test execution session ends.
3. TestCaseStart : Raised when test case execution starts.
4. TestCaseEnd : Raised when test case execution ends.
5. TestHostLaunched : Raised when test host process has been initialized.

```csharp
events.SessionStart += this.SessionStarted_Handler;
events.SessionEnd += this.SessionEnded_Handler;
events.TestCaseStart += this.Events_TestCaseStart;
events.TestCaseEnd += this.Events_TestCaseEnd;
events.TestHostLaunched += this.TestHostLaunched_Handler;
```
```csharp
private void Events_TestCaseStart(object sender, TestCaseStartEventArgs e)
{
}
```
### DataCollectionSink
DataCollectors can create files while handling events and send these files to test runner using `DataCollectionSink`.

```csharp
dataSink.SendFileAsync(context, filename, true);
```

Files sent using above api get associated with session level attachments or test case level attachments based on the context passed.

### DataCollectionEnvironmentContext
DataCollector framework maintains a session level context for test execution session and test level contexts for each test that gets executed.
`DataCollectionEnvironmentContext` passed as argument in constructor has session level context that can be accessed through property `SessionDataCollectionContext`.
Test case level context can be accessed through `TestCaseStartEventArgs.Context` or `TestCaseEndEventArgs.Context`.

```csharp
private void Events_TestCaseStart(object sender, TestCaseStartEventArgs e)
{
    // Session level attachment. environmentContext can be null, so guard before using it.
    if (this.context is not null)
    {
        this.dataSink.SendFileAsync(this.context.SessionDataCollectionContext, filename, true);
    }

    // TestCase level attachment
    this.dataSink.SendFileAsync(e.Context, filename, true);
}
```

### DataCollectionLogger
DataCollectors can also log errors or warnings using `DataCollectionLogger`.
```csharp
// environmentContext can be null, so guard before using the session context.
if (this.context is not null)
{
    logger.LogError(this.context.SessionDataCollectionContext, new Exception("my exception"));
    logger.LogWarning(this.context.SessionDataCollectionContext, "my warning");
}
```

### DataCollection Environment Variables
DataCollectors can choose to specify information about how the test execution environment should be set up by implementing `ITestExecutionEnvironmentSpecifier`.
E.g. setting up the Environment Variables required by profiler engine for code coverage.

```csharp
using System.Collections.Generic;

[DataCollectorFriendlyName("NewDataCollector")]
[DataCollectorTypeUri("my://new/datacollector")]
class NewDataCollector : DataCollector, ITestExecutionEnvironmentSpecifier
{
    public IEnumerable<KeyValuePair<string, string>> GetTestExecutionEnvironmentVariables()
    {
        return new[] { new KeyValuePair<string, string>("MY_PROFILER_SETTING", "1") };
    }
}
```
Environment variables returned by the above method are set in the test execution process while bootstrapping.

## Using DataCollector
Once the DataCollector is compiled, it can be used to monitor test execution. There are two ways by which datacollectors can be plugged in:
1. Using /collect switch :
`vstest.console.exe <TestLibrary> /collect:<DataCollector FriendlyName> /testadapterpath:<Path to test adapter> /testadapterpath:<Path to DataCollector>`

The equivalent `dotnet test` command uses `--collect` and passes adapter paths through runsettings:
`dotnet test <ProjectOrSolution> --collect:"<DataCollector FriendlyName>" -- RunConfiguration.TestAdaptersPaths=<Path to DataCollector>`

2. Using runsettings :
`vstest.console.exe <TestLibrary> /settings:<Path to runsettings file>`

or

`dotnet test <ProjectOrSolution> --settings <Path to runsettings file>`

```xml
<RunSettings>
    <DataCollectionRunSettings>
        <DataCollectors>
            <DataCollector friendlyName="NewDataCollector" />
        </DataCollectors>
    </DataCollectionRunSettings>
</RunSettings>
```
## Results
Attachments sent by DataCollectors will be part of Test Results and can also be viewed in .trx report, if specified.
Logs sent by DataCollectors will be displayed in console logger and can also be viewed in .trx report, if specified.

## Samples
1. We have implemented a sample data collector [here](https://github.com/Microsoft/vstest/tree/main/test/TestAssets/OutOfProcDataCollector).
