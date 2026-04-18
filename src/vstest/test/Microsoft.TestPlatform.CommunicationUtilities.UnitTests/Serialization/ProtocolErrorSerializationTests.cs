// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Microsoft.TestPlatform.CommunicationUtilities.UnitTests.Serialization.SerializationTestHelpers;

namespace Microsoft.TestPlatform.CommunicationUtilities.UnitTests.Serialization;

/// <summary>
/// Wire-format tests for <see cref="MessageType.ProtocolError"/> ("ProtocolError").
///
/// This message is sent when a protocol-level error occurs during communication. It carries no payload.
/// </summary>
[TestClass]
[TestCategory("Serialization")]
public class ProtocolErrorSerializationTests
{
    // ── Serialize ────────────────────────────────────────────────────────

    [TestMethod]
    public void SerializeMessage()
    {
        var json = JsonDataSerializer.Instance.SerializeMessage(MessageType.ProtocolError);

        AssertJsonEqual("""{"MessageType":"ProtocolError"}""", json);
    }

    // ── Deserialize ──────────────────────────────────────────────────────

    [TestMethod]
    public void DeserializeMessage()
    {
        var message = JsonDataSerializer.Instance.DeserializeMessage("""{"MessageType":"ProtocolError"}""");

        Assert.AreEqual("ProtocolError", message.MessageType);
    }

    // ── Round-trip ───────────────────────────────────────────────────────

    [TestMethod]
    public void RoundTrip()
    {
        var json = JsonDataSerializer.Instance.SerializeMessage(MessageType.ProtocolError);
        var message = JsonDataSerializer.Instance.DeserializeMessage(json);

        Assert.AreEqual("ProtocolError", message.MessageType);
    }

    // ── Helpers ──────────────────────────────────────────────────────────

}
