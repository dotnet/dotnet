# 0008 Translation Layer for TestPlatform Clients

## Summary
This note gives the details of the APIs for the client library which is an abstraction over TestPlatformV2.
These APIs enable all the test related operations i.e. discovery, execution and debugging.
TranslationLayer takes care of launching the TestPlatform runner, maintaining the connection and communication, sending and bringing back the results.

## Motivation
TranslationLayer is a wrapper over the new TestPlatform V2. TranslationLayer abstracts out all the internal details especially the communication protocol and serialization.
TranslationLayer provides simple APIs that can be invoked to get the desired result.

* Makes it easier for TestPlatform client writers.
* Low maintainence for clients, minimal changes required during releases for TestPlatform.

## Sample
There is a sample project available for reference. Please find the relevant links below.
* Sample project : [https://github.com/Microsoft/vstest/tree/main/samples/Microsoft.TestPlatform.TranslationLayer.E2ETest](https://github.com/Microsoft/vstest/tree/main/samples/Microsoft.TestPlatform.TranslationLayer.E2ETest)
* Nuget package : [https://www.nuget.org/packages/Microsoft.TestPlatform.TranslationLayer](https://www.nuget.org/packages/Microsoft.TestPlatform.TranslationLayer)

## Details
TranslationLayer provides a IVsTestConsoleWrapper interface that contains main APIs for the required operations.
VsTestConsoleWrapper provides the implementation for this interface. Please find following the details on the APIs and usage.

### Constructing VsTestConsoleWrapper
Client needs to create a new instance of the VsTestConsoleWrapper class. 
The full path of the vstest.console.exe needs to passed as the parameter for the Constructor.

```
public VsTestConsoleWrapper(string vstestConsolePath)
```

### Prerequites 
After creating the instance of VsTestConsoleWrapper, we need to call the following APIs as prerequites to the actual operations.

#### Start Session
This call starts the test runner process, creates the communication channel and readies it for handling requests.

```
IVsTestConsoleWrapper :: StartSession()
```

#### Initialize extensions
This call initializes the TestPlatform with paths to extensions like adapters, loggers and any other extensions.

```
IVsTestConsoleWrapper :: void IntializeExtensions(IEnumerable<string> pathtoAdditonalExtensions)
```

### Handlers
There are handlers that are passed via discovery/execution operation calls and are responsible for handling the discovery and execution events.
These handlers are defined under Microsoft.VisualStudio.TestPlatform.ObjectModel.Client namespace in Microsoft.VisualStudio.TestPlatform.ObjectModel assembly.

#### ITestDiscoveryEventsHandler
Interface contract for handling discovery events during test discovery operation.

#### Microsoft.VisualStudio.TestPlatform.ObjectModel.Client :: ITestRunEventHandler
Interface contract for handling run events during test run operation

### Discover Tests

#### IVsTestConsoleWrapper :: DiscoverTests
This api starts the discovery for the given sources and settings. 

#### API
```
void DiscoverTests(IEnumerable<string> sources, string discoverySettings, ITestDiscoveryEventsHandler discoveryEventsHandler);
```            

#### Parameter Details

| Parameters            | Type                        | Description                             |
|-----------------------|-----------------------------|-----------------------------------------|
| sources               | IEnumerable<string>         | Enumerable of paths to testcontainers   |
| discoverySettings     | string                      | Path to the settings file               |
| discoveryEventHandler | ITestDiscoveryEventsHandler | Contract for handling discovery events  |

#### Cancel Discovery
TODO : Place holder for now, not supported yet.

### Run All Tests 

#### IVsTestConsoleWrapper :: RunTests
This api starts the execution for the given sources and settings. 

#### API
```
void RunTests(IEnumerable<string> sources, string runSettings, ITestRunEventsHandler testRunEventsHandler)
```

#### Parameter Details
| Parameters            | Type                        | Description                             |
|-----------------------|-----------------------------|-----------------------------------------|
| sources               | IEnumerable<string>         | Enumerable of paths to testcontainers   |
| runSettings           | string                      | Path to the settings file               |
| testRunEventsHandler  | ITestRunEventsHandler       | Contract for handling execution events  |

### Run Selected Tests

#### IVsTestConsoleWrapper :: RunTests
This api starts the execution for the given sources and settings. 

#### API
```
void RunTests(IEnumerable<TestCase> testCases, string runSettings, ITestRunEventsHandler testRunEventsHandler)
```

#### Parameter Details
| Parameters            | Type                        | Description                             |
|-----------------------|-----------------------------|-----------------------------------------|
| testCases             | IEnumerable<TestCase>       | Enumerable of testcases                 |
| runSettings           | string                      | Path to the settings file               |
| testRunEventsHandler  | ITestRunEventsHandler       | Contract for handling execution events  |

**TestCase** is defined in Microsoft.VisualStudio.TestPlatform.ObjectModel.

#### Cancel Run Test
This call will cancel the last test run request

```
IVsTestConsoleWrapper :: CancelTestRun()
```

#### Abort Run Test
This call will abort the last test run request

```
IVsTestConsoleWrapper :: AbortTestRun()
```

### Cleanup 
After all the operations are done, these are the calls that should be made at the end as part of cleanup.

#### EndSession
This call ends the test session and stops processing requests.

```
IVsTestConsoleWrapper :: EndSession()
```
