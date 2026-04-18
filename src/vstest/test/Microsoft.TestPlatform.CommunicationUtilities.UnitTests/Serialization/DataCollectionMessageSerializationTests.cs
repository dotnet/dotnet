// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Microsoft.TestPlatform.CommunicationUtilities.UnitTests.Serialization.SerializationTestHelpers;

namespace Microsoft.TestPlatform.CommunicationUtilities.UnitTests.Serialization;

/// <summary>
/// Wire-format tests for <see cref="MessageType.DataCollectionMessage"/>
/// ("DataCollection.SendMessage").
///
/// This message is sent by a data collector to report log messages (informational, warning,
/// error) during a test run. The payload is <see cref="DataCollectionMessageEventArgs"/>
/// extending <see cref="TestRunMessageEventArgs"/> with data-collector-specific metadata.
///
/// Payload is identical for V1 and V7 serializers because it contains no TestCase/TestResult objects.
/// The only difference is the outer envelope: V1 omits the Version field, V7 includes it.
/// </summary>
[TestClass]
[TestCategory("Serialization")]
public class DataCollectionMessageSerializationTests
{
    // ── Example payload ──────────────────────────────────────────────────
    private static readonly DataCollectionMessageEventArgs Payload = new(
        TestMessageLevel.Warning, "Attachment is too large")
    {
        FriendlyName = "coverage",
        Uri = new Uri("datacollector://coverage"),
    };

    // ── V1 wire format ───────────────────────────────────────────────────
    // Protocol versions 0, 1, 3 — no Version field in the envelope.
    private static readonly string V1Json = """
        {
          "MessageType": "DataCollection.SendMessage",
          "Payload": {
            "FriendlyName": "coverage",
            "Uri": "datacollector://coverage",
            "TestCaseId": "00000000-0000-0000-0000-000000000000",
            "Message": "Attachment is too large",
            "Level": 1
          }
        }
        """;

    // ── V7 wire format ───────────────────────────────────────────────────
    // Protocol versions 2, 4, 5, 6, 7 — includes Version in the envelope.
    private static readonly string V7Json = """
        {
          "Version": 7,
          "MessageType": "DataCollection.SendMessage",
          "Payload": {
            "FriendlyName": "coverage",
            "Uri": "datacollector://coverage",
            "TestCaseId": "00000000-0000-0000-0000-000000000000",
            "Message": "Attachment is too large",
            "Level": 1
          }
        }
        """;

    // ── Serialize ────────────────────────────────────────────────────────

    [TestMethod]
    public void SerializePayloadV1()
    {
        var json = JsonDataSerializer.Instance.SerializePayload(
            MessageType.DataCollectionMessage, Payload, version: 1);

        AssertJsonEqual(V1Json, json);
    }

    [TestMethod]
    public void SerializePayloadV7()
    {
        var json = JsonDataSerializer.Instance.SerializePayload(
            MessageType.DataCollectionMessage, Payload, version: 7);

        AssertJsonEqual(V7Json, json);
    }

    // ── Deserialize ──────────────────────────────────────────────────────

    [TestMethod]
    public void DeserializePayloadV1()
    {
        var message = JsonDataSerializer.Instance.DeserializeMessage(Minify(V1Json));
        var result = JsonDataSerializer.Instance.DeserializePayload<DataCollectionMessageEventArgs>(message);

        AssertPayloadFields(result);
    }

    [TestMethod]
    public void DeserializePayloadV7()
    {
        var message = JsonDataSerializer.Instance.DeserializeMessage(Minify(V7Json));
        var result = JsonDataSerializer.Instance.DeserializePayload<DataCollectionMessageEventArgs>(message);

        AssertPayloadFields(result);
    }

    // ── Round-trip ───────────────────────────────────────────────────────

    [TestMethod]
    [DataRow(1)]
    [DataRow(7)]
    public void RoundTrip(int version)
    {
        var json = JsonDataSerializer.Instance.SerializePayload(
            MessageType.DataCollectionMessage, Payload, version);
        var message = JsonDataSerializer.Instance.DeserializeMessage(json);
        var result = JsonDataSerializer.Instance.DeserializePayload<DataCollectionMessageEventArgs>(message);

        Assert.IsNotNull(result);
        Assert.AreEqual(Payload.Level, result.Level);
        Assert.AreEqual(Payload.Message, result.Message);
        Assert.AreEqual(Payload.FriendlyName, result.FriendlyName);
    }

    // ── Helpers ──────────────────────────────────────────────────────────

    private static void AssertPayloadFields(DataCollectionMessageEventArgs? result)
    {
        Assert.IsNotNull(result);
        Assert.AreEqual("coverage", result.FriendlyName);
        Assert.AreEqual(new Uri("datacollector://coverage"), result.Uri);
        Assert.AreEqual(Guid.Empty, result.TestCaseId);
        Assert.AreEqual("Attachment is too large", result.Message);
        Assert.AreEqual(TestMessageLevel.Warning, result.Level);
    }

}
