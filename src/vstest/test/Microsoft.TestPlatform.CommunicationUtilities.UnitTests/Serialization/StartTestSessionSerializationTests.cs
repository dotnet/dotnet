// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client.Payloads;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Microsoft.TestPlatform.CommunicationUtilities.UnitTests.Serialization.SerializationTestHelpers;

namespace Microsoft.TestPlatform.CommunicationUtilities.UnitTests.Serialization;

/// <summary>
/// Wire-format tests for <see cref="MessageType.StartTestSession"/> ("TestSession.StartTestSession").
///
/// This message is sent to request a new test session to be started.
/// The payload is <see cref="StartTestSessionPayload"/> which contains the sources,
/// run settings, debugging options, and test platform options.
///
/// Payload is identical for V1 and V7 because no TestCase/TestResult objects are involved.
/// The only difference is the outer envelope: V1 omits the Version field, V7 includes it.
/// </summary>
[TestClass]
[TestCategory("Serialization")]
public class StartTestSessionSerializationTests
{
    // ── Example payload ──────────────────────────────────────────────────
    private static readonly StartTestSessionPayload Payload = new()
    {
        Sources = new List<string> { "Contoso.Math.Tests.dll" },
        RunSettings = @"<RunSettings><RunConfiguration><ResultsDirectory>.\TestResults</ResultsDirectory></RunConfiguration></RunSettings>",
        IsDebuggingEnabled = false,
        HasCustomHostLauncher = false,
        TestPlatformOptions = new TestPlatformOptions
        {
            TestCaseFilter = "Category=Unit",
            CollectMetrics = true,
            SkipDefaultAdapters = false
        }
    };

    // ── V1 wire format ───────────────────────────────────────────────────
    // Protocol versions 0, 1, 3 — no Version field in the envelope.
    private static readonly string V1Json = """
        {
          "MessageType": "TestSession.StartTestSession",
          "Payload": {
            "Sources": [
              "Contoso.Math.Tests.dll"
            ],
            "RunSettings": "\u003CRunSettings\u003E\u003CRunConfiguration\u003E\u003CResultsDirectory\u003E.\\TestResults\u003C/ResultsDirectory\u003E\u003C/RunConfiguration\u003E\u003C/RunSettings\u003E",
            "IsDebuggingEnabled": false,
            "HasCustomHostLauncher": false,
            "TestPlatformOptions": {
              "TestCaseFilter": "Category=Unit",
              "FilterOptions": null,
              "CollectMetrics": true,
              "SkipDefaultAdapters": false
            }
          }
        }
        """;

    // ── V7 wire format ───────────────────────────────────────────────────
    // Protocol versions 2, 4, 5, 6, 7 — includes Version in the envelope.
    private static readonly string V7Json = """
        {
          "Version": 7,
          "MessageType": "TestSession.StartTestSession",
          "Payload": {
            "Sources": [
              "Contoso.Math.Tests.dll"
            ],
            "RunSettings": "\u003CRunSettings\u003E\u003CRunConfiguration\u003E\u003CResultsDirectory\u003E.\\TestResults\u003C/ResultsDirectory\u003E\u003C/RunConfiguration\u003E\u003C/RunSettings\u003E",
            "IsDebuggingEnabled": false,
            "HasCustomHostLauncher": false,
            "TestPlatformOptions": {
              "TestCaseFilter": "Category=Unit",
              "FilterOptions": null,
              "CollectMetrics": true,
              "SkipDefaultAdapters": false
            }
          }
        }
        """;

    // ── Serialize ────────────────────────────────────────────────────────

    [TestMethod]
    public void SerializePayloadV1()
    {
        var json = JsonDataSerializer.Instance.SerializePayload(
            MessageType.StartTestSession, Payload, version: 1);

        AssertJsonEqual(V1Json, json);
    }

    [TestMethod]
    public void SerializePayloadV7()
    {
        var json = JsonDataSerializer.Instance.SerializePayload(
            MessageType.StartTestSession, Payload, version: 7);

        AssertJsonEqual(V7Json, json);
    }

    // ── Deserialize ──────────────────────────────────────────────────────

    [TestMethod]
    public void DeserializePayloadV1()
    {
        var message = JsonDataSerializer.Instance.DeserializeMessage(Minify(V1Json));
        var result = JsonDataSerializer.Instance.DeserializePayload<StartTestSessionPayload>(message);

        Assert.IsNotNull(result);
        Assert.IsNotNull(result.Sources);
        Assert.HasCount(1, result.Sources);
        Assert.AreEqual("Contoso.Math.Tests.dll", result.Sources[0]);
        Assert.AreEqual(@"<RunSettings><RunConfiguration><ResultsDirectory>.\TestResults</ResultsDirectory></RunConfiguration></RunSettings>", result.RunSettings);
        Assert.IsFalse(result.IsDebuggingEnabled);
        Assert.IsFalse(result.HasCustomHostLauncher);
        Assert.IsNotNull(result.TestPlatformOptions);
        Assert.AreEqual("Category=Unit", result.TestPlatformOptions.TestCaseFilter);
        Assert.IsTrue(result.TestPlatformOptions.CollectMetrics);
        Assert.IsFalse(result.TestPlatformOptions.SkipDefaultAdapters);
    }

    [TestMethod]
    public void DeserializePayloadV7()
    {
        var message = JsonDataSerializer.Instance.DeserializeMessage(Minify(V7Json));
        var result = JsonDataSerializer.Instance.DeserializePayload<StartTestSessionPayload>(message);

        Assert.IsNotNull(result);
        Assert.IsNotNull(result.Sources);
        Assert.HasCount(1, result.Sources);
        Assert.AreEqual("Contoso.Math.Tests.dll", result.Sources[0]);
        Assert.AreEqual(@"<RunSettings><RunConfiguration><ResultsDirectory>.\TestResults</ResultsDirectory></RunConfiguration></RunSettings>", result.RunSettings);
        Assert.IsFalse(result.IsDebuggingEnabled);
        Assert.IsFalse(result.HasCustomHostLauncher);
        Assert.IsNotNull(result.TestPlatformOptions);
        Assert.AreEqual("Category=Unit", result.TestPlatformOptions.TestCaseFilter);
        Assert.IsTrue(result.TestPlatformOptions.CollectMetrics);
        Assert.IsFalse(result.TestPlatformOptions.SkipDefaultAdapters);
    }

    // ── Round-trip ───────────────────────────────────────────────────────

    [TestMethod]
    [DataRow(1)]
    [DataRow(7)]
    public void RoundTrip(int version)
    {
        var json = JsonDataSerializer.Instance.SerializePayload(
            MessageType.StartTestSession, Payload, version);
        var message = JsonDataSerializer.Instance.DeserializeMessage(json);
        var result = JsonDataSerializer.Instance.DeserializePayload<StartTestSessionPayload>(message);

        Assert.IsNotNull(result);
        Assert.IsNotNull(result.Sources);
        Assert.HasCount(Payload.Sources!.Count, result.Sources);
        Assert.AreEqual(Payload.Sources![0], result.Sources[0]);
        Assert.AreEqual(Payload.RunSettings, result.RunSettings);
        Assert.AreEqual(Payload.IsDebuggingEnabled, result.IsDebuggingEnabled);
        Assert.AreEqual(Payload.HasCustomHostLauncher, result.HasCustomHostLauncher);
        Assert.IsNotNull(result.TestPlatformOptions);
        Assert.AreEqual(Payload.TestPlatformOptions!.TestCaseFilter, result.TestPlatformOptions.TestCaseFilter);
        Assert.AreEqual(Payload.TestPlatformOptions.CollectMetrics, result.TestPlatformOptions.CollectMetrics);
        Assert.AreEqual(Payload.TestPlatformOptions.SkipDefaultAdapters, result.TestPlatformOptions.SkipDefaultAdapters);
    }

    // ── Helpers ──────────────────────────────────────────────────────────

}
