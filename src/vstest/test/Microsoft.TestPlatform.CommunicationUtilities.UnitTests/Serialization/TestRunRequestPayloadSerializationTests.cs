// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Microsoft.TestPlatform.CommunicationUtilities.UnitTests.Serialization.SerializationTestHelpers;

namespace Microsoft.TestPlatform.CommunicationUtilities.UnitTests.Serialization;

/// <summary>
/// Wire-format tests for <see cref="TestRunRequestPayload"/> used by four message types:
/// <list type="bullet">
///   <item><see cref="MessageType.GetTestRunnerProcessStartInfoForRunAll"/> ("TestExecution.GetTestRunnerProcessStartInfoForRunAll")</item>
///   <item><see cref="MessageType.GetTestRunnerProcessStartInfoForRunSelected"/> ("TestExecution.GetTestRunnerProcessStartInfoForRunSelected")</item>
///   <item><see cref="MessageType.TestRunAllSourcesWithDefaultHost"/> ("TestExecution.RunAllWithDefaultHost")</item>
///   <item><see cref="MessageType.TestRunSelectedTestCasesDefaultHost"/> ("TestExecution.RunSelectedWithDefaultHost")</item>
/// </list>
///
/// These messages carry a <see cref="TestRunRequestPayload"/> specifying which test sources
/// to run, run settings, and options. The payload shape is the same for all four; only
/// the message type string differs.
///
/// Payload is identical for V1 and V7 serializers because it contains no TestCase/TestResult objects.
/// The only difference is the outer envelope: V1 omits the Version field, V7 includes it.
/// </summary>
[TestClass]
[TestCategory("Serialization")]
public class TestRunRequestPayloadSerializationTests
{
    // ── Example payload ──────────────────────────────────────────────────
    // Minimal TestRunRequestPayload using source-based execution.
    private static readonly TestRunRequestPayload Payload = new()
    {
        Sources = new List<string> { "Tests.dll" },
        RunSettings = "<RunSettings/>",
        KeepAlive = true,
        DebuggingEnabled = false,
    };

    // ── V1 wire format ───────────────────────────────────────────────────
    // Protocol versions 0, 1, 3 — no Version field in the envelope.
    // Note: The message type used here is GetTestRunnerProcessStartInfoForRunAll;
    // other message types use the same payload shape.
    private static readonly string V1Json = """
        {
          "MessageType": "TestExecution.GetTestRunnerProcessStartInfoForRunAll",
          "Payload": {
            "Sources": [
              "Tests.dll"
            ],
            "TestCases": null,
            "RunSettings": "<RunSettings/>",
            "KeepAlive": true,
            "DebuggingEnabled": false,
            "TestPlatformOptions": null,
            "TestSessionInfo": null
          }
        }
        """;

    // ── V7 wire format ───────────────────────────────────────────────────
    // Protocol versions 2, 4, 5, 6, 7 — includes Version in the envelope.
    private static readonly string V7Json = """
        {
          "Version": 7,
          "MessageType": "TestExecution.GetTestRunnerProcessStartInfoForRunAll",
          "Payload": {
            "Sources": [
              "Tests.dll"
            ],
            "TestCases": null,
            "RunSettings": "<RunSettings/>",
            "KeepAlive": true,
            "DebuggingEnabled": false,
            "TestPlatformOptions": null,
            "TestSessionInfo": null
          }
        }
        """;

    // ── Serialize ────────────────────────────────────────────────────────

    [TestMethod]
    public void SerializePayloadV1()
    {
        var json = JsonDataSerializer.Instance.SerializePayload(
            MessageType.GetTestRunnerProcessStartInfoForRunAll, Payload, version: 1);

        AssertJsonEqual(V1Json, json);
    }

    [TestMethod]
    public void SerializePayloadV7()
    {
        var json = JsonDataSerializer.Instance.SerializePayload(
            MessageType.GetTestRunnerProcessStartInfoForRunAll, Payload, version: 7);

        AssertJsonEqual(V7Json, json);
    }

    // ── Deserialize ──────────────────────────────────────────────────────

    [TestMethod]
    public void DeserializePayloadV1()
    {
        var message = JsonDataSerializer.Instance.DeserializeMessage(Minify(V1Json));
        var result = JsonDataSerializer.Instance.DeserializePayload<TestRunRequestPayload>(message);

        AssertPayloadFields(result);
    }

    [TestMethod]
    public void DeserializePayloadV7()
    {
        var message = JsonDataSerializer.Instance.DeserializeMessage(Minify(V7Json));
        var result = JsonDataSerializer.Instance.DeserializePayload<TestRunRequestPayload>(message);

        AssertPayloadFields(result);
    }

    // ── Round-trip ───────────────────────────────────────────────────────

    [TestMethod]
    [DataRow(1)]
    [DataRow(7)]
    public void RoundTrip(int version)
    {
        var json = JsonDataSerializer.Instance.SerializePayload(
            MessageType.GetTestRunnerProcessStartInfoForRunAll, Payload, version);
        var message = JsonDataSerializer.Instance.DeserializeMessage(json);
        var result = JsonDataSerializer.Instance.DeserializePayload<TestRunRequestPayload>(message);

        Assert.IsNotNull(result);
        Assert.AreEqual(Payload.KeepAlive, result.KeepAlive);
        Assert.AreEqual(Payload.DebuggingEnabled, result.DebuggingEnabled);
        Assert.AreEqual(Payload.RunSettings, result.RunSettings);
    }

    // ── Additional message types ─────────────────────────────────────────

    [TestMethod]
    [DataRow(1)]
    [DataRow(7)]
    public void RoundTrip_GetTestRunnerProcessStartInfoForRunSelected(int version)
    {
        var json = JsonDataSerializer.Instance.SerializePayload(
            MessageType.GetTestRunnerProcessStartInfoForRunSelected, Payload, version);
        var message = JsonDataSerializer.Instance.DeserializeMessage(json);
        var result = JsonDataSerializer.Instance.DeserializePayload<TestRunRequestPayload>(message);

        Assert.IsNotNull(result);
        Assert.AreEqual("TestExecution.GetTestRunnerProcessStartInfoForRunSelected", message.MessageType);
        Assert.AreEqual(Payload.RunSettings, result.RunSettings);
    }

    [TestMethod]
    [DataRow(1)]
    [DataRow(7)]
    public void RoundTrip_TestRunAllSourcesWithDefaultHost(int version)
    {
        var json = JsonDataSerializer.Instance.SerializePayload(
            MessageType.TestRunAllSourcesWithDefaultHost, Payload, version);
        var message = JsonDataSerializer.Instance.DeserializeMessage(json);
        var result = JsonDataSerializer.Instance.DeserializePayload<TestRunRequestPayload>(message);

        Assert.IsNotNull(result);
        Assert.AreEqual("TestExecution.RunAllWithDefaultHost", message.MessageType);
        Assert.AreEqual(Payload.RunSettings, result.RunSettings);
    }

    [TestMethod]
    [DataRow(1)]
    [DataRow(7)]
    public void RoundTrip_TestRunSelectedTestCasesDefaultHost(int version)
    {
        var json = JsonDataSerializer.Instance.SerializePayload(
            MessageType.TestRunSelectedTestCasesDefaultHost, Payload, version);
        var message = JsonDataSerializer.Instance.DeserializeMessage(json);
        var result = JsonDataSerializer.Instance.DeserializePayload<TestRunRequestPayload>(message);

        Assert.IsNotNull(result);
        Assert.AreEqual("TestExecution.RunSelectedWithDefaultHost", message.MessageType);
        Assert.AreEqual(Payload.RunSettings, result.RunSettings);
    }

    // ── Helpers ──────────────────────────────────────────────────────────

    private static void AssertPayloadFields(TestRunRequestPayload? result)
    {
        Assert.IsNotNull(result);
        Assert.IsNotNull(result.Sources);
        Assert.HasCount(1, result.Sources);
        Assert.AreEqual("Tests.dll", result.Sources[0]);
        Assert.IsNull(result.TestCases);
        Assert.AreEqual("<RunSettings/>", result.RunSettings);
        Assert.IsTrue(result.KeepAlive);
        Assert.IsFalse(result.DebuggingEnabled);
        Assert.IsNull(result.TestPlatformOptions);
        Assert.IsNull(result.TestSessionInfo);
    }

}
