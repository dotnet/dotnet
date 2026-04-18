// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Microsoft.TestPlatform.CommunicationUtilities.UnitTests.Serialization.SerializationTestHelpers;

namespace Microsoft.TestPlatform.CommunicationUtilities.UnitTests.Serialization;

/// <summary>
/// Wire-format tests for <see cref="MessageType.EditorAttachDebuggerCallback"/>
/// ("TestExecution.EditorAttachDebuggerCallback").
///
/// This message is sent as a response to an editor-attach-debugger request. The payload is
/// <see cref="EditorAttachDebuggerAckPayload"/> containing whether the debugger was successfully
/// attached and an optional error message.
///
/// Payload is identical for V1 and V7 serializers because it contains no TestCase/TestResult objects.
/// The only difference is the outer envelope: V1 omits the Version field, V7 includes it.
/// </summary>
[TestClass]
[TestCategory("Serialization")]
public class EditorAttachDebuggerCallbackSerializationTests
{
    // ── Example payload ──────────────────────────────────────────────────
    private static readonly EditorAttachDebuggerAckPayload Payload = new()
    {
        Attached = true,
        ErrorMessage = null,
    };

    // ── V1 wire format ───────────────────────────────────────────────────
    // Protocol versions 0, 1, 3 — no Version field in the envelope.
    private static readonly string V1Json = """
        {
          "MessageType": "TestExecution.EditorAttachDebuggerCallback",
          "Payload": {
            "Attached": true,
            "ErrorMessage": null
          }
        }
        """;

    // ── V7 wire format ───────────────────────────────────────────────────
    // Protocol versions 2, 4, 5, 6, 7 — includes Version in the envelope.
    private static readonly string V7Json = """
        {
          "Version": 7,
          "MessageType": "TestExecution.EditorAttachDebuggerCallback",
          "Payload": {
            "Attached": true,
            "ErrorMessage": null
          }
        }
        """;

    // ── Serialize ────────────────────────────────────────────────────────

    [TestMethod]
    public void SerializePayloadV1()
    {
        var json = JsonDataSerializer.Instance.SerializePayload(
            MessageType.EditorAttachDebuggerCallback, Payload, version: 1);

        AssertJsonEqual(V1Json, json);
    }

    [TestMethod]
    public void SerializePayloadV7()
    {
        var json = JsonDataSerializer.Instance.SerializePayload(
            MessageType.EditorAttachDebuggerCallback, Payload, version: 7);

        AssertJsonEqual(V7Json, json);
    }

    // ── Deserialize ──────────────────────────────────────────────────────

    [TestMethod]
    public void DeserializePayloadV1()
    {
        var message = JsonDataSerializer.Instance.DeserializeMessage(Minify(V1Json));
        var result = JsonDataSerializer.Instance.DeserializePayload<EditorAttachDebuggerAckPayload>(message);

        Assert.IsNotNull(result);
        Assert.IsTrue(result.Attached);
        Assert.IsNull(result.ErrorMessage);
    }

    [TestMethod]
    public void DeserializePayloadV7()
    {
        var message = JsonDataSerializer.Instance.DeserializeMessage(Minify(V7Json));
        var result = JsonDataSerializer.Instance.DeserializePayload<EditorAttachDebuggerAckPayload>(message);

        Assert.IsNotNull(result);
        Assert.IsTrue(result.Attached);
        Assert.IsNull(result.ErrorMessage);
    }

    // ── Round-trip ───────────────────────────────────────────────────────

    [TestMethod]
    [DataRow(1)]
    [DataRow(7)]
    public void RoundTrip(int version)
    {
        var json = JsonDataSerializer.Instance.SerializePayload(
            MessageType.EditorAttachDebuggerCallback, Payload, version);
        var message = JsonDataSerializer.Instance.DeserializeMessage(json);
        var result = JsonDataSerializer.Instance.DeserializePayload<EditorAttachDebuggerAckPayload>(message);

        Assert.IsNotNull(result);
        Assert.AreEqual(Payload.Attached, result.Attached);
        Assert.AreEqual(Payload.ErrorMessage, result.ErrorMessage);
    }

    // ── Helpers ──────────────────────────────────────────────────────────

}
