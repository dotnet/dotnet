// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Microsoft.TestPlatform.CommunicationUtilities.UnitTests.Serialization.SerializationTestHelpers;

namespace Microsoft.TestPlatform.CommunicationUtilities.UnitTests.Serialization;

/// <summary>
/// Wire-format tests for <see cref="MessageType.TestRunAttachmentsProcessingCancel"/>
/// ("TestRunAttachmentsProcessing.Cancel").
///
/// This message is sent to cancel in-progress attachment processing. It carries no payload.
/// </summary>
[TestClass]
[TestCategory("Serialization")]
public class TestRunAttachmentsProcessingCancelSerializationTests
{
    // ── Serialize ────────────────────────────────────────────────────────

    [TestMethod]
    public void SerializeMessage()
    {
        var json = JsonDataSerializer.Instance.SerializeMessage(MessageType.TestRunAttachmentsProcessingCancel);

        AssertJsonEqual("""{"MessageType":"TestRunAttachmentsProcessing.Cancel"}""", json);
    }

    // ── Deserialize ──────────────────────────────────────────────────────

    [TestMethod]
    public void DeserializeMessage()
    {
        var message = JsonDataSerializer.Instance.DeserializeMessage("""{"MessageType":"TestRunAttachmentsProcessing.Cancel"}""");

        Assert.AreEqual("TestRunAttachmentsProcessing.Cancel", message.MessageType);
    }

    // ── Round-trip ───────────────────────────────────────────────────────

    [TestMethod]
    public void RoundTrip()
    {
        var json = JsonDataSerializer.Instance.SerializeMessage(MessageType.TestRunAttachmentsProcessingCancel);
        var message = JsonDataSerializer.Instance.DeserializeMessage(json);

        Assert.AreEqual("TestRunAttachmentsProcessing.Cancel", message.MessageType);
    }

    // ── Helpers ──────────────────────────────────────────────────────────

}
