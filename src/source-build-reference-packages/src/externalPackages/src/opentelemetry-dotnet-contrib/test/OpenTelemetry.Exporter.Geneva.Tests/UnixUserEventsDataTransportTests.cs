// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

#if NET

using System.Globalization;
using Microsoft.LinuxTracepoints.Provider;
using OpenTelemetry.Exporter.Geneva.Transports;
using OpenTelemetry.Tests;
using Xunit;
using Xunit.Abstractions;

namespace OpenTelemetry.Exporter.Geneva.Tests;

[Trait("CategoryName", "Geneva:user_events")]
public class UnixUserEventsDataTransportTests
{
    /*
     * Instructions for running these tests:
     *
     *  1) You need a version of Linux with user_events available in the kernel.
     *     This can be done on WSL2 using the 6.6+ kernel.
     *
     *  2) You have to run the tests with elevation. You don't need elevation to
     *     write/emit user_events but you need elevation to read them (which
     *     these tests do).
     *
     *  Command:
     *    sudo dotnet test --configuration Debug --framework net8.0 --filter CategoryName=Geneva:user_events
     *
     * How these tests work:
     *
     *  1) The tests validate user_events are enabled and make sure the otlp_metrics tracepoint is registered.
     *
     *  2) A process is spawned to run cat /sys/kernel/debug/tracing/trace_pipe. This is what is listening for events.
     *
     *  3) Depending on the test, a process is spawned to run sh -c "echo '1' > /sys/kernel/tracing/events/user_events/{this.name}/enable" to enable events.
     *
     *  4) The thread running the tests writes to user_events using the GenevaExporter code. Then it waits for a bit. Then it checks to see what events (if any) were emitted.
     *
     *  5) Depending on the test, a process is spawned to run sh -c "echo '0' > /sys/kernel/tracing/events/user_events/{this.name}/enable" to disable events.
     */

    private static readonly byte[] testRequest = [0x0a, 0x0f, 0x12, 0x0d, 0x0a, 0x0b, 0x0a, 0x09, 0x54, 0x65, 0x73, 0x74, 0x4d, 0x65, 0x74, 0x65, 0x72];
    private readonly ITestOutputHelper testOutputHelper;

    public UnixUserEventsDataTransportTests(ITestOutputHelper testOutputHelper)
    {
        this.testOutputHelper = testOutputHelper;
    }

    [SkipUnlessPlatformMatchesFact(TestPlatform.Linux, requireElevatedProcess: true, Skip = "https://github.com/open-telemetry/opentelemetry-dotnet-contrib/issues/2326")]
    public void UserEvents_Enabled_Success_Linux()
    {
        EnsureUserEventsEnabled();

        var listener = new PerfTracepointListener(
            MetricUnixUserEventsDataTransport.MetricsTracepointName,
            MetricUnixUserEventsDataTransport.MetricsTracepointNameArgs);

        if (listener.IsEnabled())
        {
            throw new NotSupportedException($"{MetricUnixUserEventsDataTransport.MetricsTracepointName} is already enabled");
        }

        try
        {
            listener.Enable();

            MetricUnixUserEventsDataTransport.Instance.SendOtlpProtobufEvent(
                testRequest,
                testRequest.Length);

            Thread.Sleep(5000);

            foreach (var e in listener.Events)
            {
                this.testOutputHelper.WriteLine(string.Join(", ", e.Select(kvp => $"{kvp.Key}={kvp.Value}")));
            }

            var @event = Assert.Single(listener.Events);

            Assert.EndsWith($" ({MetricUnixUserEventsDataTransport.MetricsProtocol})", @event["protocol"]);
            Assert.Equal(MetricUnixUserEventsDataTransport.MetricsVersion, @event["version"]);

            var eventBufferStringData = @event["buffer"].AsSpan();

            var eventBuffer = new byte[(eventBufferStringData.Length + 1) / 3];

            var index = 0;
            var position = 0;
            while (position < eventBufferStringData.Length)
            {
                eventBuffer[index++] = byte.Parse(eventBufferStringData.Slice(position, 2), NumberStyles.HexNumber);
                position += 3;
            }

            Assert.Equal(testRequest, eventBuffer);
        }
        finally
        {
            try
            {
                listener.Disable();
            }
            catch
            {
            }

            listener.Dispose();
        }
    }

    [SkipUnlessPlatformMatchesFact(TestPlatform.Linux, requireElevatedProcess: true, Skip = "https://github.com/open-telemetry/opentelemetry-dotnet-contrib/issues/2326")]
    public void UserEvents_Disabled_Success_Linux()
    {
        EnsureUserEventsEnabled();

        var listener = new PerfTracepointListener(
            MetricUnixUserEventsDataTransport.MetricsTracepointName,
            MetricUnixUserEventsDataTransport.MetricsTracepointNameArgs);

        if (listener.IsEnabled())
        {
            throw new NotSupportedException($"{MetricUnixUserEventsDataTransport.MetricsTracepointName} is already enabled");
        }

        try
        {
            MetricUnixUserEventsDataTransport.Instance.SendOtlpProtobufEvent(
                testRequest,
                testRequest.Length);

            Thread.Sleep(5000);

            Assert.Empty(listener.Events);
        }
        finally
        {
            listener.Dispose();
        }
    }

    [SkipUnlessPlatformMatchesFact(TestPlatform.Linux, requireElevatedProcess: true, Skip = "https://github.com/open-telemetry/opentelemetry-dotnet-contrib/issues/2326")]
    public void UserEvents_Logs_Success_Linux()
    {
        var listener = new PerfTracepointListener(
            "MicrosoftOpenTelemetryLogs_L4K1",
            MetricUnixUserEventsDataTransport.MetricsTracepointNameArgs);

        var logsTracepoint = UnixUserEventsDataTransport.Instance.RegisterUserEventProviderForLogs();

        try
        {
            listener.Enable();

            Console.WriteLine("------------- ready to write events -------------");
            Thread.Sleep(5000);

            if (logsTracepoint.IsEnabled)
            {
                var eventBuilder = CreateEventHeaderDynamicBuilder();

                Console.WriteLine("About to write tracepoint:");
                eventBuilder.Write(logsTracepoint);
                Console.WriteLine("Written tracepoint.");
            }

            Thread.Sleep(5000);

            this.testOutputHelper.WriteLine("Writing events from listener:");
            Console.WriteLine("Writing events from listener:");
            foreach (var e in listener.Events)
            {
                this.testOutputHelper.WriteLine(string.Join(", ", e.Select(kvp => $"{kvp.Key}={kvp.Value}")));
                Console.WriteLine(string.Join(", ", e.Select(kvp => $"{kvp.Key}={kvp.Value}")));
            }

            this.testOutputHelper.WriteLine("Total events: " + listener.Events.Count);
            Console.WriteLine("Total events: " + listener.Events.Count);
        }
        finally
        {
            try
            {
                listener.Disable();
            }
            catch
            {
            }

            listener.Dispose();
        }
    }

    private static EventHeaderDynamicBuilder CreateEventHeaderDynamicBuilder()
    {
        var eb = new EventHeaderDynamicBuilder();
        eb.Reset("_");
        eb.AddUInt16("__csver__", 1024, Microsoft.LinuxTracepoints.EventHeaderFieldFormat.HexInt);

        eb.AddStructWithMetadataPosition("partA", out var partAFieldsCountMetadataPosition);

        string rfc3339String = DateTime.UtcNow.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss.FFFFFFZ", CultureInfo.InvariantCulture);
        eb.AddString16("time", rfc3339String);

        byte partAFieldsCount = 1;

        eb.SetStructFieldCount(partAFieldsCountMetadataPosition, partAFieldsCount);

        // Part B

        byte partBFieldsCount = 4; // We at least have three fields in Part B: _typeName, severityText, severityNumber, name
        eb.AddStructWithMetadataPosition("PartB", out var partBFieldsCountMetadataPosition);
        eb.AddString16("_typeName", "Log");
        eb.AddUInt8("severityNumber", 21);

        eb.AddString16("severityText", "Critical");
        eb.AddString16("name", "CheckoutFailed");

        eb.SetStructFieldCount(partBFieldsCountMetadataPosition, partBFieldsCount);

        // Part C

        byte partCFieldsCount = 0;
        eb.AddStructWithMetadataPosition("PartC", out var partCFieldsCountMetadataPosition);

        eb.AddString16("book_id", "12345");
        eb.AddString16("book_name", "The Hitchhiker's Guide to the Galaxy");
        partCFieldsCount += 2;

        eb.SetStructFieldCount(partCFieldsCountMetadataPosition, partCFieldsCount);

        return eb;
    }

    private static void EnsureUserEventsEnabled()
    {
        var errors = ConsoleCommand.Run("cat", "/sys/kernel/tracing/user_events_status");

        if (errors.Any())
        {
            throw new NotSupportedException("Kernel does not support user_events. Verify your distribution/kernel supports user_events: https://docs.kernel.org/trace/user_events.html.");
        }
    }
}

#endif
