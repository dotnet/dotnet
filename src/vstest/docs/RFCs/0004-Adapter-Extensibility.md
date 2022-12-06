# 0004 Adapter Extensibility

## Summary
Allow the test platform to discover/run tests based on a 3rd party test framework. This would be done via an adapter for the corresponding framework that understands how tests are defined in that framework and can run them providing back the test platform with results.

## Motivation
Users are free to write tests based on a test framework of their choice (be it xunit/nunit/mstest etc.). The test platform should be able to run any kind of tests, provided there is a corresponding adapter that understands the framework and can discover/run tests. This provides the users a choice of test framework in unison with the other features of the test platform - Logging, data collection etc.

## Detailed Design

This design will be detailed through the following sections:

1. Specifying an adapter 
2. Test Platform - Adapter interaction
3. Writing an adapter
4. Adapter specific settings

### Specifying an Adapter
Adapters can be provided to the test platform in one of the following ways:

1. "/testadapterpath:<PathToTheAdapter>" This is a switch to vstest.console.exe that feeds in the location of the adapter to the test platform. The PathToAdapter is either the full path to the adapter directory or the relative path to the current directory. For instance:                 

    ```
    /testadapterpath:"C:\Adapters\"
    ```

    This picks up files which have ***.TestAdapter.dll**  in their name from C:\Adapters\ and loads them in as adapters to the test platform. This is done to optimize the number of assemblies the test platform considers as adapters.  

    ```
    C:\Projects\UnitTestProject\bin\debug> vstest.console.exe /testadapterpath:"Adapters"
    ```

    This picks up all files which have ***.TestAdapter.dll** in their name from C:\Projects\UnitTestProject\bin\debug\Adapters and loads them in as adapters to the test platform.

2. Runsettings via "TestAdaptersPaths" node in the RunConfiguration section. Here is a sample on how this can be specified:

    ```xml
    <RunSettings>
      <!-- Configurations that affect the Test Framework -->
      <RunConfiguration>
        <TestAdaptersPaths>C:\Adapters;Adapters;%temp%\Adapters</TestAdaptersPaths>
      </RunConfiguration>
    </RunSettings>
    ```

    One can provide multiple paths that are ';' separated. Paths can be absolute like "C:\Adapters" or relative to the current directory like "Adapters" or can use environment variables like "%temp%\Adapters". Similar to /testadapterpath only files with ***.TestAdapter.dll** in their name from the above locations are fed in as adapters to the test platform. This specification will also work in non-commandline scenarios like VS IDE which can also take in a runsettings.

3. VSIX based adapters. Any vsix extension that has an Asset in the vsixmanifest file with type "UnitTestExtension" is a candidate adapter. This is picked up automatically as an adapter in VS IDE. In vstest.console.exe these adapters are picked up when the '/usevsixextensions' switch is provided like below:

    ```
    vstest.console.exe unittestproject.dll /usevsixextensions:true
    ```

4. Nuget based Adapters in VS IDE. For seamless integration with adapters shipping as nuget packages, in VS IDE we automatically pick up referenced nuget packages that have files which satisfy ***.TestAdapter.dll** in their name and provide these files as adapters to the test platform.

5. Default Adapters in Extensions folder. All the default adapters required by the test platform are packaged along with it in a folder that always gets probed. This folder lies where vstest.console.exe lies and is called the "Extensions" folder. One can get to the location of vstest.console by putting in a "where vstest.console" in a developer command prompt for VS. Any assembly placed in this folder is a candidate adapter. This is however not always recommended since this is specific to a system and involves changing contents in a VS installation folder.

### Test Platform - Adapter interaction
After the adapters are provided to the test platform via one or more of the entry points above, it then loads these adapters in the host process that runs the tests and calls into the adapter via one of the entry points defined in the interfaces detailed in the following sections.

#### Discovery

```csharp
Interface ITestDiscoverer
{
  /// <summary>
  /// Discovers the tests available from the provided container.
  /// </summary>
  /// <param name="containers">Collection of test containers.</param>
  /// <param name="discoveryContext">Context in which discovery is being performed.</param>
  /// <param name="logger">Logger used to log messages.</param>
  /// <param name="discoverySink">Used to send testcases and discovery related events back to Discoverer manager.</param>
  DiscoverTests(IEnumerable<string> containers, IDiscoveryContext discoveryContext,
    IMessageLogger logger, ITestCaseDiscoverySink discoverySink)}
}
```
The test platform iterates through all the loaded adapters and probes each one with the set of containers to get all the test cases defined in them. To optimize this process each adapter can have:
1. FileExtension attribute that informs the test platform what file extension types each adapter is interested in. The following section details this with an example.
2. [Category attribute](0020-Improving-Logic-To-Pass-Sources-To-Adapters.md) that informs the test platform whether adapter supports native or managed assemblies (applicable only when adapters support assemblies i.e. .dll and .exe file extensions).

For a single adapter the test platform calls ITestDiscoverer.DiscoverTests() during Discovery where the test platform provides the containers to discover tests from. The adapter uses the discoverySink instance to report back on test cases that it finds to the test platform. The adapter can also log any status/ warnings via the logger instance passed in and can use the discoveryContext to figure out the settings used for the current session. More on this follows in a later section.  If the adapter finds a test case it understands in a container, it creates a TestCase object and stamps an executor URI on the object. The URI notifies the test platform of the ITestExecutor that can run that test case. This avoids another probing operation during execution.

#### Execution
```csharp
Interface ITestExecutor
{
  /// <summary>
  /// Runs only the tests specified by parameter 'tests'. 
  /// </summary>
  /// <param name="tests">Tests to be run.</param>
  /// <param name="runContext">Context to use when executing the tests.</param>
  /// <param param name="frameworkHandle">Handle to the framework to record results and to do framework operations.</param>
  void RunTests(IEnumerable<TestCase> tests, IRunContext runContext, IFrameworkHandle frameworkHandle);
 
  /// <summary>
  /// Runs 'all' the tests present in the specified 'containers'. 
  /// </summary>
  /// <param name="containers">Path to test container files to look for tests in.</param>
  /// <param name="runContext">Context to use when executing the tests.</param>
  /// <param param name="frameworkHandle">Handle to the framework to record results and to do framework operations.</param>
  void RunTests(IEnumerable<string> containers, IRunContext runContext, IFrameworkHandle frameworkHandle);
 
  /// <summary>
  /// Cancel the execution of the tests.
  /// </summary>
  void Cancel();
}
```
The test platform can call into the adapters with a set of 

1. Containers or 
2. Test cases to execute tests. 

ITestExecutor.RunTests with a set of test cases gets called in mostly VS IDE scenarios("Run Selected tests" scenarios) where a discovery operation has already been performed. This would already have the information as to what ITestExecutor can run the test case via a URI property. The platform would then just call into that specific executor to run the test cases.

ITestExecutor.RunTests with a set of containers gets called when the platform does not already have a set of test cases to work with. This is the case for CLI scenarios like vstest.console.exe where the containers are just specified via an argument. This would also be the case for "Run All" scenarios from VS IDE. The adapter would then usually perform a discovery of all the test cases it understands in the container and run them reporting back the results of the run to the platform via the frameworkHandle instance. Unlike in the ITestExecutor.RunTests with a list of test cases, here the test platform is not aware of the exact executor that would run all the tests in the containers provided. So, it probes through all the adapters to run tests similar to the discovery phase.

The adapter can use the following API's exposed via the IFrameworkHandle interface to report back the status of the test run to the test platform:
```csharp
/// <summary>
/// Notify the platform about the test result.
/// </summary>
/// <param name="testResult">Test Result to be sent to the adapter.</param>
void RecordResult(TestResult testResult);

/// <summary>
/// Notify the platform about starting of the test case. 
/// The platform sends this event to data collectors enabled in the run. 
/// If no data collector is enabled, then the event is ignored. 
/// </summary>
/// <param name="testCase">testcase which will be started.</param>
void RecordStart(TestCase testCase);

/// <summary>
/// Notify the platform about completion of the test case. 
/// The platform sends this event to data collectors enabled in the run. 
/// If no data collector is enabled, then the event is ignored. 
/// </summary>
/// <param name="testCase">testcase which has completed.</param>
/// <param name="outcome">outcome of the test case.</param>
void RecordEnd(TestCase testCase, TestOutcome outcome);

/// <summary>
/// Notify the platform about run level attachments.
/// </summary>
/// <param name="attachmentSets">attachments produced in this run.</param>
void RecordAttachments(IList<AttachmentSet> attachmentSets);
```

An attachment here, could be any file that the adapter generates during the run / a file that the test writer would want attached to a run. This is surfaced to VS IDE as part of the output for a test case. More details on this follows in the "Writing an Adapter" section below.

#### Data Driven tests
Data driven test cases can have multiple test results, one against each data value. In that case adapters would post a RecordStart, RecordEnd and RecordResult for each data value. If the TestResult objects posted for all the data values are associated with the same TestCase object, then the VS IDE client would create a single entry in the Test Explorer with the aggregated outcome along with a summary for each test case in a separate view. If the TestResult objects posted for all the data values are associated with their own TestCase objects with different ID's, then each test result would be different entries in the VS IDE Test Explorer.

#### Filtering
The user might optionally also pass in a test case filter to run a subset of tests. In such cases along with invoking the adapters with the container set, the test platform also provides a callback in the run context instance in the form of [GetTestCaseFilter](./src/Microsoft.TestPlatform.ObjectModel/Adapter/Interfaces/IRunContext.cs#L38) that enables adapters to filter a [TestCase](./src/Microsoft.TestPlatform.ObjectModel/TestCase.cs) object. The adapters can query this API with the set of properties it supports filtering on, along with a provider of TestProperty instances  for each property. From the filter expression returned by this API, the adapter would then just have to perform a [ITestCaseFilterExpression.MatchTestCase](./src/Microsoft.TestPlatform.ObjectModel/Adapter/Interfaces/ITestCaseFilterExpression.cs#L20) on each test case which returns false if the test case has been filtered out. Below is sample code that demonstrates filtering:

```csharp
static readonly TestProperty PriorityProperty = TestProperty.Register(
  "CustomDiscoverer.Priority", "Priority", typeof(int), TestPropertyAttributes.Hidden, typeof(TestCase));

// Test properties supported for filtering 
Dictionary<string, TestProperty> supportedProperties =
  new Dictionary<string, TestProperty>(StringComparer.OrdinalIgnoreCase);

// In the Adapter initialization phase
supportedProperties[PriorityProperty.Label] = PriorityProperty.

// Inside the execute tests method.
var filterExpression = runContext.GetTestCaseFilter(supportedProperties, (propertyName) =>
{
    TestProperty testProperty = null;
    supportedProperties.TryGetValue(propertyName, out testProperty);
    return testProperty;
});

// The adapter can then query if the test case has been filtered out using the following snippet.
if (filterExpression != null &&
    filterExpression.MatchTestCase(currentTest, (propertyName) =>
    {
        var testProperty = supportedProperties.TryGetValue(propertyName, out testProperty);
        return currentTest.GetPropertyValue(testProperty);
    }) == false)
{
    // ...
}
```

#### TestCase Extensibility
Adapters can choose to fill in custom data into the [TestCase](./src/Microsoft.TestPlatform.ObjectModel/TestCase.cs) object through its [Property](./src/Microsoft.TestPlatform.ObjectModel/TestObject.cs#L112) bag. This data can be:

1. If the test case is adorned by a "Do Not Run"  attribute.
2. If the test case is data driven, the values of the data to drive it with.

and so on. During test execution the ITestExecutor can re-use this data to run the test. For instance, it may choose not to run the test in the first example above or feed in the stored data to data drive the test in the second example.

#### Alternate Solutions

1. Adapter extensibility can also be at a process level as opposed to using reflection to load the adapters. As long as the adapter understands the protocol with the client this could be a viable option. However there are two issues with this approach:
    * The adapter would need to maintain multiple processes one for each architecture and framework.
    * The test platform has other extensibility points in the host process that cannot be exposed if the adapter controls the host process unless each adapter initializes these extensions on its own.

### Writing an Adapter

If one needs to write a new adapter, it just needs to implement the two interfaces defined above along with a shipping mechanism so that it can be plugged into one of the entry points to the platform specified earlier. The adapter would have the following:

```csharp
[FileExtension(".xml")]
[DefaultExecutorUri("executor://XmlTestExecutor")]
class TestDiscoverer : ITestDiscoverer
{
    void DiscoverTests(IEnumerable<string> containers, IDiscoveryContext discoveryContext,
      IMessageLogger logger, ITestCaseDiscoverySink discoverySink)
    {
        // Logic to get the tests from the containers passed in.

        //Notify the test platform of the list of test cases found.
        foreach (TestCase test in testsFound)
        {
            discoverySink.SendTestCase(test);
        }
    }
}

[ExtensionUri("executor://XmlTestExecutor")]
class TestExecutor : ITestExecutor
{
    void RunTests(IEnumerable<TestCase> tests, IRunContext runContext, IFrameworkHandle frameworkHandle)
    {
        // Logic to run xml based test cases and report back results.
    }

    void RunTests(IEnumerable<string> containers, IRunContext runContext, IFrameworkHandle frameworkHandle)
    {
        // Logic to discover and run xml based tests and report back results.
    }

    void Cancel()
    {
        // Logic to cancel the current test run.
    }
}
```

As detailed in the earlier section the TestDiscoverer.DiscoverTests gets called by the test platform during discovery which stamps an executor URI on each test case that it reports back, which in the above case is "executor://XmlTestExecutor". The test discoverer can also inform the engine what file types it is interested in via the FileExtension attribute. The platform would then call into the adapter only if the container's extension type falls in the list of supported extension types advertised by the adapter. In the above case the TestDiscoverer only gets called if the container is an xml file. This ensures that the right discoverer is picked up quickly. If a discoverer does not have a FileExtension attribute then it gets probed by the platform for all file extension types.

The TestExecutor above is adorned by a ExtensionUri attribute which advertises the executor URI of this executor. The test platform uses this information to allocate test cases discovered and stamped by a URI in the discovery phase above  to the right executor. Execution, being a long running operation can be cancelled by the test platform via ITestExecutor.Cancel(). The adapter is then expected to stop execution and clean up resources. Any test result that the adapter tries to send back to the test platform after a cancel has been notified would result in a TestCanceledException.

The adapters are also provided with the runsettings xml used for the run via the IDiscoveryContext.RunSettings for discovery and IRunContext.RunSettings for execution. 

### Adapter Specific Settings
The adapters can also incorporate user provided settings via the runsettings XML by adding what is called as a Settings Provider. A settings provider would implement an ISettingsProvider interface and would be called before a discovery/ execution operation with the settings defined for that adapter in the runsettings XML(if any). 

```csharp
interface ISettingsProvider
{
  /// <summary>
  /// Load the settings from the reader.
  /// </summary>
  /// <param name="reader">Reader to load the settings from.</param>
  void Load(XmlReader reader);
}
```
A sample settings provider would look like below:

```csharp
[SettingsName("XmlAdapter")
public class XmlAdapterSettingsProvider : ISettingsProvider
{
  public void Load(XmlReader reader)
  {
    // Read the setting nodes from the XmlReader and save it for a succeeding discovery/execution operation.
  }
}
```

The settings provider will first need to advertise to the test platform that it is interested in only "XmlAdapter" specific settings. It does that via the SettingsName attribute. So whenever the test platform finds a node labelled "XmlAdapter" in the runsettings it first finds a provider that understands that setting and calls into the `Load()` with that xml sub-section.  So if the runsettings is of the following format:

```xml
<RunSettings>
  <RunConfiguration>
    <TargetPlatform>x86</TargetPlatform>
  </RunConfiguration>
  <XmlAdapter>
    <TraceLevel>4</TraceLevel>
    <ShouldPublishDataToFile>true</ShouldPublishDataToFile>
  </XmlAdapter>
</RunSettings>
```

the Load function would be called with the following sub-tree which only contains the settings for XmlAdapter

```xml
<XmlAdapter>
  <TraceLevel>4</TraceLevel>
  <ShouldPublishDataToFile>true</ShouldPublishDataToFile>
</XmlAdapter>
```

## Open questions

1. Should the user need to specify a test adapter path for an adapter to be picked up. Can we automatically figure out adapters that are dropped along with the test assembly? 

2. Currently there is no way to specify to the test platform that it should pick only one specific adapter and ignore the rest. This would save the time taken to probe each adapter to figure out if it can run the tests and make this purely a user driven scenario. Do we want to have this ability?

3. Currently there is a different host process that gets launched for discovery and execution. This does not allow adapters to easily re-use data created/read during discovery in the Run tests phase. The host process cannot be kept alive currently because Core Fx does not have a concept of app domains to load the test dll's in and unload them so that they are not locked. The only way adapters can re-use data across discovery and execution currently is by adding it in a property bag that the TestCase object exposes. Should this be the only way? Can we do something different here?
