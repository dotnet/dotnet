// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Linq;

using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Microsoft.TestPlatform.CommunicationUtilities.UnitTests.Serialization.SerializationTestHelpers;

namespace Microsoft.TestPlatform.CommunicationUtilities.UnitTests.Serialization;

/// <summary>
/// Wire-format tests for <see cref="MessageType.TestRunAttachmentsProcessingProgress"/>
/// ("TestRunAttachmentsProcessing.Progress").
///
/// This message is sent to report progress during attachment processing. The payload is
/// <see cref="TestRunAttachmentsProcessingProgressPayload"/> containing the progress event args
/// with processor index, URIs, progress percentage, and total processor count.
///
/// Payload is identical for V1 and V7 serializers because it contains no TestCase/TestResult objects.
/// The only difference is the outer envelope: V1 omits the Version field, V7 includes it.
/// </summary>
[TestClass]
[TestCategory("Serialization")]
public class TestRunAttachmentsProcessingProgressSerializationTests
{
    // ── Example payload ──────────────────────────────────────────────────
    private static readonly TestRunAttachmentsProcessingProgressPayload Payload = new()
    {
        AttachmentsProcessingProgressEventArgs = new TestRunAttachmentsProcessingProgressEventArgs(
            1, new[] { new Uri("datacollector://coverage") }, 50, 3),
    };

    // ── V1 wire format ───────────────────────────────────────────────────
    // Protocol versions 0, 1, 3 — no Version field in the envelope.
    private static readonly string V1Json = """
        {
          "MessageType": "TestRunAttachmentsProcessing.Progress",
          "Payload": {
            "AttachmentsProcessingProgressEventArgs": {
              "CurrentAttachmentProcessorIndex": 1,
              "CurrentAttachmentProcessorUris": [
                "datacollector://coverage"
              ],
              "CurrentAttachmentProcessorProgress": 50,
              "AttachmentProcessorsCount": 3
            }
          }
        }
        """;

    // ── V7 wire format ───────────────────────────────────────────────────
    // Protocol versions 2, 4, 5, 6, 7 — includes Version in the envelope.
    private static readonly string V7Json = """
        {
          "Version": 7,
          "MessageType": "TestRunAttachmentsProcessing.Progress",
          "Payload": {
            "AttachmentsProcessingProgressEventArgs": {
              "CurrentAttachmentProcessorIndex": 1,
              "CurrentAttachmentProcessorUris": [
                "datacollector://coverage"
              ],
              "CurrentAttachmentProcessorProgress": 50,
              "AttachmentProcessorsCount": 3
            }
          }
        }
        """;

    // ── Serialize ────────────────────────────────────────────────────────

    [TestMethod]
    public void SerializePayloadV1()
    {
        var json = JsonDataSerializer.Instance.SerializePayload(
            MessageType.TestRunAttachmentsProcessingProgress, Payload, version: 1);

        AssertJsonEqual(V1Json, json);
    }

    [TestMethod]
    public void SerializePayloadV7()
    {
        var json = JsonDataSerializer.Instance.SerializePayload(
            MessageType.TestRunAttachmentsProcessingProgress, Payload, version: 7);

        AssertJsonEqual(V7Json, json);
    }

    // ── Deserialize ──────────────────────────────────────────────────────

    [TestMethod]
    public void DeserializePayloadV1()
    {
        var message = JsonDataSerializer.Instance.DeserializeMessage(Minify(V1Json));
        var result = JsonDataSerializer.Instance.DeserializePayload<TestRunAttachmentsProcessingProgressPayload>(message);

        AssertPayloadFields(result);
    }

    [TestMethod]
    public void DeserializePayloadV7()
    {
        var message = JsonDataSerializer.Instance.DeserializeMessage(Minify(V7Json));
        var result = JsonDataSerializer.Instance.DeserializePayload<TestRunAttachmentsProcessingProgressPayload>(message);

        AssertPayloadFields(result);
    }

    // ── Round-trip ───────────────────────────────────────────────────────

    [TestMethod]
    [DataRow(1)]
    [DataRow(7)]
    public void RoundTrip(int version)
    {
        var json = JsonDataSerializer.Instance.SerializePayload(
            MessageType.TestRunAttachmentsProcessingProgress, Payload, version);
        var message = JsonDataSerializer.Instance.DeserializeMessage(json);
        var result = JsonDataSerializer.Instance.DeserializePayload<TestRunAttachmentsProcessingProgressPayload>(message);

        Assert.IsNotNull(result);
        Assert.IsNotNull(result.AttachmentsProcessingProgressEventArgs);
        Assert.AreEqual(1, result.AttachmentsProcessingProgressEventArgs.CurrentAttachmentProcessorIndex);
        Assert.AreEqual(50, result.AttachmentsProcessingProgressEventArgs.CurrentAttachmentProcessorProgress);
        Assert.AreEqual(3, result.AttachmentsProcessingProgressEventArgs.AttachmentProcessorsCount);
    }

    // ── Helpers ──────────────────────────────────────────────────────────

    private static void AssertPayloadFields(TestRunAttachmentsProcessingProgressPayload? result)
    {
        Assert.IsNotNull(result);
        Assert.IsNotNull(result.AttachmentsProcessingProgressEventArgs);
        var args = result.AttachmentsProcessingProgressEventArgs;
        Assert.AreEqual(1, args.CurrentAttachmentProcessorIndex);
        var uris = args.CurrentAttachmentProcessorUris.ToList();
        Assert.HasCount(1, uris);
        Assert.AreEqual(new Uri("datacollector://coverage"), uris[0]);
        Assert.AreEqual(50, args.CurrentAttachmentProcessorProgress);
        Assert.AreEqual(3, args.AttachmentProcessorsCount);
    }

}
