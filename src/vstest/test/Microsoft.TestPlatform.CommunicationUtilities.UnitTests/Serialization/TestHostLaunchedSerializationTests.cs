// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Microsoft.TestPlatform.CommunicationUtilities.UnitTests.Serialization.SerializationTestHelpers;

namespace Microsoft.TestPlatform.CommunicationUtilities.UnitTests.Serialization;

/// <summary>
/// Wire-format tests for <see cref="MessageType.TestHostLaunched"/> ("DataCollection.TestHostLaunched").
///
/// Notification that a test host process was launched. Payload is identical for V1/V7.
/// The only difference is the outer envelope: V1 omits the Version field, V7 includes it.
/// </summary>
[TestClass]
[TestCategory("Serialization")]
public class TestHostLaunchedSerializationTests
{
    // ── Example payload ──────────────────────────────────────────────────
    private static readonly TestHostLaunchedPayload Payload = new()
    {
        ProcessId = 56789
    };

    // ── V1 wire format ───────────────────────────────────────────────────
    // Protocol versions 0, 1, 3 — no Version field in the envelope.
    private static readonly string V1Json = """
        {
          "MessageType": "DataCollection.TestHostLaunched",
          "Payload": {
            "ProcessId": 56789
          }
        }
        """;

    // ── V7 wire format ───────────────────────────────────────────────────
    // Protocol versions 2, 4, 5, 6, 7 — includes Version in the envelope.
    private static readonly string V7Json = """
        {
          "Version": 7,
          "MessageType": "DataCollection.TestHostLaunched",
          "Payload": {
            "ProcessId": 56789
          }
        }
        """;

    // ── Serialize ────────────────────────────────────────────────────────

    [TestMethod]
    public void SerializePayloadV1()
    {
        var json = JsonDataSerializer.Instance.SerializePayload(
            MessageType.TestHostLaunched, Payload, version: 1);

        AssertJsonEqual(V1Json, json);
    }

    [TestMethod]
    public void SerializePayloadV7()
    {
        var json = JsonDataSerializer.Instance.SerializePayload(
            MessageType.TestHostLaunched, Payload, version: 7);

        AssertJsonEqual(V7Json, json);
    }

    // ── Deserialize ──────────────────────────────────────────────────────

    [TestMethod]
    public void DeserializePayloadV1()
    {
        var message = JsonDataSerializer.Instance.DeserializeMessage(Minify(V1Json));
        var result = JsonDataSerializer.Instance.DeserializePayload<TestHostLaunchedPayload>(message);

        Assert.IsNotNull(result);
        Assert.AreEqual(56789, result.ProcessId);
    }

    [TestMethod]
    public void DeserializePayloadV7()
    {
        var message = JsonDataSerializer.Instance.DeserializeMessage(Minify(V7Json));
        var result = JsonDataSerializer.Instance.DeserializePayload<TestHostLaunchedPayload>(message);

        Assert.IsNotNull(result);
        Assert.AreEqual(56789, result.ProcessId);
    }

    // ── Round-trip ───────────────────────────────────────────────────────

    [TestMethod]
    [DataRow(1)]
    [DataRow(7)]
    public void RoundTrip(int version)
    {
        var json = JsonDataSerializer.Instance.SerializePayload(
            MessageType.TestHostLaunched, Payload, version);
        var message = JsonDataSerializer.Instance.DeserializeMessage(json);
        var result = JsonDataSerializer.Instance.DeserializePayload<TestHostLaunchedPayload>(message);

        Assert.IsNotNull(result);
        Assert.AreEqual(Payload.ProcessId, result.ProcessId);
    }

    // ── Helpers ──────────────────────────────────────────────────────────

}
