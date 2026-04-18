// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;

using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using TestResult = Microsoft.VisualStudio.TestPlatform.ObjectModel.TestResult;

namespace Microsoft.TestPlatform.CommunicationUtilities.UnitTests.Serialization;

[TestClass]
[TestCategory("Performance")]
public class SerializationPerformanceTests
{
    private const int Iterations = 1000;

    [TestMethod]
    public void VerifySerializerName()
    {
#if NETCOREAPP
        Assert.AreEqual("System.Text.Json", JsonDataSerializer.SerializerName);
#else
        Assert.AreEqual("Jsonite", JsonDataSerializer.SerializerName);
#endif
        Console.WriteLine($"Serializer: {JsonDataSerializer.SerializerName}");
    }

    #region Payloads

    private static readonly TestMessagePayload TestMessagePayload = new()
    {
        MessageLevel = TestMessageLevel.Warning,
        Message = "Test 'CalculatorTests.AddTest' was skipped: requires .NET 8"
    };

    private static readonly List<TestCase> TestCases = new() { BuildTestCase() };

    private static readonly DiscoveryCompletePayload DiscoveryCompletePayload = BuildDiscoveryCompletePayload();

    private static readonly TestRunCompletePayload ExecutionCompletePayload = BuildExecutionCompletePayload();

    private static TestCase BuildTestCase()
    {
        return new TestCase(
            "Contoso.Math.Tests.CalculatorTests.AddTest",
            new Uri("executor://MSTestAdapter/v2"),
            "Contoso.Math.Tests.dll")
        {
            DisplayName = "AddTest(1, 2, 3)",
            Id = new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"),
            CodeFilePath = @"C:\src\Contoso.Math.Tests\CalculatorTests.cs",
            LineNumber = 42,
            Traits = { new Trait("Category", "Unit"), new Trait("Priority", "1") }
        };
    }

    private static DiscoveryCompletePayload BuildDiscoveryCompletePayload()
    {
        return new DiscoveryCompletePayload
        {
            TotalTests = 150,
            IsAborted = false,
            LastDiscoveredTests = new List<TestCase>
            {
                new("Contoso.Math.Tests.CalculatorTests.SubtractTest",
                    new Uri("executor://MSTestAdapter/v2"), "Contoso.Math.Tests.dll")
                {
                    DisplayName = "SubtractTest",
                    Id = new Guid("b2c3d4e5-f6a7-8901-bcde-f12345678901"),
                }
            },
            Metrics = new Dictionary<string, object> { ["TotalTestsDiscovered"] = 150 },
        };
    }

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

    #endregion

    #region TestMessage — Serialize

    [TestMethod]
    public void Serialize_TestMessage_V1_SystemTextJson()
    {
        JsonDataSerializer.Instance.SerializePayload(MessageType.TestMessage, TestMessagePayload, 1);

        var sw = Stopwatch.StartNew();
        for (int i = 0; i < Iterations; i++)
        {
            JsonDataSerializer.Instance.SerializePayload(MessageType.TestMessage, TestMessagePayload, 1);
        }
        sw.Stop();
        Console.WriteLine($"STJ Serialize TestMessage V1: {sw.ElapsedMilliseconds}ms for {Iterations} iterations");
    }

    [TestMethod]
    public void Serialize_TestMessage_V7_SystemTextJson()
    {
        JsonDataSerializer.Instance.SerializePayload(MessageType.TestMessage, TestMessagePayload, 7);

        var sw = Stopwatch.StartNew();
        for (int i = 0; i < Iterations; i++)
        {
            JsonDataSerializer.Instance.SerializePayload(MessageType.TestMessage, TestMessagePayload, 7);
        }
        sw.Stop();
        Console.WriteLine($"STJ Serialize TestMessage V7: {sw.ElapsedMilliseconds}ms for {Iterations} iterations");
    }

    #endregion

    #region TestMessage — Deserialize

    [TestMethod]
    public void Deserialize_TestMessage_V7_SystemTextJson()
    {
        var json = JsonDataSerializer.Instance.SerializePayload(MessageType.TestMessage, TestMessagePayload, 7);

        var msg = JsonDataSerializer.Instance.DeserializeMessage(json);
        JsonDataSerializer.Instance.DeserializePayload<TestMessagePayload>(msg);

        var sw = Stopwatch.StartNew();
        for (int i = 0; i < Iterations; i++)
        {
            msg = JsonDataSerializer.Instance.DeserializeMessage(json);
            JsonDataSerializer.Instance.DeserializePayload<TestMessagePayload>(msg);
        }
        sw.Stop();
        Console.WriteLine($"STJ Deserialize TestMessage V7: {sw.ElapsedMilliseconds}ms for {Iterations} iterations");
    }

    #endregion

    #region TestCasesFound — Serialize

    [TestMethod]
    public void Serialize_TestCasesFound_V1_SystemTextJson()
    {
        JsonDataSerializer.Instance.SerializePayload(MessageType.TestCasesFound, TestCases, 1);

        var sw = Stopwatch.StartNew();
        for (int i = 0; i < Iterations; i++)
        {
            JsonDataSerializer.Instance.SerializePayload(MessageType.TestCasesFound, TestCases, 1);
        }
        sw.Stop();
        Console.WriteLine($"STJ Serialize TestCasesFound V1: {sw.ElapsedMilliseconds}ms for {Iterations} iterations");
    }

    [TestMethod]
    public void Serialize_TestCasesFound_V7_SystemTextJson()
    {
        JsonDataSerializer.Instance.SerializePayload(MessageType.TestCasesFound, TestCases, 7);

        var sw = Stopwatch.StartNew();
        for (int i = 0; i < Iterations; i++)
        {
            JsonDataSerializer.Instance.SerializePayload(MessageType.TestCasesFound, TestCases, 7);
        }
        sw.Stop();
        Console.WriteLine($"STJ Serialize TestCasesFound V7: {sw.ElapsedMilliseconds}ms for {Iterations} iterations");
    }

    #endregion

    #region TestCasesFound — Deserialize

    [TestMethod]
    public void Deserialize_TestCasesFound_V7_SystemTextJson()
    {
        var json = JsonDataSerializer.Instance.SerializePayload(MessageType.TestCasesFound, TestCases, 7);

        var msg = JsonDataSerializer.Instance.DeserializeMessage(json);
        JsonDataSerializer.Instance.DeserializePayload<List<TestCase>>(msg);

        var sw = Stopwatch.StartNew();
        for (int i = 0; i < Iterations; i++)
        {
            msg = JsonDataSerializer.Instance.DeserializeMessage(json);
            JsonDataSerializer.Instance.DeserializePayload<List<TestCase>>(msg);
        }
        sw.Stop();
        Console.WriteLine($"STJ Deserialize TestCasesFound V7: {sw.ElapsedMilliseconds}ms for {Iterations} iterations");
    }

    #endregion

    #region DiscoveryComplete — Serialize

    [TestMethod]
    public void Serialize_DiscoveryComplete_V1_SystemTextJson()
    {
        JsonDataSerializer.Instance.SerializePayload(MessageType.DiscoveryComplete, DiscoveryCompletePayload, 1);

        var sw = Stopwatch.StartNew();
        for (int i = 0; i < Iterations; i++)
        {
            JsonDataSerializer.Instance.SerializePayload(MessageType.DiscoveryComplete, DiscoveryCompletePayload, 1);
        }
        sw.Stop();
        Console.WriteLine($"STJ Serialize DiscoveryComplete V1: {sw.ElapsedMilliseconds}ms for {Iterations} iterations");
    }

    [TestMethod]
    public void Serialize_DiscoveryComplete_V7_SystemTextJson()
    {
        JsonDataSerializer.Instance.SerializePayload(MessageType.DiscoveryComplete, DiscoveryCompletePayload, 7);

        var sw = Stopwatch.StartNew();
        for (int i = 0; i < Iterations; i++)
        {
            JsonDataSerializer.Instance.SerializePayload(MessageType.DiscoveryComplete, DiscoveryCompletePayload, 7);
        }
        sw.Stop();
        Console.WriteLine($"STJ Serialize DiscoveryComplete V7: {sw.ElapsedMilliseconds}ms for {Iterations} iterations");
    }

    #endregion

    #region DiscoveryComplete — Deserialize

    [TestMethod]
    public void Deserialize_DiscoveryComplete_V7_SystemTextJson()
    {
        var json = JsonDataSerializer.Instance.SerializePayload(MessageType.DiscoveryComplete, DiscoveryCompletePayload, 7);

        var msg = JsonDataSerializer.Instance.DeserializeMessage(json);
        JsonDataSerializer.Instance.DeserializePayload<DiscoveryCompletePayload>(msg);

        var sw = Stopwatch.StartNew();
        for (int i = 0; i < Iterations; i++)
        {
            msg = JsonDataSerializer.Instance.DeserializeMessage(json);
            JsonDataSerializer.Instance.DeserializePayload<DiscoveryCompletePayload>(msg);
        }
        sw.Stop();
        Console.WriteLine($"STJ Deserialize DiscoveryComplete V7: {sw.ElapsedMilliseconds}ms for {Iterations} iterations");
    }

    #endregion

    #region ExecutionComplete — Serialize

    [TestMethod]
    public void Serialize_ExecutionComplete_V1_SystemTextJson()
    {
        JsonDataSerializer.Instance.SerializePayload(MessageType.ExecutionComplete, ExecutionCompletePayload, 1);

        var sw = Stopwatch.StartNew();
        for (int i = 0; i < Iterations; i++)
        {
            JsonDataSerializer.Instance.SerializePayload(MessageType.ExecutionComplete, ExecutionCompletePayload, 1);
        }
        sw.Stop();
        Console.WriteLine($"STJ Serialize ExecutionComplete V1: {sw.ElapsedMilliseconds}ms for {Iterations} iterations");
    }

    [TestMethod]
    public void Serialize_ExecutionComplete_V7_SystemTextJson()
    {
        JsonDataSerializer.Instance.SerializePayload(MessageType.ExecutionComplete, ExecutionCompletePayload, 7);

        var sw = Stopwatch.StartNew();
        for (int i = 0; i < Iterations; i++)
        {
            JsonDataSerializer.Instance.SerializePayload(MessageType.ExecutionComplete, ExecutionCompletePayload, 7);
        }
        sw.Stop();
        Console.WriteLine($"STJ Serialize ExecutionComplete V7: {sw.ElapsedMilliseconds}ms for {Iterations} iterations");
    }

    #endregion

    #region ExecutionComplete — Deserialize

    [TestMethod]
    public void Deserialize_ExecutionComplete_V7_SystemTextJson()
    {
        var json = JsonDataSerializer.Instance.SerializePayload(MessageType.ExecutionComplete, ExecutionCompletePayload, 7);

        var msg = JsonDataSerializer.Instance.DeserializeMessage(json);
        JsonDataSerializer.Instance.DeserializePayload<TestRunCompletePayload>(msg);

        var sw = Stopwatch.StartNew();
        for (int i = 0; i < Iterations; i++)
        {
            msg = JsonDataSerializer.Instance.DeserializeMessage(json);
            JsonDataSerializer.Instance.DeserializePayload<TestRunCompletePayload>(msg);
        }
        sw.Stop();
        Console.WriteLine($"STJ Deserialize ExecutionComplete V7: {sw.ElapsedMilliseconds}ms for {Iterations} iterations");
    }

    #endregion

    #region Batch TestRunStatsChange — 1000 test results per message

    private static TestRunStatsPayload BuildBatchStatsPayload(int testCount)
    {
        var results = new List<TestResult>(testCount);
        var stats = new Dictionary<TestOutcome, long>
        {
            [TestOutcome.Passed] = 0,
            [TestOutcome.Failed] = 0,
        };

        for (int i = 0; i < testCount; i++)
        {
            var outcome = i % 10 == 0 ? TestOutcome.Failed : TestOutcome.Passed;
            stats[outcome]++;

            var tc = new TestCase(
                $"Contoso.Tests.Generated.TestClass{i / 10}.TestMethod{i}",
                new Uri("executor://MSTestAdapter/v2"),
                "Contoso.Tests.dll")
            {
                Id = new Guid(i, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0),
                DisplayName = $"TestMethod{i}(input: \"value with 'quotes' and \\backslash\")",
                CodeFilePath = $@"C:\src\Tests\TestClass{i / 10}.cs",
                LineNumber = i + 1,
                Traits = { new Trait("Category", "Generated"), new Trait("Priority", (i % 3).ToString(CultureInfo.InvariantCulture)) }
            };

            var tr = new TestResult(tc)
            {
                Outcome = outcome,
                Duration = TimeSpan.FromMilliseconds(i),
                StartTime = new DateTimeOffset(2026, 3, 20, 10, 0, 0, TimeSpan.Zero),
                EndTime = new DateTimeOffset(2026, 3, 20, 10, 0, 0, i, TimeSpan.Zero),
                ComputerName = "BUILD-AGENT-01",
            };

            if (outcome == TestOutcome.Failed)
            {
                tr.ErrorMessage = $"Assert.AreEqual failed. Expected: {i} Actual: {i + 1}";
                tr.ErrorStackTrace = $"   at Contoso.Tests.Generated.TestClass{i / 10}.TestMethod{i}() in C:\\src\\Tests\\TestClass{i / 10}.cs:line {i + 1}";
            }

            results.Add(tr);
        }

        var runStats = new TestRunStatistics(testCount, stats);
        return new TestRunStatsPayload
        {
            TestRunChangedArgs = new TestRunChangedEventArgs(
                runStats, results, Array.Empty<TestCase>()),
        };
    }

    [TestMethod]
    public void Serialize_BatchStatsChange_1000Results_V7()
    {
        var payload = BuildBatchStatsPayload(1000);
        // Warmup
        JsonDataSerializer.Instance.SerializePayload(MessageType.TestRunStatsChange, payload, 7);

        var sw = Stopwatch.StartNew();
        for (int i = 0; i < 10; i++)
        {
            JsonDataSerializer.Instance.SerializePayload(MessageType.TestRunStatsChange, payload, 7);
        }
        sw.Stop();
        Console.WriteLine($"Serialize 1000-result StatsChange V7: {sw.ElapsedMilliseconds}ms for 10 iterations ({sw.ElapsedMilliseconds / 10.0}ms avg)");
    }

    [TestMethod]
    public void Deserialize_BatchStatsChange_1000Results_V7()
    {
        var payload = BuildBatchStatsPayload(1000);
        var json = JsonDataSerializer.Instance.SerializePayload(MessageType.TestRunStatsChange, payload, 7);
        Console.WriteLine($"JSON size: {json.Length:N0} chars");

        // Warmup
        var msg = JsonDataSerializer.Instance.DeserializeMessage(json);
        JsonDataSerializer.Instance.DeserializePayload<TestRunStatsPayload>(msg);

        var sw = Stopwatch.StartNew();
        for (int i = 0; i < 10; i++)
        {
            msg = JsonDataSerializer.Instance.DeserializeMessage(json);
            JsonDataSerializer.Instance.DeserializePayload<TestRunStatsPayload>(msg);
        }
        sw.Stop();
        Console.WriteLine($"Deserialize 1000-result StatsChange V7: {sw.ElapsedMilliseconds}ms for 10 iterations ({sw.ElapsedMilliseconds / 10.0}ms avg)");
    }

    [TestMethod]
    public void RoundTrip_BatchStatsChange_1000Results_V7()
    {
        var payload = BuildBatchStatsPayload(1000);
        var json = JsonDataSerializer.Instance.SerializePayload(MessageType.TestRunStatsChange, payload, 7);
        var msg = JsonDataSerializer.Instance.DeserializeMessage(json);
        var result = JsonDataSerializer.Instance.DeserializePayload<TestRunStatsPayload>(msg);

        Assert.IsNotNull(result?.TestRunChangedArgs);
        Assert.AreEqual(1000, result.TestRunChangedArgs.NewTestResults!.Count());
        Assert.AreEqual(900L, result.TestRunChangedArgs.TestRunStatistics!.Stats![TestOutcome.Passed]);
        Assert.AreEqual(100L, result.TestRunChangedArgs.TestRunStatistics.Stats[TestOutcome.Failed]);

        // Spot-check a result
        var first = result.TestRunChangedArgs.NewTestResults!.First();
        Assert.AreEqual("Contoso.Tests.Generated.TestClass0.TestMethod0", first.TestCase.FullyQualifiedName);
        Assert.AreEqual(TestOutcome.Failed, first.Outcome);
        Assert.Contains("Assert.AreEqual failed", first.ErrorMessage!);

        var passed = result.TestRunChangedArgs.NewTestResults!.ElementAt(1);
        Assert.AreEqual(TestOutcome.Passed, passed.Outcome);
    }

    #endregion

}