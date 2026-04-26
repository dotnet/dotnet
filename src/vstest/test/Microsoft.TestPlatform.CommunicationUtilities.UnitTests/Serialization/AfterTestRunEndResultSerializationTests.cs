// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.Common.DataCollection;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Microsoft.TestPlatform.CommunicationUtilities.UnitTests.Serialization.SerializationTestHelpers;

namespace Microsoft.TestPlatform.CommunicationUtilities.UnitTests.Serialization;

/// <summary>
/// Wire-format tests for <see cref="MessageType.AfterTestRunEndResult"/> ("DataCollection.AfterTestRunEndResult").
///
/// Result sent from data collector after a test run ends. Contains attachment sets, invoked
/// collectors, and metrics. Payload is identical for V1/V7.
/// The only difference is the outer envelope: V1 omits the Version field, V7 includes it.
/// </summary>
[TestClass]
[TestCategory("Serialization")]
public class AfterTestRunEndResultSerializationTests
{
    // ── Example payload ──────────────────────────────────────────────────
    private static readonly AfterTestRunEndResult Payload = BuildPayload();

    private static AfterTestRunEndResult BuildPayload()
    {
        var attachmentSet = new AttachmentSet(
            new Uri("datacollector://microsoft/CodeCoverage/2.0"),
            "Code Coverage");
        attachmentSet.Attachments.Add(
            UriDataAttachment.CreateFrom(@"C:\TestResults\coverage.cobertura.xml", "Coverage Report"));

        var invokedCollector = new InvokedDataCollector(
            new Uri("datacollector://microsoft/CodeCoverage/2.0"),
            "Code Coverage",
            "Microsoft.VisualStudio.Coverage.DynamicCoverageDataCollector, Microsoft.VisualStudio.TraceDataCollector",
            @"C:\Extensions\coverage.dll",
            hasAttachmentProcessor: true);

        return new AfterTestRunEndResult(
            new Collection<AttachmentSet> { attachmentSet },
            new Collection<InvokedDataCollector> { invokedCollector },
            new Dictionary<string, object>
            {
                ["TotalTests"] = 42
            });
    }

    // ── V1 wire format ───────────────────────────────────────────────────
    // Protocol versions 0, 1, 3 — no Version field in the envelope.
    private static readonly string V1Json = """
        {
          "MessageType": "DataCollection.AfterTestRunEndResult",
          "Payload": {
            "AttachmentSets": [
              {
                "Uri": "datacollector://microsoft/CodeCoverage/2.0",
                "DisplayName": "Code Coverage",
                "Attachments": [
                  {
                    "Description": "Coverage Report",
                    "Uri": "file://C:/TestResults/coverage.cobertura.xml"
                  }
                ]
              }
            ],
            "InvokedDataCollectors": [
              {
                "Uri": "datacollector://microsoft/CodeCoverage/2.0",
                "FriendlyName": "Code Coverage",
                "AssemblyQualifiedName": "Microsoft.VisualStudio.Coverage.DynamicCoverageDataCollector, Microsoft.VisualStudio.TraceDataCollector",
                "FilePath": "C:\\Extensions\\coverage.dll",
                "HasAttachmentProcessor": true
              }
            ],
            "Metrics": {
              "TotalTests": 42
            }
          }
        }
        """;

    // ── V7 wire format ───────────────────────────────────────────────────
    // Protocol versions 2, 4, 5, 6, 7 — includes Version in the envelope.
    private static readonly string V7Json = """
        {
          "Version": 7,
          "MessageType": "DataCollection.AfterTestRunEndResult",
          "Payload": {
            "AttachmentSets": [
              {
                "Uri": "datacollector://microsoft/CodeCoverage/2.0",
                "DisplayName": "Code Coverage",
                "Attachments": [
                  {
                    "Description": "Coverage Report",
                    "Uri": "file://C:/TestResults/coverage.cobertura.xml"
                  }
                ]
              }
            ],
            "InvokedDataCollectors": [
              {
                "Uri": "datacollector://microsoft/CodeCoverage/2.0",
                "FriendlyName": "Code Coverage",
                "AssemblyQualifiedName": "Microsoft.VisualStudio.Coverage.DynamicCoverageDataCollector, Microsoft.VisualStudio.TraceDataCollector",
                "FilePath": "C:\\Extensions\\coverage.dll",
                "HasAttachmentProcessor": true
              }
            ],
            "Metrics": {
              "TotalTests": 42
            }
          }
        }
        """;

    // ── Serialize ────────────────────────────────────────────────────────

    [TestMethod]
    public void SerializePayloadV1()
    {
        var json = JsonDataSerializer.Instance.SerializePayload(
            MessageType.AfterTestRunEndResult, Payload, version: 1);

        AssertJsonEqual(V1Json, json);
    }

    [TestMethod]
    public void SerializePayloadV7()
    {
        var json = JsonDataSerializer.Instance.SerializePayload(
            MessageType.AfterTestRunEndResult, Payload, version: 7);

        AssertJsonEqual(V7Json, json);
    }

    // ── Deserialize ──────────────────────────────────────────────────────

    [TestMethod]
    public void DeserializePayloadV1()
    {
        var message = JsonDataSerializer.Instance.DeserializeMessage(Minify(V1Json));
        var result = JsonDataSerializer.Instance.DeserializePayload<AfterTestRunEndResult>(message);

        Assert.IsNotNull(result);
        Assert.IsNotNull(result.AttachmentSets);
        Assert.HasCount(1, result.AttachmentSets);
        var attachment = result.AttachmentSets[0];
        Assert.AreEqual("datacollector://microsoft/CodeCoverage/2.0", attachment.Uri.AbsoluteUri);
        Assert.AreEqual("Code Coverage", attachment.DisplayName);
        Assert.HasCount(1, attachment.Attachments);
        Assert.AreEqual("Coverage Report", attachment.Attachments[0].Description);
        Assert.IsNotNull(result.InvokedDataCollectors);
        Assert.HasCount(1, result.InvokedDataCollectors);
        Assert.AreEqual("Code Coverage", result.InvokedDataCollectors[0].FriendlyName);
        Assert.IsTrue(result.InvokedDataCollectors[0].HasAttachmentProcessor);
    }

    [TestMethod]
    public void DeserializePayloadV7()
    {
        var message = JsonDataSerializer.Instance.DeserializeMessage(Minify(V7Json));
        var result = JsonDataSerializer.Instance.DeserializePayload<AfterTestRunEndResult>(message);

        Assert.IsNotNull(result);
        Assert.IsNotNull(result.AttachmentSets);
        Assert.HasCount(1, result.AttachmentSets);
        var attachment = result.AttachmentSets[0];
        Assert.AreEqual("datacollector://microsoft/CodeCoverage/2.0", attachment.Uri.AbsoluteUri);
        Assert.AreEqual("Code Coverage", attachment.DisplayName);
        Assert.HasCount(1, attachment.Attachments);
        Assert.AreEqual("Coverage Report", attachment.Attachments[0].Description);
        Assert.IsNotNull(result.InvokedDataCollectors);
        Assert.HasCount(1, result.InvokedDataCollectors);
        Assert.AreEqual("Code Coverage", result.InvokedDataCollectors[0].FriendlyName);
        Assert.IsTrue(result.InvokedDataCollectors[0].HasAttachmentProcessor);
    }

    // ── Round-trip ───────────────────────────────────────────────────────

    [TestMethod]
    [DataRow(1)]
    [DataRow(7)]
    public void RoundTrip(int version)
    {
        var json = JsonDataSerializer.Instance.SerializePayload(
            MessageType.AfterTestRunEndResult, Payload, version);
        var message = JsonDataSerializer.Instance.DeserializeMessage(json);
        var result = JsonDataSerializer.Instance.DeserializePayload<AfterTestRunEndResult>(message);

        Assert.IsNotNull(result);
        Assert.IsNotNull(result.AttachmentSets);
        Assert.HasCount(1, result.AttachmentSets);
        Assert.AreEqual("Code Coverage", result.AttachmentSets[0].DisplayName);
        Assert.IsNotNull(result.InvokedDataCollectors);
        Assert.HasCount(1, result.InvokedDataCollectors);
        Assert.AreEqual("Code Coverage", result.InvokedDataCollectors[0].FriendlyName);
    }

    // ── Helpers ──────────────────────────────────────────────────────────

}
