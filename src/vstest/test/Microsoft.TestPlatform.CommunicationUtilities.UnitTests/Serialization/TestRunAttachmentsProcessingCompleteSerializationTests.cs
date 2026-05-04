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
/// Wire-format tests for <see cref="MessageType.TestRunAttachmentsProcessingComplete"/>
/// ("TestRunAttachmentsProcessing.Complete").
///
/// This message is sent when attachment processing finishes. The payload is
/// <see cref="TestRunAttachmentsProcessingCompletePayload"/> containing the completion event args
/// and the processed attachment sets.
///
/// Payload is identical for V1 and V7 serializers because it contains no TestCase/TestResult objects.
/// The only difference is the outer envelope: V1 omits the Version field, V7 includes it.
/// </summary>
[TestClass]
[TestCategory("Serialization")]
public class TestRunAttachmentsProcessingCompleteSerializationTests
{
    // ── Example payload ──────────────────────────────────────────────────
    private static readonly TestRunAttachmentsProcessingCompletePayload Payload = new()
    {
        AttachmentsProcessingCompleteEventArgs = new TestRunAttachmentsProcessingCompleteEventArgs(false, null),
        Attachments = new[] { new AttachmentSet(new Uri("datacollector://coverage"), "Code Coverage") },
    };

    // ── V1 wire format ───────────────────────────────────────────────────
    // Protocol versions 0, 1, 3 — no Version field in the envelope.
    private static readonly string V1Json = """
        {
          "MessageType": "TestRunAttachmentsProcessing.Complete",
          "Payload": {
            "AttachmentsProcessingCompleteEventArgs": {
              "IsCanceled": false,
              "Error": null,
              "Metrics": null
            },
            "Attachments": [
              {
                "Uri": "datacollector://coverage",
                "DisplayName": "Code Coverage",
                "Attachments": []
              }
            ]
          }
        }
        """;

    // ── V7 wire format ───────────────────────────────────────────────────
    // Protocol versions 2, 4, 5, 6, 7 — includes Version in the envelope.
    private static readonly string V7Json = """
        {
          "Version": 7,
          "MessageType": "TestRunAttachmentsProcessing.Complete",
          "Payload": {
            "AttachmentsProcessingCompleteEventArgs": {
              "IsCanceled": false,
              "Error": null,
              "Metrics": null
            },
            "Attachments": [
              {
                "Uri": "datacollector://coverage",
                "DisplayName": "Code Coverage",
                "Attachments": []
              }
            ]
          }
        }
        """;

    // ── Serialize ────────────────────────────────────────────────────────

    [TestMethod]
    public void SerializePayloadV1()
    {
        var json = JsonDataSerializer.Instance.SerializePayload(
            MessageType.TestRunAttachmentsProcessingComplete, Payload, version: 1);

        AssertJsonEqual(V1Json, json);
    }

    [TestMethod]
    public void SerializePayloadV7()
    {
        var json = JsonDataSerializer.Instance.SerializePayload(
            MessageType.TestRunAttachmentsProcessingComplete, Payload, version: 7);

        AssertJsonEqual(V7Json, json);
    }

    // ── Deserialize ──────────────────────────────────────────────────────

    [TestMethod]
    public void DeserializePayloadV1()
    {
        var message = JsonDataSerializer.Instance.DeserializeMessage(Minify(V1Json));
        var result = JsonDataSerializer.Instance.DeserializePayload<TestRunAttachmentsProcessingCompletePayload>(message);

        AssertPayloadFields(result);
    }

    [TestMethod]
    public void DeserializePayloadV7()
    {
        var message = JsonDataSerializer.Instance.DeserializeMessage(Minify(V7Json));
        var result = JsonDataSerializer.Instance.DeserializePayload<TestRunAttachmentsProcessingCompletePayload>(message);

        AssertPayloadFields(result);
    }

    // ── Round-trip ───────────────────────────────────────────────────────

    [TestMethod]
    [DataRow(1)]
    [DataRow(7)]
    public void RoundTrip(int version)
    {
        var json = JsonDataSerializer.Instance.SerializePayload(
            MessageType.TestRunAttachmentsProcessingComplete, Payload, version);
        var message = JsonDataSerializer.Instance.DeserializeMessage(json);
        var result = JsonDataSerializer.Instance.DeserializePayload<TestRunAttachmentsProcessingCompletePayload>(message);

        Assert.IsNotNull(result);
        Assert.IsNotNull(result.AttachmentsProcessingCompleteEventArgs);
        Assert.IsFalse(result.AttachmentsProcessingCompleteEventArgs.IsCanceled);
    }

    // ── Helpers ──────────────────────────────────────────────────────────

    private static void AssertPayloadFields(TestRunAttachmentsProcessingCompletePayload? result)
    {
        Assert.IsNotNull(result);
        Assert.IsNotNull(result.AttachmentsProcessingCompleteEventArgs);
        Assert.IsFalse(result.AttachmentsProcessingCompleteEventArgs.IsCanceled);
        Assert.IsNull(result.AttachmentsProcessingCompleteEventArgs.Error);
        Assert.IsNotNull(result.Attachments);
        var attachments = result.Attachments.ToList();
        Assert.HasCount(1, attachments);
        Assert.AreEqual(new Uri("datacollector://coverage"), attachments[0].Uri);
        Assert.AreEqual("Code Coverage", attachments[0].DisplayName);
    }

}
