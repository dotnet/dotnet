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
/// Wire-format tests for <see cref="MessageType.BeforeTestRunStart"/> ("DataCollection.BeforeTestRunStart").
///
/// Sent to the data collector before a test run starts. Payload is identical for V1/V7.
/// The only difference is the outer envelope: V1 omits the Version field, V7 includes it.
/// </summary>
[TestClass]
[TestCategory("Serialization")]
public class BeforeTestRunStartSerializationTests
{
    // ── Example payload ──────────────────────────────────────────────────
    private static readonly BeforeTestRunStartPayload Payload = new()
    {
        SettingsXml = "<RunSettings><DataCollectionRunSettings><DataCollectors><DataCollector friendlyName=\"Code Coverage\"><Configuration><CodeCoverage /></Configuration></DataCollector></DataCollectors></DataCollectionRunSettings></RunSettings>",
        Sources = new List<string> { "Contoso.Math.Tests.dll", "Contoso.Core.Tests.dll" },
        IsTelemetryOptedIn = true
    };

    // ── V1 wire format ───────────────────────────────────────────────────
    // Protocol versions 0, 1, 3 — no Version field in the envelope.
    private static readonly string V1Json = """
        {
          "MessageType": "DataCollection.BeforeTestRunStart",
          "Payload": {
            "SettingsXml": "\u003CRunSettings\u003E\u003CDataCollectionRunSettings\u003E\u003CDataCollectors\u003E\u003CDataCollector friendlyName=\u0022Code Coverage\u0022\u003E\u003CConfiguration\u003E\u003CCodeCoverage /\u003E\u003C/Configuration\u003E\u003C/DataCollector\u003E\u003C/DataCollectors\u003E\u003C/DataCollectionRunSettings\u003E\u003C/RunSettings\u003E",
            "Sources": [
              "Contoso.Math.Tests.dll",
              "Contoso.Core.Tests.dll"
            ],
            "IsTelemetryOptedIn": true
          }
        }
        """;

    // ── V7 wire format ───────────────────────────────────────────────────
    // Protocol versions 2, 4, 5, 6, 7 — includes Version in the envelope.
    private static readonly string V7Json = """
        {
          "Version": 7,
          "MessageType": "DataCollection.BeforeTestRunStart",
          "Payload": {
            "SettingsXml": "\u003CRunSettings\u003E\u003CDataCollectionRunSettings\u003E\u003CDataCollectors\u003E\u003CDataCollector friendlyName=\u0022Code Coverage\u0022\u003E\u003CConfiguration\u003E\u003CCodeCoverage /\u003E\u003C/Configuration\u003E\u003C/DataCollector\u003E\u003C/DataCollectors\u003E\u003C/DataCollectionRunSettings\u003E\u003C/RunSettings\u003E",
            "Sources": [
              "Contoso.Math.Tests.dll",
              "Contoso.Core.Tests.dll"
            ],
            "IsTelemetryOptedIn": true
          }
        }
        """;

    // ── Serialize ────────────────────────────────────────────────────────

    [TestMethod]
    public void SerializePayloadV1()
    {
        var json = JsonDataSerializer.Instance.SerializePayload(
            MessageType.BeforeTestRunStart, Payload, version: 1);

        AssertJsonEqual(V1Json, json);
    }

    [TestMethod]
    public void SerializePayloadV7()
    {
        var json = JsonDataSerializer.Instance.SerializePayload(
            MessageType.BeforeTestRunStart, Payload, version: 7);

        AssertJsonEqual(V7Json, json);
    }

    // ── Deserialize ──────────────────────────────────────────────────────

    [TestMethod]
    public void DeserializePayloadV1()
    {
        var message = JsonDataSerializer.Instance.DeserializeMessage(Minify(V1Json));
        var result = JsonDataSerializer.Instance.DeserializePayload<BeforeTestRunStartPayload>(message);

        Assert.IsNotNull(result);
        Assert.AreEqual(Payload.SettingsXml, result.SettingsXml);
        Assert.IsNotNull(result.Sources);
        var sources = result.Sources.ToList();
        Assert.HasCount(2, sources);
        Assert.AreEqual("Contoso.Math.Tests.dll", sources[0]);
        Assert.AreEqual("Contoso.Core.Tests.dll", sources[1]);
        Assert.IsTrue(result.IsTelemetryOptedIn);
    }

    [TestMethod]
    public void DeserializePayloadV7()
    {
        var message = JsonDataSerializer.Instance.DeserializeMessage(Minify(V7Json));
        var result = JsonDataSerializer.Instance.DeserializePayload<BeforeTestRunStartPayload>(message);

        Assert.IsNotNull(result);
        Assert.AreEqual(Payload.SettingsXml, result.SettingsXml);
        Assert.IsNotNull(result.Sources);
        var sources = result.Sources.ToList();
        Assert.HasCount(2, sources);
        Assert.AreEqual("Contoso.Math.Tests.dll", sources[0]);
        Assert.AreEqual("Contoso.Core.Tests.dll", sources[1]);
        Assert.IsTrue(result.IsTelemetryOptedIn);
    }

    // ── Round-trip ───────────────────────────────────────────────────────

    [TestMethod]
    [DataRow(1)]
    [DataRow(7)]
    public void RoundTrip(int version)
    {
        var json = JsonDataSerializer.Instance.SerializePayload(
            MessageType.BeforeTestRunStart, Payload, version);
        var message = JsonDataSerializer.Instance.DeserializeMessage(json);
        var result = JsonDataSerializer.Instance.DeserializePayload<BeforeTestRunStartPayload>(message);

        Assert.IsNotNull(result);
        Assert.AreEqual(Payload.SettingsXml, result.SettingsXml);
        Assert.IsNotNull(result.Sources);
        var sources = result.Sources.ToList();
        Assert.HasCount(2, sources);
        Assert.AreEqual("Contoso.Math.Tests.dll", sources[0]);
        Assert.AreEqual("Contoso.Core.Tests.dll", sources[1]);
        Assert.IsTrue(result.IsTelemetryOptedIn);
    }

    // ── Helpers ──────────────────────────────────────────────────────────

}
