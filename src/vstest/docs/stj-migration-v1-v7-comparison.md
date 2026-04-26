# V1 vs V7 Wire Format Comparison

## Summary

**18 messages are IDENTICAL** between V1 and V7 — only the outer envelope changes (V7 adds `"Version": 7`).

**5 messages are STRUCTURALLY DIFFERENT** — all because they contain `TestCase` and/or `TestResult` objects, which are serialized differently between protocol versions.

## Identical Messages (18)

These payloads are byte-for-byte identical between V1 and V7. The only difference is the message envelope:
- V1: `{"MessageType":"...","Payload":{...}}`
- V7: `{"Version":7,"MessageType":"...","Payload":{...}}`

| Message | Payload Type | Notes |
|---------|-------------|-------|
| VersionCheck | `int` | Protocol handshake |
| TestMessage | `TestMessagePayload` | Log/trace output |
| StartDiscovery | `DiscoveryCriteria` | No TestCase in payload |
| StartTestExecutionWithSources | `TestRunCriteriaWithSources` | Source paths only, no TestCase |
| CustomTestHostLaunch | `TestProcessStartInfo` | Process launch info |
| CustomTestHostLaunchCallback | `int` | Process ID |
| LaunchAdapterProcessWithDebuggerAttached | `TestProcessStartInfo` | Debug launch |
| LaunchAdapterProcessWithDebuggerAttachedCallback | `int` | Process ID |
| AttachDebugger | `TestProcessAttachDebuggerPayload` | PID + framework |
| AttachDebuggerCallback | `bool` | Success/failure |
| BeforeTestRunStart | `BeforeTestRunStartPayload` | Settings + sources |
| AfterTestRunEnd | `bool` | IsCanceled |
| AfterTestRunEndResult | `AfterTestRunEndResult` | Attachments + metrics |
| TestHostLaunched | `TestHostLaunchedPayload` | Process ID |
| StartTestSession | `StartTestSessionPayload` | Session creation |
| StartTestSessionCallback | `StartTestSessionAckPayload` | Session ack |
| StopTestSession | `StopTestSessionPayload` | Session teardown |
| StopTestSessionCallback | `StopTestSessionAckPayload` | Session teardown ack |

## Different Messages (5)

All differences come from **TestCase** and **TestResult** serialization:

| Message | Payload Type | What Differs |
|---------|-------------|-------------|
| TestCasesFound | `List<TestCase>` | TestCase format |
| DiscoveryComplete | `DiscoveryCompletePayload` | Nested TestCase in LastDiscoveredTests |
| StartTestExecutionWithTests | `TestRunCriteriaWithTests` | TestCase in Tests list |
| TestRunStatsChange | `TestRunStatsPayload` | TestCase + TestResult |
| ExecutionComplete | `TestRunCompletePayload` | TestCase + TestResult in nested args |

### What changes between V1 and V7

#### TestCase

**V1 (Properties array — verbose):**
```json
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
      "Value": "MyNamespace.MyTest"
    },
    { "Key": { "Id": "TestCase.ExecutorUri", ... }, "Value": "executor://mstest" },
    { "Key": { "Id": "TestCase.Source", ... }, "Value": "test.dll" },
    { "Key": { "Id": "TestCase.Id", ... }, "Value": "guid-here" },
    ...
  ]
}
```

**V7 (Flat object — compact):**
```json
{
  "Id": "guid-here",
  "FullyQualifiedName": "MyNamespace.MyTest",
  "DisplayName": "MyTest",
  "ExecutorUri": "executor://mstest",
  "Source": "test.dll",
  "CodeFilePath": null,
  "LineNumber": -1,
  "Properties": [
    // Only custom/non-built-in properties (e.g., Traits)
  ]
}
```

#### TestResult

**V1:** TestCase as Properties array + result fields as Properties array (Outcome, ErrorMessage, Duration, etc.)

**V7:** TestCase as flat object + result fields as top-level properties:
```json
{
  "TestCase": { /* flat TestCase */ },
  "Attachments": [...],
  "Outcome": 1,
  "ErrorMessage": "...",
  "ErrorStackTrace": "...",
  "DisplayName": "...",
  "ComputerName": "...",
  "Duration": "00:00:00.025",
  "StartTime": "2026-03-20T10:00:00+00:00",
  "EndTime": "2026-03-20T10:00:00.012+00:00",
  "Messages": [...],
  "Properties": []
}
```

### Why the difference?

V1 (protocol 0, 1, 3) uses `TestPlatformContractResolver1` which routes TestCase and TestResult through custom converters (`TestCaseConverter`, `TestResultConverter`) that serialize everything as Key/Value property bags.

V2+ (protocol 2, 4, 5, 6, 7) uses `DefaultTestPlatformContractResolver` which lets the default serializer handle TestCase/TestResult as flat objects, with only the custom `TestObject.Properties` bag going through a converter.

The V2 format is ~3-5x more compact and faster to parse.
