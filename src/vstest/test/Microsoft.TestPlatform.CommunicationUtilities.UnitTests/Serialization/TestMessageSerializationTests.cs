// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Microsoft.TestPlatform.CommunicationUtilities.UnitTests.Serialization.SerializationTestHelpers;

namespace Microsoft.TestPlatform.CommunicationUtilities.UnitTests.Serialization;

/// <summary>
/// Wire-format tests for <see cref="MessageType.TestMessage"/> ("TestSession.Message").
///
/// This message is sent by the test host to report log/trace output (informational, warning, error).
/// The payload is <see cref="TestMessagePayload"/> which contains a severity level and a text message.
///
/// Payload is identical for V1 and V2 serializers because it contains no TestCase/TestResult objects.
/// The only difference is the outer envelope: V1 omits the Version field, V7 includes it.
/// </summary>
[TestClass]
[TestCategory("Serialization")]
public class TestMessageSerializationTests
{
    // ── Example payload ──────────────────────────────────────────────────
    // A warning message that a test was skipped. Uses single quotes in the
    // text to exercise character escaping.
    private static readonly TestMessagePayload Payload = new()
    {
        MessageLevel = TestMessageLevel.Warning,
        Message = "Test 'CalculatorTests.AddTest' was skipped: requires .NET 8"
    };

    // ── V1 wire format ───────────────────────────────────────────────────
    // Protocol versions 0, 1, 3 — no Version field in the envelope.
    private static readonly string V1Json = """
        {
          "MessageType": "TestSession.Message",
          "Payload": {
            "MessageLevel": 1,
            "Message": "Test \u0027CalculatorTests.AddTest\u0027 was skipped: requires .NET 8"
          }
        }
        """;

    // ── V7 wire format ───────────────────────────────────────────────────
    // Protocol versions 2, 4, 5, 6, 7 — includes Version in the envelope.
    private static readonly string V7Json = """
        {
          "Version": 7,
          "MessageType": "TestSession.Message",
          "Payload": {
            "MessageLevel": 1,
            "Message": "Test \u0027CalculatorTests.AddTest\u0027 was skipped: requires .NET 8"
          }
        }
        """;

    // ── Serialize ────────────────────────────────────────────────────────

    [TestMethod]
    public void SerializePayloadV1()
    {
        var json = JsonDataSerializer.Instance.SerializePayload(
            MessageType.TestMessage, Payload, version: 1);

        AssertJsonEqual(V1Json, json);
    }

    [TestMethod]
    public void SerializePayloadV7()
    {
        var json = JsonDataSerializer.Instance.SerializePayload(
            MessageType.TestMessage, Payload, version: 7);

        AssertJsonEqual(V7Json, json);
    }

    // ── Deserialize ──────────────────────────────────────────────────────

    [TestMethod]
    public void DeserializePayloadV1()
    {
        // Simulate receiving a V1 message: deserialize the full message,
        // then extract the payload through DeserializePayload<T>.
        var message = JsonDataSerializer.Instance.DeserializeMessage(Minify(V1Json));
        var result = JsonDataSerializer.Instance.DeserializePayload<TestMessagePayload>(message);

        Assert.IsNotNull(result);
        Assert.AreEqual(TestMessageLevel.Warning, result.MessageLevel);
        Assert.AreEqual("Test 'CalculatorTests.AddTest' was skipped: requires .NET 8", result.Message);
    }

    [TestMethod]
    public void DeserializePayloadV7()
    {
        var message = JsonDataSerializer.Instance.DeserializeMessage(Minify(V7Json));
        var result = JsonDataSerializer.Instance.DeserializePayload<TestMessagePayload>(message);

        Assert.IsNotNull(result);
        Assert.AreEqual(TestMessageLevel.Warning, result.MessageLevel);
        Assert.AreEqual("Test 'CalculatorTests.AddTest' was skipped: requires .NET 8", result.Message);
    }

    // ── Round-trip ───────────────────────────────────────────────────────

    [TestMethod]
    [DataRow(1)]
    [DataRow(7)]
    public void RoundTrip(int version)
    {
        // Serialize → deserialize → verify the C# object survives the trip.
        var json = JsonDataSerializer.Instance.SerializePayload(
            MessageType.TestMessage, Payload, version);
        var message = JsonDataSerializer.Instance.DeserializeMessage(json);
        var result = JsonDataSerializer.Instance.DeserializePayload<TestMessagePayload>(message);

        Assert.IsNotNull(result);
        Assert.AreEqual(Payload.MessageLevel, result.MessageLevel);
        Assert.AreEqual(Payload.Message, result.Message);
    }

    // ── Edge cases: special characters ───────────────────────────────────
    // These verify that control characters, unicode escapes, newlines, and
    // other tricky strings round-trip correctly through both STJ and Jsonite.
    // See testfx#5120 — Jsonite previously threw on control chars < 0x20.

    [TestMethod]
    [DataRow(1)]
    [DataRow(7)]
    public void RoundTrip_ControlCharacters(int version)
    {
        var payload = new TestMessagePayload
        {
            MessageLevel = TestMessageLevel.Error,
            Message = "Output contained \u0003ETX and \u0001SOH control chars"
        };

        var json = JsonDataSerializer.Instance.SerializePayload(
            MessageType.TestMessage, payload, version);
        var message = JsonDataSerializer.Instance.DeserializeMessage(json);
        var result = JsonDataSerializer.Instance.DeserializePayload<TestMessagePayload>(message);

        Assert.IsNotNull(result);
        Assert.AreEqual(payload.Message, result.Message);
    }

    [TestMethod]
    [DataRow(1)]
    [DataRow(7)]
    public void RoundTrip_NewlinesAndTabs(int version)
    {
        var payload = new TestMessagePayload
        {
            MessageLevel = TestMessageLevel.Informational,
            Message = "Line1\nLine2\r\nLine3\tTabbed"
        };

        var json = JsonDataSerializer.Instance.SerializePayload(
            MessageType.TestMessage, payload, version);
        var message = JsonDataSerializer.Instance.DeserializeMessage(json);
        var result = JsonDataSerializer.Instance.DeserializePayload<TestMessagePayload>(message);

        Assert.IsNotNull(result);
        Assert.AreEqual(payload.Message, result.Message);
    }

    [TestMethod]
    [DataRow(1)]
    [DataRow(7)]
    public void RoundTrip_UnicodeAndEmoji(int version)
    {
        var payload = new TestMessagePayload
        {
            MessageLevel = TestMessageLevel.Warning,
            Message = "日本語テスト — Ñoño — Ü — 🎯 emoji"
        };

        var json = JsonDataSerializer.Instance.SerializePayload(
            MessageType.TestMessage, payload, version);
        var message = JsonDataSerializer.Instance.DeserializeMessage(json);
        var result = JsonDataSerializer.Instance.DeserializePayload<TestMessagePayload>(message);

        Assert.IsNotNull(result);
        Assert.AreEqual(payload.Message, result.Message);
    }

    [TestMethod]
    [DataRow(1)]
    [DataRow(7)]
    public void RoundTrip_BackslashesAndQuotes(int version)
    {
        var payload = new TestMessagePayload
        {
            MessageLevel = TestMessageLevel.Error,
            Message = "Path: C:\\Users\\test\\\"file\".txt"
        };

        var json = JsonDataSerializer.Instance.SerializePayload(
            MessageType.TestMessage, payload, version);
        var message = JsonDataSerializer.Instance.DeserializeMessage(json);
        var result = JsonDataSerializer.Instance.DeserializePayload<TestMessagePayload>(message);

        Assert.IsNotNull(result);
        Assert.AreEqual(payload.Message, result.Message);
    }

    [TestMethod]
    [DataRow(1)]
    [DataRow(7)]
    public void RoundTrip_NullAndEmptyStrings(int version)
    {
        var payload = new TestMessagePayload
        {
            MessageLevel = TestMessageLevel.Informational,
            Message = ""
        };

        var json = JsonDataSerializer.Instance.SerializePayload(
            MessageType.TestMessage, payload, version);
        var message = JsonDataSerializer.Instance.DeserializeMessage(json);
        var result = JsonDataSerializer.Instance.DeserializePayload<TestMessagePayload>(message);

        Assert.IsNotNull(result);
        Assert.AreEqual("", result.Message);
    }

    // ── Helpers ──────────────────────────────────────────────────────────

}
