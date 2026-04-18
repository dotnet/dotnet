// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Microsoft.TestPlatform.CommunicationUtilities.UnitTests.Serialization.SerializationTestHelpers;

namespace Microsoft.TestPlatform.CommunicationUtilities.UnitTests.Serialization;

/// <summary>
/// Wire-format tests for <see cref="MessageType.DataCollectionTestStartAck"/>
/// ("DataCollection.TestStartAck").
///
/// This message is sent by the data collector host to acknowledge that a test-start
/// notification has been processed. It carries no payload.
/// </summary>
[TestClass]
[TestCategory("Serialization")]
public class DataCollectionTestStartAckSerializationTests
{
    // ── Serialize ────────────────────────────────────────────────────────

    [TestMethod]
    public void SerializeMessage()
    {
        var json = JsonDataSerializer.Instance.SerializeMessage(MessageType.DataCollectionTestStartAck);

        AssertJsonEqual("""{"MessageType":"DataCollection.TestStartAck"}""", json);
    }

    // ── Deserialize ──────────────────────────────────────────────────────

    [TestMethod]
    public void DeserializeMessage()
    {
        var message = JsonDataSerializer.Instance.DeserializeMessage("""{"MessageType":"DataCollection.TestStartAck"}""");

        Assert.AreEqual("DataCollection.TestStartAck", message.MessageType);
    }

    // ── Round-trip ───────────────────────────────────────────────────────

    [TestMethod]
    public void RoundTrip()
    {
        var json = JsonDataSerializer.Instance.SerializeMessage(MessageType.DataCollectionTestStartAck);
        var message = JsonDataSerializer.Instance.DeserializeMessage(json);

        Assert.AreEqual("DataCollection.TestStartAck", message.MessageType);
    }

    // ── Helpers ──────────────────────────────────────────────────────────

}
