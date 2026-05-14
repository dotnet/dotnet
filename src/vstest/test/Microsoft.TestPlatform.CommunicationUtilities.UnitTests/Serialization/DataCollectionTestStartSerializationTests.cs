// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Microsoft.TestPlatform.CommunicationUtilities.UnitTests.Serialization.SerializationTestHelpers;

namespace Microsoft.TestPlatform.CommunicationUtilities.UnitTests.Serialization;

/// <summary>
/// Wire-format tests for <see cref="MessageType.DataCollectionTestStart"/>
/// ("DataCollection.TestStart").
///
/// This message is sent by the test execution engine to data collectors when a test case starts.
/// The payload is <see cref="TestCaseStartEventArgs"/> containing the test case metadata and
/// data collection context.
///
/// V1 and V7 differ in how the nested <see cref="TestCase"/> object (TestElement) is serialized:
/// V1 uses the Properties array format, V7 uses flat objects with well-known fields.
/// </summary>
[TestClass]
[TestCategory("Serialization")]
public class DataCollectionTestStartSerializationTests
{
    // ── Example payload ──────────────────────────────────────────────────
    private static readonly TestCaseStartEventArgs Payload = BuildPayload();

    // ── V1 wire format ───────────────────────────────────────────────────
    // Protocol versions 0, 1, 3 — TestCase uses Properties array format.
    private static readonly string V1Json = """
        {
          "MessageType": "DataCollection.TestStart",
          "Payload": {
            "TestCaseId": "a1b2c3d4-e5f6-7890-abcd-ef1234567890",
            "TestCaseName": "Contoso.Tests.MyTest",
            "IsChildTestCase": false,
            "TestElement": {
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
                  "Value": "Contoso.Tests.MyTest"
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
                  "Value": "Tests.dll"
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
                  "Value": "Contoso.Tests.MyTest"
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
                  "Value": "a1b2c3d4-e5f6-7890-abcd-ef1234567890"
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
            },
            "Context": {
              "TestCase": null,
              "SessionId": {
                "Id": "00000000-0000-0000-0000-000000000000"
              },
              "TestExecId": null,
              "HasTestCase": false
            }
          }
        }
        """;

    // ── V7 wire format ───────────────────────────────────────────────────
    // Protocol versions 2, 4, 5, 6, 7 — TestCase as flat object.
    private static readonly string V7Json = """
        {
          "Version": 7,
          "MessageType": "DataCollection.TestStart",
          "Payload": {
            "TestCaseId": "a1b2c3d4-e5f6-7890-abcd-ef1234567890",
            "TestCaseName": "Contoso.Tests.MyTest",
            "IsChildTestCase": false,
            "TestElement": {
              "Id": "a1b2c3d4-e5f6-7890-abcd-ef1234567890",
              "FullyQualifiedName": "Contoso.Tests.MyTest",
              "DisplayName": "Contoso.Tests.MyTest",
              "ExecutorUri": "executor://MSTestAdapter/v2",
              "Source": "Tests.dll",
              "CodeFilePath": null,
              "LineNumber": -1,
              "Properties": []
            },
            "Context": {
              "TestCase": null,
              "SessionId": {
                "Id": "00000000-0000-0000-0000-000000000000"
              },
              "TestExecId": null,
              "HasTestCase": false
            }
          }
        }
        """;

    // ── Serialize ────────────────────────────────────────────────────────

    [TestMethod]
    public void SerializePayloadV1()
    {
        var json = JsonDataSerializer.Instance.SerializePayload(
            MessageType.DataCollectionTestStart, Payload, version: 1);

        AssertJsonEqual(V1Json, json);
    }

    [TestMethod]
    public void SerializePayloadV7()
    {
        var json = JsonDataSerializer.Instance.SerializePayload(
            MessageType.DataCollectionTestStart, Payload, version: 7);

        AssertJsonEqual(V7Json, json);
    }

    // ── Deserialize ──────────────────────────────────────────────────────

    [TestMethod]
    public void DeserializePayloadV1()
    {
        var message = JsonDataSerializer.Instance.DeserializeMessage(Minify(V1Json));
        var result = JsonDataSerializer.Instance.DeserializePayload<TestCaseStartEventArgs>(message);

        AssertPayloadFields(result);
    }

    [TestMethod]
    public void DeserializePayloadV7()
    {
        var message = JsonDataSerializer.Instance.DeserializeMessage(Minify(V7Json));
        var result = JsonDataSerializer.Instance.DeserializePayload<TestCaseStartEventArgs>(message);

        AssertPayloadFields(result);
    }

    // ── Round-trip ───────────────────────────────────────────────────────

    [TestMethod]
    [DataRow(1)]
    [DataRow(7)]
    public void RoundTrip(int version)
    {
        var json = JsonDataSerializer.Instance.SerializePayload(
            MessageType.DataCollectionTestStart, Payload, version);
        var message = JsonDataSerializer.Instance.DeserializeMessage(json);
        var result = JsonDataSerializer.Instance.DeserializePayload<TestCaseStartEventArgs>(message);

        Assert.IsNotNull(result);
        Assert.AreEqual(new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), result.TestCaseId);
        Assert.AreEqual("Contoso.Tests.MyTest", result.TestCaseName);
    }

    // ── Helpers ──────────────────────────────────────────────────────────

    private static TestCaseStartEventArgs BuildPayload()
    {
        var tc = new TestCase(
            "Contoso.Tests.MyTest",
            new Uri("executor://MSTestAdapter/v2"),
            "Tests.dll")
        {
            Id = new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"),
        };

        return new TestCaseStartEventArgs(tc);
    }

    private static void AssertPayloadFields(TestCaseStartEventArgs? result)
    {
        Assert.IsNotNull(result);
        Assert.AreEqual(new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), result.TestCaseId);
        Assert.AreEqual("Contoso.Tests.MyTest", result.TestCaseName);
        Assert.IsFalse(result.IsChildTestCase);
        Assert.IsNotNull(result.TestElement);
        Assert.AreEqual("Contoso.Tests.MyTest", result.TestElement.FullyQualifiedName);
        Assert.AreEqual("executor://mstestadapter/v2", result.TestElement.ExecutorUri.AbsoluteUri);
        Assert.AreEqual("Tests.dll", result.TestElement.Source);
    }

}
