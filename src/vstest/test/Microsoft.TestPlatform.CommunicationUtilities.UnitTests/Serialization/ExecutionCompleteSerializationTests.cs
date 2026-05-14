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
/// Wire-format tests for <see cref="MessageType.ExecutionComplete"/> ("TestExecution.Completed").
///
/// This is the most complex message in the protocol. It is sent by the test host when a test
/// run finishes. The payload is a <see cref="TestRunCompletePayload"/> containing:
/// - <see cref="TestRunCompleteEventArgs"/> with run statistics, cancellation/abort flags,
///   elapsed time, attachment sets, and discovered extensions.
/// - <see cref="TestRunChangedEventArgs"/> with the last batch of test results and active tests.
/// - A list of executor URIs that participated in the run.
///
/// V1 and V7 differ in how the nested <see cref="TestCase"/> and <see cref="TestResult"/>
/// objects are serialized: V1 uses the Properties array format for both, V7 uses flat objects
/// with well-known fields promoted to top-level properties.
/// </summary>
[TestClass]
[TestCategory("Serialization")]
public class ExecutionCompleteSerializationTests
{
    // ── Example payload ──────────────────────────────────────────────────
    // A successful run of one test (AddTest) that passed in 12ms.
    private static readonly TestRunCompletePayload Payload = BuildPayload();

    // ── V1 wire format ───────────────────────────────────────────────────
    // Protocol versions 0, 1, 3 — TestCase and TestResult use Properties arrays.
    private static readonly string V1Json = """
        {
          "MessageType": "TestExecution.Completed",
          "Payload": {
            "TestRunCompleteArgs": {
              "TestRunStatistics": {
                "ExecutedTests": 1,
                "Stats": {
                  "Passed": 1
                }
              },
              "IsCanceled": false,
              "IsAborted": false,
              "Error": null,
              "AttachmentSets": [],
              "InvokedDataCollectors": [],
              "ElapsedTimeInRunningTests": "00:00:02",
              "Metrics": null,
              "DiscoveredExtensions": {}
            },
            "LastRunTests": {
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
                        "Value": "Contoso.Math.Tests.CalculatorTests.AddTest"
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
                      "Value": 1
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
                      "Value": null
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
                      "Value": null
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
                      "Value": "AddTest(1, 2, 3)"
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
                      "Value": "BUILD-AGENT-01"
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
                      "Value": "00:00:00.0120000"
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
                      "Value": "2026-03-20T10:00:00\u002B00:00"
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
                      "Value": "2026-03-20T10:00:00.012\u002B00:00"
                    }
                  ]
                }
              ],
              "TestRunStatistics": {
                "ExecutedTests": 1,
                "Stats": {
                  "Passed": 1
                }
              },
              "ActiveTests": []
            },
            "RunAttachments": null,
            "ExecutorUris": [
              "executor://MSTestAdapter/v2"
            ]
          }
        }
        """;

    // ── V7 wire format ───────────────────────────────────────────────────
    // Protocol versions 2, 4, 5, 6, 7 — TestCase and TestResult as flat objects.
    private static readonly string V7Json = """
        {
          "Version": 7,
          "MessageType": "TestExecution.Completed",
          "Payload": {
            "TestRunCompleteArgs": {
              "TestRunStatistics": {
                "ExecutedTests": 1,
                "Stats": {
                  "Passed": 1
                }
              },
              "IsCanceled": false,
              "IsAborted": false,
              "Error": null,
              "AttachmentSets": [],
              "InvokedDataCollectors": [],
              "ElapsedTimeInRunningTests": "00:00:02",
              "Metrics": null,
              "DiscoveredExtensions": {}
            },
            "LastRunTests": {
              "NewTestResults": [
                {
                  "TestCase": {
                    "Id": "a1b2c3d4-e5f6-7890-abcd-ef1234567890",
                    "FullyQualifiedName": "Contoso.Math.Tests.CalculatorTests.AddTest",
                    "DisplayName": "Contoso.Math.Tests.CalculatorTests.AddTest",
                    "ExecutorUri": "executor://MSTestAdapter/v2",
                    "Source": "Contoso.Math.Tests.dll",
                    "CodeFilePath": null,
                    "LineNumber": -1,
                    "Properties": []
                  },
                  "Attachments": [],
                  "Outcome": 1,
                  "ErrorMessage": null,
                  "ErrorStackTrace": null,
                  "DisplayName": "AddTest(1, 2, 3)",
                  "Messages": [],
                  "ComputerName": "BUILD-AGENT-01",
                  "Duration": "00:00:00.0120000",
                  "StartTime": "2026-03-20T10:00:00\u002B00:00",
                  "EndTime": "2026-03-20T10:00:00.012\u002B00:00",
                  "Properties": []
                }
              ],
              "TestRunStatistics": {
                "ExecutedTests": 1,
                "Stats": {
                  "Passed": 1
                }
              },
              "ActiveTests": []
            },
            "RunAttachments": null,
            "ExecutorUris": [
              "executor://MSTestAdapter/v2"
            ]
          }
        }
        """;

    // ── Serialize ────────────────────────────────────────────────────────

    [TestMethod]
    public void SerializePayloadV1()
    {
        var json = JsonDataSerializer.Instance.SerializePayload(
            MessageType.ExecutionComplete, Payload, version: 1);

        AssertJsonEqual(V1Json, json);
    }

    [TestMethod]
    public void SerializePayloadV7()
    {
        var json = JsonDataSerializer.Instance.SerializePayload(
            MessageType.ExecutionComplete, Payload, version: 7);

        AssertJsonEqual(V7Json, json);
    }

    // ── Deserialize ──────────────────────────────────────────────────────

    [TestMethod]
    public void DeserializePayloadV1()
    {
        var message = JsonDataSerializer.Instance.DeserializeMessage(Minify(V1Json));
        var result = JsonDataSerializer.Instance.DeserializePayload<TestRunCompletePayload>(message);

        AssertPayloadFields(result);
    }

    [TestMethod]
    public void DeserializePayloadV7()
    {
        var message = JsonDataSerializer.Instance.DeserializeMessage(Minify(V7Json));
        var result = JsonDataSerializer.Instance.DeserializePayload<TestRunCompletePayload>(message);

        AssertPayloadFields(result);
    }

    // ── Round-trip ───────────────────────────────────────────────────────

    [TestMethod]
    [DataRow(1)]
    [DataRow(7)]
    public void RoundTrip(int version)
    {
        var json = JsonDataSerializer.Instance.SerializePayload(
            MessageType.ExecutionComplete, Payload, version);
        var message = JsonDataSerializer.Instance.DeserializeMessage(json);
        var result = JsonDataSerializer.Instance.DeserializePayload<TestRunCompletePayload>(message);

        Assert.IsNotNull(result);
        Assert.IsNotNull(result.TestRunCompleteArgs);
        Assert.AreEqual(Payload.TestRunCompleteArgs!.IsCanceled, result.TestRunCompleteArgs.IsCanceled);
        Assert.AreEqual(Payload.TestRunCompleteArgs.IsAborted, result.TestRunCompleteArgs.IsAborted);
        Assert.IsNotNull(result.LastRunTests);
        var newResults = result.LastRunTests.NewTestResults!.ToList();
        Assert.HasCount(1, newResults);
        Assert.AreEqual(TestOutcome.Passed, newResults[0].Outcome);
    }

    // ── Helpers ──────────────────────────────────────────────────────────

    private static TestRunCompletePayload BuildPayload()
    {
        var tc = new TestCase(
            "Contoso.Math.Tests.CalculatorTests.AddTest",
            new Uri("executor://MSTestAdapter/v2"),
            "Contoso.Math.Tests.dll")
        {
            Id = new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"),
        };

        var tr = new TestResult(tc)
        {
            Outcome = TestOutcome.Passed,
            DisplayName = "AddTest(1, 2, 3)",
            Duration = TimeSpan.FromMilliseconds(12),
            StartTime = new DateTimeOffset(2026, 3, 20, 10, 0, 0, TimeSpan.Zero),
            EndTime = new DateTimeOffset(2026, 3, 20, 10, 0, 0, 12, TimeSpan.Zero),
            ComputerName = "BUILD-AGENT-01",
        };

        var stats = new Dictionary<TestOutcome, long> { [TestOutcome.Passed] = 1 };
        var runStats = new TestRunStatistics(1, stats);

        return new TestRunCompletePayload
        {
            TestRunCompleteArgs = new TestRunCompleteEventArgs(
                runStats, false, false, null, null, null, TimeSpan.FromSeconds(2)),
            LastRunTests = new TestRunChangedEventArgs(
                runStats, new[] { tr }, Array.Empty<TestCase>()),
            ExecutorUris = new List<string> { "executor://MSTestAdapter/v2" },
        };
    }

    private static void AssertPayloadFields(TestRunCompletePayload? result)
    {
        Assert.IsNotNull(result);

        // TestRunCompleteArgs
        Assert.IsNotNull(result.TestRunCompleteArgs);
        Assert.IsFalse(result.TestRunCompleteArgs.IsCanceled);
        Assert.IsFalse(result.TestRunCompleteArgs.IsAborted);
        Assert.IsNotNull(result.TestRunCompleteArgs.TestRunStatistics);
        Assert.AreEqual(1, result.TestRunCompleteArgs.TestRunStatistics.ExecutedTests);

        // LastRunTests
        Assert.IsNotNull(result.LastRunTests);
        var newResults = result.LastRunTests.NewTestResults!.ToList();
        Assert.HasCount(1, newResults);
        Assert.AreEqual(TestOutcome.Passed, newResults[0].Outcome);
        Assert.AreEqual("AddTest(1, 2, 3)", newResults[0].DisplayName);
        Assert.AreEqual("Contoso.Math.Tests.CalculatorTests.AddTest", newResults[0].TestCase.FullyQualifiedName);

        // ExecutorUris
        Assert.IsNotNull(result.ExecutorUris);
        Assert.Contains("executor://MSTestAdapter/v2", result.ExecutorUris);
    }

}
