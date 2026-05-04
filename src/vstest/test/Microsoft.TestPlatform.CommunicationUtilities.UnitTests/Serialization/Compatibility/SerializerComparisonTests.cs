// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;

using Microsoft.TestPlatform.CommunicationUtilities.UnitTests.NewtonsoftReference;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Engine.ClientProtocol;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using TestResult = Microsoft.VisualStudio.TestPlatform.ObjectModel.TestResult;

namespace Microsoft.TestPlatform.CommunicationUtilities.UnitTests.Serialization;

/// <summary>
/// Compares JsonDataSerializer.Instance (production) against
/// NewtonsoftJsonDataSerializer (reference) for every MessageType on v1 and v7.
/// </summary>
[TestClass]
[TestCategory("Serialization")]
public class SerializerComparisonTests
{
    private static readonly JsonDataSerializer Serializer = JsonDataSerializer.Instance;
    private static readonly NewtonsoftJsonDataSerializer Reference = NewtonsoftJsonDataSerializer.Instance;

    // ══════════════════════════════════════════════════════════════════════
    //  Diagnostics
    // ══════════════════════════════════════════════════════════════════════

    [TestMethod]
    public void VerifySerializerName()
    {
#if NETCOREAPP
        Assert.AreEqual("System.Text.Json", JsonDataSerializer.SerializerName);
#else
        Assert.AreEqual("Jsonite", JsonDataSerializer.SerializerName);
#endif
    }

    // ══════════════════════════════════════════════════════════════════════
    //  SerializeMessage (no payload)
    // ══════════════════════════════════════════════════════════════════════

    [TestMethod]
    public void SerializeMessage_NoPayload()
    {
        var ours = Serializer.SerializeMessage(MessageType.TestMessage);
        var reference = Reference.SerializeMessage(MessageType.TestMessage);

        // Both must deserialize to the same MessageType.
        // Newtonsoft may include "Payload":null while our serializer omits it — both are valid.
        var ourMsg = Serializer.DeserializeMessage(ours);
        var refMsg = Serializer.DeserializeMessage(reference);
        Assert.AreEqual(refMsg.MessageType, ourMsg.MessageType);
    }

    // ══════════════════════════════════════════════════════════════════════
    //  SerializePayload — compare production vs Newtonsoft for each type
    // ══════════════════════════════════════════════════════════════════════

    [TestMethod]
    [DataRow(1)]
    [DataRow(7)]
    public void SerializePayload_TestMessage(int version)
    {
        var ours = Serializer.SerializePayload(MessageType.TestMessage, TestMessagePayloadData, version);
        var reference = Reference.SerializePayload(MessageType.TestMessage, TestMessagePayloadData, version);

        SerializationTestHelpers.AssertJsonEqual(reference, ours,
            $"SerializePayload mismatch for TestMessage v{version}");
    }

    [TestMethod]
    [DataRow(1)]
    [DataRow(7)]
    public void SerializePayload_VersionCheck(int version)
    {
        var ours = Serializer.SerializePayload(MessageType.VersionCheck, VersionCheckPayload, version);
        var reference = Reference.SerializePayload(MessageType.VersionCheck, VersionCheckPayload, version);

        SerializationTestHelpers.AssertJsonEqual(reference, ours,
            $"SerializePayload mismatch for VersionCheck v{version}");
    }

    [TestMethod]
    [DataRow(1)]
    [DataRow(7)]
    public void SerializePayload_TestCasesFound(int version)
    {
        var ours = Serializer.SerializePayload(MessageType.TestCasesFound, TestCasesFoundPayload, version);
        var reference = Reference.SerializePayload(MessageType.TestCasesFound, TestCasesFoundPayload, version);

        SerializationTestHelpers.AssertJsonEqual(reference, ours,
            $"SerializePayload mismatch for TestCasesFound v{version}");
    }

    [TestMethod]
    [DataRow(1)]
    [DataRow(7)]
    public void SerializePayload_DiscoveryComplete(int version)
    {
        var ours = Serializer.SerializePayload(MessageType.DiscoveryComplete, DiscoveryCompletePayloadData, version);
        var reference = Reference.SerializePayload(MessageType.DiscoveryComplete, DiscoveryCompletePayloadData, version);

        SerializationTestHelpers.AssertJsonEqual(reference, ours,
            $"SerializePayload mismatch for DiscoveryComplete v{version}");
    }

    [TestMethod]
    [DataRow(1)]
    [DataRow(7)]
    public void SerializePayload_ExecutionComplete(int version)
    {
        var ours = Serializer.SerializePayload(MessageType.ExecutionComplete, ExecutionCompletePayloadData, version);
        var reference = Reference.SerializePayload(MessageType.ExecutionComplete, ExecutionCompletePayloadData, version);

        SerializationTestHelpers.AssertJsonEqual(reference, ours,
            $"SerializePayload mismatch for ExecutionComplete v{version}");
    }

    [TestMethod]
    [DataRow(1)]
    [DataRow(7)]
    public void SerializePayload_TestRunStatsChange(int version)
    {
        var ours = Serializer.SerializePayload(MessageType.TestRunStatsChange, TestRunStatsPayloadData, version);
        var reference = Reference.SerializePayload(MessageType.TestRunStatsChange, TestRunStatsPayloadData, version);

        SerializationTestHelpers.AssertJsonEqual(reference, ours,
            $"SerializePayload mismatch for TestRunStatsChange v{version}");
    }

    [TestMethod]
    [DataRow(1)]
    [DataRow(7)]
    public void SerializePayload_StartDiscovery(int version)
    {
        var ours = Serializer.SerializePayload(MessageType.StartDiscovery, StartDiscoveryPayload, version);
        var reference = Reference.SerializePayload(MessageType.StartDiscovery, StartDiscoveryPayload, version);

        SerializationTestHelpers.AssertJsonEqual(reference, ours,
            $"SerializePayload mismatch for StartDiscovery v{version}");
    }

    [TestMethod]
    [DataRow(1)]
    [DataRow(7)]
    public void SerializePayload_StartTestExecutionWithSources(int version)
    {
        var ours = Serializer.SerializePayload(MessageType.StartTestExecutionWithSources, StartTestExecutionWithSourcesPayload, version);
        var reference = Reference.SerializePayload(MessageType.StartTestExecutionWithSources, StartTestExecutionWithSourcesPayload, version);

        SerializationTestHelpers.AssertJsonEqual(reference, ours,
            $"SerializePayload mismatch for StartTestExecutionWithSources v{version}");
    }

    [TestMethod]
    [DataRow(1)]
    [DataRow(7)]
    public void SerializePayload_StartTestExecutionWithTests(int version)
    {
        var ours = Serializer.SerializePayload(MessageType.StartTestExecutionWithTests, StartTestExecutionWithTestsPayload, version);
        var reference = Reference.SerializePayload(MessageType.StartTestExecutionWithTests, StartTestExecutionWithTestsPayload, version);

        SerializationTestHelpers.AssertJsonEqual(reference, ours,
            $"SerializePayload mismatch for StartTestExecutionWithTests v{version}");
    }

    [TestMethod]
    [DataRow(1)]
    [DataRow(7)]
    public void SerializePayload_BeforeTestRunStart(int version)
    {
        var ours = Serializer.SerializePayload(MessageType.BeforeTestRunStart, BeforeTestRunStartPayloadData, version);
        var reference = Reference.SerializePayload(MessageType.BeforeTestRunStart, BeforeTestRunStartPayloadData, version);

        SerializationTestHelpers.AssertJsonEqual(reference, ours,
            $"SerializePayload mismatch for BeforeTestRunStart v{version}");
    }

    [TestMethod]
    [DataRow(1)]
    [DataRow(7)]
    public void SerializePayload_AfterTestRunEnd(int version)
    {
        var ours = Serializer.SerializePayload(MessageType.AfterTestRunEnd, AfterTestRunEndPayload, version);
        var reference = Reference.SerializePayload(MessageType.AfterTestRunEnd, AfterTestRunEndPayload, version);

        SerializationTestHelpers.AssertJsonEqual(reference, ours,
            $"SerializePayload mismatch for AfterTestRunEnd v{version}");
    }

    // ══════════════════════════════════════════════════════════════════════
    //  DeserializeMessage — verify header parsing from Newtonsoft-produced JSON
    // ══════════════════════════════════════════════════════════════════════

    [TestMethod]
    [DataRow(1)]
    [DataRow(7)]
    public void DeserializeMessage_TestMessage(int version)
    {
        var json = Reference.SerializePayload(MessageType.TestMessage, TestMessagePayloadData, version);

        var message = Serializer.DeserializeMessage(json);

        Assert.AreEqual(MessageType.TestMessage, message.MessageType);
        Assert.AreEqual(version > 1 ? version : 0, message.Version);
    }

    [TestMethod]
    [DataRow(1)]
    [DataRow(7)]
    public void DeserializeMessage_VersionCheck(int version)
    {
        var json = Reference.SerializePayload(MessageType.VersionCheck, VersionCheckPayload, version);

        var message = Serializer.DeserializeMessage(json);

        Assert.AreEqual(MessageType.VersionCheck, message.MessageType);
        Assert.AreEqual(version > 1 ? version : 0, message.Version);
    }

    [TestMethod]
    [DataRow(1)]
    [DataRow(7)]
    public void DeserializeMessage_TestCasesFound(int version)
    {
        var json = Reference.SerializePayload(MessageType.TestCasesFound, TestCasesFoundPayload, version);

        var message = Serializer.DeserializeMessage(json);

        Assert.AreEqual(MessageType.TestCasesFound, message.MessageType);
        Assert.AreEqual(version > 1 ? version : 0, message.Version);
    }

    [TestMethod]
    [DataRow(1)]
    [DataRow(7)]
    public void DeserializeMessage_DiscoveryComplete(int version)
    {
        var json = Reference.SerializePayload(MessageType.DiscoveryComplete, DiscoveryCompletePayloadData, version);

        var message = Serializer.DeserializeMessage(json);

        Assert.AreEqual(MessageType.DiscoveryComplete, message.MessageType);
        Assert.AreEqual(version > 1 ? version : 0, message.Version);
    }

    [TestMethod]
    [DataRow(1)]
    [DataRow(7)]
    public void DeserializeMessage_ExecutionComplete(int version)
    {
        var json = Reference.SerializePayload(MessageType.ExecutionComplete, ExecutionCompletePayloadData, version);

        var message = Serializer.DeserializeMessage(json);

        Assert.AreEqual(MessageType.ExecutionComplete, message.MessageType);
        Assert.AreEqual(version > 1 ? version : 0, message.Version);
    }

    [TestMethod]
    [DataRow(1)]
    [DataRow(7)]
    public void DeserializeMessage_TestRunStatsChange(int version)
    {
        var json = Reference.SerializePayload(MessageType.TestRunStatsChange, TestRunStatsPayloadData, version);

        var message = Serializer.DeserializeMessage(json);

        Assert.AreEqual(MessageType.TestRunStatsChange, message.MessageType);
        Assert.AreEqual(version > 1 ? version : 0, message.Version);
    }

    [TestMethod]
    [DataRow(1)]
    [DataRow(7)]
    public void DeserializeMessage_StartDiscovery(int version)
    {
        var json = Reference.SerializePayload(MessageType.StartDiscovery, StartDiscoveryPayload, version);

        var message = Serializer.DeserializeMessage(json);

        Assert.AreEqual(MessageType.StartDiscovery, message.MessageType);
        Assert.AreEqual(version > 1 ? version : 0, message.Version);
    }

    [TestMethod]
    [DataRow(1)]
    [DataRow(7)]
    public void DeserializeMessage_StartTestExecutionWithSources(int version)
    {
        var json = Reference.SerializePayload(MessageType.StartTestExecutionWithSources, StartTestExecutionWithSourcesPayload, version);

        var message = Serializer.DeserializeMessage(json);

        Assert.AreEqual(MessageType.StartTestExecutionWithSources, message.MessageType);
        Assert.AreEqual(version > 1 ? version : 0, message.Version);
    }

    [TestMethod]
    [DataRow(1)]
    [DataRow(7)]
    public void DeserializeMessage_StartTestExecutionWithTests(int version)
    {
        var json = Reference.SerializePayload(MessageType.StartTestExecutionWithTests, StartTestExecutionWithTestsPayload, version);

        var message = Serializer.DeserializeMessage(json);

        Assert.AreEqual(MessageType.StartTestExecutionWithTests, message.MessageType);
        Assert.AreEqual(version > 1 ? version : 0, message.Version);
    }

    [TestMethod]
    [DataRow(1)]
    [DataRow(7)]
    public void DeserializeMessage_BeforeTestRunStart(int version)
    {
        var json = Reference.SerializePayload(MessageType.BeforeTestRunStart, BeforeTestRunStartPayloadData, version);

        var message = Serializer.DeserializeMessage(json);

        Assert.AreEqual(MessageType.BeforeTestRunStart, message.MessageType);
        Assert.AreEqual(version > 1 ? version : 0, message.Version);
    }

    [TestMethod]
    [DataRow(1)]
    [DataRow(7)]
    public void DeserializeMessage_AfterTestRunEnd(int version)
    {
        var json = Reference.SerializePayload(MessageType.AfterTestRunEnd, AfterTestRunEndPayload, version);

        var message = Serializer.DeserializeMessage(json);

        Assert.AreEqual(MessageType.AfterTestRunEnd, message.MessageType);
        Assert.AreEqual(version > 1 ? version : 0, message.Version);
    }

    // ══════════════════════════════════════════════════════════════════════
    //  DeserializePayload roundtrip — Newtonsoft-serialized → our deserializer
    // ══════════════════════════════════════════════════════════════════════

    [TestMethod]
    [DataRow(1)]
    [DataRow(7)]
    public void DeserializePayload_TestMessage(int version)
    {
        var json = Reference.SerializePayload(MessageType.TestMessage, TestMessagePayloadData, version);

        var message = Serializer.DeserializeMessage(json);
        var payload = Serializer.DeserializePayload<TestMessagePayload>(message);

        Assert.IsNotNull(payload);
        Assert.AreEqual(TestMessagePayloadData.Message, payload.Message);
        Assert.AreEqual(TestMessagePayloadData.MessageLevel, payload.MessageLevel);
    }

    [TestMethod]
    [DataRow(1)]
    [DataRow(7)]
    public void DeserializePayload_VersionCheck(int version)
    {
        var json = Reference.SerializePayload(MessageType.VersionCheck, VersionCheckPayload, version);

        var message = Serializer.DeserializeMessage(json);
        var payload = Serializer.DeserializePayload<int>(message);

        Assert.AreEqual(VersionCheckPayload, payload);
    }

    [TestMethod]
    [DataRow(1)]
    [DataRow(7)]
    public void DeserializePayload_TestCasesFound(int version)
    {
        var json = Reference.SerializePayload(MessageType.TestCasesFound, TestCasesFoundPayload, version);

        var message = Serializer.DeserializeMessage(json);
        var payload = Serializer.DeserializePayload<List<TestCase>>(message);

        Assert.IsNotNull(payload);
        Assert.HasCount(1, payload);
        Assert.AreEqual("Contoso.Math.Tests.CalculatorTests.AddTest", payload[0].FullyQualifiedName);
        Assert.AreEqual(new Uri("executor://MSTestAdapter/v2"), payload[0].ExecutorUri);
    }

    [TestMethod]
    [DataRow(1)]
    [DataRow(7)]
    public void DeserializePayload_DiscoveryComplete(int version)
    {
        var json = Reference.SerializePayload(MessageType.DiscoveryComplete, DiscoveryCompletePayloadData, version);

        var message = Serializer.DeserializeMessage(json);
        var payload = Serializer.DeserializePayload<DiscoveryCompletePayload>(message);

        Assert.IsNotNull(payload);
        Assert.AreEqual(DiscoveryCompletePayloadData.TotalTests, payload.TotalTests);
        Assert.AreEqual(DiscoveryCompletePayloadData.IsAborted, payload.IsAborted);
    }

    [TestMethod]
    [DataRow(1)]
    [DataRow(7)]
    public void DeserializePayload_ExecutionComplete(int version)
    {
        var json = Reference.SerializePayload(MessageType.ExecutionComplete, ExecutionCompletePayloadData, version);

        var message = Serializer.DeserializeMessage(json);
        var payload = Serializer.DeserializePayload<TestRunCompletePayload>(message);

        Assert.IsNotNull(payload);
        Assert.IsNotNull(payload.TestRunCompleteArgs);
        Assert.IsFalse(payload.TestRunCompleteArgs.IsCanceled);
    }

    [TestMethod]
    [DataRow(1)]
    [DataRow(7)]
    public void DeserializePayload_TestRunStatsChange(int version)
    {
        var json = Reference.SerializePayload(MessageType.TestRunStatsChange, TestRunStatsPayloadData, version);

        var message = Serializer.DeserializeMessage(json);
        var payload = Serializer.DeserializePayload<TestRunStatsPayload>(message);

        Assert.IsNotNull(payload);
        Assert.IsNotNull(payload.TestRunChangedArgs);
    }

    [TestMethod]
    [DataRow(1)]
    [DataRow(7)]
    public void DeserializePayload_StartDiscovery(int version)
    {
        var json = Reference.SerializePayload(MessageType.StartDiscovery, StartDiscoveryPayload, version);

        var message = Serializer.DeserializeMessage(json);
        var payload = Serializer.DeserializePayload<DiscoveryCriteria>(message);

        Assert.IsNotNull(payload);
        Assert.AreEqual(StartDiscoveryPayload.FrequencyOfDiscoveredTestsEvent, payload.FrequencyOfDiscoveredTestsEvent);
        Assert.AreEqual("Category=Unit", payload.TestCaseFilter);
    }

    [TestMethod]
    [DataRow(1)]
    [DataRow(7)]
    public void DeserializePayload_StartTestExecutionWithSources(int version)
    {
        var json = Reference.SerializePayload(MessageType.StartTestExecutionWithSources, StartTestExecutionWithSourcesPayload, version);

        var message = Serializer.DeserializeMessage(json);
        var payload = Serializer.DeserializePayload<TestRunCriteriaWithSources>(message);

        Assert.IsNotNull(payload);
        Assert.AreEqual(StartTestExecutionWithSourcesPayload.Package, payload.Package);
        Assert.IsNotNull(payload.AdapterSourceMap);
    }

    [TestMethod]
    [DataRow(1)]
    [DataRow(7)]
    public void DeserializePayload_StartTestExecutionWithTests(int version)
    {
        var json = Reference.SerializePayload(MessageType.StartTestExecutionWithTests, StartTestExecutionWithTestsPayload, version);

        var message = Serializer.DeserializeMessage(json);
        var payload = Serializer.DeserializePayload<TestRunCriteriaWithTests>(message);

        Assert.IsNotNull(payload);
        Assert.AreEqual(StartTestExecutionWithTestsPayload.Package, payload.Package);
        Assert.IsNotNull(payload.Tests);
    }

    [TestMethod]
    [DataRow(1)]
    [DataRow(7)]
    public void DeserializePayload_BeforeTestRunStart(int version)
    {
        var json = Reference.SerializePayload(MessageType.BeforeTestRunStart, BeforeTestRunStartPayloadData, version);

        var message = Serializer.DeserializeMessage(json);
        var payload = Serializer.DeserializePayload<BeforeTestRunStartPayload>(message);

        Assert.IsNotNull(payload);
        Assert.AreEqual(BeforeTestRunStartPayloadData.IsTelemetryOptedIn, payload.IsTelemetryOptedIn);
        Assert.IsNotNull(payload.SettingsXml);
    }

    [TestMethod]
    [DataRow(1)]
    [DataRow(7)]
    public void DeserializePayload_AfterTestRunEnd(int version)
    {
        var json = Reference.SerializePayload(MessageType.AfterTestRunEnd, AfterTestRunEndPayload, version);

        var message = Serializer.DeserializeMessage(json);
        var payload = Serializer.DeserializePayload<bool>(message);

        Assert.AreEqual(AfterTestRunEndPayload, payload);
    }

    // ══════════════════════════════════════════════════════════════════════
    //  Performance — our serializer vs Newtonsoft
    // ══════════════════════════════════════════════════════════════════════

    [TestMethod]
    [TestCategory("Performance")]
    public void Performance_SerializePayload_TestMessage()
    {
        const int iterations = 1000;

        // Warm up
        Serializer.SerializePayload(MessageType.TestMessage, TestMessagePayloadData, 7);
        Reference.SerializePayload(MessageType.TestMessage, TestMessagePayloadData, 7);

        var swOurs = Stopwatch.StartNew();
        for (int i = 0; i < iterations; i++)
            Serializer.SerializePayload(MessageType.TestMessage, TestMessagePayloadData, 7);
        swOurs.Stop();

        var swRef = Stopwatch.StartNew();
        for (int i = 0; i < iterations; i++)
            Reference.SerializePayload(MessageType.TestMessage, TestMessagePayloadData, 7);
        swRef.Stop();

        Console.WriteLine($"[{JsonDataSerializer.SerializerName}] Ours={swOurs.ElapsedMilliseconds}ms Newtonsoft={swRef.ElapsedMilliseconds}ms");

        if (swRef.ElapsedMilliseconds >= 10)
        {
            Assert.IsLessThanOrEqualTo(swRef.ElapsedMilliseconds * 3, swOurs.ElapsedMilliseconds,
                $"Performance regression: Ours={swOurs.ElapsedMilliseconds}ms Newtonsoft={swRef.ElapsedMilliseconds}ms");
        }
    }

    [TestMethod]
    [TestCategory("Performance")]
    public void Performance_SerializePayload_ExecutionComplete()
    {
        const int iterations = 1000;

        // Warm up
        Serializer.SerializePayload(MessageType.ExecutionComplete, ExecutionCompletePayloadData, 7);
        Reference.SerializePayload(MessageType.ExecutionComplete, ExecutionCompletePayloadData, 7);

        var swOurs = Stopwatch.StartNew();
        for (int i = 0; i < iterations; i++)
            Serializer.SerializePayload(MessageType.ExecutionComplete, ExecutionCompletePayloadData, 7);
        swOurs.Stop();

        var swRef = Stopwatch.StartNew();
        for (int i = 0; i < iterations; i++)
            Reference.SerializePayload(MessageType.ExecutionComplete, ExecutionCompletePayloadData, 7);
        swRef.Stop();

        Console.WriteLine($"[{JsonDataSerializer.SerializerName}] Ours={swOurs.ElapsedMilliseconds}ms Newtonsoft={swRef.ElapsedMilliseconds}ms");

        if (swRef.ElapsedMilliseconds >= 10)
        {
            Assert.IsLessThanOrEqualTo(swRef.ElapsedMilliseconds * 3, swOurs.ElapsedMilliseconds,
                $"Performance regression: Ours={swOurs.ElapsedMilliseconds}ms Newtonsoft={swRef.ElapsedMilliseconds}ms");
        }
    }

    [TestMethod]
    [TestCategory("Performance")]
    public void Performance_RoundTrip_TestMessage()
    {
        const int iterations = 1000;
        var json = Reference.SerializePayload(MessageType.TestMessage, TestMessagePayloadData, 7);

        // Warm up
        var msg = Serializer.DeserializeMessage(json);
        Serializer.DeserializePayload<TestMessagePayload>(msg);
        var refMsg = Reference.DeserializeMessage(json);
        Reference.DeserializePayload<TestMessagePayload>(refMsg);

        var swOurs = Stopwatch.StartNew();
        for (int i = 0; i < iterations; i++)
        {
            msg = Serializer.DeserializeMessage(json);
            Serializer.DeserializePayload<TestMessagePayload>(msg);
        }
        swOurs.Stop();

        var swRef = Stopwatch.StartNew();
        for (int i = 0; i < iterations; i++)
        {
            refMsg = Reference.DeserializeMessage(json);
            Reference.DeserializePayload<TestMessagePayload>(refMsg);
        }
        swRef.Stop();

        Console.WriteLine($"[{JsonDataSerializer.SerializerName}] RoundTrip TestMessage: Ours={swOurs.ElapsedMilliseconds}ms Newtonsoft={swRef.ElapsedMilliseconds}ms");

        if (swRef.ElapsedMilliseconds >= 10)
        {
            Assert.IsLessThanOrEqualTo(swRef.ElapsedMilliseconds * 3, swOurs.ElapsedMilliseconds,
                $"Performance regression: Ours={swOurs.ElapsedMilliseconds}ms Newtonsoft={swRef.ElapsedMilliseconds}ms");
        }
    }

    [TestMethod]
    [TestCategory("Performance")]
    public void Performance_RoundTrip_ExecutionComplete()
    {
        const int iterations = 1000;
        var json = Reference.SerializePayload(MessageType.ExecutionComplete, ExecutionCompletePayloadData, 7);

        // Warm up
        var msg = Serializer.DeserializeMessage(json);
        Serializer.DeserializePayload<TestRunCompletePayload>(msg);
        var refMsg = Reference.DeserializeMessage(json);
        Reference.DeserializePayload<TestRunCompletePayload>(refMsg);

        var swOurs = Stopwatch.StartNew();
        for (int i = 0; i < iterations; i++)
        {
            msg = Serializer.DeserializeMessage(json);
            Serializer.DeserializePayload<TestRunCompletePayload>(msg);
        }
        swOurs.Stop();

        var swRef = Stopwatch.StartNew();
        for (int i = 0; i < iterations; i++)
        {
            refMsg = Reference.DeserializeMessage(json);
            Reference.DeserializePayload<TestRunCompletePayload>(refMsg);
        }
        swRef.Stop();

        Console.WriteLine($"[{JsonDataSerializer.SerializerName}] RoundTrip ExecutionComplete: Ours={swOurs.ElapsedMilliseconds}ms Newtonsoft={swRef.ElapsedMilliseconds}ms");

        if (swRef.ElapsedMilliseconds >= 10)
        {
            Assert.IsLessThanOrEqualTo(swRef.ElapsedMilliseconds * 3, swOurs.ElapsedMilliseconds,
                $"Performance regression: Ours={swOurs.ElapsedMilliseconds}ms Newtonsoft={swRef.ElapsedMilliseconds}ms");
        }
    }

    // ══════════════════════════════════════════════════════════════════════
    //  Test Data
    // ══════════════════════════════════════════════════════════════════════

    private static readonly TestMessagePayload TestMessagePayloadData = new()
    {
        MessageLevel = TestMessageLevel.Warning,
        Message = "Test 'CalculatorTests.AddTest' was skipped: requires .NET 8"
    };

    private static readonly int VersionCheckPayload = 7;

    private static readonly List<TestCase> TestCasesFoundPayload = new()
    {
        BuildTestCase()
    };

    private static readonly DiscoveryCompletePayload DiscoveryCompletePayloadData = new()
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

    private static readonly TestRunCompletePayload ExecutionCompletePayloadData = BuildExecutionCompletePayload();

    private static readonly TestRunStatsPayload TestRunStatsPayloadData = BuildTestRunStatsPayload();

    private static readonly DiscoveryCriteria StartDiscoveryPayload = BuildDiscoveryCriteria();

    private static readonly TestRunCriteriaWithSources StartTestExecutionWithSourcesPayload = BuildTestRunCriteriaWithSources();

    private static readonly TestRunCriteriaWithTests StartTestExecutionWithTestsPayload = BuildTestRunCriteriaWithTests();

    private static readonly BeforeTestRunStartPayload BeforeTestRunStartPayloadData = new()
    {
        SettingsXml = "<RunSettings><DataCollectionRunSettings><DataCollectors><DataCollector friendlyName=\"Code Coverage\"><Configuration><CodeCoverage /></Configuration></DataCollector></DataCollectors></DataCollectionRunSettings></RunSettings>",
        Sources = new List<string> { "Contoso.Math.Tests.dll", "Contoso.Core.Tests.dll" },
        IsTelemetryOptedIn = true
    };

    private static readonly bool AfterTestRunEndPayload = true;

    // ── Builder Methods ──────────────────────────────────────────────────

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

    private static TestRunStatsPayload BuildTestRunStatsPayload()
    {
        var tc = new TestCase(
            "Contoso.Math.Tests.CalculatorTests.DivideTest",
            new Uri("executor://MSTestAdapter/v2"),
            "Contoso.Math.Tests.dll")
        {
            Id = new Guid("c3d4e5f6-a7b8-9012-cdef-123456789012"),
        };

        var tr = new TestResult(tc)
        {
            Outcome = TestOutcome.Failed,
            ErrorMessage = "Assert.AreEqual failed. Expected:<0.5>. Actual:<0>.",
            ErrorStackTrace = @"   at Contoso.Math.Tests.CalculatorTests.DivideTest() in C:\src\CalculatorTests.cs:line 55",
            DisplayName = "DivideTest",
            Duration = TimeSpan.FromMilliseconds(3),
            StartTime = new DateTimeOffset(2026, 3, 20, 10, 0, 1, TimeSpan.Zero),
            EndTime = new DateTimeOffset(2026, 3, 20, 10, 0, 1, 3, TimeSpan.Zero),
        };

        var stats = new Dictionary<TestOutcome, long> { [TestOutcome.Failed] = 1 };
        var runStats = new TestRunStatistics(1, stats);

        return new TestRunStatsPayload
        {
            TestRunChangedArgs = new TestRunChangedEventArgs(
                runStats, new[] { tr }, Array.Empty<TestCase>()),
        };
    }

    private static DiscoveryCriteria BuildDiscoveryCriteria()
    {
        var criteria = new DiscoveryCriteria(
            new[] { "Contoso.Math.Tests.dll", "Contoso.Core.Tests.dll" },
            frequencyOfDiscoveredTestsEvent: 10,
            discoveredTestEventTimeout: TimeSpan.FromSeconds(30),
            runSettings: @"<RunSettings><RunConfiguration><ResultsDirectory>.\TestResults</ResultsDirectory></RunConfiguration></RunSettings>");

        criteria.TestCaseFilter = "Category=Unit";

        return criteria;
    }

    private static TestRunCriteriaWithSources BuildTestRunCriteriaWithSources()
    {
        var ctx = new TestExecutionContext(
            frequencyOfRunStatsChangeEvent: 10,
            runStatsChangeEventTimeout: TimeSpan.FromSeconds(30),
            inIsolation: false,
            keepAlive: true,
            isDataCollectionEnabled: false,
            areTestCaseLevelEventsRequired: true,
            hasTestRun: true,
            isDebug: false,
            testCaseFilter: null,
            filterOptions: null);

        return new TestRunCriteriaWithSources(
            new Dictionary<string, IEnumerable<string>>
            {
                ["executor://MSTestAdapter/v2"] = new[] { "Contoso.Math.Tests.dll", "Contoso.Core.Tests.dll" }
            },
            "Contoso.Math.Tests",
            @"<RunSettings><RunConfiguration><ResultsDirectory>.\TestResults</ResultsDirectory></RunConfiguration></RunSettings>",
            ctx);
    }

    private static TestRunCriteriaWithTests BuildTestRunCriteriaWithTests()
    {
        var testCase = BuildTestCase();

        var ctx = new TestExecutionContext(
            frequencyOfRunStatsChangeEvent: 10,
            runStatsChangeEventTimeout: TimeSpan.FromSeconds(30),
            inIsolation: false,
            keepAlive: true,
            isDataCollectionEnabled: false,
            areTestCaseLevelEventsRequired: true,
            hasTestRun: true,
            isDebug: false,
            testCaseFilter: null,
            filterOptions: null);

        return new TestRunCriteriaWithTests(
            new[] { testCase },
            "Contoso.Math.Tests",
            @"<RunSettings><RunConfiguration><ResultsDirectory>.\TestResults</ResultsDirectory></RunConfiguration></RunSettings>",
            ctx);
    }
}


