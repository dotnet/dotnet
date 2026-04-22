// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Engine.ClientProtocol;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Microsoft.TestPlatform.CommunicationUtilities.UnitTests.Serialization.SerializationTestHelpers;

namespace Microsoft.TestPlatform.CommunicationUtilities.UnitTests.Serialization;

/// <summary>
/// Wire-format tests for <see cref="MessageType.StartTestExecutionWithTests"/> ("TestExecution.StartWithTests").
///
/// This message is sent to start test execution with specific test cases and execution context.
/// The payload is <see cref="TestRunCriteriaWithTests"/> which contains tests, run settings, and execution context.
///
/// V1 and V7 differ because TestCase objects are serialized as Properties array in V1
/// vs flat objects in V7.
/// </summary>
[TestClass]
[TestCategory("Serialization")]
public class StartTestExecutionWithTestsSerializationTests
{
    // ── Example payload ──────────────────────────────────────────────────
    private static readonly TestRunCriteriaWithTests Payload = BuildPayload();

    private static TestRunCriteriaWithTests BuildPayload()
    {
        var testCase = new TestCase(
            "Contoso.Math.Tests.CalculatorTests.AddTest",
            new Uri("executor://MSTestAdapter/v2"),
            "Contoso.Math.Tests.dll")
        {
            DisplayName = "AddTest(1, 2, 3)",
            Id = new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"),
            CodeFilePath = @"C:\src\Contoso.Math.Tests\CalculatorTests.cs",
            LineNumber = 42
        };

        var ctx = new TestExecutionContext(
            frequencyOfRunStatsChangeEvent: 10,
            runStatsChangeEventTimeout: TimeSpan.FromSeconds(30),
            inIsolation: false,
            keepAlive: true,
            isDataCollectionEnabled: false,
            areTestCaseLevelEventsRequired: true,
            hasTestRun: true,
            isDebug: false,
            testCaseFilter: null,
            filterOptions: null);

        return new TestRunCriteriaWithTests(
            new[] { testCase },
            "Contoso.Math.Tests",
            @"<RunSettings><RunConfiguration><ResultsDirectory>.\TestResults</ResultsDirectory></RunConfiguration></RunSettings>",
            ctx);
    }

    // ── V1 wire format ───────────────────────────────────────────────────
    // Protocol versions 0, 1, 3 — no Version field in the envelope.
    // TestCase is serialized as a Properties array.
    private static readonly string V1Json = """
        {
          "MessageType": "TestExecution.StartWithTests",
          "Payload": {
            "Tests": [
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
                  }
                ]
              }
            ],
            "RunSettings": "\u003CRunSettings\u003E\u003CRunConfiguration\u003E\u003CResultsDirectory\u003E.\\TestResults\u003C/ResultsDirectory\u003E\u003C/RunConfiguration\u003E\u003C/RunSettings\u003E",
            "TestExecutionContext": {
              "FrequencyOfRunStatsChangeEvent": 10,
              "RunStatsChangeEventTimeout": "00:00:30",
              "InIsolation": false,
              "KeepAlive": true,
              "AreTestCaseLevelEventsRequired": true,
              "IsDebug": false,
              "TestCaseFilter": null,
              "FilterOptions": null
            },
            "Package": "Contoso.Math.Tests"
          }
        }
        """;

    // ── V7 wire format ───────────────────────────────────────────────────
    // Protocol versions 2, 4, 5, 6, 7 — includes Version in the envelope.
    // TestCase is serialized as a flat object.
    private static readonly string V7Json = """
        {
          "Version": 7,
          "MessageType": "TestExecution.StartWithTests",
          "Payload": {
            "Tests": [
              {
                "Id": "a1b2c3d4-e5f6-7890-abcd-ef1234567890",
                "FullyQualifiedName": "Contoso.Math.Tests.CalculatorTests.AddTest",
                "DisplayName": "AddTest(1, 2, 3)",
                "ExecutorUri": "executor://MSTestAdapter/v2",
                "Source": "Contoso.Math.Tests.dll",
                "CodeFilePath": "C:\\src\\Contoso.Math.Tests\\CalculatorTests.cs",
                "LineNumber": 42,
                "Properties": []
              }
            ],
            "RunSettings": "\u003CRunSettings\u003E\u003CRunConfiguration\u003E\u003CResultsDirectory\u003E.\\TestResults\u003C/ResultsDirectory\u003E\u003C/RunConfiguration\u003E\u003C/RunSettings\u003E",
            "TestExecutionContext": {
              "FrequencyOfRunStatsChangeEvent": 10,
              "RunStatsChangeEventTimeout": "00:00:30",
              "InIsolation": false,
              "KeepAlive": true,
              "AreTestCaseLevelEventsRequired": true,
              "IsDebug": false,
              "TestCaseFilter": null,
              "FilterOptions": null
            },
            "Package": "Contoso.Math.Tests"
          }
        }
        """;

    // ── Serialize ────────────────────────────────────────────────────────

    [TestMethod]
    public void SerializePayloadV1()
    {
        var json = JsonDataSerializer.Instance.SerializePayload(
            MessageType.StartTestExecutionWithTests, Payload, version: 1);

        AssertJsonEqual(V1Json, json);
    }

    [TestMethod]
    public void SerializePayloadV7()
    {
        var json = JsonDataSerializer.Instance.SerializePayload(
            MessageType.StartTestExecutionWithTests, Payload, version: 7);

        AssertJsonEqual(V7Json, json);
    }

    // ── Deserialize ──────────────────────────────────────────────────────

    [TestMethod]
    public void DeserializePayloadV1()
    {
        var message = JsonDataSerializer.Instance.DeserializeMessage(Minify(V1Json));
        var result = JsonDataSerializer.Instance.DeserializePayload<TestRunCriteriaWithTests>(message);

        AssertPayloadFields(result);
    }

    [TestMethod]
    public void DeserializePayloadV7()
    {
        var message = JsonDataSerializer.Instance.DeserializeMessage(Minify(V7Json));
        var result = JsonDataSerializer.Instance.DeserializePayload<TestRunCriteriaWithTests>(message);

        AssertPayloadFields(result);
    }

    // ── Round-trip ───────────────────────────────────────────────────────

    [TestMethod]
    [DataRow(1)]
    [DataRow(7)]
    public void RoundTrip(int version)
    {
        var json = JsonDataSerializer.Instance.SerializePayload(
            MessageType.StartTestExecutionWithTests, Payload, version);
        var message = JsonDataSerializer.Instance.DeserializeMessage(json);
        var result = JsonDataSerializer.Instance.DeserializePayload<TestRunCriteriaWithTests>(message);

        AssertPayloadFields(result);
    }

    // ── Helpers ──────────────────────────────────────────────────────────

    private static void AssertPayloadFields(TestRunCriteriaWithTests? result)
    {
        Assert.IsNotNull(result);
        var tests = result.Tests.ToList();
        Assert.HasCount(1, tests);
        var tc = tests[0];
        Assert.AreEqual("Contoso.Math.Tests.CalculatorTests.AddTest", tc.FullyQualifiedName);
        Assert.AreEqual(new Uri("executor://MSTestAdapter/v2"), tc.ExecutorUri);
        Assert.AreEqual("Contoso.Math.Tests.dll", tc.Source);
        Assert.AreEqual(@"C:\src\Contoso.Math.Tests\CalculatorTests.cs", tc.CodeFilePath);
        Assert.AreEqual("AddTest(1, 2, 3)", tc.DisplayName);
        Assert.AreEqual(new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), tc.Id);
        Assert.AreEqual(42, tc.LineNumber);
        Assert.AreEqual(@"<RunSettings><RunConfiguration><ResultsDirectory>.\TestResults</ResultsDirectory></RunConfiguration></RunSettings>", result.RunSettings);
        Assert.AreEqual("Contoso.Math.Tests", result.Package);
        Assert.IsNotNull(result.TestExecutionContext);
        Assert.AreEqual(10, result.TestExecutionContext.FrequencyOfRunStatsChangeEvent);
    }

}
