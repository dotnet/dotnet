# 0009 Editors API Revision Update

## Summary
This note outlines the proposed changes for the JSON protocol between an Editor/IDE
and the test platform.


Here is the link to v1 specs: [Editors-API-Specification](./RFCs/0007-Editors-API-Specification.md)

## Motivation
Here are the key factors:
* Reducing size of the payload of message responses to improve performance. Refer to [https://github.com/Microsoft/vstest/issues/396](https://github.com/Microsoft/vstest/issues/396)
* Improvements to versioning and ability to handle future breaking changes.

## Overview of changes
* New data structure for communication for v2 and above.
* Negotiation for protocol version: Editor and Test Runner, Test Runner and Test Host.
* Json optimization and changes to payloads.

### Messages for communication
Protocol related communication messages between Editor and Test Platform can have either of the following structures.

#### Message
This is existing Message structure used for communication in v1.

##### API Payload
| Key         | Type   | Description                                            |
|-------------|--------|--------------------------------------------------------|
| MessageType | string | Type of message                                        |
| Payload     | object | Payload for an operation. Can be null.                 |

##### Example
```json
{
    "MessageType": "ProtocolVersion",
    "Payload": 1
}
```

#### Versioned Message
This is the new data structure for Message introduced in v2. 
It has an additional version field.

##### API Payload
| Key         | Type   | Description                                            |
|-------------|--------|--------------------------------------------------------|
| MessageType | string | Type of message                                        |
| Version     | int    | Version based on which message should be deserialized  |
| Payload     | object | Payload for an operation input or output. Can be null. |

##### Example
```json
{
  "MessageType": "Extensions.Initialize",
  "Version": 2,
  "Payload": [
    "E:\\UnitTest1\\UnitTest1\\bin\\Debug\\net452\\Microsoft.VisualStudio.TestPlatform.MSTest.TestAdapter.dll"
  ]
}
```

#### Need for Versioned Message
A solution can contain projects that depend on different versions of Test Host. The tests from these projects can
be run in parallel, which means Editor will be receiving test cases/results from both the Test Hosts.
Editor will be able to deserialize both the messages, based on the version stamped on the responses.

#### Using Correct Message Structures
Message(v1) is used for protocol version negotiation in the beginning.
For rest of the communication, the structure for messages is decided based on negotiated version.

* If negotiated version = 1, Message (v1) structure will be used.
* If negotiated version >= 2, Versioned Message (v2) will be used.

Since now we have two kind of messages, implementation should be capable of deserializing both kind of messages.
For Versioned Messages, the embedded version will be used to choose the implementation for serializing and deserializing the payloads.

### Version Negotiations
Editor uses the version operation to negotiate for the highest common version supported
by the available Test platform.
Similarly, Test Runner uses version operation to negotiate for the highest common version
supported by the available Test host.
All the components will have a range of versions they support.

#### Negotiation Between IDE and Test Runner
Editor sends version request with highest version.
Test Runner then sends back the highest common version.

For Example:
* Editor with range (1-3) sends Version(Request) with version = 3, and Test Runner with range (1-2) will send back version as 2.
* Editor with range (1-2) sends version(Request) with version = 2, and Test Runner with range (1-3) will send back version as 2.

If Test Runner does not support the Editor, it will send ProtocolError message as discussed in the section below.

At this point the initial handshake between Editor and Test Runner is
complete. Version operation is required only once per launch of test runner.
Editor stamps the highest common version on the subsequent requests.

Note: For v1 though, since structure Message(v1) will be used, version is implicit.

#### Negotiation Between Test Runner and Test Host
Similarly, when the Test Runner receives run/discovery request, it launches and negotiates version
with Test Host.

Example:
Let us say, Editor supports range (1-3) and Test Platform Runner supports range (1-3),
so the version stamped on the run/discovery request will be 3.

* Test Host which supports range (1-2), will send back version as 2.
* Test Host which supports range (1-3), will send back version as 3.

If the TestHost does not support the version, it will send ProtocolError message to Editor via Runner.
Hence the version stamped on the response from Test Host will be the highest common
version for all the three components.

#### Version (Request)
Editor will send this protocol version request with the highest version that it supports.

##### API Payload
| Key         | Type   | Description                |
|-------------|--------|----------------------------|
| MessageType | string | ProtocolVersion            |
| Payload     | int    | Highest supported version  |

##### Example
```json
{
    "MessageType": "ProtocolVersion",
    "Payload": 1
}
```

#### Version (Response)
Test Runner sends back the highest common version as a response.

##### API Payload
| Key         | Type   | Description               |
|-------------|--------|---------------------------|
| MessageType | string | ProtocolVersion           |
| Payload     | int    | Highest common version    |

##### Example
```json
{
    "MessageType": "ProtocolVersion",
    "Payload": 2
}
```

#### Version Error (Response)
If there is a mismatch, Test Runner or Test Host can send ProtocolError message.

##### API Payload
| Key         | Type   | Description                                  |
|-------------|--------|----------------------------------------------|
| MessageType | string | ProtocolError                                |
| Payload     | object | String containing the supported range        |

##### Example
```json
{
    "MessageType": "ProtocolError",
    "Payload": null
}
```
Note : The Payload should contain the following information
* Component that reported the mismatch
* The version range supported by the component

## Json optimization
There are two objects that largely contribute to the size of the payload in the responses.
* TestCase object
* TestResult object

Both these objects contain properties that are well-known and few others that are added by the
adapters. For the well-known properties, we have reduced the verbosity and made them
self-describing. Please checkout the following requests and responses for details and examples.

### Discover Tests
#### Start Discovery (Request)
For v2, TestDiscovery.Start request will have the negotiated version (i.e. 2 currently) as part of the message.

##### API Payload
| Key         | Type   | Description            |
|-------------|--------|------------------------|
| MessageType | string | TestDiscovery.Start    |
| Version     | int    | Version of the message |
| Payload     | object | See below              |

##### Message Payload
| Key         | Type   | Description                    |
|-------------|--------|--------------------------------|
| Sources     | array  | Array of test container paths. |
| RunSettings | string | Run settings xml               |

##### Example
```json
{
  "MessageType": "TestDiscovery.Start",
  "Version" : 2,
  "Payload": {
    "Sources": [
      ".\\samples\\UnitTestProject\\bin\\Debug\\netcoreapp1.0\\UnitTestProject.dll"
    ],
    "RunSettings": null
  }
}
```

#### Tests Found (Response)
TestFound response will also have the version based on which Editor will be able to deserialize the message.
Verbosity for the json of TestCase object inside the TestFound payload has been reduced significantly.

##### API Payload
| Key         | Type   | Description                       |
|-------------|--------|-----------------------------------|
| MessageType | string | TestDiscovery.TestFound           |
| Version     | string | Version of the message            |
| Payload     | array  | List of TestCases                 |

##### Example
```json
{
  "MessageType": "TestDiscovery.TestFound",
  "Version": 2,
  "Payload": [
    {
      "Id": "850ad69f-0dc9-fb92-8500-8d2f8d8dfe2a",
      "FullyQualifiedName": "_20TestProject.UnitTest1.Test1",
      "DisplayName": "Test1",
      "ExecutorUri": "executor://MSTestAdapter/v2",
      "Source": ".\\samples\\UnitTestProject\\bin\\Debug\\netcoreapp1.0\\UnitTestProject.dll",
      "CodeFilePath": null,
      "LineNumber": 0,
      "Properties": [
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
          "Value": "TestProject.UnitTest1"
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
  ]
}
```

Similarly, DiscoveryComplete result will also have the version stamping.

### Run Tests
Similar to discovery requests, run tests request will also have the version as part of the message. Here are the examples.

#### Run Tests (Request) Example
```json
{
  "MessageType": "TestExecution.RunAllWithDefaultHost",
  "Version": 2,
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

#### Test Run Statistics (Response) Example
```json
{
  "MessageType": "TestExecution.Completed",
  "Version": 2,
  "Payload": {
    "TestRunCompleteArgs": {
      "TestRunStatistics": {
        "ExecutedTests": 1,
        "Stats": {
          "Passed": 1
        }
      },
      "IsCanceled": false,
      "IsAborted": false,
      "Error": null,
      "AttachmentSets": [],
      "ElapsedTimeInRunningTests": "00:00:00.8677523"
    },
    "LastRunTests": {
      "NewTestResults": [
        {
          "TestCase": {
            "Id": "850ad69f-0dc9-fb92-8500-8d2f8d8dfe2a",
            "FullyQualifiedName": "_20TestProject.UnitTest1.Test1",
            "DisplayName": "Test1",
            "ExecutorUri": "executor://MSTestAdapter/v2",
            "Source": "C:\\\\Users\\\\sasin\\\\Documents\\\\Visual Studio 2017\\\\Projects\\\\20TestProject\\\\20TestProject\\\\bin\\\\Debug\\\\net452\\\\20TestProject.dll",
            "CodeFilePath": null,
            "LineNumber": 0,
            "Properties": [
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
                "Value": "_20TestProject.UnitTest1"
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
          "Attachments": [],
          "Outcome": 1,
          "ErrorMessage": null,
          "ErrorStackTrace": null,
          "DisplayName": null,
          "Messages": [],
          "ComputerName": null,
          "Duration": "00:00:00.0072901",
          "StartTime": "2017-03-20T19:57:18.2262042+05:30",
          "EndTime": "2017-03-20T19:57:18.2921987+05:30",
          "Properties": []
        }
      ],
      "TestRunStatistics": {
        "ExecutedTests": 1,
        "Stats": {
          "Passed": 1
        }
      },
      "ActiveTests": []
    },
    "RunAttachments": [],
    "ExecutorUris": [
      "executor://mstestadapter/v2"
    ]
  }
}
```

All the other messages like Cancel, Abort or debugging related messages will follow the same pattern.
