// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Microsoft.TestPlatform.CommunicationUtilities.UnitTests.Serialization.SerializationTestHelpers;

namespace Microsoft.TestPlatform.CommunicationUtilities.UnitTests.Serialization;

/// <summary>
/// Wire-format tests for <see cref="MessageType.DiscoveryComplete"/> ("TestDiscovery.Completed").
///
/// This message is sent by the test host when test discovery finishes (or is aborted).
/// The payload is a <see cref="DiscoveryCompletePayload"/> containing the total count of
/// discovered tests, an abort flag, the last batch of discovered test cases, and metrics.
///
/// V1 and V7 differ in how the nested <see cref="TestCase"/> objects in LastDiscoveredTests
/// are serialized: V1 uses the Properties array format, V7 uses the flat object format.
/// The rest of the payload (TotalTests, IsAborted, Metrics, etc.) is identical.
/// </summary>
[TestClass]
[TestCategory("Serialization")]
public class DiscoveryCompleteSerializationTests
{
    // ── Example payload ──────────────────────────────────────────────────
    // Discovery completed successfully with 150 tests found. The last
    // batch contains one test case (SubtractTest).
    private static readonly DiscoveryCompletePayload Payload = BuildPayload();

    // ── V1 wire format ───────────────────────────────────────────────────
    // Protocol versions 0, 1, 3 — TestCase in LastDiscoveredTests uses Properties array.
    private static readonly string V1Json = """
        {
          "MessageType": "TestDiscovery.Completed",
          "Payload": {
            "TotalTests": 150,
            "LastDiscoveredTests": [
              {
                "Properties": [
                  {
                    "Key": {
                      "Id": "TestCase.FullyQualifiedName",
                      "Label": "FullyQualifiedName",
                      "Category": "",
                      "Description": "",
                      "Attributes": 1,
                      "ValueType": "System.String"
                    },
                    "Value": "Contoso.Math.Tests.CalculatorTests.SubtractTest"
                  },
                  {
                    "Key": {
                      "Id": "TestCase.ExecutorUri",
                      "Label": "Executor Uri",
                      "Category": "",
                      "Description": "",
                      "Attributes": 1,
                      "ValueType": "System.Uri"
                    },
                    "Value": "executor://MSTestAdapter/v2"
                  },
                  {
                    "Key": {
                      "Id": "TestCase.Source",
                      "Label": "Source",
                      "Category": "",
                      "Description": "",
                      "Attributes": 0,
                      "ValueType": "System.String"
                    },
                    "Value": "Contoso.Math.Tests.dll"
                  },
                  {
                    "Key": {
                      "Id": "TestCase.CodeFilePath",
                      "Label": "File Path",
                      "Category": "",
                      "Description": "",
                      "Attributes": 0,
                      "ValueType": "System.String"
                    },
                    "Value": null
                  },
                  {
                    "Key": {
                      "Id": "TestCase.DisplayName",
                      "Label": "Name",
                      "Category": "",
                      "Description": "",
                      "Attributes": 0,
                      "ValueType": "System.String"
                    },
                    "Value": "SubtractTest"
                  },
                  {
                    "Key": {
                      "Id": "TestCase.Id",
                      "Label": "Id",
                      "Category": "",
                      "Description": "",
                      "Attributes": 1,
                      "ValueType": "System.Guid"
                    },
                    "Value": "b2c3d4e5-f6a7-8901-bcde-f12345678901"
                  },
                  {
                    "Key": {
                      "Id": "TestCase.LineNumber",
                      "Label": "Line Number",
                      "Category": "",
                      "Description": "",
                      "Attributes": 1,
                      "ValueType": "System.Int32"
                    },
                    "Value": -1
                  }
                ]
              }
            ],
            "IsAborted": false,
            "Metrics": {
              "TotalTestsDiscovered": 150
            },
            "FullyDiscoveredSources": [],
            "PartiallyDiscoveredSources": [],
            "NotDiscoveredSources": [],
            "SkippedDiscoverySources": [],
            "DiscoveredExtensions": {}
          }
        }
        """;

    // ── V7 wire format ───────────────────────────────────────────────────
    // Protocol versions 2, 4, 5, 6, 7 — TestCase as flat object.
    private static readonly string V7Json = """
        {
          "Version": 7,
          "MessageType": "TestDiscovery.Completed",
          "Payload": {
            "TotalTests": 150,
            "LastDiscoveredTests": [
              {
                "Id": "b2c3d4e5-f6a7-8901-bcde-f12345678901",
                "FullyQualifiedName": "Contoso.Math.Tests.CalculatorTests.SubtractTest",
                "DisplayName": "SubtractTest",
                "ExecutorUri": "executor://MSTestAdapter/v2",
                "Source": "Contoso.Math.Tests.dll",
                "CodeFilePath": null,
                "LineNumber": -1,
                "Properties": []
              }
            ],
            "IsAborted": false,
            "Metrics": {
              "TotalTestsDiscovered": 150
            },
            "FullyDiscoveredSources": [],
            "PartiallyDiscoveredSources": [],
            "NotDiscoveredSources": [],
            "SkippedDiscoverySources": [],
            "DiscoveredExtensions": {}
          }
        }
        """;

    // ── Serialize ────────────────────────────────────────────────────────

    [TestMethod]
    public void SerializePayloadV1()
    {
        var json = JsonDataSerializer.Instance.SerializePayload(
            MessageType.DiscoveryComplete, Payload, version: 1);

        AssertJsonEqual(V1Json, json);
    }

    [TestMethod]
    public void SerializePayloadV7()
    {
        var json = JsonDataSerializer.Instance.SerializePayload(
            MessageType.DiscoveryComplete, Payload, version: 7);

        AssertJsonEqual(V7Json, json);
    }

    // ── Deserialize ──────────────────────────────────────────────────────

    [TestMethod]
    public void DeserializePayloadV1()
    {
        var message = JsonDataSerializer.Instance.DeserializeMessage(Minify(V1Json));
        var result = JsonDataSerializer.Instance.DeserializePayload<DiscoveryCompletePayload>(message);

        AssertPayloadFields(result);
    }

    [TestMethod]
    public void DeserializePayloadV7()
    {
        var message = JsonDataSerializer.Instance.DeserializeMessage(Minify(V7Json));
        var result = JsonDataSerializer.Instance.DeserializePayload<DiscoveryCompletePayload>(message);

        AssertPayloadFields(result);
    }

    // ── Round-trip ───────────────────────────────────────────────────────

    [TestMethod]
    [DataRow(1)]
    [DataRow(7)]
    public void RoundTrip(int version)
    {
        var json = JsonDataSerializer.Instance.SerializePayload(
            MessageType.DiscoveryComplete, Payload, version);
        var message = JsonDataSerializer.Instance.DeserializeMessage(json);
        var result = JsonDataSerializer.Instance.DeserializePayload<DiscoveryCompletePayload>(message);

        Assert.IsNotNull(result);
        Assert.AreEqual(Payload.TotalTests, result.TotalTests);
        Assert.AreEqual(Payload.IsAborted, result.IsAborted);
        Assert.IsNotNull(result.LastDiscoveredTests);
        var tc = result.LastDiscoveredTests.First();
        Assert.AreEqual("Contoso.Math.Tests.CalculatorTests.SubtractTest", tc.FullyQualifiedName);
    }

    // ── Helpers ──────────────────────────────────────────────────────────

    private static DiscoveryCompletePayload BuildPayload()
    {
        return new DiscoveryCompletePayload
        {
            TotalTests = 150,
            IsAborted = false,
            LastDiscoveredTests = new List<TestCase>
            {
                new("Contoso.Math.Tests.CalculatorTests.SubtractTest",
                    new Uri("executor://MSTestAdapter/v2"), "Contoso.Math.Tests.dll")
                {
                    DisplayName = "SubtractTest",
                    Id = new Guid("b2c3d4e5-f6a7-8901-bcde-f12345678901"),
                }
            },
            Metrics = new Dictionary<string, object> { ["TotalTestsDiscovered"] = 150 },
        };
    }

    private static void AssertPayloadFields(DiscoveryCompletePayload? result)
    {
        Assert.IsNotNull(result);
        Assert.AreEqual(150, result.TotalTests);
        Assert.IsFalse(result.IsAborted);
        Assert.IsNotNull(result.LastDiscoveredTests);
        var tests = result.LastDiscoveredTests.ToList();
        Assert.HasCount(1, tests);
        Assert.AreEqual("Contoso.Math.Tests.CalculatorTests.SubtractTest", tests[0].FullyQualifiedName);
        Assert.AreEqual(new Uri("executor://MSTestAdapter/v2"), tests[0].ExecutorUri);
        Assert.AreEqual("Contoso.Math.Tests.dll", tests[0].Source);
        Assert.AreEqual("SubtractTest", tests[0].DisplayName);
        Assert.AreEqual(new Guid("b2c3d4e5-f6a7-8901-bcde-f12345678901"), tests[0].Id);
    }

}
