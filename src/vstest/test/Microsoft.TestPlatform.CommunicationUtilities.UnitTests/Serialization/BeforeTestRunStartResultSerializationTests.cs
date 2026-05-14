// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.Common.DataCollection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Microsoft.TestPlatform.CommunicationUtilities.UnitTests.Serialization.SerializationTestHelpers;

namespace Microsoft.TestPlatform.CommunicationUtilities.UnitTests.Serialization;

/// <summary>
/// Wire-format tests for <see cref="MessageType.BeforeTestRunStartResult"/>
/// ("DataCollection.BeforeTestRunStartResult").
///
/// This message is sent by the data collector host after processing the before-test-run-start
/// event. The payload is <see cref="BeforeTestRunStartResult"/> containing environment variables
/// to inject and the data collection events port.
///
/// Payload is identical for V1 and V7 serializers because it contains no TestCase/TestResult objects.
/// The only difference is the outer envelope: V1 omits the Version field, V7 includes it.
/// </summary>
[TestClass]
[TestCategory("Serialization")]
public class BeforeTestRunStartResultSerializationTests
{
    // ── Example payload ──────────────────────────────────────────────────
    private static readonly BeforeTestRunStartResult Payload = new(
        new Dictionary<string, string?> { ["COVERAGE_ENABLED"] = "true" }, 9042);

    // ── V1 wire format ───────────────────────────────────────────────────
    // Protocol versions 0, 1, 3 — no Version field in the envelope.
    private static readonly string V1Json = """
        {
          "MessageType": "DataCollection.BeforeTestRunStartResult",
          "Payload": {
            "EnvironmentVariables": {
              "COVERAGE_ENABLED": "true"
            },
            "DataCollectionEventsPort": 9042
          }
        }
        """;

    // ── V7 wire format ───────────────────────────────────────────────────
    // Protocol versions 2, 4, 5, 6, 7 — includes Version in the envelope.
    private static readonly string V7Json = """
        {
          "Version": 7,
          "MessageType": "DataCollection.BeforeTestRunStartResult",
          "Payload": {
            "EnvironmentVariables": {
              "COVERAGE_ENABLED": "true"
            },
            "DataCollectionEventsPort": 9042
          }
        }
        """;

    // ── Serialize ────────────────────────────────────────────────────────

    [TestMethod]
    public void SerializePayloadV1()
    {
        var json = JsonDataSerializer.Instance.SerializePayload(
            MessageType.BeforeTestRunStartResult, Payload, version: 1);

        AssertJsonEqual(V1Json, json);
    }

    [TestMethod]
    public void SerializePayloadV7()
    {
        var json = JsonDataSerializer.Instance.SerializePayload(
            MessageType.BeforeTestRunStartResult, Payload, version: 7);

        AssertJsonEqual(V7Json, json);
    }

    // ── Deserialize ──────────────────────────────────────────────────────

    [TestMethod]
    public void DeserializePayloadV1()
    {
        var message = JsonDataSerializer.Instance.DeserializeMessage(Minify(V1Json));
        var result = JsonDataSerializer.Instance.DeserializePayload<BeforeTestRunStartResult>(message);

        AssertPayloadFields(result);
    }

    [TestMethod]
    public void DeserializePayloadV7()
    {
        var message = JsonDataSerializer.Instance.DeserializeMessage(Minify(V7Json));
        var result = JsonDataSerializer.Instance.DeserializePayload<BeforeTestRunStartResult>(message);

        AssertPayloadFields(result);
    }

    // ── Round-trip ───────────────────────────────────────────────────────

    [TestMethod]
    [DataRow(1)]
    [DataRow(7)]
    public void RoundTrip(int version)
    {
        var json = JsonDataSerializer.Instance.SerializePayload(
            MessageType.BeforeTestRunStartResult, Payload, version);
        var message = JsonDataSerializer.Instance.DeserializeMessage(json);
        var result = JsonDataSerializer.Instance.DeserializePayload<BeforeTestRunStartResult>(message);

        Assert.IsNotNull(result);
        Assert.AreEqual(9042, result.DataCollectionEventsPort);
        Assert.IsNotNull(result.EnvironmentVariables);
        Assert.AreEqual("true", result.EnvironmentVariables["COVERAGE_ENABLED"]);
    }

    // ── Helpers ──────────────────────────────────────────────────────────

    private static void AssertPayloadFields(BeforeTestRunStartResult? result)
    {
        Assert.IsNotNull(result);
        Assert.IsNotNull(result.EnvironmentVariables);
        Assert.AreEqual("true", result.EnvironmentVariables["COVERAGE_ENABLED"]);
        Assert.AreEqual(9042, result.DataCollectionEventsPort);
    }

}
