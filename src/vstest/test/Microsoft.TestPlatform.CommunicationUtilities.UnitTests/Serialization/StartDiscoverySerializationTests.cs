// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Microsoft.TestPlatform.CommunicationUtilities.UnitTests.Serialization.SerializationTestHelpers;

namespace Microsoft.TestPlatform.CommunicationUtilities.UnitTests.Serialization;

/// <summary>
/// Wire-format tests for <see cref="MessageType.StartDiscovery"/> ("TestDiscovery.Start").
///
/// This message is sent to start test discovery with the given criteria.
/// The payload is <see cref="DiscoveryCriteria"/> which contains sources, run settings, and filters.
///
/// Payload is identical for V1 and V7 because no TestCase/TestResult objects are involved.
/// The only difference is the outer envelope: V1 omits the Version field, V7 includes it.
/// </summary>
[TestClass]
[TestCategory("Serialization")]
public class StartDiscoverySerializationTests
{
    // ── Example payload ──────────────────────────────────────────────────
    private static readonly DiscoveryCriteria Payload = BuildPayload();

    private static DiscoveryCriteria BuildPayload()
    {
        var criteria = new DiscoveryCriteria(
            new[] { "Contoso.Math.Tests.dll", "Contoso.Core.Tests.dll" },
            frequencyOfDiscoveredTestsEvent: 10,
            discoveredTestEventTimeout: TimeSpan.FromSeconds(30),
            runSettings: @"<RunSettings><RunConfiguration><ResultsDirectory>.\TestResults</ResultsDirectory></RunConfiguration></RunSettings>");

        criteria.TestCaseFilter = "Category=Unit";

        return criteria;
    }

    // ── V1 wire format ───────────────────────────────────────────────────
    // Protocol versions 0, 1, 3 — no Version field in the envelope.
    private static readonly string V1Json = """
        {
          "MessageType": "TestDiscovery.Start",
          "Payload": {
            "Package": null,
            "AdapterSourceMap": {
              "_none_": [
                "Contoso.Math.Tests.dll",
                "Contoso.Core.Tests.dll"
              ]
            },
            "FrequencyOfDiscoveredTestsEvent": 10,
            "DiscoveredTestEventTimeout": "00:00:30",
            "RunSettings": "\u003CRunSettings\u003E\u003CRunConfiguration\u003E\u003CResultsDirectory\u003E.\\TestResults\u003C/ResultsDirectory\u003E\u003C/RunConfiguration\u003E\u003C/RunSettings\u003E",
            "TestCaseFilter": "Category=Unit",
            "TestSessionInfo": null
          }
        }
        """;

    // ── V7 wire format ───────────────────────────────────────────────────
    // Protocol versions 2, 4, 5, 6, 7 — includes Version in the envelope.
    private static readonly string V7Json = """
        {
          "Version": 7,
          "MessageType": "TestDiscovery.Start",
          "Payload": {
            "Package": null,
            "AdapterSourceMap": {
              "_none_": [
                "Contoso.Math.Tests.dll",
                "Contoso.Core.Tests.dll"
              ]
            },
            "FrequencyOfDiscoveredTestsEvent": 10,
            "DiscoveredTestEventTimeout": "00:00:30",
            "RunSettings": "\u003CRunSettings\u003E\u003CRunConfiguration\u003E\u003CResultsDirectory\u003E.\\TestResults\u003C/ResultsDirectory\u003E\u003C/RunConfiguration\u003E\u003C/RunSettings\u003E",
            "TestCaseFilter": "Category=Unit",
            "TestSessionInfo": null
          }
        }
        """;

    // ── Serialize ────────────────────────────────────────────────────────

    [TestMethod]
    public void SerializePayloadV1()
    {
        var json = JsonDataSerializer.Instance.SerializePayload(
            MessageType.StartDiscovery, Payload, version: 1);

        AssertJsonEqual(V1Json, json);
    }

    [TestMethod]
    public void SerializePayloadV7()
    {
        var json = JsonDataSerializer.Instance.SerializePayload(
            MessageType.StartDiscovery, Payload, version: 7);

        AssertJsonEqual(V7Json, json);
    }

    // ── Deserialize ──────────────────────────────────────────────────────

    [TestMethod]
    public void DeserializePayloadV1()
    {
        var message = JsonDataSerializer.Instance.DeserializeMessage(Minify(V1Json));
        var result = JsonDataSerializer.Instance.DeserializePayload<DiscoveryCriteria>(message);

        Assert.IsNotNull(result);
        Assert.IsNotNull(result.AdapterSourceMap);
        Assert.IsTrue(result.AdapterSourceMap.ContainsKey("_none_"));
        var sources = result.AdapterSourceMap["_none_"].ToList();
        Assert.HasCount(2, sources);
        Assert.AreEqual("Contoso.Math.Tests.dll", sources[0]);
        Assert.AreEqual("Contoso.Core.Tests.dll", sources[1]);
        Assert.AreEqual(10, result.FrequencyOfDiscoveredTestsEvent);
        Assert.AreEqual(TimeSpan.FromSeconds(30), result.DiscoveredTestEventTimeout);
        Assert.AreEqual(@"<RunSettings><RunConfiguration><ResultsDirectory>.\TestResults</ResultsDirectory></RunConfiguration></RunSettings>", result.RunSettings);
        Assert.AreEqual("Category=Unit", result.TestCaseFilter);
        Assert.IsNull(result.TestSessionInfo);
    }

    [TestMethod]
    public void DeserializePayloadV7()
    {
        var message = JsonDataSerializer.Instance.DeserializeMessage(Minify(V7Json));
        var result = JsonDataSerializer.Instance.DeserializePayload<DiscoveryCriteria>(message);

        Assert.IsNotNull(result);
        Assert.IsNotNull(result.AdapterSourceMap);
        Assert.IsTrue(result.AdapterSourceMap.ContainsKey("_none_"));
        var sources = result.AdapterSourceMap["_none_"].ToList();
        Assert.HasCount(2, sources);
        Assert.AreEqual("Contoso.Math.Tests.dll", sources[0]);
        Assert.AreEqual("Contoso.Core.Tests.dll", sources[1]);
        Assert.AreEqual(10, result.FrequencyOfDiscoveredTestsEvent);
        Assert.AreEqual(TimeSpan.FromSeconds(30), result.DiscoveredTestEventTimeout);
        Assert.AreEqual(@"<RunSettings><RunConfiguration><ResultsDirectory>.\TestResults</ResultsDirectory></RunConfiguration></RunSettings>", result.RunSettings);
        Assert.AreEqual("Category=Unit", result.TestCaseFilter);
        Assert.IsNull(result.TestSessionInfo);
    }

    // ── Round-trip ───────────────────────────────────────────────────────

    [TestMethod]
    [DataRow(1)]
    [DataRow(7)]
    public void RoundTrip(int version)
    {
        var json = JsonDataSerializer.Instance.SerializePayload(
            MessageType.StartDiscovery, Payload, version);
        var message = JsonDataSerializer.Instance.DeserializeMessage(json);
        var result = JsonDataSerializer.Instance.DeserializePayload<DiscoveryCriteria>(message);

        Assert.IsNotNull(result);
        Assert.IsNotNull(result.AdapterSourceMap);
        Assert.IsTrue(result.AdapterSourceMap.ContainsKey("_none_"));
        var sources = result.AdapterSourceMap["_none_"].ToList();
        Assert.HasCount(2, sources);
        Assert.AreEqual("Contoso.Math.Tests.dll", sources[0]);
        Assert.AreEqual("Contoso.Core.Tests.dll", sources[1]);
        Assert.AreEqual(10, result.FrequencyOfDiscoveredTestsEvent);
        Assert.AreEqual(TimeSpan.FromSeconds(30), result.DiscoveredTestEventTimeout);
        Assert.AreEqual("Category=Unit", result.TestCaseFilter);
    }

    // ── Helpers ──────────────────────────────────────────────────────────

}
