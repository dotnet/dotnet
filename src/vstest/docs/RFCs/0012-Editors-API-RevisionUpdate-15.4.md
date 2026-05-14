# 0012 Editors API Revision Update - 15.4

## Summary
This details the change in the JSON protocol between an Editor/IDE
and the test platform to support test case filtering.

## Motivation
The console runner has an ability to run a specific set of tests via the `/testcasefilter` argument. Editors require this functionality as well to run a subset of tests which have not yet been discovered. This document builds on top of the v2 protocol between the Editor/IDE and the test platform.

Here is the link to v1 specs: [Editors-API-Specification](./RFCs/0007-Editors-API-Specification.md)
and here is the link to the v2 specs: [Editors-API-Specification-V2](./RFCs/0009-Editors-API-RevisionUpdate.md)

## Design
Filtering in the test platform is only supported when a test run request with sources is requested. This is unsupported when a run request with a set of TestCases is passed in. To that effect the following messages would change to accommodate filtering.

### Run All Tests
The default run tests request will now have a TestCaseFilter passed in.

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
| **TestCaseFilter**| **string**| **the test case filter string**         |

#### Example
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
    "DebuggingEnabled": false,
    "TestCaseFilter": "FullyQualifiedName~UnitTestClass1&TestCategory=CategoryA"
  }
}
```
The responses to this request do not change.

### Debug All Tests
The request to get the Process StartInfo for the Test host will now have a `TestCaseFilter`.

#### API Payload
| Key         | Type   | Description                                         |
|-------------|--------|-----------------------------------------------------|
| MessageType | string | TestSession.GetTestRunnerProcessStartInfoForRunAll  |
| Payload     | object | See details below                                   |

**Payload** object has following structure.

| Key              | Type    | Description                                   |
|------------------|---------|-----------------------------------------------|
| Sources          | array   | Set of test containers. *Required*            |
| TestCases        | array   | Set of `TestCase` objects. Null in this case. |
| RunSettings      | string  | Run settings for the test run                 |
| KeepAlive        | boolean | Reserved for future                           |
| DebuggingEnabled | boolean | `true` indicates a test run under debugger    |
| **TestCaseFilter**| **string**| **the test case filter string**            |

#### Example
```json
{
  "MessageType": "TestExecution.GetTestRunnerProcessStartInfoForRunAll",
  "Version": 2,
  "Payload": {
    "Sources": [
      "E:\\git\\singh\\vstest\\samples\\UnitTestProject\\bin\\Debug\\netcoreapp1.0\\UnitTestProject.dll"
    ],
    "TestCases": null,
    "RunSettings": null,
    "KeepAlive": false,
    "DebuggingEnabled": true,
    "TestCaseFilter": "FullyQualifiedName~UnitTestClass1&TestCategory=CategoryA"
  }
}
```

The responses to this request do not change.

## Note
1. This new parameter is purely optional and does not require any changes in editors based on earlier protocols. **This isn't a breaking change.**
2. This change is only available in the v2 version of the protocol.
