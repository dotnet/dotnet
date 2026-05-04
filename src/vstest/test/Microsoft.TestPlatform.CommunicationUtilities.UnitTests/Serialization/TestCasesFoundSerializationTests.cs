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
/// Wire-format tests for <see cref="MessageType.TestCasesFound"/> ("TestDiscovery.TestFound").
///
/// This message is sent by the test host during discovery to report discovered test cases.
/// The payload is a <see cref="List{TestCase}"/> containing the batch of discovered tests.
///
/// V1 and V7 wire formats are dramatically different:
/// - V1 serializes each <see cref="TestCase"/> as a Properties array with Key/Value objects,
///   where each property Key contains metadata (Id, Label, Category, Attributes, ValueType).
/// - V7 serializes each <see cref="TestCase"/> as a flat JSON object with well-known fields
///   (Id, FullyQualifiedName, DisplayName, etc.) and only custom/non-default properties
///   remain in the Properties array.
/// </summary>
[TestClass]
[TestCategory("Serialization")]
public class TestCasesFoundSerializationTests
{
    // ── Example payload ──────────────────────────────────────────────────
    // A single discovered test case with traits, line number, and code file path.
    private static readonly List<TestCase> Payload = new() { BuildTestCase() };

    // ── V1 wire format ───────────────────────────────────────────────────
    // Protocol versions 0, 1, 3 — TestCase serialized as Properties array.
    private static readonly string V1Json = """
        {
          "MessageType": "TestDiscovery.TestFound",
          "Payload": [
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
                  "Value": "Contoso.Math.Tests.CalculatorTests.AddTest"
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
                  "Value": "C:\\src\\Contoso.Math.Tests\\CalculatorTests.cs"
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
                  "Value": "AddTest(1, 2, 3)"
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
                  "Value": 42
                },
                {
                  "Key": {
                    "Id": "TestObject.Traits",
                    "Label": "Traits",
                    "Category": "",
                    "Description": "",
                    "Attributes": 5,
                    "ValueType": "System.Collections.Generic.KeyValuePair\u00602[[System.String],[System.String]][]"
                  },
                  "Value": [
                    {
                      "Key": "Category",
                      "Value": "Unit"
                    },
                    {
                      "Key": "Priority",
                      "Value": "1"
                    }
                  ]
                }
              ]
            }
          ]
        }
        """;

    // ── V7 wire format ───────────────────────────────────────────────────
    // Protocol versions 2, 4, 5, 6, 7 — TestCase as flat object with only
    // non-default properties in the Properties array (here: just Traits).
    private static readonly string V7Json = """
        {
          "Version": 7,
          "MessageType": "TestDiscovery.TestFound",
          "Payload": [
            {
              "Id": "a1b2c3d4-e5f6-7890-abcd-ef1234567890",
              "FullyQualifiedName": "Contoso.Math.Tests.CalculatorTests.AddTest",
              "DisplayName": "AddTest(1, 2, 3)",
              "ExecutorUri": "executor://MSTestAdapter/v2",
              "Source": "Contoso.Math.Tests.dll",
              "CodeFilePath": "C:\\src\\Contoso.Math.Tests\\CalculatorTests.cs",
              "LineNumber": 42,
              "Properties": [
                {
                  "Key": {
                    "Id": "TestObject.Traits",
                    "Label": "Traits",
                    "Category": "",
                    "Description": "",
                    "Attributes": 5,
                    "ValueType": "System.Collections.Generic.KeyValuePair\u00602[[System.String],[System.String]][]"
                  },
                  "Value": [
                    {
                      "Key": "Category",
                      "Value": "Unit"
                    },
                    {
                      "Key": "Priority",
                      "Value": "1"
                    }
                  ]
                }
              ]
            }
          ]
        }
        """;

    // ── Serialize ────────────────────────────────────────────────────────

    [TestMethod]
    public void SerializePayloadV1()
    {
        var json = JsonDataSerializer.Instance.SerializePayload(
            MessageType.TestCasesFound, Payload, version: 1);

        AssertJsonEqual(V1Json, json);
    }

    [TestMethod]
    public void SerializePayloadV7()
    {
        var json = JsonDataSerializer.Instance.SerializePayload(
            MessageType.TestCasesFound, Payload, version: 7);

        AssertJsonEqual(V7Json, json);
    }

    // ── Deserialize ──────────────────────────────────────────────────────

    [TestMethod]
    public void DeserializePayloadV1()
    {
        var message = JsonDataSerializer.Instance.DeserializeMessage(Minify(V1Json));
        var result = JsonDataSerializer.Instance.DeserializePayload<IEnumerable<TestCase>>(message);

        Assert.IsNotNull(result);
        var tc = result.First();
        Assert.AreEqual("Contoso.Math.Tests.CalculatorTests.AddTest", tc.FullyQualifiedName);
        Assert.AreEqual(new Uri("executor://MSTestAdapter/v2"), tc.ExecutorUri);
        Assert.AreEqual("Contoso.Math.Tests.dll", tc.Source);
        Assert.AreEqual(@"C:\src\Contoso.Math.Tests\CalculatorTests.cs", tc.CodeFilePath);
        Assert.AreEqual("AddTest(1, 2, 3)", tc.DisplayName);
        Assert.AreEqual(new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), tc.Id);
        Assert.AreEqual(42, tc.LineNumber);
        var traits = tc.Traits.ToList();
        Assert.HasCount(2, traits);
        Assert.AreEqual("Category", traits[0].Name);
        Assert.AreEqual("Unit", traits[0].Value);
    }

    [TestMethod]
    public void DeserializePayloadV7()
    {
        var message = JsonDataSerializer.Instance.DeserializeMessage(Minify(V7Json));
        var result = JsonDataSerializer.Instance.DeserializePayload<IEnumerable<TestCase>>(message);

        Assert.IsNotNull(result);
        var tc = result.First();
        Assert.AreEqual("Contoso.Math.Tests.CalculatorTests.AddTest", tc.FullyQualifiedName);
        Assert.AreEqual(new Uri("executor://MSTestAdapter/v2"), tc.ExecutorUri);
        Assert.AreEqual("Contoso.Math.Tests.dll", tc.Source);
        Assert.AreEqual(@"C:\src\Contoso.Math.Tests\CalculatorTests.cs", tc.CodeFilePath);
        Assert.AreEqual("AddTest(1, 2, 3)", tc.DisplayName);
        Assert.AreEqual(new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), tc.Id);
        Assert.AreEqual(42, tc.LineNumber);
        var traits = tc.Traits.ToList();
        Assert.HasCount(2, traits);
        Assert.AreEqual("Category", traits[0].Name);
        Assert.AreEqual("Unit", traits[0].Value);
    }

    // ── Round-trip ───────────────────────────────────────────────────────

    [TestMethod]
    [DataRow(1)]
    [DataRow(7)]
    public void RoundTrip(int version)
    {
        var json = JsonDataSerializer.Instance.SerializePayload(
            MessageType.TestCasesFound, Payload, version);
        var message = JsonDataSerializer.Instance.DeserializeMessage(json);
        var result = JsonDataSerializer.Instance.DeserializePayload<IEnumerable<TestCase>>(message);

        Assert.IsNotNull(result);
        var tc = result.First();
        Assert.AreEqual(Payload[0].FullyQualifiedName, tc.FullyQualifiedName);
        Assert.AreEqual(Payload[0].DisplayName, tc.DisplayName);
        Assert.AreEqual(Payload[0].Id, tc.Id);
        Assert.AreEqual(Payload[0].Source, tc.Source);
        Assert.AreEqual(Payload[0].LineNumber, tc.LineNumber);
    }

    // ── Helpers ──────────────────────────────────────────────────────────

    private static TestCase BuildTestCase()
    {
        return new TestCase(
            "Contoso.Math.Tests.CalculatorTests.AddTest",
            new Uri("executor://MSTestAdapter/v2"),
            "Contoso.Math.Tests.dll")
        {
            DisplayName = "AddTest(1, 2, 3)",
            Id = new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"),
            CodeFilePath = @"C:\src\Contoso.Math.Tests\CalculatorTests.cs",
            LineNumber = 42,
            Traits = { new Trait("Category", "Unit"), new Trait("Priority", "1") }
        };
    }

}
