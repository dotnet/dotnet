// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Linq;

using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Microsoft.TestPlatform.CommunicationUtilities.UnitTests.Serialization.SerializationTestHelpers;

namespace Microsoft.TestPlatform.CommunicationUtilities.UnitTests.Serialization;

/// <summary>
/// Wire-format tests for <see cref="MessageType.TestRunAttachmentsProcessingStart"/>
/// ("TestRunAttachmentsProcessing.Start").
///
/// This message is sent to start processing test run attachments (e.g., merging code coverage data).
/// The payload is <see cref="TestRunAttachmentsProcessingPayload"/> containing the attachment sets
/// to process, invoked data collectors, run settings, and a flag for metrics collection.
///
/// Payload is identical for V1 and V7 serializers because it contains no TestCase/TestResult objects.
/// The only difference is the outer envelope: V1 omits the Version field, V7 includes it.
/// </summary>
[TestClass]
[TestCategory("Serialization")]
public class TestRunAttachmentsProcessingStartSerializationTests
{
    // ── Example payload ──────────────────────────────────────────────────
    private static readonly TestRunAttachmentsProcessingPayload Payload = new()
    {
        Attachments = new[] { new AttachmentSet(new Uri("datacollector://coverage"), "Code Coverage") },
        RunSettings = "<RunSettings/>",
        CollectMetrics = true,
    };

    // ── V1 wire format ───────────────────────────────────────────────────
    // Protocol versions 0, 1, 3 — no Version field in the envelope.
    private static readonly string V1Json = """
        {
          "MessageType": "TestRunAttachmentsProcessing.Start",
          "Payload": {
            "Attachments": [
              {
                "Uri": "datacollector://coverage",
                "DisplayName": "Code Coverage",
                "Attachments": []
              }
            ],
            "InvokedDataCollectors": null,
            "RunSettings": "<RunSettings/>",
            "CollectMetrics": true
          }
        }
        """;

    // ── V7 wire format ───────────────────────────────────────────────────
    // Protocol versions 2, 4, 5, 6, 7 — includes Version in the envelope.
    private static readonly string V7Json = """
        {
          "Version": 7,
          "MessageType": "TestRunAttachmentsProcessing.Start",
          "Payload": {
            "Attachments": [
              {
                "Uri": "datacollector://coverage",
                "DisplayName": "Code Coverage",
                "Attachments": []
              }
            ],
            "InvokedDataCollectors": null,
            "RunSettings": "<RunSettings/>",
            "CollectMetrics": true
          }
        }
        """;

    // ── Serialize ────────────────────────────────────────────────────────

    [TestMethod]
    public void SerializePayloadV1()
    {
        var json = JsonDataSerializer.Instance.SerializePayload(
            MessageType.TestRunAttachmentsProcessingStart, Payload, version: 1);

        AssertJsonEqual(V1Json, json);
    }

    [TestMethod]
    public void SerializePayloadV7()
    {
        var json = JsonDataSerializer.Instance.SerializePayload(
            MessageType.TestRunAttachmentsProcessingStart, Payload, version: 7);

        AssertJsonEqual(V7Json, json);
    }

    // ── Deserialize ──────────────────────────────────────────────────────

    [TestMethod]
    public void DeserializePayloadV1()
    {
        var message = JsonDataSerializer.Instance.DeserializeMessage(Minify(V1Json));
        var result = JsonDataSerializer.Instance.DeserializePayload<TestRunAttachmentsProcessingPayload>(message);

        AssertPayloadFields(result);
    }

    [TestMethod]
    public void DeserializePayloadV7()
    {
        var message = JsonDataSerializer.Instance.DeserializeMessage(Minify(V7Json));
        var result = JsonDataSerializer.Instance.DeserializePayload<TestRunAttachmentsProcessingPayload>(message);

        AssertPayloadFields(result);
    }

    // ── Round-trip ───────────────────────────────────────────────────────

    [TestMethod]
    [DataRow(1)]
    [DataRow(7)]
    public void RoundTrip(int version)
    {
        var json = JsonDataSerializer.Instance.SerializePayload(
            MessageType.TestRunAttachmentsProcessingStart, Payload, version);
        var message = JsonDataSerializer.Instance.DeserializeMessage(json);
        var result = JsonDataSerializer.Instance.DeserializePayload<TestRunAttachmentsProcessingPayload>(message);

        Assert.IsNotNull(result);
        Assert.IsTrue(result.CollectMetrics);
        Assert.AreEqual("<RunSettings/>", result.RunSettings);
    }

    // ── Helpers ──────────────────────────────────────────────────────────

    private static void AssertPayloadFields(TestRunAttachmentsProcessingPayload? result)
    {
        Assert.IsNotNull(result);
        Assert.IsNotNull(result.Attachments);
        var attachments = result.Attachments.ToList();
        Assert.HasCount(1, attachments);
        Assert.AreEqual(new Uri("datacollector://coverage"), attachments[0].Uri);
        Assert.AreEqual("Code Coverage", attachments[0].DisplayName);
        Assert.IsNull(result.InvokedDataCollectors);
        Assert.AreEqual("<RunSettings/>", result.RunSettings);
        Assert.IsTrue(result.CollectMetrics);
    }

}
