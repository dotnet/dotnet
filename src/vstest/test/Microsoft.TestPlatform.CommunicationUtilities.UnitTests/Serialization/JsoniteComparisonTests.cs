// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;

using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using TestResult = Microsoft.VisualStudio.TestPlatform.ObjectModel.TestResult;

namespace Microsoft.TestPlatform.CommunicationUtilities.UnitTests.Serialization;

/// <summary>
/// Performance tests for the production serialization code path (JsonDataSerializer).
/// On .NET Core this exercises System.Text.Json with our custom converters.
/// On .NET Framework this exercises Jsonite with our custom converters.
/// </summary>
[TestClass]
[TestCategory("Serialization")]
public class SerializerPerformanceAndDiagnosticTests
{
    private const int PerfIterations = 1000;

    // ══════════════════════════════════════════════════════════════════════
    //  Diagnostics — verify which serializer is active
    // ══════════════════════════════════════════════════════════════════════

    [TestMethod]
    public void VerifySerializerName()
    {
#if NETCOREAPP
        Assert.AreEqual("System.Text.Json", JsonDataSerializer.SerializerName,
            "Expected STJ on .NET Core but got: " + JsonDataSerializer.SerializerName);
#else
        Assert.AreEqual("Jsonite", JsonDataSerializer.SerializerName,
            "Expected Jsonite on .NET Framework but got: " + JsonDataSerializer.SerializerName);
#endif
        Console.WriteLine($"Serializer: {JsonDataSerializer.SerializerName}");
    }

    // ══════════════════════════════════════════════════════════════════════
    //  Performance — Production code path (JsonDataSerializer)
    // ══════════════════════════════════════════════════════════════════════

    [TestMethod]
    [TestCategory("Performance")]
    public void Perf_DeserializeMessage_TestMessage()
    {
        var json = JsonDataSerializer.Instance.SerializePayload(MessageType.TestMessage, TestMessagePayload, 7);

        JsonDataSerializer.Instance.DeserializeMessage(json);

        var sw = Stopwatch.StartNew();
        for (int i = 0; i < PerfIterations; i++)
        {
            JsonDataSerializer.Instance.DeserializeMessage(json);
        }

        sw.Stop();
        Console.WriteLine($"[{JsonDataSerializer.SerializerName}] DeserializeMessage TestMessage: {sw.ElapsedMilliseconds}ms for {PerfIterations} iterations");
    }

    [TestMethod]
    [TestCategory("Performance")]
    public void Perf_DeserializeMessage_ExecutionComplete()
    {
        var json = JsonDataSerializer.Instance.SerializePayload(MessageType.ExecutionComplete, ExecutionCompletePayload, 7);

        JsonDataSerializer.Instance.DeserializeMessage(json);

        var sw = Stopwatch.StartNew();
        for (int i = 0; i < PerfIterations; i++)
        {
            JsonDataSerializer.Instance.DeserializeMessage(json);
        }

        sw.Stop();
        Console.WriteLine($"[{JsonDataSerializer.SerializerName}] DeserializeMessage ExecutionComplete: {sw.ElapsedMilliseconds}ms for {PerfIterations} iterations");
    }

    [TestMethod]
    [TestCategory("Performance")]
    public void Perf_SerializePayload_TestMessage()
    {
        JsonDataSerializer.Instance.SerializePayload(MessageType.TestMessage, TestMessagePayload, 7);

        var sw = Stopwatch.StartNew();
        for (int i = 0; i < PerfIterations; i++)
        {
            JsonDataSerializer.Instance.SerializePayload(MessageType.TestMessage, TestMessagePayload, 7);
        }

        sw.Stop();
        Console.WriteLine($"[{JsonDataSerializer.SerializerName}] SerializePayload TestMessage: {sw.ElapsedMilliseconds}ms for {PerfIterations} iterations");
    }

    [TestMethod]
    [TestCategory("Performance")]
    public void Perf_SerializePayload_ExecutionComplete()
    {
        JsonDataSerializer.Instance.SerializePayload(MessageType.ExecutionComplete, ExecutionCompletePayload, 7);

        var sw = Stopwatch.StartNew();
        for (int i = 0; i < PerfIterations; i++)
        {
            JsonDataSerializer.Instance.SerializePayload(MessageType.ExecutionComplete, ExecutionCompletePayload, 7);
        }

        sw.Stop();
        Console.WriteLine($"[{JsonDataSerializer.SerializerName}] SerializePayload ExecutionComplete: {sw.ElapsedMilliseconds}ms for {PerfIterations} iterations");
    }

    [TestMethod]
    [TestCategory("Performance")]
    public void Perf_RoundTrip_TestMessage()
    {
        var json = JsonDataSerializer.Instance.SerializePayload(MessageType.TestMessage, TestMessagePayload, 7);

        var msg = JsonDataSerializer.Instance.DeserializeMessage(json);
        JsonDataSerializer.Instance.DeserializePayload<TestMessagePayload>(msg);

        var sw = Stopwatch.StartNew();
        for (int i = 0; i < PerfIterations; i++)
        {
            msg = JsonDataSerializer.Instance.DeserializeMessage(json);
            JsonDataSerializer.Instance.DeserializePayload<TestMessagePayload>(msg);
        }

        sw.Stop();
        Console.WriteLine($"[{JsonDataSerializer.SerializerName}] RoundTrip TestMessage: {sw.ElapsedMilliseconds}ms for {PerfIterations} iterations");
    }

    [TestMethod]
    [TestCategory("Performance")]
    public void Perf_RoundTrip_ExecutionComplete()
    {
        var json = JsonDataSerializer.Instance.SerializePayload(MessageType.ExecutionComplete, ExecutionCompletePayload, 7);

        var msg = JsonDataSerializer.Instance.DeserializeMessage(json);
        JsonDataSerializer.Instance.DeserializePayload<TestRunCompletePayload>(msg);

        var sw = Stopwatch.StartNew();
        for (int i = 0; i < PerfIterations; i++)
        {
            msg = JsonDataSerializer.Instance.DeserializeMessage(json);
            JsonDataSerializer.Instance.DeserializePayload<TestRunCompletePayload>(msg);
        }

        sw.Stop();
        Console.WriteLine($"[{JsonDataSerializer.SerializerName}] RoundTrip ExecutionComplete: {sw.ElapsedMilliseconds}ms for {PerfIterations} iterations");
    }

    // ══════════════════════════════════════════════════════════════════════
    //  Payloads
    // ══════════════════════════════════════════════════════════════════════

    private static readonly TestMessagePayload TestMessagePayload = new()
    {
        MessageLevel = TestMessageLevel.Warning,
        Message = "Test 'CalculatorTests.AddTest' was skipped: requires .NET 8"
    };

    private static readonly TestRunCompletePayload ExecutionCompletePayload = BuildExecutionCompletePayload();

    private static TestRunCompletePayload BuildExecutionCompletePayload()
    {
        var tc = new TestCase(
            "Contoso.Math.Tests.CalculatorTests.AddTest",
            new Uri("executor://MSTestAdapter/v2"),
            "Contoso.Math.Tests.dll")
        {
            Id = new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"),
        };

        var tr = new TestResult(tc)
        {
            Outcome = TestOutcome.Passed,
            DisplayName = "AddTest(1, 2, 3)",
            Duration = TimeSpan.FromMilliseconds(12),
            StartTime = new DateTimeOffset(2026, 3, 20, 10, 0, 0, TimeSpan.Zero),
            EndTime = new DateTimeOffset(2026, 3, 20, 10, 0, 0, 12, TimeSpan.Zero),
            ComputerName = "BUILD-AGENT-01",
        };

        var stats = new Dictionary<TestOutcome, long> { [TestOutcome.Passed] = 1 };
        var runStats = new TestRunStatistics(1, stats);

        return new TestRunCompletePayload
        {
            TestRunCompleteArgs = new TestRunCompleteEventArgs(
                runStats, false, false, null, null, null, TimeSpan.FromSeconds(2)),
            LastRunTests = new TestRunChangedEventArgs(
                runStats, new[] { tr }, Array.Empty<TestCase>()),
            ExecutorUris = new List<string> { "executor://MSTestAdapter/v2" },
        };
    }
}
