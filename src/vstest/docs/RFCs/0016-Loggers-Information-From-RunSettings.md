# 0016 Loggers information from RunSettings

## Summary
This note outlines the proposed changes for:
1. Enabling logger support in test platform for protocol and C# library based clients.
2. Allowing users to provide loggers from the runsettings.

## Motivation
1. Enabling logger support for protocol and C# library based clients will allow clients to use logger extensibility feature.

2. Allowing users to provide loggers from the runsettings enables:
  * Protocol and C# library based clients to provide loggers to the test platform.
  * Configuring loggers as code. Users can commit a runsettings with required loggers and it doesn't need to be specified at each invocation.

## Specifying a logger
Loggers can be provided to the test platform in one of the following ways:

1.  `/Logger:"UriOrFriendlyName";Key1=Value1;Key2=Value2` This is a switch to vstest.console.exe that feeds logger information to the test platform. For instance:

    ```
    /Logger:"logger://Microsoft/TestPlatform/TrxLogger/v1"
    ```

    This loads and initializes logger with "logger://Microsoft/TestPlatform/TrxLogger/v1" as uri.

    ```
    /Logger:sampleLogger;Key1=Value1;Key1=Value2
    ```

    This loads and initializes logger with `sampleLogger` as friendly name. `Key1=Value1` and `Key2=Value2` are passed as dictionary parameters to the logger while initialization.

2. Runsettings via `Logger` node in the `LoggerRunSettings` section. Here is a sample on how this can be specified:

    ```xml
    <RunSettings>
        <LoggerRunSettings>
            <Loggers>
                <Logger friendlyName="sampleLoggerwithParameters">
                    <Configuration>
                        <Key1>Value1</Key1>
                        <Key2>Value2</Key2>
                    </Configuration>
                </Logger>
                <Logger uri="logger://sample/sampleLoggerWithoutParameters1"
                        friendlyName="sampleLoggerWithoutParameters1" />
                <Logger uri="logger://sample/sampleLoggerWithoutParameters2"
                        assemblyQualifiedName="Sample.Sample.Sample.SampleLogger, Sample.Sample.Logger, Version=0.0.0.0, Culture=neutral, PublicKeyToken=xxxxxxxxxxxxxxxx"
                        friendlyName="sampleLoggerWithoutParameters2" />
            </Loggers>
        </LoggerRunSettings>
    </RunSettings>
    ```

    This loads and initializes:
    * Logger with `friendlyName="sampleLoggerwithParameters"`. `Key1=Value1` and `Key2=Value2` are passed as dictionary parameters to the logger while initialization. i.e. `SampleLoggerWithParameters.Initialize(TestLoggerEvents events, Dictionary<string, string> parameters)` is invoked with `parameters = {{"Key1", "Value1"}, {"Key2", "Value2"}}`
    * Logger with `uri="logger://sample/sampleLoggerWithoutParameters1"`. FriendlyName is ignored in this case as uri takes more precedence.
    * Logger with `assemblyQualifiedName="Sample.Sample.Sample.SampleLogger, Sample.Sample.Logger, Version=0.0.0.0, Culture=neutral, PublicKeyToken=xxxxxxxxxxxxxxxx"`. Uri and friendlyName are ignored in this case as assemblyQualifiedName takes more precedence.

Test platform loads custom logger assemblies from test adapter paths and source directory. Check [adapter extensibility] (0004-Adapter-Extensibility.md) to know about how to provide test adapter paths to test platform.

## Specification
1. If same logger is specified both in command line arguments and run settings, command line takes precedence.
2. User can override existing logger value in runsettings from command line. For example `vstest.console foo.dll -- LoggerRunSettings.Loggers="<Logger friendlyName=\"sampleLoggerwithParameters\" />"`.
3. Multiple Loggers can be added in runsettings by adding `Logger` node in `LoggerRunSettings.Loggers` section.
4. Configuration is optional in `Logger` node.
5. Atleast one attribute among `uri`, `friendlyName`, `assemblyQualifiedName` should be present in `Logger` node.
6. If more than one attributes among `uri`, `friendlyName`, `assemblyQualifiedName` are present, then precedence order is `assemblyQualifiedName > uri > friendlyName`. Attributes other than precedent attribute are ignored.
7. Logger can be enabled or disabled using `enabled` attribute in `Logger` node. For example to disable a logger: `<Logger friendlyName="sampleLogger" enabled="false" />`.

## Error and Warning scenarios:
Exception scenarios: 
In case of exception, test run is aborted. Following are the exception scenarios:
1. `LoggerRunSettings` or `Loggers` node has attributes.
2. `LoggerRunSettings` has any node other than `Loggers`.
3. `Loggers` has any node other than `Logger`.
4. `Logger` has any node other than `Configuration`.
5. `Logger` node has any attribute other than `uri`, `assemblyQualifiedName`, `friendlyName`, or `enabled`.
6. Invalid format `uri` is given.
7. Unable to find logger using precedent attribute.
8. If none of the attributes `uri`, `assemblyQualifiedName`, `friendlyName` is given.

Warning scenarios:
1. Key value pair has empty or whitespace value. Example: `<Key1></Key1>`.

Ignored scenarios:
1. Duplicate keys in `Configuration` node is ignored and verbose is added for it.

## Design
Following are the proposed design changes for enabling logger support for protocol and C# library based clients:
1. TestLoggerManager will manage all the loggers. TestLoggerManager will be responsible for:
  * Initializing loggers from the run settings.
  * Enabling and triggering logger events.
  * Disposing logger events.

2. TestEngine API will have GetLoggerManager method which will return new TestLoggerManager instance on every call.
3. TestPlatform will get instance from TestEngine and initialize TestLoggerManager and will pass this TestLoggerManager instance to TestRunRequest and DiscoveryRequest.
4. TestRunRequest and DiscoveryRequest will hold the TestLoggerManager instance. On receiving any run/discovery events, respective methods of TestLoggerManager will be invoked. i.e. TestLoggerManager will no longer register logger events with run or discovery events.
5. On test run/discovery completion, TestLoggerManager will be disposed by TestRunRequest and DiscoveryRequest.
6. Limitation with this approach is that if TestRunRequest users like TestRequestManager tries to call `TestRunRequest.ExecuteAsync()` second time, then logger events will not be invoked as TestLoggerManager will already be disposed in first run.
