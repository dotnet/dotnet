// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using TestResult = Microsoft.VisualStudio.TestPlatform.ObjectModel.TestResult;
using static Microsoft.TestPlatform.CommunicationUtilities.UnitTests.Serialization.SerializationTestHelpers;

namespace Microsoft.TestPlatform.CommunicationUtilities.UnitTests.Serialization;

/// <summary>
/// Wire-format tests for <see cref="MessageType.TestRunStatsChange"/> ("TestExecution.StatsChange").
///
/// This message is sent by the test host during execution to report incremental progress:
/// newly completed test results and currently in-progress test cases. The payload is a
/// <see cref="TestRunStatsPayload"/> containing <see cref="TestRunChangedEventArgs"/> (with
/// new results, run statistics, and active tests) and an optional list of in-progress test cases.
///
/// V1 and V7 differ in how the nested <see cref="TestCase"/> and <see cref="TestResult"/>
/// objects are serialized: V1 uses the Properties array format, V7 uses flat objects.
/// This example includes a failed test (DivideTest) with error message and stack trace.
/// </summary>
[TestClass]
[TestCategory("Serialization")]
public class TestRunStatsChangeSerializationTests
{
    // ── Example payload ──────────────────────────────────────────────────
    // A stats change reporting one failed test (DivideTest) with an active
    // test (MultiplyTest) still in progress.
    private static readonly TestRunStatsPayload Payload = BuildPayload();

    // ── V1 wire format ───────────────────────────────────────────────────
    // Protocol versions 0, 1, 3 — TestCase and TestResult use Properties arrays.
    private static readonly string V1Json = """
        {
          "MessageType": "TestExecution.StatsChange",
          "Payload": {
            "TestRunChangedArgs": {
              "NewTestResults": [
                {
                  "TestCase": {
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
                        "Value": "Contoso.Math.Tests.CalculatorTests.DivideTest"
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
                        "Value": "Contoso.Math.Tests.CalculatorTests.DivideTest"
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
                        "Value": "c3d4e5f6-a7b8-9012-cdef-123456789012"
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
                  "Attachments": [],
                  "Messages": [],
                  "Properties": [
                    {
                      "Key": {
                        "Id": "TestResult.Outcome",
                        "Label": "Outcome",
                        "Category": "",
                        "Description": "",
                        "Attributes": 0,
                        "ValueType": "Microsoft.VisualStudio.TestPlatform.ObjectModel.TestOutcome, Microsoft.VisualStudio.TestPlatform.ObjectModel, Version=15.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
                      },
                      "Value": 2
                    },
                    {
                      "Key": {
                        "Id": "TestResult.ErrorMessage",
                        "Label": "Error Message",
                        "Category": "",
                        "Description": "",
                        "Attributes": 0,
                        "ValueType": "System.String"
                      },
                      "Value": "Assert.AreEqual failed. Expected:\u003C0.5\u003E. Actual:\u003C0\u003E."
                    },
                    {
                      "Key": {
                        "Id": "TestResult.ErrorStackTrace",
                        "Label": "Error Stack Trace",
                        "Category": "",
                        "Description": "",
                        "Attributes": 0,
                        "ValueType": "System.String"
                      },
                      "Value": "   at Contoso.Math.Tests.CalculatorTests.DivideTest() in C:\\src\\CalculatorTests.cs:line 55"
                    },
                    {
                      "Key": {
                        "Id": "TestResult.DisplayName",
                        "Label": "TestResult Display Name",
                        "Category": "",
                        "Description": "",
                        "Attributes": 1,
                        "ValueType": "System.String"
                      },
                      "Value": "DivideTest"
                    },
                    {
                      "Key": {
                        "Id": "TestResult.ComputerName",
                        "Label": "Computer Name",
                        "Category": "",
                        "Description": "",
                        "Attributes": 0,
                        "ValueType": "System.String"
                      },
                      "Value": ""
                    },
                    {
                      "Key": {
                        "Id": "TestResult.Duration",
                        "Label": "Duration",
                        "Category": "",
                        "Description": "",
                        "Attributes": 0,
                        "ValueType": "System.TimeSpan"
                      },
                      "Value": "00:00:00.0030000"
                    },
                    {
                      "Key": {
                        "Id": "TestResult.StartTime",
                        "Label": "Start Time",
                        "Category": "",
                        "Description": "",
                        "Attributes": 0,
                        "ValueType": "System.DateTimeOffset"
                      },
                      "Value": "2026-03-20T10:00:01\u002B00:00"
                    },
                    {
                      "Key": {
                        "Id": "TestResult.EndTime",
                        "Label": "End Time",
                        "Category": "",
                        "Description": "",
                        "Attributes": 0,
                        "ValueType": "System.DateTimeOffset"
                      },
                      "Value": "2026-03-20T10:00:01.003\u002B00:00"
                    }
                  ]
                }
              ],
              "TestRunStatistics": {
                "ExecutedTests": 1,
                "Stats": {
                  "Failed": 1
                }
              },
              "ActiveTests": [
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
                      "Value": "Contoso.Math.Tests.CalculatorTests.MultiplyTest"
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
                      "Value": "Contoso.Math.Tests.CalculatorTests.MultiplyTest"
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
                      "Value": "d4e5f6a7-b8c9-0123-defa-234567890123"
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
              ]
            },
            "InProgressTestCases": null
          }
        }
        """;

    // ── V7 wire format ───────────────────────────────────────────────────
    // Protocol versions 2, 4, 5, 6, 7 — TestCase and TestResult as flat objects.
    private static readonly string V7Json = """
        {
          "Version": 7,
          "MessageType": "TestExecution.StatsChange",
          "Payload": {
            "TestRunChangedArgs": {
              "NewTestResults": [
                {
                  "TestCase": {
                    "Id": "c3d4e5f6-a7b8-9012-cdef-123456789012",
                    "FullyQualifiedName": "Contoso.Math.Tests.CalculatorTests.DivideTest",
                    "DisplayName": "Contoso.Math.Tests.CalculatorTests.DivideTest",
                    "ExecutorUri": "executor://MSTestAdapter/v2",
                    "Source": "Contoso.Math.Tests.dll",
                    "CodeFilePath": null,
                    "LineNumber": -1,
                    "Properties": []
                  },
                  "Attachments": [],
                  "Outcome": 2,
                  "ErrorMessage": "Assert.AreEqual failed. Expected:\u003C0.5\u003E. Actual:\u003C0\u003E.",
                  "ErrorStackTrace": "   at Contoso.Math.Tests.CalculatorTests.DivideTest() in C:\\src\\CalculatorTests.cs:line 55",
                  "DisplayName": "DivideTest",
                  "Messages": [],
                  "ComputerName": null,
                  "Duration": "00:00:00.0030000",
                  "StartTime": "2026-03-20T10:00:01\u002B00:00",
                  "EndTime": "2026-03-20T10:00:01.003\u002B00:00",
                  "Properties": []
                }
              ],
              "TestRunStatistics": {
                "ExecutedTests": 1,
                "Stats": {
                  "Failed": 1
                }
              },
              "ActiveTests": [
                {
                  "Id": "d4e5f6a7-b8c9-0123-defa-234567890123",
                  "FullyQualifiedName": "Contoso.Math.Tests.CalculatorTests.MultiplyTest",
                  "DisplayName": "Contoso.Math.Tests.CalculatorTests.MultiplyTest",
                  "ExecutorUri": "executor://MSTestAdapter/v2",
                  "Source": "Contoso.Math.Tests.dll",
                  "CodeFilePath": null,
                  "LineNumber": -1,
                  "Properties": []
                }
              ]
            },
            "InProgressTestCases": null
          }
        }
        """;

    // ── Serialize ────────────────────────────────────────────────────────

    [TestMethod]
    public void SerializePayloadV1()
    {
        var json = JsonDataSerializer.Instance.SerializePayload(
            MessageType.TestRunStatsChange, Payload, version: 1);

        AssertJsonEqual(V1Json, json);
    }

    [TestMethod]
    public void SerializePayloadV7()
    {
        var json = JsonDataSerializer.Instance.SerializePayload(
            MessageType.TestRunStatsChange, Payload, version: 7);

        AssertJsonEqual(V7Json, json);
    }

    // ── Deserialize ──────────────────────────────────────────────────────

    [TestMethod]
    public void DeserializePayloadV1()
    {
        var message = JsonDataSerializer.Instance.DeserializeMessage(Minify(V1Json));
        var result = JsonDataSerializer.Instance.DeserializePayload<TestRunStatsPayload>(message);

        AssertPayloadFields(result);
    }

    [TestMethod]
    public void DeserializePayloadV7()
    {
        var message = JsonDataSerializer.Instance.DeserializeMessage(Minify(V7Json));
        var result = JsonDataSerializer.Instance.DeserializePayload<TestRunStatsPayload>(message);

        AssertPayloadFields(result);
    }

    // ── Round-trip ───────────────────────────────────────────────────────

    [TestMethod]
    [DataRow(1)]
    [DataRow(7)]
    public void RoundTrip(int version)
    {
        var json = JsonDataSerializer.Instance.SerializePayload(
            MessageType.TestRunStatsChange, Payload, version);
        var message = JsonDataSerializer.Instance.DeserializeMessage(json);
        var result = JsonDataSerializer.Instance.DeserializePayload<TestRunStatsPayload>(message);

        Assert.IsNotNull(result);
        Assert.IsNotNull(result.TestRunChangedArgs);
        var newResults = result.TestRunChangedArgs.NewTestResults!.ToList();
        Assert.HasCount(1, newResults);
        Assert.AreEqual(TestOutcome.Failed, newResults[0].Outcome);
        Assert.AreEqual("Assert.AreEqual failed. Expected:<0.5>. Actual:<0>.", newResults[0].ErrorMessage);
    }

    // ── Helpers ──────────────────────────────────────────────────────────

    private static TestRunStatsPayload BuildPayload()
    {
        var tc = new TestCase(
            "Contoso.Math.Tests.CalculatorTests.DivideTest",
            new Uri("executor://MSTestAdapter/v2"),
            "Contoso.Math.Tests.dll")
        {
            Id = new Guid("c3d4e5f6-a7b8-9012-cdef-123456789012"),
        };

        var tr = new TestResult(tc)
        {
            Outcome = TestOutcome.Failed,
            ErrorMessage = "Assert.AreEqual failed. Expected:<0.5>. Actual:<0>.",
            ErrorStackTrace = @"   at Contoso.Math.Tests.CalculatorTests.DivideTest() in C:\src\CalculatorTests.cs:line 55",
            DisplayName = "DivideTest",
            Duration = TimeSpan.FromMilliseconds(3),
            StartTime = new DateTimeOffset(2026, 3, 20, 10, 0, 1, TimeSpan.Zero),
            EndTime = new DateTimeOffset(2026, 3, 20, 10, 0, 1, 3, TimeSpan.Zero),
        };

        var stats = new Dictionary<TestOutcome, long> { [TestOutcome.Failed] = 1 };
        var runStats = new TestRunStatistics(1, stats);

        var inProgress = new List<TestCase>
        {
            new("Contoso.Math.Tests.CalculatorTests.MultiplyTest",
                new Uri("executor://MSTestAdapter/v2"), "Contoso.Math.Tests.dll")
            {
                Id = new Guid("d4e5f6a7-b8c9-0123-defa-234567890123"),
            }
        };

        return new TestRunStatsPayload
        {
            TestRunChangedArgs = new TestRunChangedEventArgs(
                runStats, new[] { tr }, inProgress),
        };
    }

    private static void AssertPayloadFields(TestRunStatsPayload? result)
    {
        Assert.IsNotNull(result);
        Assert.IsNotNull(result.TestRunChangedArgs);

        // Verify the failed test result
        var newResults = result.TestRunChangedArgs.NewTestResults!.ToList();
        Assert.HasCount(1, newResults);
        Assert.AreEqual(TestOutcome.Failed, newResults[0].Outcome);
        Assert.AreEqual("Assert.AreEqual failed. Expected:<0.5>. Actual:<0>.", newResults[0].ErrorMessage);
        Assert.AreEqual(
            @"   at Contoso.Math.Tests.CalculatorTests.DivideTest() in C:\src\CalculatorTests.cs:line 55",
            newResults[0].ErrorStackTrace);
        Assert.AreEqual("DivideTest", newResults[0].DisplayName);
        Assert.AreEqual("Contoso.Math.Tests.CalculatorTests.DivideTest",
            newResults[0].TestCase.FullyQualifiedName);

        // Verify run statistics
        Assert.IsNotNull(result.TestRunChangedArgs.TestRunStatistics);
        Assert.AreEqual(1, result.TestRunChangedArgs.TestRunStatistics.ExecutedTests);

        // Verify active tests (in-progress)
        var activeTests = result.TestRunChangedArgs.ActiveTests!.ToList();
        Assert.HasCount(1, activeTests);
        Assert.AreEqual("Contoso.Math.Tests.CalculatorTests.MultiplyTest",
            activeTests[0].FullyQualifiedName);

        // InProgressTestCases is null (not set on payload)
        Assert.IsNull(result.InProgressTestCases);
    }

}
