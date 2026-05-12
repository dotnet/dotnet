// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Microsoft.TestPlatform.CommunicationUtilities.UnitTests.Serialization.SerializationTestHelpers;

namespace Microsoft.TestPlatform.CommunicationUtilities.UnitTests.Serialization;

/// <summary>
/// Wire-format tests for <see cref="MessageType.CancelTestRun"/> ("TestExecution.Cancel").
///
/// This message is sent to cancel an in-progress test run gracefully. It carries no payload.
/// </summary>
[TestClass]
[TestCategory("Serialization")]
public class CancelTestRunSerializationTests
{
    // ── Serialize ────────────────────────────────────────────────────────

    [TestMethod]
    public void SerializeMessage()
    {
        var json = JsonDataSerializer.Instance.SerializeMessage(MessageType.CancelTestRun);

        AssertJsonEqual("""{"MessageType":"TestExecution.Cancel"}""", json);
    }

    // ── Deserialize ──────────────────────────────────────────────────────

    [TestMethod]
    public void DeserializeMessage()
    {
        var message = JsonDataSerializer.Instance.DeserializeMessage("""{"MessageType":"TestExecution.Cancel"}""");

        Assert.AreEqual("TestExecution.Cancel", message.MessageType);
    }

    // ── Round-trip ───────────────────────────────────────────────────────

    [TestMethod]
    public void RoundTrip()
    {
        var json = JsonDataSerializer.Instance.SerializeMessage(MessageType.CancelTestRun);
        var message = JsonDataSerializer.Instance.DeserializeMessage(json);

        Assert.AreEqual("TestExecution.Cancel", message.MessageType);
    }

    // ── Helpers ──────────────────────────────────────────────────────────

}
