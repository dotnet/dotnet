# 0015 Telemetry

## Summary
This document outlines the telemetry data points to be collected by vstest platform along with the expectations from any vstest consuming platform, wanting to use these telemetry data points for their own needs.

## Overview
Going forward vstest platform will enable collection of rich telemetry data points to helps us and any vstest consuming platform in making the right choices to improve end user experience. The telemetry data points we plan to collect in vstest, doesn't contain any information that can be used to identify an individual e.g. name, email, username etc. More details on telemetry data points below.

## Data Points
Following telemetry data points will be collected by vstest and also enabled for collection by any vstest consuming platform:
* Size of tests (e.g. numberOfTestsSources, numberOfTestsRun, duration etc.)
* Configuration of tests (e.g. parallelExecution, dataCollectorUsed, adapterVersion etc.)
* Composition of tests (e.g. targetOS, targetPlatform, targetFramework etc.)
* Features used (e.g. codeCoverage, settings etc.)
* Time (time of operation)
* Source of invocation (vstest consuming platform)
For more details please refer to the **Detailed List of Data Points** section at the end

## Conceptual Flow
![alt text](./RFCs/Images/vstest.telemetry.png) 

## Scope
* vstest and vstest.console.exe will emit telemetry data points that any vstest consuming platform can listen to
* vstest consuming platform needs to provide the following information to the vstest
  * indicator of user consent
  * source of invocation
* Consuming platform needs to create a listener that can collect the data points and route it to their respective telemetry backend
* No PII or EUII data will be collected by vstest
* vstest will respect the user consent flowing in from the consuming platform

## User Consent [Opt-In, Opt-Out]
Any vstest consuming platform can collect the telemetry events and redirect to the backend of their choice, however it is the responsibility of the consuming platform to
* Enable opt-in | opt-out experience for their users
* Ensure compliance (data, security, privacy etc.)

## License
The Microsoft distribution of vstest is licensed with the [Link](https://www.visualstudio.com/microsoft-visual-studio-test-platform/). This license includes the "DATA" section to enable telemetry (shown below).
* **DATA COLLECTION.** The software may collect information about you and your use of the software, and send that to Microsoft. Microsoft may use this information to provide services and improve our products and services.  You may opt-out of many of these scenarios, but not all, as described in the product documentation.  There are also some features in the software that may enable you and Microsoft to collect data from users of your applications. If you use these features, you must comply with applicable law, including providing appropriate notices to users of your applications together with a copy of Microsoft’s privacy statement. Our privacy statement is located at http://go.microsoft.com/fwlink/?LinkID=528096. You can learn more about data collection and use in the help documentation and the privacy statement. Your use of the software operates as your consent to these practices.

* **Processing of Personal Data.** To the extent Microsoft is a processor or subprocessor of personal data in connection with the software, Microsoft makes the commitments in the European Union General Data Protection Regulation Terms of the Online Services Terms to all customers effective May 25, 2018, at http://go.microsoft.com/?linkid=9840733. 

## Detailed List of Data Points
| Group          | Attributes      |
|----------------|-----------------|
| Size           | numberOfTestsSources |
|                | numberOfTestsRun |
|                | duration |
|                | discoveryState |
|                | totalTests (Discovered) |
|                | totalTimeTakenInSeconds (Discovered) |
|                | duration |
|                | totalTests (Executed) |
|                | totalTimeTakenInSeconds (Executed) |
| Composition    | adapterVersion |
|                | targetFramework |
|                | targetPlatform |
|                | targetDevice (UWP or Not) |
|                | testPlatformVersion |
|                | targetOS |
| Configuration  | loggerUsed |
|                | dataCollectorUsed |
|                | parallelExecution |
|                | maxCpuCount |
|                | adapterUsedCount (Discovered) |
|                | adapterUsedCount (Executed) |
|                | platform (commandLineSwitches) |
|                | framework (commandLineSwitches) |
| Features       | setting (commandLineSwitches) |
|                | parallel (commandLineSwitches) |
|                | enableCodeCoverage (commandLineSwitches) |
|                | inIsolation (commandLineSwitches)|
|                | useVSIXExtensions (commandLineSwitches) |
|                | logger (commandLineSwitches) |
| Time           | dateTime |
| Source         | consumingPlatform |

## Sending Consent for collecting Telemetry Metrics in Test Platform in Design Mode
**Collection in Test Platform will only happen if TestPlatform receives consent from the consumers.**

The "CollectMetrics" field has been added in [TestPlatformOptions](./src/Microsoft.TestPlatform.ObjectModel/Client/TestPlatformOptions.cs) to send consent from Design Mode Scenarios(VS, VSCode etc) to TestPlatform. The users have to set "CollectMetrics" to "true" so that TestPlatform can collect Metrics and send it back to the client.

The [IVsTestConsoleWrapper](./src/Microsoft.TestPlatform.VsTestConsole.TranslationLayer/Interfaces/IVsTestConsoleWrapper.cs) and [IVsTestConsoleWrapperAsync](./src/Microsoft.TestPlatform.VsTestConsole.TranslationLayer/Interfaces/IVsTestConsoleWrapperAsync.cs) contains API's that support TestPlatformOptions. Users must use these API's to send consent to the TestPlatform for collecting Metrics.

**For Instance**
For Discovery : Users have to use this API to pass TestPlatform Options.

        /// <summary>
        /// Start Discover Tests for the given sources and discovery settings.
        /// </summary>
        /// <param name="sources">List of source assemblies, files to discover tests</param>
        /// <param name="discoverySettings">Settings XML for test discovery</param>
        /// <param name="options">Options to be passed into the platform.</param>
        /// <param name="discoveryEventsHandler">EventHandler to receive discovery events</param>
        void DiscoverTests(IEnumerable<string> sources, string discoverySettings, TestPlatformOptions options, ITestDiscoveryEventsHandler2 discoveryEventsHandler);
        
For Execution using sources: Users have to use this API to pass TestPlatform Options.

        /// <summary>
        /// Starts a test run given a list of sources.
        /// </summary>
        /// <param name="sources">Sources to Run tests on</param>
        /// <param name="runSettings">RunSettings XML to run the tests</param>
        /// <param name="options">Options to be passed into the platform.</param>
        /// <param name="testRunEventsHandler">EventHandler to receive test run events</param>
        void RunTests(IEnumerable<string> sources, string runSettings, TestPlatformOptions options, ITestRunEventsHandler testRunEventsHandler);

Similarly, while running tests using TestCases, there are API's available to send TestPlatformOptions along with them.

## Consuming Metrics in Design Mode Scenarios from Test Platform
The whole aggregated metrics will be appended in vstest.console process in the final Execution/Disocvery Complete message and it will be send back to the consumers. When users use API's that are available in [IVSTestConsoleWrapper](./src/Microsoft.TestPlatform.VsTestConsole.TranslationLayer/Interfaces/IVsTestConsoleWrapper.cs), they have to pass the event handler which will contain metrics from TestPlatform.

In Case of Execution, users will pass [ITestRunEventsHandler](./src/Microsoft.TestPlatform.ObjectModel/Client/Interfaces/ITestRunEventsHandler.cs), which contains [TestRunCompleteEventArgs](./src/Microsoft.TestPlatform.ObjectModel/Client/Events/TestRunCompleteEventArgs.cs) which will have Metrics in it which users can consume.

In Case of Discovery, users have to pass new interface [ITestDiscoveryEventsHandler2](./src/Microsoft.TestPlatform.ObjectModel/Client/Interfaces/ITestDiscoveryEventsHandler2.cs) which contains [DiscoveryCompleteEventArgs](./src/Microsoft.TestPlatform.ObjectModel/Client/Events/DiscoveryCompleteEventArgs.cs) which will have metrics in it which users can consume.

## Telemetry Design
### Collecting Data
* Collect Telemetry Data Points in various process(testhost,datacollector etc) and send it to vstest.console process where it will be uploaded or send back to users in design mode. 

**For eg:**
* In Test Host Process:
We have to collect how much time does each executor took to run test, Time taken to load adapters etc.

* In Vstest.console:
We have to collect Total Discovery Time taken, Total Tests Run in case of parallel scenarios.

#### Aggregating data points in Test Host process

* It may happen that TestHost is on a newer version whereas vstest.console is on older version. So, TestHost process should not collect Metrics by default. So, it should only collect Metrics when users give consent to collect Metrics.
So, for sending consent from Vstest.console process to Test Host, Command line argument i.e. boolean **TelemetryOptedIn** is sent to TestHost Process from Vstest.console process.

The interface [IRequestData](./src/Microsoft.TestPlatform.ObjectModel/Client/Interfaces/IRequestData.cs) has been has been exposed to TestHost process which contains [IMetricCollection](./src/Microsoft.TestPlatform.ObjectModel/Client/Interfaces/IMetricsCollection.cs) which will collect the Metrics in a dictionary.

#### Sending Metrics from TestHost process to vstest.console process
Currently, At the end of Discovery Complete, we are sending **TestDiscovery.Complete** message event along with DiscoveryCompletePayload, so we will add our collected Metrics along with [DiscoveryCompletePayload](./src/Microsoft.TestPlatform.CommunicationUtilities/Messages/DiscoveryCompletePayload.cs). Similar will be done at end of **TestExecution.Complete** event where we will add the Metrics in [TestRunCompleteEventArgs](./src/Microsoft.TestPlatform.ObjectModel/Client/Events/TestRunCompleteEventArgs.cs). This helps us in sending whole metrics in one go and helps us to decrease performance overhead of sending messages from test host process to vstest.console process.

In VsTestConsole process, the metrics which will be received from TestHost process will be aggregated with the VsTestConsole own metrics.

### Publishing Data

For Publishing data, there are two scenarios:
* Design Mode: We aggregate all the data in vstest.console and send to various IDE's(VS, VSCode etc) giving the IDE's options to add this telemetry data along with thier own Telemetry.

* Non-Design Mode: We will be extending support for it soon.
