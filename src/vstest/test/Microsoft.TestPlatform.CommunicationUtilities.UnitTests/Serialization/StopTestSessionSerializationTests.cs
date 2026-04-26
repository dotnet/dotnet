// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client.Payloads;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Microsoft.TestPlatform.CommunicationUtilities.UnitTests.Serialization.SerializationTestHelpers;

namespace Microsoft.TestPlatform.CommunicationUtilities.UnitTests.Serialization;

/// <summary>
/// Wire-format tests for <see cref="MessageType.StopTestSession"/> ("TestSession.StopTestSession").
///
/// This message is sent to request an existing test session to be stopped.
/// The payload is <see cref="StopTestSessionPayload"/> which contains the session info
/// and whether to collect metrics.
///
/// Payload is identical for V1 and V7 because no TestCase/TestResult objects are involved.
/// The only difference is the outer envelope: V1 omits the Version field, V7 includes it.
/// </summary>
[TestClass]
[TestCategory("Serialization")]
public class StopTestSessionSerializationTests
{
    // ── Example payload ──────────────────────────────────────────────────
    private static readonly StopTestSessionPayload Payload = BuildPayload();

    private static StopTestSessionPayload BuildPayload()
    {
        var sessionInfo = new TestSessionInfo();
        typeof(TestSessionInfo).GetProperty("Id")!.SetValue(sessionInfo, new Guid("abcd1234-5678-90ab-cdef-1234567890ab"));

        return new StopTestSessionPayload
        {
            TestSessionInfo = sessionInfo,
            CollectMetrics = true
        };
    }

    // ── V1 wire format ───────────────────────────────────────────────────
    // Protocol versions 0, 1, 3 — no Version field in the envelope.
    private static readonly string V1Json = """
        {
          "MessageType": "TestSession.StopTestSession",
          "Payload": {
            "TestSessionInfo": {
              "Id": "abcd1234-5678-90ab-cdef-1234567890ab"
            },
            "CollectMetrics": true
          }
        }
        """;

    // ── V7 wire format ───────────────────────────────────────────────────
    // Protocol versions 2, 4, 5, 6, 7 — includes Version in the envelope.
    private static readonly string V7Json = """
        {
          "Version": 7,
          "MessageType": "TestSession.StopTestSession",
          "Payload": {
            "TestSessionInfo": {
              "Id": "abcd1234-5678-90ab-cdef-1234567890ab"
            },
            "CollectMetrics": true
          }
        }
        """;

    // ── Serialize ────────────────────────────────────────────────────────

    [TestMethod]
    public void SerializePayloadV1()
    {
        var json = JsonDataSerializer.Instance.SerializePayload(
            MessageType.StopTestSession, Payload, version: 1);

        AssertJsonEqual(V1Json, json);
    }

    [TestMethod]
    public void SerializePayloadV7()
    {
        var json = JsonDataSerializer.Instance.SerializePayload(
            MessageType.StopTestSession, Payload, version: 7);

        AssertJsonEqual(V7Json, json);
    }

    // ── Deserialize ──────────────────────────────────────────────────────

    [TestMethod]
    public void DeserializePayloadV1()
    {
        var message = JsonDataSerializer.Instance.DeserializeMessage(Minify(V1Json));
        var result = JsonDataSerializer.Instance.DeserializePayload<StopTestSessionPayload>(message);

        Assert.IsNotNull(result);
        Assert.IsNotNull(result.TestSessionInfo);
        Assert.AreEqual(new Guid("abcd1234-5678-90ab-cdef-1234567890ab"), result.TestSessionInfo.Id);
        Assert.IsTrue(result.CollectMetrics);
    }

    [TestMethod]
    public void DeserializePayloadV7()
    {
        var message = JsonDataSerializer.Instance.DeserializeMessage(Minify(V7Json));
        var result = JsonDataSerializer.Instance.DeserializePayload<StopTestSessionPayload>(message);

        Assert.IsNotNull(result);
        Assert.IsNotNull(result.TestSessionInfo);
        Assert.AreEqual(new Guid("abcd1234-5678-90ab-cdef-1234567890ab"), result.TestSessionInfo.Id);
        Assert.IsTrue(result.CollectMetrics);
    }

    // ── Round-trip ───────────────────────────────────────────────────────

    [TestMethod]
    [DataRow(1)]
    [DataRow(7)]
    public void RoundTrip(int version)
    {
        var json = JsonDataSerializer.Instance.SerializePayload(
            MessageType.StopTestSession, Payload, version);
        var message = JsonDataSerializer.Instance.DeserializeMessage(json);
        var result = JsonDataSerializer.Instance.DeserializePayload<StopTestSessionPayload>(message);

        Assert.IsNotNull(result);
        Assert.IsNotNull(result.TestSessionInfo);
        Assert.AreEqual(Payload.TestSessionInfo!.Id, result.TestSessionInfo.Id);
        Assert.AreEqual(Payload.CollectMetrics, result.CollectMetrics);
    }

    // ── Helpers ──────────────────────────────────────────────────────────

}
