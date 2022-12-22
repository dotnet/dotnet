# 0007 Editors API Specification

## Summary
This note outlines the JSON protcol between an Editor/IDE and the test platform.
It is an addendum to [Discovery Protocol][discovery] and [Execution
Protocol][execution] documents.

## Terminology
* **Editor** is an application which wants to discover, run tests from a test
  assembly. It has intimate knowledge of the test project.
* **Test Platform** is the runner and engine which knows how to discover/run
  tests.
* **Request** is a message from the Editor to Test Platform.
* **Response** is a message from the Test Platform to Editor.

## SDK Implementations
`Microsoft.TestPlatform.TranslationLayer` provides a .net implementation of this
API. TODO: Link to sdk doc.

## Overview
### Bootstrap
Editor should launch the test platform with its process id and a port that test
platform can connect to. It will start console runner as follows:

```
> dotnet vstest --ParentProcessId <pid> --Port <port>
```

The windows only console runner `vstest.console.exe` also supports the same
parameters. The test platform tries to connect to `<port>` and sends a
`connection` message.

### Message
All the communication messages between an Editor and Test Platform have
following structure:

#### API Payload
| Key         | Type   | Description                                            |
|-------------|--------|--------------------------------------------------------|
| MessageType | string | Type of message                                        |
| Payload     | object | Payload for an operation input or output. Can be null. |

#### Example
```json
{
    "MessageType": "TestSession.Connected",
    "Payload": null
}
```

The `Payload` provides data specific to an request or response. It may be null
for `connection` and `version` messsages.

### Connection (Response)
The test platform sends a connection message after launch.

#### API Payload
| Key         | Type   | Description           |
|-------------|--------|-----------------------|
| MessageType | string | TestSession.Connected |
| Payload     | object | null                  |

#### Example
```json
{
    "MessageType": "TestSession.Connected",
    "Payload": null
}
```

### Version Operation
An Editor can use the version operation to check the protocol version supported
by the available test platform. It may modify the protocol for further
communication if needed.

#### Version (Request)
A version request from Editor to Test Platform has following structure.

##### API Payload
| Key         | Type   | Description     |
|-------------|--------|-----------------|
| MessageType | string | ProtocolVersion |
| Payload     | object | null            |

##### Example
```json
{
    "MessageType": "ProtocolVersion",
    "Payload": null
}
```

#### Version (Response)
A version response from Test Platform has following structure.

##### API Payload
| Key         | Type   | Description     |
|-------------|--------|-----------------|
| MessageType | string | ProtocolVersion |
| Payload     | object | 1               |

##### Example
```json
{
    "MessageType": "ProtocolVersion",
    "Payload": null
}
```

At this point the initial handshake between Editor and Test Platform is
complete. Above operation is required only once per launch of test platform.
Subsequent discovery, execution operation don't require an initialization.

### Initialize Extensions (Request)
This request ensures that all the extensions that are required for discovery or execution
get loaded before the actual operation request.
While running test targeting .net4x framework, this is required for initializing the test adapters.

#### API Payload
| Key         | Type   | Description                 |
|-------------|--------|-----------------------------|
| MessageType | string | Extensions.Initialize       |
| Payload     | array  | List of paths of extensions |

#### Example
```json
{
  "MessageType": "Extensions.Initialize",
  "Payload": [
    "E:\\UnitTest1\\UnitTest1\\bin\\Debug\\net452\\Microsoft.VisualStudio.TestPlatform.MSTest.TestAdapter.dll"
  ]
}
```

## Discover Tests
A discovery operation requests the test platform to load the test container, use
the appropriate test adapter and list all tests available within the container.

Various `Message` types supported during discovery operation are:
* TestDiscovery.Start
* TestDiscovery.TestFound
* TestDiscovery.Completed

### Start Discovery (Request)
An Editor triggers the discovery operation with a `TestDiscovery.Start` message.

#### API Payload
| Key         | Type   | Description         |
|-------------|--------|---------------------|
| MessageType | string | TestDiscovery.Start |
| Payload     | object | See below           |

##### Message Payload
| Key         | Type   | Description                    |
|-------------|--------|--------------------------------|
| Sources     | array  | Array of test container paths. |
| RunSettings | string | Run settings xml               |

#### Example
```json
{
  "MessageType": "TestDiscovery.Start",
  "Payload": {
    "Sources": [
      ".\\samples\\UnitTestProject\\bin\\Debug\\netcoreapp1.0\\UnitTestProject.dll"
    ],
    "RunSettings": null
  }
}
```

Refer [msdn](https://msdn.microsoft.com/en-us/library/jj635153.aspx#Anchor_1) for run settings sample.

### Tests Found (Response)
The test platform sends tests found messages for the tests discovered. There can
be one or many such messages. Editor is expected to listen to such messages
until a `TestDiscovery.Completed` message is received.

#### API Payload
| Key         | Type   | Description                       |
|-------------|--------|-----------------------------------|
| MessageType | string | TestDiscovery.TestFound           |
| Payload     | array  | See below for details of Property |

For `TestDiscovery.TestFound` message, the Payload is an array of `TestCase`
objects. See [appendix](#DataStructure.TestCase) for details.

#### Example
```json
{
  "MessageType": "TestDiscovery.TestFound",
  "Payload": [
    {
      "Properties": [
        {
          "Key": {
            "Id": "TestCase.FullyQualifiedName",
            "Label": "FullyQualifiedName",
            "Category": "",
            "Description": "",
            "Attributes": 1,
            "ValueType": "System.String"
          },
          "Value": "UnitTestProject.UnitTest.PassingTest"
        },
        {
          "Key": {
            "Id": "TestCase.ExecutorUri",
            "Label": "Executor Uri",
            "Category": "",
            "Description": "",
            "Attributes": 1,
            "ValueType": "System.Uri"
          },
          "Value": "executor://MSTestAdapter/v2"
        },
        {
          "Key": {
            "Id": "TestCase.Source",
            "Label": "Source",
            "Category": "",
            "Description": "",
            "Attributes": 0,
            "ValueType": "System.String"
          },
          "Value": ".\\samples\\UnitTestProject\\bin\\Debug\\netcoreapp1.0\\UnitTestProject.dll"
        },
        {
          "Key": {
            "Id": "TestCase.DisplayName",
            "Label": "Name",
            "Category": "",
            "Description": "",
            "Attributes": 0,
            "ValueType": "System.String"
          },
          "Value": "PassingTest"
        },
        {
          "Key": {
            "Id": "MSTestDiscovererv2.IsEnabled",
            "Label": "IsEnabled",
            "Category": "",
            "Description": "",
            "Attributes": 1,
            "ValueType": "System.Boolean"
          },
          "Value": true
        },
        {
          "Key": {
            "Id": "MSTestDiscovererv2.TestClassName",
            "Label": "ClassName",
            "Category": "",
            "Description": "",
            "Attributes": 1,
            "ValueType": "System.String"
          },
          "Value": "UnitTestProject.UnitTest"
        },
        {
          "Key": {
            "Id": "TestObject.Traits",
            "Label": "Traits",
            "Category": "",
            "Description": "",
            "Attributes": 5,
            "ValueType": "System.Collections.Generic.KeyValuePair`2[[System.String],[System.String]][]"
          },
          "Value": []
        }
      ]
    },
    {
      "Properties": [
        {
          "Key": {
            "Id": "TestCase.FullyQualifiedName",
            "Label": "FullyQualifiedName",
            "Category": "",
            "Description": "",
            "Attributes": 1,
            "ValueType": "System.String"
          },
          "Value": "UnitTestProject.UnitTest.TestWithPriority"
        },
        {
          "Key": {
            "Id": "TestCase.ExecutorUri",
            "Label": "Executor Uri",
            "Category": "",
            "Description": "",
            "Attributes": 1,
            "ValueType": "System.Uri"
          },
          "Value": "executor://MSTestAdapter/v2"
        },
        {
          "Key": {
            "Id": "TestCase.Source",
            "Label": "Source",
            "Category": "",
            "Description": "",
            "Attributes": 0,
            "ValueType": "System.String"
          },
          "Value": ".\\samples\\UnitTestProject\\bin\\Debug\\netcoreapp1.0\\UnitTestProject.dll"
        },
        {
          "Key": {
            "Id": "TestCase.DisplayName",
            "Label": "Name",
            "Category": "",
            "Description": "",
            "Attributes": 0,
            "ValueType": "System.String"
          },
          "Value": "TestWithPriority"
        },
        {
          "Key": {
            "Id": "MSTestDiscovererv2.IsEnabled",
            "Label": "IsEnabled",
            "Category": "",
            "Description": "",
            "Attributes": 1,
            "ValueType": "System.Boolean"
          },
          "Value": true
        },
        {
          "Key": {
            "Id": "MSTestDiscovererv2.TestClassName",
            "Label": "ClassName",
            "Category": "",
            "Description": "",
            "Attributes": 1,
            "ValueType": "System.String"
          },
          "Value": "UnitTestProject.UnitTest"
        },
        {
          "Key": {
            "Id": "MSTestDiscovererv2.Priority",
            "Label": "Priority",
            "Category": "",
            "Description": "",
            "Attributes": 1,
            "ValueType": "System.Int32"
          },
          "Value": 0
        },
        {
          "Key": {
            "Id": "TestObject.Traits",
            "Label": "Traits",
            "Category": "",
            "Description": "",
            "Attributes": 5,
            "ValueType": "System.Collections.Generic.KeyValuePair`2[[System.String],[System.String]][]"
          },
          "Value": [
            {
              "Key": "Priority",
              "Value": "0"
            }
          ]
        }
      ]
    }
  ]
}
```

### Discovery Complete (Response)
A discovery complete message from Test Platform marks the end of discovery process.

#### API Payload
| Key         | Type   | Description             |
|-------------|--------|-------------------------|
| MessageType | string | TestDiscovery.Completed |
| Payload     | object | See below for details   |

**Payload** for `TestDiscovery.Completed` has following structure.

| Key                 | Type    | Description                                     |
|---------------------|---------|-------------------------------------------------|
| TotalTests          | number  | Number of tests discovered                      |
| LastDiscoveredTests | array   | Set of `TestCase` objects in the final chunk    |
| IsAborted           | boolean | `true` indicates an aborted discovery operation |

#### Example
```json
{
  "MessageType": "TestDiscovery.Completed",
  "Payload": {
    "TotalTests": 7,
    "LastDiscoveredTests": null,
    "IsAborted": false
  }
}
```

## Run Tests
### Run Tests (Request)
A run tests request will trigger test execution for a given test container.

#### API Payload
| Key         | Type   | Description                         |
|-------------|--------|-------------------------------------|
| MessageType | string | TestExecution.RunAllWithDefaultHost |
| Payload     | object | See below for details               |

**Payload** for `TestExecution.RunAllWithDefaultHost` has following structure.

| Key              | Type    | Description                                |
|------------------|---------|--------------------------------------------|
| Sources          | array   | Set of test containers                     |
| TestCases        | array   | Set of `TestCase` objects                  |
| RunSettings      | string  | Run settings for the test run              |
| KeepAlive        | boolean | Reserved for future                        |
| DebuggingEnabled | boolean | `true` indicates a test run under debugger |

#### Example
```json
{
  "MessageType": "TestExecution.RunAllWithDefaultHost",
  "Payload": {
    "Sources": [
      ".\\samples\\UnitTestProject\\bin\\Debug\\netcoreapp1.0\\UnitTestProject.dll"
    ],
    "TestCases": null,
    "RunSettings": null,
    "KeepAlive": false,
    "DebuggingEnabled": false
  }
}
```

TargetFramework for the tests can be set using the runsettings.
See [appendix](#RunSettings.TargetFramework) for details.

### Test Run Statistics (Response)
Test results are provided by test platform in batches. An editor should continue
to listen to `TestExecution.StatsChange` messages until a
`TestExecution.Completed` message is received.

#### API Payload
| Key         | Type   | Description               |
|-------------|--------|---------------------------|
| MessageType | string | TestExecution.StatsChange |
| Payload     | object | See details below         |

**Payload** object has following structure.

| Key               | Type   | Description                                      |
|-------------------|--------|--------------------------------------------------|
| NewTestResults    | array  | Set of `TestResult` objects                      |
| TestRunStatistics | object | See details below                                |
| ActiveTests       | array  | Set of active or inprogress `TestCase` objects   |

**TestRunStatistics** object has following structure

| Key             | Type   | Description                             |
|-----------------|--------|-----------------------------------------|
| ExecutedTests   | number | Total number test executed              |
| Stats           | object | Map for count of tests for each outcome |

Details of a `TestResult` object is available in
[appendix](#4-testresult).

Details of a `TestCase` object is available in
[appendix](#3-testcase).

#### Example
```json
{
  "MessageType": "TestExecution.StatsChange",
  "Payload": {
    "NewTestResults": [
      {
        "TestCase": {
          "Properties": [
            {
              "Key": {
                "Id": "TestCase.FullyQualifiedName",
                "Label": "FullyQualifiedName",
                "Category": "",
                "Description": "",
                "Attributes": 1,
                "ValueType": "System.String"
              },
              "Value": "UnitTestProject.UnitTest.FailingTest"
            },
            {
              "Key": {
                "Id": "TestCase.ExecutorUri",
                "Label": "Executor Uri",
                "Category": "",
                "Description": "",
                "Attributes": 1,
                "ValueType": "System.Uri"
              },
              "Value": "executor://MSTestAdapter/v2"
            },
            {
              "Key": {
                "Id": "TestCase.Source",
                "Label": "Source",
                "Category": "",
                "Description": "",
                "Attributes": 0,
                "ValueType": "System.String"
              },
              "Value": "UnitTestProject\\bin\\Debug\\netcoreapp1.0\\UnitTestProject.dll"
            },
            {
              "Key": {
                "Id": "TestCase.DisplayName",
                "Label": "Name",
                "Category": "",
                "Description": "",
                "Attributes": 0,
                "ValueType": "System.String"
              },
              "Value": "FailingTest"
            },
            {
              "Key": {
                "Id": "MSTestDiscovererv2.IsEnabled",
                "Label": "IsEnabled",
                "Category": "",
                "Description": "",
                "Attributes": 1,
                "ValueType": "System.Boolean"
              },
              "Value": true
            },
            {
              "Key": {
                "Id": "MSTestDiscovererv2.TestClassName",
                "Label": "ClassName",
                "Category": "",
                "Description": "",
                "Attributes": 1,
                "ValueType": "System.String"
              },
              "Value": "UnitTestProject.UnitTest"
            },
            {
              "Key": {
                "Id": "TestObject.Traits",
                "Label": "Traits",
                "Category": "",
                "Description": "",
                "Attributes": 5,
                "ValueType": "System.Collections.Generic.KeyValuePair`2[[System.String],[System.String]][]"
              },
              "Value": []
            },
            {
              "Key": {
                "Id": "TestCase.LineNumber",
                "Label": "Line Number",
                "Category": "",
                "Description": "",
                "Attributes": 1,
                "ValueType": "System.Int32"
              },
              "Value": 25
            },
            {
              "Key": {
                "Id": "TestCase.CodeFilePath",
                "Label": "File Path",
                "Category": "",
                "Description": "",
                "Attributes": 0,
                "ValueType": "System.String"
              },
              "Value": "UnitTestProject\\UnitTest.cs"
            }
          ]
        },
        "Attachments": [],
        "Messages": [],
        "Properties": [
          {
            "Key": {
              "Id": "TestResult.DisplayName",
              "Label": "TestResult Display Name",
              "Category": "",
              "Description": "",
              "Attributes": 1,
              "ValueType": "System.String"
            },
            "Value": null
          },
          {
            "Key": {
              "Id": "TestResult.Duration",
              "Label": "Duration",
              "Category": "",
              "Description": "",
              "Attributes": 0,
              "ValueType": "System.TimeSpan"
            },
            "Value": "00:00:10.0941957"
          },
          {
            "Key": {
              "Id": "TestResult.ErrorMessage",
              "Label": "Error Message",
              "Category": "",
              "Description": "",
              "Attributes": 0,
              "ValueType": "System.String"
            },
            "Value": "Assert.AreEqual failed. Expected:<2>. Actual:<3>. "
          },
          {
            "Key": {
              "Id": "TestResult.ErrorStackTrace",
              "Label": "Error Stack Trace",
              "Category": "",
              "Description": "",
              "Attributes": 0,
              "ValueType": "System.String"
            },
            "Value": "   at UnitTestProject.UnitTest.FailingTest() in UnitTestProject\\UnitTest.cs:line 27\r\n"
          },
          {
            "Key": {
              "Id": "TestResult.Outcome",
              "Label": "Outcome",
              "Category": "",
              "Description": "",
              "Attributes": 0,
              "ValueType": "Microsoft.VisualStudio.TestPlatform.ObjectModel.TestOutcome, Microsoft.VisualStudio.TestPlatform.ObjectModel, Version=15.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
            },
            "Value": 2
          },
          {
            "Key": {
              "Id": "TestResult.StartTime",
              "Label": "Start Time",
              "Category": "",
              "Description": "",
              "Attributes": 0,
              "ValueType": "System.DateTimeOffset"
            },
            "Value": "2017-01-25T16:22:42.0912112+05:30"
          },
          {
            "Key": {
              "Id": "TestResult.EndTime",
              "Label": "End Time",
              "Category": "",
              "Description": "",
              "Attributes": 0,
              "ValueType": "System.DateTimeOffset"
            },
            "Value": "2017-01-25T16:22:52.1862902+05:30"
          }
        ]
      }
    ],
    "TestRunStatistics": {
      "ExecutedTests": 2,
      "Stats": {
        "Passed": 1,
        "Failed": 1
      }
    },
    "ActiveTests": [
      {
        "Properties": [
          {
            "Key": {
              "Id": "TestCase.FullyQualifiedName",
              "Label": "FullyQualifiedName",
              "Category": "",
              "Description": "",
              "Attributes": 1,
              "ValueType": "System.String"
            },
            "Value": "UnitTestProject.UnitTest.SkippingTest"
          },
          {
            "Key": {
              "Id": "TestCase.ExecutorUri",
              "Label": "Executor Uri",
              "Category": "",
              "Description": "",
              "Attributes": 1,
              "ValueType": "System.Uri"
            },
            "Value": "executor://MSTestAdapter/v2"
          },
          {
            "Key": {
              "Id": "TestCase.Source",
              "Label": "Source",
              "Category": "",
              "Description": "",
              "Attributes": 0,
              "ValueType": "System.String"
            },
            "Value": "UnitTestProject\\bin\\Debug\\netcoreapp1.0\\UnitTestProject.dll"
          },
          {
            "Key": {
              "Id": "TestCase.DisplayName",
              "Label": "Name",
              "Category": "",
              "Description": "",
              "Attributes": 0,
              "ValueType": "System.String"
            },
            "Value": "SkippingTest"
          },
          {
            "Key": {
              "Id": "MSTestDiscovererv2.IsEnabled",
              "Label": "IsEnabled",
              "Category": "",
              "Description": "",
              "Attributes": 1,
              "ValueType": "System.Boolean"
            },
            "Value": false
          },
          {
            "Key": {
              "Id": "MSTestDiscovererv2.TestClassName",
              "Label": "ClassName",
              "Category": "",
              "Description": "",
              "Attributes": 1,
              "ValueType": "System.String"
            },
            "Value": "UnitTestProject.UnitTest"
          },
          {
            "Key": {
              "Id": "TestObject.Traits",
              "Label": "Traits",
              "Category": "",
              "Description": "",
              "Attributes": 5,
              "ValueType": "System.Collections.Generic.KeyValuePair`2[[System.String],[System.String]][]"
            },
            "Value": []
          },
          {
            "Key": {
              "Id": "TestCase.LineNumber",
              "Label": "Line Number",
              "Category": "",
              "Description": "",
              "Attributes": 1,
              "ValueType": "System.Int32"
            },
            "Value": 36
          },
          {
            "Key": {
              "Id": "TestCase.CodeFilePath",
              "Label": "File Path",
              "Category": "",
              "Description": "",
              "Attributes": 0,
              "ValueType": "System.String"
            },
            "Value": "UnitTestProject\\UnitTest.cs"
          }
        ]
      }
    ]
  }
}
```

### Test Run Complete (Response)
A `TestExecution.Completed` message indicates completion of a test run.

#### API Payload
| Key         | Type   | Description             |
|-------------|--------|-------------------------|
| MessageType | string | TestExecution.Completed |
| Payload     | object | See details below       |

**Payload** object has following structure.

| Key                 | Type   | Description                                                   |
|---------------------|--------|---------------------------------------------------------------|
| TestRunCompleteArgs | object | Summary of test run. See below                                |
| LastRunTests        | array  | Set of `TestResult` for the last results batch                |
| RunAttachments      | array  | Test attachments for the run                                  |
| ExecutorUris        | array  | Set of executor uri for adapters that participated in the run |

**TestRunCompleteArgs** object has following structure.

| Key                                   | Type    | Description                     |
|---------------------------------------|---------|---------------------------------|
| TestRunStatistics["ExecutedTests"]    | number  | Total tests executed            |
| TestRunStatistics["Stats"]["Passed"]  | number  | Passed tests count              |
| TestRunStatistics["Stats"]["Failed"]  | number  | Failed tests count              |
| TestRunStatistics["Stats"]["Skipped"] | number  | Skipped tests count             |
| IsCanceled                            | boolean | `true` indicates a canceled run |
| IsAborted                             | boolean | `true` indicates aborted run    |
| Error                                 | string  | Error during the run            |
| AttachmentSets                        | array   | Array of run attachments        |
| ElapsedTimeInRunningTests             | string  | Duration for test run           |

#### Example
```json
{
  "MessageType": "TestExecution.Completed",
  "Payload": {
    "TestRunCompleteArgs": {
      "TestRunStatistics": {
        "ExecutedTests": 7,
        "Stats": {
          "Passed": 4,
          "Failed": 2,
          "Skipped": 1
        }
      },
      "IsCanceled": false,
      "IsAborted": false,
      "Error": null,
      "AttachmentSets": [],
      "ElapsedTimeInRunningTests": "00:00:00.1820000"
    },
    "LastRunTests": null,
    "RunAttachments": [],
    "ExecutorUris": [
      "executor://mstestadapter/v2"
    ]
  }
}
```

## Run Selected Tests
Run selected tests operation differs from Run All tests in the request. The
responses from test platform are same as above.

### Test Run With Filter (Request)
#### API Payload

| Key         | Type   | Description                         |
|-------------|--------|-------------------------------------|
| MessageType | string | TestExecution.RunSelectedWithDefaultHost |
| Payload     | object | See below for details               |

**Payload** for `TestExecution.RunSelectedWithDefaultHost` has following structure.

| Key              | Type    | Description                                |
|------------------|---------|--------------------------------------------|
| Sources          | array   | Set of test containers. Null in this case  |
| TestCases        | array   | Set of `TestCase` objects. *Required*.     |
| RunSettings      | string  | Run settings for the test run              |
| KeepAlive        | boolean | Reserved for future                        |
| DebuggingEnabled | boolean | `true` indicates a test run under debugger |

Note that **TestCases** must be a valid set of `TestCase` objects, these should
represent user's test selection.

#### Example
```json
{
  "MessageType": "TestExecution.RunAllWithDefaultHost",
  "Payload": {
    "Sources": null,
    "TestCases": [
      {
        "Properties": [
          {
            "Key": {
              "Id": "TestCase.FullyQualifiedName",
              "Label": "FullyQualifiedName",
              "Category": "",
              "Description": "",
              "Attributes": 1,
              "ValueType": "System.String"
            },
            "Value": "UnitTestProject.UnitTest.PassingTest"
          },
          {
            "Key": {
              "Id": "TestCase.ExecutorUri",
              "Label": "Executor Uri",
              "Category": "",
              "Description": "",
              "Attributes": 1,
              "ValueType": "System.Uri"
            },
            "Value": "executor://MSTestAdapter/v2"
          },
          {
            "Key": {
              "Id": "TestCase.Source",
              "Label": "Source",
              "Category": "",
              "Description": "",
              "Attributes": 0,
              "ValueType": "System.String"
            },
            "Value": ".\\samples\\UnitTestProject\\bin\\Debug\\netcoreapp1.0\\UnitTestProject.dll"
          },
          {
            "Key": {
              "Id": "TestCase.DisplayName",
              "Label": "Name",
              "Category": "",
              "Description": "",
              "Attributes": 0,
              "ValueType": "System.String"
            },
            "Value": "PassingTest"
          },
          {
            "Key": {
              "Id": "MSTestDiscovererv2.IsEnabled",
              "Label": "IsEnabled",
              "Category": "",
              "Description": "",
              "Attributes": 1,
              "ValueType": "System.Boolean"
            },
            "Value": true
          },
          {
            "Key": {
              "Id": "MSTestDiscovererv2.TestClassName",
              "Label": "ClassName",
              "Category": "",
              "Description": "",
              "Attributes": 1,
              "ValueType": "System.String"
            },
            "Value": "UnitTestProject.UnitTest"
          },
          {
            "Key": {
              "Id": "TestObject.Traits",
              "Label": "Traits",
              "Category": "",
              "Description": "",
              "Attributes": 5,
              "ValueType": "System.Collections.Generic.KeyValuePair`2[[System.String],[System.String]][]"
            },
            "Value": []
          }
        ]
      }
    ],
    "RunSettings": null,
    "KeepAlive": false,
    "DebuggingEnabled": false
  }
}
```
### Cancel Test Run (Request)
This request will cancel the current test run request.

#### API Payload
| Key         | Type   | Description           |
|-------------|--------|-----------------------|
| MessageType | string | TestExecution.Cancel  |
| Payload     | object | null                  |

#### Example
```json
{
    "MessageType": "TestExecution.Cancel",
    "Payload": null
}
```

### Abort Test Run (Request)
This request will abort the current test run request.

#### API Payload
| Key         | Type   | Description           |
|-------------|--------|-----------------------|
| MessageType | string | TestExecution.Abort   |
| Payload     | object | null                  |

#### Example
```json
{
    "MessageType": "TestExecution.Abort",
    "Payload": null
}
```

## Debug All/Selected Tests
Debug all or selected test operations follow this sequence:

1. Request for the process to be launched under debugger (along with start parameters). `TestSession.GetTestRunnerProcessStartInfoForRunAll` and `TestSession.GetTestRunnerProcessStartInfoForRunSelected` messages need to be sent for DebugAll and DebugSelected operation respectively.
2. Runner responds with a `TestSession.CustomTestHostLaunch` message. It has the executable which needs to be launched with debugger attached.
3. After starting the host process, send `TestSession.CustomTestHostLaunchCallback` as an acknowledgment to the runner.

After this, runner sends TestRunStatsChange and TestExecutionComplete events similar to a RunAll operation.

### Get Process StartInfo For Debug All (Request)
The request to get the Process StartInfo for the Test host.

#### API Payload
| Key         | Type   | Description                                         |
|-------------|--------|-----------------------------------------------------|
| MessageType | string | TestSession.GetTestRunnerProcessStartInfoForRunAll  |
| Payload     | object | See details below                                   |

**Payload** object is has following structure.

| Key              | Type    | Description                                   |
|------------------|---------|-----------------------------------------------|
| Sources          | array   | Set of test containers. *Required*            |
| TestCases        | array   | Set of `TestCase` objects. Null in this case. |
| RunSettings      | string  | Run settings for the test run                 |
| KeepAlive        | boolean | Reserved for future                           |
| DebuggingEnabled | boolean | `true` indicates a test run under debugger    |

#### Example
```json
{
  "MessageType": "TestExecution.GetTestRunnerProcessStartInfoForRunAll",
  "Payload": {
    "Sources": [
      "E:\\git\\singh\\vstest\\samples\\UnitTestProject\\bin\\Debug\\netcoreapp1.0\\UnitTestProject.dll"
    ],
    "TestCases": null,
    "RunSettings": null,
    "KeepAlive": false,
    "DebuggingEnabled": true
  }
}
```

### Get Process StartInfo For Debug Selected (Request)
The request to get the Process StartInfo for the Test host.

#### API Payload
| Key         | Type   | Description                                              |
|-------------|--------|----------------------------------------------------------|
| MessageType | string | TestSession.GetTestRunnerProcessStartInfoForRunSelected  |
| Payload     | object | See details below                                        |

**Payload** object is has following structure.

| Key              | Type    | Description                                   |
|------------------|---------|-----------------------------------------------|
| Sources          | array   | Set of test containers. Null in this case.    |
| TestCases        | array   | Set of `TestCase` objects. *Required*         |
| RunSettings      | string  | Run settings for the test run                 |
| KeepAlive        | boolean | Reserved for future                           |
| DebuggingEnabled | boolean | `true` indicates a test run under debugger    |

#### Example
```json
{
  "MessageType": "TestExecution.GetTestRunnerProcessStartInfoForRunSelected",
  "Payload": {
    "Sources": null,
    "TestCases": [
      {
        "Properties": [
          {
            "Key": {
              "Id": "TestCase.FullyQualifiedName",
              "Label": "FullyQualifiedName",
              "Category": "",
              "Description": "",
              "Attributes": 1,
              "ValueType": "System.String"
            },
            "Value": "UnitTestProject.UnitTest.PassingTest"
          },
          {
            "Key": {
              "Id": "TestCase.ExecutorUri",
              "Label": "Executor Uri",
              "Category": "",
              "Description": "",
              "Attributes": 1,
              "ValueType": "System.Uri"
            },
            "Value": "executor://MSTestAdapter/v2"
          },
          {
            "Key": {
              "Id": "TestCase.Source",
              "Label": "Source",
              "Category": "",
              "Description": "",
              "Attributes": 0,
              "ValueType": "System.String"
            },
            "Value": "E:\\git\\singh\\vstest\\samples\\UnitTestProject\\bin\\Debug\\netcoreapp1.0\\UnitTestProject.dll"
          },
          {
            "Key": {
              "Id": "TestCase.DisplayName",
              "Label": "Name",
              "Category": "",
              "Description": "",
              "Attributes": 0,
              "ValueType": "System.String"
            },
            "Value": "PassingTest"
          },
          {
            "Key": {
              "Id": "MSTestDiscovererv2.IsEnabled",
              "Label": "IsEnabled",
              "Category": "",
              "Description": "",
              "Attributes": 1,
              "ValueType": "System.Boolean"
            },
            "Value": true
          },
          {
            "Key": {
              "Id": "MSTestDiscovererv2.TestClassName",
              "Label": "ClassName",
              "Category": "",
              "Description": "",
              "Attributes": 1,
              "ValueType": "System.String"
            },
            "Value": "UnitTestProject.UnitTest"
          },
          {
            "Key": {
              "Id": "TestObject.Traits",
              "Label": "Traits",
              "Category": "",
              "Description": "",
              "Attributes": 5,
              "ValueType": "System.Collections.Generic.KeyValuePair`2[[System.String],[System.String]][]"
            },
            "Value": []
          },
          {
            "Key": {
              "Id": "TestCase.LineNumber",
              "Label": "Line Number",
              "Category": "",
              "Description": "",
              "Attributes": 1,
              "ValueType": "System.Int32"
            },
            "Value": 16
          },
          {
            "Key": {
              "Id": "TestCase.CodeFilePath",
              "Label": "File Path",
              "Category": "",
              "Description": "",
              "Attributes": 0,
              "ValueType": "System.String"
            },
            "Value": "E:\\git\\singh\\vstest\\samples\\UnitTestProject\\UnitTest.cs"
          }
        ]
      }
    ],
    "RunSettings": null,
    "KeepAlive": false,
    "DebuggingEnabled": true
  }
}
```

### Custom TestHost Launch (Response)
CustomTestHostLaunch is the response to GetTestRunnerProcessStartInfoRunAll/GetTestRunnerProcessStartInfoRunSelected request. This message contains the StartInfo for the testhost process.

#### API Payload
| Key         | Type   | Description                          |
|-------------|--------|--------------------------------------|
| MessageType | string | TestSession.CustomTestHostLaunch     |
| Payload     | object | See details below                    |

**Payload** object is has following structure.

| Key                  | Type    | Description                                                |
|----------------------|---------|------------------------------------------------------------|
| FileName             | string  | Name of the host process                                   |
| Arguments            | string  | Arguments to be passed to the host process                 |
| WorkingDirectory     | string  | Working directory for the host process                     |
| EnvironmentVariables | array   | Environment variables associated with host process         |
| CustomProperties     | array   | Any custom properties that need to set                     |

#### Example
```json
{
  "MessageType": "TestExecution.CustomTestHostLaunch",
  "Payload": {
    "FileName": "C:\\Program Files\\dotnet\\dotnet.exe",
    "Arguments": "exec --runtimeconfig \"UnitTestProject\\bin\\Debug\\netcoreapp1.0\\UnitTestProject.runtimeconfig.json\" --depsfile \"E:\\git\\singh\\vstest\\samples\\UnitTestProject\\bin\\Debug\\netcoreapp1.0\\UnitTestProject.deps.json\" \"E:\\git\\packages\\microsoft.testplatform.testhost/15.0.0-preview-20170106-08\\lib/netstandard1.5/testhost.dll\" --port 64531 --parentprocessid 12292",
    "WorkingDirectory": "vstest\\samples\\Microsoft.TestPlatform.Protocol",
    "EnvironmentVariables": {},
    "CustomProperties": null
  }
}
```

### Custom TestHost LaunchCallback (Acknowledgement)
CustomTestHostLaunchCallback is the acknowledgement sent to the runner after starting the host process.

#### API Payload
| Key         | Type   | Description                                  |
|-------------|--------|----------------------------------------------|
| MessageType | string | TestSession.CustomTestHostLaunchCallback     |
| Payload     | object | See details below                            |

**Payload** object is has following structure.

| Key                 | Type    | Description                                                |
|---------------------|---------|------------------------------------------------------------|
| HostProcessId       | number  | Process Id of the host process                             |
| ErrorMessage        | string  | Error message in case host process does not start          |

#### Example
```json
{
  "MessageType": "TestExecution.CustomTestHostLaunchCallback",
  "Payload": {
    "HostProcessId": 53572,
    "ErrorMessage": null
  }
}
```

## Test Session Messages (Response)
The log messages are sent as `TestSession.Message`. Error messages are also reported via this message response.

#### API Payload
| Key         | Type   | Description             |
|-------------|--------|-------------------------|
| MessageType | string | TestSession.Message     |
| Payload     | object | See details below       |

**Payload** object is has following structure.

| Key                 | Type    | Description                                                   |
|---------------------|---------|---------------------------------------------------------------|
| MessageLevel        | number  | Represent the TestMessageLevel. See details below             |
| Message             | string  | The actual message reported by the runner.                    |

**MessageLevel** provides an integer specifying the type of log message.
Possible values are:
  * `0x0`: Informational
  * `0x1`: Warning
  * `0x2`: Error

**Example**
```json
{
    "MessageType": "TestSession.Message",
    "Payload": {
      "MessageLevel" : 1,
      "Message" : "The test run was aborted."
    }
}
```

### Test Session End (Request)
This request is used to the end the current test session.

#### API Payload
| Key         | Type   | Description           |
|-------------|--------|-----------------------|
| MessageType | string | TestSession.Terminate |
| Payload     | object | null                  |

#### Example
```json
{
    "MessageType": "TestSession.Terminate",
    "Payload": null
}
```

## Appendix
### Key Data Structures
#### 1. Message<a name="DataStructure.Message"></a>
A `Message` is basic data unit for the Editor and Test Platform communication
protocol.

**Structure**

| Key         | Type   | Description                                            |
|-------------|--------|--------------------------------------------------------|
| MessageType | string | Type of message                                        |
| Payload     | object | Payload for an operation input or output. Can be null. |

**Example**
```json
{
    "MessageType": "TestSession.Connected",
    "Payload": null
}
```

#### 2. Property<a name="DataStructure.Property"></a>
Each `Property` is a <Key, Value> pair as shown below.

| Key   | Type   | Description                          |
|-------|--------|--------------------------------------|
| Key   | object | Definition of a Property. See below. |
| Value | string | Value of the Property                |

The `Key` identifies the property and `Value` is the data. Further a `Key` is
defined as follows:

| Key         | Type   | Description                                     |
|-------------|--------|-------------------------------------------------|
| Id          | object | Definition of a Property                        |
| Label       | string | Value of the Property                           |
| Category    | string | Type of a Property. Reserved for future.        |
| Description | string | Description of a Property.                      |
| Attributes  | number | Various attributes of this Property. See below. |
| ValueType   | string | A .net type that represents the Value           |

`Attributes` are a bitwise `OR` of following values:
  * `0x0`: None (default)
  * `0x1`: Hidden. Should be set for hidden properties.
  * `0x2`: Immutable. Should be set of properties that are readonly.
  * `0x4`: Trait (Deprecated). Should be set if a Property is a Trait.

#### 3. TestCase<a name="DataStructure.TestCase"></a>
A `TestCase` is a set of `Property` elements. Every testcase object must have following properties:

- **TestCase.FullyQualifiedName** represents the unique name for a test case.
```json
{
  "Key": {
    "Id": "TestCase.FullyQualifiedName",
    "Label": "FullyQualifiedName",
    "Category": "",
    "Description": "",
    "Attributes": 1,
    "ValueType": "System.String"
  },
  "Value": "UnitTestProject.UnitTest.TestWithPriority"
}
```

- **TestCase.ExecutorUri** represents the Adapter which owns this test case.
```json
{
  "Key": {
    "Id": "TestCase.ExecutorUri",
    "Label": "Executor Uri",
    "Category": "",
    "Description": "",
    "Attributes": 1,
    "ValueType": "System.Uri"
  },
  "Value": "executor://MSTestAdapter/v2"
}
```

- **TestCase.Source** is the path to the test container which contains the
   source of this test case.
```json
{
  "Key": {
    "Id": "TestCase.Source",
    "Label": "Source",
    "Category": "",
    "Description": "",
    "Attributes": 0,
    "ValueType": "System.String"
  },
  "Value": ".\\samples\\UnitTestProject\\bin\\Debug\\netcoreapp1.0\\UnitTestProject.dll"
}
```

- **TestCase.DisplayName** represents a user friendly notation for the test
   case. An editor or a runner can choose to show this to user.
```json
{
  "Key": {
    "Id": "TestCase.DisplayName",
    "Label": "Name",
    "Category": "",
    "Description": "",
    "Attributes": 0,
    "ValueType": "System.String"
  },
  "Value": "TestWithPriority"
}
```

- **TestCase.Traits** are a set of <Key, Value> pair of additional data related
   to a test case. User can use these values to filter tests. An editor or
   runner may show this to user.
```json
{
  "Key": {
    "Id": "TestObject.Traits",
    "Label": "Traits",
    "Category": "",
    "Description": "",
    "Attributes": 5,
    "ValueType": "System.Collections.Generic.KeyValuePair`2[[System.String],[System.String]][]"
  },
  "Value": [
    {
      "Key": "Priority",
      "Value": "0"
    }
  ]
}
```

Apart from these properties, a `TestCase` object may have adapter specific
properties.

#### 4. TestResult<a name="DataStructure.TestResult"></a>
A `TestResult` object represents the result of a test case execution. It has the
following structure.

| Key         | Type   | Description                                     |
|-------------|--------|-------------------------------------------------|
| TestCase    | object | `TestCase` that executed                        |
| Attachments | array  | Paths to test attachments                       |
| Messages    | array  | Set of messages generated during test execution |
| Properties  | array  | Set of `Property` for this result               |

##### 4.1 Properties
These are the mandatory properties available in every `TestResult` object.

- **TestResult.DisplayName** provides a friendly name for the test result.
```json
{
  "Key": {
    "Id": "TestResult.DisplayName",
    "Label": "TestResult Display Name",
    "Category": "",
    "Description": "",
    "Attributes": 1,
    "ValueType": "System.String"
  },
  "Value": ""
}
```

- **TestResult.Duration** provides the entire duration of this test case
   execution.
```json
{
  "Key": {
    "Id": "TestResult.Duration",
    "Label": "Duration",
    "Category": "",
    "Description": "",
    "Attributes": 0,
    "ValueType": "System.TimeSpan"
  },
  "Value": "00:00:00.0306600"
}
```

- **TestResult.ErrorMessage** provides an error message if the test failed.
```json
{
  "Key": {
    "Id": "TestResult.ErrorMessage",
    "Label": "Error Message",
    "Category": "",
    "Description": "",
    "Attributes": 0,
    "ValueType": "System.String"
  },
  "Value": "Assert.AreEqual failed. Expected:<2>. Actual:<3>. "
}
```

- **TestResult.ErrorStackTrace** provides the stack trace for the error.
```json
{
  "Key": {
    "Id": "TestResult.ErrorStackTrace",
    "Label": "Error Stack Trace",
    "Category": "",
    "Description": "",
    "Attributes": 0,
    "ValueType": "System.String"
  },
  "Value": "   at UnitTestProject.UnitTest.FailingTest() in D:\\dd\\gh\\Microsoft\\vstest\\samples\\UnitTestProject\\UnitTest.cs:line 26\r\n"
}
```

- **TestResult.Outcome** provides an integer specifying the result of a test
   case execution.
Possible outcomes are:
  * `0x0`: None. Test case doesn't have an outcome.
  * `0x1`: Passed
  * `0x2`: Failed
  * `0x3`: Skipped
  * `0x4`: Not found. Test case was not found during execution.

```json
{
  "Key": {
    "Id": "TestResult.Outcome",
    "Label": "Outcome",
    "Category": "",
    "Description": "",
    "Attributes": 0,
    "ValueType": "Microsoft.VisualStudio.TestPlatform.ObjectModel.TestOutcome, Microsoft.VisualStudio.TestPlatform.ObjectModel, Version=15.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
  },
  "Value": 2
}
```

- **TestResult.StartTime** provides the start time of the test case execution.
```json
{
  "Key": {
    "Id": "TestResult.StartTime",
    "Label": "Start Time",
    "Category": "",
    "Description": "",
    "Attributes": 0,
    "ValueType": "System.DateTimeOffset"
  },
  "Value": "2017-01-06T10:15:39.9907073+05:30"
}
```

- **TestResult.EndTime** provides the end time of test case execution.
```json
{
  "Key": {
    "Id": "TestResult.EndTime",
    "Label": "End Time",
    "Category": "",
    "Description": "",
    "Attributes": 0,
    "ValueType": "System.DateTimeOffset"
  },
  "Value": "2017-01-06T10:15:40.021772+05:30"
}
```

#### 5. TargetFramework<a name="RunSettings.TargetFramework"></a>
The test runner needs the target framework for running the tests using the appropriate
hosting environment. It can be set using TargetFrameworkVersion node in the RunSettings
passed as part of discovery/execution request. TargetFrameworkVersion value is the fullname
of the target framework. For example, ".NETFramework,Version=v4.5.2". Other supported
.Net Framework version shortcuts are Framework35, Framework40 and Framework45.

##### Example
```
<?xml version=\"1.0\" encoding=\"utf-8\"?>
<RunSettings>
  <RunConfiguration>
    <TargetFrameworkVersion>.NETFramework,Version=v4.5.2</TargetFrameworkVersion>
  </RunConfiguration>
</RunSettings>
```

##### Example for Test Execution Request
```
{
  "MessageType": "TestExecution.RunAllWithDefaultHost",
  "Payload": {
    "Sources": [
      "E:\\UnitTest1\\UnitTest1\\bin\\Debug\\net452\\UnitTest1.dll"
    ],
    "TestCases": null,
    "RunSettings": "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<RunSettings>\r\n<RunConfiguration>\r\n<TargetFrameworkVersion>Framework45</TargetFrameworkVersion>\r\n</RunConfiguration>\r\n</RunSettings>",
    "KeepAlive": false,
    "DebuggingEnabled": false
  }
}
```

[discovery]: 0002-Test-Discovery-Protocol.md
[execution]: 0003-Test-Execution-Protocol.md
[sample]: https://github.com/Microsoft/vstest/tree/main/samples/Microsoft.TestPlatform.Protocol
