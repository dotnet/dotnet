// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Microsoft.TestPlatform.CommunicationUtilities.UnitTests.Serialization.SerializationTestHelpers;

namespace Microsoft.TestPlatform.CommunicationUtilities.UnitTests.Serialization;

/// <summary>
/// Wire-format tests for <see cref="MessageType.EditorAttachDebugger2"/>
/// ("TestExecution.EditorAttachDebugger2").
///
/// This is the v2 editor-attach-debugger request that includes the target framework and
/// source list in addition to the process ID. The payload is
/// <see cref="EditorAttachDebuggerPayload"/>.
///
/// Payload is identical for V1 and V7 serializers because it contains no TestCase/TestResult objects.
/// The only difference is the outer envelope: V1 omits the Version field, V7 includes it.
/// </summary>
[TestClass]
[TestCategory("Serialization")]
public class EditorAttachDebugger2SerializationTests
{
    // ── Example payload ──────────────────────────────────────────────────
    private static readonly EditorAttachDebuggerPayload Payload = new()
    {
        ProcessID = 12345,
        TargetFramework = ".NETCoreApp,Version=v9.0",
        Sources = new List<string> { "Tests.dll" },
    };

    // ── V1 wire format ───────────────────────────────────────────────────
    // Protocol versions 0, 1, 3 — no Version field in the envelope.
    private static readonly string V1Json = """
        {
          "MessageType": "TestExecution.EditorAttachDebugger2",
          "Payload": {
            "ProcessID": 12345,
            "TargetFramework": ".NETCoreApp,Version=v9.0",
            "Sources": [
              "Tests.dll"
            ]
          }
        }
        """;

    // ── V7 wire format ───────────────────────────────────────────────────
    // Protocol versions 2, 4, 5, 6, 7 — includes Version in the envelope.
    private static readonly string V7Json = """
        {
          "Version": 7,
          "MessageType": "TestExecution.EditorAttachDebugger2",
          "Payload": {
            "ProcessID": 12345,
            "TargetFramework": ".NETCoreApp,Version=v9.0",
            "Sources": [
              "Tests.dll"
            ]
          }
        }
        """;

    // ── Serialize ────────────────────────────────────────────────────────

    [TestMethod]
    public void SerializePayloadV1()
    {
        var json = JsonDataSerializer.Instance.SerializePayload(
            MessageType.EditorAttachDebugger2, Payload, version: 1);

        AssertJsonEqual(V1Json, json);
    }

    [TestMethod]
    public void SerializePayloadV7()
    {
        var json = JsonDataSerializer.Instance.SerializePayload(
            MessageType.EditorAttachDebugger2, Payload, version: 7);

        AssertJsonEqual(V7Json, json);
    }

    // ── Deserialize ──────────────────────────────────────────────────────

    [TestMethod]
    public void DeserializePayloadV1()
    {
        var message = JsonDataSerializer.Instance.DeserializeMessage(Minify(V1Json));
        var result = JsonDataSerializer.Instance.DeserializePayload<EditorAttachDebuggerPayload>(message);

        AssertPayloadFields(result);
    }

    [TestMethod]
    public void DeserializePayloadV7()
    {
        var message = JsonDataSerializer.Instance.DeserializeMessage(Minify(V7Json));
        var result = JsonDataSerializer.Instance.DeserializePayload<EditorAttachDebuggerPayload>(message);

        AssertPayloadFields(result);
    }

    // ── Round-trip ───────────────────────────────────────────────────────

    [TestMethod]
    [DataRow(1)]
    [DataRow(7)]
    public void RoundTrip(int version)
    {
        var json = JsonDataSerializer.Instance.SerializePayload(
            MessageType.EditorAttachDebugger2, Payload, version);
        var message = JsonDataSerializer.Instance.DeserializeMessage(json);
        var result = JsonDataSerializer.Instance.DeserializePayload<EditorAttachDebuggerPayload>(message);

        Assert.IsNotNull(result);
        Assert.AreEqual(Payload.ProcessID, result.ProcessID);
        Assert.AreEqual(Payload.TargetFramework, result.TargetFramework);
    }

    // ── Helpers ──────────────────────────────────────────────────────────

    private static void AssertPayloadFields(EditorAttachDebuggerPayload? result)
    {
        Assert.IsNotNull(result);
        Assert.AreEqual(12345, result.ProcessID);
        Assert.AreEqual(".NETCoreApp,Version=v9.0", result.TargetFramework);
        Assert.IsNotNull(result.Sources);
        Assert.Contains("Tests.dll", result.Sources);
    }

}
