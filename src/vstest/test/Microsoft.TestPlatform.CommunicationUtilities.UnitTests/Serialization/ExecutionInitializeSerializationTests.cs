// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Microsoft.TestPlatform.CommunicationUtilities.UnitTests.Serialization.SerializationTestHelpers;

namespace Microsoft.TestPlatform.CommunicationUtilities.UnitTests.Serialization;

/// <summary>
/// Wire-format tests for <see cref="MessageType.ExecutionInitialize"/> ("TestExecution.Initialize").
///
/// This message is sent by the runner to the test host at the start of an execution session to
/// supply the list of extension assembly paths. The payload is an IEnumerable of string.
///
/// Payload is identical for V1 and V7 serializers because it is a simple array of strings.
/// The only difference is the outer envelope: V1 omits the Version field, V7 includes it.
/// </summary>
[TestClass]
[TestCategory("Serialization")]
public class ExecutionInitializeSerializationTests
{
    // ── Example payload ──────────────────────────────────────────────────
    private static readonly IEnumerable<string> Payload = new[] { "path/to/extension1.dll", "path/to/extension2.dll" };

    // ── V1 wire format ───────────────────────────────────────────────────
    // Protocol versions 0, 1, 3 — no Version field in the envelope.
    private static readonly string V1Json = """
        {
          "MessageType": "TestExecution.Initialize",
          "Payload": [
            "path/to/extension1.dll",
            "path/to/extension2.dll"
          ]
        }
        """;

    // ── V7 wire format ───────────────────────────────────────────────────
    // Protocol versions 2, 4, 5, 6, 7 — includes Version in the envelope.
    private static readonly string V7Json = """
        {
          "Version": 7,
          "MessageType": "TestExecution.Initialize",
          "Payload": [
            "path/to/extension1.dll",
            "path/to/extension2.dll"
          ]
        }
        """;

    // ── Serialize ────────────────────────────────────────────────────────

    [TestMethod]
    public void SerializePayloadV1()
    {
        var json = JsonDataSerializer.Instance.SerializePayload(
            MessageType.ExecutionInitialize, Payload, version: 1);

        AssertJsonEqual(V1Json, json);
    }

    [TestMethod]
    public void SerializePayloadV7()
    {
        var json = JsonDataSerializer.Instance.SerializePayload(
            MessageType.ExecutionInitialize, Payload, version: 7);

        AssertJsonEqual(V7Json, json);
    }

    // ── Deserialize ──────────────────────────────────────────────────────

    [TestMethod]
    public void DeserializePayloadV1()
    {
        var message = JsonDataSerializer.Instance.DeserializeMessage(Minify(V1Json));
        var result = JsonDataSerializer.Instance.DeserializePayload<IEnumerable<string>>(message);

        Assert.IsNotNull(result);
        var list = result.ToList();
        Assert.HasCount(2, list);
        Assert.AreEqual("path/to/extension1.dll", list[0]);
        Assert.AreEqual("path/to/extension2.dll", list[1]);
    }

    [TestMethod]
    public void DeserializePayloadV7()
    {
        var message = JsonDataSerializer.Instance.DeserializeMessage(Minify(V7Json));
        var result = JsonDataSerializer.Instance.DeserializePayload<IEnumerable<string>>(message);

        Assert.IsNotNull(result);
        var list = result.ToList();
        Assert.HasCount(2, list);
        Assert.AreEqual("path/to/extension1.dll", list[0]);
        Assert.AreEqual("path/to/extension2.dll", list[1]);
    }

    // ── Round-trip ───────────────────────────────────────────────────────

    [TestMethod]
    [DataRow(1)]
    [DataRow(7)]
    public void RoundTrip(int version)
    {
        var json = JsonDataSerializer.Instance.SerializePayload(
            MessageType.ExecutionInitialize, Payload, version);
        var message = JsonDataSerializer.Instance.DeserializeMessage(json);
        var result = JsonDataSerializer.Instance.DeserializePayload<IEnumerable<string>>(message);

        Assert.IsNotNull(result);
        CollectionAssert.AreEqual(Payload.ToList(), result.ToList());
    }

    // ── Helpers ──────────────────────────────────────────────────────────

}
