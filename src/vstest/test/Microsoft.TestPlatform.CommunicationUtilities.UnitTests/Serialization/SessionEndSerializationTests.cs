// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Microsoft.TestPlatform.CommunicationUtilities.UnitTests.Serialization.SerializationTestHelpers;

namespace Microsoft.TestPlatform.CommunicationUtilities.UnitTests.Serialization;

/// <summary>
/// Wire-format tests for <see cref="MessageType.SessionEnd"/> ("TestSession.Terminate").
///
/// This message is sent to signal the graceful termination of a test session. It carries no payload.
/// </summary>
[TestClass]
[TestCategory("Serialization")]
public class SessionEndSerializationTests
{
    // ── Serialize ────────────────────────────────────────────────────────

    [TestMethod]
    public void SerializeMessage()
    {
        var json = JsonDataSerializer.Instance.SerializeMessage(MessageType.SessionEnd);

        AssertJsonEqual("""{"MessageType":"TestSession.Terminate"}""", json);
    }

    // ── Deserialize ──────────────────────────────────────────────────────

    [TestMethod]
    public void DeserializeMessage()
    {
        var message = JsonDataSerializer.Instance.DeserializeMessage("""{"MessageType":"TestSession.Terminate"}""");

        Assert.AreEqual("TestSession.Terminate", message.MessageType);
    }

    // ── Round-trip ───────────────────────────────────────────────────────

    [TestMethod]
    public void RoundTrip()
    {
        var json = JsonDataSerializer.Instance.SerializeMessage(MessageType.SessionEnd);
        var message = JsonDataSerializer.Instance.DeserializeMessage(json);

        Assert.AreEqual("TestSession.Terminate", message.MessageType);
    }

    // ── Helpers ──────────────────────────────────────────────────────────

}
