// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Microsoft.TestPlatform.CommunicationUtilities.UnitTests.Serialization.SerializationTestHelpers;

namespace Microsoft.TestPlatform.CommunicationUtilities.UnitTests.Serialization;

/// <summary>
/// Wire-format tests for <see cref="MessageType.SessionConnected"/> ("TestSession.Connected").
///
/// This message is sent when a test host connects to the runner. It carries no payload.
/// </summary>
[TestClass]
[TestCategory("Serialization")]
public class SessionConnectedSerializationTests
{
    // ── Serialize ────────────────────────────────────────────────────────

    [TestMethod]
    public void SerializeMessage()
    {
        var json = JsonDataSerializer.Instance.SerializeMessage(MessageType.SessionConnected);

        AssertJsonEqual("""{"MessageType":"TestSession.Connected"}""", json);
    }

    // ── Deserialize ──────────────────────────────────────────────────────

    [TestMethod]
    public void DeserializeMessage()
    {
        var message = JsonDataSerializer.Instance.DeserializeMessage("""{"MessageType":"TestSession.Connected"}""");

        Assert.AreEqual("TestSession.Connected", message.MessageType);
    }

    // ── Round-trip ───────────────────────────────────────────────────────

    [TestMethod]
    public void RoundTrip()
    {
        var json = JsonDataSerializer.Instance.SerializeMessage(MessageType.SessionConnected);
        var message = JsonDataSerializer.Instance.DeserializeMessage(json);

        Assert.AreEqual("TestSession.Connected", message.MessageType);
    }

    // ── Helpers ──────────────────────────────────────────────────────────

}
