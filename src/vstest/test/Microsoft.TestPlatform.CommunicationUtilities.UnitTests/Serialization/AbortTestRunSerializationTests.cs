// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Microsoft.TestPlatform.CommunicationUtilities.UnitTests.Serialization.SerializationTestHelpers;

namespace Microsoft.TestPlatform.CommunicationUtilities.UnitTests.Serialization;

/// <summary>
/// Wire-format tests for <see cref="MessageType.AbortTestRun"/> ("TestExecution.Abort").
///
/// This message is sent to abort a test run immediately. It carries no payload.
/// </summary>
[TestClass]
[TestCategory("Serialization")]
public class AbortTestRunSerializationTests
{
    // ── Serialize ────────────────────────────────────────────────────────

    [TestMethod]
    public void SerializeMessage()
    {
        var json = JsonDataSerializer.Instance.SerializeMessage(MessageType.AbortTestRun);

        AssertJsonEqual("""{"MessageType":"TestExecution.Abort"}""", json);
    }

    // ── Deserialize ──────────────────────────────────────────────────────

    [TestMethod]
    public void DeserializeMessage()
    {
        var message = JsonDataSerializer.Instance.DeserializeMessage("""{"MessageType":"TestExecution.Abort"}""");

        Assert.AreEqual("TestExecution.Abort", message.MessageType);
    }

    // ── Round-trip ───────────────────────────────────────────────────────

    [TestMethod]
    public void RoundTrip()
    {
        var json = JsonDataSerializer.Instance.SerializeMessage(MessageType.AbortTestRun);
        var message = JsonDataSerializer.Instance.DeserializeMessage(json);

        Assert.AreEqual("TestExecution.Abort", message.MessageType);
    }

    // ── Helpers ──────────────────────────────────────────────────────────

}
