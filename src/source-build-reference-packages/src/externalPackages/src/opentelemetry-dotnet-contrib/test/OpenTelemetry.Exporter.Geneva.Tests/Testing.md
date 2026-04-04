# Manual testing steps: Reading events from user_events on Linux

`UnixUserEventsDataTransport` class can write events to user_events on .NET 8
and later on versions of Linux with user_events available in the kernel (6.6+).

However, the agents in GitHub Action are not running on hosts with compatible kernel.
Reading these events is a bit tricky because it needs elevation. Thus this
documents records some manual steps to run tests that can validate the
user_events feature works.

## Testing

### Prerequisites

* Perf Tool: Install the [perf](https://perf.wiki.kernel.org/index.php/Main_Page)
  tool to capture the user_events.
* Decode-Perf Tool: Install the [decode-perf](https://github.com/microsoft/LinuxTracepoints/tree/main/libeventheader-decode-cpp/tools)
  tool to decode the user_events.

### Steps for testing UnixUserEventsDataTransport

To capture the user_events, the perf tool has to be run while `dotnet test` is
running. The most simple way is to do in two terminals.

#### Terminal 1

1. Remove `(Skip = "This would fail on Ubuntu. Skipping for now. See
issue: #2326.")` for the `UserEvents_Logs_Success_Linux` test case to enable the
tests.

2. Go to the `test/OpenTelemetry.Exporter.Geneva.Tests/` folder and run the tests:

```bash
$ sudo dotnet test --configuration Debug --framework net8.0 --filter CategoryName=Geneva:user_events --no-build
[xUnit.net 00:00:00.00] xUnit.net VSTest Adapter v2.8.2+699d445a1a (64-bit .NET 8.0.12)
[xUnit.net 00:00:00.12]   Discovering: OpenTelemetry.Exporter.Geneva.Tests
[xUnit.net 00:00:00.45]   Discovered:  OpenTelemetry.Exporter.Geneva.Tests
[xUnit.net 00:00:00.46]   Starting:    OpenTelemetry.Exporter.Geneva.Tests
[xUnit.net 00:00:00.51]     OpenTelemetry.Exporter.Geneva.Tests.UnixUserEventsDataTransportTests.UserEvents_Enabled_Success_Linux [SKIP]
[xUnit.net 00:00:00.51]       This would fail on Ubuntu. Skipping for now.
cat /sys/kernel/tracing/user_events_status
cat /sys/kernel/tracing/user_events_status [OUT] Active: 0
cat /sys/kernel/tracing/user_events_status [OUT] Busy: 0
cat /sys/kernel/debug/tracing/trace_pipe
cat /sys/kernel/tracing/events/user_events/MicrosoftOpenTelemetryLogs_L4K1/enable
cat /sys/kernel/tracing/events/user_events/MicrosoftOpenTelemetryLogs_L4K1/enable [OUT] 0
sh -c "echo '1' > /sys/kernel/tracing/events/user_events/MicrosoftOpenTelemetryLogs_L4K1/enable"
------------- ready to write events -------------
About to write tracepoint:
Written tracepoint.
cat /sys/kernel/debug/tracing/trace_pipe [OUT]   .NET TP Worker-19820   [004] ..... 620527.192577: MicrosoftOpenTelemetryLogs_L4K1: eventheader_flags=(7) version=(0) id=0x0 (0) tag=0x0 (0) opcode=(0) level=(4)
Writing events from listener:
eventheader_flags=(7), version=(0), id=0x0 (0), tag=0x0 (0), opcode=(0), level=(4)
Total events: 1
sh -c "echo '0' > /sys/kernel/tracing/events/user_events/MicrosoftOpenTelemetryLogs_L4K1/enable"
[xUnit.net 00:00:10.63]     OpenTelemetry.Exporter.Geneva.Tests.UnixUserEventsDataTransportTests.UserEvents_Disabled_Success_Linux [SKIP]
[xUnit.net 00:00:10.63]       This would fail on Ubuntu. Skipping for now.
[xUnit.net 00:00:10.63]   Finished:    OpenTelemetry.Exporter.Geneva.Tests
  OpenTelemetry.Exporter.Geneva.Tests test net8.0 succeeded (11.6s)

Test summary: total: 3, failed: 0, succeeded: 1, skipped: 2, duration: 11.6s
Build succeeded in 12.1s
```

#### Terminal 2

1. Check if events are ready with this command:
`sudo ls /sys/kernel/tracing/events/user_events/`. Before the test sets up the
user_events, it would return an error:

    ```bash
    $ sudo ls /sys/kernel/tracing/events/user_events/
    ls: cannot access '/sys/kernel/tracing/events/user_events/': No such file or directory
    ```

1. When the dotnet test outputs reached
`------------- ready to write events -------------`, the test would wait for 5
seconds before actually sending user_events. You can see events are ready by
running the first command:

    ```bash
    $ sudo ls /sys/kernel/tracing/events/user_events/
    MicrosoftOpenTelemetryLogs_L4K1  enable  filter  otlp_metrics
    ```

1. Before the tests send out user_events in 5 seconds, run the perf tool:
`sudo ./perf record -v -e user_events:MicrosoftOpenTelemetryLogs_L4K1`.

1. Once the dotnet test command finishes, ctrl-c to terminate the capture.

1. Run `sudo /mnt/c/repos/LinuxTracepoints/bin/perf-decode ./perf.data` to
decode the user_events data:

```bash
$ sudo ./perf-decode ./perf.data
{
"./perf.data": [
  { "n": "MicrosoftOpenTelemetryLogs:_", "__csver__": "0x400", "partA": { "time": "2025-01-24T02:43:03.938446Z" }, "PartB": { "_typeName": "Log", "severityNumber": 21, "severityText": "Critical", "name": "CheckoutFailed" }, "PartC": { "book_id": "12345", "book_name": "The Hitchhiker's Guide to the Galaxy" }, "meta": { "time": 620719.617558900, "cpu": 4, "pid": 89238, "tid": 89259, "level": 4, "keyword": "0x1" } } ]
}
```

Formatted:

```json
{
    "./perf.data": [{
        "n": "MicrosoftOpenTelemetryLogs:_",
        "__csver__": "0x400",
        "partA": {
            "time": "2025-01-24T02:43:03.938446Z"
        },
        "PartB": {
            "_typeName": "Log",
            "severityNumber": 21,
            "severityText": "Critical",
            "name": "CheckoutFailed"
        },
        "PartC": {
            "book_id": "12345",
            "book_name": "The Hitchhiker's Guide to the Galaxy"
        },
        "meta": {
            "time": 620719.617558900,
            "cpu": 4,
            "pid": 89238,
            "tid": 89259,
            "level": 4,
            "keyword": "0x1"
        }
    }]
}
```

### Steps for testing GenevaLogExporter

Go into `test/OpenTelemetry.Exporter.Geneva.Tests` folder
and run the following command in one terminal.

```bash
sudo dotnet test --configuration Debug --framework net8.0 --filter SuccessfulUserEventsExport_Linux
```

In the other terminal, run the same commands in the above steps. See the
following for the result.

```bash
$ sudo /mnt/c/repos/LinuxTracepoints/bin/perf-decode ./perf.data
{
"./perf.data": [
  { "n": "MicrosoftOpenTelemetryLogs:Log", "__csver__": "0x400", "PartA": { "time": "2025-02-28T02:54:12.704863Z", "ext_cloud_role": "BusyWorker", "ext_cloud_roleInstance": "CY1SCH030021417", "ext_cloud_roleVer": "9.0.15289.2", "PartB": { "_typeName": "Log", "severityText": "Information", "severityNumber": 9, "body": "Hello from {Food} {Price}.", "name": "OpenTelemetry.Exporter.Geneva.Tests.GenevaLogExporterTests" } }, "PartC": { "Food": "artichoke", "Price": 3.99 }, "meta": { "time": 612654.051887500, "cpu": 14, "pid": 38500, "tid": 38519, "level": 4, "keyword": "0x1" } } ]
}
```

Formatted:

```json
{
    "./perf.data": [{
        "n": "MicrosoftOpenTelemetryLogs:Log",
        "__csver__": "0x400",
        "PartA": {
            "time": "2025-02-28T02:54:12.704863Z",
            "ext_cloud_role": "BusyWorker",
            "ext_cloud_roleInstance": "CY1SCH030021417",
            "ext_cloud_roleVer": "9.0.15289.2",
            "PartB": {
                "_typeName": "Log",
                "severityText": "Information",
                "severityNumber": 9,
                "body": "Hello from {Food} {Price}.",
                "name": "OpenTelemetry.Exporter.Geneva.Tests.GenevaLogExporterTests"
            }
        },
        "PartC": {
            "Food": "artichoke",
            "Price": 3.99
        },
        "meta": {
            "time": 612654.051887500,
            "cpu": 14,
            "pid": 38500,
            "tid": 38519,
            "level": 4,
            "keyword": "0x1"
        }
    }]
}
```
