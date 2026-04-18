// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Microsoft.TestPlatform.CommunicationUtilities.UnitTests.Serialization.SerializationTestHelpers;

namespace Microsoft.TestPlatform.CommunicationUtilities.UnitTests.Serialization;

/// <summary>
/// Wire-format tests for <see cref="MessageType.TelemetryEventMessage"/>
/// ("TestPlatform.TelemetryEvent").
///
/// This message is sent to report telemetry data from the test platform. The payload is
/// <see cref="TelemetryEvent"/> containing a named event and a dictionary of properties.
///
/// Payload is identical for V1 and V7 serializers because it contains no TestCase/TestResult objects.
/// The only difference is the outer envelope: V1 omits the Version field, V7 includes it.
/// </summary>
[TestClass]
[TestCategory("Serialization")]
public class TelemetryEventMessageSerializationTests
{
    // ── Example payload ──────────────────────────────────────────────────
    private static readonly TelemetryEvent Payload = new(
        "vs/testplatform/run",
        new Dictionary<string, object> { ["duration"] = 1500 });

    // ── V1 wire format ───────────────────────────────────────────────────
    // Protocol versions 0, 1, 3 — no Version field in the envelope.
    private static readonly string V1Json = """
        {
          "MessageType": "TestPlatform.TelemetryEvent",
          "Payload": {
            "Name": "vs/testplatform/run",
            "Properties": {
              "duration": 1500
            }
          }
        }
        """;

    // ── V7 wire format ───────────────────────────────────────────────────
    // Protocol versions 2, 4, 5, 6, 7 — includes Version in the envelope.
    private static readonly string V7Json = """
        {
          "Version": 7,
          "MessageType": "TestPlatform.TelemetryEvent",
          "Payload": {
            "Name": "vs/testplatform/run",
            "Properties": {
              "duration": 1500
            }
          }
        }
        """;

    // ── Serialize ────────────────────────────────────────────────────────

    [TestMethod]
    public void SerializePayloadV1()
    {
        var json = JsonDataSerializer.Instance.SerializePayload(
            MessageType.TelemetryEventMessage, Payload, version: 1);

        AssertJsonEqual(V1Json, json);
    }

    [TestMethod]
    public void SerializePayloadV7()
    {
        var json = JsonDataSerializer.Instance.SerializePayload(
            MessageType.TelemetryEventMessage, Payload, version: 7);

        AssertJsonEqual(V7Json, json);
    }

    // ── Deserialize ──────────────────────────────────────────────────────

    [TestMethod]
    public void DeserializePayloadV1()
    {
        var message = JsonDataSerializer.Instance.DeserializeMessage(Minify(V1Json));
        var result = JsonDataSerializer.Instance.DeserializePayload<TelemetryEvent>(message);

        AssertPayloadFields(result);
    }

    [TestMethod]
    public void DeserializePayloadV7()
    {
        var message = JsonDataSerializer.Instance.DeserializeMessage(Minify(V7Json));
        var result = JsonDataSerializer.Instance.DeserializePayload<TelemetryEvent>(message);

        AssertPayloadFields(result);
    }

    // ── Round-trip ───────────────────────────────────────────────────────

    [TestMethod]
    [DataRow(1)]
    [DataRow(7)]
    public void RoundTrip(int version)
    {
        var json = JsonDataSerializer.Instance.SerializePayload(
            MessageType.TelemetryEventMessage, Payload, version);
        var message = JsonDataSerializer.Instance.DeserializeMessage(json);
        var result = JsonDataSerializer.Instance.DeserializePayload<TelemetryEvent>(message);

        Assert.IsNotNull(result);
        Assert.AreEqual("vs/testplatform/run", result.Name);
        Assert.IsNotNull(result.Properties);
        Assert.IsTrue(result.Properties.ContainsKey("duration"));
    }

    // ── Helpers ──────────────────────────────────────────────────────────

    private static void AssertPayloadFields(TelemetryEvent? result)
    {
        Assert.IsNotNull(result);
        Assert.AreEqual("vs/testplatform/run", result.Name);
        Assert.IsNotNull(result.Properties);
        Assert.IsTrue(result.Properties.ContainsKey("duration"));
    }

}
